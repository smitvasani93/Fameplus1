using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Transactiondetails.DBModels;
using Transactiondetails.Models.Utility;

public class AccountDataLayer
{
    public List<AccountMaster> GetAccounts(string companyCode, string branchCode, string FYear)
    {
        var accountMasterList = new List<AccountMaster>();
        //if Data Exists in session
        if (HttpContext.Current.Session["Accounts"] != null)
        {
            return (List<AccountMaster>)HttpContext.Current.Session["Accounts"];
        }
        using (CompanyDBContext db = new CompanyDBContext(companyCode))
        {
            var pFYear = new SqlParameter("@FinancialYearCode", FYear);

            accountMasterList = db.Database.SqlQuery<AccountMaster>("exec spGetCustomerAccount @FinancialYearCode", pFYear).ToList();

            HttpContext.Current.Session["Accounts"] = accountMasterList;
        }

        return accountMasterList;
    }

    public List<AccountMaster> GetSalesAccounts(string companyCode, string branchCode, string FYear)
    {
        var accountMasterList = new List<AccountMaster>();
        //if Data Exists in session
        if (HttpContext.Current.Session["SalesAccounts"] != null)
        {
            return (List<AccountMaster>)HttpContext.Current.Session["SalesAccounts"];
        }
        using (CompanyDBContext db = new CompanyDBContext(companyCode))
        {
            var pFYear = new SqlParameter("@FinancialYearCode", FYear);

            accountMasterList = db.Database.SqlQuery<AccountMaster>("exec spGetSalesAccount @FinancialYearCode", pFYear).ToList();

            HttpContext.Current.Session["SalesAccounts"] = accountMasterList;
        }

        return accountMasterList;
    }

    public List<AccountMaster> GetDespatchAccounts(string companyCode, string branchCode, string FYear)
    {
        var accountMasterList = new List<AccountMaster>();
        using (CompanyDBContext db = new CompanyDBContext(companyCode))
        {
            var pFYear = new SqlParameter("@FinancialYearCode", FYear);

            accountMasterList = db.Database.SqlQuery<AccountMaster>("exec spGetDespatchAccount @FinancialYearCode", pFYear).ToList();
        }

        return accountMasterList;
    }

    public List<AccountMaster> GetBillAccounts(string companyCode, string branchCode, string FYear)
    {
        var accountMasterList = new List<AccountMaster>();
        using (CompanyDBContext db = new CompanyDBContext(companyCode))
        {
            var pFYear = new SqlParameter("@FinancialYearCode", FYear);

            accountMasterList = db.Database.SqlQuery<AccountMaster>("exec spGetBillAccount @FinancialYearCode", pFYear).ToList();
        }

        return accountMasterList;
    }
}