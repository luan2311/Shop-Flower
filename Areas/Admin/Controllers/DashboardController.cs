using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Bảng điều khiển";
            return View();
        }
    }
}
