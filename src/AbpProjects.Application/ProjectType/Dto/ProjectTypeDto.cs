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
    [AutoMapFrom(typeof(projecttype))]
    public  class ProjectTypeDto : EntityDto
    {
       [Required]
        public string ProjectTypeName { get; set; }
    }
}
