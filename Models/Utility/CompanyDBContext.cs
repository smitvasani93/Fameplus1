using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Transactiondetails.Models.Utility
{
    public class CompanyDBContext : DbContext
    {
        public CompanyDBContext(string companyCode)
            : base("CompanyGen")
        {
            if (!string.IsNullOrEmpty(companyCode))
            {
                companyCode= "BSComp" + companyCode;
                var objContext = ((IObjectContextAdapter)this).ObjectContext;
                objContext.Connection.ChangeDatabase(companyCode);
                objContext.CommandTimeout = 0;
            }
            else
            {
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 0;
            }
        }
    }
}