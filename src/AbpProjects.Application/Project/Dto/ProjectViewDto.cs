using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    [AutoMapFrom(typeof(project))]
    public class ProjectViewDto : EntityDto
    {
        public virtual int BeneficiaryCompanyId { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime? TeamDeadline { get; set; }
        public virtual DateTime? ActualEndDate { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual int? Marketing_LeaderId { get; set; }
        public virtual string Marketing_LeaderName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string Status { get; set; }
        public virtual decimal totalhours { get; set; }
    }
}
