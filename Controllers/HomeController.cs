using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Transactiondetails.Models;
using Transactiondetails.Models.Utility;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
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
            userData.FYearName= companyViewModel.FYearName;
            userData.BranchName = companyViewModel.BranchName;

           Session["UserData"] = userData;

            return View(companyViewModel);
        }


        #region JobworkReceipt
        public ActionResult JobworkReceipt()
        {
            ViewBag.Menu = "JobworkReceipt";
            //var dbutility = new DBUtility();

            //try
            //{
            //    var fYearList = dbutility.FYearList(CompanyCode);
            //    return Json(fYearList, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    string message = ex.Message;
            //}


            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                var recieptNo = db.JobReceiptMas.Max(a => a.SerialNumber);
                recieptNo++;
                TempData["recieptNo"] = recieptNo;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
                List<sp_GetSearchData_Result> lstGetSearchData = new List<sp_GetSearchData_Result>();
                lstGetSearchData = db.sp_GetSearchData().ToList();
                ViewBag.Search = lstGetSearchData;
                ViewBag.Process = processList;
                ViewBag.Customer = customerList;
                return View();
            }
        }
        [HttpPost]
        public PartialViewResult EditJobworkReceipt(int serialNo)
        {
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                List<sp_GetSelectedData_Result> lstSelectedData = new List<sp_GetSelectedData_Result>();
                lstSelectedData = db.sp_GetSelectedData(serialNo).ToList();
                ViewBag.Data = lstSelectedData;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
                TempData["AccountCode"] = db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().AccountCode;
                TempData["ReferenceDate"] = Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");
                ViewBag.Process = processList;
                ViewBag.Customer = customerList;
                TempData["serialNo"] = serialNo;
                TempData.Keep("serialNo");
                return PartialView(lstSelectedData);
            }
        }
        public JsonResult SaveJobworkReceipt(List<JobReceiptDet> lstjobReceiptDets, string referenceDate = "", string accountCode = "", int serialNumber = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(referenceDate) && !string.IsNullOrEmpty(accountCode) && serialNumber > 0)
                {
                    var jobRecieptMas = new JobReceiptMa();
                    //put your Fields Here
                    jobRecieptMas.SerialNumber = serialNumber;
                    jobRecieptMas.AccountCode = accountCode;
                    jobRecieptMas.ReferenceDate = Convert.ToDateTime(referenceDate);
                    jobRecieptMas.EntryDate = DateTime.Now;
                    jobRecieptMas.ModiDate = DateTime.Now;

                    var jobRecipt = new JobReceipt();
                    jobRecipt.JobReceiptMaster = jobRecieptMas;
                    jobRecipt.JobReceiptDetails = lstjobReceiptDets;


                    var xmlString = XmlUtility.Serialize(jobRecipt);

                    var pxmlString = new SqlParameter("@xmlString", xmlString);

                    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
                    {
                        //Call Stored Procedure to dump the xml to database
                        var data = db.Database.SqlQuery<DatabaseResponse>("exec spJobReceiptAdd @xmlString", pxmlString);

                    
                    if (data != null)
                        {
                            if (data.First().ErrorCode == "00")
                            {
                                var successMessage = new { message = "Success", error = "false" };
                                return Json(successMessage, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var failedMessage = new { message = "Failed", error = "True" };
                                return Json(failedMessage, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
                else if (lstjobReceiptDets != null && !string.IsNullOrEmpty(TempData["serialNo"].ToString()))
                {
                    using (TransactionDetailsEntities db = new TransactionDetailsEntities())
                    {
                        var serialNo = Convert.ToInt32(TempData["serialNo"]);
                        JobReceiptMa jobRecieptMasData = db.JobReceiptMas.Find(serialNo);
                        jobRecieptMasData.ModiDate = DateTime.Now;
                        List<JobReceiptDet> lstjobReceipt = db.JobReceiptDets.Where(a => a.SerialNumber == serialNo).ToList();
                        foreach (var item in lstjobReceiptDets)
                        {
                            foreach (var data in lstjobReceipt)
                            {
                                if (item.ItemSerialNumber == data.ItemSerialNumber)
                                {
                                    data.ProcessCode = item.ProcessCode;
                                    data.PacketNumber = item.PacketNumber;
                                    data.ItemPieces = item.ItemPieces;
                                    data.ItemCarats = item.ItemCarats;
                                    data.ItemLines = item.ItemLines;
                                    data.Remarks = item.Remarks;
                                }
                            }
                        }
                        lstjobReceiptDets = lstjobReceiptDets.Where(a => a.ItemSerialNumber == 0).ToList();
                        if (lstjobReceiptDets.Count > 0)
                        {
                            int ItemSerialNumber = lstjobReceipt.Max(x => x.ItemSerialNumber);
                            foreach (var item in lstjobReceiptDets)
                            {
                                ItemSerialNumber++;
                                item.ItemSerialNumber = (short)ItemSerialNumber;
                                item.SerialNumber = serialNo;
                                db.JobReceiptDets.Add(item);
                            }
                        }
                        db.SaveChanges();
                    }
                }
                var json2 = new { message = "Success", error = "false" };
                return Json(json2, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var json2 = new { message = "Exception occured", error = "True" };
                return Json(json2, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion



        #region JobWorkDespatch
        public ActionResult JobWorkDespatch()
        {
            ViewBag.Menu = "JobWorkDespatch";
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                List<sp_GetCustomers_Result> lstGetCustomerData = new List<sp_GetCustomers_Result>();
                lstGetCustomerData = db.sp_GetCustomers().ToList();
                ViewBag.Search = lstGetCustomerData;
                var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
                ViewBag.Customer = customerList;
                int serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
                TempData["serialNo"] = serialNo;
                return View();
            }
        }
        [HttpPost]
        public PartialViewResult EditJobWorkDespatch(string accountNo)
        {
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                int serialNo;
                if (db.JobDespatchMas.Any(a => a.AccountCode == accountNo))
                    serialNo = db.JobDespatchMas.Where(a => a.AccountCode == accountNo).FirstOrDefault().SerialNumber;
                else
                    serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
                List<sp_GetDespatchData_Result> lstSelectedData = new List<sp_GetDespatchData_Result>();
                lstSelectedData = db.sp_GetDespatchData(accountNo).ToList();
                ViewBag.Data = lstSelectedData;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
                TempData["serialNo"] = serialNo;
                TempData["ReferenceDate"] = Convert.ToDateTime(db.JobReceiptMas.Where(x => x.AccountCode == accountNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");
                ViewBag.Process = processList;
                ViewBag.Customer = customerList;
                TempData["Customer"] = db.Accounts.Where(a => a.AccountCode == accountNo).FirstOrDefault().AccountCode;
                ViewBag.serialNo = lstSelectedData.Select(a => a.SerialNumber).Distinct().ToList();
                return PartialView(lstSelectedData);
            }
        }
        [HttpPost]
        public PartialViewResult SelectWorkDespatch(string serialNos = "")
        {
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                List<sp_GetSelectedDespatchData_Result> lstSelectedData = new List<sp_GetSelectedDespatchData_Result>();
                lstSelectedData = db.sp_GetSelectedDespatchData(serialNos).ToList();
                ViewBag.Data = lstSelectedData;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                ViewBag.Process = processList;
                return PartialView(lstSelectedData);
            }
        }
        [HttpPost]
        public PartialViewResult EditJobWorkDespatchNew(string accountNo)
        {
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                //int  serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
                List<sp_GetNewDespatchData_Result> lstSelectedData = new List<sp_GetNewDespatchData_Result>();
                lstSelectedData = db.sp_GetNewDespatchData(accountNo).ToList();
                ViewBag.Data = lstSelectedData;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
                //TempData["serialNo"] = serialNo;
                ViewBag.Process = processList;
                ViewBag.Customer = customerList;
                ViewBag.serialNo = lstSelectedData.Select(a => a.SerialNumber).Distinct().ToList();
                return PartialView(lstSelectedData);
            }
        }
        public JsonResult SaveJobworkDespatch(List<JobDespatchDet> lstjobDespatchDets, string referenceDate = "", string accountCode = "", int serialNumber = 0)
        {
            try
            {
                using (TransactionDetailsEntities db = new TransactionDetailsEntities())
                {
                    if (!string.IsNullOrEmpty(referenceDate) && !string.IsNullOrEmpty(accountCode) && !db.JobDespatchMas.Any(a => a.SerialNumber == serialNumber))
                    {
                        var jobDespatchMa = new JobDespatchMa();
                        //put your Fields Here
                        jobDespatchMa.SerialNumber = serialNumber;
                        jobDespatchMa.AccountCode = accountCode;
                        jobDespatchMa.ReferenceDate = Convert.ToDateTime(referenceDate);
                        jobDespatchMa.EntryDate = DateTime.Now;
                        jobDespatchMa.ModiDate = DateTime.Now;
                        db.JobDespatchMas.Add(jobDespatchMa);
                        foreach (var item in lstjobDespatchDets)
                        {
                            db.JobDespatchDets.Add(item);
                        }
                        db.SaveChanges();
                    }
                    else if (lstjobDespatchDets != null && db.JobDespatchMas.Any(a => a.SerialNumber == serialNumber))
                    {
                        JobDespatchMa jobRecieptMasData = db.JobDespatchMas.Find(serialNumber);
                        jobRecieptMasData.ModiDate = DateTime.Now;
                        List<JobDespatchDet> lstjobDespatch = db.JobDespatchDets.Where(a => a.SerialNumber == serialNumber).ToList();
                        foreach (var item in lstjobDespatchDets)
                        {
                            foreach (var data in lstjobDespatch)
                            {
                                if (item.ItemSerialNumber == data.ItemSerialNumber)
                                {
                                    data.JRSerialNumber = item.JRSerialNumber;
                                    data.JRItemSerialNumber = item.JRItemSerialNumber;
                                    data.ItemPieces = item.ItemPieces;
                                    data.ItemCarats = item.ItemCarats;
                                    data.ItemLines = item.ItemLines;
                                    data.WeightLoss = item.WeightLoss;
                                    data.PacketStatus = item.PacketStatus;
                                    data.Remarks = item.Remarks;
                                    data.BillingQuantity = item.BillingQuantity;
                                    data.NoChargeQuantity = item.NoChargeQuantity;
                                    data.BillingRate = item.BillingRate;
                                }
                            }
                        }
                        db.SaveChanges();
                    }
                }
                var json2 = new { message = "Success", error = "false" };
                return Json(json2, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var json2 = new { message = "Failed", error = "True" };
                return Json(json2, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        [HttpPost]
        public PartialViewResult EditSearchDespatch(string serialNos = "")
        {
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                List<sp_GetSelectedDespatchData_Result> lstSelectedData = new List<sp_GetSelectedDespatchData_Result>();
                lstSelectedData = db.sp_GetSelectedDespatchData(serialNos).ToList();
                ViewBag.Data = lstSelectedData;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
                //TempData["AccountCode"] = db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().AccountCode;
                //TempData["ReferenceDate"] = Convert.ToDateTime(db.JobReceiptMas.Where(x => x.SerialNumber == serialNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");
                ViewBag.Process = processList;
                ViewBag.Customer = customerList;
                //TempData["serialNo"] = serialNo;
                return PartialView(lstSelectedData);
            }
        }


        public ActionResult Test(string accountNo = "A0000901")
        {
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                int serialNo;
                if (db.JobDespatchMas.Any(a => a.AccountCode == accountNo))
                    serialNo = db.JobDespatchMas.Where(a => a.AccountCode == accountNo).FirstOrDefault().SerialNumber;
                else
                    serialNo = db.JobDespatchMas.Max(a => a.SerialNumber) + 1;
                List<sp_GetDespatchData_Result> lstSelectedData = new List<sp_GetDespatchData_Result>();
                lstSelectedData = db.sp_GetDespatchData(accountNo).ToList();
                ViewBag.Data = lstSelectedData;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                var customerList = db.Accounts.Select(a => new { a.AccountCode, a.AccountName }).ToList();
                TempData["serialNo"] = serialNo;
                TempData["ReferenceDate"] = Convert.ToDateTime(db.JobReceiptMas.Where(x => x.AccountCode == accountNo).FirstOrDefault().ReferenceDate).ToString("yyyy-MM-dd");
                ViewBag.Process = processList;
                ViewBag.Customer = customerList;
                TempData["Customer"] = db.Accounts.Where(a => a.AccountCode == accountNo).FirstOrDefault().AccountCode;
                ViewBag.serialNo = lstSelectedData.Select(a => a.SerialNumber).Distinct().ToList();
                return PartialView(lstSelectedData);
            }
        }
        [HttpPost]
        public PartialViewResult SelectWorkDespatchTest(string serialNos = "")
        {
            using (TransactionDetailsEntities db = new TransactionDetailsEntities())
            {
                List<sp_GetSelectedDespatchData_Result> lstSelectedData = new List<sp_GetSelectedDespatchData_Result>();
                //lstSelectedData = db.sp_GetSelectedDespatchData(serialNo).ToList();
                ViewBag.Data = lstSelectedData;
                var processList = db.ProcessMas.Select(a => new { a.ProcessCode, a.ProcessName }).ToList();
                ViewBag.Process = processList;
                return PartialView(lstSelectedData);
            }
        }

    }
}