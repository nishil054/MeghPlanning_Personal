using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.AuditLogservice.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.AuditLogservice
{
    public interface IAuditLogsAppService : IApplicationService
    {
        PagedResultDto<ListData> GetAuditLogs(GetAuditlogInput input);
    }
}
