using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Transactiondetails.Models.Utility
{
    public class GenDBContext : DbContext
    {
        public GenDBContext()
            : base("CompanyGen")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 0;
        }

        //public GenDBContext(string connectionstring)
        //    : base(connectionstring)
        //{
        //}
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        //    // Turn off Globally EF creation of all one-to-many relations with ON DELETE CASCADE
        //    modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        //}
    }
}