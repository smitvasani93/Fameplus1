using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models.Utility
{
    public class JobBillingDataLayer
    {
        public List<PendingJobRecieptDetail> GetPendingJobDespatch(string accountCode, string companyCode, string branchCode, string FYear)
        {
            var pendingJobReciepts = new List<PendingJobRecieptDetail>();
             using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pAccountCode = new SqlParameter("@AccountCode", accountCode);
                var pFYear = new SqlParameter("@FinancialYearCode", FYear);
                var pBranch = new SqlParameter("@BranchCode", branchCode);

                pendingJobReciepts = db.Database.SqlQuery<PendingJobRecieptDetail>("exec SpGetJobRecieptByAccount @AccountCode,@BranchCode,@FinancialYearCode", pAccountCode, pBranch, pFYear).ToList();
            }

            return pendingJobReciepts;
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

        public JobRecieptData GetJobDesptachBySerialNo(string companyCode, string FYear, int serialNo)
        {
            var JobRecieptData = new JobRecieptData();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pSNumber = new SqlParameter("@SerialNumber", serialNo);
                JobRecieptData.JobRecieptDets = db.Database.SqlQuery<JobRecieptDetail>("exec SpGetJobDesptachBySerialNumber @SerialNumber", pSNumber).ToList();
            }

            return JobRecieptData;
        }


        public DatabaseResponse SaveJobDespatch(JobDespatch jobDespatch, string companyCode, string fYear)
        {
            var xmlString = XmlUtility.Serialize(jobDespatch);
            var pxmlString = new SqlParameter("@xmlString", xmlString);
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to dump the xml to database
                return db.Database.SqlQuery<DatabaseResponse>("exec spJobDespatchAdd @xmlString", pxmlString).FirstOrDefault();
            }
        }
        public DatabaseResponse UpdateJobDespatch(JobDespatch jobDespatch, string companyCode, string fYear)
        {
            var xmlString = XmlUtility.Serialize(jobDespatch);
            var pxmlString = new SqlParameter("@xmlString", xmlString);
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to dump the xml to database
                return db.Database.SqlQuery<DatabaseResponse>("exec spJobDespatchUpdate @xmlString", pxmlString).FirstOrDefault();
            }
        }
    }
}
