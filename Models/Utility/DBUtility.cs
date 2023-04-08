using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models.Utility
{
    public class DBUtility
    {
        static readonly object cacheLock = new object();

        public bool CheckLogin(string userName, string password)
        {
            bool isSuccessFulllogin = false;
            using (GenDBContext db = new GenDBContext())
            {
                var puserName = new SqlParameter("@UserName", userName);
                var ppassword = new SqlParameter("@Password", password);

                var data = db.Database.SqlQuery<System3>("exec sp_CheckLogin @UserName , @Password", puserName, ppassword).FirstOrDefault();

                if (data != null)
                {
                    if (data.Field2.Trim().ToLower() == userName.ToLower())
                    {
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

        //public List<AccountMaster> GetAccounts()
        //{
        //    var accountMasterList = new List<AccountMaster>();

        //    if (HttpContext.Current.Cache["Accounts"] != null)
        //    {
        //        accountMasterList = (List<AccountMaster>)HttpContext.Current.Cache["Accounts"];
        //        return accountMasterList;
        //    }
        //    using (GenDBContext db = new GenDBContext())
        //    {
        //        accountMasterList = db.Database.SqlQuery<AccountMaster>("exec spGetAccount").ToList();
        //    }

        //    return accountMasterList;
        //}
        public List<ProcessMaster> GetProcesses()
        {

            var processMasterList = new List<ProcessMaster>();

            if (HttpContext.Current.Cache["Process"] != null)
            {
                processMasterList = (List<ProcessMaster>) HttpContext.Current.Cache["Process"];
                return processMasterList;
            }
            lock (cacheLock)
            {

                using (GenDBContext db = new GenDBContext())
                {
                    processMasterList = db.Database.SqlQuery<ProcessMaster>("exec spGetProcessMaster").ToList();

                    if (processMasterList != null)
                    {
                        HttpContext.Current.Cache["Process"] = processMasterList;
                    }
                }
            }

            return processMasterList;
        }
    }

}