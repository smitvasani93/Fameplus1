using GridMVCAjaxDemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Transactiondetails.Models;
using Transactiondetails.Models.Utility;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
    [Authorize]
    public class JobWorkReceiptsController : Controller
    {
        private IGridMvcHelper gridMvcHelper;
        private const string GRID_PARTIAL_PATH = "~/Views/Home/JobworkReceipt.cshtml";

        public JobWorkReceiptsController()
        {
            this.gridMvcHelper = new GridMvcHelper();
        }

        public ActionResult JobworkReceipt()
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkReceipt";

            try
            {
                var userData = (UserData)Session["UserData"];
                var jobReciept = jobReceiptDataLayer.GetJobReciept(userData.Company, userData.Company, userData.FYear);
                var accounts = jobReceiptDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
                var process = dbutility.GetProcesses();
                var recieptNo = jobReciept.JobRecieptMasts.FirstOrDefault().MaxSerialNumber;
                recieptNo++;
                TempData["recieptNo"] = recieptNo;
                ViewBag.Search = jobReciept.JobRecieptMasts.OrderByDescending(x => x.ReferenceDate);
                ViewBag.Process = process;
                ViewBag.Customer = accounts;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }


            //var jobReciept = dbutility.GetJobReciept(userData.Company, userData.FYear);
            //var recieptNo = jobReciept.JobReciepts.Max(a => a.SerialNumber);
            //recieptNo++;
            //TempData["recieptNo"] = recieptNo;
            //var data = jobReciept.JobReciepts.Select(sel => new JobReciptVM
            //{
            //    SerialNumber = sel.SerialNumber,
            //    AccountCode = sel.AccountCode,
            //    AccountName = sel.AccountName,
            //    ReferenceDate = sel.ReferenceDate
            //});
            //ViewBag.Search = jobReciept.JobReciepts;
            //ViewBag.Process = jobReciept.Processes;
            //ViewBag.Customer = jobReciept.Accounts;
            //var model = jobReciept.JobReciepts.Select(sel => new JobReciptVM
            //{
            //    AccountCode = sel.AccountCode,
            //    AccountName = sel.AccountName,
            //    ReferenceDate = sel.ReferenceDate,
            //    SerialNumber = sel.SerialNumber
            //});
            //return View(data);


            return View();
        }

        public JsonResult SaveJobworkReceipt(List<JobReceiptDet> lstjobReceiptDets, string referenceDate = "", string accountCode = "", int serialNumber = 0, bool isEdit = false)
        {
            var dbutility = new JobReceiptDataLayer();
            var message = new { message = "Failed", error = "True" };
            DatabaseResponse databaseResponse = null;
            try
            {
                var userData = (UserData)Session["UserData"];

                var jobRecieptMas = new JobReceiptMa();
                //put your Fields Here
                jobRecieptMas.SerialNumber = serialNumber;
                jobRecieptMas.AccountCode = accountCode;
                jobRecieptMas.ReferenceDate = Convert.ToDateTime(referenceDate);
                //jobRecieptMas.EntryDate = DateTime.Now;
                jobRecieptMas.ModiDate = DateTime.Now;
                jobRecieptMas.FinancialYearCode = userData.FYear;
                jobRecieptMas.BranchCode = userData.Branch;
                jobRecieptMas.UserCode = 1; //Get usercode from session
                jobRecieptMas.ModiUserCode = 1; //Get usercode from session
                jobRecieptMas.DeletedFlag = false;

                var jobRecipt = new JobReceipt();
                jobRecipt.JobReceiptMaster = jobRecieptMas;
                jobRecipt.JobReceiptDetails = lstjobReceiptDets;

                if (isEdit) //Edit 
                {
                    //   var serialNo = Convert.ToInt32(TempData["serialNo"]);
                    databaseResponse = dbutility.UpdateJobworkReceipt(jobRecipt, userData.Company, userData.FYear);
                }
                else  //Add
                {
                    databaseResponse = dbutility.SaveJobworkReceipt(jobRecipt, userData.Company, userData.FYear);
                }

                if (databaseResponse != null)
                {
                    if (databaseResponse.ErrorCode == "00")
                    {
                        message = new { message = "Success", error = "false" };
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        message = new { message = "Failed", error = "True" };
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult GetJobworkReciptBySerialNo(int id)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();

            //try
            //{
            var userData = (UserData)Session["UserData"];

            var jobReciept = jobReceiptDataLayer.GetJobRecieptBySerialNumber(userData.Company, userData.FYear, id);
            var process = dbutility.GetProcesses();
            var accounts = jobReceiptDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);

            TempData["AccountCode"] = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
            TempData["ReferenceDate"] = Convert.ToDateTime(jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");  //Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");

            ViewBag.Process = process;
            ViewBag.Customer = accounts;
            TempData["serialNo"] = id;
            TempData.Keep("serialNo");
            return PartialView(jobReciept.JobRecieptDets);

            //var jobrecipt=  new JobReciptVM
            //  {
            //      AccountCode = sel.AccountCode,
            //      AccountName = sel.AccountName,
            //      ReferenceDate = sel.ReferenceDate,
            //      SerialNumber = sel.SerialNumber
            //  }

            //  //  var filterJobReciepts = jobReciept.JobReciepts.Where(filter => filter.SerialNumber == id);

            //      //var jobReciept1 = jobReciept.JobRecieptMasts.Where(filter => filter.SerialNumber == id).Select(sel => new JobReciptVM
            //      //{
            //      //    AccountCode = sel.AccountCode,
            //      //    AccountName = sel.AccountName,
            //      //    ReferenceDate = sel.ReferenceDate,
            //      //    SerialNumber = sel.SerialNumber
            //      //}).FirstOrDefault();

            //      //TempData["AccountCode"] = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
            //      //TempData["ReferenceDate"] = Convert.ToDateTime(jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");  //Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");

            //      ViewBag.Process = jobReciept.Processes;
            //  ViewBag.Customer = jobReciept.Accounts;

            //  jobReciept1.Processes = jobReciept.Processes.Select(sel => new ProcessMasterVM
            //  {
            //      ProcessCode = sel.ProcessCode,
            //      ProcessName = sel.ProcessName
            //  });

            //  jobReciept1.Accounts = jobReciept.Accounts.Select(sel => new AccountMasterVM
            //  {
            //      AccountCode = sel.AccountCode,
            //      AccountName = sel.AccountName
            //  });

            //TempData["serialNo"] = serialNo;
            //TempData.Keep("serialNo");
            //return PartialView(jobReciept1);


            return PartialView("_JobworkReceiptPartial");
        }
    }
}