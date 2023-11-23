using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    [AutoMapFrom(typeof(Projecttype_details))]
    public class ProjectDetailsDto : EntityDto
    {
        public virtual int ProjecttypeId { get; set; }
        public virtual string ProjectType { get; set; }
        public virtual decimal Typeprice { get; set; }
        public virtual string hours { get; set; }
        public virtual decimal? CostforCompany { get; set; }
        public virtual string Comments { get; set; }
        public int MilestoneCount { get; set; }
    }
}
