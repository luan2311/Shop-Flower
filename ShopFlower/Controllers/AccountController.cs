using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult Dang_nhap()
        {
            return View();
        }

        public ActionResult Dang_ky()
        {
            return View();
        }

        public ActionResult Doi_mat_khau() 
        { 
            return View();
        }
    }
}