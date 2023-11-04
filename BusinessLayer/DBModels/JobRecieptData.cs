using System.Collections.Generic;

namespace Transactiondetails.DBModels
{
    public class JobBillingData
    {
        public JobBillingMaster JobBillingMast { get; set; }
        public List<JobBillingDetail> JobBillingDets { get; set; }

        public JobBillingData()
        {
            JobBillingDets = new List<JobBillingDetail>();
        }
    }
}