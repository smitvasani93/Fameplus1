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
                        ctx.Response.Redirect("~/account/Login");
                        //new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Login" } });
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}