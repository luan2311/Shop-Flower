using System;
using System.Collections.Generic;

namespace ShopFlower.Models
{
    public class OrderViewModel
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
    }

    public class OrderDetailViewModel
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string AnhSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }
}