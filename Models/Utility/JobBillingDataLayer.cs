using System;
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
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pSNumber = new SqlParameter("@SerialNumber", serialNo);
                jobBillingData.JobBillingDets = db.Database.SqlQuery<JobBillingDetail>("exec SpGetJobBillingBySerialNumber @SerialNumber", pSNumber).ToList();
            }

            return jobBillingData;
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
