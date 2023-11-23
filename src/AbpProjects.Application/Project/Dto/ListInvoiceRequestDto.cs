using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.InvoiceRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    public class GetInvoiceInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int? Status { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string DomainName { get; set; }
        
    }
    [AutoMapFrom(typeof(invoicerequest))]
    public class ListInvoiceRequestDto : EntityDto
    {
        public virtual int ServiceId { get; set; }
        public virtual int ServiceReqId { get; set; }
        public virtual int TypeId { get; set; }
        public virtual string Comment { get; set; }
        public virtual int Status { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string CreatorName { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string MarketingPerson { get; set; }
        public virtual string ServiceName { get; set; }
        public virtual string DomainName { get; set; }
        public virtual string HostingSpace { get; set; }
        public virtual string TypeName { get; set; }
        public virtual string ServerName { get; set; }
        public virtual int? NoOfEmail { get; set; }
        public virtual string Typeofssl { get; set; }
        public virtual string Title { get; set; }
        public virtual int? Credits { get; set; }
        public virtual string ActionName { get; set; }
        public virtual string InvoiceNote { get; set; }
        public virtual string DatabaseSpace { get; set; }
        public virtual string Period { get; set; }
    }
}
