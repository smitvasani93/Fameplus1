using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models.Utility
{
    public class JobDespatchDataLayer
    {
        public List<PendingJobRecieptDetail> GetPendingJobReciept(string accountCode, string companyCode, string branchCode, string FYear)
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

        public List<JobDespatchMaster> GetJobDespatch(string companyCode, string branchCode, string FYear)
        {
            var jobDespatchMasters = new List<JobDespatchMaster>();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pFYear = new SqlParameter("@FinancialYearCode", FYear);
                var pBranch = new SqlParameter("@BranchCode", branchCode);

                jobDespatchMasters = db.Database.SqlQuery<JobDespatchMaster>("exec spGetJobDespatch @FinancialYearCode, @BranchCode", pFYear, pBranch).ToList();
            }

            return jobDespatchMasters;
        }


        public JobDespatch GetJobDesptachBySerialNo(string companyCode, string FYear, int serialNo)
        {
            var jobDespatch = new JobDespatch();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pSNumber = new SqlParameter("@SerialNumber", serialNo);
                jobDespatch.JobDespatchDetails = db.Database.SqlQuery<JobDespatchDetail>("exec SpGetJobDesptachBySerialNumber @SerialNumber", pSNumber).ToList();
            }

            return jobDespatch;
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

        public DatabaseResponse DeleteJobDespatch(string companyCode, string fYear, int serialNo)
        {
             using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                var pSerialNumber = new SqlParameter("@SerialNumber", serialNo);
                //Call Stored Procedure to dump the xml to database
                return db.Database.SqlQuery<DatabaseResponse>("exec SpDeleteJobDespatch @SerialNumber", pSerialNumber).FirstOrDefault();
            }
        }
    }
}
