using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.MeghPlanningSupports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupportServices.Dto
{
    [AutoMapFrom(typeof(ServiceRequestHistory))]
  public class HistoryListDto : EntityDto
    {
        public virtual int ServiceId { get; set; }
        public virtual string ServiceName { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal AdjustmentAmount { get; set; }
        public virtual string Comment { get; set; }
        public virtual int Actiontype { get; set; }
        public virtual string ActionName { get; set; }
        
        public virtual DateTime? RegistrationDate { get; set; }
        public virtual DateTime? NextRenewalDate { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
    }
}
