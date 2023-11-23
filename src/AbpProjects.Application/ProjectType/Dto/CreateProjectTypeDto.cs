using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectType.Dto
{
    [AutoMapTo(typeof(projecttype))]
    public  class CreateProjectTypeDto : EntityDto
    {
        [Required]
        public virtual string ProjectTypeName { get; set; }
    }
}
