using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Bills;
using AbpProjects.Company;
using AbpProjects.ExpenseCategories;
using AbpProjects.ExpenseEnteryForm;
using AbpProjects.FinancialYear;
using AbpProjects.gstdashboard;
using AbpProjects.InvoiceRequest;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.Opportunities;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.PerformaInvoices;
using AbpProjects.Productions;
using AbpProjects.Project;
using AbpProjects.Receipt;
using AbpProjects.Reports.Dto;
using AbpProjects.TimeSheet;
using AbpProjects.TimeSheet.Dto;
using AbpProjects.UserLoginDetails;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace AbpProjects.Reports
{
    [AbpAuthorize(PermissionNames.Pages_Reports)]
    public class ReportsAppService : AbpProjectsApplicationModule, IReportsAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<Projecttype_details> _projectDetailsRepository;
        private readonly IRepository<ProjectStatus> _projectstatusRepository;
        private readonly IRepository<invoicerequest> _invoiceRequestRepository;
        private readonly IRepository<ManageService> _manageserviceRepository;
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<BillPymtRecd> _billpymtRepository;
        private readonly IRepository<Production> _productionRepository;
        private readonly IRepository<financialYear> _financialRepository;
        private readonly IRepository<gstDashboard> _gstdataRepository;
        private readonly IRepository<company> _companyRepository;
        private readonly IRepository<UserLoginData> _UserLoginDataRepository;
        private readonly IRepository<PerformaInvoice> _performainvoiceRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IAbpSession _session;
        private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;
        private readonly IRepository<ExpenseSubcategory> _expenseSubCategoryRepository;
        private readonly IRepository<ExpenseEntry> _expenseEntryRepository;
        private readonly IRepository<OpportunityFollowUp> _OpportunityFollowUpRepository;
        private readonly IRepository<Opportunity> _OpportunityRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        public ReportsAppService(IRepository<User, long> userRepository,
            IRepository<timesheet> timesheetRepository,
            IRepository<project> projectRepository,
            IAbpSession session,
            IRepository<Projecttype_details> projectDetailsRepository,
            IRepository<ProjectStatus> projectstatusRepository, IRepository<invoicerequest> invoiceRequestRepository,
            IRepository<ManageService> manageserviceRepository, IRepository<Service> serviceRepository,
            IRepository<Bill> billRepository, IRepository<Client> clientRepository,
            IRepository<BillPymtRecd> billpymtRepository,
            IRepository<Production> productionRepository, IRepository<financialYear> financialRepository, IRepository<gstDashboard> gstdataRepository, IRepository<company> companyRepository,
            IRepository<UserLoginData> UserLoginDataRepository, IRepository<PerformaInvoice> performainvoiceRepository,
            IRepository<ExpenseCategory> expenseCategoryRepository, IRepository<ExpenseSubcategory> expenseSubCategoryRepository, IRepository<ExpenseEntry> expenseEntryRepository, IRepository<Opportunity> opportunityRepository, IRepository<OpportunityFollowUp> OpportunityFollowUpRepository
            ,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository
            )
        {
            _userRepository = userRepository;
            _timesheetRepository = timesheetRepository;
            _projectRepository = projectRepository;
            _projectDetailsRepository = projectDetailsRepository;
            _projectstatusRepository = projectstatusRepository;
            _session = session;
            _invoiceRequestRepository = invoiceRequestRepository;
            _serviceRepository = serviceRepository;
            _manageserviceRepository = manageserviceRepository;
            _clientRepository = clientRepository;
            _billRepository = billRepository;
            _billpymtRepository = billpymtRepository;
            _productionRepository = productionRepository;
            _financialRepository = financialRepository;
            _gstdataRepository = gstdataRepository;
            _companyRepository = companyRepository;
            _UserLoginDataRepository = UserLoginDataRepository;
            _expenseCategoryRepository = expenseCategoryRepository;
            _expenseSubCategoryRepository = expenseSubCategoryRepository;
            _expenseEntryRepository = expenseEntryRepository;
            _performainvoiceRepository = performainvoiceRepository;
            _OpportunityRepository = opportunityRepository;
            _OpportunityFollowUpRepository = OpportunityFollowUpRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public DataTable GetAllData(GetInputDto input)
        {
            //var firstDayOfMonth = new DateTime(input.Year, input.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var projectlist = (from a in _timesheetRepository.GetAll()
                               join b in _projectRepository.GetAll()
                               on a.ProjectId equals b.Id
                               where a.ProjectId == b.Id && a.UserId == input.UserId
                               group b by new
                               {
                                   a.ProjectId,
                                   b.ProjectName
                               } into g
                               select new Reports.Dto.ProjectDto
                               {
                                   ProjectId = g.Key.ProjectId,
                                   ProjectName = g.Key.ProjectName
                               }).ToList();

            //get datewise data
            var datewiselist = (from a in _timesheetRepository.GetAll()
                                where a.UserId == input.UserId && a.Date.Month == input.Month && a.Date.Year == input.Year
                                orderby a.Date
                                select new ReportsDto
                                {
                                    Id = a.Id,
                                    ProjectId = a.ProjectId,
                                    UserId = a.UserId,
                                    Description = a.Description,
                                    Hours = a.Hours,
                                    Date = a.Date,
                                }).ToList();
            //get Distinct date
            //var finalData = datewiselist.Select(x=>x.Date).Distinct().ToList();
            var finalData = (from d in datewiselist
                             select new
                             {
                                 Year = d.Date.Year,
                                 Month = d.Date.Month,
                                 Day = d.Date.Day
                             }).Distinct().ToList();

            //get projectname and description
            var descriptionlist = (from b in _timesheetRepository.GetAll()
                                   join a in _projectRepository.GetAll()
                                   on b.ProjectId equals a.Id
                                   where b.UserId == input.UserId && b.Date.Month == input.Month && b.Date.Year == input.Year
                                   select new ReportDetails
                                   {
                                       ProjectId = b.ProjectId,
                                       ProjectName = a.ProjectName,
                                       Description = b.Description,
                                       UserId = b.UserId,
                                       Date = b.Date,
                                   }).ToList();

            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("Date"); //date column

            for (int i = 0; i < projectlist.Count; i++)  //project column
            {
                dt.Columns.Add(projectlist[i].ProjectName);
            }
            dt.Columns.Add("Comment");   //comment column

            if (input.Type != 0)
            {
                foreach (var item in finalData)
                {
                    dr = dt.NewRow();
                    DateTime dDate = new DateTime();
                    var dateConvert = item.Year + "/" + item.Month + "/" + item.Day;
                    dDate = Convert.ToDateTime(dateConvert);

                    dr["Date"] = String.Format("{0:dd/MM/yyyy}", dDate); //date row


                    for (int i = 0; i < projectlist.Count; i++)
                    {
                        int projectcode = projectlist[i].ProjectId;
                        var hourdata = _timesheetRepository.GetAll().Where(e => e.ProjectId == projectcode && e.UserId == input.UserId && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) && e.Date.Month == input.Month && e.Date.Year == input.Year).Select(e => e.Hours).DefaultIfEmpty(0).Sum();

                        var projectname = descriptionlist.Where(e => e.ProjectId == projectcode && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) && e.Date.Month == input.Month && e.Date.Year == input.Year).ToList();
                        string strProj = "";
                        if (projectname.Count > 0)
                        {
                            strProj = projectname.Select(x => x.ProjectName).FirstOrDefault();
                            dr["Comment"] += strProj + " : ";
                        }

                        var description = descriptionlist.Where(e => e.ProjectId == projectcode && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) /*&& e.Date == item.Date */&& e.Date.Month == input.Month && e.Date.Year == input.Year).ToList();

                        if (description.Count > 0)
                        {
                            var strDes = description.Select(x => x.Description).ToList();
                            string str = "";
                            foreach (var itemDes in strDes)
                            {
                                str += itemDes + ", " + "\r\n" + System.Environment.NewLine;
                            }
                            str = str.Remove(str.ToString().LastIndexOf(','));
                            dr["Comment"] += str + ", ";
                            dr["Comment"] = dr["Comment"].ToString().Remove(dr["Comment"].ToString().LastIndexOf(',')) + System.Environment.NewLine;

                        }

                        dr[projectlist[i].ProjectName] = hourdata.ToString(); //Hours rows

                    }

                    dt.Rows.Add(dr);
                }
            }
            else
            {
                foreach (var item in finalData)
                {
                    dr = dt.NewRow();
                    DateTime dDate = new DateTime();
                    var dateConvert = item.Year + "/" + item.Month + "/" + item.Day;
                    dDate = Convert.ToDateTime(dateConvert);

                    dr["Date"] = String.Format("{0:dd/MM/yyyy}", dDate); //date row

                    for (int i = 0; i < projectlist.Count; i++)
                    {
                        int projectcode = projectlist[i].ProjectId;
                        var hourdata = _timesheetRepository.GetAll().Where(e => e.ProjectId == projectcode && e.UserId == input.UserId && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) && e.Date.Month == input.Month && e.Date.Year == input.Year).Select(e => e.Hours).DefaultIfEmpty(0).Sum();

                        var projectname = descriptionlist.Where(e => e.ProjectId == projectcode && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) && e.Date.Month == input.Month && e.Date.Year == input.Year).ToList();
                        string strProj = "";
                        if (projectname.Count > 0)
                        {
                            strProj = projectname.Select(x => x.ProjectName).FirstOrDefault();
                            dr["Comment"] += "<b>" + strProj + "</b>" + " : ";
                        }

                        var description = descriptionlist.Where(e => e.ProjectId == projectcode && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) /*&& e.Date == item.Date */&& e.Date.Month == input.Month && e.Date.Year == input.Year).ToList();

                        if (description.Count > 0)
                        {
                            var strDes = description.Select(x => x.Description).ToList();
                            string str = "";
                            foreach (var itemDes in strDes)
                            {
                                str += itemDes + ", " + "<br/>";
                            }
                            str = str.Remove(str.ToString().LastIndexOf(','));
                            dr["Comment"] += str + ", ";
                            dr["Comment"] = dr["Comment"].ToString().Remove(dr["Comment"].ToString().LastIndexOf(',')) + "<br/>";
                        }

                        dr[projectlist[i].ProjectName] = hourdata.ToString(); //Hours rows

                    }
                    dt.Rows.Add(dr);
                }


            }
            return dt;

        }

        static string getFullName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat
                .GetAbbreviatedMonthName(month);
        }
        public List<MonthDto> GetMonth()
        {

            List<MonthDto> MonthList = new List<MonthDto>();
            for (int i = 1; i <= 12; i++)
            {
                MonthList.Add(new MonthDto { Id = i, MonthName = getFullName(i) });
            }
            return MonthList;
        }
        public List<Reports.Dto.ProjectDto> GetProjects(GetInputDto input)
        {
            var projectlist = (from a in _timesheetRepository.GetAll()
                               join b in _projectRepository.GetAll()
                               on a.ProjectId equals b.Id
                               where a.ProjectId == b.Id && a.UserId == input.UserId
                               group b by new
                               {
                                   a.ProjectId,
                                   b.ProjectName
                               } into g
                               select new Reports.Dto.ProjectDto
                               {
                                   ProjectId = g.Key.ProjectId,
                                   ProjectName = g.Key.ProjectName
                               }).ToList();
            return projectlist;
        }
        public List<AllUserDto> GetUser()
        {
            var persons = (from a in _userRepository.GetAll()
                           where a.IsActive == true && a.UserName != "admin"
                           orderby a.UserName
                           select new AllUserDto
                           {
                               Id = (int)a.Id,
                               Name = a.Name + " " + a.Surname
                           }).ToList();
            return persons;
        }

        public List<YearDto> GetYear()
        {
            List<YearDto> obj = new List<YearDto>();
            var Years = from n in Enumerable.Range(0, 5)
                        select DateTime.Now.Year - n;
            int i = 1;
            foreach (var y in Years)
            {
                obj.Add(new YearDto { Id = i, Year = y.ToString() });
                i++;
            }
            return obj;
        }
        public List<StatusDto> GetStatus()
        {
            var statuslist = (from a in _projectstatusRepository.GetAll()
                              select new StatusDto
                              {
                                  Id = a.Id,
                                  Status = a.Status,
                              }).OrderBy(x => x.Status).ToList();
            return statuslist;
        }
        public PagedResultDto<StatsAmountDto> GetProjectStatsAmount(GetInputDto input)
        {
            // get actual cost
            var actualcost = _timesheetRepository.GetAll();


            //project list
            var projectlist = (from a in _projectRepository.GetAll()
                               join b in _projectstatusRepository.GetAll()
                               on a.Status equals b.Id
                               join c in _timesheetRepository.GetAll()
                               on a.Id equals c.ProjectId
                               where a.Price > 0
                               group new { a, b, c }
                               by new { pid = c.ProjectId } into gf
                               let unit = gf.FirstOrDefault()
                               select new StatsAmountDto
                               {
                                   Id = unit.a.Id,
                                   ProjectName = unit.a.ProjectName,
                                   ProjectCost = unit.a.Price,
                                   CompanyCost = (decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum(),
                                   CostPercentage = unit.a.Price > 0 ? ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum()) * 100 / unit.a.Price : 0,
                                   Profit = unit.a.Price > 0 ? ((unit.a.Price) - ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum())) : 0,
                                   ProfitPercentage = unit.a.Price > 0 ? (((unit.a.Price) - ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum())) * 100 / unit.a.Price) : 0,
                                   StatusId = unit.b.Id,
                                   Status = unit.b.Status,
                                   //AmtInPer = ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum()) * 100 / unit.a.Price >= 60 ? true : false,
                               }).Where(x => x.CostPercentage >= 60)
                               //.Where(x => x.ProjectCost > 0)
                               .WhereIf(input.Status.HasValue, s => s.StatusId == input.Status);
            var ccData = projectlist.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = projectlist.Count();
            return new PagedResultDto<StatsAmountDto>(ccCount, ccData.MapTo<List<StatsAmountDto>>());
        }
        public List<StatsAmountDto> ExportProjectStateAmount(GetInputDto input)
        {
            // get actual cost
            var actualcost = _timesheetRepository.GetAll();


            //project list
            var projectlist = (from a in _projectRepository.GetAll()
                               join b in _projectstatusRepository.GetAll()
                               on a.Status equals b.Id
                               join c in _timesheetRepository.GetAll()
                               on a.Id equals c.ProjectId
                               where a.Price > 0
                               group new { a, b, c }
                               by new { pid = c.ProjectId } into gf
                               let unit = gf.FirstOrDefault()
                               select new StatsAmountDto
                               {
                                   Id = unit.a.Id,
                                   ProjectName = unit.a.ProjectName,
                                   ProjectCost = unit.a.Price,
                                   CompanyCost = (decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum(),
                                   CostPercentage = unit.a.Price > 0 ? ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum()) * 100 / unit.a.Price : 0,
                                   Profit = unit.a.Price > 0 ? ((unit.a.Price) - ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum())) : 0,
                                   ProfitPercentage = unit.a.Price > 0 ? (((unit.a.Price) - ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum())) * 100 / unit.a.Price) : 0,
                                   StatusId = unit.b.Id,
                                   Status = unit.b.Status,
                               })
                                .Where(x => x.CostPercentage >= 60)
                               .WhereIf(input.Status.HasValue, s => s.StatusId == input.Status)
                               .OrderByDescending(s => s.CostPercentage)
                               .ToList();
            return projectlist;
        }

        #region Sales,OutStanding,Invoice,Production, Report
        public List<SalesReport> GetSalesReport(int year)
        {
            List<SalesReport> SalesReportList = new List<SalesReport>();
            List<string> FinancialMonthList = new List<string>();
            List<FiancialYearDropdown> mFiancialYearDropdownList = new List<FiancialYearDropdown>();
            string FinancialYear = "";

            if (year != null || year != 0)
            {
                DateTime _LoopDate1 = new DateTime(year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }
            else
            {
                DateTime _LoopDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }

            // Get financial year eg.2020-2021
            var finaMnthye = GetFinancialYear(FinancialYear);

            string[] financialYearList = finaMnthye.Split('-');

            DateTime _LoopDate = new DateTime(Convert.ToInt32(financialYearList[0]), 04, 01);
            DateTime FinStartDate = _LoopDate;
            string month = string.Empty;
            DateTime _EndDate = new DateTime(Convert.ToInt32(financialYearList[1]), 03, 31);
            DateTime FinEndDate = _EndDate;

            //Get Full finacial year e.g. apr-2021 to mar-2022
            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();
                month = d.MonthNames[_LoopDate.Month - 1];
                FinancialMonthList.Add(month.Substring(0, 3) + "-" + _LoopDate.Year.ToString());
                _LoopDate = _LoopDate.AddMonths(1);
            }

            //Bind dropdown with finacial year
            var startdrpFinancialYear = _projectDetailsRepository.GetAll()
                                                                 .OrderBy(x => x.CreationTime)
                                                                 .Select(x => x.CreationTime.Year)
                                                                 .FirstOrDefault();
            int GivenYear = startdrpFinancialYear;
            int j = DateTime.Now.Year - (GivenYear - 1);
            for (int i = j; i >= 0; i--)
            {
                int fy = DateTime.Now.Year - i;
                int fy1 = fy + 1;
                FiancialYearDropdown mFiancialYearDropdown = new FiancialYearDropdown();

                if (DateTime.Now.Date > Convert.ToDateTime(fy + "-03-31").Date)
                {
                    mFiancialYearDropdown.text = fy.ToString() + "-" + fy1.ToString();
                    mFiancialYearDropdown.value = fy.ToString();
                    mFiancialYearDropdownList.Add(mFiancialYearDropdown);
                }

            }

            var data1 = (from p in _projectRepository.GetAll()
                         join pd in _projectDetailsRepository.GetAll()
                         on p.Id equals pd.ProjectId
                         // where pd.CreationTime.Year == year
                         where pd.CreationTime >= FinStartDate && pd.CreationTime <= FinEndDate
                         select new { p.ProjectName, pd.Price, pd.CreationTime }).AsQueryable().Select(k => new { k.ProjectName, k.CreationTime.Month, k.CreationTime.Year, k.Price })
                           .GroupBy(x => new { x.Year, x.Month, x.ProjectName }, (key, group) => new
                           {
                               yr = key.Year,
                               mnth = key.Month,
                               tCharge = group.Sum(k => k.Price),
                               projectName = key.ProjectName
                           }).AsQueryable();

            foreach (var item in FinancialMonthList)
            {
                SalesReport mSalesReport = new SalesReport();
                decimal TotalYearsales = 0;
                foreach (var item1 in data1)
                {
                    string monthName = getAbbreviatedName(item1.mnth);
                    if (item.Contains(monthName))
                    {
                        ProjectWiseSales mProjectWiseSales = new ProjectWiseSales();

                        TotalYearsales = TotalYearsales + item1.tCharge;
                        //mSalesReport.MonthYear = item;
                        mProjectWiseSales.ProjectName = item1.projectName;
                        mProjectWiseSales.totalsales = item1.tCharge;
                        mSalesReport.mProjectWiseSalesList.Add(mProjectWiseSales);
                        mSalesReport.totalYearsales = TotalYearsales;
                    }
                    else
                    {
                        //mSalesReport.MonthYear = item;
                        mSalesReport.totalsales = 0;
                    }
                }

                mSalesReport.MonthYear = item;
                mSalesReport.FinancialYear = finaMnthye;
                mSalesReport.mFiancialYearDropdownList.AddRange(mFiancialYearDropdownList);
                mSalesReport.mFiancialYearDropdownList = mSalesReport.mFiancialYearDropdownList.OrderByDescending(x => x.value).ToList();
                SalesReportList.Add(mSalesReport);
            }

            return SalesReportList;
        }
        public List<SalesReport> GetSalesReport_Service(int year)
        {
            List<SalesReport> SalesReportList = new List<SalesReport>();
            List<string> FinancialMonthList = new List<string>();
            List<FiancialYearDropdown> mFiancialYearDropdownList = new List<FiancialYearDropdown>();
            string FinancialYear = "";

            if (year != null || year != 0)
            {
                DateTime _LoopDate1 = new DateTime(year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }
            else
            {
                DateTime _LoopDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }

            // Get financial year eg.2020-2021
            var finaMnthye = GetFinancialYear(FinancialYear);

            string[] financialYearList = finaMnthye.Split('-');

            DateTime _LoopDate = new DateTime(Convert.ToInt32(financialYearList[0]), 04, 01);
            DateTime FinStartDate = _LoopDate;
            string month = string.Empty;
            DateTime _EndDate = new DateTime(Convert.ToInt32(financialYearList[1]), 03, 31);
            DateTime FinEndDate = _EndDate;

            //Get Full finacial year e.g. apr-2021 to mar-2022
            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();
                month = d.MonthNames[_LoopDate.Month - 1];
                FinancialMonthList.Add(month.Substring(0, 3) + "-" + _LoopDate.Year.ToString());
                _LoopDate = _LoopDate.AddMonths(1);
            }

            //Bind dropdown with finacial year
            var startdrpFinancialYear = _invoiceRequestRepository.GetAll()
                                                                 .OrderBy(x => x.CreationTime)
                                                                 .Select(x => x.CreationTime.Year)
                                                                 .FirstOrDefault();
            int GivenYear = startdrpFinancialYear;
            int j = DateTime.Now.Year - (GivenYear - 1);

            for (int i = j; i >= 0; i--)
            {
                int fy = DateTime.Now.Year - i;
                int fy1 = fy + 1;
                FiancialYearDropdown mFiancialYearDropdown = new FiancialYearDropdown();

                if (DateTime.Now.Date > Convert.ToDateTime(fy + "-03-31").Date)
                {
                    mFiancialYearDropdown.text = fy.ToString() + "-" + fy1.ToString();
                    mFiancialYearDropdown.value = fy.ToString();
                    mFiancialYearDropdownList.Add(mFiancialYearDropdown);
                }
            }

            var data1 = (from i in _invoiceRequestRepository.GetAll()
                         join ms in _manageserviceRepository.GetAll()
                         on i.ServiceId equals ms.Id
                         join s in _serviceRepository.GetAll()
                         on ms.ServiceId equals s.Id
                         //where i.CreationTime.Year == year
                         where i.CreationTime >= FinStartDate && i.CreationTime <= FinEndDate
                         select new { s.Name, i.Amount, i.CreationTime }).AsQueryable()
                         .Select(k => new { k.Name, k.CreationTime.Month, k.CreationTime.Year, k.Amount })
                            .GroupBy(x => new { x.Year, x.Month, x.Name }, (key, group) => new
                            {
                                yr = key.Year,
                                mnth = key.Month,
                                totalsales = group.Sum(k => k.Amount),
                                ServiceName = key.Name,
                                NoOfServcies = group.Count()

                            }).AsQueryable();

            foreach (var item in FinancialMonthList)
            {
                SalesReport mSalesReport = new SalesReport();
                decimal TotalYearsales = 0;
                foreach (var item1 in data1)
                {
                    string monthName = getAbbreviatedName(item1.mnth);
                    if (item.Contains(monthName))
                    {
                        ServiceWiseSales mServiceWiseSales = new ServiceWiseSales();

                        TotalYearsales = TotalYearsales + item1.totalsales;
                        //mSalesReport.MonthYear = item;
                        mServiceWiseSales.ServiceName = item1.ServiceName;
                        mServiceWiseSales.NoOfServcies = item1.NoOfServcies;
                        mServiceWiseSales.totalsales = item1.totalsales;
                        mSalesReport.mServiceWiseSales.Add(mServiceWiseSales);
                        mSalesReport.totalYearsales = TotalYearsales;
                    }
                    else
                    {
                        //mSalesReport.MonthYear = item;
                        mSalesReport.totalsales = 0;
                    }
                }

                mSalesReport.MonthYear = item;
                mSalesReport.FinancialYear = finaMnthye;
                mSalesReport.mFiancialYearDropdownList.AddRange(mFiancialYearDropdownList);
                mSalesReport.mFiancialYearDropdownList = mSalesReport.mFiancialYearDropdownList.OrderByDescending(x => x.value).ToList();
                SalesReportList.Add(mSalesReport);
            }

            return SalesReportList;
        }

        public List<SalesReport> GetCollectionReport(int year)
        {
            List<SalesReport> SalesReportList = new List<SalesReport>();
            List<string> FinancialMonthList = new List<string>();
            List<FiancialYearDropdown> mFiancialYearDropdownList = new List<FiancialYearDropdown>();
            string FinancialYear = "";

            if (year != null || year != 0)
            {
                DateTime _LoopDate1 = new DateTime(year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }
            else
            {
                DateTime _LoopDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }

            // Get financial year eg.2020-2021
            var finaMnthye = GetFinancialYear(FinancialYear);

            string[] financialYearList = finaMnthye.Split('-');

            DateTime _LoopDate = new DateTime(Convert.ToInt32(financialYearList[0]), 04, 01);
            string month = string.Empty;
            DateTime _EndDate = new DateTime(Convert.ToInt32(financialYearList[1]), 03, 31);

            //Get Full finacial year e.g. apr-2021 to mar-2022
            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();
                month = d.MonthNames[_LoopDate.Month - 1];
                FinancialMonthList.Add(month.Substring(0, 3) + "-" + _LoopDate.Year.ToString());
                _LoopDate = _LoopDate.AddMonths(1);
            }

            //Bind dropdown with finacial year
            var startdrpFinancialYear = _billpymtRepository.GetAll().Where(x => x.RcptDate != null)
                                                                 .OrderBy(x => x.RcptDate)
                                                                 .Select(x => x.RcptDate.Value.Year)
                                                                 .FirstOrDefault();
            int GivenYear = startdrpFinancialYear;
            int j = DateTime.Now.Year - (GivenYear - 1);
            for (int i = j; i >= 0; i--)
            {
                int fy = DateTime.Now.Year - i;
                int fy1 = fy + 1;
                FiancialYearDropdown mFiancialYearDropdown = new FiancialYearDropdown();

                if (DateTime.Now.Date > Convert.ToDateTime(fy + "-03-31").Date)
                {
                    mFiancialYearDropdown.text = fy.ToString() + "-" + fy1.ToString();
                    mFiancialYearDropdown.value = fy.ToString();
                    mFiancialYearDropdownList.Add(mFiancialYearDropdown);
                }

            }
            int financStartYear = Convert.ToInt32(financialYearList[0]);
            var data1 = (from bpr in _billpymtRepository.GetAll()
                         join b in _billRepository.GetAll()
                         on bpr.BillNo equals b.BillNo
                         join cli in _clientRepository.GetAll()
                         on b.ClientID equals cli.Id
                         where bpr.RcptDate.Value.Year == financStartYear
                         select new { bpr.PymtRecd, cli.ClientName, bpr.RcptDate.Value }).AsQueryable()
                         .Select(k => new { k.PymtRecd, k.ClientName, k.Value.Month, k.Value.Year, })
                            .GroupBy(x => new { x.Year, x.Month, x.ClientName }, (key, group) => new
                            {
                                yr = key.Year,
                                mnth = key.Month,
                                tCharge = group.Sum(k => k.PymtRecd),
                                ClientName = key.ClientName,
                            }).ToList();

            foreach (var item in FinancialMonthList)
            {
                SalesReport mSalesReport = new SalesReport();
                decimal TotalYearsales = 0;
                if (data1.ToList().Count > 0)
                {
                    foreach (var item1 in data1)
                    {
                        string monthName = getAbbreviatedName(item1.mnth) + "-" + item1.yr;
                        if (item.Contains(monthName))
                        {
                            Collection_Report mCollectionReport = new Collection_Report();

                            TotalYearsales = TotalYearsales + item1.tCharge ?? 0;
                            mSalesReport.MonthYear = item;
                            mCollectionReport.ClientName = item1.ClientName;
                            mCollectionReport.PymtRecd = Convert.ToString(item1.tCharge);

                            mSalesReport.mCollection_Report.Add(mCollectionReport);
                            mSalesReport.totalYearsales = TotalYearsales;
                        }
                        else
                        {
                            mSalesReport.MonthYear = item;
                            mSalesReport.totalsales = 0;
                        }
                    }
                }
                else
                {
                    mSalesReport.MonthYear = item;
                    mSalesReport.totalsales = 0;
                }

                mSalesReport.FinancialYear = finaMnthye;
                mSalesReport.mFiancialYearDropdownList.AddRange(mFiancialYearDropdownList);
                mSalesReport.mFiancialYearDropdownList = mSalesReport.mFiancialYearDropdownList.OrderByDescending(x => x.value).ToList();
                SalesReportList.Add(mSalesReport);
            }

            return SalesReportList;
        }

        public List<SalesReport> GetInvoiceReport(int year)
        {
            List<SalesReport> SalesReportList = new List<SalesReport>();
            List<string> FinancialMonthList = new List<string>();
            List<FiancialYearDropdown> mFiancialYearDropdownList = new List<FiancialYearDropdown>();
            string FinancialYear = "";

            if (year != null || year != 0)
            {
                DateTime _LoopDate1 = new DateTime(year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }
            else
            {
                DateTime _LoopDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }

            // Get financial year eg.2020-2021
            var finaMnthye = GetFinancialYear(FinancialYear);

            string[] financialYearList = finaMnthye.Split('-');

            DateTime _LoopDate = new DateTime(Convert.ToInt32(financialYearList[0]), 04, 01);
            string month = string.Empty;
            DateTime _EndDate = new DateTime(Convert.ToInt32(financialYearList[1]), 03, 31);

            //Get Full finacial year e.g. apr-2021 to mar-2022
            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();
                month = d.MonthNames[_LoopDate.Month - 1];
                FinancialMonthList.Add(month.Substring(0, 3) + "-" + _LoopDate.Year.ToString());
                _LoopDate = _LoopDate.AddMonths(1);
            }



            //Bind dropdown with finacial year
            var startdrpFinancialYear = _billRepository.GetAll()
                                                                 .OrderByDescending(x => x.BillDate)
                                                                 .Select(x => x.BillDate.Value.Year)
                                                                 .FirstOrDefault();

            int GivenYear = startdrpFinancialYear;
            int j = DateTime.Now.Year - (GivenYear - 1);
            for (int i = j; i >= 0; i--)
            {
                int fy = DateTime.Now.Year - i;
                int fy1 = fy + 1;
                FiancialYearDropdown mFiancialYearDropdown = new FiancialYearDropdown();

                if (DateTime.Now.Date > Convert.ToDateTime(fy + "-03-31").Date)
                {
                    mFiancialYearDropdown.text = fy.ToString() + "-" + fy1.ToString();
                    mFiancialYearDropdown.value = fy.ToString();
                    mFiancialYearDropdownList.Add(mFiancialYearDropdown);
                }

            }
            int financStartYear = Convert.ToInt32(financialYearList[0]);
            int financEndYear = Convert.ToInt32(financialYearList[1]);

            var data1 = (from b in _billRepository.GetAll()
                         join cli in _clientRepository.GetAll()
                         on b.ClientID equals cli.Id
                         where b.BillDate.Value.Year <= financEndYear
                         select new { b.totalbillamount, cli.ClientName, b.BillDate.Value }).AsQueryable()
                         .Select(k => new { k.totalbillamount, k.ClientName, k.Value.Month, k.Value.Year, })
                            .GroupBy(x => new { x.Year, x.Month, x.ClientName }, (key, group) => new
                            {
                                yr = key.Year,
                                mnth = key.Month,
                                tCharge = group.Sum(k => k.totalbillamount),
                                ClientName = key.ClientName,
                            }).AsQueryable();

            foreach (var item in FinancialMonthList)
            {
                string[] monthlist = item.Split('-');

                SalesReport mSalesReport = new SalesReport();
                decimal TotalYearsales = 0;
                if (data1.ToList().Count > 0)
                {
                    foreach (var item1 in data1)
                    {
                        string monthName = getAbbreviatedName(item1.mnth);
                        string yr = Convert.ToString(item1.yr);
                        if (monthlist[0].Contains(monthName) && monthlist[1].Contains(yr))
                        {
                            InoiceReport mCollectionReport = new InoiceReport();

                            TotalYearsales = TotalYearsales + item1.tCharge ?? 0;
                            mSalesReport.MonthYear = item;
                            mCollectionReport.ClientName = item1.ClientName;
                            mCollectionReport.totalbillamount = Convert.ToString(item1.tCharge);

                            mSalesReport.mInoiceReport.Add(mCollectionReport);
                            mSalesReport.totalYearsales = TotalYearsales;
                        }
                        else
                        {
                            mSalesReport.MonthYear = item;
                            mSalesReport.totalsales = 0;
                        }
                    }
                }
                else
                {
                    mSalesReport.MonthYear = item;
                    mSalesReport.totalsales = 0;
                }

                mSalesReport.FinancialYear = finaMnthye;
                mSalesReport.mFiancialYearDropdownList.AddRange(mFiancialYearDropdownList);
                mSalesReport.mFiancialYearDropdownList = mSalesReport.mFiancialYearDropdownList.OrderByDescending(x => x.value).ToList();
                SalesReportList.Add(mSalesReport);
            }

            return SalesReportList;
        }

        public List<SalesReport> GetProductionReport(int year)
        {
            List<SalesReport> SalesReportList = new List<SalesReport>();
            List<string> FinancialMonthList = new List<string>();
            List<FiancialYearDropdown> mFiancialYearDropdownList = new List<FiancialYearDropdown>();
            string FinancialYear = "";

            if (year != null || year != 0)
            {
                DateTime _LoopDate1 = new DateTime(year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }
            else
            {
                DateTime _LoopDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }

            // Get financial year eg.2020-2021
            var finaMnthye = GetFinancialYear(FinancialYear);

            string[] financialYearList = finaMnthye.Split('-');

            DateTime _LoopDate = new DateTime(Convert.ToInt32(financialYearList[0]), 04, 01);
            string month = string.Empty;
            DateTime _EndDate = new DateTime(Convert.ToInt32(financialYearList[1]), 03, 31);

            //Get Full finacial year e.g. apr-2021 to mar-2022
            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();
                month = d.MonthNames[_LoopDate.Month - 1];
                FinancialMonthList.Add(month.Substring(0, 3) + "-" + _LoopDate.Year.ToString());
                _LoopDate = _LoopDate.AddMonths(1);
            }

            //Bind dropdown with finacial year
            var startdrpFinancialYear = _productionRepository.GetAll()
                                                                 .OrderBy(x => x.Invoicedate)
                                                                 .Select(x => x.Invoicedate.Year)
                                                                 .FirstOrDefault();
            int GivenYear = startdrpFinancialYear;
            int j = DateTime.Now.Year - (GivenYear - 1);
            for (int i = j; i >= 0; i--)
            {
                int fy = DateTime.Now.Year - i;
                int fy1 = fy + 1;
                FiancialYearDropdown mFiancialYearDropdown = new FiancialYearDropdown();

                if (DateTime.Now.Date > Convert.ToDateTime(fy + "-03-31").Date)
                {
                    mFiancialYearDropdown.text = fy.ToString() + "-" + fy1.ToString();
                    mFiancialYearDropdown.value = fy.ToString();
                    mFiancialYearDropdownList.Add(mFiancialYearDropdown);
                }

            }
            int financStartYear = Convert.ToInt32(financialYearList[0]);
            int financEndYear = Convert.ToInt32(financialYearList[1]);

            var data1 = (from p in _productionRepository.GetAll()
                         join pr in _projectRepository.GetAll()
                         on p.Projectid equals pr.Id
                         where p.ProductionFlag == 1
                         select new ProductionReport()
                         {
                             InvoiceAmount = p.InvoiceAmount,
                             Name = pr.ProjectName,
                             Invoicedate = p.Invoicedate
                         }).ToList();

            var data2 = (from p in _productionRepository.GetAll()
                         join sr in _serviceRepository.GetAll()
                         on p.Serviceid equals sr.Id
                         where p.ProductionFlag == 1
                         select new ProductionReport()
                         {
                             InvoiceAmount = p.InvoiceAmount,
                             Name = sr.Name,
                             Invoicedate = p.Invoicedate
                         }).ToList();

            var data3 = data1.Union(data2);

            var data4 = (from b in data3
                         where b.Invoicedate.Year <= financEndYear
                         select new { b.InvoiceAmount, b.Name, b.Invoicedate }).AsQueryable()
                      .Select(k => new { k.InvoiceAmount, k.Name, k.Invoicedate.Month, k.Invoicedate.Year, })
                         .GroupBy(x => new { x.Year, x.Month, x.Name }, (key, group) => new
                         {
                             yr = key.Year,
                             mnth = key.Month,
                             tCharge = group.Sum(k => k.InvoiceAmount),
                             ClientName = key.Name,
                         }).AsQueryable();

            foreach (var item in FinancialMonthList)
            {
                string[] monthlist = item.Split('-');

                SalesReport mSalesReport = new SalesReport();
                decimal TotalYearsales = 0;
                if (data4.ToList().Count > 0)
                {
                    foreach (var item1 in data4)
                    {
                        string monthName = getAbbreviatedName(item1.mnth);
                        string yr = Convert.ToString(item1.yr);
                        if (monthlist[0].Contains(monthName) && monthlist[1].Contains(yr))
                        {
                            InoiceReport mCollectionReport = new InoiceReport();

                            TotalYearsales = TotalYearsales + item1.tCharge;
                            mSalesReport.MonthYear = item;
                            mCollectionReport.ClientName = item1.ClientName;
                            mCollectionReport.totalbillamount = Convert.ToString(item1.tCharge);

                            mSalesReport.mInoiceReport.Add(mCollectionReport);
                            mSalesReport.totalYearsales = TotalYearsales;
                        }
                        else
                        {
                            mSalesReport.MonthYear = item;
                            mSalesReport.totalsales = 0;
                        }
                    }
                }
                else
                {
                    mSalesReport.MonthYear = item;
                    mSalesReport.totalsales = 0;
                }

                mSalesReport.FinancialYear = finaMnthye;
                mSalesReport.mFiancialYearDropdownList.AddRange(mFiancialYearDropdownList);
                mSalesReport.mFiancialYearDropdownList = mSalesReport.mFiancialYearDropdownList.OrderByDescending(x => x.value).ToList();
                SalesReportList.Add(mSalesReport);
            }

            return SalesReportList;
        }

        public List<GSTData> GetGSTDataReport(int year)
        {
            List<GSTData> GSTDataList = new List<GSTData>();
            var gstdata = _gstdataRepository.GetAll().Where(x => x.Month != null)
                .Select(x => new { x.Month, x.TotalPendingPayment, x.CompanyId, x.FinancialyearId }).ToList();
            var companydata = _companyRepository.GetAll().Select(x => new { x.Beneficial_Company_Name, x.CompanyId }).ToList();

            var GSTdata = (from gst in gstdata
                           join
                           company in companydata
                           on gst.CompanyId equals company.CompanyId
                           where company.CompanyId > 0 && gst.FinancialyearId == year
                           group new { gst, company }
                                          by new { gst.Month, company.Beneficial_Company_Name } into gf
                           select new GSTData
                           {
                               monthyear = gf.Key.Month,
                               gstAmount = gf.Sum(k => k.gst.TotalPendingPayment),
                               companyname = gf.Key.Beneficial_Company_Name
                           }).ToList();

            var qry_monthyearWiseData = GSTdata.GroupBy(v => new { v.monthyear })
    .Select(g => new
    {
        monthyear = g.Key.monthyear,
        companynameList = GSTdata.Where(x => x.monthyear == g.Key.monthyear).Select(x => new { x.companyname, x.gstAmount }).ToList(),
        gstrowwiseAmount = GSTdata.Where(x => x.monthyear == g.Key.monthyear).Select(x => x.gstAmount).Sum(),
    }).ToList();

            var qry_companynameWiseData = GSTdata.GroupBy(v => new { v.companyname })
 .Select(g => new
 {
     companyname = g.Key.companyname,
     gstrowwiseAmount = GSTdata.Where(x => x.companyname == g.Key.companyname).Select(x => x.gstAmount).Sum(),
 }).ToList();

            foreach (var item in qry_monthyearWiseData)
            {

                GSTData mGSTData = new GSTData();
                mGSTData.companynames = _companyRepository.GetAll().OrderBy(x => x.Beneficial_Company_Name).Where(x => x.CompanyId > 0).Select(x => x.Beneficial_Company_Name).ToList();
                foreach (var item2 in mGSTData.companynames)
                {
                    GSTAmountcompanynameList mobj = new GSTAmountcompanynameList();

                    var companyname = item.companynameList.Where(x => x.companyname == item2).FirstOrDefault();

                    if (companyname != null)
                    {
                        mobj.companyname = companyname.companyname;
                        mobj.gstAmount = companyname.gstAmount;
                    }
                    else
                    {
                        mobj.companyname = item2;
                        mobj.gstAmount = 0;
                    }
                    mobj.columnTotal = qry_companynameWiseData.Where(x => x.companyname == item2).Select(x => x.gstrowwiseAmount).FirstOrDefault();
                    mGSTData.companynameList.Add(mobj);
                }
                mGSTData.monthyear = item.monthyear;
                mGSTData.gstAmount = item.gstrowwiseAmount;
                GSTDataList.Add(mGSTData);
            }
            return GSTDataList;
        }

        public PagedResultDto<OutStandingInvoice> GetOutStandingInvoice(OutStandingInvoice input)
        {
            DateTime billdate = Convert.ToDateTime(ConfigurationManager.AppSettings["BillDate"].ToString().ToLower());

            List<OutStandingInvoice> mOutStandingInvoiceList = new List<OutStandingInvoice>();

            var frmdate = input.FromDate == null ? billdate.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            var billtbldata = (from a in _billRepository.GetAll()
                               join b in _billpymtRepository.GetAll() on a.Id equals b.Billid
                               group new { a.Performaid, b.PymtRecd } by new { a.Performaid, b.PymtRecd } into g
                               select new
                               {
                                   Id = g.Key.Performaid,
                                   PymtRecd = g.Key.PymtRecd,
                               });

            var performadata = (from perinv in _performainvoiceRepository.GetAll()
                                join cli in _clientRepository.GetAll() on perinv.ClientID equals cli.Id
                                where perinv.IsMarkAsConfirm == false
                                select new OutStandingInvoice
                                {
                                    PerformaId = perinv.Id,
                                    InvoiceNo = perinv.invoiceno,
                                    TotalBillAmt = perinv.totalbillamount.HasValue ? perinv.totalbillamount.Value : 0,
                                    TotalCollection = billtbldata.Where(x => x.Id == perinv.Id).Select(x => x.PymtRecd).DefaultIfEmpty(0).Sum(),
                                    ClientName = cli.ClientName,
                                    ClientId = perinv.ClientID.HasValue ? perinv.ClientID.Value : 0,
                                    OutStandingAmt = perinv.totalbillamount.HasValue ? (perinv.totalbillamount.Value - billtbldata.Where(x => x.Id == perinv.Id).Select(x => x.PymtRecd).DefaultIfEmpty(0).Sum()) : 0,
                                    BillDate = perinv.BillDate,
                                    IsMarkAsConfirm = perinv.IsMarkAsConfirm,
                                }).ToList();

            var billdata = (from b in _billRepository.GetAll()
                            join bpr in _billpymtRepository.GetAll() on b.Id equals bpr.Billid into t
                            from rt in t.DefaultIfEmpty()
                            join cli in _clientRepository.GetAll() on b.ClientID equals cli.Id into cligrp
                            from cligrprt in cligrp.DefaultIfEmpty()
                            join perinv in _performainvoiceRepository.GetAll() on b.Performaid equals perinv.Id into perinvgrp
                            from perinvgrprt in perinvgrp.DefaultIfEmpty()
                            where perinvgrprt.IsMarkAsConfirm != false
                            group new { b.invoiceno, b.totalbillamount, rt.PymtRecd } by new
                            {
                                Invno = b.invoiceno,
                                b.BillDate,
                                b.ClientID,
                                cligrprt.ClientName,
                                perinvgrprt.IsMarkAsConfirm,
                                performaid = b.Performaid,
                                b.totalbillamount
                            } into g

                            select new OutStandingInvoice
                            {
                                PerformaId = g.Key.performaid.HasValue ? g.Key.performaid.Value : 0,
                                InvoiceNo = g.Key.Invno,
                                TotalBillAmt = g.Key.totalbillamount.HasValue ? g.Key.totalbillamount.Value : 0,
                                TotalCollection = g.Sum(y => y.PymtRecd.HasValue ? y.PymtRecd : 0),
                                ClientName = g.Key.ClientName,
                                ClientId = g.Key.ClientID.HasValue ? g.Key.ClientID.Value : 0,
                                OutStandingAmt = g.Key.totalbillamount.HasValue ? (g.Key.totalbillamount - g.Sum(y => y.PymtRecd.HasValue ? y.PymtRecd : 0)) : 0,
                                BillDate = g.Key.BillDate,
                                IsMarkAsConfirm = g.Key.IsMarkAsConfirm,
                            }).ToList();

            mOutStandingInvoiceList = performadata.Union(billdata)
                .Where(x => x.OutStandingAmt > 0)
                .Where(p => p.BillDate >= dtfrm)
                .Where(p => p.BillDate <= dt)
                .WhereIf(input.ClientId != 0, p => p.ClientId == input.ClientId).ToList();

            var queryable = mOutStandingInvoiceList.AsQueryable();
            var Data = queryable.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = queryable.Count();
            return new PagedResultDto<OutStandingInvoice>(ccCount, Data);
        }

        public PagedResultDto<OutStandingInvoice> GetOutStandingClient(OutStandingInvoice input)
        {
            DateTime billdate = Convert.ToDateTime(ConfigurationManager.AppSettings["BillDate"].ToString().ToLower());

            List<OutStandingInvoice> mOutStandingInvoiceList = new List<OutStandingInvoice>();

            var frmdate = input.FromDate == null ? billdate.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            var pymtrecddata = (from a in _billRepository.GetAll()
                                join b in _billpymtRepository.GetAll() on a.Id equals b.Billid
                                group new { a.ClientID, b.PymtRecd } by new { a.ClientID, b.PymtRecd } into g
                                select new
                                {
                                    Id = g.Key.ClientID,
                                    PymtRecd = g.Key.PymtRecd,
                                });

            var perfomadata = _performainvoiceRepository.GetAll().Where(x => x.IsMarkAsConfirm == false);
            var billdata = (from a in _billRepository.GetAll()
                            join perinv in _performainvoiceRepository.GetAll() on a.Performaid equals perinv.Id into perinvgrp
                            from perinvgrprt in perinvgrp.DefaultIfEmpty()
                            where perinvgrprt.IsMarkAsConfirm != false
                            select new { totalbillamount = a.totalbillamount, clientid = a.ClientID, billdate = a.BillDate });

            var resultdata = (from cli in _clientRepository.GetAll()
                              join b in _billRepository.GetAll() on cli.Id equals b.ClientID
                              where b.BillDate >= dtfrm && b.BillDate <= dt
                              group new { cli.Id } by new
                              {
                                  cli.Id,
                                  cli.ClientName,
                              } into g
                              select new OutStandingInvoice
                              {
                                  ClientName = g.Key.ClientName,
                                  ClientId = g.Key.Id,
                                  TotalBillAmt = perfomadata.Where(x => x.ClientID == g.Key.Id).Select(x => x.totalbillamount).DefaultIfEmpty(0).Sum() + billdata.Where(x => x.clientid == g.Key.Id).Select(x => x.totalbillamount).DefaultIfEmpty(0).Sum(),
                                  TotalCollection = pymtrecddata.Where(x => x.Id == g.Key.Id).Select(x => x.PymtRecd).DefaultIfEmpty(0).Sum(),
                                  OutStandingAmt = (perfomadata.Where(x => x.ClientID == g.Key.Id).Select(x => x.totalbillamount).DefaultIfEmpty(0).Sum() + billdata.Where(x => x.clientid == g.Key.Id).Select(x => x.totalbillamount).DefaultIfEmpty(0).Sum()) - pymtrecddata.Where(x => x.Id == g.Key.Id).Select(x => x.PymtRecd).DefaultIfEmpty(0).Sum(),
                              }).ToList();



            mOutStandingInvoiceList = resultdata
                .Where(x => x.OutStandingAmt > 0)
                //.Where(p => p.BillDate >= dtfrm)
                //.Where(p => p.BillDate <= dt)
                .WhereIf(input.ClientId != 0, p => p.ClientId == input.ClientId).ToList();


            var queryable = mOutStandingInvoiceList.AsQueryable();
            var Data = queryable.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = queryable.Count();

            return new PagedResultDto<OutStandingInvoice>(ccCount, Data);
        }

        static string getAbbreviatedName(int month)
        {
            DateTime date = new DateTime(DateTime.Now.Year, month, 1);

            return date.ToString("MMM");
        }
        public static string GetFinancialYear(string cdate)
        {
            string finyear = "";
            DateTime dt = Convert.ToDateTime(cdate);
            int m = dt.Month;
            int y = dt.Year;
            if (m > 3)
            {
                finyear = y.ToString() + "-" + Convert.ToString((y + 1));
                //get last  two digits (eg: 10 from 2010);
            }
            else
            {
                finyear = Convert.ToString((y - 1)) + "-" + y.ToString();
            }
            return finyear;
        }



        #endregion

        //public DataTable ExportReportToExcel(GetInputDto input)
        //{
        //     var projectlist = (from a in _timesheetRepository.GetAll()
        //                       join b in _projectRepository.GetAll()
        //                       on a.ProjectId equals b.Id
        //                       where a.ProjectId == b.Id && a.UserId == input.UserId
        //                       group b by new
        //                       {
        //                           a.ProjectId,
        //                           b.ProjectName
        //                       } into g
        //                       select new ProjectDto
        //                       {
        //                           ProjectId = g.Key.ProjectId,
        //                           ProjectName = g.Key.ProjectName
        //                       }).ToList();

        //    //get datewise data
        //    var datewiselist = (from a in _timesheetRepository.GetAll()
        //                        where a.UserId == input.UserId && a.Date.Month == input.Month && a.Date.Year == input.Year
        //                        orderby a.Date
        //                        select new ReportsDto
        //                        {
        //                            Id = a.Id,
        //                            ProjectId = a.ProjectId,
        //                            UserId = a.UserId,
        //                            Description = a.Description,
        //                            Hours = a.Hours,
        //                            Date = a.Date,
        //                        }).ToList();
        //    //get Distinct date
        //    //var finalData = datewiselist.Select(x=>x.Date).Distinct().ToList();
        //    var finalData = (from d in datewiselist
        //                     select new
        //                     {
        //                         Year = d.Date.Year,
        //                         Month = d.Date.Month,
        //                         Day = d.Date.Day
        //                     }).Distinct().ToList();

        //    //get projectname and description
        //    var descriptionlist = (from b in _timesheetRepository.GetAll()
        //                           join a in _projectRepository.GetAll()
        //                           on b.ProjectId equals a.Id
        //                           where b.UserId == input.UserId && b.Date.Month == input.Month && b.Date.Year == input.Year
        //                           select new ReportDetails
        //                           {
        //                               ProjectId = b.ProjectId,
        //                               ProjectName = a.ProjectName,
        //                               Description = b.Description,
        //                               UserId = b.UserId,
        //                               Date = b.Date,
        //                           }).ToList();

        //    DataTable dt = new DataTable();
        //    DataRow dr;
        //    dt.Columns.Add("Date"); //date column

        //    for (int i = 0; i < projectlist.Count; i++)  //project column
        //    {
        //        dt.Columns.Add(projectlist[i].ProjectName);
        //    }
        //    dt.Columns.Add("Comment");   //comment column

        //    foreach (var item in finalData)
        //    {
        //        dr = dt.NewRow();
        //        DateTime dDate = new DateTime();
        //        var dateConvert = item.Year + "/" + item.Month + "/" + item.Day;
        //        dDate = Convert.ToDateTime(dateConvert);

        //        dr["Date"] = String.Format("{0:dd/MM/yyyy}", dDate); //date row

        //        for (int i = 0; i < projectlist.Count; i++)
        //        {

        //            int projectcode = projectlist[i].ProjectId;
        //            var hourdata = _timesheetRepository.GetAll().Where(e => e.ProjectId == projectcode && e.UserId == input.UserId && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) && e.Date.Month == input.Month && e.Date.Year == input.Year).Select(e => e.Hours).DefaultIfEmpty(0).Sum();

        //            var projectname = descriptionlist.Where(e => e.ProjectId == projectcode && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) && e.Date.Month == input.Month && e.Date.Year == input.Year).ToList();
        //            string strProj = "";
        //            if (projectname.Count > 0)
        //            {
        //                strProj = projectname.Select(x => x.ProjectName).FirstOrDefault();
        //                dr["Comment"] +=  strProj  + " : ";
        //            }

        //            var description = descriptionlist.Where(e => e.ProjectId == projectcode && (e.Date.Year == item.Year && e.Date.Month == item.Month && e.Date.Day == item.Day) /*&& e.Date == item.Date */&& e.Date.Month == input.Month && e.Date.Year == input.Year).ToList();

        //            if (description.Count > 0)
        //            {
        //                var strDes = description.Select(x => x.Description).ToList();
        //                string str = "";
        //                foreach (var itemDes in strDes)
        //                {
        //                    str += itemDes + ", ";
        //                }
        //                dr["Comment"] += str + ", ";
        //                dr["Comment"] = dr["Comment"].ToString().Remove(dr["Comment"].ToString().LastIndexOf(','));
        //            }

        //            dr[projectlist[i].ProjectName] = hourdata.ToString(); //Hours rows

        //        }
        //        dt.Rows.Add(dr);
        //    }
        //    return dt;
        //}

        public List<ProjectWiseTimesheetDto> GetProjectWiseTimesheetReport(GetInputDto input)
        {
            List<ProjectWiseTimesheetDto> data = new List<ProjectWiseTimesheetDto>();
            var userlist = (from u in _userRepository.GetAll()
                            join t in _timesheetRepository.GetAll() on u.Id equals t.UserId
                            where t.Date.Month == input.Month && t.Date.Year == input.Year && t.ProjectId == input.ProjectId
                            select new Getuserlist
                            {
                                UserName = u.UserName,
                                Userid = (int)u.Id
                            }

                 ).Distinct().OrderBy(X => X.UserName);

            var timesheetData = (from a in _timesheetRepository.GetAll().ToList()
                                 where a.Date.Month == input.Month && a.Date.Year == input.Year && a.ProjectId == input.ProjectId
                                 select a.Date.Date).Distinct();
            data = (from a in timesheetData
                    select new ProjectWiseTimesheetDto
                    {
                        Date = a,
                        //UserList= userlist.ToList(),
                        TimesheetData = (from dataT in userlist
                                         join ts in _timesheetRepository.GetAll().Where(c => c.Date.Month == a.Date.Month && c.Date.Year == a.Date.Year && c.Date.Day == a.Date.Day && c.ProjectId == input.ProjectId)
                                         on dataT.Userid equals ts.UserId into result
                                         //from info in result.DefaultIfEmpty()
                                         select new GetTmesheetData
                                         {
                                             Description = result.Select(x => x.Description).ToList(),
                                             hours = result.Count() > 0 ? result.Where(X => X.Hours > 0).Sum(x => x.Hours) : 0,
                                             UserId = (int)dataT.Userid,
                                             UserName = dataT.UserName
                                         }).ToList()


                    }).Distinct().OrderBy(x => x.Date).ToList();



            return data;

        }
        public List<ExpenseReportDto> GetExpenseEntryReport(int year)
        {
            List<string> FinancialMonthList = new List<string>();
            List<string> FinancialMnthList = new List<string>();
            List<FiancialYearDropdown> mFiancialYearDropdownList = new List<FiancialYearDropdown>();
            string FinancialYear = "";

            if (year != null || year != 0)
            {
                DateTime _LoopDate1 = new DateTime(year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }
            else
            {
                DateTime _LoopDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }

            // Get financial year eg.2020-2021
            var finaMnthye = GetFinancialYear(FinancialYear);

            string[] financialYearList = finaMnthye.Split('-');

            DateTime _LoopDate = new DateTime(Convert.ToInt32(financialYearList[0]), 04, 01);
            DateTime dtstart = _LoopDate;
            string month = string.Empty;
            string mn = "";
            DateTime _EndDate = new DateTime(Convert.ToInt32(financialYearList[1]), 03, 31);
            DateTime dtEnd = _EndDate;
            //Get Full finacial year e.g.apr - 2021 to mar-2022
            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();
                month = d.MonthNames[_LoopDate.Month - 1];
                mn = Convert.ToString(_LoopDate.Month);
                FinancialMonthList.Add(month.Substring(0, 3) + "-" + _LoopDate.Year.ToString());
                FinancialMnthList.Add(mn + "-" + _LoopDate.Year.ToString());
                _LoopDate = _LoopDate.AddMonths(1);
            }

            //Bind dropdown with finacial year
            var startdrpFinancialYear = _projectDetailsRepository.GetAll()
                                                                 .OrderBy(x => x.CreationTime)
                                                                 .Select(x => x.CreationTime.Year)
                                                                 .FirstOrDefault();
            int GivenYear = startdrpFinancialYear;
            int j = DateTime.Now.Year - (GivenYear - 1);
            for (int i = j; i >= 0; i--)
            {
                int fy = DateTime.Now.Year - i;
                int fy1 = fy + 1;
                FiancialYearDropdown mFiancialYearDropdown = new FiancialYearDropdown();

                if (DateTime.Now.Date > Convert.ToDateTime(fy + "-03-31").Date)
                {
                    mFiancialYearDropdown.text = fy.ToString() + "-" + fy1.ToString();
                    mFiancialYearDropdown.value = fy.ToString();
                    mFiancialYearDropdownList.Add(mFiancialYearDropdown);
                }

            }
            List<ExpenseReportDto> objlst = new List<ExpenseReportDto>();
            var categorylist = _expenseCategoryRepository.GetAll().ToList();

            var ss = _expenseEntryRepository.GetAll();
            var subcat = _expenseSubCategoryRepository.GetAll();
            var data2 = (from std in _expenseCategoryRepository.GetAll()
                         where std.IsDeleted == false
                         select new ExpenseReportDto
                         {
                             categoryid = std.Id,
                             Category = std.Category,
                             months = (ss.Where(e => e.MonthYear >= dtstart && e.MonthYear <= dtEnd && e.CategoryId == std.Id
                                                                                  ).ToList().GroupBy(x => new { x.MonthYear.Month, x.MonthYear.Year })
                                                                                 .Select(f => new monthdetail
                                                                                 {
                                                                                     expense = f.Sum(x => x.Expense == null ? 0 : x.Expense),
                                                                                     monthyr = f.Key.Month + "-" + f.Key.Year
                                                                                 }).Distinct().OrderBy(x => x.monthyr).ToList()
                                                                                 ),
                             subcatdetaillist = (from s in subcat
                                                 join
                                exp in ss on s.Id equals exp.SubCategoryId into g
                                                 from exp in g.DefaultIfEmpty()
                                                 where (exp.MonthYear >= dtstart && exp.MonthYear <= dtEnd && exp.CategoryId == std.Id)
                                                 select new subcategorydetail
                                                 {
                                                     subcategory = s.SubCategory,
                                                     //amount = exp.Expense == null ? 0 : exp.Expense,
                                                     //monthyr =   exp.MonthYear.Month +"-"+ exp.MonthYear.Year,
                                                     subcategoryId = s.Id
                                                 }).Distinct().OrderBy(x => x.subcategory).ToList()
                         }).AsQueryable();

            var newdt = data2.ToList();
            foreach (var item in data2)
            {
                ExpenseReportDto obj = new ExpenseReportDto();
                obj.categoryid = item.categoryid;
                obj.Category = item.Category;

                List<monthdetail> monthlist = new List<monthdetail>();
                List<subcategorydetail> subdetaillist = new List<subcategorydetail>();

                var mntdt = (from i in FinancialMnthList
                             join mnt in item.months on i equals mnt.monthyr
                              into g
                             //from mnt in g.DefaultIfEmpty()
                             select new monthdetail
                             {
                                 monthyr = i,
                                 expense = g.Select(e => e.expense).FirstOrDefault() == null ? 0 : g.Select(e => e.expense).FirstOrDefault()
                             }).ToList();
                var totalmntdt = (from i in FinancialMnthList
                                  join mnt in item.months on i equals mnt.monthyr
                                   into g
                                  //from mnt in g.DefaultIfEmpty()
                                  select new subcategorydetaillist
                                  {
                                      amount = g.Sum(e => e.expense == null ? 0 : e.expense)
                                  }).ToList();
                for (int i = 0; i < item.subcatdetaillist.Count; i++)
                {
                    subcategorydetail dt = new subcategorydetail();
                    dt.subcategory = item.subcatdetaillist[i].subcategory;
                    var id = item.subcatdetaillist[i].subcategoryId;
                    for (int k = 0; k < FinancialMnthList.Count; k++)
                    {
                        string[] mntlist = FinancialMnthList[k].Split('-');
                        int submn = Convert.ToInt32(mntlist[0]);
                        int subyr = Convert.ToInt32(mntlist[1]);

                        var amt = (from e in _expenseEntryRepository.GetAll()
                                   where (e.SubCategoryId == id && e.MonthYear.Month == submn && e.MonthYear.Year == subyr)
                                   select new subcategorydetaillist { amount = e.Expense == null ? 0 : e.Expense }).FirstOrDefault();


                        dt.st.Add(amt);
                    }
                    subdetaillist.Add(dt);
                }

                obj.months = mntdt;
                obj.subcatdetaillist = subdetaillist;
                //obj.subname = subname;
                obj.FinancialYear = finaMnthye;
                obj.mFiancialYearDropdownList.AddRange(mFiancialYearDropdownList);
                obj.mFiancialYearDropdownList = obj.mFiancialYearDropdownList.OrderByDescending(x => x.value).ToList();
                obj.MonthYear = FinancialMonthList;
                obj.isCollapsed = false;
                objlst.Add(obj);
            }


            return objlst;
        }
        public List<subcategorydetaillist> GetExpenseEntryReportTotal(int year)
        {
            List<string> FinancialMonthList = new List<string>();
            List<string> FinancialMnthList = new List<string>();
            List<subcategorydetaillist> objlst = new List<subcategorydetaillist>();

            string FinancialYear = "";

            if (year != null || year != 0)
            {
                DateTime _LoopDate1 = new DateTime(year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }
            else
            {
                DateTime _LoopDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                FinancialYear = _LoopDate1.ToString();
            }

            // Get financial year eg.2020-2021
            var finaMnthye = GetFinancialYear(FinancialYear);

            string[] financialYearList = finaMnthye.Split('-');
            subcategorydetaillist obj1 = new subcategorydetaillist();
            obj1.amount = null;
            DateTime _LoopDate = new DateTime(Convert.ToInt32(financialYearList[0]), 04, 01);
            DateTime dtstart = _LoopDate;
            string month = string.Empty;
            string mn = "";
            DateTime _EndDate = new DateTime(Convert.ToInt32(financialYearList[1]), 03, 31);
            DateTime dtEnd = _EndDate;
            //Get Full finacial year e.g.apr - 2021 to mar-2022
            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();
                month = d.MonthNames[_LoopDate.Month - 1];
                mn = Convert.ToString(_LoopDate.Month);
                FinancialMonthList.Add(month.Substring(0, 3) + "-" + _LoopDate.Year.ToString());
                FinancialMnthList.Add(mn + "-" + _LoopDate.Year.ToString());
                _LoopDate = _LoopDate.AddMonths(1);
            }

            //Bind dropdown with finacial year

            objlst.Add(obj1);
            for (int k = 0; k < FinancialMnthList.Count; k++)
            {
                subcategorydetaillist obj = new subcategorydetaillist();
                string[] mntlist = FinancialMnthList[k].Split('-');
                int submn = Convert.ToInt32(mntlist[0]);
                int subyr = Convert.ToInt32(mntlist[1]);
                //decimal? amt = 0;
                obj.amount = (from e in _expenseEntryRepository.GetAll()
                              where (e.MonthYear.Month == submn && e.MonthYear.Year == subyr)
                              select (Decimal?)e.Expense).Sum() ?? 0;
                objlst.Add(obj);
            }
            return objlst;
        }

        #region loginLogoutReport


        public PagedResultDto<LoginLogoutReportDto> GetLoginLogoutReportData(InputLoginLogoutReportDto input)
        {
            //  var fromDate = new DateTime(input.Year, input.Month, 1);
            //var da=  DateTime.DaysInMonth(input.Year, input.Month);
            //  var toDate = new DateTime(input.Year, input.Month, da);
            //var fDate = Convert.ToDateTime(fromDate + " 00:00:00");
            //var tDate = Convert.ToDateTime(toDate + " 23:59:59");
            var fromDate = new DateTime(input.FromDate.Year, input.FromDate.Month, 1);
            var toDate = fromDate.AddMonths(1).AddTicks(-1);

            var data1 = (from a in _userRepository.GetAll()
                         join b in _UserLoginDataRepository.GetAll()
                         .Where(x => x.LoggedIn >= fromDate && x.LoggedIn
                         <= toDate)
                         on a.Id equals b.UserId
                        into userpunch
                         where a.UserName.ToLower() != "admin"
                         select new LoginLogoutReportDto
                         {
                             userId = (int)a.Id,
                             EmployeeName = a.Name + " " + a.Surname,
                             LoginCount = userpunch.Count()
                         });
            //.Where(x => x.userId != (int)_session.UserId)
            var data = (from l in _UserLoginDataRepository.GetAll()
                        join u in _userRepository.GetAll()
                        on l.UserId equals u.Id
                        group new
                        {
                            u.Id,
                            l.LoggedIn
                        }
                        by new
                        {
                            userId = u.Id,
                            employeeName = u.Name + " " + u.Surname,
                            loginCount = l.LoggedIn

                        } into g
                        select new LoginLogoutReportDto
                        {
                            userId = (int)g.Key.userId,
                            EmployeeName = g.Key.employeeName,
                            LoginCount = g.Count()
                        });
            var entites = data1.OrderBy(input.Sorting).PageBy(input).ToList();
            var entitiesCount = data1.Count();
            return new PagedResultDto<LoginLogoutReportDto>(entitiesCount, entites.MapTo<List<LoginLogoutReportDto>>());
        }

        public List<GetYearDto> GetUniqueYear()
        {
            var data = (from t in _timesheetRepository.GetAll()
                        select new GetYearDto
                        {
                            year = t.CreationTime.Year
                        }).Distinct().ToList();
            return data;
        }
        public List<GetMonth> GetUniqueMonth()
        {
            List<GetMonth> MonthList = new List<GetMonth>();
            for (int i = 1; i <= 12; i++)
            {
                MonthList.Add(new GetMonth { Id = i, Name = getFullName(i) });
            }
            return MonthList;
        }

        public PagedResultDto<EmployeeInOutReportDetailsDto> GetInOutDetailsReportData(EmployeeInOutDetails input)
        {
            var fromDate = new DateTime(input.FromDate.Year, input.FromDate.Month, 1);
            var toDate = fromDate.AddMonths(1).AddTicks(-1);

            var data = (from l in _UserLoginDataRepository.GetAll()
                        join u in _userRepository.GetAll()
                        on l.UserId equals u.Id
                        where l.LoggedIn >= fromDate && l.LoggedIn <= toDate && l.UserId == input.userId
                        select new EmployeeInOutReportDetailsDto
                        {
                            userId = l.UserId,
                            EmployeeName = u.Name + " " + u.Surname,
                            Date = l.LoggedIn,
                            LogInTime = l.LoggedIn,
                            LogOutTime = l.LoggedOut.Value
                        });
            var entites = data.OrderBy(input.Sorting).PageBy(input).ToList();
            var entitiesCount = data.Count();
            return new PagedResultDto<EmployeeInOutReportDetailsDto>(entitiesCount, entites.MapTo<List<EmployeeInOutReportDetailsDto>>());
        }

        #endregion

        public string GetFinYear()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string FinancialYear = dt.ToString();
            var finayear = GetFinancialYear(FinancialYear);
            return finayear;
        }

        public List<OutStandingInvoice> ExportToExcelGetOutStandingClient(ImportOutStandingInvoiceDto input)
        {
            DateTime billdate = Convert.ToDateTime(ConfigurationManager.AppSettings["BillDate"].ToString().ToLower());

            List<OutStandingInvoice> mOutStandingInvoiceList = new List<OutStandingInvoice>();

            var frmdate = input.FromDate == null ? billdate.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            var billtbldata = (from a in _billRepository.GetAll()
                               join b in _billpymtRepository.GetAll() on a.Id equals b.Billid
                               group new { a.Performaid, b.PymtRecd } by new { a.Performaid, b.PymtRecd } into g
                               select new
                               {
                                   Id = g.Key.Performaid,
                                   PymtRecd = g.Key.PymtRecd,
                               });

            var performadata = (from perinv in _performainvoiceRepository.GetAll()
                                join cli in _clientRepository.GetAll() on perinv.ClientID equals cli.Id
                                where perinv.IsMarkAsConfirm == false
                                select new OutStandingInvoice
                                {
                                    PerformaId = perinv.Id,
                                    InvoiceNo = perinv.invoiceno,
                                    TotalBillAmt = perinv.totalbillamount.HasValue ? perinv.totalbillamount.Value : 0,
                                    TotalCollection = billtbldata.Where(x => x.Id == perinv.Id).Select(x => x.PymtRecd).DefaultIfEmpty(0).Sum(),
                                    ClientName = cli.ClientName,
                                    ClientId = perinv.ClientID.HasValue ? perinv.ClientID.Value : 0,
                                    OutStandingAmt = perinv.totalbillamount.HasValue ? (perinv.totalbillamount.Value - billtbldata.Where(x => x.Id == perinv.Id).Select(x => x.PymtRecd).DefaultIfEmpty(0).Sum()) : 0,
                                    BillDate = perinv.BillDate,
                                    IsMarkAsConfirm = perinv.IsMarkAsConfirm,
                                }).ToList();

            var billdata = (from b in _billRepository.GetAll()
                            join bpr in _billpymtRepository.GetAll() on b.Id equals bpr.Billid into t
                            from rt in t.DefaultIfEmpty()
                            join cli in _clientRepository.GetAll() on b.ClientID equals cli.Id into cligrp
                            from cligrprt in cligrp.DefaultIfEmpty()
                            join perinv in _performainvoiceRepository.GetAll() on b.Performaid equals perinv.Id into perinvgrp
                            from perinvgrprt in perinvgrp.DefaultIfEmpty()
                            where perinvgrprt.IsMarkAsConfirm != false
                            group new { b.invoiceno, b.totalbillamount, rt.PymtRecd } by new
                            {
                                Invno = b.invoiceno,
                                b.BillDate,
                                b.ClientID,
                                cligrprt.ClientName,
                                perinvgrprt.IsMarkAsConfirm,
                                performaid = b.Performaid,
                                b.totalbillamount
                            } into g

                            select new OutStandingInvoice
                            {
                                PerformaId = g.Key.performaid.HasValue ? g.Key.performaid.Value : 0,
                                InvoiceNo = g.Key.Invno,
                                TotalBillAmt = g.Key.totalbillamount.HasValue ? g.Key.totalbillamount.Value : 0,
                                TotalCollection = g.Sum(y => y.PymtRecd.HasValue ? y.PymtRecd : 0),
                                ClientName = g.Key.ClientName,
                                ClientId = g.Key.ClientID.HasValue ? g.Key.ClientID.Value : 0,
                                OutStandingAmt = g.Key.totalbillamount.HasValue ? (g.Key.totalbillamount - g.Sum(y => y.PymtRecd.HasValue ? y.PymtRecd : 0)) : 0,
                                BillDate = g.Key.BillDate,
                                IsMarkAsConfirm = g.Key.IsMarkAsConfirm,
                            }).ToList();

            mOutStandingInvoiceList = performadata.Union(billdata)
                .Where(x => x.OutStandingAmt > 0)
                .Where(p => p.BillDate >= dtfrm)
                .Where(p => p.BillDate <= dt)
                .WhereIf(input.ClientId != 0, p => p.ClientId == input.ClientId).ToList();

            //var queryable = mOutStandingInvoiceList.AsQueryable();
            //var Data = queryable.OrderBy(input.Sorting).ToList();
            //var ccCount = queryable.Count();
            return mOutStandingInvoiceList;
        }

        public ListResultDto<GetDailyActivityReportDto> GetDailyActivityReport(GetDailyActivityReportInputDto input)
        {

            var frmdate = input.FromDate == null ? DateTime.Now.Date.ToShortDateString() : input.FromDate.Value.Date.ToShortDateString();
            DateTime dtfrm = Convert.ToDateTime(frmdate);

            var todate = input.ToDate == null ? DateTime.Now.Date.ToShortDateString() : input.ToDate.Value.Date.ToShortDateString();
            DateTime dt = Convert.ToDateTime(todate);

            int dtdiff = Convert.ToInt32((dt.AddDays(1) - dtfrm).Days);
            var dtlist = GetDateList(dtfrm, Convert.ToInt32(dtdiff));

            var opportunityFollowUps = _OpportunityFollowUpRepository.GetAll().Where(p => p.CreationTime >= dtfrm && p.CreationTime <= dt).Where(p => p.Followuptypeid == 2 || p.Followuptypeid == 3);

            var reportdate = (from a in dtlist
                              let previousDate = a.Date.AddDays(-1).Date
                              select new GetDailyActivityReportDto
                              {
                                  FollowUpDate = a.Date,
                                  Date = a.Date.ToShortDateString(),
                                  UserData = (from b in _userRepository.GetAll()
                                              join ur in _userRoleRepository.GetAll()
                                                         on b.Id equals ur.UserId
                                              join r in _roleRepository.GetAll()
                                              on ur.RoleId equals r.Id
                                              where (r.DisplayName == "Marketing Leader" || r.DisplayName == "TeleMarketing" || r.DisplayName == "Operating Leader" || r.DisplayName == "Admin")
                                              let previousFollowUpIds = opportunityFollowUps
                                     .Where(x => x.CreatorUserId == b.Id && x.CreationTime <= previousDate)
                                     .Select(x => x.opporutnityid)
                                     .Distinct()
                                     .ToList()
                                              let firstAttemptCount = opportunityFollowUps.Where(x => x.CreationTime.Day == a.Day && x.CreationTime.Month == a.Month && x.CreationTime.Year == a.Year && x.CreatorUserId == b.Id)
                                                  .Count(X=>X!=null && !previousFollowUpIds.Contains(X.opporutnityid))

                                              select new UserDetails
                                              {
                                                  UserId = (int)b.Id,
                                                  UserName = b.Name + " " + b.Surname,
                                                  FirstAtmptCount = firstAttemptCount,
                                                  TotalCount = opportunityFollowUps.Where(x => x.CreationTime.Day == a.Day && x.CreationTime.Month == a.Month && x.CreationTime.Year == a.Year && x.CreatorUserId == b.Id).ToList().Count()
                                              }).Distinct().ToList()
                              }).Distinct().ToList();

            return new ListResultDto<GetDailyActivityReportDto>(reportdate.MapTo<List<GetDailyActivityReportDto>>());

        }
        public static List<DateTime> GetDateList(DateTime startDate, int numberOfDays)
        {
            List<DateTime> dateList = new List<DateTime>();

            for (int i = 0; i < numberOfDays; i++)
            {
                DateTime currentDate = startDate.AddDays(i);
                dateList.Add(currentDate);
            }

            return dateList;
        }
    }
}
