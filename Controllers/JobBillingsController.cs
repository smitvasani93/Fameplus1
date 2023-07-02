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
                                         //ReferenceDate = x.JDReferenceDate,
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
                                         JDItemPieces = x.JDItemPieces,
                                         BillingRate = x.BillingRate,
                                         JDBillingQuantity = x.JDBillingQuantity,
                                         BillingUnit = x.BillingType,
                                         TotalAmount = x.TotalAmount,
                                         NoChargeQuantity = x.NoChargeQuantity,
                                         DiscountAmount = 0,
                                         DiscountRate = 0,
                                         Taxes = x.Taxes,
                                         NetAmount = x.NetAmount
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
                var jobBillingViewModel = new JobBillingViewModel();

                jobBillingViewModel.AccountCode = jobBill.JobBillingMast.AccountCode;
                jobBillingViewModel.ReferenceDate = jobBill.JobBillingMast.ReferenceDate;
                jobBillingViewModel.SerialNumber = id;
                jobBillingViewModel.SalesAccountCode = jobBill.JobBillingMast.SaleAccountCode;
                jobBillingViewModel.PostingDate = jobBill.JobBillingMast.PostingDate;
                jobBillingViewModel.ReferenceNumber = id.ToString();
                //jobBillingViewModel.CashAmount = jobBill.JobBillingMast.CreditDays;
                //jobBillingViewModel.CreditDays = jobBill.JobBillingMast.CreditDays;
                jobBillingViewModel.FinalAmount = jobBill.JobBillingMast.FinalAmount;
                jobBillingViewModel.PostingAmount = jobBill.JobBillingMast.PostingAmount;
                jobBillingViewModel.PostingAmount2 = jobBill.JobBillingMast.PostingAmount2;
                jobBillingViewModel.PostingAmount3 = jobBill.JobBillingMast.PostingAmount3;
                jobBillingViewModel.RoundedAmount = jobBill.JobBillingMast.RoundedAmount;
                jobBillingViewModel.FinalAmount = jobBill.JobBillingMast.FinalAmount;

                //jobBillingViewModel.NetAmount = 0;
                jobBillingViewModel.JobBillingDetails = jobBill.JobBillingDets.Select(x => new JobBillingDetailViewModel
                {
                    ReferenceDate = x.ReferenceDate,
                    ItemPieces = x.ItemPieces,
                    ItemLines = x.ItemLines,
                    ItemCarats = x.ItemCarats,
                    ItemSerialNumber = x.ItemSerialNumber,
                    Remarks = x.Remarks,
                    ProcessCode = x.ProcessCode,
                    JDSerialNumber = x.JDSerialNumber,
                    JDItemSerialNumber = x.JDItemSerialNumber,
                    PaymentCB = x.BankCashFlag,
                    PacketNumber = x.PacketNumber,
                    Taxes = x.Addless1,
                    BillingQuantity = x.BillingQuantity,
                    NoChargeQuantity = x.NoChargeQuantity,
                    TotalAmount = x.TotalAmount,
                    NetAmount = x.NetAmount,
                    BillingRate = x.BillingRate,
                    BillingAmount = x.BillingAmount,
                    DiscountPerAmt = x.DiscountPerAmt,
                    DiscountRate = x.DiscountRate,
                    DiscountAmount =x.DiscountAmount,
                }).ToList();

                var accounts = accountDataLayer.GetBillAccounts(userData.Company, userData.Company, userData.FYear);

                jobBillingViewModel.Accounts = accounts.OrderBy(ord => ord.AccountName).Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                var salesaccounts = accountDataLayer.GetSalesAccounts(userData.Company, userData.Company, userData.FYear);

                jobBillingViewModel.SalesAccounts = salesaccounts.OrderBy(ord => ord.AccountName).Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });


                jobBillingViewModel.Mode = Mode.Update;


                return PartialView("_JobBillingUpdatePartial", jobBillingViewModel);
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
                var accounts = accountDataLayer.GetBillAccounts(userData.Company, userData.Branch, userData.FYear);
                var process = dbutility.GetProcesses();
                var recieptNo = jobBillings.FirstOrDefault().MaxSerialNumber;
                recieptNo++;
                var data = jobBillings.Select(sel => new JobReciptVM
                {
                    SerialNumber = sel.SerialNumber,
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName,
                    ReferenceDate = sel.ReferenceDate,
                    ReferenceNumber = sel.ReferenceNumber
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

        public JsonResult SaveJobBilling(JobBillingViewModel model)
        {
            var dbutility = new JobBillingDataLayer();
            var message = new { message = "Failed", error = "True" };
            DatabaseResponse databaseResponse = null;
            try
            {
                var userData = (UserData)Session["UserData"];
                var jobBillingMaster = new JobBillingMaster();
                //put your Fields Here
                jobBillingMaster.SerialNumber = model.SerialNumber.Value;
                jobBillingMaster.AccountCode = model.AccountCode;
                jobBillingMaster.ReferenceDate = model.ReferenceDate.Value;
                jobBillingMaster.PostingDate = model.PostingDate.Value;
                jobBillingMaster.EntryDate = DateTime.Now;
                jobBillingMaster.ModiDate = DateTime.Now;
                jobBillingMaster.FinancialYearCode = userData.FYear;
                jobBillingMaster.BranchCode = userData.Branch;
                jobBillingMaster.UserCode = userData.UserId; //Get usercode from session
                jobBillingMaster.ModiUserCode = userData.UserId; //Get usercode from session
                var jobBill = new JobBill();
                jobBill.JobBillingMaster = jobBillingMaster;
                jobBill.JobBillingDetails = model.JobBillingDetails.Select(sel => new JobBillingDetail
                {
                    SerialNumber = model.SerialNumber.Value,
                    ItemSerialNumber = sel.ItemSerialNumber,
                    JDSerialNumber = sel.JDSerialNumber,
                    JDItemSerialNumber = sel.JDItemSerialNumber,
                    BillingType =sel.BillingType,
                    BankCashFlag = sel.PaymentCB,
                    BillingQuantity = sel.BillingQuantity,
                    BillingAmount   = sel.BillingAmount,
                    BillingRate = sel.BillingRate,
                    NoChargeQuantity = sel.NoChargeQuantity,
                    DiscountPerAmt = sel.DiscountPerAmt,
                    DiscountAmount = sel.DiscountAmount,
                    DiscountRate = sel.DiscountRate,
                    Addless1 = sel.Taxes,
                    TotalAmount = sel.TotalAmount,
                    NetAmount = sel.NetAmount,
                    Remarks = sel.Remarks,
                }).ToList();

                jobBill.JobBillingMaster.ReferenceType = jobBill.JobBillingDetails.Any(x => x.BankCashFlag.ToLower() == "b") ? "B" : "C";

                if (model.Mode == Mode.Add)
                {
                    databaseResponse = dbutility.SaveJobBill(jobBill, userData.Company, userData.FYear);
                }
                else if (model.Mode == Mode.Update)
                {
                    databaseResponse = dbutility.UpdateJobBill(jobBill, userData.Company, userData.FYear);
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
                var accounts = accountDataLayer.GetBillAccounts(userData.Company, userData.Company, userData.FYear);

                //Salesaccount
                var salesaccounts = accountDataLayer.GetSalesAccounts(userData.Company, userData.Company, userData.FYear);

                var jobBilling = jobBilingDataLayer.GetJobBilling(userData.Company, userData.Company, userData.FYear);
                var recieptNo = jobBilling.FirstOrDefault().MaxSerialNumber;
                recieptNo++;

                var jobBillingVM = new JobBillingViewModel();

                jobBillingVM.SerialNumber = recieptNo;
                jobBillingVM.ReferenceDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                jobBillingVM.PostingDate = jobBillingVM.ReferenceDate;
                jobBillingVM.ReferenceNumber = jobBillingVM.ReferenceNumber;

                jobBillingVM.Accounts = accounts.OrderBy(ord => ord.AccountName).Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                jobBillingVM.SalesAccounts = salesaccounts.OrderBy(ord => ord.AccountName).Select(sel => new AccountMasterVM
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
        public ActionResult DeleteBilling(string serialNo)
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