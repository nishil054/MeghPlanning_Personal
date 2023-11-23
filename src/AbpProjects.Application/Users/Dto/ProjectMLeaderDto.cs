using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Users.Dto
{
    [AutoMapFrom(typeof(project))]
    public  class ProjectMLeaderDto : EntityDto
    {
        public virtual int? Marketing_LeaderId { get; set; }
    }
}
