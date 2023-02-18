using System.Web.Mvc;
using System.Web;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System;

namespace Transactiondetails.Controllers
{
    public class AccountController : Controller
    {
        //GET: Account
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            Session["UserName"] = userName;
            //return Content(userName);
            return RedirectToAction("Index", "Home");
        }


        //public ActionResult LogOut()
        //{

        //    //PurgeSessionInfo();
        //    //var requestContext = HttpContext.Current.Request.RequestContext;
        //    return new System.Web.Mvc.UrlHelper(requestContext).Action("Login", "Account");
        //}

        //public void PurgeSessionInfo()
        //{
        //    var context = HttpContext.Current;
        //    context.Session.Abandon();
        //    context.Session.Clear();
        //    context.Session.RemoveAll();

        //    context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);

        //    context.Request.Cookies.Remove("ASP.NET_SessionId");
        //    //In the response pass an empty/null SessionId with backdated expiry date
        //    var ASPNET_SessionIdcookie = new HttpCookie("ASP.NET_SessionId", "");
        //    ASPNET_SessionIdcookie.Expires = DateTime.Now.AddDays(-1);
        //    context.Response.Cookies.Add(ASPNET_SessionIdcookie);
        //}
    }
    

    //public class LoginModel
    //{
    //    [Required]
    //    [Display(Name = "User name")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }
    //}
}