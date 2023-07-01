using System;
using System.Collections.Generic;

namespace Transactiondetails.ViewModels
{
    public class JobBillingViewModel
    {
        public string AccountCode { get; set; }
        public string SalesAccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public int? SerialNumber { get; set; }
        public int CreditDays { get; set; }
        public int RefNo { get; set; }

        public string Remarks { get; set; }
        public double BillAmmount { get; set; }
        public double CashAmount { get; set; }
        public double SGST { get; set; }
        public double CGST { get; set; }
        public double IGST { get; set; }
        public double NetAmount { get; set; }
        public double FinalAmount { get; set; }
        public double TotalTax { get; set; }
        public Mode Mode { get; set; }
        public IEnumerable<AccountMasterVM> Accounts { get; set; }
        public IEnumerable<AccountMasterVM> SalesAccounts { get; set; }
        public IEnumerable<ProcessMasterVM> Processes { get; set; }
        public List<JobBillingDetailViewModel> JobBillingDetails { get; set; }
        public JobBillingViewModel()
        {
            JobBillingDetails = new List<JobBillingDetailViewModel>();
        }
    }
}
