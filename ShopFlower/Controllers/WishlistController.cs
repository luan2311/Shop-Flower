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
    public class WishlistController : Controller
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
            return !string.IsNullOrEmpty(username) ? "Wish_" + username : "Wish";
        }

        private string GetConnectionString()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QL_SHOPFLOWEREntities"].ConnectionString;
            var builder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(connStr);
            return builder.ProviderConnectionString;
        }

        #endregion

        #region Get Wishlist

        public List<Wishlist> LayDanhSachYeuThich()
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return GetWishlistFromDatabase(userId.Value);
            }
            else
            {
                return GetWishlistFromSession();
            }
        }

        private List<Wishlist> GetWishlistFromDatabase(int maTK)
        {
            var wishlist = new List<Wishlist>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetWishlistItems", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                wishlist.Add(new Wishlist(
                                    maWishlist: Convert.ToInt32(reader["MaWishlist"]),
                                    productId: reader["MaSP"].ToString(),
                                    productName: reader["TenSP"].ToString(),
                                    productImage: reader["AnhSP"].ToString(),
                                    price: Convert.ToInt32(reader["GiaBan"]),
                                    ngayThem: Convert.ToDateTime(reader["NgayThem"])
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting wishlist from database: {ex.Message}");
            }

            return wishlist;
        }

        private List<Wishlist> GetWishlistFromSession()
        {
            var sessionKey = GetSessionKey();
            var wishlist = Session[sessionKey] as List<Wishlist>;

            if (wishlist == null)
            {
                wishlist = new List<Wishlist>();

                // Migrate từ legacy session nếu có
                var legacy = Session["Wishlist"] as List<Wishlist>;
                if (legacy != null && legacy.Count > 0)
                {
                    foreach (var w in legacy)
                    {
                        if (!wishlist.Any(x => x.ProductId == w.ProductId))
                        {
                            wishlist.Add(w);
                        }
                    }
                    Session["Wishlist"] = null;
                }

                Session[sessionKey] = wishlist;
            }

            return wishlist;
        }

        #endregion

        #region Add to Wishlist

        public ActionResult ThemVaoYeuThich(string ms, string strURL)
        {
            if (string.IsNullOrEmpty(ms))
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
            }

            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return ThemVaoYeuThichDatabase(userId.Value, ms, strURL);
            }
            else
            {
                return ThemVaoYeuThichSession(ms, strURL);
            }
        }

        private ActionResult ThemVaoYeuThichDatabase(int maTK, string maSP, string strURL)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_AddToWishlist", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        if (Request.IsAjaxRequest())
                        {
                            int count = GetWishlistCountFromDatabase(maTK);
                            return Json(new { success = true, count = count }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
        }

        private ActionResult ThemVaoYeuThichSession(string maSP, string strURL)
        {
            var wishlist = GetWishlistFromSession();

            if (!wishlist.Any(x => x.ProductId == maSP))
            {
                wishlist.Add(new Wishlist(maSP));
            }

            var sessionKey = GetSessionKey();
            Session[sessionKey] = wishlist;

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true, count = wishlist.Count }, JsonRequestBehavior.AllowGet);
            }

            return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
        }

        #endregion

        #region Remove from Wishlist

        public ActionResult XoaSanPhamYeuThich(string MaSP)
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return XoaSanPhamYeuThichDatabase(userId.Value, MaSP);
            }
            else
            {
                return XoaSanPhamYeuThichSession(MaSP);
            }
        }

        private ActionResult XoaSanPhamYeuThichDatabase(int maTK, string maSP)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_RemoveFromWishlist", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        var wishlist = GetWishlistFromDatabase(maTK);

                        if (Request.IsAjaxRequest())
                        {
                            return Json(new
                            {
                                success = true,
                                count = wishlist.Count,
                                isEmpty = wishlist.Count == 0
                            }, JsonRequestBehavior.AllowGet);
                        }

                        if (wishlist.Count == 0)
                        {
                            return RedirectToAction("empty_wishlist", "Wishlist");
                        }
                        return RedirectToAction("Wishlist", "Wishlist");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Wishlist", "Wishlist");
            }
        }

        private ActionResult XoaSanPhamYeuThichSession(string maSP)
        {
            var wishlist = GetWishlistFromSession();
            var item = wishlist.SingleOrDefault(s => s.ProductId == maSP);

            if (item != null)
            {
                wishlist.RemoveAll(s => s.ProductId == maSP);

                var sessionKey = GetSessionKey();
                Session[sessionKey] = wishlist;

                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        success = true,
                        count = wishlist.Count,
                        isEmpty = wishlist.Count == 0
                    }, JsonRequestBehavior.AllowGet);
                }

                if (wishlist.Count == 0)
                {
                    return RedirectToAction("empty_wishlist", "Wishlist");
                }
                return RedirectToAction("Wishlist", "Wishlist");
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Wishlist", "Wishlist");
        }

        #endregion

        #region Clear Wishlist

        public ActionResult XoaDanhSachYeuThich_All()
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_ClearWishlist", conn))
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
                var wishlist = GetWishlistFromSession();
                wishlist.Clear();
                Session[GetSessionKey()] = wishlist;
            }

            return RedirectToAction("empty_wishlist", "Wishlist");
        }

        #endregion

        #region Move to Cart

        public ActionResult ChuyenVaoGioHang(string MaSP)
        {
            // Kiểm tra sản phẩm có tồn tại và còn bán không
            var sanpham = db.SANPHAMs.FirstOrDefault(s => s.MaSP == MaSP);

            if (sanpham == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Wishlist", "Wishlist");
            }

            if (sanpham.SoLuongTon == 0)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã ngừng kinh doanh. Không thể thêm vào giỏ hàng.";
                return RedirectToAction("Wishlist", "Wishlist");
            }

            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                return ChuyenVaoGioHangDatabase(userId.Value, MaSP);
            }
            else
            {
                return ChuyenVaoGioHangSession(MaSP);
            }
        }

        private ActionResult ChuyenVaoGioHangDatabase(int maTK, string maSP)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_MoveWishlistToCart", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaTK", maTK);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng!";

                        var wishlist = GetWishlistFromDatabase(maTK);
                        if (wishlist.Count == 0)
                        {
                            return RedirectToAction("empty_wishlist", "Wishlist");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm vào giỏ hàng.";
                System.Diagnostics.Debug.WriteLine($"Error moving to cart: {ex.Message}");
            }

            return RedirectToAction("Wishlist", "Wishlist");
        }

        private ActionResult ChuyenVaoGioHangSession(string maSP)
        {
            var wishlist = GetWishlistFromSession();
            var item = wishlist.SingleOrDefault(s => s.ProductId == maSP);

            if (item != null)
            {
                var cartKey = GetCurrentUserId().HasValue ? "Cart_" + User.Identity.Name : "Cart";
                var cart = Session[cartKey] as List<Cart>;
                if (cart == null)
                {
                    cart = new List<Cart>();
                    Session[cartKey] = cart;
                }

                var cartItem = cart.Find(x => x.ProductId == maSP);
                if (cartItem == null)
                {
                    cart.Add(new Cart(maSP));
                }
                else
                {
                    cartItem.Quantity++;
                }

                wishlist.RemoveAll(s => s.ProductId == maSP);
                Session[GetSessionKey()] = wishlist;

                TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng!";

                if (wishlist.Count == 0)
                {
                    return RedirectToAction("empty_wishlist", "Wishlist");
                }
            }

            return RedirectToAction("Wishlist", "Wishlist");
        }

        #endregion

        #region Views

        public ActionResult Wishlist()
        {
            var wishlist = LayDanhSachYeuThich();
            if (wishlist == null || wishlist.Count == 0)
            {
                return RedirectToAction("empty_wishlist", "Wishlist");
            }

            ViewBag.TongSoSanPham = wishlist.Count;
            return View(wishlist);
        }

        public ActionResult empty_wishlist()
        {
            return View();
        }

        public ActionResult WishlistPartial()
        {
            var userId = GetCurrentUserId();
            int count = 0;

            if (userId.HasValue)
            {
                count = GetWishlistCountFromDatabase(userId.Value);
            }
            else
            {
                var wishlist = GetWishlistFromSession();
                count = wishlist.Count;
            }

            ViewBag.WishlistCount = count;
            return PartialView("WishlistPartial");
        }

        #endregion

        #region API

        [HttpGet]
        public ActionResult GetWishlistInfo()
        {
            var userId = GetCurrentUserId();

            if (userId.HasValue)
            {
                int count = GetWishlistCountFromDatabase(userId.Value);
                return Json(new
                {
                    success = true,
                    count = count
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var wishlist = GetWishlistFromSession();
                return Json(new
                {
                    success = true,
                    count = wishlist.Count
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Helper - Get Count

        private int GetWishlistCountFromDatabase(int maTK)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetWishlistCount", conn))
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

        private int TongSoSanPham()
        {
            var wishlist = LayDanhSachYeuThich();
            return wishlist?.Count ?? 0;
        }

        #endregion

        #region Migration - Chuyển Session sang Database khi đăng nhập

        public void MigrateSessionWishlistToDatabase(int maTK)
        {
            try
            {
                var sessionWishlist = GetWishlistFromSession();
                if (sessionWishlist == null || sessionWishlist.Count == 0)
                    return;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    foreach (var item in sessionWishlist)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_AddToWishlist", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaTK", maTK);
                            cmd.Parameters.AddWithValue("@MaSP", item.ProductId);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                Session[GetSessionKey()] = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error migrating wishlist: {ex.Message}");
            }
        }

        #endregion
    }
}