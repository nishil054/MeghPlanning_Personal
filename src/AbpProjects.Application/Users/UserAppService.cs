using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Permissions;
using AbpProjects.Permissions.Dto;
using AbpProjects.Roles.Dto;
using AbpProjects.Users.Dto;
using Microsoft.AspNet.Identity;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using AbpProjects.Company;
using AbpProjects.Company.Dto;
using AbpProjects.Team;
using Abp.Extensions;
using Abp.UI;
using System;
using Abp.Runtime.Session;
using AbpProjects.Project;
using AbpProjects.MeghPlanningSupports;

namespace AbpProjects.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users, PermissionNames.Pages_Project)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly PermissionManager _permissionManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<company> _companyRepository;
        private readonly IRepository<team> _teamRepository;
        private readonly IAbpSession _abpSession;
        private readonly PermissionChecker _permissionChecker;
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<ManageService> _manageserviceRepository;
        private readonly IRepository<Service> _serviceRepository;

        public UserAppService(
            PermissionChecker permissionChecker,
            IRepository<User, long> repository,
            IRepository<UserRole, long> userRoleRepository,
            UserManager userManager,
            IRepository<Role> roleRepository,
            RoleManager roleManager,
            IRepository<company> companyRepository,
            IRepository<team> teamRepository,
            IAbpSession abpSession,
            IRepository<project> projectRepository,
            IRepository<ManageService> manageserviceRepository,
            IRepository<Service> serviceRepository,

        PermissionManager permissionManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _userRepository = repository;
            _userRoleRepository = userRoleRepository;
            _companyRepository = companyRepository;
            _teamRepository = teamRepository;
            _abpSession = abpSession;
            _permissionChecker = permissionChecker;
            _projectRepository = projectRepository;
            _manageserviceRepository = manageserviceRepository;
            _serviceRepository = serviceRepository;
        }
        public PagedResultDto<UserDto> GetUserdata(GetUserInputDto Input)
        {
            var userQuery = _userRepository.GetAll().Where(x => x.UserName != "admin" && x.IsActive == true);
            var userData = userQuery.OrderBy(Input.Sorting).PageBy(Input).ToList();
            var userCount = userQuery.Count();
            return new PagedResultDto<UserDto>(userCount, userData.MapTo<List<UserDto>>());

        }

        public PagedResultDto<UserDto> GetUserList(GetUserInputDto input)
        {
            var userrolelist = _userRoleRepository.GetAll().WhereIf(input.RoleId.HasValue, p => p.RoleId == input.RoleId);



            var cc = (from u in _userRepository.GetAll()
                      join ur in userrolelist
                      on u.Id equals ur.UserId

                      join r in _roleRepository.GetAll()
                      on ur.RoleId equals r.Id
                      where u.UserName != "admin"
                      group r by u into g

                      select new UserDto

                      {
                          Id = g.Key.Id,
                          UserName = g.Key.UserName,
                          Name = g.Key.Name,
                          Surname = g.Key.Surname,
                          FullName = g.Key.Name + " " + g.Key.Surname,
                          EmailAddress = g.Key.EmailAddress,
                          IsActive = g.Key.IsActive,
                          Next_Renewaldate = g.Key.Next_Renewaldate,
                          Salary_Hour = g.Key.Salary_Hour,
                          Salary_Month = g.Key.Salary_Month,
                          TeamId = g.Key.TeamId,
                          CompanyId = g.Key.CompanyId,

                          RoleName = "",
                          // RoleName = string.Join(",", g.Select(y => y.Name).ToList()) ,
                          Role_Name = g.Select(y => y.Name).ToList()
                      })
                        .WhereIf(!input.Name.IsNullOrEmpty(), p => p.FullName.ToLower().Contains(input.Name.ToLower()))
                        .WhereIf(input.TeamId.HasValue, p => p.TeamId == input.TeamId)
                        .WhereIf(input.CompanyId.HasValue, p => p.CompanyId == input.CompanyId)
                        .WhereIf(input.ActiveStatus.HasValue, p => p.IsActive == false)
                        .WhereIf(!input.ActiveStatus.HasValue, p => p.IsActive == true);

            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();

            var ccCount = cc.Count();

            //return await Task.FromResult(cc.ToList());
            return new PagedResultDto<UserDto>(ccCount, ccData.MapTo<List<UserDto>>());
        }

        private string GetRoleName(long id)
        {
            try
            {
                string roleName = "";
                var cc = (from u in _userRepository.GetAll()
                          join ur in _userRoleRepository.GetAll()
                          on u.Id equals ur.UserId
                          join r in _roleRepository.GetAll()
                          on ur.RoleId equals r.Id
                          where u.Id == id
                          select r
                          ).ToList();
                if (cc.Count > 0)
                {
                    foreach (var item in cc)
                    {
                        if (roleName == "")
                        {
                            roleName = item.Name;
                        }
                        else
                        {
                            roleName += "," + item.Name;
                        }
                    }
                }
                return roleName;
            }
            catch (Exception ex)
            {


            }
            return null;
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

        public override async Task<UserDto> GetAsync(EntityDto<long> input)
        {
            var user = await base.GetAsync(input);
            var userRoles = await _userManager.GetRolesAsync(user.Id);
            user.Roles = userRoles.Select(ur => ur).ToArray();
            return user;
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.Password = new PasswordHasher().HashPassword(input.Password);
            user.IsEmailConfirmed = true;

            // Assign roles
            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.RoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }

            //user.Resigndate = null;
            //user.Lastdate = null;
            CheckErrors(await _userManager.CreateAsync(user));

            try
            {
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }

            return MapToEntityDto(user);
        }
        public List<UserDto> GetImmediateSupervisor(string input)
        {

            var users = (from user in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                         join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                         where role.Name == input && user.IsActive == true
                         select user).OrderBy(x => x.Name).ToList();

            return new List<UserDto>(ObjectMapper.Map<List<UserDto>>(users));

        }
        public List<TeamDto> GetTeam()
        {
            var result = (from a in _teamRepository.GetAll()
                          select new TeamDto
                          {
                              Id = a.Id,
                              TeamName = a.TeamName,
                          }).OrderBy(x => x.TeamName).ToList();
            return result;
        }

        public List<RoleDto> GetRoles()
        {
            var roles = _roleRepository.GetAllList();
            return new List<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public override async Task<UserDto> UpdateAsync(UpdateUserDto input)
        {
            CheckUpdatePermission();
            try
            {
                if (input.Id == input.Immediate_supervisorId)
                {
                    throw new UserFriendlyException("You Can't assign your own role");
                }
                var user = await _userManager.GetUserByIdAsync(input.Id);

                MapToEntity(input, user);
                //if (user.Id == user.Immediate_supervisorId)
                //{
                //    throw new UserFriendlyException("message");
                //}
                CheckErrors(await _userManager.UpdateAsync(user));

                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
                }

                return await GetAsync(input);
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }


        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            return user;
        }

        protected override void MapToEntity(UpdateUserDto input, User user)
        {
            ObjectMapper.Map(input, user);
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = Repository.GetAllIncluding(x => x.Roles).FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(user);
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        #region permission 
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            var permissions = _permissionManager.GetAllPermissions();
            var grantedPermissions = await _userManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }


        public async Task ResetUserSpecificPermissions(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.ResetAllPermissionsAsync(user);
        }


        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = _permissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await _userManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }

        public async Task<string> GetUserById(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            return user.UserName;
        }

        //public PagedResultDto<UserDto> GetUserRenewdata(GetUserInputDto Input)
        //{
        //    DateTime todaysDate = DateTime.Now;
        //    DateTime nextDate = DateTime.Now.AddDays(75);
        //    var user = _userRepository.GetAll().Where(x => x.Next_Renewaldate <= nextDate)
        //                                       .Where(x => x.IsActive == true)
        //                                       .Where(x => x.Next_Renewaldate != null)
        //                                       .OrderByDescending(x => x.Id)
        //                    .WhereIf(!Input.Name.IsNullOrEmpty(), p => p.Name.ToLower().Contains(Input.Name.ToLower()))

        //                   //.PageBy(Input)
        //                   .OrderBy(Input.Sorting)
        //                   .ToList();

        //    var count = user.Count();

        //    return new PagedResultDto<UserDto>(count, user.MapTo<List<UserDto>>());


        //}

        public async Task UpdateUserRenew(UpdateRenewUserDto input)
        {

            try
            {
                var user = await _userRepository.GetAsync(input.Id);
                user.Next_Renewaldate = input.Next_Renewaldate;
                user.Salary_Hour = input.Salary_Hour;
                user.Salary_Month = input.Salary_Month;


                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {


            }
        }

        public PagedResultDto<UserDto> GetUserRenewList(GetUserInputDto input)
        {
            DateTime todaysDate = DateTime.Now;
            DateTime nextDate = DateTime.Now.AddDays(75);
            var query = _userRepository.GetAll().Where(x => x.Next_Renewaldate <= nextDate).Where(x => x.IsActive == true)
                            .WhereIf(!input.Name.IsNullOrEmpty(), p => p.Name.ToLower().Contains(input.Name.ToLower())
                            );

            var userdata = query
                           .OrderBy(input.Sorting)
                           .PageBy(input)
                           .ToList();
            var count = userdata.Count();

            return new PagedResultDto<UserDto>(count, userdata.MapTo<List<UserDto>>());

        }

        public async Task UpdateUserResign(UpdateRenewUserDto input)
        {
            try
            {
                var user = await _userRepository.GetAsync(input.Id);
                user.Resigndate = input.Resigndate;
                user.IsActive = false;

                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {


            }
        }

        public async Task UpdateUserTerminate(UpdateRenewUserDto input)
        {
            try
            {
                var user = await _userRepository.GetAsync(input.Id);
                user.Lastdate = input.Lastdate;
                user.IsActive = false;

                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {


            }
        }

        public PagedResultDto<UserDto> GetUserMarketingLead()
        {
            var User_List = _userRepository.GetAll();
            var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
            List<User> userQuery = new List<User>();
            if (_permissionChecker.IsGranted("Pages.Project.Admin"))
            {
                userQuery = (from u in _userRepository.GetAll()
                             join
                             r in _userRoleRepository.GetAll()
                             on u.Id equals r.UserId
                             join
                             role in _roleRepository.GetAll()
                             on r.RoleId equals role.Id
                             where role.Name == "Marketing Leader" && u.IsActive == true
                             select u).OrderBy(x => x.Name).ToList();
            }
            else
            {
                if (_permissionChecker.IsGranted("Pages.Project.Marketing"))
                {
                    userQuery = (from u in _userRepository.GetAll()
                                 join
                                 r in _userRoleRepository.GetAll()
                                 on u.Id equals r.UserId
                                 join
                                 role in _roleRepository.GetAll()
                                 on r.RoleId equals role.Id
                                 where role.Name == "Marketing Leader" && u.Id == uid
                                 select u).OrderBy(x => x.Name).ToList();
                }
                else
                {
                    userQuery = (from u in _userRepository.GetAll()
                                 join
                                 r in _userRoleRepository.GetAll()
                                 on u.Id equals r.UserId
                                 join
                                 role in _roleRepository.GetAll()
                                 on r.RoleId equals role.Id
                                 where role.Name == "Marketing Leader" && u.IsActive == true
                                 select u).OrderBy(x => x.Name).ToList();
                }
            }
            
            var userData = userQuery.ToList();
            var userCount = userQuery.Count();

            return new PagedResultDto<UserDto>(userCount, userData.MapTo<List<UserDto>>());
        }

        public async Task UpdateChangePassword(UpdatePasswordDto inputs)
        {
            try
            {
                var user = await _userRepository.GetAsync(inputs.Id);
                user.Password = new PasswordHasher().HashPassword(inputs.Password);
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {


            }
        }

        public async Task<bool> DeactiveUser(EntityDto input)
        {
            bool result = false;
            var user = _userRepository.Get(input.Id);

            user.IsActive = false;
            result = true;

            await _userRepository.UpdateAsync(user);
            return result;
        }

        public async Task<bool> ActiveUser(EntityDto input)
        {
            bool result = false;
            var user = _userRepository.Get(input.Id);
            user.IsActive = true;
            result = user.IsActive;

            await _userRepository.UpdateAsync(user);
            return result;
        }

        public List<ProjectServiceDto> GetProjectServiceCount(EntityDto input)
        {
            var projectlist = _projectRepository.GetAll().Where(x => x.Marketing_LeaderId == input.Id).ToList();
            var servicelist = _manageserviceRepository.GetAll().Where(x => x.EmployeeId == input.Id).ToList();
            var PName = projectlist.Select(x => x.ProjectName).ToArray();
            var DName = servicelist.Select(x => x.DomainName).ToArray();
            var PCount = projectlist.GroupBy(x => x.Id).Count();
            var SCount = servicelist.GroupBy(x => x.Id).Count();

            List<ProjectServiceDto> obj = new List<ProjectServiceDto>();

            if (PCount > 0 || SCount > 0)
            {
                obj.Add(new ProjectServiceDto { ProjectCount = PCount, ServiceCount = SCount, ProjectName = PName, DomainName = DName });
                return obj;
            }
            else
            {
                return obj;
            }

        }

        public async Task DeactiveUserProjectUpdate(ProjectMLeaderDto input)
        {
            var project = _projectRepository.GetAll().Where(x => x.Marketing_LeaderId == input.Id).ToList();
            if (project != null)
            {
                foreach (var item in project)
                {
                    item.Marketing_LeaderId = input.Marketing_LeaderId;
                    await _projectRepository.UpdateAsync(item);
                }
            }

        }

        public async Task DeactiveUserServiceUpdate(ServiceAccMgrDto input)
        {
            var service = _manageserviceRepository.GetAll().Where(x => x.EmployeeId == input.Id).ToList();
            if (service != null)
            {
                foreach (var item in service)
                {
                    item.EmployeeId = input.EmployeeId;
                    await _manageserviceRepository.UpdateAsync(item);
                }
            }

        }

        public List<UserDto> GetImmediateSupervisorById(int id)
        {
            var users = (from user in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                         join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                         where role.Name.ToLower() == "marketing leader" && user.IsActive == true &&
                                user.Id != id
                         select new UserDto
                         {
                             Id = user.Id,
                             Name = user.Name + " " + user.Surname,
                         }).OrderBy(x => x.Name).ToList();

            return new List<UserDto>(ObjectMapper.Map<List<UserDto>>(users));
        }


        #endregion
    }
}