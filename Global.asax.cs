using System.Web.Mvc;
using System.Web.Routing;
using Transactiondetails.Models.Utility;

namespace Transactiondetails
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Load data to application cache
            LoadAplicatioCache();
        }

        private void LoadAplicatioCache()
        {
            var dbutility = new DBUtility();
            Application["Process"] =  dbutility.GetProcesses();
           // Application["Accounts"] = dbutility.GetAccounts();
        }
    }
}
