using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopFlower.Models
{
    public class Cart
    {
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => Price * Quantity;
    

        public Cart(string productId)
        {
                ProductId = productId;
                var product = db.SANPHAMs.SingleOrDefault(p => p.MaSP == productId);
                ProductName = product.TenSP;
                ProductImage = product.AnhSP;
                Price = product.GiaBan??0;
                Quantity = 1;
        }
    }
}