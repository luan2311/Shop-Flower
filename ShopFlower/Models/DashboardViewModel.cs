using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopFlower.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int TodaysOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<HOADON> RecentOrders { get; set; }
        public List<TopSellingProduct> TopSellingProducts { get; set; }
    }

    public class TopSellingProduct
    {
        public string ProductName { get; set; }
        public int SL { get; set; } 
        public decimal TotalSold { get; set; }
    }
}