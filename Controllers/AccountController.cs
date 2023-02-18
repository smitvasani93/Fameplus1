using System.Web.Mvc;
using System.Web;
using System.Web.Security;

namespace Transactiondetails.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public string LogOut()
        {
        

            var requestContext = HttpContext.Current.Request.RequestContext;
            return   new System.Web.Mvc.UrlHelper(requestContext).Action("Login", "Account");
        }
    }
}