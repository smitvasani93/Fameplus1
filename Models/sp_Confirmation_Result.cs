//------------------------------------------------------------------------------
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
    
    public partial class sp_Confirmation_Result
    {
        public int SerialNumber { get; set; }
        public short ItemSerialNumber { get; set; }
        public Nullable<System.DateTime> JDRefDate { get; set; }
        public Nullable<System.DateTime> JRRefDate { get; set; }
        public Nullable<double> JDItemPieces { get; set; }
        public Nullable<double> JDItemCarats { get; set; }
        public Nullable<double> JDItemLines { get; set; }
        public Nullable<double> JDBillingQuantity { get; set; }
        public Nullable<bool> ReWorkingFlag { get; set; }
        public decimal JDNoChargeQuantity { get; set; }
        public short ProcessCode { get; set; }
        public short PacketNumber { get; set; }
        public string ProcessName { get; set; }
        public string BillingType { get; set; }
        public string AccountName { get; set; }
        public string GroupField { get; set; }
        public string Field1Title { get; set; }
        public string Field1Name { get; set; }
        public Nullable<double> BillingRate { get; set; }
    }
}
