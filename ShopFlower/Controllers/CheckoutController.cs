using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class CheckoutController : Controller
    {
        private QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        private string GetCartKey()
        {
            if (User.Identity.IsAuthenticated)
            {
                return "Cart_" + User.Identity.Name;
            }
            return "Cart"; // Giỏ hàng cho khách vãng lai
        }

        // GET: Checkout
        [Authorize]
        public ActionResult ThanhToan()
        {
            // **QUAN TRỌNG**: Lấy đúng session key
            var cartKey = User.Identity.IsAuthenticated ? "Cart_" + User.Identity.Name : "Cart";
            var cart = Session[cartKey] as List<Cart>;

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Cart", "Cart");
            }

            var model = new ThanhToanViewModel
            {
                CartItems = cart, // Truyền giỏ hàng chứa thông tin ảnh
                TongTien = (decimal)cart.Sum(item => item.TotalPrice)
            };

            var user = db.TAIKHOANs.FirstOrDefault(u => u.TenDangNhap == User.Identity.Name);
            if (user != null)
            {
                model.HoTenNguoiNhan = user.TenHienThi;
                model.Email = user.Email;
            }

            return View("ThanhToan", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(ThanhToanViewModel model)
        {
            var cartKey = GetCartKey();
            var cart = Session[cartKey] as List<Cart>;

            if (cart == null || !cart.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
                return View("ThanhToan", model);
            }

            model.CartItems = cart;
            model.TongTien = (decimal)cart.Sum(item => item.TotalPrice);

            if (!ModelState.IsValid)
            {
                return View("ThanhToan", model);
            }

            // Lấy thông tin tài khoản để lưu vào đơn hàng
            var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == User.Identity.Name);
            if (user == null)
            {
                // Xử lý trường hợp không tìm thấy người dùng (dù đã đăng nhập)
                return RedirectToAction("Dang_nhap", "Account");
            }

            // 1. Tạo và lưu hóa đơn
            var hoadon = new HOADON
            {
                MaTK = user.MaTK,
                NgayDat = DateTime.Now,
                TongTien = model.TongTien,
                DiaChiNhan = model.DiaChiGiaoHang,
                SDTNhan = model.SoDienThoai,
                TenNguoiNhan = model.HoTenNguoiNhan,
                GhiChu = model.GhiChu,
                TrangThai = "Pending",
                PhuongThucThanhToan = model.PhuongThucThanhToan
            };
            db.HOADONs.Add(hoadon);
            db.SaveChanges(); // Lưu để có MaHD cho chi tiết hóa đơn

            // 2. Lưu chi tiết hóa đơn
            foreach (var item in cart)
            {
                var cthd = new CTHD
                {
                    MaHD = hoadon.MaHD,
                    MaSP = item.ProductId,
                    SoLuong = item.Quantity,
                    DonGia = (decimal)item.Price
                };
                db.CTHDs.Add(cthd);
            }
            db.SaveChanges();

            // 3. Xóa giỏ hàng sau khi đặt hàng thành công
            Session.Remove(cartKey);

            // 4. Chuyển đến trang lịch sử đơn hàng
            TempData["OrderSuccess"] = "Bạn đã đặt hàng thành công!";
            return RedirectToAction("Orders", "Account");
        }

        public ActionResult OrderConfirmation(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}