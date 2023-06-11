using System.Collections.Generic;
using Transactiondetails.DBModels;

public class JobDespatch
{
    public JobDespatchMaster JobDespatchMaster { get; set; }
    public List<JobDespatchDetail> JobDespatchDetails { get; set; }

}