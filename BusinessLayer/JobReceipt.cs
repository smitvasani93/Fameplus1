using System.Collections.Generic;
using BusinessLayer.DBModels;

namespace BusinessLayer.Models
{
    public class JobReceipt
    {
        public JobRecieptMaster JobReceiptMaster { get; set; }
        public List<JobRecieptDetail> JobReceiptDetails { get; set; }

    }
}