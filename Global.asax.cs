using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Transactiondetails.Controllers;
using System.Web.Optimization;
using Transactiondetails.App_Start;

namespace Transactiondetails
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Load data to application cache
            //LoadAplicatioCache();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            //  _logger.LogError("UnhandledError", exception);
            Server.ClearError();

            var httpContext = ((MvcApplication)sender).Context;
            var controller = new ErrorController();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            Session.Abandon();
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");           //Remove Server Header   
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version Header
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            //if (Context.Items["AjaxPermissionDenied"] is bool)
            //{
            //    Context.Response.StatusCode = 401;
            //    Context.Response.End();
            //}

            var context = new HttpContextWrapper(Context);
            if (context.Request.IsAjaxRequest() && context.Response.StatusCode == 401)
            {
                new RedirectResult("~/Account/Login");
            }
        }
    }
}
