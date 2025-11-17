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
        
        public List<Wishlist> LayDanhSachYeuThich()
        {
            List<Wishlist> lstWishlist = Session["Wishlist"] as List<Wishlist>;
            if (lstWishlist == null)
            {
                lstWishlist = new List<Wishlist>();
                Session["Wishlist"] = lstWishlist;
            }
            return lstWishlist;
        }

        public ActionResult ThemVaoYeuThich(string ms, string strURL)
        {
            List<Wishlist> lstWishlist = LayDanhSachYeuThich();
            Wishlist SP = lstWishlist.Find(x => x.ProductId == ms);
            if (SP == null) 
            {
                SP = new Wishlist(ms);
                lstWishlist.Add(SP);
                return Redirect(strURL);
            }
            else
            {
                return Redirect(strURL);
            }
        }

        //T?ng s? s?n ph?m yêu thích
        private int TongSoSanPham()
        {
            int tsl = 0;
            List<Wishlist> lstWishlist = Session["Wishlist"] as List<Wishlist>;
            if (lstWishlist != null)
            {
                tsl = lstWishlist.Count;
            }
            return tsl;
        }

        //Trang Danh sách yêu thích
        public ActionResult Wishlist()
        {
            if (Session["Wishlist"] == null)
            {
                return RedirectToAction("empty_wishlist", "Wishlist");
            }
            List<Wishlist> lstWishlist = LayDanhSachYeuThich();

            ViewBag.TongSoSanPham = TongSoSanPham();
            return View(lstWishlist);
        }

        public ActionResult empty_wishlist()
        {
            return View();
        }

        //Icon Wishlist + Show S? l??ng SP yêu thích
        public ActionResult WishlistPartial()
        {
            ViewBag.TongSoSanPham = TongSoSanPham();
            return PartialView();
        }

        //Xóa 1 SP trong Wishlist
        public ActionResult XoaSanPhamYeuThich(string MaSP)
        {
            List<Wishlist> lstWishlist = LayDanhSachYeuThich();
            //ki?m tra s?n ph?m c?n xóa còn trong Wishlist không
            Wishlist SP = lstWishlist.SingleOrDefault(s => s.ProductId == MaSP);
            if (SP != null) //có thì xóa
            {
                lstWishlist.RemoveAll(s => s.ProductId == MaSP);
                
                if (lstWishlist.Count == 0) //Wishlist empty
                {
                    return RedirectToAction("empty_wishlist", "Wishlist");
                }
                return RedirectToAction("Wishlist", "Wishlist");
            }
            return RedirectToAction("Wishlist", "Wishlist");
        }

        //Xóa ALL SP yêu thích
        public ActionResult XoaDanhSachYeuThich_All()
        {
            //L?y danh sách yêu thích
            List<Wishlist> lstWishlist = LayDanhSachYeuThich();
            lstWishlist.Clear();
            return RedirectToAction("empty_wishlist", "Wishlist");
        }

        //Chuy?n s?n ph?m t? Wishlist sang Cart
        public ActionResult ChuyenVaoGioHang(string MaSP)
        {
            //L?y danh sách yêu thích
            List<Wishlist> lstWishlist = LayDanhSachYeuThich();
            Wishlist SP = lstWishlist.SingleOrDefault(s => s.ProductId == MaSP);
            
            if (SP != null)
            {
                //Thêm vào gi? hàng
                List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
                if (lstGioHang == null)
                {
                    lstGioHang = new List<Cart>();
                    Session["Cart"] = lstGioHang;
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
                
                //Xóa kh?i wishlist
                lstWishlist.RemoveAll(s => s.ProductId == MaSP);
                
                if (lstWishlist.Count == 0)
                {
                    return RedirectToAction("empty_wishlist", "Wishlist");
                }
            }
            
            return RedirectToAction("Wishlist", "Wishlist");
        }
    }
}