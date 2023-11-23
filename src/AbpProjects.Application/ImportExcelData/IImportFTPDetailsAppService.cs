using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ImportExcelData.Dto;
using AbpProjects.ManageKnowledgeCenter.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportExcelData
{
    public interface IImportFTPDetailsAppService : IApplicationService
    {
        Task ImportExcelOfFTPDetails(List<importFTPDetails> inputList);
        PagedResultDto<ImportDto> GetImportdata(GetImportExcelDto Input);
        Task<ImportDto> getImportFTPDetail(EntityDto input);
        // List<GetTeamListDto> GetTeams();
        Task CreateFTPDetails(CreateFTPDetailsDto input);
        Task<ImportDto> GetServiceForEdit(int id);
        Task UpdateService(EditDto input);
        Task DeleteFtp(int id);
        bool CheckExist(ImportDto input);
        bool ftpExsistenceById(ImportDto input);


    }
}
