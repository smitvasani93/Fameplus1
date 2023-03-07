using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Web;

namespace Transactiondetails.Models.Utility
{
    public class CompanyDBContext : DbContext
    { 
        public CompanyDBContext(string companyCode)
        {
            if (!string.IsNullOrEmpty(companyCode))
            {
                string conn = ConfigurationManager.ConnectionStrings["CompanyDB"].ConnectionString;
                conn = conn.Replace("sampletest", "BSComp001");

                 
                this.Database.Connection.ConnectionString = conn;
                // companyCode = "BSComp" + companyCode;
                //var objContext = new ObjectContext(conn);
                 
                //objContext.Connection.ConnectionString = conn;
                //objContext.Connection.ChangeDatabase(companyCode);
                //objContext.CommandTimeout = 0;
            }
            
        }
    }
}