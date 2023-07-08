using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models.Utility
{
    public class JobBillingDataLayer
    {
        public List<PendingJobDespatchDetail> GetPendingJobDespatch(string accountCode, string companyCode, string branchCode, string FYear)
        {
            var pendingJobDespatchDetail = new List<PendingJobDespatchDetail>();
             using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pAccountCode = new SqlParameter("@AccountCode", accountCode);
                var pFYear = new SqlParameter("@FinancialYearCode", FYear);
                var pBranch = new SqlParameter("@BranchCode", branchCode);

                pendingJobDespatchDetail = db.Database.SqlQuery<PendingJobDespatchDetail>("exec SpGetJobDespatchByAccount @AccountCode,@BranchCode,@FinancialYearCode", pAccountCode, pBranch, pFYear).ToList();
            }

            return pendingJobDespatchDetail;
        }

        public List<JobBillingMaster> GetJobBilling(string companyCode, string branchCode, string FYear)
        {
            var jobBillingMasters = new List<JobBillingMaster>();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pFYear = new SqlParameter("@FinancialYearCode", FYear);
                var pBranch = new SqlParameter("@BranchCode", branchCode);

                jobBillingMasters = db.Database.SqlQuery<JobBillingMaster>("exec spGetJobBilling @FinancialYearCode, @BranchCode", pFYear, pBranch).ToList();
            }

            return jobBillingMasters;
        }

        public JobBillingData GetJobBillBySerialNo(string companyCode, string FYear, int serialNo)
        {
            var jobBillingData = new JobBillingData();
            var jobMaster =  new JobBillingMaster();
            var jobBillingDets = new List<JobBillingDetail>();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pSNumber = new SqlParameter("@SerialNumber", serialNo);
                jobMaster = db.Database.SqlQuery<JobBillingMaster>("exec SpGetJobBillingMasterBySerialNumber @SerialNumber", pSNumber).FirstOrDefault();

                var pSNumber2 = new SqlParameter("@SerialNumber", serialNo);
                jobBillingDets = db.Database.SqlQuery<JobBillingDetail>("exec SpGetJobBillingBySerialNumber @SerialNumber", pSNumber2).ToList();
            }
            jobBillingData.JobBillingMast = jobMaster;
            jobBillingData.JobBillingDets = jobBillingDets;
            return jobBillingData;
        }

        public DatabaseResponse SaveJobBill(JobBill jobBilling, string companyCode, string fYear)
        {
            var xmlString = XmlUtility.Serialize(jobBilling);
            var pxmlString = new SqlParameter("@xmlString", xmlString);
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to dump the xml to database
                return db.Database.SqlQuery<DatabaseResponse>("exec spJobBillAdd @xmlString", pxmlString).FirstOrDefault();
            }
        }

        public DatabaseResponse UpdateJobBill(JobBill jobBilling, string companyCode, string fYear)
        {
            var xmlString = XmlUtility.Serialize(jobBilling);
            var pxmlString = new SqlParameter("@xmlString", xmlString);
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to dump the xml to database
                return db.Database.SqlQuery<DatabaseResponse>("exec spJobBillUpdate @xmlString", pxmlString).FirstOrDefault();
            }
        }
    }
}
