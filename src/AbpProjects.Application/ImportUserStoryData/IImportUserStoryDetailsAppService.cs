using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ImportUserStoryData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportUserStoryData
{
    public interface IImportUserStoryDetailsAppService: IApplicationService
    {
        Task ImportUserDataDetails(List<importUserStoryDetails> inputList);
        PagedResultDto<ImportUserStoryDto> GetImportUserStoryData(GetImportUserstoryDto Input);
        PagedResultDto<ImportUserStoryDto> GetImportUserStoryReport(GetImportUserstoryDto Input);
        Task AddNewUserStory(AddUserStoryDto inputs);
        Task Delete(EntityDto inputs);
        Task UpdateUserstoryStatus(AddUserStoryDto input);
        Task UpdateAssignUserstoryToEmployee(AssignToUserstoryDto input);
        List<ImportUserStoryDto> ExportUserStory(GetImportUserstoryDto input);
    }
}
