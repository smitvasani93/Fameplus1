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
    public class JobWorkDespatchsController : Controller
    {
        private IGridMvcHelper gridMvcHelper;
        private const string GRID_PARTIAL_PATH = "~/Views/Home/JobworkReceipt.cshtml";

        public JobWorkDespatchsController()
        {
            this.gridMvcHelper = new GridMvcHelper();
        }

        public ActionResult GetPendingJobworkReceipt(string accountcode)
        {
            //string accountcode = "A0000101";
            var userData = (UserData)Session["UserData"];

            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var accountDataLayer = new AccountDataLayer();
            var account = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear).FirstOrDefault(x => x.AccountCode == accountcode);

            var pendingJobReciepts = jobDespatchDataLayer.GetPendingJobReciept(accountcode, userData.Company, userData.Branch, userData.FYear)
                                     .Select(x => new JobDespatchDetailViewModel
                                     {
                                         SerialNumber = x.SerialNumber,
                                         CustomrName = account.AccountName,
                                         ItemSerialNumber = x.ItemSerialNumber,
                                         ReferenceDate = x.ReferenceDate,
                                         ProcessName = x.ProcessName,
                                         ProcessCode = x.ProcessCode,
                                         PacketNumber = x.PacketNumber,
                                         BalItemCarats = x.Bal_ItemCarats,
                                         BalItemPieces = x.Bal_ItemPieces,
                                         BillingType = x.BillingType,
                                         ItemCarats = x.ItemCarats,
                                         ItemLines = x.ItemLines,
                                         ItemPieces = x.ItemPieces
                                     });

            return Json(pendingJobReciepts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobDesptachBySerialNo(int id)
        {
            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var accountDataLayer = new AccountDataLayer();

            try
            {
                var userData = (UserData)Session["UserData"];

                var jobDespatch = jobDespatchDataLayer.GetJobDesptachBySerialNo(userData.Company, userData.FYear, id);
                var jobDespatchViewModel = new JobDespatchViewModel();

                jobDespatchViewModel.AccountCode = jobDespatch.JobDespatchDetails.FirstOrDefault().AccountCode;
                jobDespatchViewModel.ReferenceDate = jobDespatch.JobDespatchDetails.FirstOrDefault().ReferenceDate;
                jobDespatchViewModel.SerialNumber = id;

                jobDespatchViewModel.JobDespatchDetails = jobDespatch.JobDespatchDetails.Select(x => new JobDespatchDetailViewModel
                {
                    ReferenceDate = x.ReferenceDate.Value,
                    WeightLoss = x.WeightLoss,
                    JRSerialNumber = x.JRSerialNumber,
                    JRItemSerialNumber = x.JRItemSerialNumber,
                    ItemPieces = x.ItemPieces,
                    ItemLines = x.ItemLines,
                    ItemCarats = x.ItemCarats,
                    NoChargeQuantity = x.NoChargeQuantity,
                    BillingQuantity = x.BillingQuantity,
                    Rate = x.BillingRate,
                    ItemSerialNumber = x.ItemSerialNumber,
                    Remarks = x.Remarks,
                    Status = x.PacketStatus,
                    ProcessCode = x.ProcessCode,
                    ProcessName = x.ProcessName,
                    BillingType =x.BillingType,
                    BillingUnit = x.BillingType

                }).ToList();


                //jobReceiptVM.JobReceiptDetails = jobReciept.JobRecieptDets.Join(process,
                //   jd => jd.ProcessCode,
                //   prc => prc.ProcessCode,
                //   (jobreciptdet, process1) => new JobReceiptDetailVM
                //   {
                //       ProcessName = process1.ProcessName,
                //       ProcessCode = jobreciptdet.ProcessCode,
                //       ItemCarats = jobreciptdet.ItemCarats,
                //       ItemLines = jobreciptdet.ItemLines,
                //       ItemPieces = jobreciptdet.ItemPieces,
                //       PacketNumber = jobreciptdet.PacketNumber,
                //       ItemSerialNumber = jobreciptdet.ItemSerialNumber,
                //       Remarks = jobreciptdet.Remarks
                //   }).ToList();

                //jobReceiptVM.Processes = process.Select(sel => new ProcessMasterVM
                //{
                //    ProcessCode = sel.ProcessCode,
                //    ProcessName = sel.ProcessName
                //});
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);

                jobDespatchViewModel.Accounts = accounts.Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                jobDespatchViewModel.Mode = Mode.Update;

                return PartialView("_JobworkDespatchUpdatePartial", jobDespatchViewModel);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult JobworkDespatch()
        {
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkDespatch";
            return View(Enumerable.Empty<JobDespatchViewModel>());
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
        public JsonResult JobworkDespatchDataTable(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString, string filters, string id)
        {
            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();

            try
            {
                sord = (sord == null) ? "" : sord;
                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;

                var userData = (UserData)Session["UserData"];
                var jobDespatchs = jobDespatchDataLayer.GetJobDespatch(userData.Company, userData.Branch, userData.FYear);
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Branch, userData.FYear);
                var process = dbutility.GetProcesses();
                var recieptNo = jobDespatchs.FirstOrDefault().MaxSerialNumber;
                recieptNo++;
                var data = jobDespatchs.Select(sel => new JobReciptVM
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

        public ActionResult GetJobworkDespatch()
        {
            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();
            try
            {
                var userData = (UserData)Session["UserData"];

                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);

                //var process = dbutility.GetProcesses();

                var jobDespatchMasters = jobDespatchDataLayer.GetJobDespatch(userData.Company, userData.Company, userData.FYear);
                var recieptNo = jobDespatchMasters.FirstOrDefault().MaxSerialNumber;
                recieptNo++;

                var jobDespatchVM = new JobDespatchViewModel();

                jobDespatchVM.SerialNumber = recieptNo;
                jobDespatchVM.ReferenceDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));

                //jobDespatchVM.Processes = process.Select(sel => new ProcessMasterVM
                //{
                //    ProcessCode = sel.ProcessCode,
                //    ProcessName = sel.ProcessName
                //});

                jobDespatchVM.Accounts = accounts.Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                jobDespatchVM.JobDespatchDetails = new List<JobDespatchDetailViewModel>
                {
                    new JobDespatchDetailViewModel{
                         ItemSerialNumber=1,
                         ItemLines=0,
                         PacketNumber=1,
                         ItemPieces=1,
                         ItemCarats=0
                    }
                };

                jobDespatchVM.Mode = Mode.Add;
                return PartialView("_JobworkDespatchPartial", jobDespatchVM);
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

                if(processdetails!= null)
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