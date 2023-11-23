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
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.Project;
using AbpProjects.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AbpProjects.ImportUserStoryData
{
    //[AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Project)]
    public class ImportUserStoryDetailsAppService : IImportUserStoryDetailsAppService
    {
        private readonly IRepository<ImportUserStoryDetails> _importUserDataRepository;
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IAbpSession _session;

        public ImportUserStoryDetailsAppService(IRepository<ImportUserStoryDetails> importUserDataRepository,
            IRepository<project> projectRepository,
            IRepository<timesheet> timesheetRepository,
            IRepository<User, long> userRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IAbpSession session)
        {
            _importUserDataRepository = importUserDataRepository;
            _projectRepository = projectRepository;
            _timesheetRepository = timesheetRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _session = session;
        }

        public async Task AddNewUserStory(AddUserStoryDto inputs)
        {
            ImportUserStoryDetails obj = new ImportUserStoryDetails();
            obj.ProjectId = inputs.Id;
            obj.UserStory = inputs.UserStory;
            obj.DeveloperHours = inputs.DeveloperHours;
            obj.ExpectedHours = inputs.ExpectedHours;
            obj.EmployeeId = inputs.EmployeeId;
            obj.status = 0;
            await _importUserDataRepository.InsertAsync(obj);
        }

        public async Task Delete(EntityDto inputs)
        {
            await _importUserDataRepository.DeleteAsync(x => x.Id == inputs.Id);
        }

        public List<ImportUserStoryDto> ExportUserStory(GetImportUserstoryDto input)
        {
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            var cc = (from i in _importUserDataRepository.GetAll()
                      join p in _projectRepository.GetAll()
                      on i.ProjectId equals p.Id
                      join ts in _timesheetRepository.GetAll().WhereIf(input.EmployeeId.HasValue, s => s.UserId == input.EmployeeId.Value)
                      on i.Id equals ts.UserStoryId into tsjoin
                      select new ImportUserStoryDto
                      {
                          Id = i.Id,
                          ProjectId = i.ProjectId,
                          ProjectName = p.ProjectName,
                          UserStory = i.UserStory,
                          EmployeeId = i.EmployeeId,
                          CreationDate = i.CreationTime,
                          DeveloperHours = i.DeveloperHours.HasValue ? i.DeveloperHours.Value : 0,
                          ExpectedHours = i.ExpectedHours.HasValue ? i.ExpectedHours.Value : 0,
                          ActualHours = tsjoin.Where(x => x.UserStoryId == i.Id).Select(x => x.Hours).DefaultIfEmpty(0).Sum(),
                          Userstorycount = tsjoin.Count(),
                          status = i.status,
                      }).Distinct()
                      .Where(x => x.Userstorycount > 0)
                      .WhereIf(input.ProjectId.HasValue, s => s.ProjectId == input.ProjectId.Value)
                      //.WhereIf(Input.EmployeeId.HasValue, s => s.EmployeeId == Input.EmployeeId.Value);
                      .WhereIf(input.FromDate.HasValue, p => p.CreationDate >= dtfrm)
                      .WhereIf(input.ToDate.HasValue, p => p.CreationDate <= dt)
                      .WhereIf(input.status.HasValue, s => s.status == input.status.Value)
                      .ToList();

            return cc;
        }

        public PagedResultDto<ImportUserStoryDto> GetImportUserStoryData(GetImportUserstoryDto Input)
        {
            var actualhours = _timesheetRepository.GetAll().WhereIf(Input.EmployeeId.HasValue, s => s.UserId == Input.EmployeeId.Value)
                            .GroupBy(a => a.UserStoryId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), UserStoryId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

            var cc = (from i in _importUserDataRepository.GetAll()
                      join p in _projectRepository.GetAll()
                      on i.ProjectId equals p.Id
                      where p.Id == Input.Id
                      join u in _userRepository.GetAll()
                      on i.EmployeeId equals u.Id into us
                      from u in us.DefaultIfEmpty()
                      select new ImportUserStoryDto
                      {
                          Id = i.Id,
                          ProjectId = i.ProjectId,
                          ProjectName = p.ProjectName,
                          UserStory = i.UserStory,
                          EmployeeId = i.EmployeeId,
                          UserName = u.Name + " " + u.Surname,
                          DeveloperHours = i.DeveloperHours.HasValue ? i.DeveloperHours.Value : 0,
                          ExpectedHours = i.ExpectedHours.HasValue ? i.ExpectedHours.Value : 0,
                          ActualHours = i.ActualHours.HasValue ? i.ActualHours.Value : 0,
                          status = i.status

                      })
                      .WhereIf(Input.ProjectId.HasValue, s => s.ProjectId == Input.ProjectId.Value)
                      .WhereIf(Input.status.HasValue, s => s.status == Input.status.Value)
                      .WhereIf(Input.EmployeeId.HasValue && Input.EmployeeId.Value == 0, s => s.EmployeeId == 0)
                      .WhereIf(Input.EmployeeId.HasValue && Input.EmployeeId.Value == -1, s => s.EmployeeId > 0)
                      .WhereIf(Input.EmployeeId.HasValue && Input.EmployeeId.Value > 0, s => s.EmployeeId == Input.EmployeeId.Value);

            var userData = cc.OrderByDescending(x => x.Id).PageBy(Input).ToList();
            foreach (var item in userData)
            {
                item.ActualHours = actualhours.Where(x => x.UserStoryId == item.Id).Select(x => x.Hours).FirstOrDefault();

            }
            var userCount = cc.Count();
            return new PagedResultDto<ImportUserStoryDto>(userCount, userData.MapTo<List<ImportUserStoryDto>>());
        }

        public PagedResultDto<ImportUserStoryDto> GetImportUserStoryReport(GetImportUserstoryDto Input)
        {   
            //var actualhours = _timesheetRepository.GetAll().WhereIf(Input.EmployeeId.HasValue, s => s.UserId == Input.EmployeeId.Value)
            //                        .GroupBy(a => a.UserStoryId)
            //                        .Select(a => new { Hours = a.Sum(b => b.Hours), UserStoryId = a.Key })
            //                        .OrderByDescending(a => a.Hours)
            //                        .ToList();
            
            var frmdate = Input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : Input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = Input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : Input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            var cc = (from i in _importUserDataRepository.GetAll()
                      join p in _projectRepository.GetAll()
                      on i.ProjectId equals p.Id
                      //join u in _userRepository.GetAll()
                      //on i.EmployeeId equals u.Id into us
                      //from u in us.DefaultIfEmpty()
                      join ts in _timesheetRepository.GetAll().WhereIf(Input.EmployeeId.HasValue, s => s.UserId == Input.EmployeeId.Value)
                      on i.Id equals ts.UserStoryId into tsjoin
                      //into e1
                      //from ts in e1.DefaultIfEmpty()
                      //where i.ProjectId == Input.ProjectId
                      select new ImportUserStoryDto
                      {
                          Id = i.Id,
                          ProjectId = i.ProjectId,
                          ProjectName = p.ProjectName,
                          UserStory = i.UserStory,
                          EmployeeId = i.EmployeeId,
                          //UserId = ts.UserId,
                          //UserName = u.Name + " " + u.Surname,
                          CreationDate = i.CreationTime,
                          DeveloperHours = i.DeveloperHours.HasValue ? i.DeveloperHours.Value : 0,
                          ExpectedHours = i.ExpectedHours.HasValue ? i.ExpectedHours.Value : 0,
                          ActualHours = tsjoin.Where(x => x.UserStoryId == i.Id).Select(x=>x.Hours).DefaultIfEmpty(0).Sum(),
                          Userstorycount = tsjoin.Count(),
                          status = i.status,

                      }).Distinct()
                      .Where(x => x.Userstorycount > 0)
                      .WhereIf(Input.ProjectId.HasValue, s => s.ProjectId == Input.ProjectId.Value)
                      //.WhereIf(Input.EmployeeId.HasValue, s => s.EmployeeId == Input.EmployeeId.Value);
                      .WhereIf(Input.FromDate.HasValue, p => p.CreationDate >= dtfrm)
                      .WhereIf(Input.ToDate.HasValue, p => p.CreationDate <= dt)
                      .WhereIf(Input.status.HasValue, p => p.status == Input.status.Value);

            //var data = cc.ToList();

            //var Query = _importUserDataRepository.GetAll();
            var userData = cc.OrderBy(Input.Sorting).PageBy(Input).ToList();
            //foreach (var item in userData)
            //{
            //    item.ActualHours = actualhours.Where(x => x.UserStoryId == item.Id).Select(x => x.Hours).FirstOrDefault();

            //}
            var userCount = cc.Count();

            return new PagedResultDto<ImportUserStoryDto>(userCount, userData.MapTo<List<ImportUserStoryDto>>());
            //int UserId = (int)_session.UserId;
            //try
            //{
                
            //}
            //catch (Exception ex)
            //{

            //} return null;
        }

        public async Task ImportUserDataDetails(List<importUserStoryDetails> inputList)
        {
            //bool result = false;
            if (inputList != null)
            {
                //var deleteitem = _importUserDataRepository.GetAll();
                //foreach (var delitem in deleteitem)
                //{
                //    var id = delitem.Id;
                //    await _importUserDataRepository.DeleteAsync(id);
                //}
                foreach (var item in inputList)
                {
                    //var efExsist = _importExcelDataRepository.GetAll().Where(e => e.DomainName == item.DomainName).FirstOrDefault();
                    //if (efExsist == null)
                    //{
                    var importData = item.MapTo<ImportUserStoryDetails>();
                    try
                    {
                        await _importUserDataRepository.InsertAsync(importData);
                    }
                    catch (Exception e)
                    {


                    }

                    //}
                }
            }
        }

        public async Task UpdateAssignUserstoryToEmployee(AssignToUserstoryDto input)
        {
            var objmap = _importUserDataRepository.Get(input.Id);
            objmap.EmployeeId = input.EmployeeId;
            objmap.AssignToDate = DateTime.Now;
            await _importUserDataRepository.UpdateAsync(objmap);
        }

        public async Task UpdateUserstoryStatus(AddUserStoryDto input)
        {
            if (input.status != null)
            {
                //for (int i = 0; i < input.UpdateStatus.Length; i++)
                //{
                var data = _importUserDataRepository.Get(input.Id);
                data.status = input.status;
                _importUserDataRepository.Update(data);
                //}
            }
        }
    }
}
