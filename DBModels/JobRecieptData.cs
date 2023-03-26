using System.Collections.Generic;

namespace Transactiondetails.DBModels
{
    public class JobRecieptData
    {
        public List<JobRecieptMaster> JobReciepts { get; set; }

        public List<AccountMaster> Accounts { get; set; }
        public List<ProcessMaster> Processes { get; set; }


        public JobRecieptData()
        {
            JobReciepts = new List<JobRecieptMaster>();
            Accounts = new List<AccountMaster>();
            Processes = new List<ProcessMaster>();
        }
    }
}