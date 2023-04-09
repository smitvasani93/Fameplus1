using GridMVCAjaxDemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Transactiondetails.CustomFilter;
using Transactiondetails.Models;
using Transactiondetails.Models.Utility;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
    [SessionExpireFilter]
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
                var jobReceiept = jobReceiptDataLayer.GetJobReciept(userData.Company, userData.Company, userData.FYear);
                var accounts = jobReceiptDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
                var process = dbutility.GetProcesses();
                var recieptNo = jobReceiept.JobRecieptMasts.FirstOrDefault().MaxSerialNumber;
                recieptNo++;
                TempData["recieptNo"] = recieptNo;
                ViewBag.Search = jobReceiept.JobRecieptMasts.OrderByDescending(x => x.ReferenceDate);
                ViewBag.Process = process;
                ViewBag.Customer = accounts;

                var data = jobReceiept.JobRecieptMasts.Select(sel => new JobReciptVM
                {
                    SerialNumber = sel.SerialNumber,
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName,
                    ReferenceDate = sel.ReferenceDate
                });

                return View(data);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return View();
        }

        public ActionResult GetJobworkReciptBySerialNo(int id)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();

            try
            {
                var userData = (UserData)Session["UserData"];

                var jobReciept = jobReceiptDataLayer.GetJobRecieptBySerialNumber(userData.Company, userData.FYear, id);
                var process = dbutility.GetProcesses();
                var accounts = jobReceiptDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
                var jobReceiptVM = new JobReciptVM();

                //TempData["AccountCode"] = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
                //TempData["ReferenceDate"] = Convert.ToDateTime(jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");  //Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");

                jobReceiptVM.AccountCode = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
                jobReceiptVM.ReferenceDate = jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate;
                jobReceiptVM.SerialNumber = id;

                //ViewBag.Process = process;
                //ViewBag.Customer = accounts;
                //TempData["serialNo"] = id;
                //TempData.Keep("serialNo");

                jobReceiptVM.JobReceiptDetails = jobReciept.JobRecieptDets.Select(sel => new JobReceiptDetailVM
                {
                    ProcessCode = sel.ProcessCode,
                    ItemCarats = sel.ItemCarats,
                    ItemLines = sel.ItemLines,
                    ItemPieces = sel.ItemPieces,
                    PacketNumber = sel.PacketNumber,
                    ItemSerialNumber = sel.ItemSerialNumber,
                    Remarks = sel.Remarks
                }).ToList();

                jobReceiptVM.Processes = process.Select(sel => new ProcessMasterVM
                {
                    ProcessCode = sel.ProcessCode,
                    ProcessName = sel.ProcessName
                });

                jobReceiptVM.Accounts = accounts.Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });
                return PartialView("_JobworkReceiptPartial", jobReceiptVM);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            //return PartialView("_JobworkReceiptPartial");
        }

        public JsonResult SaveJobworkReceipt(JobReciptVM model)
        {
            var dbutility = new JobReceiptDataLayer();
            var message = new { message = "Failed", error = "True" };
            DatabaseResponse databaseResponse = null;
            try
            {
                var userData = (UserData)Session["UserData"];
                var jobRecieptMas = new JobReceiptMa();
                //put your Fields Here
                jobRecieptMas.SerialNumber = model.SerialNumber;
                jobRecieptMas.AccountCode = model.AccountCode;
                jobRecieptMas.ReferenceDate = model.ReferenceDate;
                //jobRecieptMas.EntryDate = DateTime.Now;
                jobRecieptMas.ModiDate = DateTime.Now;
                jobRecieptMas.FinancialYearCode = userData.FYear;
                jobRecieptMas.BranchCode = userData.Branch;
                jobRecieptMas.UserCode = 1; //Get usercode from session
                jobRecieptMas.ModiUserCode = 1; //Get usercode from session

                var jobRecipt = new JobReceipt();
                jobRecipt.JobReceiptMaster = jobRecieptMas;
                jobRecipt.JobReceiptDetails = model.JobReceiptDetails.Select(sel => new JobReceiptDet
                {
                    ItemCarats = sel.ItemCarats,
                    PacketNumber = sel.PacketNumber,
                    ItemSerialNumber = sel.ItemSerialNumber,
                    ItemLines = sel.ItemLines,
                    SerialNumber = model.SerialNumber,
                    ItemPieces = sel.ItemPieces,
                    Remarks = sel.Remarks
                }).ToList();

                databaseResponse = dbutility.UpdateJobworkReceipt(jobRecipt, userData.Company, userData.FYear);
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

    }
}