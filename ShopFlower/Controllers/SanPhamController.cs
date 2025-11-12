using ShopFlower.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        public ActionResult tat_ca_san_pham(string sortOrder)
        {
            var listBoHoaTuoi = db.SANPHAMs.ToList();
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "default";
            }

            ViewBag.CurrentSort = sortOrder;
            var products = db.SANPHAMs.AsQueryable();
            switch (sortOrder)
            {
                case "alpha-asc":
                    products = products.OrderBy(p => p.TenSP);
                    break;
                case "alpha-desc":
                    products = products.OrderByDescending(p => p.TenSP);
                    break;
                case "price-asc":
                    products = products.OrderBy(p => p.GiaBan);
                    break;
                case "price-desc":
                    products = products.OrderByDescending(p => p.GiaBan);
                    break;
                default:
                    products = products.OrderBy(p => p.MaSP);
                    break;
            }
            return View(listBoHoaTuoi);
        }
        public ActionResult DetailsDept(string id)
        {
            var d = db.SANPHAMs.FirstOrDefault(x => x.MaSP == id);
            return View(d);
        }
    }
}