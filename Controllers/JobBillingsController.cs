using GridMVCAjaxDemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Transactiondetails.CustomFilter;
using Transactiondetails.DBModels;
using Transactiondetails.Models;
using Transactiondetails.Models.Utility;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
    [SessionExpireFilter]
    [Authorize]
    public class JobBillingsController : Controller
    {
        private IGridMvcHelper gridMvcHelper;
        private const string GRID_PARTIAL_PATH = "~/Views/Home/JobworkReceipt.cshtml";

        public JobBillingsController()
        {
            this.gridMvcHelper = new GridMvcHelper();
        }

        public ActionResult GetPendingJobDespatch(string accountcode)
        {
            //string accountcode = "A0000101";
            var userData = (UserData)Session["UserData"];

            var jobBillingDataLayer = new JobBillingDataLayer();
            var accountDataLayer = new AccountDataLayer();
            var account = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear).FirstOrDefault(x => x.AccountCode == accountcode);
            


            var pendingJobDespatches = jobBillingDataLayer.GetPendingJobDespatch(accountcode, userData.Company, userData.Branch, userData.FYear)
                                     .Select(x => new JobBillingDetailViewModel
                                     {
                                         SerialNumber = x.SerialNumber,
                                         CustomrName = account.AccountName,
                                         ItemSerialNumber = x.ItemSerialNumber,
                                         JDItemSerialNumber = x.JDItemSerialNumber,
                                         //ReferenceDate = x.ReferenceDate,
                                         JDReferenceDate = x.JDReferenceDate,
                                         JRReferenceDate = x.JRReferenceDate,
                                         ProcessName = x.ProcessName,
                                         ProcessCode = x.ProcessCode,
                                         PacketNumber = x.PacketNumber,
                                         BillingType = x.BillingType,
                                         ItemCarats = x.ItemCarats,
                                         ItemLines = x.ItemLines,
                                         ItemPieces = x.ItemPieces,
                                         JDItemCarats = x.JDItemCarats,
                                         JDItemLines = x.JDItemLines,
                                         JDItemPieces = x.JDItemPieces
                                     });

            return Json(pendingJobDespatches, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobBillingBySerialNo(int id)
        {
            var jobBillingDataLayer = new JobBillingDataLayer();
            var accountDataLayer = new AccountDataLayer();

            try
            {
                var userData = (UserData)Session["UserData"];

                var jobBill = jobBillingDataLayer.GetJobBillBySerialNo(userData.Company, userData.FYear, id);
                var jobDespatchViewModel = new JobBillingDetailViewModel();

                //jobDespatchViewModel.AccountCode = jobBill.JobBillingDets.FirstOrDefault().AccountCode;
                //jobDespatchViewModel.ReferenceDate = jobBill.JobBillingDets.FirstOrDefault().ReferenceDate;
                //jobDespatchViewModel.SerialNumber = id;

                //jobDespatchViewModel.JobDespatchDetails = jobBill.JobBillingDets.Select(x => new JobDespatchDetailViewModel
                //{
                //    ReferenceDate = x.ReferenceDate.Value,
                //  //  WeightLoss = x.WeightLoss,
                //    //JRSerialNumber = x.JRSerialNumber,
                //    //JRItemSerialNumber = x.JRItemSerialNumber,
                //    ItemPieces = x.ItemPieces,
                //    ItemLines = x.ItemLines,
                //    ItemCarats = x.ItemCarats,
                //    //NoChargeQuantity = x.NoChargeQuantity,
                //    //BillingQuantity = x.BillingQuantity,
                //    //Rate = x.BillingRate,
                //    ItemSerialNumber = x.ItemSerialNumber,
                //    Remarks = x.Remarks,
                //    //Status = x.PacketStatus,
                //    //ProcessCode = x.ProcessCode,
                //    //ProcessName = x.ProcessName,
                //    //BillingType = x.BillingType,
                //    //BillingUnit = x.BillingType

                //}).ToList();


                //var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);

                //jobDespatchViewModel.Accounts = accounts.Select(sel => new AccountMasterVM
                //{
                //    AccountCode = sel.AccountCode,
                //    AccountName = sel.AccountName
                //});

                //jobDespatchViewModel.Mode = Mode.Update;

                return PartialView("_JobworkDespatchUpdatePartial", jobDespatchViewModel);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult JobworkBilling()
        {
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobBilling";
            return View(Enumerable.Empty<JobBillingViewModel>());
        }

        public ActionResult JobworkReceiptDtTable()
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();

            var accountDataLayer = new AccountDataLayer();

            try
            {
                var userData = (UserData)Session["UserData"];
                var jobReceiept = jobReceiptDataLayer.GetJobReciept(userData.Company, userData.Company, userData.FYear);
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
                var process = dbutility.GetProcesses();
                var recieptNo = jobReceiept.JobRecieptMasts.FirstOrDefault().MaxSerialNumber;
                recieptNo++;
                var data = jobReceiept.JobRecieptMasts.Select(sel => new JobReciptVM
                {
                    SerialNumber = sel.SerialNumber,
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName,
                    ReferenceDate = sel.ReferenceDate
                }).OrderByDescending(x => x.SerialNumber);

                return View("~/Views/JobWorkReceipts/JobworkReceiptDtTable.cshtml", data);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return View();
        }


        [HttpGet]
        public JsonResult JobBillinghDataTable(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString, string filters, string id)
        {
            var jobBillingDataLayer = new JobBillingDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();

            try
            {
                sord = (sord == null) ? "" : sord;
                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;

                var userData = (UserData)Session["UserData"];
                var jobBillings = jobBillingDataLayer.GetJobBilling(userData.Company, userData.Branch, userData.FYear);
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Branch, userData.FYear);
                var process = dbutility.GetProcesses();
                var recieptNo = jobBillings.FirstOrDefault().MaxSerialNumber;
                recieptNo++;
                var data = jobBillings.Select(sel => new JobReciptVM
                {
                    SerialNumber = sel.SerialNumber,
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName,
                    ReferenceDate = sel.ReferenceDate
                }).OrderByDescending(x => x.SerialNumber).ToList();

                if (_search)
                {

                    var serializer = new JavaScriptSerializer();

                    Filters f = (!_search || string.IsNullOrEmpty(filters)) ? null : serializer.Deserialize<Filters>(filters);
                    //ObjectQuery<Question> filteredQuery =
                    //    (f == null ? context.Questions : f.FilterObjectSet(data));
                    //filteredQuery.MergeOption = MergeOption.NoTracking; // we don't want to update the data
                    //var totalRecords = filteredQuery.Count();

                    var jobRecs = f.rules
                        .Select(x => new JobReciptVM
                        {
                            AccountName = x.field == "AccountName" ? x.data : string.Empty,
                            ReferenceDate = x.field == "ReferenceDate" ? (string.IsNullOrEmpty(x.data) ? (DateTime?)null : Convert.ToDateTime(x.data)) : (DateTime?)null
                        });

                    foreach (var jr in jobRecs)
                    {
                        if (!string.IsNullOrEmpty(jr.AccountName))
                        {
                            data = data.Where(x => x.AccountName.ToLower().Contains(jr.AccountName.ToLower())).ToList();
                        }

                        if (jr.ReferenceDate.HasValue)
                        {
                            data = data.Where(x => x.ReferenceDate == jr.ReferenceDate).ToList();
                        }
                    }


                    //switch (searchField)
                    //{
                    //    case "AccountName":
                    //        data = data.Where(x => x.AccountName.Contains(searchString)).ToList();
                    //        break;
                    //    case "ReferenceDate":
                    //        var dt = Convert.ToDateTime(searchString);
                    //        data = data.Where(x => x.ReferenceDate == dt).ToList();
                    //        break;
                    //}
                }
                /*
               
                */
                switch (sidx)
                {
                    case "AccountCode":
                        {
                            if (sord == "desc")
                            {
                                data = data.OrderByDescending(x => x.AccountCode).ToList();
                            }
                            else
                            {
                                data = data.OrderBy(x => x.AccountCode).ToList();
                            }
                        }

                        break;
                    case "AccountName":
                        {
                            if (sord == "desc")
                            {
                                data = data.OrderByDescending(x => x.AccountName).ToList();
                            }
                            else
                            {
                                data = data.OrderBy(x => x.AccountName).ToList();
                            }
                        }

                        break;
                    case "SerialNumber":
                        {
                            if (sord == "desc")
                            {
                                data = data.OrderByDescending(x => x.SerialNumber).ToList();
                            }
                            else
                            {
                                data = data.OrderBy(x => x.SerialNumber).ToList();
                            }
                        }
                        break;
                    case "ReferenceDate":
                        {
                            if (sord == "desc")
                            {
                                data = data.OrderByDescending(x => x.ReferenceDate).ToList();
                            }
                            else
                            {
                                data = data.OrderBy(x => x.ReferenceDate).ToList();
                            }
                        }
                        break;
                }
                int totalRecords = data.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                data = data.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = data
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return Json(new { }, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult SaveJobworkDespatch(JobDespatchViewModel model)
        {
            var dbutility = new JobDespatchDataLayer();
            var message = new { message = "Failed", error = "True" };
            DatabaseResponse databaseResponse = null;
            try
            {
                var userData = (UserData)Session["UserData"];
                var jobDespatchMaster = new JobDespatchMaster();
                //put your Fields Here
                jobDespatchMaster.SerialNumber = model.SerialNumber.Value;
                jobDespatchMaster.AccountCode = model.AccountCode;
                jobDespatchMaster.ReferenceDate = model.ReferenceDate.Value;
                jobDespatchMaster.EntryDate = DateTime.Now;
                jobDespatchMaster.ModiDate = DateTime.Now;
                jobDespatchMaster.FinancialYearCode = userData.FYear;
                jobDespatchMaster.BranchCode = userData.Branch;
                jobDespatchMaster.UserCode = userData.UserId; //Get usercode from session
                jobDespatchMaster.ModiUserCode = userData.UserId; //Get usercode from session

                var jobDespatch = new JobDespatch();
                jobDespatch.JobDespatchMaster = jobDespatchMaster;
                jobDespatch.JobDespatchDetails = model.JobDespatchDetails.Select(sel => new JobDespatchDetail
                {
                    SerialNumber = model.SerialNumber.Value,
                    ItemSerialNumber = sel.ItemSerialNumber,
                    JRSerialNumber = sel.JRSerialNumber,
                    JRItemSerialNumber = sel.JRItemSerialNumber,
                    ItemCarats = sel.BalItemCarats,
                    ItemLines = sel.BalItemLines,
                    ItemPieces = sel.BalItemPieces,
                    BillingQuantity = sel.BillingQuantity,
                    BillingRate = sel.Rate,
                    NoChargeQuantity = sel.NoChargeQuantity,
                    WeightLoss = sel.WeightLoss,
                    PacketStatus = sel.Status.ToLower() == "yes" ? "y" : "n",
                    Remarks = sel.Remarks,
                }).ToList();

                if (model.Mode == Mode.Add)
                {
                    databaseResponse = dbutility.SaveJobDespatch(jobDespatch, userData.Company, userData.FYear);
                }
                else if (model.Mode == Mode.Update)
                {
                    databaseResponse = dbutility.UpdateJobDespatch(jobDespatch, userData.Company, userData.FYear);
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

        public ActionResult GetJobBilling()
        {
            //var jobDespatchDataLayer = new JobDespatchDataLayer();
            var jobBilingDataLayer = new JobBillingDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();
            try
            {
                var userData = (UserData)Session["UserData"];
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);

                //Salesaccount
                var salesaccounts = accountDataLayer.GetSalesAccounts(userData.Company, userData.Company, userData.FYear);

                var jobBilling = jobBilingDataLayer.GetJobBilling(userData.Company, userData.Company, userData.FYear);
                var recieptNo = jobBilling.FirstOrDefault().MaxSerialNumber;
                recieptNo++;

                var jobBillingVM = new JobBillingViewModel();

                jobBillingVM.SerialNumber = recieptNo;
                jobBillingVM.ReferenceDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                jobBillingVM.PostingDate = jobBillingVM.ReferenceDate;

                jobBillingVM.Accounts = accounts.Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                jobBillingVM.SalesAccounts = salesaccounts.Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                jobBillingVM.JobBillingDetails = new List<JobBillingDetailViewModel>
                {
                    new JobBillingDetailViewModel{
                         ItemSerialNumber=1,
                         ItemLines=0,
                         PacketNumber=1,
                         ItemPieces=1,
                         ItemCarats=0
                    }
                };

                jobBillingVM.Mode = Mode.Add;
                return PartialView("_JobBillingPartial", jobBillingVM);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetRateByBillingQty(int BillingQty, short ProcessCode)
        {
            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();
            decimal rate = 0;

            try
            {
                var processdetails = dbutility.GetProcessDetails().Where(x => x.ProcessCode == ProcessCode).ToList();

                if (processdetails != null)
                {

                    var billingRate = (from process in processdetails
                                       where process.RangeFrom <= BillingQty && process.RangeTo >= BillingQty
                                       select process.BillingRate
                                 ).FirstOrDefault();

                    //var data = processdetails.Where(x => BillingQty <= x.RangeFrom && BillingQty >= x.RangeTo).FirstOrDefault();
                    //var data = processdetails.Where(x => x.RangeFrom >= BillingQty).OrderBy(y=> y.RangeFrom).FirstOrDefault();
                    //if (data != null)
                    //{
                    //    rate = data.BillingRate;
                    //}

                    rate = billingRate;
                }

                return Json(new { Rate = rate, message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult deleteDespatch(string serialNo)
        {
            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var message = new { message = "Failed", error = "True" };
            try
            {
                var userData = (UserData)Session["UserData"];
                var databaseResponse = jobDespatchDataLayer.DeleteJobDespatch(userData.Company, userData.FYear, Convert.ToInt32(serialNo));

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

                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}