using System;

namespace Transactiondetails.ViewModels
{
    public class JobDespatchViewModel
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public int? SerialNumber { get; set; }

    }
}
