using System;
using System.Collections.Generic;

namespace Transactiondetails.ViewModels
{
    public class JobBillingViewModel
    {
        public string AccountCode { get; set; }
        public string SalesAccountCode { get; set; }
        public string AccountName { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public int? SerialNumber { get; set; }
        public int CreditDays { get; set; }
        public string Remarks { get; set; }
        public double BillAmount { get; set; }
        public double CashAmount { get; set; }
        public double NetAmount { get; set; }
        public double TotalTax { get; set; }
        public double PostingAmount { get; set; }
        public double PostingAmount2 { get; set; }
        public double PostingAmount3 { get; set; }
        public double RoundedAmount { get; set; }
        public double FinalAmount { get; set; }
        public double Addless1 { get; set; }
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
