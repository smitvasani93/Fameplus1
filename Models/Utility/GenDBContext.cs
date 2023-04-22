using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Transactiondetails.Models.Utility
{
    public class GenDBContext : DbContext
    {
        public GenDBContext()
            : base("CompanyGen")
        {
            // ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 0;
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout =180;
        } 
    }
}