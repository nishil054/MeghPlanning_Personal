using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.TimeSheet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TimeSheet
{
    public interface ITimeSheetAppService  : IApplicationService
    {
        Task CreateTimeSheet(CreateTimeSheetDto input);
        List<ProjectDto> GetProject();
        List<UserDto> GetUser();
        List<UserDto> GetUserByImmediateSupId(int id);
        List<WorkTypeDto> GetWorkType();
        Task<TimeSheetDto> GetTimeSheet(EntityDto input);
        Task<TimeSheetDto> GetDataById(EntityDto input);
        Task UpdateTimeSheet(EditTimeSheetDto input);
        Task DeleteTimeSheet(EntityDto input);
        PagedResultDto<TimeSheetDto> GetTimeSheetData(GetTimeSheetDto input); //Bind-Grid at page Loading
        PagedResultDto<TimeSheetDto> GetUserStoryDetailsList(GetTimeSheetDto input);
        PagedResultDto<TimeSheetDto> GetProjectReport(inputmaster inputs);
        ListResultDto<TimeSheetDto> Workdetail(int userId, int projectid);
        List<TimeSheetDto> GetProjectReportInExcel(inputmaster inputs);
    }
}
