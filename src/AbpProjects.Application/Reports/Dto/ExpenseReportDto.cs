using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    public class ExpenseReportDto
    {
        public ExpenseReportDto()
        {

            mFiancialYearDropdownList = new List<FiancialYearDropdown>();
           
            
        }
        public bool isCollapsed { get; set; }
        public int? categoryid { get; set; }
        public string Category { get; set; }
        public decimal? monthtotal { get; set; }
        public string FinancialYear { get; set; }
        public List<string> MonthYear { get; set; }
        public List<monthdetail> months { get; set; }
        public List<subcategorydetail> subcatdetaillist { get; set; }
        public List<FiancialYearDropdown> mFiancialYearDropdownList { get; set; }
        public List<subcategorynamelist> subname { get; set; }
        public List<subcategorydetaillist> finaltotal { get; set; }
    }
    public class monthdetail
    {
        public decimal? expense { get; set; }
        public string monthyr { get; set; }


    }

    public class subcategorydetail
    {
        public subcategorydetail()
            {
            st=new List<subcategorydetaillist>();
    }
        public string subcategory { get; set; }
        public int subcategoryId { get; set; }
        public decimal? amount { get; set; }
        public string monthyr { get; set; }
        public List<subcategorydetaillist> st { get; set; }
       
    }
    public class subcategorydetaillist
    {
        public decimal? amount { get;set;}
    }
    public class subcategorynamelist
    {
        public string subcategory { get; set; }
        public int subcategoryId { get; set; }
        
    }



}
