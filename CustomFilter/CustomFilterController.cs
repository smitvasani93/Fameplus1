using System;
using System.Web;
using System.Web.Mvc;

namespace Transactiondetails.CustomFilter
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Custom attribute for handling session timeout
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check if session is supported
            if (ctx.Session != null)
            {
                if (HttpContext.Current.Session["UserData"] == null)
                {
                    ctx.Response.Redirect("~/account/Login");
                    return;
                }

                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            filterContext.HttpContext.Response.StatusCode = 403;
                            filterContext.Result = new JsonResult { Data = "LogOut" };
                        }
                        else
                        {
                            filterContext.Result = new RedirectResult("~/account/Login");
                        }
                        //ctx.Response.Redirect("~/account/Login");
                        //new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Login" } });
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class AjaxAuthorizeAttribute : AuthorizeAttribute
    //{
    //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        // returns a 401 already
    //        base.HandleUnauthorizedRequest(filterContext);
    //        if (filterContext.HttpContext.Request.IsAjaxRequest())
    //        {
    //            // we simply have to tell mvc not to redirect to login page
    //            filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
    //        }
    //    }
    //}
}