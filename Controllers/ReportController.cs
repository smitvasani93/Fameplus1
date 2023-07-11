using System;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Transactiondetails.App_Start;
using Transactiondetails.ViewModels;

namespace Transactiondetails.Controllers
{
    [SessionExpireFilter]
    [Authorize]
    public class ReportController : Controller
    {
        public ActionResult JobWorkBillingReport(int id)
        {
              
            try
            {
                ViewBag.Menu = "Master";
                ViewBag.SubMenu = "JobBilling";

                int serialNumber = id;
                var userData = (UserData)Session["UserData"];

                var reportDataLayer = new ReportDataLayer();
                var dtBill = reportDataLayer.GetJobBillingMasPrint(serialNumber,userData.Company, userData.Company, userData.FYear);

                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                reportViewer.Height = System.Web.UI.WebControls.Unit.Pixel(600);
                reportViewer.ZoomPercent = 100;
                string reportpath= Request.MapPath(Request.ApplicationPath) + @"\Rdlc\JobBillingMasPrint.rdlc";
                
                //var Status = formCollection["Status"];
                reportViewer.LocalReport.ReportPath = reportpath;//  Request.MapPath(Request.ApplicationPath) + @"\Report\JobWorkBillingGSTReport.rdlc";

                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtBill));
                ViewBag.ReportViewer = reportViewer;

            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return View();
        }

    }

}
