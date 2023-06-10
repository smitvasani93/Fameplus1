using System.Collections.Generic;

namespace Transactiondetails.DBModels
{
    public class JobBillingData
    {
        public List<JobBillingMaster> JobBillingMasts { get; set; }
        public List<JobBillingDetail> JobBillingDets { get; set; }

        public JobBillingData()
        {
            JobBillingMasts = new List<JobBillingMaster>();
            JobBillingDets = new List<JobBillingDetail>();
            //  Accounts = new List<AccountMaster>();
            // Processes = new List<ProcessMaster>();
        }
    }
}