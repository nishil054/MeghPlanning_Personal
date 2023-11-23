using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Roles.Dto;
using AbpProjects.Users.Dto;

namespace AbpProjects.Roles
{
    public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedResultRequestDto, CreateRoleDto, RoleDto>
    {
        Task<ListResultDto<PermissionDto>> GetAllPermissions();

        PagedResultDto<RoleDto> GetAllRoles(GetRoleInputDto Input);

        #region permission
        Task<GetUserPermissionsForEditOutput> GetRolePermissionsForEdit(EntityDto<int> input);
        Task UpdaterolePermissions(UpdateUserPermissionsInput input);
        Task ResetRoleSpecificPermissions(EntityDto<int> input);

        #endregion
    }
}
