using System;

namespace Transactiondetails.DBModels
{
    public class JobBillingMaster
    {
        public int SerialNumber { get; set; }
        public string ProcessingPointCode { get; set; }
        public string FinancialYearCode { get; set; }
        public string BranchCode { get; set; }
        public string AccountCode { get; set; }
        public DateTime ReferenceDate { get; set; }
        public bool DeletedFlag { get; set; }
        public int UserCode { get; set; }
        public DateTime EntryDate { get; set; }
        public int ModiUserCode { get; set; }
        public DateTime ModiDate { get; set; }
        public string ReferenceNumber { get; set; }
        public int MaxSerialNumber { get; set; }
        public string AccountName { get; set; }
        public int LedSerialNumber { get; set; }
        public string SaleAccountCode { get; set; }
        public Int16 CreditDays { get; set; }
        public DateTime PostingDate { get; set; }
        public double PostingAmount { get; set; }
        public double PostingAmount2 { get; set; }
        public string ReferenceType { get; set; }
        public double Addless1 { get; set; }

        public double Addless2 { get; set; }
        public double Addless3 { get; set; }
        public double PostingAmount3 { get; set; }
        public double RoundedAmount { get; set; }

        public double FinalAmount { get; set; }
         
    }
}
