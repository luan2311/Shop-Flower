using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        public ActionResult tat_ca_san_pham(int page = 1, string sortOrder = "")
        {
            int pageSize = 12; // Số sản phẩm trên mỗi trang
            var products = db.SANPHAMs.AsQueryable();

            // Sắp xếp sản phẩm theo yêu cầu
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

            // Phân trang
            int totalProducts = products.Count();
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentSort = sortOrder;

            return View(paginatedProducts);
        }

        public ActionResult bo_hoa_tuoi(int page = 1, string sortOrder = "")
        {
            int pageSize = 8; // Số sản phẩm trên mỗi trang
            var products = db.SANPHAMs.Where(x=>x.MaLoai == "LH001").AsQueryable();

            // Sắp xếp sản phẩm theo yêu cầu
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

            // Phân trang
            int totalProducts = products.Count();
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentSort = sortOrder;

            return View(paginatedProducts);
        }

        public ActionResult ke_hoa_chuc_mung(int page = 1, string sortOrder = "")
        {
            int pageSize = 8; // Số sản phẩm trên mỗi trang
            var products = db.SANPHAMs.Where(x => x.MaLoai == "LH002").AsQueryable();

            // Sắp xếp sản phẩm theo yêu cầu
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

            // Phân trang
            int totalProducts = products.Count();
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentSort = sortOrder;

            return View(paginatedProducts);
        }

        public ActionResult gio_hoa_chuc_mung(int page = 1, string sortOrder = "")
        {
            int pageSize = 8; // Số sản phẩm trên mỗi trang
            var products = db.SANPHAMs.Where(x => x.MaLoai == "LH003").AsQueryable();

            // Sắp xếp sản phẩm theo yêu cầu
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

            // Phân trang
            int totalProducts = products.Count();
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentSort = sortOrder;

            return View(paginatedProducts);
        }

        public ActionResult hoa_cuoi(int page = 1, string sortOrder = "")
        {
            int pageSize = 8; // Số sản phẩm trên mỗi trang
            var products = db.SANPHAMs.Where(x => x.MaLoai == "LH004").AsQueryable();

            // Sắp xếp sản phẩm theo yêu cầu
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

            // Phân trang
            int totalProducts = products.Count();
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentSort = sortOrder;

            return View(paginatedProducts);
        }

        public ActionResult hoa_sap(int page = 1, string sortOrder = "")
        {
            int pageSize = 8; // Số sản phẩm trên mỗi trang
            var products = db.SANPHAMs.Where(x => x.MaLoai == "LH005").AsQueryable();

            // Sắp xếp sản phẩm theo yêu cầu
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

            // Phân trang
            int totalProducts = products.Count();
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentSort = sortOrder;

            return View(paginatedProducts);
        }

       public ActionResult chi_tiet_san_pham(string id)
       {
            if (id == null)
                return RedirectToAction("page_not_found", "Home");

            // Lấy sản phẩm theo id
            var product = db.SANPHAMs.FirstOrDefault(x => x.MaSP == id);

            if (product == null)
                return RedirectToAction("page_not_found", "Home");

            // Lấy sản phẩm liên quan (ví dụ: cùng loại, trừ sản phẩm hiện tại)
            var sanPhamLienQuan = db.SANPHAMs.Where(sp => sp.MaLoai == product.MaLoai && sp.MaSP != product.MaSP).Take(4).ToList();
            
            ViewBag.SanPhamLienQuan = sanPhamLienQuan;

            return View(product);
       }

        public ActionResult Search(string query, int page = 1)
        {
            int pageSize = 8;

            // Kiểm tra từ khóa tìm kiếm
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("tat_ca_san_pham");
            }

            // Gọi Stored Procedure
            var products = db.Database.SqlQuery<SANPHAM>(
                "EXEC SearchProducts @Keyword",
                new SqlParameter("@Keyword", query)
            ).ToList();

            // Phân trang
            int totalProducts = products.Count();
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.Keyword = query;

            return View(paginatedProducts);
        }
    }
}