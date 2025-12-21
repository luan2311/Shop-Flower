using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class CheckoutController : Controller
    {
        private QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        #region Helper Methods

        private int? GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = db.TAIKHOANs.FirstOrDefault(u => u.TenDangNhap == username);
                return user?.MaTK;
            }
            return null;
        }

        private string GetCartKey()
        {
            var username = User.Identity.Name;
            return !string.IsNullOrEmpty(username) ? "Cart_" + username : "Cart";
        }

        private string GetConnectionString()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QL_SHOPFLOWEREntities"].ConnectionString;
            var builder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(connStr);
            return builder.ProviderConnectionString;
        }

        private List<Cart> LayGioHang()
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return GetCartFromDatabase(userId.Value);
            }
            else
            {
                return GetCartFromSession();
            }
        }

        private List<Cart> GetCartFromDatabase(int maTK)
        {
            var cartList = new List<Cart>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetCartItems", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cartList.Add(new Cart(
                                    maCartItem: Convert.ToInt32(reader["MaCartItem"]),
                                    productId: reader["MaSP"].ToString(),
                                    productName: reader["TenSP"].ToString(),
                                    productImage: reader["AnhSP"].ToString(),
                                    price: Convert.ToDouble(reader["GiaBan"]),
                                    quantity: Convert.ToInt32(reader["SoLuong"])
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting cart from database: {ex.Message}");
            }

            return cartList;
        }

        private List<Cart> GetCartFromSession()
        {
            var sessionKey = GetCartKey();
            var cart = Session[sessionKey] as List<Cart>;

            if (cart == null)
            {
                cart = new List<Cart>();
            }

            return cart;
        }

        #endregion

        // GET: Checkout
        [Authorize]
        public ActionResult ThanhToan()
        {
            var cart = LayGioHang();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Cart", "Cart");
            }

            var model = new ThanhToanViewModel
            {
                CartItems = cart,
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
            var cart = LayGioHang();

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

            // Tạo địa chỉ đầy đủ theo format: địa chỉ, phường/xã, quận/huyện, tỉnh/thành phố
            var diaChiDayDu = string.Format("{0}, {1}, {2}, {3}",
                model.DiaChiGiaoHang,
                model.PhuongXa,
                model.QuanHuyen,
                model.TinhThanh);

            // 1. Tạo và lưu hóa đơn
            var hoadon = new HOADON
            {
                MaTK = user.MaTK,
                NgayDat = DateTime.Now,
                TongTien = model.TongTien,
                DiaChiNhan = diaChiDayDu, // Lưu địa chỉ đầy đủ
                SDTNhan = model.SoDienThoai,
                TenNguoiNhan = model.HoTenNguoiNhan,
                Email = model.Email,
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
            ClearCart(user.MaTK);

            // 4. Chuyển đến trang lịch sử đơn hàng
            TempData["OrderSuccess"] = "Bạn đã đặt hàng thành công!";
            return RedirectToAction("Orders", "Account");
        }

        private void ClearCart(int maTK)
        {
            try
            {
                // Xóa giỏ hàng từ database nếu người dùng đã đăng nhập
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ClearCart", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing cart from database: {ex.Message}");
            }

            // Xóa session (để đảm bảo)
            var cartKey = GetCartKey();
            Session.Remove(cartKey);
        }

        public ActionResult OrderConfirmation(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}