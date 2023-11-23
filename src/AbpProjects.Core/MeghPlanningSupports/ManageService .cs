using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupports
{
    [Table("TblManageService")]
    public class ManageService : FullAuditedEntity
    {
        public virtual int ServiceId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string DomainName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual DateTime NextRenewalDate { get; set; }
        public virtual string Comment { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual string HostingSpace { get; set; }
        public virtual string Typeofssl { get; set; }
        public virtual string Title { get; set; }
        public virtual int? ServerType { get; set; }
        public virtual int? TypeName { get; set; }
        public virtual int? NoOfEmail { get; set; }

        public virtual bool? Cancelflag { get; set; }
        public virtual DateTime? CancelDate { get; set; }
        public virtual DateTime? RenewalDate { get; set; }
        public virtual DateTime? RegistrationDate { get; set; }
        //public virtual decimal AdjustmentAmount { get; set; }
        public virtual int? Credits { get; set; }
        public virtual string DatabaseSpace { get; set; }
        //Auto Renewal Flag 
        public virtual bool IsAutoRenewal { get; set; }
        public virtual int Term { get; set; }
    }
}
