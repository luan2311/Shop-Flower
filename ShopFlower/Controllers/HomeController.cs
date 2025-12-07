using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopFlower.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();
        public ActionResult Trang_chu()
        {
            var lstAllSP = db.SANPHAMs.ToList();

            var boHoaTuoi = db.SANPHAMs.Where(sp => sp.MaLoai == "LH001").Take(8).ToList();
            var keHoa = db.SANPHAMs.Where(sp => sp.MaLoai == "LH002").Take(8).ToList();
            var gioHoa = db.SANPHAMs.Where(sp => sp.MaLoai == "LH003").Take(8).ToList();
            var hoaSap = db.SANPHAMs.Where(sp => sp.MaLoai == "LH005").Take(8).ToList();
            var hoaCuoi = db.SANPHAMs.Where(sp => sp.MaLoai == "LH004").Take(8).ToList();

            // Truyền dữ liệu sang View
            
            ViewBag.BoHoaTuoi = boHoaTuoi;
            ViewBag.KeHoa = keHoa;
            ViewBag.GioHoa = gioHoa;
            ViewBag.HoaSap = hoaSap;
            ViewBag.HoaCuoi = hoaCuoi;
            return View();
        }

        public ActionResult page_not_found()
        {
            return View();
        }

        public ActionResult Lien_he()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lien_he(LIENHE model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Tạo ObjectParameter cho output parameter
                    var maLHParam = new ObjectParameter("MaLH", typeof(string));
                    
                    // Gọi Stored Procedure
                    int result = db.sp_ThemLienHe(
                        model.HOTEN,
                        model.EMAIL,
                        model.DIENTHOAI,
                        model.NOIDUNG,
                        maLHParam
                    );

                    // Lấy giá trị MaLH được tạo
                    string maLH = maLHParam.Value?.ToString();

                    if (result >= 0 || !string.IsNullOrEmpty(maLH))
                    {
                        ViewBag.SuccessMessage = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất.";
                        
                        // Clear form sau khi gửi thành công
                        ModelState.Clear();
                        return View(new LIENHE());
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Đã xảy ra lỗi khi gửi thông tin. Vui lòng thử lại!";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Đã xảy ra lỗi: " + ex.Message;
                    return View(model);
                }
            }

            return View(model);
        }

        public ActionResult Tin_tuc()
        {
            var danhSachTinTuc = db.TINTUCs.ToList();
            return View(danhSachTinTuc);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
                return RedirectToAction("page_not_found", "Home");

            // Lấy tin tức theo id
            var tinTuc = db.TINTUCs.FirstOrDefault(t => t.MATT == id);


            if (tinTuc == null)
                return RedirectToAction("page_not_found", "Home");

            // Lấy sản phẩm liên quan (ví dụ: cùng loại, trừ sản phẩm hiện tại)
            var tintucLienQuan = db.TINTUCs.Where(sp =>sp.MATT != tinTuc.MATT).Take(3).ToList();

            ViewBag.tinTucLienQuan = tintucLienQuan;
            ViewBag.tinTucNoiBat = db.TINTUCs.OrderByDescending(t => t.NGAYTHEM).Take(5).ToList();
            return View(tinTuc);
        }
    }
}