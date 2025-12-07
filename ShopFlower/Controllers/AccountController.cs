using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopFlower.Controllers
{
    public class AccountController : Controller
    {
        private readonly QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        // --- Helper: password hashing (PBKDF2) ---
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        private static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var hash = pbkdf2.GetBytes(HashSize);
                return (hash, salt);
            }
        }

        private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations))
            {
                var computed = pbkdf2.GetBytes(HashSize);
                return computed.SequenceEqual(storedHash);
            }
        }

        // Ensure PasswordResets table exists (simple create-if-not-exists SQL)
        private void EnsurePasswordResetsTable()
        {
            var sql = @"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PasswordResets' AND xtype='U')
BEGIN
    CREATE TABLE PasswordResets
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MaTK INT NOT NULL,
        Token NVARCHAR(200) NOT NULL,
        Expiry DATETIME NOT NULL
    )
END
";
            db.Database.ExecuteSqlCommand(sql);
        }


        // GET: Dang_nhap
        public ActionResult Dang_nhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dang_nhap(string TenDangNhap, string MatKhau, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên đăng nhập và mật khẩu.");
                return View();
            }

            var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == TenDangNhap);

            // If user is not found, or is inactive, fail immediately.
            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Tài khoản đã bị vô hiệu hóa.");
                return View();
            }

            bool isAuthenticated = false;
            try
            {
                var storedHash = user.MatKhauHash;
                var storedSalt = user.Salt;

                if (storedHash != null && storedSalt != null && storedHash.Length > 0 && storedSalt.Length > 0)
                {
                    // 1. PBKDF2 verify (new users)
                    if (VerifyPassword(MatKhau, storedHash, storedSalt))
                    {
                        isAuthenticated = true;
                    }
                    else
                    {
                        // 2. SHA256(password + salt) fallback for seeded users
                        using (var sha = SHA256.Create())
                        {
                            var pwdBytes = Encoding.UTF8.GetBytes(MatKhau);
                            var concat = new byte[pwdBytes.Length + storedSalt.Length];
                            Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
                            Buffer.BlockCopy(storedSalt, 0, concat, pwdBytes.Length, storedSalt.Length);
                            var computed = sha.ComputeHash(concat);
                            if (computed.Length == storedHash.Length && computed.SequenceEqual(storedHash))
                            {
                                isAuthenticated = true;
                                // Re-hash with PBKDF2 on first successful SHA256 login (optional upgrade)
                                try
                                {
                                    var (newHash, newSalt) = HashPassword(MatKhau);
                                    user.MatKhauHash = newHash;
                                    user.Salt = newSalt;
                                    db.SaveChanges();
                                }
                                catch
                                {
                                    // ignore upgrade failure
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Do not reveal internal errors to user
            }

            // optional fallback to stored proc auth (keeps compatibility)
            if (!isAuthenticated)
            {
                try
                {
                    var maTK = db.Database.SqlQuery<int?>(
                        "EXEC dbo.sp_XacThucTaiKhoan @TenDangNhap, @MatKhau",
                        new SqlParameter("@TenDangNhap", TenDangNhap),
                        new SqlParameter("@MatKhau", MatKhau)
                    ).FirstOrDefault();

                    if (maTK.HasValue)
                    {
                        isAuthenticated = true;
                    }
                }
                catch
                {
                    // ignore
                }
            }

            if (isAuthenticated)
            {
                // fetch roles (may be empty)
                List<string> roles;
                try
                {
                    roles = db.Database.SqlQuery<string>(
                        "EXEC dbo.sp_LayVaiTroTheoNguoiDung @TenDangNhap",
                        new SqlParameter("@TenDangNhap", TenDangNhap)
                    ).ToList();
                }
                catch
                {
                    roles = new List<string>();
                }

                var rolesString = string.Join(",", roles);

                var ticket = new FormsAuthenticationTicket(1, TenDangNhap, DateTime.Now, DateTime.Now.AddHours(8), false, rolesString);
                var encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                Response.Cookies.Add(cookie);

                // migrate anonymous session cart/wish
                var username = TenDangNhap;
                var anonCart = Session["Cart"] as List<Cart>;
                if (anonCart != null)
                {
                    var userKey = "Cart_" + username;
                    var userCart = Session[userKey] as List<Cart> ?? new List<Cart>();
                    foreach (var it in anonCart)
                    {
                        var existing = userCart.FirstOrDefault(c => c.ProductId == it.ProductId);
                        if (existing != null) existing.Quantity += it.Quantity;
                        else userCart.Add(it);
                    }
                    Session[userKey] = userCart;
                    Session.Remove("Cart");
                }
                var anonWish = Session["Wish"] as List<string>;
                if (anonWish != null)
                {
                    var userWishKey = "Wish_" + username;
                    var userWish = Session[userWishKey] as List<string> ?? new List<string>();
                    foreach (var pid in anonWish)
                    {
                        if (!userWish.Contains(pid)) userWish.Add(pid);
                    }
                    Session[userWishKey] = userWish;
                    Session.Remove("Wish");
                }

                // Redirect logic: ReturnUrl > admin if Admin role > homepage
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (roles.Any(r => string.Equals(r, "Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                return RedirectToAction("Trang_chu", "Home");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View();
        }

        //public ActionResult Dang_nhap(string TenDangNhap, string MatKhau, string returnUrl)
        //{
        //    if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau))
        //    {
        //        ModelState.AddModelError("", "Vui lòng nhập tên đăng nhập và mật khẩu.");
        //        return View();
        //    }

        //    int? maTK = null;
        //    var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == TenDangNhap);
        //    if (user != null)
        //    {
        //        try
        //        {
        //            var storedHash = user.MatKhauHash;
        //            var storedSalt = user.Salt;

        //            if (storedHash != null && storedSalt != null && storedHash.Length > 0 && storedSalt.Length > 0)
        //            {
        //                // 1. PBKDF2 verify (new users)
        //                if (VerifyPassword(MatKhau, storedHash, storedSalt))
        //                {
        //                    maTK = user.MaTK;
        //                }
        //                else
        //                {
        //                    // 2. SHA256(password + salt) fallback for seeded users
        //                    using (var sha = SHA256.Create())
        //                    {
        //                        var pwdBytes = Encoding.UTF8.GetBytes(MatKhau);
        //                        var concat = new byte[pwdBytes.Length + storedSalt.Length];
        //                        Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
        //                        Buffer.BlockCopy(storedSalt, 0, concat, pwdBytes.Length, storedSalt.Length);
        //                        var computed = sha.ComputeHash(concat);
        //                        if (computed.Length == storedHash.Length && computed.SequenceEqual(storedHash))
        //                        {
        //                            maTK = user.MaTK;
        //                            // Re-hash with PBKDF2 on first successful SHA256 login (optional upgrade)
        //                            try
        //                            {
        //                                var (newHash, newSalt) = HashPassword(MatKhau);
        //                                user.MatKhauHash = newHash;
        //                                user.Salt = newSalt;
        //                                db.SaveChanges();
        //                            }
        //                            catch
        //                            {
        //                                // ignore upgrade failure
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            // Do not reveal internal errors to user
        //        }
        //    }

        //    // optional fallback to stored proc auth (keeps compatibility)
        //    if (!maTK.HasValue)
        //    {
        //        try
        //        {
        //            maTK = db.Database.SqlQuery<int?>(
        //                "EXEC dbo.sp_XacThucTaiKhoan @TenDangNhap, @MatKhau",
        //                new SqlParameter("@TenDangNhap", TenDangNhap),
        //                new SqlParameter("@MatKhau", MatKhau)
        //            ).FirstOrDefault();
        //        }
        //        catch
        //        {
        //            // ignore
        //        }
        //    }

        //    if (maTK.HasValue)
        //    {
        //        // fetch roles (may be empty)
        //        List<string> roles;
        //        try
        //        {
        //            roles = db.Database.SqlQuery<string>(
        //                "EXEC dbo.sp_LayVaiTroTheoNguoiDung @TenDangNhap",
        //                new SqlParameter("@TenDangNhap", TenDangNhap)
        //            ).ToList();
        //        }
        //        catch
        //        {
        //            roles = new List<string>();
        //        }

        //        var rolesString = string.Join(",", roles);

        //        var ticket = new FormsAuthenticationTicket(1, TenDangNhap, DateTime.Now, DateTime.Now.AddHours(8), false, rolesString);
        //        var encrypted = FormsAuthentication.Encrypt(ticket);
        //        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
        //        {
        //            HttpOnly = true,
        //            Secure = Request.IsSecureConnection
        //        };
        //        Response.Cookies.Add(cookie);

        //        // migrate anonymous session cart/wish
        //        var username = TenDangNhap;
        //        var anonCart = Session["Cart"] as List<Cart>;
        //        if (anonCart != null)
        //        {
        //            var userKey = "Cart_" + username;
        //            var userCart = Session[userKey] as List<Cart> ?? new List<Cart>();
        //            foreach (var it in anonCart)
        //            {
        //                var existing = userCart.FirstOrDefault(c => c.ProductId == it.ProductId);
        //                if (existing != null) existing.Quantity += it.Quantity;
        //                else userCart.Add(it);
        //            }
        //            Session[userKey] = userCart;
        //            Session.Remove("Cart");
        //        }
        //        var anonWish = Session["Wish"] as List<string>;
        //        if (anonWish != null)
        //        {
        //            var userWishKey = "Wish_" + username;
        //            var userWish = Session[userWishKey] as List<string> ?? new List<string>();
        //            foreach (var pid in anonWish)
        //            {
        //                if (!userWish.Contains(pid)) userWish.Add(pid);
        //            }
        //            Session[userWishKey] = userWish;
        //            Session.Remove("Wish");
        //        }

        //        // Redirect logic: ReturnUrl > admin if Admin role > homepage
        //        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        //        {
        //            return Redirect(returnUrl);
        //        }

        //        if (roles.Any(r => string.Equals(r, "Admin", StringComparison.OrdinalIgnoreCase)))
        //        {
        //            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        //        }

        //        return RedirectToAction("Trang_chu", "Home");
        //    }

        //    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dang_xuat()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Dang_nhap");
        }

        // GET: Dang_ky
        public ActionResult Dang_ky()
        {
            return View();
        }

        // POST: Dang_ky
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dang_ky(string TenDangNhap, string MatKhau, string XacNhanMatKhau, string Email, string TenHienThi, string AdminCode = null)
        {
            if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau) || string.IsNullOrWhiteSpace(Email))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin bắt buộc.");
                return View();
            }

            if (MatKhau != XacNhanMatKhau)
            {
                ModelState.AddModelError("", "Mật khẩu và xác nhận mật khẩu không khớp.");
                return View();
            }

            // Check uniqueness
            if (db.TAIKHOANs.Any(u => u.TenDangNhap == TenDangNhap))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                return View();
            }
            if (db.TAIKHOANs.Any(u => u.Email == Email))
            {
                ModelState.AddModelError("", "Email đã được sử dụng.");
                return View();
            }

            // Create salt (16 bytes) and compute SHA256(password + salt) to match seeded users
            var salt = Guid.NewGuid().ToByteArray(); // 16 bytes
            var pwdBytes = Encoding.UTF8.GetBytes(MatKhau);
            var concat = new byte[pwdBytes.Length + salt.Length];
            Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
            Buffer.BlockCopy(salt, 0, concat, pwdBytes.Length, salt.Length);

            byte[] hash;
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                hash = sha.ComputeHash(concat);
            }

            var newUser = new TAIKHOAN
            {
                TenDangNhap = TenDangNhap,
                MatKhauHash = hash,
                Salt = salt,
                TenHienThi = string.IsNullOrWhiteSpace(TenHienThi) ? TenDangNhap : TenHienThi,
                Email = Email,
                IsActive = true,
                NgayTao = DateTime.Now
            };

            db.TAIKHOANs.Add(newUser);
            db.SaveChanges();

            // Assign default role "User" (create role if missing) and map to TAIKHOAN_VAITRO
            try
            {
                var userRole = db.VAITROes.SingleOrDefault(r => r.TenVaiTro == "User");
                if (userRole == null)
                {
                    userRole = new VAITRO { TenVaiTro = "User", MoTa = "Người dùng" };
                    db.VAITROes.Add(userRole);
                    db.SaveChanges();
                }

                var mapping = new TAIKHOAN_VAITRO { MaTK = newUser.MaTK, MaVaiTro = userRole.MaVaiTro };
                db.TAIKHOAN_VAITRO.Add(mapping);
                db.SaveChanges();

                // Optional: create Admin role if AdminCode matches configured secret
                var secret = System.Configuration.ConfigurationManager.AppSettings["AdminRegistrationKey"];
                if (!string.IsNullOrEmpty(secret) && !string.IsNullOrEmpty(AdminCode) && AdminCode == secret)
                {
                    var adminRole = db.VAITROes.SingleOrDefault(r => r.TenVaiTro == "Admin");
                    if (adminRole == null)
                    {
                        adminRole = new VAITRO { TenVaiTro = "Admin", MoTa = "Quản trị hệ thống" };
                        db.VAITROes.Add(adminRole);
                        db.SaveChanges();
                    }
                    db.TAIKHOAN_VAITRO.Add(new TAIKHOAN_VAITRO { MaTK = newUser.MaTK, MaVaiTro = adminRole.MaVaiTro });
                    db.SaveChanges();
                }
            }
            catch
            {
                // ignore role assign failure
            }

            // Do not auto-login. Show success message and redirect to login page.
            TempData["RegisterSuccess"] = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Dang_nhap", "Account");
        }
        // GET: Quen_mat_khau
        public ActionResult Quen_mat_khau()
        {
            // Thay vì tìm một view không tồn tại, hãy chuyển hướng đến trang đăng nhập
            // và dùng tham số để tự động hiển thị form quên mật khẩu.
            return RedirectToAction("Dang_nhap", new { show_recover = true });
        }
        // POST: Quen_mat_khau
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quen_mat_khau(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ForgotError"] = "Vui lòng nhập email.";
                return RedirectToAction("Dang_nhap", new { show_recover = true });
            }

            var user = db.TAIKHOANs.SingleOrDefault(u => u.Email == email);

            if (user == null)
            {
                TempData["ForgotError"] = "Email không tồn tại trong hệ thống.";
                return RedirectToAction("Dang_nhap", new { show_recover = true });
            }

            // Nếu email hợp lệ, lấy TenDangNhap và chuyển hướng đến trang Reset_mat_khau
            return RedirectToAction("Reset_mat_khau", new { tenDangNhap = user.TenDangNhap });
        }

        // GET: Reset_mat_khau
        public ActionResult Reset_mat_khau(string tenDangNhap)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                return RedirectToAction("Dang_nhap");
            }

            var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == tenDangNhap);
            if (user == null)
            {
                TempData["ForgotError"] = "Tài khoản không hợp lệ.";
                return RedirectToAction("Dang_nhap", new { show_recover = true });
            }

            ViewBag.TenDangNhap = tenDangNhap;
            return View();
        }

        // POST: Reset_mat_khau
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset_mat_khau(string tenDangNhap, string MatKhau, string XacNhanMatKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                return RedirectToAction("Dang_nhap");
            }
            if (MatKhau != XacNhanMatKhau)
            {
                ModelState.AddModelError("", "Mật khẩu và xác nhận mật khẩu không khớp.");
                ViewBag.TenDangNhap = tenDangNhap;
                return View();
            }

            var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == tenDangNhap);
            if (user == null)
            {
                TempData["ResetError"] = "Tài khoản không hợp lệ.";
                return RedirectToAction("Dang_nhap");
            }

            // Cập nhật mật khẩu mới
            var (hash, salt) = HashPassword(MatKhau);
            user.MatKhauHash = hash;
            user.Salt = salt;
            db.SaveChanges();

            TempData["ResetSuccess"] = "Mật khẩu đã được đặt lại thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Dang_nhap");
        }

        // GET: Doi_mat_khau
        public ActionResult Doi_mat_khau()
        {
            return View();
        }

        // POST: Doi_mat_khau
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Doi_mat_khau(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (!User?.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Dang_nhap");
            }

            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu mới và xác nhận không khớp.");
                return View();
            }

            var username = User.Identity.Name;
            var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == username);
            if (user == null)
            {
                return RedirectToAction("Dang_nhap");
            }

            if (user.MatKhauHash == null || user.Salt == null || !VerifyPassword(OldPassword, user.MatKhauHash, user.Salt))
            {
                ModelState.AddModelError("", "Mật khẩu cũ không đúng.");
                return View();
            }

            var (hash, salt) = HashPassword(NewPassword);
            user.MatKhauHash = hash;
            user.Salt = salt;
            db.SaveChanges();

            TempData["ChangePasswordSuccess"] = "Đổi mật khẩu thành công.";
            return RedirectToAction("Doi_mat_khau");
        }


        // Helper class to map PasswordResets query result
        private class PasswordResetRow
        {
            public int Id { get; set; }
            public int MaTK { get; set; }
            public string Token { get; set; }
            public DateTime Expiry { get; set; }
        }

        [Authorize]
        public ActionResult Orders()
        {
            var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Dang_nhap");
            }

            // Lấy danh sách hóa đơn của người dùng và join với các bảng liên quan
            var orders = db.HOADONs
                .Where(h => h.MaTK == user.MaTK)
                .OrderByDescending(h => h.NgayDat)
                .Select(h => new OrderViewModel
                {
                    OrderNumber = h.MaHD.ToString(),
                    OrderDate = h.NgayDat,
                    TotalAmount = h.TongTien,
                    PaymentStatus = h.TrangThai,
                    ShippingAddress = h.DiaChiNhan,
                    RecipientName = h.TenNguoiNhan,
                    RecipientPhone = h.SDTNhan,
                    PaymentMethod = h.PhuongThucThanhToan,
                    Note = h.GhiChu,
                    OrderDetails = h.CTHDs.Select(ct => new OrderDetailViewModel
                    {
                        MaSP = ct.MaSP,
                        TenSP = ct.SANPHAM.TenSP,
                        AnhSP = ct.SANPHAM.AnhSP,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia
                    }).ToList()
                }).ToList();

            return View(orders);
        }
    }
}