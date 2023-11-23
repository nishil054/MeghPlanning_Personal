using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Dasboard.Dto;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.MeghPlanningSupportServices.Dto;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.TimeSheet.Dto;
using AbpProjects.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Dasboard
{
    public interface IDashboardAppService: IApplicationService
    {
        Task<LogInDto> CreateLogInTime();
        Task<LogInDto> UpdateLogOutTime(EntityDto input);
        Task<LogInDto> GetLoginUserList();
        ListResultDto<LogOutMissingDto> GetMissingLogOutList();
        List<TimeSheetData> GetMissingTimesheet();
        Task UpdateTimesheetStatus(TimeSheetData input);
        ListResultDto<TimeSheetDto> GetTimeSheetData(TimeSheetData input);
        ListResultDto<LogInDto> GetUserMissingTimesheetCount();
        ListResultDto<EmployeeAssignToUserstoryDto> GetAssignUserstoryEmployeewise(GetImportUserstoryDto Input);
        ListResultDto<ProjectDto> GetSalesReport(GetProjectInput input);
        ListResultDto<MonthlySalesDto> GetMonthlySales();
        ListResultDto<ProjectStatsAmountDto> GetProjectStatsAmount();
        ListResultDto<Project.Dto.ProjectDto> GetProjectStatesHour(GetProjectInput input);
        ListResultDto<Project.Dto.ProjectDto> StatusList();
        //ListResultDto<Project.Dto.ProjectDto> GetProjectStatesHourreport(GetProjectInput input);
        ListResultDto<MonthlySalesreportDto> GetSalestarget();
        ListResultDto<morrisDto> GetSalestargetmoriss();
        PagedResultDto<Project.Dto.ProjectDto> GetProjectStatesHourreport(GetProjectInput input);
        ListResultDto<ListDataDto> GetServiceNameWithoutClient(GetServiceInput input);
        PagedResultDto<Users.Dto.UserDto> GetUserRenewdata(GetUserInputDto Input);
        Task UpdateDashboardService(int id);
        ListResultDto<ListDataDto> GetDomainDate(GetServiceInput input);
        decimal leaveBalance();

        //List<SumDto> GetCollectionSum();
        decimal? GetTotalOutstanding();
        decimal? GetTotalInvoice();
        decimal? GetCollectionSum();

        string LeaveUpteDate();
        decimal PendingLeave();

        ListResultDto<FollowupHistoryDto> GetFollowUp();

    }
}
