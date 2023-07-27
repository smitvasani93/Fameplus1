using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace Transactiondetails.Models.Utility
{
    public class CompanyDBContext : DbContext
    {
        public static string DBPrefix = "Test_FMComp";
        static CompanyDBContext()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("DBPrefix"))
            {
                DBPrefix = ConfigurationManager.AppSettings["DBPrefix"];
            }
            //string.IsNullOrEmpty()
        }

        public CompanyDBContext(string companyCode)
        {
            if (!string.IsNullOrEmpty(companyCode))
            {
                string companyname = DBPrefix + companyCode;
                string conn = ConfigurationManager.ConnectionStrings["CompanyDB"].ConnectionString;
                if (companyCode == "999")
                {
                    string dbPrefix2 = ConfigurationManager.AppSettings["DBPrefix2"];

                    conn = conn.Replace("sampletest", dbPrefix2);

                }
                else
                {
                    conn = conn.Replace("sampletest", companyname);

                }
                this.Database.Connection.ConnectionString = conn;
            }
        }
    }
}