using System.Collections.Generic;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models
{
    public class JobBill
    {
        public JobBillingMaster JobBillingMaster { get; set; }
        public List<JobBillingDetail> JobBillingDetails { get; set; }

    }
}