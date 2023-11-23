using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Extensions;
using AbpProjects.Project;
using Abp.Domain.Repositories;
using AbpProjects.WorkType;
using AbpProjects.TimeSheet.Dto;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AbpProjects.Authorization.Users;
using Abp.Runtime.Session;
using Abp.Authorization.Users;
using AbpProjects.Authorization.Roles;
using Abp.Authorization;
using AbpProjects.UserLoginDetails;
using Abp.Timing;
using Abp.Timing.Timezone;
using AbpProjects.Authorization;

namespace AbpProjects.TimeSheet
{
    [AbpAuthorize]
    public class TimeSheetAppService : AbpProjectsApplicationModule, ITimeSheetAppService
    {
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<worktype> _worktypeRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<UserLoginData> _userloginRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<ImportUserStoryDetails> _importUserDataRepository;
        private readonly ITimeZoneConverter _timeZoneConverter;
        //private readonly ITimeZoneService _timeZoneService;
        private readonly PermissionChecker _permissionChecker;
        private readonly IAbpSession _session;
        public TimeSheetAppService(ITimeZoneConverter timeZoneConverter, IRepository<project> projectRepository, PermissionChecker permissionChecker, IRepository<worktype> worktypeRepository, IRepository<timesheet> timesheetRepository, IRepository<User, long> userRepository, IRepository<UserRole, long> userRoleRepository, IRepository<UserLoginData> userloginRepository, IRepository<Role> roleRepository, IRepository<ImportUserStoryDetails> importUserDataRepository, IAbpSession session)
        {
            _projectRepository = projectRepository;
            _worktypeRepository = worktypeRepository;
            _timesheetRepository = timesheetRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userloginRepository = userloginRepository;
            _roleRepository = roleRepository;
            _importUserDataRepository = importUserDataRepository;
            _session = session;
            _timeZoneConverter = timeZoneConverter;
            _permissionChecker = permissionChecker;


        }

        public async Task CreateTimeSheet(CreateTimeSheetDto input)
        {
            int curId = (int)_session.UserId;
            input.UserId = curId;
            var result = input.MapTo<timesheet>();
            var salary = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x.Salary_Hour).FirstOrDefault();
            decimal totalefforts = salary * input.Hours;

            var getdata = _userloginRepository.GetAll().Where(x => x.UserId == curId && (x.LoggedIn.Year == input.Date.Year && x.LoggedIn.Month == input.Date.Month && x.LoggedIn.Day == input.Date.Day)).FirstOrDefault();
            if (getdata != null)
            {
                var currentDate = DateTime.Now;
                //if (getdata.TimesheetComplete == 0)
                //{
                if (getdata.LoggedIn.Year == currentDate.Year && getdata.LoggedIn.Month == currentDate.Month && getdata.LoggedIn.Day == currentDate.Day)
                {
                    getdata.TimesheetComplete = 2;
                }
                else
                {
                    getdata.TimesheetComplete = 1;
                }
                _userloginRepository.UpdateAsync(getdata);

                //}
            }
           
            result.Efforts = totalefforts;

            await _timesheetRepository.InsertAsync(result);
        }

        public async Task DeleteTimeSheet(EntityDto input)
        {
            await _timesheetRepository.DeleteAsync(input.Id);
        }

        public async Task<TimeSheetDto> GetDataById(EntityDto input)
        {
            var c = (await _timesheetRepository.GetAsync(input.Id)).MapTo<TimeSheetDto>();
            return c;
        }

        public List<ProjectDto> GetProject()
        {
            var projectlist = (from a in _projectRepository.GetAll()
                               where a.IsEnable == 0
                               select new ProjectDto
                               {
                                   Id = a.Id,
                                   ProjectName = a.ProjectName,
                               }).OrderBy(x => x.ProjectName).ToList();
            return projectlist;
        }

        public async Task<TimeSheetDto> GetTimeSheet(EntityDto input)
        {
            var timesheets = (from a in _timesheetRepository.GetAll()
                              join b in _projectRepository.GetAll()
                              on a.ProjectId equals b.Id
                              join c in _worktypeRepository.GetAll()
                              on a.WorkTypeId equals c.Id
                              join us in _importUserDataRepository.GetAll()
                              on a.UserStoryId equals us.Id
                              into userStory
                              from us in userStory.DefaultIfEmpty()
                              where input.Id == a.Id
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
                                  UserStory = us.UserStory
                              }).FirstOrDefault();

