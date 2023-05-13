﻿using GridMVCAjaxDemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        public ActionResult GetPendingJobworkReceipt()
        {
            string accountcode = "A0000101";
            var userData = (UserData)Session["UserData"];

            var jobDespatchDataLayer = new JobDespatchDataLayer();
            jobDespatchDataLayer.GetPendingJobReciept(accountcode, userData.Company, userData.Branch, userData.FYear);

            var jobDespatchDetails = new List<JobDespatchDetail>();
            jobDespatchDetails.Add(new JobDespatchDetail { BillingQuantity = 1, BillingRate = 1 });

            var jobDespatch = new JobDespatch();
            jobDespatch.JobDespatchMaster = new JobDespatchMaster
            {
                AccountCode = accountcode,
                BranchCode = userData.Branch,
            };
            jobDespatch.JobDespatchDetails = jobDespatchDetails;

            jobDespatchDataLayer.SaveJobDespatch(jobDespatch, userData.Company, userData.FYear);

            return View();

        }
 
        public ActionResult GetJobDesptachBySerialNo(int id)
        {
            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var dbutility = new DBUtility();
            var accountDataLayer = new AccountDataLayer();

            try
            {
                var userData = (UserData)Session["UserData"];

                var jobReciept = jobDespatchDataLayer.GetJobDesptachBySerialNo(userData.Company, userData.FYear, id);
                var process = dbutility.GetProcesses();
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
                var jobReceiptVM = new JobReciptVM();

                jobReceiptVM.AccountCode = jobReciept.JobRecieptDets.FirstOrDefault().AccountCode;
                jobReceiptVM.ReferenceDate = jobReciept.JobRecieptDets.FirstOrDefault().ReferenceDate;
                jobReceiptVM.SerialNumber = id;

                jobReceiptVM.JobReceiptDetails = jobReciept.JobRecieptDets.Join(process,
                   jd => jd.ProcessCode,
                   prc => prc.ProcessCode,
                   (jobreciptdet, process1) => new JobReceiptDetailVM
                   {
                       ProcessName = process1.ProcessName,
                       ProcessCode = jobreciptdet.ProcessCode,
                       ItemCarats = jobreciptdet.ItemCarats,
                       ItemLines = jobreciptdet.ItemLines,
                       ItemPieces = jobreciptdet.ItemPieces,
                       PacketNumber = jobreciptdet.PacketNumber,
                       ItemSerialNumber = jobreciptdet.ItemSerialNumber,
                       Remarks = jobreciptdet.Remarks
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

                jobReceiptVM.Mode = Mode.Update;

                return PartialView("_JobworkDespatchPartial", jobReceiptVM);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult JobworkDespatch()
        {
            //var jobDespatchDataLayer = new JobDespatchDataLayer();
            //var dbutility = new DBUtility();
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkDespatch";
            //var accountDataLayer = new AccountDataLayer();
            //try
            //{
            //    var userData = (UserData)Session["UserData"];
            //    var jobDespatchs = jobDespatchDataLayer.GetJobDespatch(userData.Company, userData.Company, userData.FYear);
            //    //var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
            //    //var process = dbutility.GetProcesses();
            //    //var recieptNo = jobDespatchs.FirstOrDefault().MaxSerialNumber;
            //    //recieptNo++;
            //    var data = jobDespatchs.Select(sel => new JobDespatchViewModel
            //    {
            //        SerialNumber = sel.SerialNumber,
            //        AccountCode = sel.AccountCode,
            //        AccountName = sel.AccountName,
            //        ReferenceDate = sel.ReferenceDate
            //    }).OrderByDescending(x => x.SerialNumber);

            //    return View(data);
            //}
            //catch (Exception ex)
            //{
            //    string message = ex.Message;
            //}s

            return View(new List<JobDespatchViewModel>());
        }

        public ActionResult JobworkReceiptDtTable()
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

                return View("~/Views/JobWorkReceipts/JobworkReceiptDtTable.cshtml", data);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return View();
        }
         

        [HttpGet]
        public JsonResult JobworkDespatchDataTable(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString, string id)
        {
            var jobDespatchDataLayer = new JobDespatchDataLayer();
            var dbutility = new DBUtility();
            ViewBag.Menu = "Master";
            ViewBag.SubMenu = "JobworkDespatch";
            var accountDataLayer = new AccountDataLayer();

            try
            {
                sord = (sord == null) ? "" : sord;
                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;

                var userData = (UserData)Session["UserData"];
                var jobDespatchs = jobDespatchDataLayer.GetJobDespatch(userData.Company, userData.Company, userData.FYear);
                var accounts = accountDataLayer.GetAccounts(userData.Company, userData.Company, userData.FYear);
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
                    switch (searchField)
                    {
                        case "AccountName":
                            data = data.Where(x => x.AccountName.Contains(searchString)).ToList();
                            break;
                        case "ReferenceDate":
                            var dt = Convert.ToDateTime(searchString);
                            data = data.Where(x => x.ReferenceDate == dt).ToList();
                            break;
                    }
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
        public ActionResult GetFilterJobworkRecipt(JobReciptVM model)
        {
            var jobReceiptDataLayer = new JobReceiptDataLayer();
            var dbutility = new DBUtility();
            try
            {
                var userData = (UserData)Session["UserData"];
                var jobReceiept = jobReceiptDataLayer.GetJobReciept(userData.Company, userData.Company, userData.FYear);

                var list = Enumerable.Empty<JobReciptVM>();

                var data = jobReceiept.JobRecieptMasts.Select(sel => new JobReciptVM
                {
                    SerialNumber = sel.SerialNumber,
                    AccountCode = sel.AccountCode,
                    AccountName = sel.AccountName,
                    ReferenceDate = sel.ReferenceDate
                }).OrderByDescending(x => x.SerialNumber);

                if (!string.IsNullOrEmpty(model.AccountName) &&
                    model.ReferenceDate.HasValue)
                {
                    list = data.Where(x => x.AccountName.Contains(model.AccountName) && x.ReferenceDate == x.ReferenceDate);
                }
                else if (!string.IsNullOrEmpty(model.AccountName) &&
                        !model.ReferenceDate.HasValue)
                {
                    list = data.Where(x => x.AccountName.Contains(model.AccountName));
                }
                else if (string.IsNullOrEmpty(model.AccountName) &&
                        model.ReferenceDate.HasValue)
                {
                    list = data.Where(x => x.ReferenceDate == x.ReferenceDate);
                }

                var lst = list.ToList();

                string strTable = "";
                strTable = strTable + "<table class=\"table table-striped table-bordered\"><thead><tr class=\"gridHead\"><th>Account Code</th><th>Account Name</th>";
                strTable = strTable + "<th>Serial Number</th><th>Reference Date</th></tr></thead><tbody>";

                foreach (var item in lst)
                {
                    strTable = strTable + "<tr>";
                    strTable = strTable + "<td>" + item.AccountCode + "</td>";
                    strTable = strTable + "<td>" + item.AccountName + "</td>";
                    strTable = strTable + "<td>" + item.SerialNumber + "</td>";
                    strTable = strTable + "<td>" + item.ReferenceDate + "</td>";
                    strTable = strTable + "</tr>";
                }
                strTable = strTable + "</tbody></table>";

                return Json(strTable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var message = new { message = "Exception occured", error = "True" };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}