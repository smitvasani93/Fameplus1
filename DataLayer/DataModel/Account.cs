using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataModel
{
    public  class Account
    {
        public string FinancialYearCode { get; set; }
        public string AccountCode { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public short? AccountGroupCode { get; set; }
        public short? ExceptGroupCode { get; set; }
        public bool GroupCompany { get; set; }
        public string Relation { get; set; }
        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }
        public short? CurrencyCode { get; set; }
        public string AccountAddress1 { get; set; }
        public string AccountAddress2 { get; set; }
        public string AccountAddress3 { get; set; }
        public string AccountPin { get; set; }
        public string AccountPhone { get; set; }
        public string AccountFax { get; set; }
        public string AccountResidencePhone { get; set; }
        public string AccountHandPhone { get; set; }
        public string AccountEMail { get; set; }
        public decimal? InterestRate { get; set; }
        public string InterestMethod { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IncomeTaxNumber { get; set; }
        public string BranchCode { get; set; }
        public string AccountField1 { get; set; }
        public string AccountField2 { get; set; }
        public string AccountField3 { get; set; }
        public string AccountField4 { get; set; }
        public string AccountField5 { get; set; }
        public string ModuleID { get; set; }
        public int? Table1Code { get; set; }
        public int? Table2Code { get; set; }
        public string Suspended { get; set; }
        public int? CityCode { get; set; }
        public short? UserCode { get; set; }
        public DateTime? EntryDate { get; set; }
        public short? ModiUserCode { get; set; }
        public DateTime? ModiDate { get; set; }
        public int? Field1 { get; set; }
        public int? Field2 { get; set; }
        public string AccountField6 { get; set; }
        public string AccountField7 { get; set; }
        public string AccountField8 { get; set; }
        public string AccountField9 { get; set; }
        public string AccountField10 { get; set; }
        public string Transfer { get; set; }
    }
}
