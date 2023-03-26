using System.Collections.Generic;

namespace Transactiondetails.Models
{
    public class JobReceipt
    {
        public JobReceiptMa JobReceiptMaster { get; set; }
        public List<JobReceiptDet> JobReceiptDetails { get; set; }

    }

    public class DatabaseResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}