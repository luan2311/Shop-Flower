using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class WishlistController : Controller
    {
        // GET: Wishlist
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        // Centralize session key logic to match CartController pattern:
        private string GetSessionKey()
        {
            var username = User?.Identity?.Name;
            return !string.IsNullOrEmpty(username) ? "Wish_" + username : "Wish";
        }

        // Return the wishlist for current session/key. Migrate old Session["Wishlist"] if present.
        public List<Wishlist> LayDanhSachYeuThich()
        {
            var key = GetSessionKey();
            var lstWishlist = Session[key] as List<Wishlist>;
            if (lstWishlist == null)
            {
                lstWishlist = new List<Wishlist>();

                // Migrate from legacy Session["Wishlist"] if any items exist there
                var legacy = Session["Wishlist"] as List<Wishlist>;
                if (legacy != null && legacy.Count > 0)
                {
                    // Avoid duplicates when migrating
                    foreach (var w in legacy)
                    {
                        if (!lstWishlist.Any(x => x.ProductId == w.ProductId))
                        {
                            lstWishlist.Add(w);
                        }
                    }
                    // Clear legacy storage (optional)
                    Session["Wishlist"] = null;
                }

                Session[key] = lstWishlist;
            }
            return lstWishlist;
        }

        // Add item to wishlist (per-user session). Mirrors CartController behavior.
        public ActionResult ThemVaoYeuThich(string ms, string strURL)
        {
            if (string.IsNullOrEmpty(ms))
            {
                if (Request.IsAjaxRequest()) return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
            }

            var list = LayDanhSachYeuThich();

            if (!list.Any(x => x.ProductId == ms))
            {
                list.Add(new Wishlist(ms));
            }

            // Persist list back to session (LayDanhSachYeuThich already ensures storage)
            var key = GetSessionKey();
            Session[key] = list;

            if (Request.IsAjaxRequest()) return Json(new { success = true, count = list.Count }, JsonRequestBehavior.AllowGet);
            return Redirect(string.IsNullOrEmpty(strURL) ? Url.Action("Trang_chu", "Home") : strURL);
        }

        // Tổng số sản phẩm yêu thích
        private int TongSoSanPham()
        {
            var lstWishlist = LayDanhSachYeuThich();
            return lstWishlist?.Count ?? 0;
        }

        // Trang Danh sách yêu thích
        public ActionResult Wishlist()
        {
            var lstWishlist = LayDanhSachYeuThich();
            if (lstWishlist == null || lstWishlist.Count == 0)
            {
                return RedirectToAction("empty_wishlist", "Wishlist");
            }

            ViewBag.TongSoSanPham = lstWishlist.Count;
            return View(lstWishlist);
        }

        public ActionResult empty_wishlist()
        {
            return View();
        }

        // Icon Wishlist + Show Số lượng SP yêu thích (per-user)
        public ActionResult WishlistPartial()
        {
            var count = TongSoSanPham();
            ViewBag.WishlistCount = count;
            return PartialView("WishlistPartial");
        }

        // Xóa 1 SP trong Wishlist
        public ActionResult XoaSanPhamYeuThich(string MaSP)
        {
            var lstWishlist = LayDanhSachYeuThich();
            // kiểm tra sản phẩm cần xóa còn trong Wishlist không
            var SP = lstWishlist.SingleOrDefault(s => s.ProductId == MaSP);
            if (SP != null) // có thì xóa
            {
                lstWishlist.RemoveAll(s => s.ProductId == MaSP);

                var key = GetSessionKey();
                Session[key] = lstWishlist;

                if (lstWishlist.Count == 0) //Wishlist empty
                {
                    return RedirectToAction("empty_wishlist", "Wishlist");
                }
                return RedirectToAction("Wishlist", "Wishlist");
            }
            return RedirectToAction("Wishlist", "Wishlist");
        }

        // Xóa ALL SP yêu thích
        public ActionResult XoaDanhSachYeuThich_All()
        {
            var lstWishlist = LayDanhSachYeuThich();
            lstWishlist.Clear();
            var key = GetSessionKey();
            Session[key] = lstWishlist;
            return RedirectToAction("empty_wishlist", "Wishlist");
        }

        // Chuyển sản phẩm từ Wishlist sang Cart
        public ActionResult ChuyenVaoGioHang(string MaSP)
        {
            var lstWishlist = LayDanhSachYeuThich();
            var SP = lstWishlist.SingleOrDefault(s => s.ProductId == MaSP);

            if (SP != null)
            {
                // Thêm vào giỏ hàng (keep Cart behavior)
                var username = User?.Identity?.Name;
                var cartKey = !string.IsNullOrEmpty(username) ? "Cart_" + username : "Cart";
                List<Cart> lstGioHang = Session[cartKey] as List<Cart>;
                if (lstGioHang == null)
                {
                    lstGioHang = new List<Cart>();
                    Session[cartKey] = lstGioHang;
                }

                Cart cartItem = lstGioHang.Find(x => x.ProductId == MaSP);
                if (cartItem == null)
                {
                    cartItem = new Cart(MaSP);
                    lstGioHang.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }

                // Xóa khỏi wishlist
                lstWishlist.RemoveAll(s => s.ProductId == MaSP);
                var key = GetSessionKey();
                Session[key] = lstWishlist;

                if (lstWishlist.Count == 0)
                {
                    return RedirectToAction("empty_wishlist", "Wishlist");
                }
            }

            return RedirectToAction("Wishlist", "Wishlist");
        }
    }
}