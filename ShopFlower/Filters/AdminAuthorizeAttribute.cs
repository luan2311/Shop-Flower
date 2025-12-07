using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace ShopFlower.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        private const string AdminRoleName = "Admin";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) return false;
            var user = httpContext.User;
            if (user == null || !user.Identity.IsAuthenticated) return false;

            var principal = user as IPrincipal;
            if (principal != null && principal.IsInRole(AdminRoleName)) return true;

            var formsIdentity = user.Identity as System.Web.Security.FormsIdentity;
            if (formsIdentity != null)
            {
                var ticket = formsIdentity.Ticket;
                if (!string.IsNullOrEmpty(ticket.UserData))
                {
                    var roles = ticket.UserData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    return roles.Any(r => string.Equals(r.Trim(), AdminRoleName, StringComparison.OrdinalIgnoreCase));
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                        { "controller", "Account" },
                        { "action", "Dang_nhap" },
                        { "area", "" }
                    });
            }
            else
            {
                filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Unauthorized.cshtml" };
            }
        }
    }
}