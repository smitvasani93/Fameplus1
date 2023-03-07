using System.Collections.Generic;
using Transactiondetails.DBModels;

namespace Transactiondetails.ViewModels
{
    public class CompanyViewModel
    {
        public string Company { get; set; }
        public string FYear { get; set; }
        public string Branch { get; set; }

        public List<Company> Companys { get; set; }
    }
}