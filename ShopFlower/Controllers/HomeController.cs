using ShopFlower.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        
    }
}