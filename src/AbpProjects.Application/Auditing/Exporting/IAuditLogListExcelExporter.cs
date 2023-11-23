using System.Collections.Generic;
using AbpProjects.Auditing.Dto;
using AbpProjects.Dto;

namespace AbpProjects.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
