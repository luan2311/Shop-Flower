using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();
        public List<Cart> LayGioHang()
        {
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang == null)
            {
                //Nếu lstGioHang chưa tồn tại thì khởi tạo mới'
                lstGioHang = new List<Cart>();
                Session["Cart"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(string ms, string strURL)
        {
            //Lấy giỏ hàng
            List<Cart> lstGioHang = LayGioHang();
            //Kiểm tra hoa có tồn tại trong Session["Cart"] chưa?
            Cart SP = lstGioHang.Find(x => x.ProductId == ms);
            if (SP == null) //chưa có trong giỏ
            {
                SP = new Cart(ms);
                lstGioHang.Add(SP);
                return Redirect(strURL);
            }
            else // có thì +1
            {
                SP.Quantity++;
                return Redirect(strURL);
            }
        }

        //Tổng số lượng
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(x => x.Quantity);
            }
            return tsl;
        }

        //Tổng thành tiền
        private double TongThanhTien()
        {
            double ttt = 0;
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang != null)
            {
                ttt = lstGioHang.Sum(x => x.TotalPrice);
            }
            return ttt;
        }

        //Trang Giỏ hàng
        public ActionResult Cart()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("empty_cart", "Cart");
            }
            List<Cart> lstGioHang = LayGioHang();

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(lstGioHang);
        }

        public ActionResult empty_cart()
        {
            return View();
        }

        //Icon Giỏ hàng + Show Số lượng SP trong giỏ
        public ActionResult CartPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        //Xóa 1 SP trong Cart
        public ActionResult XoaGioHang(string MaSP)
        {
            List<Cart> lstGioHang = LayGioHang();
            //kiểm tra hoa cần xóa còn trong Cart không
            Cart SP = lstGioHang.SingleOrDefault(s => s.ProductId == MaSP);
            if (SP != null) //có thì xóa
            {
                lstGioHang.RemoveAll(s => s.ProductId == MaSP);
                
                if (lstGioHang.Count == 0) //Cart empty
                {
                    return RedirectToAction("empty_cart", "Cart");
                }
                return RedirectToAction("Cart", "Cart");
            }
            return RedirectToAction("Cart", "Cart");
        }

        //Xóa ALL SP
        public ActionResult XoaGioHang_All()
        {
            //Lấy giỏ hàng
            List<Cart> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("empty_cart", "Cart");
        }

        //Cập nhật số lượng giỏ hàng
        public ActionResult CapNhatGioHang(string MaSP, FormCollection f)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart SP = lstGioHang.Single(x => x.ProductId == MaSP);
            //nếu có thì update
            if (SP != null)
            {
                SP.Quantity = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Cart", "Cart");
        }
    }
}