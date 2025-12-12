using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopFlower.Models;

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

        //public ActionResult ThemGioHang(string ms, string strURL)
        //{
        //    //Lấy giỏ hàng
        //    List<Cart> lstGioHang = LayGioHang();
        //    //Kiểm tra hoa có tồn tại trong Session["Cart"] chưa?
        //    Cart SP = lstGioHang.Find(x => x.ProductId == ms);
        //    if (SP == null) //chưa có trong giỏ
        //    {
        //        SP = new Cart(ms);
        //        lstGioHang.Add(SP);
        //        return Redirect(strURL);
        //    }
        //    else // có thì +1
        //    {
        //        SP.Quantity++;
        //        return Redirect(strURL);
        //    }
        //}
        public ActionResult ThemGioHang(string ms, string strURL)
        {
            var username = User?.Identity?.Name;
            var sessionKey = !string.IsNullOrEmpty(username) ? "Cart_" + username : "Cart";
            var cart = Session[sessionKey] as List<Cart> ?? new List<Cart>();

            var item = cart.FirstOrDefault(c => c.ProductId == ms);
            if (item != null)
            {
                item.Quantity += 1;
            }
            else
            {
                cart.Add(new Cart(ms));
            }

            Session[sessionKey] = cart;

            if (Request.IsAjaxRequest()) return Json(new { success = true, count = cart.Sum(c => c.Quantity) });
            return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
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
        //public ActionResult Cart()
        //{
        //    if (Session["Cart"] == null)
        //    {
        //        return RedirectToAction("empty_cart", "Cart");
        //    }
        //    List<Cart> lstGioHang = LayGioHang();

        //    ViewBag.TongSoLuong = TongSoLuong();
        //    ViewBag.TongThanhTien = TongThanhTien();
        //    return View(lstGioHang);
        //}
        public ActionResult Cart()
        {
            var username = User?.Identity?.Name;
            var sessionKey = !string.IsNullOrEmpty(username) ? "Cart_" + username : "Cart";
            var cart = Session[sessionKey] as List<Cart> ?? new List<Cart>();

            ViewBag.TongSoLuong = cart.Sum(c => c.Quantity);
            ViewBag.TongThanhTien = cart.Sum(c => c.TotalPrice);
            return View(cart);
        }

        public ActionResult empty_cart()
        {
            return View();
        }

        //Icon Giỏ hàng + Show Số lượng SP trong giỏ
        //public ActionResult CartPartial()
        //{
        //    ViewBag.TongSoLuong = TongSoLuong();
        //    return PartialView();
        //}
        public ActionResult CartPartial()
        {
            var username = User?.Identity?.Name;
            var sessionKey = !string.IsNullOrEmpty(username) ? "Cart_" + username : "Cart";
            var cart = Session[sessionKey] as List<Cart> ?? new List<Cart>();
            ViewBag.CartCount = cart.Sum(c => c.Quantity);
            return PartialView("CartPartial");
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
        //public ActionResult CapNhatGioHang(string MaSP, FormCollection f)
        //{
        //    List<Cart> lstGioHang = LayGioHang();
        //    Cart SP = lstGioHang.Single(x => x.ProductId == MaSP);
        //    //nếu có thì update
        //    if (SP != null)
        //    {
        //        SP.Quantity = int.Parse(f["txtSoLuong"].ToString());
        //    }
        //    return RedirectToAction("Cart", "Cart");
        //}
        //[HttpPost]
        public ActionResult CapNhatGioHang(string MaSP, int txtSoLuong)
        {
            try
            {
                var username = User?.Identity?.Name;
                var sessionKey = !string.IsNullOrEmpty(username) ? "Cart_" + username : "Cart";
                
                // Lấy giỏ hàng từ session với sessionKey đúng
                List<Cart> lstGioHang = Session[sessionKey] as List<Cart>;
                
                if (lstGioHang == null || !lstGioHang.Any())
                {
                    return Json(new { success = false, message = "Giỏ hàng không tồn tại." });
                }

                Cart itemToUpdate = lstGioHang.FirstOrDefault(item => item.ProductId == MaSP);

                if (itemToUpdate != null)
                {
                    // Ensure quantity is at least 1
                    if (txtSoLuong < 1)
                    {
                        txtSoLuong = 1;
                    }
                    itemToUpdate.Quantity = txtSoLuong;
                }
                else
                {
                    return Json(new { success = false, message = "Sản phẩm không tìm thấy trong giỏ." });
                }

                // Recalculate totals correctly
                double cartSubtotal = lstGioHang.Sum(item => item.Price * item.Quantity);
                int cartCount = lstGioHang.Sum(item => item.Quantity);

                // Lưu lại vào session với sessionKey đúng
                Session[sessionKey] = lstGioHang;

                // Return JSON data for the client-side script to use
                return Json(new
                {
                    success = true,
                    itemTotalPrice = (itemToUpdate.Price * itemToUpdate.Quantity).ToString("N0"),
                    cartSubtotal = cartSubtotal.ToString("N0"),
                    cartCount = cartCount
                });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) for debugging
                return Json(new { success = false, message = "Đã có lỗi xảy ra trên máy chủ: " + ex.Message });
            }
        }
    }
}