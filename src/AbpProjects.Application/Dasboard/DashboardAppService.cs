using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Timing.Timezone;
using Abp.UI;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Bills;
using AbpProjects.CallCategories;
using AbpProjects.Dasboard.Dto;
using AbpProjects.Holidays;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.InvoiceRequest;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.MeghPlanningSupportServices.Dto;
using AbpProjects.Opportunities;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.PerformaInvoices;
using AbpProjects.Project;
using AbpProjects.Project.Dto;
using AbpProjects.ProjectType;
using AbpProjects.Receipt;
using AbpProjects.TimeSheet;
using AbpProjects.TimeSheet.Dto;
using AbpProjects.UserLoginDetails;
using AbpProjects.Users.Dto;
using AbpProjects.WorkType;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Dasboard
{
    [AbpAuthorize]
    public class DashboardAppService : AbpProjectsApplicationModule, IDashboardAppService
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IRepository<UserLoginData> _userloginRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IRepository<Holiday> _holidayRepository;
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<ProjectStatus> _projectstatusRepository;
        private readonly IRepository<Projecttype_details> _projectsDetailsRepository;
        private readonly IRepository<worktype> _worktypeRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IAbpSession _session;
        private readonly IRepository<projecttype> _projecttypeRepository;
        //private readonly IRepository<ProjectStatus> _projectStatusRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<ServerTypeDetail> _serverRepository;
        private readonly IRepository<Typename> _typenameRepository;
        private readonly IRepository<ManageService> _manageserviceRepository;
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<ServiceRequestHistory> _serviceRequestRepository;
        private readonly IRepository<invoicerequest> _invoiceRequestRepository;
        private readonly IRepository<ImportUserStoryDetails> _importUserDataRepository;
        private readonly IRepository<PerformaInvoice> _performainvoiceRepository;
        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<BillPymtRecd> _billpymtRepository;
        //follow up dependency
        private readonly IRepository<Opportunity> _OpportunityRepository;
        private readonly IRepository<InterestedOpportunity> _InterestedOpportunityRepository;
        private readonly IRepository<OpportunityFollowUp> _OpportunityFollowUpRepository;
        private readonly IRepository<FollowupIntrest> _FollowupIntrestRepository;
        private readonly IRepository<CallCategory> _CallCategoriesRepository;
        private readonly UserManager _userManager;
        public DashboardAppService(
            ITimeZoneConverter timeZoneConverter,
            IRepository<UserLoginData> userloginRepository,
            IRepository<User, long> userRepository,
            IRepository<timesheet> timesheetRepository,
            IRepository<Holiday> holidayRepository,
            IRepository<project> projectRepository,
            IRepository<ProjectStatus> projectstatusRepository,
            IRepository<Projecttype_details> projectsDetailsRepository,
            IRepository<worktype> worktypeRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<projecttype> projecttypeRepository,
            IAbpSession session,
            IRepository<ManageService> manageserviceRepository,
            IRepository<Service> serviceRepository,
            IRepository<ServiceRequestHistory> serviceRequestRepository,
            IRepository<ServerTypeDetail> serverRepository,
            IRepository<invoicerequest> invoiceRequestRepository,
            IRepository<ImportUserStoryDetails> importUserDataRepository,
            IRepository<Client> clientRepository,
             IRepository<Bill> billRepository,
             IRepository<BillPymtRecd> billpymtRepository,
             IRepository<Typename> typenameRepository,
             IRepository<PerformaInvoice> performainvoiceRepository,
             IRepository<Opportunity> OpportunityRepository,
             IRepository<InterestedOpportunity> InterestedOpportunityRepository,
             IRepository<OpportunityFollowUp> OpportunityFollowUpRepository,
             IRepository<FollowupIntrest> FollowupIntrestRepository,
             IRepository<CallCategory> CallCategoriesRepository,
             UserManager userManager

            )
        {
            _timeZoneConverter = timeZoneConverter;
            _userloginRepository = userloginRepository;
            _userRepository = userRepository;
            _timesheetRepository = timesheetRepository;
            _holidayRepository = holidayRepository;
            _projectRepository = projectRepository;
            _projectsDetailsRepository = projectsDetailsRepository;
            _worktypeRepository = worktypeRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _session = session;
            _projecttypeRepository = projecttypeRepository;
            _projectsDetailsRepository = projectsDetailsRepository;
            _projectstatusRepository = projectstatusRepository;
            _manageserviceRepository = manageserviceRepository;
            _serviceRepository = serviceRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _serverRepository = serverRepository;
            _invoiceRequestRepository = invoiceRequestRepository;
            _importUserDataRepository = importUserDataRepository;
            _clientRepository = clientRepository;
            _billRepository = billRepository;
            _billpymtRepository = billpymtRepository;
            _typenameRepository = typenameRepository;
            _performainvoiceRepository = performainvoiceRepository;

            //followUp
            _OpportunityRepository = OpportunityRepository;
            _InterestedOpportunityRepository = InterestedOpportunityRepository;
            _OpportunityFollowUpRepository = OpportunityFollowUpRepository;
            _FollowupIntrestRepository = FollowupIntrestRepository;
            _CallCategoriesRepository = CallCategoriesRepository;
            _userManager = userManager;
        }

        public async Task<LogInDto> CreateLogInTime()
        {
            LogInDto input = new LogInDto();
            input.UserId = (int)_session.UserId;
            input.LoggedIn = Clock.Now;
            input.TimesheetComplete = 0;
            var logintime = input.MapTo<UserLoginData>();//ObjectMapper.Map<WorkType>(input);
            int id = _userloginRepository.InsertAndGetId(logintime);
            input.Id = id;
            return input;
        }

        public async Task<LogInDto> GetLoginUserList()
        {
            try
            {
                LogInDto input = new LogInDto();
                input.UserId = (int)_session.UserId;
                DateTime currentdate = Clock.Now;
                var dateCurrent = _timeZoneConverter.Convert(currentdate, _session.TenantId, _session.GetUserId());
                var timesheet = _userloginRepository.GetAll().Where(x => x.UserId == input.UserId && (x.LoggedIn.Year == dateCurrent.Value.Year && x.LoggedIn.Month == dateCurrent.Value.Month && x.LoggedIn.Day == dateCurrent.Value.Day)).FirstOrDefault();

                if (timesheet != null)
                {
                    input.Id = timesheet.Id;
                    input.UserId = timesheet.UserId;
                    input.LoggedIn = timesheet.LoggedIn;
                    input.LoggedOut = timesheet.LoggedOut;
                    //input.TimesheetComplete = 0;
                }

                input.LeaveBalance = _userRepository.GetAll().Where(x => x.Id == input.UserId).Select(x => x.LeaveBalance).FirstOrDefault();
                return input;
            }
            catch (Exception ex)
            {


            }
            return null;
        }

        public ListResultDto<LogOutMissingDto> GetMissingLogOutList()
        {
            try
            {
                int UserId = (int)_session.UserId;
                //DateTime currentdate = DateTime.Now;
                //var timeCurrent = currentdate.ToString("dd/MM/yyyy");
                //DateTime dateCurrent = DateTime.Parse(timeCurrent);

                DateTime currentdate = Clock.Now;
                //var timeCurrent = currentdate.ToString("yyyy-MM-dd");
                //DateTime dateCurrent = DateTime.Parse(timeCurrent);
                var dateCurrent = _timeZoneConverter.Convert(currentdate, _session.TenantId, _session.GetUserId());

                var entities = (from u in _userloginRepository.GetAll()
                                join um in _userRepository.GetAll()
                                on u.UserId equals um.Id
                                where u.LoggedOut == null
                                select new LogOutMissingDto
                                {
                                    Id = u.Id,
                                    UserId = u.UserId,
                                    UserName = um.UserName,
                                    LoggedIn = u.LoggedIn,
                                    LoggedOut = u.LoggedOut

                                })
                               .Where(x => x.LoggedIn != null && x.LoggedIn < dateCurrent && x.UserId == UserId)
                               .OrderByDescending(x => x.Id)
                               .ToList();


                return new ListResultDto<LogOutMissingDto>(entities.MapTo<List<LogOutMissingDto>>());
            }
            catch (Exception ex)
            {


            }


            return null;
        }

        public async Task<LogInDto> UpdateLogOutTime(EntityDto input)
        {
            LogInDto obj = new LogInDto();
            int UserId = (int)_session.UserId;
            var User = _userloginRepository.GetAll().Where(u => u.Id == input.Id).FirstOrDefault();
            User.LoggedOut = Clock.Now;
            await _userloginRepository.UpdateAsync(User);
            obj.LoggedIn = User.LoggedIn;
            obj.LoggedOut = User.LoggedOut;
            return obj;
        }

        public List<TimeSheetData> GetMissingTimesheet()
        {
            try
            {
                int UserId = (int)_session.UserId;
                DateTime dateCurrent = Clock.Now;
                //var timeCurrent = currentdate.ToString("yyyy-MM-dd");
                //DateTime dateCurrent = DateTime.Parse(timeCurrent);
                //var dateCurrent = _timeZoneConverter.Convert(currentdate, _session.TenantId, _session.GetUserId());
                var timesheetdata = _timesheetRepository.GetAll().Where(x=>x.UserId == UserId);
                var entity = (from u in _userloginRepository.GetAll()
                              join um in _userRepository.GetAll()
                              on u.UserId equals um.Id
                              join ur in _userRoleRepository.GetAll()
                              on u.UserId equals ur.UserId
                              join r in _roleRepository.GetAll()
                              on ur.RoleId equals r.Id
                              where r.Name == "Employee"
                              select new LogInDto
                              {
                                  Id = u.Id,
                                  UserId = u.UserId,
                                  LoggedIn = u.LoggedIn,
                                  LoggedOut = u.LoggedOut,
                                  EmpCode = u.EmpCode,
                                  TimesheetComplete = u.TimesheetComplete,
                                  createTime = u.CreationTime
                              })
                               .Where(x => x.LoggedIn != null && dateCurrent.Date > x.LoggedIn.Value && x.UserId == UserId && (x.TimesheetComplete == 0 || x.TimesheetComplete == 1))
                               //.Where(x=>x.LoggedIn!=x.createTime)
                               .OrderBy(x => x.Id)
                               .ToList();

                List<TimeSheetData> objlist = new List<TimeSheetData>();
                for (int i = 0; i < entity.Count; i++)
                {
                    TimeSheetData obj = new TimeSheetData();
                    var datetime = entity[i].LoggedIn.Value.ToString("yyyy/MM/dd");
                    DateTime dt = DateTime.Parse(datetime);
                    var entities = (from t in _timesheetRepository.GetAll()
                                    join u in _userRepository.GetAll()
                                    on t.UserId equals u.Id
                                    join ul in _userloginRepository.GetAll().Where(x => x.LoggedIn.Year == dt.Year && x.LoggedIn.Month == dt.Month && x.LoggedIn.Day == dt.Day)
                                    on u.Id equals ul.UserId
                                    where u.Id == UserId
                                    select new TimeSheetData
                                    {
                                        Id = t.Id,
                                        UserId = t.UserId,
                                        Date = t.Date,
                                        TimesheetComplete = ul.TimesheetComplete,
                                        TimesheetCount = timesheetdata.Where(x=>x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date.Day == dt.Day).Select(x=>x).Count(),
                                    })
                                    .Where(x => x.UserId == UserId && (x.Date.Value.Year == dt.Year && x.Date.Value.Month == dt.Month && x.Date.Value.Day == dt.Day)
                                    )
                                    .FirstOrDefault();

                    if (entities == null)
                    {
                        obj.Id = 0;
                        obj.ProjectId = 0;
                        obj.WorkTypeId = 0;
                        obj.Date = dt;
                        obj.UserId = UserId;
                        obj.Description = null;
                        obj.Hours = 0;
                        obj.TimesheetComplete = 0;
                        //var getdata = _userloginRepository.GetAll().Where(x => x.UserId == UserId && (x.LoggedIn.Year == dt.Year && x.LoggedIn.Month == dt.Month && x.LoggedIn.Day == dt.Day)).FirstOrDefault();
                        //if (getdata != null)
                        //{
                        //    getdata.TimesheetComplete = 0;
                        //    _userloginRepository.UpdateAsync(getdata);
                        //}
                        //objlist.Add(obj);
                    }
                    else
                    {
                        obj.Id = entities.Id;
                        obj.ProjectId = 0;
                        obj.WorkTypeId = 0;
                        obj.Date = entities.Date;
                        obj.UserId = UserId;
                        obj.Description = null;
                        obj.Hours = 0;
                        obj.TimesheetComplete = entities.TimesheetComplete;
                        obj.TimesheetCount = entities.TimesheetCount;
                    }
                    objlist.Add(obj);
                }


                return objlist;
            }
            catch (Exception ex)
            {


            }

            return null;
        }

        public async Task UpdateTimesheetStatus(TimeSheetData input)
        {
            var getdata = _userloginRepository.GetAll().Where(x => x.UserId == input.UserId && (x.LoggedIn.Year == input.Date.Value.Year && x.LoggedIn.Month == input.Date.Value.Month && x.LoggedIn.Day == input.Date.Value.Day)).FirstOrDefault();
            if (getdata != null)
            {
                getdata.TimesheetComplete = 2;
                _userloginRepository.UpdateAsync(getdata);
            }

        }

        public ListResultDto<TimeSheetDto> GetTimeSheetData(TimeSheetData input)
        {
            int UserId = (int)_session.UserId;
            var Query = (from a in _timesheetRepository.GetAll()
                         join b in _projectRepository.GetAll()
                          on a.ProjectId equals b.Id
                         join c in _worktypeRepository.GetAll()
                         on a.WorkTypeId equals c.Id

                         select new TimeSheetDto
                         {
                             Id = a.Id,
                             ProjectId = a.ProjectId,
                             ProjectName = b.ProjectName,
                             WorkTypeId = c.Id,
                             WorkTypeName = c.WorkTypeName,
                             Description = a.Description,
                             Hours = a.Hours,
                             Date = a.Date,
                             UserId = a.UserId,

                         }).Where(x => x.UserId == UserId && (x.Date.Value.Year == input.Date.Value.Year && x.Date.Value.Month == input.Date.Value.Month && x.Date.Value.Day == input.Date.Value.Day)).ToList();


            return new ListResultDto<TimeSheetDto>(Query.MapTo<List<TimeSheetDto>>());
        }

        public ListResultDto<LogInDto> GetUserMissingTimesheetCount()
        {
            string roleName = "";
            long UserId = (int)_session.UserId;
            var curUser = _userRepository.GetAll().Where(x => x.Id == UserId).Select(x => x.UserName).FirstOrDefault();
            var roleId = _userRoleRepository.GetAll().Where(x => x.UserId == UserId).Select(x => x.RoleId).FirstOrDefault();
            if (roleId != null)
            {
                roleName = _roleRepository.GetAll().Where(x => x.Id == roleId).Select(x => x.Name).FirstOrDefault();
            }
            //getting role
            //DateTime currentdate = DateTime.Now;
            //var timeCurrent = currentdate.ToString("yyyy-MM-dd");
            //DateTime dateCurrent = DateTime.Parse(timeCurrent);
            DateTime dateCurrent = Clock.Now;
            //var timeCurrent = currentdate.ToString("yyyy-MM-dd");
            //DateTime dateCurrent = DateTime.Parse(timeCurrent);
            //var dateCurrent = _timeZoneConverter.Convert(currentdate, _session.TenantId, _session.GetUserId());
            roleName = roleName.ToLower();

            if (roleName == "supervisor")
            {
                var entity = (from um in _userRepository.GetAll()
                              join u in _userloginRepository.GetAll().Where(x => x.LoggedIn != null && DateTime.Compare(dateCurrent.Date, x.LoggedIn) > 0 && x.TimesheetComplete == 0)
                              on um.Id equals u.UserId
                              join ur in _userRoleRepository.GetAll()
                              on u.UserId equals ur.UserId
                              join r in _roleRepository.GetAll()
                              on ur.RoleId equals r.Id
                              where um.Immediate_supervisorId == UserId && um.IsActive == true && r.Name == "Employee"
                              group um by new
                              {
                                  um.Id,
                                  um.UserName,

                              } into g
                              select new LogInDto
                              {
                                  UserId = (int)g.Key.Id,
                                  UserName = g.Key.UserName,
                                  Count = g.Count()
                              }).ToList();

                return new ListResultDto<LogInDto>(entity.MapTo<List<LogInDto>>());

            }
            else if (roleName == "admin")
            {
                var entity = (from um in _userRepository.GetAll()
                              join u in _userloginRepository.GetAll().Where(x => x.LoggedIn != null && DateTime.Compare(dateCurrent.Date, x.LoggedIn) > 0 && x.TimesheetComplete == 0)
                              on um.Id equals u.UserId
                              join ur in _userRoleRepository.GetAll()
                              on u.UserId equals ur.UserId
                              join r in _roleRepository.GetAll()
                              on ur.RoleId equals r.Id
                              where um.Id != UserId && um.IsActive == true && r.Name == "Employee"
                              group um by new
                              {
                                  um.Id,
                                  um.UserName
                              } into g
                              select new LogInDto
                              {
                                  UserId = (int)g.Key.Id,
                                  UserName = g.Key.UserName,
                                  Count = g.Count()
                              }).ToList();

                return new ListResultDto<LogInDto>(entity.MapTo<List<LogInDto>>());

            }
            return null;
        }

        public ListResultDto<MonthlySalesDto> GetMonthlySales()
        {
            var saleslist = (from u in _userRepository.GetAll()
                             join e1 in _projectRepository.GetAll()
                             on (int)u.Id equals e1.Marketing_LeaderId
                             join e2 in _projectsDetailsRepository.GetAll()
                             on e1.Id equals e2.ProjectId
                             where e1.Marketing_LeaderId != null && e2.CreationTime.Month == DateTime.Now.Month && e2.CreationTime.Year == DateTime.Now.Year
                             group new
                             {
                                 e1.Marketing_LeaderId,
                                 u.Name,
                                 //e2.Price,
                                 Price = (e2.IsOutSource == true ? ((e2.CostforCompany.Value) * 2) : e2.Price),
                                 e2.TypeID,
                                 e2.CostforCompany
                             }
                             by new
                             {
                                 marketingperson = u.Name,
                                 marketingid = e1.Marketing_LeaderId
                             } into g
                             select new MonthlySalesDto
                             {
                                 MarketingLeaderId = g.Key.marketingid.Value,
                                 MarketingLeaderName = g.Key.marketingperson,
                                 //Amount = g.Sum(x => x.Price),
                                 Amount = (decimal)g.Sum(x => x.Price),
                                 NoOfSales = g.Count()
                             }).ToList();

            return new ListResultDto<MonthlySalesDto>(saleslist.MapTo<List<MonthlySalesDto>>());

        }
        // Follow Up 
        public ListResultDto<FollowupHistoryDto> GetFollowUp()
        {
            var sevendaysAgo = DateTime.UtcNow.AddDays(7);
            int curId = (int)_session.UserId;
            //bool checkUser = await _userManager.IsInRoleAsync(curId, "Admin");
            var data = (from a in _OpportunityFollowUpRepository.GetAll().Where(x => x.CreatorUserId == curId)
                        join b in _OpportunityRepository.GetAll() on a.opporutnityid equals b.Id
                        into ab
                        from b in ab.DefaultIfEmpty()
                        join c in _CallCategoriesRepository.GetAll() on b.CalllCategoryId equals c.Id
                        join d in _FollowupIntrestRepository.GetAll() on a.Id equals d.followupid
                        join e in _projecttypeRepository.GetAll() on d.intestedid equals e.Id
                        group e by new { a, c, b } into g
                        select new FollowupHistoryDto
                        {
                            Opporutnityid = g.Key.a.Id,
                            CreationDate = g.Key.a.CreationTime,
                            CalllCategoryId = g.Key.b.CalllCategoryId,
                            CalllCategoryName = g.Key.c.Name,
                            //ProjectType = g.Select(x => x.ProjectTypeName).ToList(),
                            NextActionDate = g.Key.a.nextactiondate.HasValue ? g.Key.a.nextactiondate : null,
                            ClosingDate = g.Key.a.expectedclosingdate.HasValue ? g.Key.a.expectedclosingdate : null,
                            Comment = g.Key.a.Comment,
                            CompanyName = g.Key.b.CompanyName,
                            PersonName = g.Key.b.PersonName,
                            MobileNumber = g.Key.b.MobileNumber,
                            MasterOpporutnityid = g.Key.a.opporutnityid
                        })
                        .Where(p => p.NextActionDate >= DateTime.UtcNow && p.NextActionDate < sevendaysAgo)
                        .Where(p => p.CalllCategoryId == 2 || p.CalllCategoryId == 3 || p.CalllCategoryId == 4)
                       .OrderBy(x => x.NextActionDate)
                       .ToList();
            return new ListResultDto<FollowupHistoryDto>(data.MapTo<List<FollowupHistoryDto>>());
            //throw new NotImplementedException();
        }

        //Sales Report For 7days

        public ListResultDto<TimeSheet.Dto.ProjectDto> GetSalesReport(GetProjectInput input)
        {
            var sevendaysAgo = DateTime.UtcNow.AddDays(-7);

            var tasks = (from e1 in _projectsDetailsRepository.GetAll()
                         join e2 in _projecttypeRepository.GetAll() on e1.TypeID equals e2.Id
                         join e3 in _projectRepository.GetAll() on e1.ProjectId equals e3.Id
                         join e4 in _userRepository.GetAll() on (int)e3.Marketing_LeaderId equals e4.Id
                         select new TimeSheet.Dto.ProjectDto
                         {
                             Id = e1.Id,
                             ProjectName = e3.ProjectName,
                             Price = (e1.IsOutSource == true ? ((e1.CostforCompany.Value) * 2) : e1.Price),
                             Marketing_LeaderName = e4.Name,
                             ptid = e1.TypeID,
                             ProjectType = e2.ProjectTypeName,
                             ProjectCreatedate = e3.CreationTime,
                             ProjectTypedate = e1.CreationTime,

                         })
                         .Where(p => p.ProjectTypedate >= sevendaysAgo && p.ProjectTypedate <= DateTime.UtcNow)
                         .OrderByDescending(p => p.Id).ToList();

            return new ListResultDto<TimeSheet.Dto.ProjectDto>(tasks.MapTo<List<TimeSheet.Dto.ProjectDto>>());
        }

        public ListResultDto<ProjectStatsAmountDto> GetProjectStatsAmount()
        {
            //get actual cost
            var actualcost = _timesheetRepository.GetAll();


            //project list
            var projectlist = (from a in _projectRepository.GetAll()
                               join b in _projectstatusRepository.GetAll()
                               on a.Status equals b.Id
                               join c in _timesheetRepository.GetAll()
                               on a.Id equals c.ProjectId
                               where b.Status.ToLower() == "working" || b.Status.ToLower() == "review" && a.Price > 0
                               group new { a, c }
                               by new { pid = c.ProjectId } into gf
                               let unit = gf.FirstOrDefault()

                               select new ProjectStatsAmountDto
                               {
                                   Id = unit.a.Id,
                                   ProjectName = unit.a.ProjectName,
                                   Amount = unit.a.Price,
                                   Efforts = (decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).DefaultIfEmpty(0).Sum(),
                                   AmtInPer = (unit.a.Price > 0 ? ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).DefaultIfEmpty(0).Sum()) * 100 / unit.a.Price : 0) >= 60 ? true : false,
                               }).Where(x => x.AmtInPer == true)

                               .ToList();


            return new ListResultDto<ProjectStatsAmountDto>(projectlist.MapTo<List<ProjectStatsAmountDto>>());
        }

        public ListResultDto<Project.Dto.ProjectDto> GetProjectStatesHour(GetProjectInput input)
        {

            var actualhoursrepo = _timesheetRepository.GetAll();

            var cc = (from p in _projectRepository.GetAll()
                      join b in _projectstatusRepository.GetAll()
                      on p.Status equals b.Id
                      join c in _timesheetRepository.GetAll()
                      on p.Id equals c.ProjectId
                      where (p.Status == 6 || p.Status == 7) && p.totalhours > 0
                      group new { p, c, b }
                      by new { pid = c.ProjectId } into gf
                      let unit = gf.FirstOrDefault()
                      select new Project.Dto.ProjectDto
                      {
                          Id = unit.p.Id,
                          ProjectName = unit.p.ProjectName,
                          ProjectStatusId = unit.p.Status,
                          ProjectStatus = unit.b.Status,
                          ProjectCost = unit.p.Price,
                          totalhours = unit.p.totalhours,
                          actualhours = actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum(),
                          //sixtyper = (unit.p.totalhours * 60) / 100,
                          //eightyper= (unit.p.totalhours * 80) / 100,
                          // profit= (unit.p.totalhours-(actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).Sum())),
                          // profitper =((actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).Sum())*100)/unit.p.totalhours
                          hourPercentage = unit.p.totalhours > 0 ? ((decimal)actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / unit.p.totalhours : 0,

                      })
                    .Where(x => x.hourPercentage >= 60);

            var ccData = cc.OrderBy(x => x.Id).ToList();

            //var dd = ccData.WhereIf(input.actualhours.HasValue, x => x.actualhours >= 60).WhereIf(input.eightyper.HasValue, x => x.actualhours >= x.eightyper).ToList();

            return new ListResultDto<Project.Dto.ProjectDto>(ccData.MapTo<List<Project.Dto.ProjectDto>>());
        }

        public PagedResultDto<Project.Dto.ProjectDto> GetProjectStatesHourreport(GetProjectInput input)
        {

            var actualhoursrepo = _timesheetRepository.GetAll();

            var cc = (from p in _projectRepository.GetAll()
                      join b in _projectstatusRepository.GetAll()
                      on p.Status equals b.Id
                      join c in _timesheetRepository.GetAll()
                      on p.Id equals c.ProjectId
                      where p.totalhours > 0
                      group new { p, c, b }
                      by new { pid = c.ProjectId } into gf
                      let unit = gf.FirstOrDefault()
                      select new Project.Dto.ProjectDto
                      {
                          Id = unit.p.Id,
                          ProjectName = unit.p.ProjectName,
                          ProjectStatusId = unit.p.Status,
                          ProjectStatus = unit.b.Status,
                          ProjectCost = unit.p.Price,
                          totalhours = unit.p.totalhours,
                          actualhours = actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum(),
                          hourPercentage = unit.p.totalhours > 0 ? ((decimal)actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / unit.p.totalhours : 0,

                          //sixtyper = (unit.p.totalhours * 60) / 100,
                          //eightyper = (unit.p.totalhours * 80) / 100,

                          profit = unit.p.totalhours > 0 ? (unit.p.totalhours - (actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum())) : 0,
                          profitper = unit.p.totalhours > 0 ? ((actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100) / unit.p.totalhours : 0

                          //profitper = unit.p.totalhours > 0 ? (((unit.p.totalhours-(actualhoursrepo.Where(s => s.ProjectId == unit.p.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum())) * 100) / unit.p.totalhours) : 0


                          //ProfitPercentage = unit.a.Price > 0 ? (((unit.a.Price) - ((decimal)actualcost.Where(s => s.ProjectId == unit.a.Id).Select(s => s.Efforts).Sum())) * 100 / unit.a.Price) : 0,

                      })
                      .Where(x => x.hourPercentage >= 60)
                      //.Where(x => x.ProjectCost > 0)
                      .WhereIf(input.ProjectStatusId.HasValue, s => s.ProjectStatusId == input.ProjectStatusId.Value);

            //   var datacheck = cc.ToList().Count();
            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            //var ccData = cc.OrderByDescending(x => x.hourPercentage).WhereIf(input.ProjectStatusId.HasValue, s => s.ProjectStatusId == input.ProjectStatusId.Value).ToList(); // vikas

            //var dd = ccData.WhereIf(input.sixtyper.HasValue, x => x.actualhours >= x.sixtyper).WhereIf(input.eightyper.HasValue, x => x.actualhours >= x.eightyper)
            //    .WhereIf(input.actualhours.HasValue, x => x.actualhours >= x.totalhours)
            //    .WhereIf(input.ProjectStatusId.HasValue, s => s.ProjectStatusId == input.ProjectStatusId.Value).ToList();

            return new PagedResultDto<Project.Dto.ProjectDto>(ccCount, ccData.MapTo<List<Project.Dto.ProjectDto>>());
        }
        public ListResultDto<Project.Dto.ProjectDto> StatusList()
        {
            var tenDaysAgo = DateTime.UtcNow.AddDays(10);
            var tasks = (from e1 in _projectstatusRepository.GetAll()


                         select new Project.Dto.ProjectDto
                         {
                             Id = e1.Id,
                             statusname = e1.Status

                         }).OrderBy(x => x.statusname).
                        ToList();
            return new ListResultDto<Project.Dto.ProjectDto>(tasks.MapTo<List<Project.Dto.ProjectDto>>());
        }

        public ListResultDto<MonthlySalesreportDto> GetSalestarget()
        {
            List<MonthlySalesreportDto> saleslist = new List<MonthlySalesreportDto>();
            var targetdata = _userRepository.GetAll().Where(e => e.TargetAmount != null).Select(e => e.TargetAmount).Sum();
            if (targetdata == null)
            { targetdata = 0; }
            int[] months = new int[12] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3 };
            List<int> financemonth = months.ToList<int>();
            var finyear = 0;
            //DateTime currentDate = DateTime.Now.AddMonths(1);
            //DateTime currentDate = DateTime.Now.AddMonths(-1);
            DateTime currentDate = DateTime.Now;
            for (int i = 0; i < financemonth.Count; i++)
            {
                int month = financemonth[i];

                var finmonth = month;
                if (currentDate.Month > 3 && finmonth>3)
                {
                    finyear = currentDate.Year;
                }
               else if (currentDate.Month > 3 && finmonth <=3)
                {
                    var dt = currentDate.AddYears(1);
                    finyear = dt.Year;

                }
                else if (currentDate.Month <= 3 && finmonth > 3)
                {
                    var dt = currentDate.AddYears(-1);
                    finyear = dt.Year;
                }

                else if (currentDate.Month <= 3 && finmonth <= 3)
                {
                    finyear = currentDate.Year;
                }
                string mnthname = getAbbreviatedName(month) + "-" + finyear;
                MonthlySalesreportDto salesdata = new MonthlySalesreportDto();

                var salesdt = (from e1 in _projectRepository.GetAll()
                               join e2 in _projectsDetailsRepository.GetAll()
                               on e1.Id equals e2.ProjectId
                               where e1.Marketing_LeaderId != null && e2.CreationTime.Year == finyear && e2.CreationTime.Month == month
                               group new
                               {
                                   // e2.Price,
                                   Price = (e2.IsOutSource.Value == true ? ((e2.CostforCompany.Value) * 2) : e2.Price),
                                   e2.CostforCompany,
                                   e2.IsOutSource
                               }
                            by new
                            {
                                month = e2.CreationTime.Month
                            } into g
                               select new MonthlySalesreportDto
                               {
                                   y = mnthname,
                                   //  month = i,
                                   Amount = g.Sum(x => x.Price == null ? 0 : x.Price),
                                   // Amount = (decimal)g.Sum(x => x.IsOutSource == false ?   (x.Price == null ? 0 : x.Price) : (x.CostforCompany == null ? 0 : x.CostforCompany)),
                                   TargetAmount = (decimal)targetdata
                               }).FirstOrDefault();
                if (salesdt == null)
                {
                    salesdata.y = mnthname; salesdata.Amount = 0; salesdata.TargetAmount = (decimal)targetdata;
                    saleslist.Add(salesdata);
                }
                else
                {
                    saleslist.Add(salesdt);
                }

            }

            return new ListResultDto<MonthlySalesreportDto>(saleslist.MapTo<List<MonthlySalesreportDto>>());

        }

        public ListResultDto<morrisDto> GetSalestargetmoriss()
        {
            var saleslist = (from u in _userRepository.GetAll()
                             join e1 in _projectRepository.GetAll()
                             on (int)u.Id equals e1.Marketing_LeaderId
                             join e2 in _projectsDetailsRepository.GetAll()
                             on e1.Id equals e2.ProjectId
                             where e1.Marketing_LeaderId != null && e2.CreationTime.Month == DateTime.Now.Month && e2.CreationTime.Year == DateTime.Now.Year
                             group new
                             {
                                 e1.Marketing_LeaderId,
                                 u.Name,
                                 e2.Price,
                                 e2.TypeID
                             }
                             by new
                             {
                                 marketingperson = u.Name,
                                 marketingid = e1.Marketing_LeaderId
                             } into g
                             select new morrisDto
                             {

                                 y = g.Key.marketingperson,
                                 a = (int)g.Sum(x => x.Price),

                             }).ToList();

            return new ListResultDto<morrisDto>(saleslist.MapTo<List<morrisDto>>());
        }
        public ListResultDto<ListDataDto> GetServiceNameWithoutClient(GetServiceInput input)
        {
            var monthAgo = DateTime.UtcNow.AddDays(30);
            var tasks = (from e1 in _manageserviceRepository.GetAll()
                         join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
                          into distservice
                         from e2 in distservice.DefaultIfEmpty()

                         where (/*e1.Cancelflag == false*/ e1.ClientId == 0)
                         select new ListDataDto
                         {
                             Id = e1.Id,
                             DomainName = e1.DomainName,
                             ServiceId = e1.ServiceId,
                             ServiceName = e2.Name,


                         })


              .OrderByDescending(p => p.Id).ToList();
            return new ListResultDto<ListDataDto>(tasks.MapTo<List<ListDataDto>>());
        }

        public PagedResultDto<Users.Dto.UserDto> GetUserRenewdata(GetUserInputDto Input)
        {
            DateTime todaysDate = DateTime.Now;
            DateTime nextDate = DateTime.Now.AddDays(75);
            var user = _userRepository.GetAll().Where(x => x.Next_Renewaldate <= nextDate)
                                               .Where(x => x.IsActive == true)
                                               .Where(x => x.Next_Renewaldate != null)
                                               .OrderByDescending(x => x.Id)
                            .WhereIf(!Input.Name.IsNullOrEmpty(), p => p.Name.ToLower().Contains(Input.Name.ToLower()))

                           //.PageBy(Input)
                           .OrderBy(Input.Sorting)
                           .ToList();

            var count = user.Count();

            return new PagedResultDto<Users.Dto.UserDto>(count, user.MapTo<List<Users.Dto.UserDto>>());


        }

        public async Task UpdateDashboardService(int id)
        {
            try
            {
                var per = await _manageserviceRepository.FirstOrDefaultAsync(id);

                per.Cancelflag = true;
                per.CancelDate = DateTime.Now;
                //per.RenewalDate = DateTime.Now;

                await _manageserviceRepository.UpdateAsync(per);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ListResultDto<ListDataDto> GetDomainDate(GetServiceInput input)
        {
            var monthAgo = DateTime.UtcNow.AddDays(60);

            var tasks = (from e1 in _manageserviceRepository.GetAll()

                         join servireq in _serviceRequestRepository.GetAll() on e1.Id equals servireq.ServiceId
                         into serviceRequest

                         join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
                          into distservice
                         from e2 in distservice.DefaultIfEmpty()
                         join e3 in _clientRepository.GetAll() on e1.ClientId equals e3.Id
                          into distclient
                         from e3 in distclient.DefaultIfEmpty()
                         join e4 in _userRepository.GetAll() on e1.EmployeeId equals e4.Id
                          into distuser
                         from e4 in distuser.DefaultIfEmpty()
                         join e5 in _serverRepository.GetAll() on e1.ServerType equals e5.Id
                          into distserver
                         from e5 in distserver.DefaultIfEmpty()
                         join e6 in _invoiceRequestRepository.GetAll()
                           on e1.Id equals e6.ServiceId
                           into invoicerequest
                         join e7 in _typenameRepository.GetAll() on e1.TypeName equals e7.Id
                             into distrtype
                         from e7 in distrtype.DefaultIfEmpty()
                         where (/*e1.ServiceId==1 && */e1.Cancelflag == false)
                         select new ListDataDto
                         {
                             Id = e1.Id,
                             DomainName = e1.DomainName,
                             Price = e1.Price,
                             HostingSpace = e1.HostingSpace,
                             ServiceId = e1.ServiceId,
                             ServiceName = e2.Name,
                             NextRenewalDate = e1.NextRenewalDate,
                             ClientId = e1.ClientId,
                             ClientName = e3.ClientName,
                             EmployeeId = e1.EmployeeId,
                             EmployeeName = e4.Name + " " + e4.Surname,
                             ServerName = e5.Name,
                             TypeName = e1.TypeName,
                             NoOfEmail = e1.NoOfEmail,
                             Typeofssl = e1.Typeofssl,
                             Title = e1.Title,
                             Cancelflag = e1.Cancelflag,
                             Actionstatus = serviceRequest.OrderByDescending(x => x.Id).Select(x => x.Actiontype).FirstOrDefault(),
                             Status = invoicerequest.OrderByDescending(x => x.Id).Select(x => x.Status).FirstOrDefault(),
                             Credits = e1.Credits,
                             DatabaseSpace = e1.DatabaseSpace,
                             displayTypename1 = e7.Name,
                             IsAutoRenewal = e1.IsAutoRenewal,
                             //StatusName = serviceRequest.OrderByDescending(x => x.Id).Select(x => x.ActionName).FirstOrDefault()
                         })
                        .WhereIf(input.ServiceId.HasValue, x => x.ServiceId == input.ServiceId)
                        //.WhereIf(tasks.ServiceId==2, x => x.NextRenewalDate == input.NextRenewalDate)
                        .WhereIf(!input.DomainName.IsNullOrEmpty(), p => p.DomainName.ToLower().Contains(input.DomainName.ToLower()))
                        .Where(p => p.NextRenewalDate <= monthAgo)
              //.OrderBy(P=>P.NextRenewalDate).ThenBy(P=>P.DomainName)

              .OrderBy(p => p.NextRenewalDate).ThenBy(x => x.DomainName).ToList();

            return new ListResultDto<ListDataDto>(tasks.MapTo<List<ListDataDto>>());
        }

        public decimal leaveBalance()
        {
            decimal LeaveBalance = _userRepository.GetAll().Where(x => x.Id == (int)_session.UserId).Select(x => x.LeaveBalance).FirstOrDefault();
            return LeaveBalance;
        }
        public decimal PendingLeave()
        {
            decimal pendingleave = _userRepository.GetAll().Where(x => x.Id == (int)_session.UserId).Select(x => x.PendingLeaves).FirstOrDefault();
            return pendingleave;
        }


        public ListResultDto<EmployeeAssignToUserstoryDto> GetAssignUserstoryEmployeewise(GetImportUserstoryDto Input)
        {
            int UserId = (int)_session.UserId;
            var actualhours = _timesheetRepository.GetAll().Where(s => s.UserId == UserId)
                            .GroupBy(a => a.UserStoryId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), UserStoryId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

            var cc = (from i in _importUserDataRepository.GetAll()
                      join p in _projectRepository.GetAll()
                      on i.ProjectId equals p.Id
                      where p.Id == i.ProjectId
                      join u in _userRepository.GetAll()
                      on i.EmployeeId equals u.Id into us
                      from u in us.DefaultIfEmpty()
                      join ur in _userRoleRepository.GetAll()
                      on u.Id equals ur.UserId
                      join r in _roleRepository.GetAll()
                      on ur.RoleId equals r.Id
                      where u.Id == UserId && u.IsActive == true && i.status == 0 && r.Name == "Employee"
                      select new EmployeeAssignToUserstoryDto
                      {
                          Id = i.Id,
                          ProjectId = i.ProjectId,
                          ProjectName = p.ProjectName,
                          UserStory = i.UserStory,
                          //EmployeeId = i.EmployeeId,
                          //UserName = u.Name + " " + u.Surname,
                          DeveloperHours = i.DeveloperHours.HasValue ? i.DeveloperHours.Value : 0,
                          //ExpectedHours = i.ExpectedHours.HasValue ? i.ExpectedHours.Value : 0,
                          ActualHours = i.ActualHours.HasValue ? i.ActualHours.Value : 0,
                          //status = i.status

                      }).ToList();

            foreach (var item in cc)
            {
                item.ActualHours = actualhours.Where(x => x.UserStoryId == item.Id).Select(x => x.Hours).FirstOrDefault();

            }
            return new ListResultDto<EmployeeAssignToUserstoryDto>(cc.MapTo<List<EmployeeAssignToUserstoryDto>>());
        }


        //public List<SumDto> GetCollectionSum()
        //{

        //    try
        //    {
        //        List<SumDto> mSumDtoList = new List<SumDto>();

        //        DateTime _LoopDate = new DateTime(Convert.ToInt32(DateTime.Now.Year - 1), 04, 01);
        //        DateTime _EndDate = new DateTime(Convert.ToInt32(DateTime.Now.Year), 03, 31);


        //        var d =  (from b in _billRepository.GetAll()
        //                               join
        //                               bpr in _billpymtRepository.GetAll()
        //                               on b.BillNo equals bpr.BillNo 
        //                               where bpr.RcptDate >= _LoopDate && bpr.RcptDate <= _EndDate && b.BillDate >= _LoopDate && b.BillDate <= _EndDate && bpr.RcptDate!=null && b.BillDate!=null
        //                               group new { b.totalbillamount, bpr.PymtRecd } 
        //                               by new  { bpr.RcptDate.Value.Month, bpr.RcptDate.Value.Year } into grp
        //                  select new SumDto
        //                               {
        //                                   totalinvoice = grp.Sum(e => e.totalbillamount),
        //                                   totalcollection = grp.Sum(e => e.PymtRecd),
        //                                   totaloutstanding = grp.Sum(e => e.totalbillamount) - grp.Sum(e => e.PymtRecd),
        //                                   //Date = bpr.RcptDate
        //                               }).ToList();




        //        return d;


        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}

        public decimal? GetTotalInvoice()
        {
            try
            {
                DateTime _LoopDate, _EndDate;

                int cm = DateTime.Now.Month;   //getting current month
                if (cm >= 4) //April or after April
                {
                    _LoopDate = new DateTime(Convert.ToInt32(DateTime.Now.Year), 04, 01);
                    _EndDate = new DateTime(Convert.ToInt32(DateTime.Now.Year + 1), 03, 31);
                }
                else
                {
                    _LoopDate = new DateTime(Convert.ToInt32(DateTime.Now.Year - 1), 04, 01);
                    _EndDate = new DateTime(Convert.ToInt32(DateTime.Now.Year), 03, 31);
                }
                //var billrepo = _billRepository.GetAll()
                //    .Where(c => (c.BillDate >= _LoopDate && c.BillDate <= _EndDate) && c.BillDate != null)
                //    .Select(d => d.totalbillamount).Sum();

                var billrepo = (from a in _billRepository.GetAll()
                                join b in _performainvoiceRepository.GetAll() on a.Performaid equals b.Id into ab
                                from b in ab.DefaultIfEmpty()
                                where b.IsMarkAsConfirm != false && (a.BillDate >= _LoopDate && a.BillDate <= _EndDate) && a.BillDate != null
                                select a).ToList();
                decimal billamount = billrepo.Select(x => x.totalbillamount.HasValue ? x.totalbillamount.Value : 0).Sum();

                var performaamount = _performainvoiceRepository.GetAll()
                   .Where(c => (c.BillDate >= _LoopDate && c.BillDate <= _EndDate) && c.BillDate != null && c.IsMarkAsConfirm == false)
                   .Select(d => d.totalbillamount).ToList().Sum();

                return billamount + performaamount;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public decimal? GetCollectionSum()
        {

            try
            {
                DateTime _LoopDate, _EndDate;

                int cm = DateTime.Now.Month;   //getting current month
                if (cm >= 4) //April or after April
                {
                    _LoopDate = new DateTime(Convert.ToInt32(DateTime.Now.Year), 04, 01);
                    _EndDate = new DateTime(Convert.ToInt32(DateTime.Now.Year + 1), 03, 31);
                }
                else
                {
                    _LoopDate = new DateTime(Convert.ToInt32(DateTime.Now.Year - 1), 04, 01);
                    _EndDate = new DateTime(Convert.ToInt32(DateTime.Now.Year), 03, 31);
                }

                // var billpayrepo = _billpymtRepository.GetAll().Where(c => c.RcptDate >= _LoopDate && c.RcptDate <= _EndDate && c.RcptDate != null).Select(d => d.PymtRecd).Sum();
                //var billpymtrepo = _billpymtRepository.GetAll();
                //var invosum = billrepo.Select(c => c.totalbillamount).Sum();
                //decimal? pymtrec = Convert.ToDecimal(billpymtrepo.Select(c => c.PymtRecd).Sum());

                //var outstanding = invosum - pymtrec;

                var billpayrepo1 = (from b in _billRepository.GetAll()
                                    join bpr in _billpymtRepository.GetAll() on b.BillNo equals bpr.BillNo into t
                                    from rt in t.DefaultIfEmpty()
                                    where b.BillDate >= _LoopDate && b.BillDate <= _EndDate && b.BillDate != null
                                    select new
                                    { pymtreceived = rt.PymtRecd.HasValue ? rt.PymtRecd : 0 }).ToList();


                decimal? billpayrepo = Convert.ToDecimal(billpayrepo1.Select(c => c.pymtreceived).Sum());
                return billpayrepo;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //public List<decimal> GetCollectionReport()
        //{
        //    var d = _billpymtRepository.GetAll();
        //    var data1 = (from bpr in _billpymtRepository.GetAll()
        //                 join b in _billRepository.GetAll()
        //                 on bpr.BillNo equals b.BillNo
        //                 //join cli in _clientRepository.GetAll()
        //                 //on b.ClientID equals cli.Id
        //                 where bpr.RcptDate.Value.Year == DateTime.Now.Year
        //                 //select new { bpr.PymtRecd,bpr.RcptDate.Value }).AsQueryable()
        //                 .Select(k => new { k.PymtRecd, k.Value.Month, k.Value.Year, })

        //                    {

        //                        tCharge = d.Sum(k => k.PymtRecd),
        //                        //ClientName = key.ClientName,
        //                    }).FirstOrDefault();



        //    return data1;
        //} 

        public decimal? GetTotalOutstanding()
        {
            try
            {
                decimal? invosum = GetTotalInvoice();
                decimal? pymtrec = GetCollectionSum();

                var outstanding = invosum - pymtrec;

                return outstanding;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        static string getAbbreviatedName(int month)
        {
            DateTime date = new DateTime(DateTime.Now.Year, month, 1);

            return date.ToString("MMM");
        }

        public string LeaveUpteDate()
        {
            var LeaveUpdateDt = _userRepository.GetAll().Where(x => x.Id == (int)_session.UserId).Select(x => x.LeaveUpdateDate).FirstOrDefault();
            if (LeaveUpdateDt != null)
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(LeaveUpdateDt);
                var data = dt.ToString("dd/MM/yyyy");
                var monthname = dt.AddMonths(-1).ToString("MMMM");
                return monthname;
            }
            else
            {
                return null;
            }

        }


    }
}
