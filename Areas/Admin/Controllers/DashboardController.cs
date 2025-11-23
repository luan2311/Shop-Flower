using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        // GET: Admin/Dashboard
        [OverrideAuthorization]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Index()
        {
            var username = User?.Identity?.Name;
            TAIKHOAN model = null;
            if (!string.IsNullOrEmpty(username))
            {
                model = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == username);
            }
            return View(model);
        }
        public ActionResult SomeAdminOnlyAction()
        {
            // admin-only logic
            return View();
        }
    }
}
