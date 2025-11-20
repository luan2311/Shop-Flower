using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopFlower.Filters;

namespace ShopFlower.Areas.Admin.Controllers
{
    [AdminAuthorize]       
    public class BaseAdminController : Controller
    {
    }
}
