using System.Web.Mvc;
using System.Web.Security;

namespace Transactiondetails.Controllers
{
    public class AccountController : Controller
    {
        //GET: Account
        //public ActionResult Index()
        //{
        //    return View();
        //}


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            FormsAuthentication.SetAuthCookie(userName,false);
            return RedirectToAction("CompanyDetails", "Account");
        }

        public ActionResult CompanyDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CompanyDetails(string CompanyName, string FinancialYear, string Branch)
        {


            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        } 
    }



}