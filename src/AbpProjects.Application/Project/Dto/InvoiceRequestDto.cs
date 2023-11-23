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
    [AutoMapTo(typeof(invoicerequest))]
    public class InvoiceRequestDto : EntityDto
    {
        public virtual int ServiceId { get; set; }
        public virtual int ServiceReqId { get; set; }
        public virtual int TypeId { get; set; }
        public virtual string Comment { get; set; }
        public virtual int Status { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual string InvoiceNote { get; set; }

    }
}
