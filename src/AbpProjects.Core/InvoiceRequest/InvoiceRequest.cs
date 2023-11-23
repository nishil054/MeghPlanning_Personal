using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.InvoiceRequest
{
    [Table("InvoiceRequest")]
    public class invoicerequest : FullAuditedEntity
    {
        public virtual int ServiceId { get; set; }
        public virtual int ServiceReqId { get; set; }
        public virtual int TypeId { get; set; }
        public virtual string Comment { get; set; }
        public virtual int Status{ get; set; }
        public virtual decimal Amount { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual string InvoiceNote { get; set; }
        public virtual string Period { get; set; }
    }
}
