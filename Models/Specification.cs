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
    
    public partial class Specification
    {
        public short SpecificationCode { get; set; }
        public string SpecificationName { get; set; }
        public bool DeletedFlag { get; set; }
        public Nullable<short> UserCode { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<short> ModiUserCode { get; set; }
        public Nullable<System.DateTime> ModiDate { get; set; }
    }
}
