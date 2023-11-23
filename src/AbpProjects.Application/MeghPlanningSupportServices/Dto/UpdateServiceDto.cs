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
    [AutoMapTo(typeof(ManageService))]
    public class UpdateServiceDto : EntityDto
    {
        public virtual int ServiceId { get; set; }
        public virtual int? ClientId { get; set; }
        public virtual string DomainName { get; set; }
    }
}
