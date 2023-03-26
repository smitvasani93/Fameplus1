﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Transactiondetails.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TransactionDetailsEntities : DbContext
    {
        public TransactionDetailsEntities()
            : base("name=TransactionDetailsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<JobBillingDet> JobBillingDets { get; set; }
        public virtual DbSet<JobBillingMa> JobBillingMas { get; set; }
        public virtual DbSet<JobDespatchDet> JobDespatchDets { get; set; }
        public virtual DbSet<JobDespatchMa> JobDespatchMas { get; set; }
        public virtual DbSet<JobReceiptDet> JobReceiptDets { get; set; }
        public virtual DbSet<JobReceiptMa> JobReceiptMas { get; set; }
        public virtual DbSet<ProcessDet> ProcessDets { get; set; }
        public virtual DbSet<ProcessMa> ProcessMas { get; set; }
    
        [DbFunction("TransactionDetailsEntities", "SplitString")]
        public virtual IQueryable<SplitString_Result> SplitString(string input, string character)
        {
            var inputParameter = input != null ?
                new ObjectParameter("Input", input) :
                new ObjectParameter("Input", typeof(string));
    
            var characterParameter = character != null ?
                new ObjectParameter("Character", character) :
                new ObjectParameter("Character", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<SplitString_Result>("[TransactionDetailsEntities].[SplitString](@Input, @Character)", inputParameter, characterParameter);
        }
    
        public virtual ObjectResult<sp_GetCustomers_Result> sp_GetCustomers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetCustomers_Result>("sp_GetCustomers");
        }
    
        public virtual ObjectResult<sp_GetDespatchData_Result> sp_GetDespatchData(string accountCode)
        {
            var accountCodeParameter = accountCode != null ?
                new ObjectParameter("AccountCode", accountCode) :
                new ObjectParameter("AccountCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetDespatchData_Result>("sp_GetDespatchData", accountCodeParameter);
        }
    
        public virtual ObjectResult<sp_GetProcess_Result> sp_GetProcess()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetProcess_Result>("sp_GetProcess");
        }
    
        public virtual ObjectResult<sp_GetSearchData_Result> sp_GetSearchData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetSearchData_Result>("sp_GetSearchData");
        }
    
        public virtual ObjectResult<sp_GetSelectedData_Result> sp_GetSelectedData(Nullable<long> serialNumber)
        {
            var serialNumberParameter = serialNumber.HasValue ?
                new ObjectParameter("SerialNumber", serialNumber) :
                new ObjectParameter("SerialNumber", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetSelectedData_Result>("sp_GetSelectedData", serialNumberParameter);
        }
    
        public virtual ObjectResult<sp_GetSelectedDespatchData_Result> sp_GetSelectedDespatchData(string input)
        {
            var inputParameter = input != null ?
                new ObjectParameter("Input", input) :
                new ObjectParameter("Input", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetSelectedDespatchData_Result>("sp_GetSelectedDespatchData", inputParameter);
        }
    
        public virtual ObjectResult<sp_GetNewDespatchData_Result> sp_GetNewDespatchData(string input)
        {
            var inputParameter = input != null ?
                new ObjectParameter("Input", input) :
                new ObjectParameter("Input", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetNewDespatchData_Result>("sp_GetNewDespatchData", inputParameter);
        }
    }
}
