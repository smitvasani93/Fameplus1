using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Transactiondetails.DBModels;

namespace Transactiondetails.Models.Utility
{
    public class DBUtility
    {
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
            //return isSuccessFulllogin;
            return true;
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



    }

}