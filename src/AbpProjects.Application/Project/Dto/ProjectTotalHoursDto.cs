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
   public class ProjectTotalHoursDto: EntityDto
    {
        public virtual string ProjectName { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual string totalhours { get; set; }
        public virtual string Hours { get; set; }
    }
}
