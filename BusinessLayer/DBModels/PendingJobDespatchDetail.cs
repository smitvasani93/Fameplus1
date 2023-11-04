using System;

namespace Transactiondetails.DBModels
{
    public class PendingJobDespatchDetail
    {
        public int SerialNumber { get; set; }
        public DateTime JDReferenceDate { get; set; }
        public DateTime JRReferenceDate { get; set; }
        public Int16 ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string BillingType { get; set; }
        public Int16 ItemSerialNumber { get; set; }
        public Int16 JDItemSerialNumber { get; set; }
        public Int16 PacketNumber { get; set; }
        public double ItemPieces { get; set; }
        public double ItemCarats { get; set; }
        public double ItemLines { get; set; }
        public double JDItemPieces { get; set; }
        public double JDItemCarats { get; set; }
        public double JDItemLines { get; set; }
        public double JDBillingQuantity { get; set; }
        public double BillingQuantity { get; set; }
        public double BillingRate { get; set; }
        public double TotalAmount { get; set; }
        public double NetAmount { get; set; }
        public double Taxes { get; set; }
        public double NoChargeQuantity { get; set; }
    }
}
