using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace ShopFlower.APIController
{
    public class AccountController : ApiController
    {
        private readonly QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

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

        public class LoginModel
        {
            public string TenDangNhap { get; set; }
            public string MatKhau { get; set; }
        }

        public class RegisterModel
        {
            public string TenDangNhap { get; set; }
            public string MatKhau { get; set; }
            public string Email { get; set; }
            public string TenHienThi { get; set; }
        }

        // POST: api/Account/Login
        [HttpPost]
        [Route("api/Account/Login")]
        public IHttpActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.TenDangNhap) || string.IsNullOrWhiteSpace(model.MatKhau))
                {
                    return BadRequest("Vui lòng cung cấp tên đăng nhập và mật khẩu");
                }

                var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == model.TenDangNhap);
                if (user == null || !user.IsActive)
                {
                    return BadRequest("Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản đã bị vô hiệu hóa.");
                }

                bool isAuthenticated = false;
                var storedHash = user.MatKhauHash;
                var storedSalt = user.Salt;

                if (storedHash != null && storedSalt != null && storedHash.Length > 0 && storedSalt.Length > 0)
                {
                    // 1. PBKDF2 verify
                    if (VerifyPassword(model.MatKhau, storedHash, storedSalt))
                    {
                        isAuthenticated = true;
                    }
                    else
                    {
                        // 2. SHA256 fallback
                        using (var sha = SHA256.Create())
                        {
                            var pwdBytes = Encoding.UTF8.GetBytes(model.MatKhau);
                            var concat = new byte[pwdBytes.Length + storedSalt.Length];
                            Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
                            Buffer.BlockCopy(storedSalt, 0, concat, pwdBytes.Length, storedSalt.Length);
                            var computed = sha.ComputeHash(concat);
                            if (computed.Length == storedHash.Length && computed.SequenceEqual(storedHash))
                            {
                                isAuthenticated = true;
                                // Auto upgrade to PBKDF2
                                try
                                {
                                    var (newHash, newSalt) = HashPassword(model.MatKhau);
                                    user.MatKhauHash = newHash;
                                    user.Salt = newSalt;
                                    db.SaveChanges();
                                }
                                catch { }
                            }
                        }
                    }
                }

                // Stored Procedure verification fallback
                if (!isAuthenticated)
                {
                    try
                    {
                        var maTK = db.Database.SqlQuery<int?>(
                            "EXEC dbo.sp_XacThucTaiKhoan @TenDangNhap, @MatKhau",
                            new SqlParameter("@TenDangNhap", model.TenDangNhap),
                            new SqlParameter("@MatKhau", model.MatKhau)
                        ).FirstOrDefault();

                        if (maTK.HasValue)
                        {
                            isAuthenticated = true;
                        }
                    }
                    catch { }
                }

                if (isAuthenticated)
                {
                    // Lấy vai trò của tài khoản
                    List<string> roles;
                    try
                    {
                        roles = db.Database.SqlQuery<string>(
                            "EXEC dbo.sp_LayVaiTroTheoNguoiDung @TenDangNhap",
                            new SqlParameter("@TenDangNhap", model.TenDangNhap)
                        ).ToList();
                    }
                    catch
                    {
                        roles = new List<string> { "User" };
                    }

                    return Ok(new
                    {
                        success = true,
                        message = "Đăng nhập thành công",
                        profile = new
                        {
                            MaTK = user.MaTK,
                            TenDangNhap = user.TenDangNhap,
                            TenHienThi = user.TenHienThi,
                            Email = user.Email,
                            Roles = roles
                        }
                    });
                }

                return BadRequest("Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi đăng nhập: {ex.Message}"));
            }
        }

        // POST: api/Account/Register
        [HttpPost]
        [Route("api/Account/Register")]
        public IHttpActionResult Register([FromBody] RegisterModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.TenDangNhap) || string.IsNullOrWhiteSpace(model.MatKhau) || string.IsNullOrWhiteSpace(model.Email))
                {
                    return BadRequest("Vui lòng điền đầy đủ các thông tin bắt buộc");
                }

                if (db.TAIKHOANs.Any(u => u.TenDangNhap == model.TenDangNhap))
                {
                    return BadRequest("Tên đăng nhập đã tồn tại");
                }

                if (db.TAIKHOANs.Any(u => u.Email == model.Email))
                {
                    return BadRequest("Email đã được đăng ký");
                }

                // Tạo salt & băm mật khẩu
                var salt = Guid.NewGuid().ToByteArray(); // 16 bytes
                var pwdBytes = Encoding.UTF8.GetBytes(model.MatKhau);
                var concat = new byte[pwdBytes.Length + salt.Length];
                Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
                Buffer.BlockCopy(salt, 0, concat, pwdBytes.Length, salt.Length);

                byte[] hash;
                using (var sha = SHA256.Create())
                {
                    hash = sha.ComputeHash(concat);
                }

                var newUser = new TAIKHOAN
                {
                    TenDangNhap = model.TenDangNhap,
                    MatKhauHash = hash,
                    Salt = salt,
                    TenHienThi = string.IsNullOrWhiteSpace(model.TenHienThi) ? model.TenDangNhap : model.TenHienThi,
                    Email = model.Email,
                    IsActive = true,
                    NgayTao = DateTime.Now
                };

                db.TAIKHOANs.Add(newUser);
                db.SaveChanges();

                // Gán vai trò mặc định "User"
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
                }
                catch { }

                return Ok(new
                {
                    success = true,
                    message = "Đăng ký thành công",
                    profile = new
                    {
                        MaTK = newUser.MaTK,
                        TenDangNhap = newUser.TenDangNhap,
                        TenHienThi = newUser.TenHienThi,
                        Email = newUser.Email
                    }
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi đăng ký: {ex.Message}"));
            }
        }

        // GET: api/Account/Profile?username=abc
        [HttpGet]
        [Route("api/Account/Profile")]
        public IHttpActionResult GetProfile(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest("Vui lòng cung cấp tên đăng nhập");
                }

                var user = db.TAIKHOANs.FirstOrDefault(u => u.TenDangNhap == username);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    MaTK = user.MaTK,
                    TenDangNhap = user.TenDangNhap,
                    TenHienThi = user.TenHienThi,
                    Email = user.Email,
                    NgayTao = user.NgayTao
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi lấy thông tin tài khoản: {ex.Message}"));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
