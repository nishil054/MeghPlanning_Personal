using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Category;
using AbpProjects.Category.Dto;
using AbpProjects.Company;
using AbpProjects.Company.Dto;
using AbpProjects.FinancialYear;
using AbpProjects.FinancialYear.Dto;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.MeghPlanningSupportServices.Dto;
using AbpProjects.Project;
using AbpProjects.ProjectType;
using AbpProjects.ProjectType.Dto;
using AbpProjects.Roles.Dto;
using AbpProjects.TimeSheet;
using AbpProjects.TimeSheet.Dto;
using AbpProjects.Users.Dto;
using AbpProjects.WorkType;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Authorization.Users;
using AbpProjects.NotificationServices.Dto;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.Opportunities;

namespace AbpProjects.MasterList
{
    [AbpAuthorize]
    public class MasterListAppService : IMasterListAppService
    {
        private readonly IRepository<company> _companyRepository;
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<User,long> _userRepository;
        private readonly IRepository<projecttype> _projecttypeRepository;
        private readonly IRepository<worktype> _worktypeRepository;
        private readonly IRepository<Typename> _typenameRepository;
        private readonly IRepository<category> _categoryRepository;
        private readonly IRepository<tbl_category_team> _categoryteamRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<ImportUserStoryDetails> _importUserStoryRepository;
        private readonly IRepository<financialYear> _financialRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IAbpSession _session;
        private readonly IRepository<Followuptype> _FollowuptypeRepository;
        public MasterListAppService(IRepository<company> companyRepository,
            IRepository<project> projectRepository,
            IRepository<User,long> userRepository,
            IRepository<projecttype> projecttypeRepository,
            IRepository<worktype> worktypeRepository,
            IRepository<Typename> typenameRepository,
            IRepository<category> categoryRepository,
            IRepository<tbl_category_team> categoryteamRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<ImportUserStoryDetails> importUserStoryRepository,
            IRepository<financialYear> financialRepository,
            IRepository<timesheet> timesheetRepository,
             IAbpSession session, IRepository<Followuptype> FollowuptypeRepository)
        {
            _companyRepository = companyRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _projecttypeRepository = projecttypeRepository;
            _worktypeRepository = worktypeRepository;
            _typenameRepository = typenameRepository;
            _categoryRepository = categoryRepository;
            _categoryteamRepository = categoryteamRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _importUserStoryRepository = importUserStoryRepository;
            _financialRepository = financialRepository;
            _timesheetRepository = timesheetRepository;
            _session = session;
            _FollowuptypeRepository = FollowuptypeRepository;
        }
        public List<CompanyDto> GetCompany()
        {
            var result = (from a in _companyRepository.GetAll()
                          select new CompanyDto
                          {
                              Id = a.Id,
                              Beneficial_Company_Name = a.Beneficial_Company_Name,
                          }).OrderBy(x => x.Beneficial_Company_Name).ToList();
            return result;
        }

        public List<ProjectType.Dto.ProjectTypeDto> GetProjectType()
        {
            var result = (from a in _projecttypeRepository.GetAll()
                          select new ProjectType.Dto.ProjectTypeDto
                          {
                              Id = a.Id,
                              ProjectTypeName = a.ProjectTypeName,
                          }).OrderBy(x => x.ProjectTypeName).ToList();
            return result;
        }

        public List<WorkTypeDto> GetWorkType()
        {
            var result = (from a in _worktypeRepository.GetAll()
                          select new WorkTypeDto
                          {
                              Id = a.Id,
                              WorkTypeName = a.WorkTypeName,
                          }).OrderBy(x => x.WorkTypeName).ToList();
            return result;
        }

        public ListResultDto<ListDataDto> GetTypeName(GetServiceInput input)
        {
            var persons = (from item in _typenameRepository
                 .GetAll()
                           select new ListDataDto
                           {

                               TypeName = item.Id,
                               DisplayTypename = item.Name
                           })
                           .OrderBy(x => x.DisplayTypename).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }

        public List<CategoryDto> GetCategories()
        {
            var Datalist = (from a in _categoryRepository.GetAll()
                            orderby a.Category ascending
                            select new CategoryDto
                            {
                                Id = a.Id,
                                Category = a.Category,
                            }).OrderBy(x => x.Category).ToList();

            return Datalist;

        }

        public List<RoleDto> GetRoles()
        {
            var result = (from a in _roleRepository.GetAll()
                          select new RoleDto
                          {
                              Id = a.Id,
                              Name = a.Name,
                              DisplayName = a.DisplayName,
                          }).OrderBy(x => x.Name).ToList();
            return result;

        }

        //public List<UserStoryDto> GetUserStory()
        //{
        //    var userstory = (from a in _importUserStoryRepository.GetAll()
        //                  select new UserStoryDto
        //                  {
        //                      Id = a.Id,
        //                      UserStory = a.UserStory,
        //                  }).ToList();
        //    return userstory;
        //}

