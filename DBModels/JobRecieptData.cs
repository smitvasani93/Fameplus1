using System.Collections.Generic;

namespace Transactiondetails.DBModels
{
    public class JobRecieptData
    {
        public List<JobRecieptMaster> JobRecieptMasts { get; set; }
        public List<JobRecieptDetail> JobRecieptDets { get; set; }
 
 
        //public List<AccountMaster> Accounts { get; set; }
        //public List<ProcessMaster> Processes { get; set; }


        public JobRecieptData()
        {
            JobRecieptMasts = new List<JobRecieptMaster>();
            JobRecieptDets = new List<JobRecieptDetail>();
            //  Accounts = new List<AccountMaster>();
            // Processes = new List<ProcessMaster>();
        }
    }
}