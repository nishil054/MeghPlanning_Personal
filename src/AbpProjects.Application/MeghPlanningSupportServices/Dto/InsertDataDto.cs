using Abp.AutoMapper;
using AbpProjects.MeghPlanningSupports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupportServices.Dto
{
    [AutoMapTo(typeof(ManageService))]
    public class InsertDataDto
    {
        public virtual int ServiceId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string DomainName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual DateTime NextRenewalDate { get; set; }
        public virtual string Comment { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual string HostingSpace { get; set; }
        public virtual int? ServerType { get; set; }
        public virtual int? TypeName { get; set; }
        public virtual int? NoOfEmail { get; set; }
        public virtual bool? Cancelflag { get; set; }
        public virtual DateTime? CancelDate { get; set; }
        public virtual DateTime? RenewalDate { get; set; }
        public virtual string DisplayTypename { get; set; }
        public virtual DateTime? RegistrationDate { get; set; }
        public virtual decimal AdjustmentAmount { get; set; }
        public virtual string solution { get; set; }
        public virtual string Typeofssl { get; set; }
        public virtual string Title { get; set; }
        public virtual int? Credits { get; set; }
        public virtual string DatabaseSpace { get; set; }
        public virtual bool IsAutoRenewal { get; set; }
        public virtual string InvoiceNote { get; set; }
        public virtual int Term { get; set; }
    }
}