        public List<ProjectDto> GetProject()
        {
            var projectlist = (from a in _projectRepository.GetAll()
                               select new ProjectDto
                               {
                                   Id = a.Id,
                                   ProjectName = a.ProjectName,
                               }).OrderBy(x => x.ProjectName).ToList();
            return projectlist;
        }

        public async Task<ListResultDto<UserStoryDto>> GetUserStory(UserStoryDto projectId)
        {
            var userstory = _importUserStoryRepository.GetAll().Where(x => x.ProjectId == projectId.ProjectId && x.status==0).OrderBy(p => p.UserStory).ToList();
            return new ListResultDto<UserStoryDto>(userstory.MapTo<List<UserStoryDto>>());
        }

        public List<FinancialYearListDto> GetFinancialYear()
        {
            var result = (from a in _financialRepository.GetAll()
                          select new FinancialYearListDto
                          {
                              Id = a.Id,
                              Title = a.Title,
                              StartYear = a.StartYear,
                              EndYear = a.EndYear
                          }).ToList();
            return result;
        }

        public List<ProjectDto> GetTimesheetwise_ProjectList(AbpProjects.Reports.Dto.GetInputDto input)
        {
            var projectlist = (from a in _projectRepository.GetAll()
                               join ts in _timesheetRepository.GetAll().Where(x=>x.Date.Month== input.Month && x.Date.Year== input.Year) on a.Id equals ts.ProjectId into groupts
                               select new ProjectDto
                               {
                                   Id = a.Id,
                                   ProjectName = a.ProjectName,
                                   TimeSheetCount =groupts.Count(),
                               }).Where(a=>a.TimeSheetCount>0).OrderBy(x=>x.ProjectName).ToList();
            return projectlist;
        }

        public List<ProjectDataDto> GetProjectsByCurUser()
        {
            int curId = (int)_session.UserId;

            var userwiselist = (from a in _importUserStoryRepository.GetAll()
                                join b in _timesheetRepository.GetAll()
                                on a.Id equals b.UserStoryId
                                join c in _projectRepository.GetAll()
                                on a.ProjectId equals c.Id
                                where b.UserId==curId
                                group c by new {c.Id,c.ProjectName} into g
                                orderby g.Key.ProjectName
                                select new ProjectDataDto
                                {
                                    Id = g.Key.Id,
                                    ProjectName = g.Key.ProjectName
                                }).ToList();
          
            return userwiselist;
        }

        public List<CategoryDto> GetCategoriesByTeam(int TeamId)
        {
            var Datalist = (from a in _categoryteamRepository.GetAll()
                           join b in _categoryRepository.GetAll()
                           on a.CategoryId equals b.Id
                           where a.TeamId==TeamId
                            select new CategoryDto
                            {
                                Id = b.Id,
                                Category = b.Category,
                            }).OrderBy(x => x.Category).ToList();

            return Datalist;
        }
        public List<Users.Dto.UserDto> GetEmployee()
        {
            var emplist = (from a in _userRepository.GetAll()
                           join ur in _userRoleRepository.GetAll()
                           on a.Id equals ur.UserId
                           join r in _roleRepository.GetAll()
                           on ur.RoleId equals r.Id
                           where a.IsActive == true && a.IsDeleted == false && r.Name == "Employee"
                           select new Users.Dto.UserDto
                               {
                                   Id = a.Id,
                                   Name = a.Name + " " + a.Surname,
                               }).OrderBy(x => x.Name).ToList();
            return emplist;
        }

        public List<GetNotificationDetailsDto> GetUser()
        {
            var userlist = (from a in _userRepository.GetAll()
                           where a.IsActive == true && a.UserName.ToLower() != "admin"
                           select new GetNotificationDetailsDto
                           {
                               UId = a.Id,
                               Name = a.Name + " " + a.Surname,
                           }).OrderBy(x => x.Name).ToList();

            return userlist;
        }

        public string GetFinYear()
        {
            string finyear = ""; 
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string FinancialYear = dt.ToString();
            
            int m = dt.Month;
            int y = dt.Year;
            if (m > 3)
            {
                finyear = y.ToString() + "-" + Convert.ToString((y + 1));
            }
            else
            {
                finyear = Convert.ToString((y - 1)) + "-" + y.ToString();
            }
            return finyear;
        }

        public static string GetFinancialYear(string date)
        {
            string finyear = "";
            DateTime dt = Convert.ToDateTime(date);
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
        public List<FollowUpTypeDto> GetFollowUpType()
        {
            var result = (from a in _FollowuptypeRepository.GetAll()
                          select new FollowUpTypeDto
                          {
                              Id = a.Id,
                              FollowUpType = a.FollowUpType,
                          }).OrderBy(x => x.FollowUpType).ToList();
            return result;
        }
        public string GetRoleName()
        {
            var userid = (int)_session.UserId;

            var roleid = _userRoleRepository.GetAll().Where(x => x.UserId == userid).Select(x => x.RoleId).FirstOrDefault();
            var rolename = _roleRepository.GetAll().Where(x => x.Id == roleid).Select(x => x.Name).FirstOrDefault();
            return rolename;
        }
    }
}
