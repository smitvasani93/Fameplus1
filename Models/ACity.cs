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
    using System.Collections.Generic;
    
    public partial class ACity
    {
        public int CityCode { get; set; }
        public string CityName { get; set; }
        public string STDCode { get; set; }
        public Nullable<int> StateCode { get; set; }
        public Nullable<bool> DeletedFlag { get; set; }
        public Nullable<short> UserCode { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<short> ModiUserCode { get; set; }
        public Nullable<System.DateTime> ModiDate { get; set; }
    
        public virtual AState AState { get; set; }
    }
}
