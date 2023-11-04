using System.Data;
using System.Data.SqlClient;
using Transactiondetails.Models.Utility;
public class ReportDataLayer
{
    public DataTable GetJobBillingMasPrint(int serailNumber, string companyCode, string branchCode, string FYear)
    {
        DataTable dtBill = new DataTable();
        using (CompanyDBContext db = new CompanyDBContext(companyCode))
        {

            //SqlParameter[] param = {new SqlParameter("@FinancialYearCode",FYear),
            //            new SqlParameter("@SerialNumber",serailNumber)
            //            };
            //dtBill =   db.Database.SqlQuery<JobDespatchMaster>("exec sp_JobBillingMasPrint", param);
             
            SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_JobBillingMasPrint";
            cmd.Parameters.AddWithValue("@FinancialYearCode", FYear);
            cmd.Parameters.AddWithValue("@SerialNumber", serailNumber);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
             da.Fill(dtBill);
            conn.Close();
            da.Dispose();



            //var cmd = db.Database.Connection.CreateCommand();
            //cmd.CommandText = "exec sp_JobBillingMasPrint";

            //cmd.Parameters.AddRange(param);
            //cmd.Connection.Open();

            //cmd.exe
            //dtBill.Load(cmd.ExecuteReader());
        }

        return dtBill;
    }
}