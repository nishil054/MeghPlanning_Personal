using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupports
{
    [Table("TblServiceRequest")]
    public class ServiceRequestHistory : FullAuditedEntity
    {
        public virtual int ServiceId { get; set; }
        public virtual decimal Amount { get; set; }
        
        public virtual decimal AdjustmentAmount { get; set; }
        public virtual string Comment { get; set; }
        public virtual int Actiontype { get; set; }
        public virtual string ActionName { get; set; }
        //add two fields
        public virtual DateTime? RegistrationDate { get; set; }
        public virtual DateTime? NextRenewalDate { get; set; }
        public virtual string InvoiceNote { get; set; }
        public virtual string Period { get; set; }
    }
}
