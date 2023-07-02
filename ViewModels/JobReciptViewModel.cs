using System;
using System.Collections.Generic;

namespace Transactiondetails.ViewModels
{
    public class JobReciptVM
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public int? SerialNumber { get; set; }
        public string ReferenceNumber { get; set; }

        public Mode Mode { get; set; }
        public IEnumerable<ProcessMasterVM> Processes { get; set; }
        public IEnumerable<AccountMasterVM> Accounts { get; set; }
        public List<JobReceiptDetailVM> JobReceiptDetails { get; set; }

        public JobReciptVM()
        {
            JobReceiptDetails = new List<JobReceiptDetailVM>();
        }
    }

    public class ProcessMasterVM
    {
        public Int16 ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string BillingType { get; set; }
    }

    public class AccountMasterVM
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }

    public enum Mode
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }


    public class JobReceiptDetailVM
    {
        public string ProcessName { get; set; }
        public short? PacketNumber { get; set; }
        public double? ItemPieces { get; set; }
        public double? ItemCarats { get; set; }
        public double? ItemLines { get; set; }
        public string Remarks { get; set; }
        public short? ProcessCode { get; set; }
        public short? ItemSerialNumber { get; set; }
    }
}