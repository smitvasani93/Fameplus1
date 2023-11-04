using System;
namespace BusinessLayer.DBModels
{
    public class JobBillingDetail
    {
        public int SerialNumber { get; set; }
        public Int16 ItemSerialNumber { get; set; }
        public int JDSerialNumber { get; set; }
        public DateTime JDReferenceDate { get; set; }

        public Int16 JDItemSerialNumber { get; set; }
        public Int16 ProcessCode { get; set; }
        public string ProcessName { get; set; }

        public Int16 PacketNumber { get; set; }
        public double ItemPieces { get; set; }
        public double ItemCarats { get; set; }
        public double ItemLines { get; set; }
        public double Addless1 { get; set; }
        public string AccountCode { get; set; }
        public string Remarks { get; set; }
        public string BillingType { get; set; }

        public string BankCashFlag { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string SaleAccountCode { get; set; }
        public DateTime PostingDate { get; set; }
        public double BillingQuantity { get; set; }
        public double BillingAmount { get; set; }
        public string DiscountPerAmt { get; set; }
        public double DiscountRate { get; set; }
        public double DiscountAmount { get; set; }
        public double BillingRate { get; set; }
        public double NoChargeQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double NetAmount { get; set; }
    }
}
