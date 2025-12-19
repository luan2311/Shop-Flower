using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopFlower.Models
{
    public class Wishlist
    {
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        public int? MaWishlist { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Price { get; set; }
        public DateTime? NgayThem { get; set; }

        public Wishlist() { }

        public Wishlist(string productId)
        {
            ProductId = productId;
            var product = db.SANPHAMs.SingleOrDefault(p => p.MaSP == productId);
            if (product != null)
            {
                ProductName = product.TenSP;
                ProductImage = product.AnhSP;
                Price = product.GiaBan ?? 0;
            }
        }

        public Wishlist(int maWishlist, string productId, string productName,
                       string productImage, int price, DateTime ngayThem)
        {
            MaWishlist = maWishlist;
            ProductId = productId;
            ProductName = productName;
            ProductImage = productImage;
            Price = price;
            NgayThem = ngayThem;
        }
    }
}