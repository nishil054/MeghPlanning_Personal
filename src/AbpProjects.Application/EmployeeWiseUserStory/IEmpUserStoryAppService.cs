using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.TimeSheet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.EmployeeWiseUserStory
{
    public interface IEmpUserStoryAppService : IApplicationService
    {
        PagedResultDto<ImportUserStoryDto> GetEmployeeUserStoryReport(GetImportUserstoryDto Input);
        PagedResultDto<TimeSheetDto> GetUserStoryDetailsList(GetTimeSheetDto input);
    }
}
