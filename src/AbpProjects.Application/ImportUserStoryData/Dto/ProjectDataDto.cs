using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportUserStoryData.Dto
{
    [AutoMapFrom(typeof(project))]
    public class ProjectDataDto : EntityDto
    {
        public virtual string ProjectName { get; set; }
        public virtual int UserId { get; set; }
        public virtual int Userstorycount { get; set; }
    }
}
