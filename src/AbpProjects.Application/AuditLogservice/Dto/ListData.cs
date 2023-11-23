using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.AuditLogservice.Dto
{
    public class GetAuditlogInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string UserName { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string BrowserInfo { get; set; }

        public int? ExecutionDuration { get; set; }
        //public int? EmployeeId { get; set; }

    }
    [AutoMapFrom(typeof(AuditLog))]
    public class ListData : EntityDto
    {
        //public virtual int Id { get; set; }
        public virtual int TenantId { get; set; }
        public virtual int UserId { get; set; }
        public virtual string ServiceName { get; set; }
        public virtual string MethodName { get; set; }
        public virtual string Parameters { get; set; }
        public virtual DateTime ExecutionTime { get; set; }
        public virtual int ExecutionDuration  { get; set; }
        public virtual string ClientIpAddress { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string BrowserInfo { get; set; }
        public virtual string Exception { get; set; }
        public virtual int ImpersonatorUserId { get; set; }
        public virtual int ImpersonatorTenantId { get; set; }
        public virtual string CustomData { get; set; }
        public virtual string ReturnValue { get; set; }
        public virtual string ExceptionMessage { get; set; }

        public virtual string UserName { get; set; }
    }
}
