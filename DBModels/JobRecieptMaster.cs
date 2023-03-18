using System;

namespace Transactiondetails.DBModels
{
    public class JobRecieptMaster
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime ReferenceDate { get; set; }
        public int SerialNumber { get; set; }
    }
}