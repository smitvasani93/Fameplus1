using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Transactiondetails.CustomFilter;
using BusinessLayer.DBModels;
using BusinessLayer.Models;
using BusinessLayer.Models.Utility;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
    [SessionExpireFilter]
    [Authorize]

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(CompanyViewModel companyViewModel)
        {
            var userData = (UserData)Session["UserData"];
            userData.Branch = companyViewModel.Branch;
            userData.FYear = companyViewModel.FYear;
            userData.Company = companyViewModel.Company;
            userData.CompanyName = companyViewModel.CompanyName;
            userData.FYearName = companyViewModel.FYearName;
            userData.BranchName = companyViewModel.BranchName;
            Session["UserData"] = userData;

            return View(companyViewModel);
        }


        #region JobworkReceipt
        public ActionResult JobworkReceipt()
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkReceipt";
            try
            {
                var userData = (UserData)Session["UserData"];
                var jobReciept = jobReceiptDataLayer.GetJobReciept(userData.Company, userData.Company, userData.FYear);
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
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

            return View();
        }
        [HttpPost]
        public PartialViewResult EditJobworkReceipt(int serialNo)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();

            var accountDataLayer = new AccountDataLayer();

            //try
            //{
            var userData = (UserData)Session["UserData"];

            var jobReciept = jobReceiptDataLayer.GetJobRecieptBySerialNumber(userData.Company, userData.FYear, serialNo);
            var process = dbutility.GetProcesses();
            var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);

            TempData["AccountCode"] = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
            TempData["ReferenceDate"] = Convert.ToDateTime(jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");  //Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");

            ViewBag.Process = process;
            ViewBag.Customer = accounts;
            TempData["serialNo"] = serialNo;
            TempData.Keep("serialNo");
            return PartialView(jobReciept.JobRecieptDets);


            //}
            //catch (Exception ex)
            //{
            //}

            // return PartialView();

        }
        public JsonResult SaveJobworkReceipt(List<JobRecieptDetail> lstjobReceiptDets, string referenceDate = "", string accountCode = "", int serialNumber = 0, bool isEdit = false)
        {
            var dbutility = new JobReceiptDataLayer();
            var message = new { message = "Failed", error = "True" };
            DatabaseResponse databaseResponse = null;
            try
            {
                var userData = (UserData)Session["UserData"];

                var jobRecieptMas = new JobRecieptMaster();
                //put your Fields Here
                jobRecieptMas.SerialNumber = serialNumber;
                jobRecieptMas.AccountCode = accountCode;
                jobRecieptMas.ReferenceDate = Convert.ToDateTime(referenceDate);
                jobRecieptMas.EntryDate = DateTime.Now;
                jobRecieptMas.ModiDate = DateTime.Now;
                jobRecieptMas.FinancialYearCode = userData.FYear;
                jobRecieptMas.BranchCode = userData.Branch;
                jobRecieptMas.UserCode = userData.UserId; //Get usercode from session
                jobRecieptMas.ModiUserCode = userData.UserId; //Get usercode from session
                //jobRecieptMas.DeletedFlag = false;

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
        #endregion

        #region JobWorkDespatch
        //public ActionResult JobWorkDespatch()
        //{
        //    ViewBag.Menu = "JobWorkDespatch";
        //    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //    {
        //        List<sp_GetCustomers_Result> lstGetCustomerData = new List<sp_GetCustomers_Result>();
        //        lstGetCustomerData = db.sp_GetCustomers().ToList();
        //        ViewBag.Search = lstGetCustomerData;
        //        var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
        //        ViewBag.Customer = customerList;
        //        int serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
        //        TempData["serialNo"] = serialNo;
        //        return View();
        //    }
        //}
        //[HttpPost]
        //public PartialViewResult EditJobWorkDespatch(string accountNo)
        //{
        //    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //    {
        //        int serialNo;
        //        if (db.JobDespatchMas.Any(a => a.AccountCode == accountNo))
        //            serialNo = db.JobDespatchMas.Where(a => a.AccountCode == accountNo).FirstOrDefault().SerialNumber;
        //        else
        //            serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
        //        List<sp_GetDespatchData_Result> lstSelectedData = new List<sp_GetDespatchData_Result>();
        //        lstSelectedData = db.sp_GetDespatchData(accountNo).ToList();
        //        ViewBag.Data = lstSelectedData;
        //        var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
        //        var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
        //        TempData["serialNo"] = serialNo;
        //        TempData["ReferenceDate"] = Convert.ToDateTime(db.JobReceiptMas.Where(x => x.AccountCode == accountNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");
        //        ViewBag.Process = processList;
        //        ViewBag.Customer = customerList;
        //        TempData["Customer"] = db.Accounts.Where(a => a.AccountCode == accountNo).FirstOrDefault().AccountCode;
        //        ViewBag.serialNo = lstSelectedData.Select(a => a.SerialNumber).Distinct().ToList();
        //        return PartialView(lstSelectedData);
        //    }
        //}
        //[HttpPost]
        //public PartialViewResult SelectWorkDespatch(string serialNos = "")
        //{
        //    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //    {
        //        List<sp_GetSelectedDespatchData_Result> lstSelectedData = new List<sp_GetSelectedDespatchData_Result>();
        //        lstSelectedData = db.sp_GetSelectedDespatchData(serialNos).ToList();
        //        ViewBag.Data = lstSelectedData;
        //        var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
        //        ViewBag.Process = processList;
        //        return PartialView(lstSelectedData);
        //    }
        //}
        //[HttpPost]
        //public PartialViewResult EditJobWorkDespatchNew(string accountNo)
        //{
        //    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //    {
        //        //int  serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
        //        List<sp_GetNewDespatchData_Result> lstSelectedData = new List<sp_GetNewDespatchData_Result>();
        //        lstSelectedData = db.sp_GetNewDespatchData(accountNo).ToList();
        //        ViewBag.Data = lstSelectedData;
        //        var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
        //        var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
        //        //TempData["serialNo"] = serialNo;
        //        ViewBag.Process = processList;
        //        ViewBag.Customer = customerList;
        //        ViewBag.serialNo = lstSelectedData.Select(a => a.SerialNumber).Distinct().ToList();
        //        return PartialView(lstSelectedData);
        //    }
        //}
        //public JsonResult SaveJobworkDespatch(List<JobDespatchDet> lstjobDespatchDets, string referenceDate = "", string accountCode = "", int serialNumber = 0)
        //{
        //    try
        //    {
        //        using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //        {
        //            if (!string.IsNullOrEmpty(referenceDate) && !string.IsNullOrEmpty(accountCode) && !db.JobDespatchMas.Any(a => a.SerialNumber == serialNumber))
        //            {
        //                var jobDespatchMa = new JobDespatchMa();
        //                //put your Fields Here
        //                jobDespatchMa.SerialNumber = serialNumber;
        //                jobDespatchMa.AccountCode = accountCode;
        //                jobDespatchMa.ReferenceDate = Convert.ToDateTime(referenceDate);
        //                jobDespatchMa.EntryDate = DateTime.Now;
        //                jobDespatchMa.ModiDate = DateTime.Now;
        //                db.JobDespatchMas.Add(jobDespatchMa);
        //                foreach (var item in lstjobDespatchDets)
        //                {
        //                    db.JobDespatchDets.Add(item);
        //                }
        //                db.SaveChanges();
        //            }
        //            else if (lstjobDespatchDets != null && db.JobDespatchMas.Any(a => a.SerialNumber == serialNumber))
        //            {
        //                JobDespatchMa jobRecieptMasData = db.JobDespatchMas.Find(serialNumber);
        //                jobRecieptMasData.ModiDate = DateTime.Now;
        //                List<JobDespatchDet> lstjobDespatch = db.JobDespatchDets.Where(a => a.SerialNumber == serialNumber).ToList();
        //                foreach (var item in lstjobDespatchDets)
        //                {
        //                    foreach (var data in lstjobDespatch)
        //                    {
        //                        if (item.ItemSerialNumber == data.ItemSerialNumber)
        //                        {
        //                            data.JRSerialNumber = item.JRSerialNumber;
        //                            data.JRItemSerialNumber = item.JRItemSerialNumber;
        //                            data.ItemPieces = item.ItemPieces;
        //                            data.ItemCarats = item.ItemCarats;
        //                            data.ItemLines = item.ItemLines;
        //                            data.WeightLoss = item.WeightLoss;
        //                            data.PacketStatus = item.PacketStatus;
        //                            data.Remarks = item.Remarks;
        //                            data.BillingQuantity = item.BillingQuantity;
        //                            data.NoChargeQuantity = item.NoChargeQuantity;
        //                            data.BillingRate = item.BillingRate;
        //                        }
        //                    }
        //                }
        //                db.SaveChanges();
        //            }
        //        }
        //        var json2 = new { message = "Success", error = "false" };
        //        return Json(json2, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        var json2 = new { message = "Failed", error = "True" };
        //        return Json(json2, JsonRequestBehavior.AllowGet);
        //    }

        //}
        //#endregion

        //[HttpPost]
        //public PartialViewResult EditSearchDespatch(string serialNos = "")
        //{
        //    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //    {
        //        List<sp_GetSelectedDespatchData_Result> lstSelectedData = new List<sp_GetSelectedDespatchData_Result>();
        //        lstSelectedData = db.sp_GetSelectedDespatchData(serialNos).ToList();
        //        ViewBag.Data = lstSelectedData;
        //        var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
        //        var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
        //        //TempData["AccountCode"] = db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().AccountCode;
        //        //TempData["ReferenceDate"] = Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");
        //        ViewBag.Process = processList;
        //        ViewBag.Customer = customerList;
        //        //TempData["serialNo"] = serialNo;
        //        return PartialView(lstSelectedData);
        //    }
        //}


        //public ActionResult Test(string accountNo = "A0000901")
        //{
        //    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //    {
        //        int serialNo;
        //        if (db.JobDespatchMas.Any(a => a.AccountCode == accountNo))
        //            serialNo = db.JobDespatchMas.Where(a => a.AccountCode == accountNo).FirstOrDefault().SerialNumber;
        //        else
        //            serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
        //        List<sp_GetDespatchData_Result> lstSelectedData = new List<sp_GetDespatchData_Result>();
        //        lstSelectedData = db.sp_GetDespatchData(accountNo).ToList();
        //        ViewBag.Data = lstSelectedData;
        //        var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
        //        var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
        //        TempData["serialNo"] = serialNo;
        //        TempData["ReferenceDate"] = Convert.ToDateTime(db.JobReceiptMas.Where(x => x.AccountCode == accountNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");
        //        ViewBag.Process = processList;
        //        ViewBag.Customer = customerList;
        //        TempData["Customer"] = db.Accounts.Where(a => a.AccountCode == accountNo).FirstOrDefault().AccountCode;
        //        ViewBag.serialNo = lstSelectedData.Select(a => a.SerialNumber).Distinct().ToList();
        //        return PartialView(lstSelectedData);
        //    }
        //}
        //[HttpPost]
        //public PartialViewResult SelectWorkDespatchTest(string serialNos = "")
        //{
        //    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
        //    {
        //        List<sp_GetSelectedDespatchData_Result> lstSelectedData = new List<sp_GetSelectedDespatchData_Result>();
        //        //lstSelectedData = db.sp_GetSelectedDespatchData(serialNo).ToList();
        //        ViewBag.Data = lstSelectedData;
        //        var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
        //        ViewBag.Process = processList;
        //        return PartialView(lstSelectedData);
        //    }
        //}
        #endregion
    }
}