            TimeSheetDto obj = new TimeSheetDto();
            obj.Id = timesheets.Id;
            obj.ProjectId = timesheets.ProjectId;
            obj.ProjectName = timesheets.ProjectName;
            obj.WorkTypeId = timesheets.WorkTypeId;
            obj.WorkTypeName = timesheets.WorkTypeName;
            obj.Description = timesheets.Description;
            obj.Hours = timesheets.Hours;
            obj.Date = timesheets.Date;
            obj.UserStory = timesheets.UserStory;
            return obj;
        }

        public PagedResultDto<TimeSheetDto> GetTimeSheetData(GetTimeSheetDto input)
        {
            var prevDate = DateTime.Now.AddDays(-4);
            string roleName = "";
            int curId = (int)_session.UserId;

            //if (input.UserId == 0)
            //{
            //    input.UserId = curId;
            //}
            var roleId = _userRoleRepository.GetAll().Where(x => x.UserId == curId).Select(x => x.RoleId).FirstOrDefault();
            if (roleId != null)
            {
                roleName = _roleRepository.GetAll().Where(x => x.Id == roleId).Select(x => x.Name).FirstOrDefault();
            }
            roleName = roleName.ToLower();
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");
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
                             IsEdit = a.Date >= prevDate ? true : false,
                             UserId = a.UserId,
                         })
                        .WhereIf(!input.Filter.IsNullOrEmpty(), p => p.ProjectName.ToLower().Contains(input.Filter.ToLower()))
                        .WhereIf(input.FromDate.HasValue, p => p.Date >= dtfrm)
                         .WhereIf(input.ToDate.HasValue, p => p.Date <= dt)
                        .WhereIf(input.UserId != null, p => p.UserId == input.UserId)
                        .WhereIf(roleName != "admin" && input.UserId != null, p => p.UserId == input.UserId)
                        .WhereIf(roleName != "admin" && input.UserId == null, p => p.UserId == curId);
            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<TimeSheetDto>(userCount, userData.MapTo<List<TimeSheetDto>>());
        }

        public PagedResultDto<TimeSheetDto> GetUserStoryDetailsList(GetTimeSheetDto input)
        {
            var Query = (from a in _timesheetRepository.GetAll().WhereIf(input.EmployeeId.HasValue, s => s.UserId == input.EmployeeId.Value)
                         join b in _projectRepository.GetAll()
                          on a.ProjectId equals b.Id
                         join c in _worktypeRepository.GetAll()
                         on a.WorkTypeId equals c.Id
                         join u in _userRepository.GetAll()
                         on a.UserId equals u.Id
                         join d in _importUserDataRepository.GetAll()
                         on a.UserStoryId equals d.Id
                         where a.UserStoryId == input.Id
                         select new TimeSheetDto
                         {
                             Id = a.Id,
                             ProjectId = a.ProjectId,
                             ProjectName = b.ProjectName,
                             WorkTypeId = c.Id,
                             WorkTypeName = c.WorkTypeName,
                             Description = a.Description,
                             EmployeeId = d.EmployeeId,
                             Hours = a.Hours,
                             Date = a.Date,
                             UserStoryId = a.UserStoryId,
                             //IsEdit = a.Date >= prevDate ? true : false,
                             UserId = a.UserId,
                             UserName = u.UserName,
                         });
                         //.WhereIf(input.EmployeeId.HasValue, s => s.UserId == input.EmployeeId.Value);
                         //.WhereIf(input.Id.HasValue, s => s.UserStoryId == input.Id.Value);

            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<TimeSheetDto>(userCount, userData.MapTo<List<TimeSheetDto>>());
        }

        public List<WorkTypeDto> GetWorkType()
        {
            var worklist = (from a in _worktypeRepository.GetAll()
                            select new WorkTypeDto
                            {
                                Id = a.Id,
                                WorkTypeName = a.WorkTypeName,
                            }).ToList();
            return worklist;
        }
        public async Task UpdateTimeSheet(EditTimeSheetDto input)
        {
            //var currentDate = input.Date;
            //var prevDate = DateTime.Now.AddDays(-4);
            //if (!(currentDate >= prevDate))
            //{
            //    throw new UserFriendlyException("Only Last Three Days Timesheet Allow to Changed");
            //}
            int curId = (int)_session.UserId;
            input.UserId = curId;
            var Tests = await _timesheetRepository.GetAsync(input.Id);
            var salary = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x.Salary_Hour).FirstOrDefault();
            decimal totalefforts = salary * input.Hours;
            Tests.Id = input.Id;
            Tests.WorkTypeId = input.WorkTypeId;
            Tests.ProjectId = input.ProjectId;
            Tests.UserStoryId = input.UserStoryId.GetValueOrDefault();
            Tests.Description = input.Description;
            Tests.Hours = input.Hours;
            Tests.Date = input.Date;
            Tests.Efforts = totalefforts;
            await _timesheetRepository.UpdateAsync(Tests);
        }

        public IEnumerable<User> GetChildUsers(long id)
        {
            var ParentUser = _userRepository.GetAll().Where(x => x.Immediate_supervisorId == id).ToList();
            var childUsers = ParentUser.AsEnumerable().Union(_userRepository.GetAll().ToList().AsEnumerable().Where(x => x.Immediate_supervisorId == id).SelectMany(y => GetChildUsers(y.Id))).ToList();
            return childUsers;
        }
        public List<UserDto> GetUser()
        {
            long curId = (int)_session.UserId;




            var curUser = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x.UserName).FirstOrDefault();

            var cc = (from u in _userRepository.GetAll()
                      join ur in _userRoleRepository.GetAll()
                      on u.Id equals ur.UserId
                      join r in _roleRepository.GetAll()
                      on ur.RoleId equals r.Id
                      where u.Id == curId
                      select r).ToList();

            UserDto obj = new UserDto();

            if (cc.Any(x => x.Name.ToLower() == "admin"))
            {
                var userlist = (from a in _userRepository.GetAll()
                                where a.UserName != "admin" && a.Id != curId && a.IsActive == true
                                orderby a.UserName
                                select new UserDto
                                {
                                    Id = a.Id,
                                    Name = a.Name + " " + a.Surname,
                                    UserName = a.UserName,
                                    RoleName = "admin",
                                }).OrderBy(x => x.Name).ToList();

                return userlist;

            }

            //else if (cc.Any(x => x.Name.ToLower() == "supervisor"))
            //{
            //    var userlist = GetUserByImmediateSupId((int)curId);
            //    var resultlist = GetUserUnderImmediateSup(userlist);
            //    var finallist = resultlist.OrderBy(x => x.UserName).ToList();
            //    return finallist;
            //}

            else
            {
                var userlistTimeSheet = (from a in GetChildUsers(curId)
                                         select new UserDto
                                         {
                                             Id = a.Id,
                                             Name = a.Name + " " + a.Surname,
                                             UserName = a.UserName,

                                         }).OrderBy(x => x.Name).ToList();

                return userlistTimeSheet;
                //var userlist = (from a in _userRepository.GetAll()
                //                where a.Id == curId && a.UserName != curUser && a.IsActive == true

                //                select new UserDto
                //                {
                //                    Id = a.Id,
                //                    Name = a.Name + " " + a.Surname,
                //                    UserName = a.UserName,

                //                }).ToList();
                //return userlist;
            }
        }
        public List<UserDto> GetUserUnderImmediateSup(List<UserDto> userlist)
        {
            List<UserDto> finallist = new List<UserDto>();
            List<UserDto> ulist = new List<UserDto>();
            if (userlist != null)
            {
                foreach (var item in userlist)
                {
                    finallist.Add(item);
                    var newuserlist = GetUserByImmediateSupId((int)item.Id);
                    ulist = GetUserUnderImmediateSup(newuserlist);
                    if (ulist != null)
                    {
                        foreach (var i in ulist)
                        {
                            finallist.Add(i);
                        }
                    }

                }
                return finallist;

            }

            else
            {
                return finallist;
            }

        }
        public List<UserDto> GetUserByImmediateSupId(int id)
        {
            var userlist = (from sup in _userRepository.GetAll()
                            join emp in _userRepository.GetAll()
                            on sup.Immediate_supervisorId equals emp.Immediate_supervisorId into seluser
                            from emp in seluser.DefaultIfEmpty()
                                //join ur in _userRoleRepository.GetAll()
                                //on emp.Id equals ur.UserId
                                //join r in _roleRepository.GetAll()
                                //on ur.RoleId equals r.Id
                            where emp.UserName != "admin" && sup.Immediate_supervisorId == id && sup.IsActive == true && emp.IsActive == true
                            group sup by new
                            {
                                emp.Id,
                                sup.Immediate_supervisorId,
                                emp.Name,
                                emp.Surname,
                                emp.UserName,
                                //r.DisplayName,
                            } into g
                            orderby g.Key.UserName
                            select new UserDto
                            {
                                Id = g.Key.Id,
                                Immediate_supervisorId = g.Key.Immediate_supervisorId,
                                Name = g.Key.Name + " " + g.Key.Surname,
                                UserName = g.Key.UserName,
                                //RoleName = g.Key.DisplayName,
                            }).ToList();

            return userlist;
        }
        //project report
        public PagedResultDto<TimeSheetDto> GetProjectReport(inputmaster inputs)
        {
            try
            {

                var timesheetdetail = _timesheetRepository.GetAll();
                var tasks = (from e1 in _timesheetRepository.GetAll()
                             join e2 in _projectRepository.GetAll() on e1.ProjectId equals e2.Id
                             join e3 in _userRepository.GetAll()
                             on e1.UserId equals e3.Id
                             where (e1.ProjectId == inputs.ProjectId)
                             group new { e1, e2, e3 }

                              by new { userid = e1.UserId } into gf
                             let unit = gf.FirstOrDefault()
                             select new TimeSheetDto
                             {
                                 Id = unit.e1.Id,
                                 ProjectId = unit.e1.ProjectId,
                                 ProjectName = unit.e2.ProjectName,
                                 Hours = timesheetdetail.Where(x => x.UserId == unit.e1.UserId && x.ProjectId == unit.e1.ProjectId).Select(c => c.Hours).Sum(),
                                 //Hours = e1.Hours,
                                 Efforts = timesheetdetail.Where(x => x.UserId == unit.e1.UserId && x.ProjectId == unit.e1.ProjectId).Select(c => c.Efforts).Sum(),
                                 UserId = unit.e1.UserId,
                                 UserName = unit.e3.Name,
                                 ProjectPrice = unit.e2.Price,


                             });




                //var g = tasks.GroupBy(x => x.UserId);
                var totalcount = tasks.Count();
                var servicePageBy = tasks.OrderBy(inputs.Sorting);

                return new PagedResultDto<TimeSheetDto>(totalcount, servicePageBy.MapTo<List<TimeSheetDto>>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<TimeSheetDto> GetProjectReportInExcel(inputmaster inputs)
        {
            try
            {

                var timesheetdetail = _timesheetRepository.GetAll();
                var projectdetail = _projectRepository.GetAll();
                var tasks = (from e1 in _timesheetRepository.GetAll()
                             join e2 in _projectRepository.GetAll() on e1.ProjectId equals e2.Id
                             join e3 in _userRepository.GetAll()
                             on e1.UserId equals e3.Id
                             where (e1.ProjectId == inputs.ProjectId)
                             group new { e1, e2, e3 }

                              by new { userid = e1.UserId } into gf
                             let unit = gf.FirstOrDefault()
                             select new TimeSheetDto
                             {
                                 Id = unit.e1.Id,
                                 ProjectId = unit.e1.ProjectId,
                                 ProjectName = unit.e2.ProjectName,
                                 Hours = timesheetdetail.Where(x => x.UserId == unit.e1.UserId && x.ProjectId == unit.e1.ProjectId).Select(c => c.Hours).Sum(),
                                 Efforts = timesheetdetail.Where(x => x.UserId == unit.e1.UserId && x.ProjectId == unit.e1.ProjectId).Select(c => c.Efforts).Sum(),
                                 UserId = unit.e1.UserId,
                                 UserName = unit.e3.Name,
                                 ProjectPrice = unit.e2.Price,
                                 //totalcount = timesheetdetail.Where(x =>  x.ProjectId == unit.e1.ProjectId).Select(c => c.UserId).Count()
                                 //t=inputs.t

                             }).ToList();

                //var totalcount = tasks.Count();
                //var servicePageBy = tasks.OrderBy(inputs.Sorting);
                return tasks;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ListResultDto<TimeSheetDto> Workdetail(int userId, int projectid)
        {
            var tasks = (from e1 in _timesheetRepository.GetAll()
                         join e2 in _projectRepository.GetAll() on e1.ProjectId equals e2.Id
                         join e3 in _userRepository.GetAll()
                         on e1.UserId equals e3.Id
                         join e4 in _worktypeRepository.GetAll() on e1.WorkTypeId equals e4.Id
                         where (e1.UserId == userId && e1.ProjectId == projectid)

                         select new TimeSheetDto
                         {
                             Id = e1.Id,
                             ProjectId = e1.ProjectId,
                             ProjectName = e2.ProjectName,
                             Hours = e1.Hours,
                             Efforts = e1.Efforts,
                             UserId = e1.UserId,
                             UserName = e3.Name,
                             ProjectPrice = e2.Price,
                             WorkTypeId = e1.WorkTypeId,
                             Description = e1.Description,
                             WorkTypeName = e4.WorkTypeName,
                             Date = e1.Date
                         })

             .OrderByDescending(p => p.Id).ToList();

            return new ListResultDto<TimeSheetDto>(tasks.MapTo<List<TimeSheetDto>>());
        }
    }
}
