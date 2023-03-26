using System;
using System.Web.Mvc;
using System.Web.Security;
using Transactiondetails.DBModels;
using Transactiondetails.Models;
using Transactiondetails.Models.Utility;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
    public class AccountController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            //Check login
            var dbutility = new DBUtility();
            try
            {
                if (dbutility.CheckLogin(userName, password))
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    Session["UserData"] = new UserData { UserName = userName };
                    //Session["Username"] = userName;
                    return RedirectToAction("CompanyDetails", "Account");
                }
                else
                {
                    ViewBag.Message = "Username or password is incorrect";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went wrong. Please try again later.";
                return View();
            }

            //return View();
        }


        [HttpPost]
        public JsonResult ValidateUser(string userName, string password,
                               bool rememberme)
        {
            var dbutility = new DBUtility();
            LoginStatus status = new LoginStatus();
            if (dbutility.CheckLogin(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, rememberme);
                Session["UserData"] = new UserData { UserName = userName };
                status.Success = true;
                status.TargetURL = FormsAuthentication.
                                   GetRedirectUrl(userName, rememberme);
                if (string.IsNullOrEmpty(status.TargetURL))
                {
                    status.TargetURL = FormsAuthentication.DefaultUrl;
                }
                status.Message = "Login attempt successful!";
            }
            else
            {
                status.Success = false;
                status.Message = "Invalid UserID or Password!";
                status.TargetURL = FormsAuthentication.LoginUrl;
            }
            return Json(status);
        }

        public ActionResult CompanyDetails()
        {
            var vm = new CompanyViewModel();
            var dbutility = new DBUtility();
            var componyList = dbutility.CompanyList();
            vm.Companys = componyList;
            return View(vm);
        }

        public ActionResult GetFYear(string CompanyCode)
        {
            var fYear = new FYear();
            var dbutility = new DBUtility();

            try
            {
                var fYearList = dbutility.FYearList(CompanyCode);
                return Json(fYearList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return Json("", JsonRequestBehavior.AllowGet);

            //return View(fYearList);
        }

        public ActionResult GetBranch(string CompanyCode)
        {
            var fYear = new FYear();
            var dbutility = new DBUtility();
            var branchList = dbutility.BranchList(CompanyCode);

            return Json(branchList, JsonRequestBehavior.AllowGet);

            //return View(fYearList);
        }

        [HttpPost]
        public ActionResult CompanyDetails(string CompanyName, string FinancialYear, string Branch)
        {
            return RedirectToAction("CompanyDetails", "Account");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["UserData"] = "";
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}