using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class CartController : Controller
    {
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

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

        private string GetSessionKey()
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

        #endregion

        #region Get Cart

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
            var sessionKey = GetSessionKey();
            var cart = Session[sessionKey] as List<Cart>;

            if (cart == null)
            {
                cart = new List<Cart>();
                Session[sessionKey] = cart;
            }

            return cart;
        }

        #endregion

        #region Add to Cart

        public ActionResult ThemGioHang(string ms, string strURL)
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return ThemGioHangDatabase(userId.Value, ms, strURL);
            }
            else
            {
                return ThemGioHangSession(ms, strURL);
            }
        }

        private ActionResult ThemGioHangDatabase(int maTK, string maSP, string strURL)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_AddToCart", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.Parameters.AddWithValue("@SoLuong", 1);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Request.IsAjaxRequest())
                        {
                            int count = GetCartCountFromDatabase(maTK);
                            return Json(new { success = true, count = count });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
        }

        private ActionResult ThemGioHangSession(string maSP, string strURL)
        {
            var cart = GetCartFromSession();

            var item = cart.FirstOrDefault(c => c.ProductId == maSP);
            if (item != null)
            {
                item.Quantity += 1;
            }
            else
            {
                cart.Add(new Cart(maSP));
            }

            var sessionKey = GetSessionKey();
            Session[sessionKey] = cart;

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true, count = cart.Sum(c => c.Quantity) });
            }

            return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
        }

        #endregion

        #region Update Cart

        public ActionResult CapNhatGioHang(string MaSP, int txtSoLuong)
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return CapNhatGioHangDatabase(userId.Value, MaSP, txtSoLuong);
            }
            else
            {
                return CapNhatGioHangSession(MaSP, txtSoLuong);
            }
        }

        private ActionResult CapNhatGioHangDatabase(int maTK, string maSP, int soLuong)
        {
            try
            {
                if (soLuong < 1) soLuong = 1;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateCartItem", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        var cart = GetCartFromDatabase(maTK);
                        var item = cart.FirstOrDefault(c => c.ProductId == maSP);

                        return Json(new
                        {
                            success = true,
                            itemTotalPrice = item != null ? (item.Price * item.Quantity).ToString("N0") : "0",
                            cartSubtotal = cart.Sum(c => c.TotalPrice).ToString("N0"),
                            cartCount = cart.Sum(c => c.Quantity)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private ActionResult CapNhatGioHangSession(string maSP, int soLuong)
        {
            try
            {
                if (soLuong < 1) soLuong = 1;

                var cart = GetCartFromSession();
                var item = cart.FirstOrDefault(c => c.ProductId == maSP);

                if (item != null)
                {
                    item.Quantity = soLuong;
                }
                else
                {
                    return Json(new { success = false, message = "Sản phẩm không tìm thấy trong giỏ." });
                }

                var sessionKey = GetSessionKey();
                Session[sessionKey] = cart;

                return Json(new
                {
                    success = true,
                    itemTotalPrice = (item.Price * item.Quantity).ToString("N0"),
                    cartSubtotal = cart.Sum(c => c.TotalPrice).ToString("N0"),
                    cartCount = cart.Sum(c => c.Quantity)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Remove from Cart

        public ActionResult XoaGioHang(string MaSP)
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return XoaGioHangDatabase(userId.Value, MaSP);
            }
            else
            {
                return XoaGioHangSession(MaSP);
            }
        }

        private ActionResult XoaGioHangDatabase(int maTK, string maSP)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_RemoveFromCart", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        var cart = GetCartFromDatabase(maTK);

                        if (Request.IsAjaxRequest())
                        {
                            return Json(new
                            {
                                success = true,
                                cartSubtotal = cart.Sum(c => c.TotalPrice).ToString("N0"),
                                cartCount = cart.Sum(c => c.Quantity),
                                isEmpty = cart.Count == 0
                            }, JsonRequestBehavior.AllowGet);
                        }

                        if (cart.Count == 0)
                        {
                            return RedirectToAction("empty_cart", "Cart");
                        }
                        return RedirectToAction("Cart", "Cart");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Cart", "Cart");
            }
        }

        private ActionResult XoaGioHangSession(string maSP)
        {
            var cart = GetCartFromSession();
            var item = cart.SingleOrDefault(s => s.ProductId == maSP);

            if (item != null)
            {
                cart.RemoveAll(s => s.ProductId == maSP);

                var sessionKey = GetSessionKey();
                Session[sessionKey] = cart;

                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        success = true,
                        cartSubtotal = cart.Sum(c => c.TotalPrice).ToString("N0"),
                        cartCount = cart.Sum(c => c.Quantity),
                        isEmpty = cart.Count == 0
                    }, JsonRequestBehavior.AllowGet);
                }

                if (cart.Count == 0)
                {
                    return RedirectToAction("empty_cart", "Cart");
                }
                return RedirectToAction("Cart", "Cart");
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Cart", "Cart");
        }

        #endregion

        #region Clear Cart

        public ActionResult XoaGioHang_All()
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_ClearCart", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaTK", userId.Value);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch { }
            }
            else
            {
                var cart = GetCartFromSession();
                cart.Clear();
                Session[GetSessionKey()] = cart;
            }

            return RedirectToAction("empty_cart", "Cart");
        }

        #endregion

        #region Views

        public ActionResult Cart()
        {
            var cart = LayGioHang();
            ViewBag.TongSoLuong = cart.Sum(c => c.Quantity);
            ViewBag.TongThanhTien = cart.Sum(c => c.TotalPrice);
            return View(cart);
        }

        public ActionResult empty_cart()
        {
            return View();
        }

        public ActionResult CartPartial()
        {
            var userId = GetCurrentUserId();
            int count = 0;

            if (userId.HasValue)
            {
                count = GetCartCountFromDatabase(userId.Value);
            }
            else
            {
                var cart = GetCartFromSession();
                count = cart.Sum(c => c.Quantity);
            }

            ViewBag.CartCount = count;
            return PartialView("CartPartial");
        }

        #endregion

        #region API

        [HttpGet]
        public ActionResult GetCartInfo()
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                var cart = GetCartFromDatabase(userId.Value);
                return Json(new
                {
                    success = true,
                    count = cart.Sum(c => c.Quantity),
                    total = cart.Sum(c => c.TotalPrice).ToString("N0")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cart = GetCartFromSession();
                return Json(new
                {
                    success = true,
                    count = cart.Sum(c => c.Quantity),
                    total = cart.Sum(c => c.TotalPrice).ToString("N0")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Helper - Get Count

        private int GetCartCountFromDatabase(int maTK)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetCartCount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);

                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Migration - Chuyển Session sang Database khi đăng nhập

        public void MigrateSessionCartToDatabase(int maTK)
        {
            try
            {
                var sessionCart = GetCartFromSession();
                if (sessionCart == null || sessionCart.Count == 0)
                    return;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    foreach (var item in sessionCart)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_AddToCart", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaTK", maTK);
                            cmd.Parameters.AddWithValue("@MaSP", item.ProductId);
                            cmd.Parameters.AddWithValue("@SoLuong", item.Quantity);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                Session[GetSessionKey()] = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error migrating cart: {ex.Message}");
            }
        }

        #endregion
    }
}