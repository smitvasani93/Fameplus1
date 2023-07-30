using DataTables.Mvc;
using GridMVCAjaxDemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using Transactiondetails.CustomFilter;
using Transactiondetails.DBModels;
using Transactiondetails.Models;
using Transactiondetails.Models.Utility;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
    //public class JobReciptJq
    //{
    //    public string AccountCode { get; set; }
    //    public string AccountName { get; set; }
    //    public DateTime? ReferenceDate { get; set; }
    //    public int? SerialNumber { get; set; }
    //}

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
            //var jobReceiptDataLayer = new JobReceiptDataLayer();
            //var dbutility = new DBUtility();
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkReceipt";
            //var accountDataLayer = new AccountDataLayer();
            //try
            //{
            //    var userData = (UserData)Session["UserData"];
            //    var jobReceiept = jobReceiptDataLayer.GetJobReciept(userData.Company, userData.Company, userData.FYear);
            //    var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
            //    var process = dbutility.GetProcesses();
            //    var recieptNo = jobReceiept.JobRecieptMasts.FirstOrDefault().MaxSerialNumber;
            //    recieptNo++;
            //    var data = jobReceiept.JobRecieptMasts.Select(sel => new JobReciptVM
            //    {
            //        SerialNumber = sel.SerialNumber,
            //        AccountCode = sel.AccountCode,
            //        AccountName = sel.AccountName,
            //        ReferenceDate = sel.ReferenceDate
            //    }).OrderByDescending(x => x.SerialNumber);

            //    return View("~/Views/JobWorkReceipts/JobworkReceiptDtTable.cshtml", data);
            //}
            //catch (Exception ex)
            //{
            //    string message = ex.Message;
            //}

            return View(new List<JobReciptVM>());
        }

        [HttpGet]
        public JsonResult JobworkReceiptDataTable(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string filters, string searchString)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();

            var accountDataLayer = new AccountDataLayer();

            try
            {
                sord = (sord == null) ? "" : sord;
                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;

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

                    /*
                    switch (searchField)
                    {
                        case "AccountName":
                            data = data.Where(x => x.AccountName.Contains(searchString)).ToList();
                            break;
                        case "ReferenceDate":
                            var dt = Convert.ToDateTime(searchString);
                            data = data.Where(x => x.ReferenceDate == dt).ToList();
                            break;
                    }*/
                }

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

        public ActionResult JobworkReceiptData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkReceipt";
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

                var filteredCount = data.Count();
                var totalCount = data.Count();

                return Json(new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return View();
        }

        [OutputCache(Duration = 0)]
        public ActionResult GetJobworkRecipt()
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();

            try
            {
                var userData = (UserData)Session["UserData"];
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
                var process = dbutility.GetProcesses();
                var jobReciept = jobReceiptDataLayer.GetJobReciept(userData.Company, userData.Company, userData.FYear);
                var recieptNo = jobReciept.JobRecieptMasts.FirstOrDefault().MaxSerialNumber;
                recieptNo++;

                var jobReceiptVM = new JobReciptVM();

                jobReceiptVM.SerialNumber = recieptNo;
                jobReceiptVM.ReferenceDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));

                jobReceiptVM.Processes = process.Select(sel => new ProcessMasterVM
                {
                    ProcessCode = sel.ProcessCode,
                    ProcessName = sel.ProcessName,
                    BillingType = sel.BillingType
                });

                jobReceiptVM.Accounts = accounts.OrderBy(ord => ord.AccountName).Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                jobReceiptVM.JobReceiptDetails = new List<JobReceiptDetailVM>
                {
                    new JobReceiptDetailVM{
                         ItemSerialNumber=1,
                         ItemLines=0,
                         PacketNumber=1,
                         ItemPieces=1,
                         ItemCarats=0
                    }
                };

                jobReceiptVM.Mode = Mode.Add;
                return PartialView("_JobworkReceiptPartial", jobReceiptVM);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 0)]
        public ActionResult GetJobworkReciptBySerialNo(int id)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();

            try
            {
                var userData = (UserData)Session["UserData"];

                var jobReciept = jobReceiptDataLayer.GetJobRecieptBySerialNumber(userData.Company, userData.FYear, id);
                var process = dbutility.GetProcesses();
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
                var jobReceiptVM = new JobReciptVM();

                jobReceiptVM.AccountCode = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
                jobReceiptVM.ReferenceDate = jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate;
                jobReceiptVM.SerialNumber = id;

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
                    ProcessName = sel.ProcessName,
                    BillingType = sel.BillingType
                });

                jobReceiptVM.Accounts = accounts.OrderBy(ord => ord.AccountName).Select(sel => new AccountMasterVM
                {
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName
                });

                jobReceiptVM.Mode = Mode.Update;

                return PartialView("_JobworkReceiptPartial", jobReceiptVM);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveJobworkReceipt(JobReciptVM model)
        {
            var dbutility = new JobReceiptDataLayer();
            var message = new { message = "Failed", error = "True" };
            DatabaseResponse databaseResponse = null;
            try
            {
                var userData = (UserData)Session["UserData"];
                var jobRecieptMas = new JobRecieptMaster();
                //put your Fields Here
                jobRecieptMas.SerialNumber = model.SerialNumber.Value;
                jobRecieptMas.AccountCode = model.AccountCode;
                jobRecieptMas.ReferenceDate = model.ReferenceDate.Value;
                jobRecieptMas.EntryDate = DateTime.Now;
                jobRecieptMas.ModiDate = DateTime.Now;
                jobRecieptMas.FinancialYearCode = userData.FYear;
                jobRecieptMas.BranchCode = userData.Branch;
                jobRecieptMas.UserCode = userData.UserId; //Get usercode from session
                jobRecieptMas.ModiUserCode = userData.UserId; //Get usercode from session

                var jobRecipt = new JobReceipt();
                jobRecipt.JobReceiptMaster = jobRecieptMas;
                jobRecipt.JobReceiptDetails = model.JobReceiptDetails.Select(sel => new JobRecieptDetail
                {
                    ProcessCode = sel.ProcessCode.Value,
                    ItemCarats = sel.ItemCarats.Value,
                    PacketNumber = sel.PacketNumber.Value,
                    ItemSerialNumber = sel.ItemSerialNumber.Value,
                    ItemLines = sel.ItemLines.Value,
                    SerialNumber = model.SerialNumber.Value,
                    ItemPieces = sel.ItemPieces.Value,
                    Remarks = sel.Remarks
                }).ToList();

                if (model.Mode == Mode.Add)
                {
                    databaseResponse = dbutility.SaveJobworkReceipt(jobRecipt, userData.Company, userData.FYear);
                }
                else if (model.Mode == Mode.Update)
                {
                    databaseResponse = dbutility.UpdateJobworkReceipt(jobRecipt, userData.Company, userData.FYear);
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

        [HttpPost]
        public ActionResult DeleteReceipt(string serialNo)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var message = new { message = "Failed", error = "True" };
            try
            {
                var userData = (UserData)Session["UserData"];
                var databaseResponse = jobReceiptDataLayer.DeleteJobReciept(userData.Company, userData.FYear, Convert.ToInt32(serialNo));

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

                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}