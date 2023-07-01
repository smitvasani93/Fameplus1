using System;

namespace Transactiondetails.ViewModels
{
    public class JobBillingDetailViewModel
    {
        public int SerialNumber { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string CustomrName { get; set; }
        public int JDSerialNumber { get; set; }
        public Int16 JDItemSerialNumber { get; set; }
        public Int16 ItemSerialNumber { get; set; }
        public DateTime JDReferenceDate { get; set; }
        public DateTime JRReferenceDate { get; set; }
        public Int16 ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string BillingType { get; set; }
        public Int16 PacketNumber { get; set; }
        public double ItemPieces { get; set; }
        public double ItemCarats { get; set; }
        public double ItemLines { get; set; }
        public double JDItemPieces { get; set; }
        public double JDItemCarats { get; set; }
        public double JDItemLines { get; set; }
        public double WeightLoss { get; set; }
        public string Status { get; set; }
        public string BillingUnit { get; set; }
        public double JDBillingQuantity { get; set; }
        public double BillingQuantity { get; set; }
        public double NoChargeQuantity { get; set; }
        public double BillingRate { get; set; }
        public string BankCashFlag { get; set; }
        public string DiscountPerAmt { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountRate { get; set; }
        public double DiscountAmount { get; set; }
        public double BillingAmount { get; set; }
        public double Addless1 { get; set; }
        public double Addless2 { get; set; }
        public double Addless3 { get; set; }
        public double NetAmount { get; set; }
        public string Remarks { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double IGST { get; set; }
        public double Taxes { get; set; }
        public double PaymentCB { get; set; }
 
    }
}
