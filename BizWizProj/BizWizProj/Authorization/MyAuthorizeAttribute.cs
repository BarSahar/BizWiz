using BizWizProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace BizWizProj.Authorization
{
    class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["user"] == null)
            {
                return false;
            }
            return true;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary 
                                   {
                                       { "action", "Login" },
                                       { "controller", "Login" }
                                   });
            }
        }
    }
}