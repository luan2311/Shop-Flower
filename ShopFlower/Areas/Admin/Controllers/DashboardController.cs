using PagedList;
using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ShopFlower.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();
        
        // --- Password Hashing Helpers (from AccountController) ---
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

        // GET: Admin/Dashboard
        [OverrideAuthorization]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            var username = User?.Identity?.Name;
            TAIKHOAN model = null;
            if (!string.IsNullOrEmpty(username))
            {
                model = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == username);
                if (model != null)
                {
                    var isadmin = model.TAIKHOAN_VAITRO.Any(vt => vt.VAITRO.TenVaiTro == "Admin");
                    if (isadmin)
                    {
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return View(model);
        }
        // GET: Admin/Dashboard/Dashboard (Trang mới)
        [OverrideAuthorization]
        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {
            var today = DateTime.Today;

            var viewModel = new DashboardViewModel
            {
                TotalProducts = db.SANPHAMs.Count(),
                TotalUsers = db.TAIKHOANs.Count(),
                TodaysOrders = db.HOADONs.Count(o => o.TrangThai == "Completed"),

                TotalRevenue = db.HOADONs.Where(h => h.TrangThai == "Completed").Sum(h => (decimal?)h.TongTien) ?? 0,
                RecentOrders = db.HOADONs.OrderByDescending(o => o.NgayDat).Take(10).ToList(),
                TopSellingProducts = db.CTHDs
                                      .GroupBy(ct => ct.SANPHAM)
                                      .Select(g => new TopSellingProduct
                                      {
                                          ProductName = g.Key.TenSP,
                                          SL = g.Sum(ct => ct.SoLuong),
                                          TotalSold = g.Sum(ct => ct.DonGia)
                                      })
                                      .OrderByDescending(p => p.TotalSold)
                                      .Take(3)
                                      .ToList()
            };

            return View("Dashboard", viewModel);
        }
        public ActionResult QL_SanPham(string searchString, int? page)
        {
            ViewBag.CurrentFilter = searchString;

            var sanPhams = from s in db.SANPHAMs select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.TenSP.Contains(searchString)
                                            || s.LOAIHANG.TenLoai.Contains(searchString));
            }


            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(sanPhams.OrderBy(t => t.TenSP).ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Dashboard/CreateSanPham
        public ActionResult CreateSanPham()
        {
            ViewBag.MaLoai = new SelectList(db.LOAIHANGs, "MaLoai", "TenLoai");
            return View();
        }

        //// POST: Admin/Dashboard/CreateSanPham
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSanPham([Bind(Include = "MaSP,TenSP,GiaBan,MoTaSP,TinhTrang,ThuongHieu,SoLuongTon,MaLoai")] SANPHAM sanpham, HttpPostedFileBase AnhBiaFile)
        {
            if (ModelState.IsValid)
            {
                if (AnhBiaFile != null && AnhBiaFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(AnhBiaFile.FileName);

                    sanpham.AnhSP = fileName;
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng chọn ảnh bìa.");
                    return View(sanpham);
                }
                db.SANPHAMs.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("QL_SanPham");
            }

            ViewBag.MaLoai = new SelectList(db.LOAIHANGs, "MaLoai", "TenLoai", sanpham.MaLoai);
            return View(sanpham);
        }
        
        // GET: Admin/Dashboard/EditSanPham/5
        public ActionResult EditSanPham(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sanPham = db.SANPHAMs.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoai = new SelectList(db.LOAIHANGs, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // POST: Admin/Dashboard/EditSanPham/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSanPham([Bind(Include = "MaSP,TenSP,GiaBan,AnhSP,MoTaSP,TinhTrang,ThuongHieu,SoLuongTon,MaLoai")] SANPHAM sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QL_SanPham");
            }
            ViewBag.MaLoai = new SelectList(db.LOAIHANGs, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: Admin/Dashboard/DeleteSanPham/5
        public ActionResult DeleteSanPham(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sanPham = db.SANPHAMs.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/Dashboard/DeleteSanPham/5
        [HttpPost, ActionName("DeleteSanPham")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSanPhamConfirmed(string id)
        {
            SANPHAM sanPham = db.SANPHAMs.Find(id);
            db.SANPHAMs.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("QL_SanPham");
        }

        // QL_HoaDon Actions
        public ActionResult QL_HoaDon(string searchString, int? page)
        {
            ViewBag.CurrentFilter = searchString;
            var hoaDons = db.HOADONs.Include(h => h.TAIKHOAN).OrderByDescending(h => h.NgayDat);

            if (!String.IsNullOrEmpty(searchString))
            {
                hoaDons = hoaDons.Where(h => h.TenNguoiNhan.Contains(searchString) || h.TAIKHOAN.TenDangNhap.Contains(searchString)).OrderByDescending(h => h.NgayDat);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(hoaDons.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Dashboard/DetailsHoaDon/5
        public ActionResult DetailsHoaDon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hoaDon = db.HOADONs.Include(h => h.CTHDs.Select(c => c.SANPHAM)).SingleOrDefault(h => h.MaHD == id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderStatus(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var order = db.HOADONs.Find(id);
            if (order == null)
                return HttpNotFound();

            order.TrangThai = "Completed";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("QL_HoaDon");
        }

        // QL_TaiKhoan Actions
        public ActionResult QL_TaiKhoan(string searchString, int? page)
        {
            ViewBag.CurrentFilter = searchString;
            var taiKhoans = from t in db.TAIKHOANs select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                taiKhoans = taiKhoans.Where(t => t.TenDangNhap.Contains(searchString) || t.Email.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(taiKhoans.OrderBy(t => t.TenDangNhap).ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Dashboard/Create_Account
        public ActionResult Create_Account()
        {
            ViewBag.Roles = new MultiSelectList(db.VAITROes, "MaVaiTro", "TenVaiTro");
            return View();
        }

        // POST: Admin/Dashboard/Create_Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Account(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.TAIKHOANs.Any(u => u.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                }

                if (db.TAIKHOANs.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                }

                if (ModelState.IsValid)
                {
                    var (hash, salt) = HashPassword(model.MatKhau);

                    var taiKhoan = new TAIKHOAN
                    {
                        TenDangNhap = model.TenDangNhap,
                        Email = model.Email,
                        TenHienThi = model.TenHienThi,
                        MatKhauHash = hash,
                        Salt = salt,
                        IsActive = true,
                        NgayTao = DateTime.Now
                    };

                    db.TAIKHOANs.Add(taiKhoan);
                    db.SaveChanges(); // Save to get the new MaTK

                    if (model.SelectedRoles != null)
                    {
                        foreach (var roleId in model.SelectedRoles)
                        {
                            var roleMapping = new TAIKHOAN_VAITRO { MaTK = taiKhoan.MaTK, MaVaiTro = roleId };
                            db.TAIKHOAN_VAITRO.Add(roleMapping);
                        }
                    }
                    else // Assign "User" role by default if no roles are selected
                    {
                        var userRole = db.VAITROes.SingleOrDefault(r => r.TenVaiTro == "User");
                        if (userRole == null)
                        {
                            userRole = new VAITRO { TenVaiTro = "User", MoTa = "Người dùng" };
                            db.VAITROes.Add(userRole);
                            db.SaveChanges();
                        }
                        var roleMapping = new TAIKHOAN_VAITRO { MaTK = taiKhoan.MaTK, MaVaiTro = userRole.MaVaiTro };
                        db.TAIKHOAN_VAITRO.Add(roleMapping);
                    }

                    db.SaveChanges();
                    return RedirectToAction("QL_TaiKhoan");
                }
            }

            ViewBag.Roles = new MultiSelectList(db.VAITROes, "MaVaiTro", "TenVaiTro", model.SelectedRoles);
            return View(model);
        }

        // POST: Admin/Dashboard/Delete_Account/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Account(int id)
        {
            var userToDelete = db.TAIKHOANs.Find(id);
            if (userToDelete == null)
            {
                return HttpNotFound();
            }

            if (userToDelete.TenDangNhap.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Bạn không thể xóa tài khoản của chính mình.";
                return RedirectToAction("QL_TaiKhoan");
            }

            try
            {
                var result = db.sp_XoaTaiKhoanAnToan(id).FirstOrDefault();

                if (result != null)
                {
                    if (result.Success == 1)
                    {
                        TempData["Success"] = result.Message;
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                    }
                }
                else
                {
                    TempData["Error"] = "Không thể thực hiện thao tác xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Xóa tài khoản thất bại do có lỗi xảy ra.";
            }

            return RedirectToAction("QL_TaiKhoan");
        }


        // POST: Admin/Dashboard/Activate_Account/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate_Account(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var result = db.sp_KichHoatTaiKhoan(id).FirstOrDefault();

                if (result != null)
                {
                    if (result.Success == 1)
                    {
                        TempData["Success"] = result.Message;
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                    }
                }
                else
                {
                    TempData["Error"] = "Không thể thực hiện thao tác kích hoạt.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kích hoạt tài khoản thất bại do có lỗi xảy ra.";
            }

            return RedirectToAction("QL_TaiKhoan");
        }

        // QL_TinTuc Actions
        public ActionResult QL_TinTuc(string searchString, int? page)
        {
            ViewBag.CurrentFilter = searchString;
            var tinTucs = from tt in db.TINTUCs select tt;

            if (!String.IsNullOrEmpty(searchString))
            {
                tinTucs = tinTucs.Where(tt => tt.TIEUDE.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tinTucs.OrderByDescending(t => t.NGAYTHEM).ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Dashboard/CreateTinTuc
        public ActionResult CreateTinTuc()
        {
            return View();
        }
        // POST: Admin/Dashboard/CreateTinTuc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTinTuc([Bind(Include = "MATT,TIEUDE,MOTA")] TINTUC tinTuc, HttpPostedFileBase AnhBiaFile)
        {
            if (ModelState.IsValid)
            {
                if (AnhBiaFile != null && AnhBiaFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(AnhBiaFile.FileName);
                    string folder = Server.MapPath("~/Content/Images/TinTuc");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string path = Path.Combine(folder, fileName);
                    AnhBiaFile.SaveAs(path);

                    tinTuc.ANHBIA = fileName;  // Gán đúng tại đây
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng chọn ảnh bìa.");
                    return View(tinTuc);
                }

                tinTuc.NGAYTHEM = DateTime.Now;

                db.TINTUCs.Add(tinTuc);
                db.SaveChanges();
                return RedirectToAction("QL_TinTuc");
            }

            return View(tinTuc);
        }




        // GET: Admin/Dashboard/EditTinTuc/5
        public ActionResult EditTinTuc(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string key = id.Trim();

            TINTUC tinTuc = db.TINTUCs
                              .FirstOrDefault(t => t.MATT.Trim() == key);

            if (tinTuc == null)
            {
                return HttpNotFound();
            }

            return View(tinTuc);
        }



        // POST: Admin/Dashboard/EditTinTuc/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTinTuc([Bind(Include = "MATT,ANHBIA,TIEUDE,NGAYTHEM,MOTA")] TINTUC tinTuc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TRIM để tránh lỗi CHAR(10)
                    string key = tinTuc.MATT.Trim();

                    var result = db.sp_UpdateTinTuc(
                        key,
                        tinTuc.ANHBIA ?? "",   // tránh NULL cho NVARCHAR
                        tinTuc.TIEUDE,
                        tinTuc.NGAYTHEM,
                        tinTuc.MOTA
                    ).FirstOrDefault();

                    if (result != null && result.Success == 1)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction("QL_TinTuc");
                    }

                    ModelState.AddModelError("", result?.Message ?? "Không thể cập nhật tin tức.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật tin tức.");
                }
            }

            return View(tinTuc);
        }



        // GET: Admin/Dashboard/DeleteTinTuc/5
        public ActionResult DeleteTinTuc(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string key = id.Trim();

            TINTUC tinTuc = db.TINTUCs.FirstOrDefault(t => t.MATT.Trim() == key);

            if (tinTuc == null)
                return HttpNotFound();

            return View(tinTuc);
        }


        // POST: Admin/Dashboard/DeleteTinTuc/5
        [HttpPost, ActionName("DeleteTinTuc")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTinTucConfirmed(string id)
        {
            string key = id.Trim();

            var tinTuc = db.TINTUCs.FirstOrDefault(t => t.MATT.Trim() == key);

            if (tinTuc == null)
                return HttpNotFound();

            db.TINTUCs.Remove(tinTuc);
            db.SaveChanges();

            return RedirectToAction("QL_TinTuc");
        }



        // QL_LienHe ActionsS
        public ActionResult QL_LienHe(int? page)
        {
            var lienHes = db.LIENHEs.ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(lienHes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Dashboard/DetailsLienHe/5
        public ActionResult DetailsLienHe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LIENHE lienHe = db.LIENHEs.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // GET: Admin/Dashboard/DeleteLienHe/5
        public ActionResult DeleteLienHe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LIENHE lienHe = db.LIENHEs.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // POST: Admin/Dashboard/DeleteLienHe/5
        [HttpPost, ActionName("DeleteLienHe")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLienHeConfirmed(int id)
        {
            LIENHE lienHe = db.LIENHEs.Find(id);
            db.LIENHEs.Remove(lienHe);
            db.SaveChanges();
            return RedirectToAction("QL_LienHe");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult SomeAdminOnlyAction()
        {
            // admin-only logic
            return View();
        }
    }
}