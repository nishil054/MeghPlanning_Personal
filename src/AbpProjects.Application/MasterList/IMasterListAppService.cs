using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Authorization.Users;
using AbpProjects.Category.Dto;
using AbpProjects.Company.Dto;
using AbpProjects.FinancialYear.Dto;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.MeghPlanningSupportServices.Dto;
using AbpProjects.NotificationServices.Dto;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.ProjectType.Dto;
using AbpProjects.Roles.Dto;
using AbpProjects.TimeSheet.Dto;
using AbpProjects.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MasterList
{
    public interface IMasterListAppService : IApplicationService
    {
        List<CompanyDto> GetCompany();
        List<FinancialYearListDto> GetFinancialYear();
        List<ProjectDto> GetProject();
        List<Users.Dto.UserDto> GetEmployee();

        List<GetNotificationDetailsDto> GetUser();
        List<ProjectType.Dto.ProjectTypeDto> GetProjectType();
        List<WorkTypeDto> GetWorkType();
        ListResultDto<ListDataDto> GetTypeName(GetServiceInput input);
        List<CategoryDto> GetCategories();
        List<CategoryDto> GetCategoriesByTeam(int TeamId);
        List<RoleDto> GetRoles();
        //List<UserStoryDto> GetUserStory();
        Task<ListResultDto<UserStoryDto>> GetUserStory(UserStoryDto projectId);
        List<ProjectDto> GetTimesheetwise_ProjectList(AbpProjects.Reports.Dto.GetInputDto input);
        List<ProjectDataDto> GetProjectsByCurUser();
        List<FollowUpTypeDto> GetFollowUpType();
        string GetFinYear();
        string GetRoleName();
    }
}
