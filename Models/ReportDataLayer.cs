using System.Data;
using Transactiondetails.Models.Utility;
public class ReportDataLayer
{
    public DataTable GetJobWorkBillingReport(string companyCode, string branchCode, string FYear)
    {
        DataTable dtBill = new DataTable();
        using (CompanyDBContext db = new CompanyDBContext(companyCode))
        {
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec sp_JobWorkBilling";

            cmd.Connection.Open();
            dtBill.Load(cmd.ExecuteReader());
        }

        return dtBill;
    }
}