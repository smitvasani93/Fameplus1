using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models.Utility
{
    public class JobReceiptDataLayer
    {
        public JobRecieptData GetJobReciept(string companyCode, string FYear)
        {
            var JobRecieptData = new JobRecieptData();
            var processMasterList = new List<ProcessMaster>();
            var accountMasterList = new List<AccountMaster>();
            var jobRecieptMasterList = new List<JobRecieptMaster>();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            {
                //Call Stored Procedure to get the JobReciepts
                JobRecieptData.Processes = db.Database.SqlQuery<ProcessMaster>("exec spGetProcessMaster").ToList();
                JobRecieptData.Accounts = db.Database.SqlQuery<AccountMaster>("exec spGetAccount").ToList();
                var pFYear = new SqlParameter("@FinancialYearCode", FYear);

                JobRecieptData.JobReciepts = db.Database.SqlQuery<JobRecieptMaster>("exec spGetJobReceipt @FinancialYearCode", pFYear).ToList();
            }

            return JobRecieptData;
        }
    }
}