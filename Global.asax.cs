using System.Web.Mvc;
using System.Web.Routing;

namespace Transactiondetails
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Load data to application cache
            //LoadAplicatioCache();
        }

        //private void LoadAplicatioCache()
        //{
        //    throw new FileNotFoundException();
        //    try
        //    {
        //        var dbutility = new DBUtility();
        //        dbutility.GetProcesses();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FileNotFoundException();

        //        //Response.Redirect("~/Errors/NotFound");
        //       // throw ex;
        //    }

        //}
    }
}
