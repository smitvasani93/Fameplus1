using Syncfusion.EJ2.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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
    }
}
