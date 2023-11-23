using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    [AutoMapFrom(typeof(ProjectStatus))]
    public class StatusDto : EntityDto
    {
        public virtual string Status { get; set; }
        public virtual int sortorder { get; set; }
    }
}
