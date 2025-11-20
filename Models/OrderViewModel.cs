using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopFlower.Models
{
    public class OrderViewModel
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}