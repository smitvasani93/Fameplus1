using System;

namespace Transactiondetails.DBModels
{
    public class JobDespatchMaster
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

        //public string AccountName { get; set; }
        //public int MaxSerialNumber { get; set; }
    }
}
