using System.Collections.Generic;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models
{
    public class JobReceipt
    {
        public JobRecieptMaster JobReceiptMaster { get; set; }
        public List<JobRecieptDetail> JobReceiptDetails { get; set; }

    }

    public class DatabaseResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }

    public class JobDespatch
    {
        public JobDespatchMaster JobDespatchMaster { get; set; }
        public List<JobDespatchDetail> JobDespatchDetails { get; set; }

    }
}