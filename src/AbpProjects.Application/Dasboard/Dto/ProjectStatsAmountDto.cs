using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Dasboard.Dto
{
    [AutoMapFrom(typeof(project))]
    public class ProjectStatsAmountDto : EntityDto
    {
        public virtual string ProjectName { get; set; }
        public virtual string Status { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal Efforts { get; set; }

        public virtual bool AmtInPer { get; set; }
    }
}
