using GridMVCAjaxDemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
            var dbutility = new JobReceiptDataLayer();

            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkReceipt";
            try
            {
                var userData = (UserData)Session["UserData"];

                var jobReciept = dbutility.GetJobReciept(userData.Company, userData.FYear);
                //var recieptNo = jobReciept.JobReciepts.Max(a => a.SerialNumber);
                //recieptNo++;
                //TempData["recieptNo"] = recieptNo;
                var data = jobReciept.JobReciepts.Select(sel => new JobReciptVM
                {
                    SerialNumber = sel.SerialNumber,
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName,
                    ReferenceDate = sel.ReferenceDate
                });
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
                return View(data);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return View();
        }

        public ActionResult UpdateJobworkRecipt(JobReciptVM jobReciptVM)
        {



            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetJobworkReciptBySerialNo(int id)
        {
            var dbutility = new JobReceiptDataLayer();

            var userData = (UserData)Session["UserData"];
            var jobReciept = dbutility.GetJobReciept(userData.Company, userData.FYear);

            //  var filterJobReciepts = jobReciept.JobReciepts.Where(filter => filter.SerialNumber == id);

            var jobReciept1 = jobReciept.JobReciepts.Where(filter => filter.SerialNumber == id).Select(sel => new JobReciptVM
            {
                AccountCode = sel.AccountCode,
                AccountName = sel.AccountName,
                ReferenceDate = sel.ReferenceDate,
                SerialNumber = sel.SerialNumber
            }).FirstOrDefault();

            //TempData["AccountCode"] = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
            //TempData["ReferenceDate"] = Convert.ToDateTime(jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");  //Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");

            ViewBag.Process = jobReciept.Processes;
            ViewBag.Customer = jobReciept.Accounts;

            jobReciept1.Processes = jobReciept.Processes.Select(sel => new ProcessMasterVM
            {
                ProcessCode = sel.ProcessCode,
                ProcessName = sel.ProcessName
            });

            jobReciept1.Accounts = jobReciept.Accounts.Select(sel => new AccountMasterVM
            {
                AccountCode = sel.AccountCode,
                AccountName = sel.AccountName
            });

            //TempData["serialNo"] = serialNo;
            //TempData.Keep("serialNo");
            //return PartialView(jobReciept1);


            return PartialView("_JobworkReceiptPartial", jobReciept1);
        }
    }
}