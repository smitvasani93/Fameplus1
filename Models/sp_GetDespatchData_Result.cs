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
    
    public partial class sp_GetDespatchData_Result
    {
        public int SerialNumber { get; set; }
        public short ItemSerialNumber { get; set; }
        public short ProcessCode { get; set; }
        public short PacketNumber { get; set; }
        public Nullable<double> ItemPieces { get; set; }
        public Nullable<double> ItemCarats { get; set; }
        public Nullable<double> ItemLines { get; set; }
        public Nullable<double> JDItemPieces { get; set; }
        public Nullable<double> JDItemCarats { get; set; }
        public Nullable<double> JDItemLines { get; set; }
        public Nullable<double> WeightLoss { get; set; }
        public string PacketStatus { get; set; }
        public Nullable<double> BillingQuantity { get; set; }
        public Nullable<double> NoChargeQuantity { get; set; }
        public Nullable<double> BillingRate { get; set; }
        public string Remarks { get; set; }
    }
}
