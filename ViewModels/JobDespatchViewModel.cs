using System;
using System.Collections.Generic;

namespace Transactiondetails.ViewModels
{
    public class JobDespatchViewModel
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public int? SerialNumber { get; set; }
        public Mode Mode { get; set; }
        public IEnumerable<AccountMasterVM> Accounts { get; set; }
        public IEnumerable<ProcessMasterVM> Processes { get; set; }
        public List<JobDespatchDetailViewModel> JobDespatchDetails { get; set; }
        public JobDespatchViewModel()
        {
            var JobDespatchDetails = new List<JobDespatchDetailViewModel>();
        }
    }
    
    public class JobDespatchDetailViewModel
    {
        public string CustomrName { get; set; }
        public int SerialNumber { get; set; }
        public DateTime ReferenceDate { get; set; }
        public Int16 ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string BillingType { get; set; }
        public Int16 ItemSerialNumber { get; set; }
        public Int16 PacketNumber { get; set; }
        public double ItemPieces { get; set; }
        public double ItemCarats { get; set; }
        public double ItemLines { get; set; }
        public double BalItemPieces { get; set; }
        public double BalItemCarats { get; set; }
        public double BalItemLines { get; set; }
        public double WeightLoss { get; set; }
        public string Status{ get; set; }
        public string BillingUnit { get; set; }
        public double BillingQuantity { get; set; }
        public double NoChargeQuantity { get; set; }
        public double Rate { get; set; }
        public string Remarks { get; set; }
    }

}
