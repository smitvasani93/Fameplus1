using System;

namespace BusinessLayer.DBModels
{
    public class JobRecieptMaster
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime ReferenceDate { get; set; }
        public int SerialNumber { get; set; }
        public int MaxSerialNumber { get; set; }
        public string ProcessingPointCode { get; set; }
        public string FinancialYearCode { get; set; }
        public string BranchCode { get; set; }
        public int UserCode { get; set; }
        public DateTime EntryDate { get; set; }
        public int ModiUserCode { get; set; }
        public DateTime ModiDate { get; set; }

    }
}