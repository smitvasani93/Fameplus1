using System.Configuration;
using System.Data.Entity;

namespace Transactiondetails.Models.Utility
{
    public class CompanyDBContext : DbContext
    { 
        public CompanyDBContext(string companyCode)
        {
            if (!string.IsNullOrEmpty(companyCode))
            {
                string companyname = "Test_FMComp" + companyCode;
                string conn = ConfigurationManager.ConnectionStrings["CompanyDB"].ConnectionString;
                conn = conn.Replace("sampletest", companyname);
                this.Database.Connection.ConnectionString = conn;
            }
        }
    }
}