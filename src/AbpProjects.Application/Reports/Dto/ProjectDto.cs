using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    [AutoMapFrom(typeof(project))]
  public class ProjectDto
    {
        public virtual int ProjectId { get; set; }
        public virtual string ProjectName { get; set; }
    }
}
