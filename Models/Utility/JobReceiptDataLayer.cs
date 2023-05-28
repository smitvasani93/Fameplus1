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
        public DatabaseResponse UpdateJobworkReceipt(JobReceipt jobRecipt, string companyCode, string fYear)
        {
            var xmlString = XmlUtility.Serialize(jobRecipt);
            var pxmlString = new SqlParameter("@xmlString", xmlString);
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to dump the xml to database
                return db.Database.SqlQuery<DatabaseResponse>("exec spJobReceiptUpdate @xmlString", pxmlString).FirstOrDefault();
            }
        }

        public DatabaseResponse DeleteJobReciept(string companyCode, string FYear, int serialNo)
        {
            var JobRecieptData = new JobRecieptData();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                var pSNumber = new SqlParameter("@SerialNumber", serialNo);
                return   db.Database.SqlQuery<DatabaseResponse>("exec SpDeleteJobReciept @SerialNumber", pSNumber).FirstOrDefault();
            }
         }
    }
}