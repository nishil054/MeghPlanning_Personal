using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    public class SalesReport
    {
        public SalesReport()
        {
            mProjectWiseSalesList = new List<ProjectWiseSales>();
            mServiceWiseSales = new List<ServiceWiseSales>();
            mFiancialYearDropdownList = new List<FiancialYearDropdown>();
            mCollection_Report = new List<Collection_Report>();
            mInoiceReport = new List<InoiceReport>();


        }
        public string ProjectName { get; set; }
        public string ProjectTypeName { get; set; }

        public string FinancialYear { get; set; }

        public string MonthYear { get; set; }
        public decimal totalsales { get; set; }
        public decimal totalYearsales { get; set; }
        public decimal totalFinancialYrsales { get; set; }

        public List<ProjectWiseSales> mProjectWiseSalesList { get; set; }
        public List<ServiceWiseSales> mServiceWiseSales { get; set; }

        public List<FiancialYearDropdown> mFiancialYearDropdownList { get; set; }
        public List<Collection_Report> mCollection_Report { get; set; }
        public List<InoiceReport> mInoiceReport { get; set; }


    }

    public class GSTData
    {
        public GSTData()
        {
            companynameList =new List<GSTAmountcompanynameList> ();
            companynames = new List<string>();
            companytotals = new List<string>();
        }
        public string monthyear { get; set; }
        public decimal gstAmount { get; set; }
        public string companyname { get; set; }
        public decimal totalrowwise { get; set; }
        public decimal totalcolumnwise { get; set; }
        public decimal financialyearId { get; set; }
        public List<GSTAmountcompanynameList> companynameList { get; set; }
        public List<string> companynames { get; set; }
        public List<string> companytotals { get; set; }
    }
    public class ProjectWiseSales
    {
        public string ProjectName { get; set; }
        public decimal totalsales { get; set; }


    }
    public class GSTAmountcompanynameList
    {
        public string companyname { get; set; }
        public decimal gstAmount { get; set; }
        public decimal columnTotal { get; set; }
    }
    public class ServiceWiseSales
    {
        public string ServiceName { get; set; }
        public decimal totalsales { get; set; }
        public int NoOfServcies { get; set; }

    }
    public class FiancialYearDropdown
    {
        public string text { get; set; }
        public string value { get; set; }

    }
    public class Collection_Report
    {
        public string PymtRecd { get; set; }
        public string Mode { get; set; }
        public string RcptDate { get; set; }
        public string ClientName { get; set; }
    }
    public class InoiceReport
    {
        public string totalbillamount { get; set; }
        public string billdate { get; set; }
        public string ClientName { get; set; }
    }
    public class ProductionReport
    {
        public string Name { get; set; }
        public DateTime Invoicedate { get; set; }
        public decimal InvoiceAmount { get; set; }
    }
}
