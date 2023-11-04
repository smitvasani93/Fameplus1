using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BusinessLayer.DBModels;
using Transactiondetails.Models.Utility;

namespace BusinessLayer.Models.Utility
{
    public class DBUtility
    {
        static readonly object cacheLock = new object();

        public bool CheckLogin(string userName, string password, out int userId)
        {
            bool isSuccessFulllogin = false;
            userId = 0;
            using (GenDBContext db = new GenDBContext())
            {
                var puserName = new SqlParameter("@UserName", userName);
                var ppassword = new SqlParameter("@Password", password);

                var data = db.Database.SqlQuery<System3>("exec sp_CheckLogin @UserName , @Password", puserName, ppassword).FirstOrDefault();

                if (data != null)
                {
                    if (data.Field2.Trim().ToLower() == userName.ToLower())
                    {
                        userId = data.Field1;
                        isSuccessFulllogin = true;
                    }
                }
            }
            return isSuccessFulllogin;
        }

        public List<Company> CompanyList()
        {
            var companyList = new List<Company>();
            using (GenDBContext db = new GenDBContext())
            {
                //Call Stored Procedure to dump the xml to database
                companyList = db.Database.SqlQuery<Company>("exec sp_CompanyNameViewLoad").ToList();
            }

            return companyList;
        }

        public List<FYear> FYearList(string companyCode)
        {
            var fYearList = new List<FYear>();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            //using (GenDBContext db = new GenDBContext())
            {
                //Call Stored Procedure to dump the xml to database
                fYearList = db.Database.SqlQuery<FYear>("exec sp_FinancialViewLoad")?.ToList();
            }

            return fYearList;
        }

        public List<Branch> BranchList(string companyCode)
        {
            var branchList = new List<Branch>();
            using (CompanyDBContext db = new CompanyDBContext(companyCode))
            //using (GenDBContext db = new GenDBContext())
            {
                //Call Stored Procedure to dump the xml to database
                branchList = db.Database.SqlQuery<Branch>("exec sp_BranchNameViewLoad").ToList();
            }

            return branchList;
        }

        public List<ProcessMaster> GetProcesses()
        {
            var processMasterList = new List<ProcessMaster>();

            lock (cacheLock)
            {
                using (GenDBContext db = new GenDBContext())
                {
                    processMasterList = db.Database.SqlQuery<ProcessMaster>("exec spGetProcessMaster").ToList();
                }
            }

            return processMasterList;
        }

        public IEnumerable<ProcessDetail> GetProcessDetails()
        {
            var processDetailList = new List<ProcessDetail>();
            lock (cacheLock)
            {
                using (GenDBContext db = new GenDBContext())
                {
                    processDetailList = db.Database.SqlQuery<ProcessDetail>("exec spGetProcessDetail").ToList();
                }
            }

            return processDetailList;
        }

    }

}