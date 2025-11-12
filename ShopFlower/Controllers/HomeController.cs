using ShopFlower.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();
        public ActionResult Trang_chu()
        {
            var listAllSP = db.SANPHAMs.ToList();
            return View(listAllSP);
        }

        
    }
}