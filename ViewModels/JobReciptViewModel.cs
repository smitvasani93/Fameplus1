using System;
using System.Collections.Generic;
using Transactiondetails.DBModels;

namespace Transactiondetails.ViewModels
{
    public class JobReciptVM    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime ReferenceDate { get; set; }
        public int SerialNumber { get; set; }

        public IEnumerable<ProcessMasterVM> Processes { get; set; }
        public IEnumerable<AccountMasterVM> Accounts { get; set; }
    }

    public class ProcessMasterVM
    {
        public Int16 ProcessCode { get; set; }
        public string ProcessName { get; set; }
    }

    public class AccountMasterVM
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
    }
}