using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Auditing.Dto;
using AbpProjects.AuditLogservice.Dto;
using AbpProjects.Dto;

namespace AbpProjects.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);
        

        //Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}