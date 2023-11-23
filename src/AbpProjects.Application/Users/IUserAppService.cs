using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Company.Dto;
using AbpProjects.Roles.Dto;
using AbpProjects.Users.Dto;

namespace AbpProjects.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>
    {
        #region Permission
        List<RoleDto> GetRoles();
        List<CompanyDto> GetCompany();
        List<TeamDto> GetTeam();

        Task<bool> DeactiveUser(EntityDto input);
        Task<bool> ActiveUser(EntityDto input);

        Task DeactiveUserProjectUpdate(ProjectMLeaderDto input);
        List<UserDto> GetImmediateSupervisorById(int id);
        Task DeactiveUserServiceUpdate(ServiceAccMgrDto input);

        List<ProjectServiceDto> GetProjectServiceCount(EntityDto input);
        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);
        Task ResetUserSpecificPermissions(EntityDto<long> input);
        Task UpdateUserPermissions(UpdateUserPermissionsInput input);
        Task<string> GetUserById(EntityDto<long> input);
        PagedResultDto<UserDto> GetUserdata(GetUserInputDto Input);
        Task UpdateUserRenew(UpdateRenewUserDto input);
        Task UpdateUserResign(UpdateRenewUserDto input);
        Task UpdateUserTerminate(UpdateRenewUserDto input);
        //PagedResultDto<UserDto> GetUserRenewdata(GetUserInputDto Input);
        List<UserDto> GetImmediateSupervisor(string input);
        PagedResultDto<UserDto> GetUserList(GetUserInputDto input);
        PagedResultDto<UserDto> GetUserRenewList(GetUserInputDto input);
        PagedResultDto<UserDto> GetUserMarketingLead();
        Task UpdateChangePassword(UpdatePasswordDto inputs);
        #endregion
    }
}