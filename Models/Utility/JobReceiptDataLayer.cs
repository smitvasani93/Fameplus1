using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models.Utility
{
    public class JobReceiptDataLayer
    {
        public JobRecieptData GetJobReciept(string companyCode, string branchCode,  string FYear)
        {
            var JobRecieptData = new JobRecieptData();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pFYear = new SqlParameter("@FinancialYearCode", FYear);
                var pBranch = new SqlParameter("@BranchCode", branchCode);

                JobRecieptData.JobRecieptMasts = db.Database.SqlQuery<JobRecieptMaster>("exec spGetJobReceipt @FinancialYearCode, @BranchCode", pFYear, pBranch).ToList();
            }

            return JobRecieptData;
        }

        public List<AccountMaster> GetAccounts(string companyCode, string branchCode, string FYear)
        {
            var accountMasterList = new List<AccountMaster>();
            //if Data Exists in session
            if (HttpContext.Current.Session["Accounts"] != null)
            {
                return  (List<AccountMaster>)HttpContext.Current.Session["Accounts"];
            }
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                accountMasterList = db.Database.SqlQuery<AccountMaster>("exec spGetAccount").ToList();

                HttpContext.Current.Session["Accounts"] = accountMasterList;
            }

            return accountMasterList;
        }


        public JobRecieptData GetJobRecieptBySerialNumber(string companyCode, string FYear, int serialNo)
        {
            var JobRecieptData = new JobRecieptData();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pSNumber = new SqlParameter("@SerialNumber", serialNo);
                JobRecieptData.JobRecieptDets = db.Database.SqlQuery<JobRecieptDetail>("exec SpGetJobRecieptBySerialNumber @SerialNumber", pSNumber).ToList();
            }

            return JobRecieptData;
        }

        public DatabaseResponse SaveJobworkReceipt(JobReceipt jobRecipt, string companyCode, string fYear)
        {
            var xmlString = XmlUtility.Serialize(jobRecipt);
            var pxmlString = new SqlParameter("@xmlString", xmlString);
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to dump the xml to database
                return db.Database.SqlQuery<DatabaseResponse>("exec spJobReceiptAdd @xmlString", pxmlString).FirstOrDefault();
            }
        }
    }
}