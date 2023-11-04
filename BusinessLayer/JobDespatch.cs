using System.Collections.Generic;
using BusinessLayer.DBModels;

public class JobDespatch
{
    public JobDespatchMaster JobDespatchMaster { get; set; }
    public List<JobDespatchDetail> JobDespatchDetails { get; set; }

}