using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.UI;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Permissions;
using AbpProjects.Permissions.Dto;
using AbpProjects.Roles.Dto;
using AbpProjects.Users.Dto;
using Microsoft.AspNet.Identity;
using System.Linq.Dynamic.Core;


namespace AbpProjects.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly PermissionManager _permissionManager;
        public RoleAppService(
            IRepository<Role> repository,
            RoleManager roleManager,
            UserManager userManager,
            IRepository<User, long> userRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            PermissionManager permissionManager
            )
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _permissionManager = permissionManager;
        }

        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);

            CheckErrors(await _roleManager.CreateAsync(role));

            UnitOfWorkManager.Current.SaveChanges();

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        public override async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id);
            if (role.IsStatic)
            {
                throw new UserFriendlyException("CannotDeleteAStaticRole");
            }

            var users = await GetUsersInRoleAsync(role.Name);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.Name));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        private Task<List<long>> GetUsersInRoleAsync(string roleName)
        {
            var users = (from user in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                         join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                         where role.Name == roleName
                         select user.Id).Distinct().ToList();

            return Task.FromResult(users);
        }

        public Task<Abp.Application.Services.Dto.ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new Abp.Application.Services.Dto.ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions)
            ));
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions);
        }

        protected override Task<Role> GetEntityByIdAsync(int id)
        {
            var role = Repository.GetAllIncluding(x => x.Permissions).FirstOrDefault(x => x.Id == id);
            return Task.FromResult(role);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
            
        }

        //: PagedAndSortedResultRequestDto

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public PagedResultDto<RoleDto> GetAllRoles(GetRoleInputDto Input)
        {
            var u1 = _roleRepository.GetAll();
            if (!string.IsNullOrEmpty(Input.DisplayName))
            {
                u1 = _roleRepository.GetAll().Where(x => x.Name == Input.DisplayName);
            }

            var u2 = u1.OrderBy(Input.Sorting).Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();
            var userCount = u1.Count();
            return new PagedResultDto<RoleDto>(userCount, u2.MapTo<List<RoleDto>>());

        }


        #region permission related services
        public async Task<GetUserPermissionsForEditOutput> GetRolePermissionsForEdit(EntityDto<int> input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var permissions = _permissionManager.GetAllPermissions();
            var grantedPermissions = await _roleManager.GetGrantedPermissionsAsync(role);

            GetUserPermissionsForEditOutput GetUserPermissionsForEditOutput = new GetUserPermissionsForEditOutput();
            GetUserPermissionsForEditOutput.GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList();
            GetUserPermissionsForEditOutput.Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList();
            GetUserPermissionsForEditOutput.Rolename = role.DisplayName;


            return GetUserPermissionsForEditOutput;
        }


        public async Task ResetRoleSpecificPermissions(EntityDto<int> input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            await _roleManager.ResetAllPermissionsAsync(role);
        }


        public async Task UpdaterolePermissions(UpdateUserPermissionsInput input)
        {
            try
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id);
                var grantedPermissions = _permissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}