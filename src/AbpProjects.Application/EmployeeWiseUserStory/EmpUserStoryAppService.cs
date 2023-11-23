using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using AbpProjects.Authorization;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.Project;
using AbpProjects.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.AutoMapper;
using AbpProjects.TimeSheet.Dto;
using AbpProjects.WorkType;
using System.Linq.Dynamic.Core;
using AbpProjects.Authorization.Users;
using Abp.Extensions;

namespace AbpProjects.EmployeeWiseUserStory
{
    [AbpAuthorize(PermissionNames.Pages_Employeewise_UserStory)]
    public class EmpUserStoryAppService : IEmpUserStoryAppService
    {
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<worktype> _worktypeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ImportUserStoryDetails> _importUserStoryRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IAbpSession _session;
        private readonly PermissionChecker _permissionChecker;
        public EmpUserStoryAppService(IRepository<project> projectRepository,
                IRepository<worktype> worktypeRepository,
                IRepository<User, long> userRepository,
                IRepository<ImportUserStoryDetails> importUserStoryRepository,
                IRepository<timesheet> timesheetRepository,
                IAbpSession session,
                PermissionChecker permissionChecker
                )
        {
            _projectRepository = projectRepository;
            _worktypeRepository = worktypeRepository;
            _userRepository = userRepository;
            _importUserStoryRepository = importUserStoryRepository;
            _timesheetRepository = timesheetRepository;
            _session = session;
            _permissionChecker = permissionChecker;
        }
        public PagedResultDto<ImportUserStoryDto> GetEmployeeUserStoryReport(GetImportUserstoryDto Input)
        {
            int curId = (int)_session.UserId;

            //var actualhours = _timesheetRepository.GetAll()
            //                .GroupBy(a => a.UserStoryId)
            //                .Select(a => new { Hours = a.Sum(b => b.Hours), UserStoryId = a.Key })
            //                .OrderByDescending(a => a.Hours)
            //                .ToList();

            var frmdate = Input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : Input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = Input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : Input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            var cc = (from i in _importUserStoryRepository.GetAll()
                      join p in _projectRepository.GetAll()
                      on i.ProjectId equals p.Id
                      join ts in _timesheetRepository.GetAll().Where(x => x.UserId == curId)
                      on i.Id equals ts.UserStoryId into tsjoin

                      select new ImportUserStoryDto
                      {
                          Id = i.Id,
                          ProjectId = i.ProjectId,
                          ProjectName = p.ProjectName,
                          UserStory = i.UserStory,
                          CreationDate = i.CreationTime,
                          DeveloperHours = i.DeveloperHours.HasValue ? i.DeveloperHours.Value : 0,
                          ExpectedHours = i.ExpectedHours.HasValue ? i.ExpectedHours.Value : 0,
                          ActualHours = tsjoin.Where(x => x.UserStoryId == i.Id).Sum(x => x.Hours),
                          Userstorycount = tsjoin.Count(),
                          status = i.status,
                      }).Distinct()
                      .Where(x => x.Userstorycount > 0)
                      .WhereIf(Input.ProjectId.HasValue, s => s.ProjectId == Input.ProjectId.Value)
                      .WhereIf(!Input.UserStory.IsNullOrEmpty(), p => p.UserStory.ToLower().Contains(Input.UserStory.ToLower()))
                      .WhereIf(Input.FromDate.HasValue, p => p.CreationDate >= dtfrm)
                      .WhereIf(Input.ToDate.HasValue, p => p.CreationDate <= dt)
                      .WhereIf(Input.status.HasValue, p => p.status == Input.status.Value);
           
            var userData = cc.OrderBy(Input.Sorting).PageBy(Input).ToList();
            var userCount = cc.Count();
            return new PagedResultDto<ImportUserStoryDto>(userCount, userData.MapTo<List<ImportUserStoryDto>>());
        }

        public PagedResultDto<TimeSheetDto> GetUserStoryDetailsList(GetTimeSheetDto input)
        {
            var Query = (from a in _timesheetRepository.GetAll()
                         join b in _projectRepository.GetAll()
                          on a.ProjectId equals b.Id
                         join c in _worktypeRepository.GetAll()
                         on a.WorkTypeId equals c.Id
                         join u in _userRepository.GetAll()
                         on a.UserId equals u.Id
                         join d in _importUserStoryRepository.GetAll()
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
                             Hours = a.Hours,
                             Date = a.Date,
                             UserStoryId = a.UserStoryId,
                             UserId = a.UserId,
                             UserName = u.UserName,
                         });
            int curId = (int)_session.UserId;
            if (_permissionChecker.IsGranted("Pages.Employeewise.UserStory"))
            {
                Query = Query.Where(x => x.UserId == curId);
            }

            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<TimeSheetDto>(userCount, userData.MapTo<List<TimeSheetDto>>());
        }
    }
}

