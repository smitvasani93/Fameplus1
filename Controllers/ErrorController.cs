using System.Web.Mvc;

namespace Transactiondetails.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }
        public ActionResult UserNotExist()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult NotAssigned()
        {
            return View();
        }
        public ActionResult DispLocationUrls()
        {
            return View();
        }

    }
     
}