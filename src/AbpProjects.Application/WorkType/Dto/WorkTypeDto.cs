using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.WorkType.Dto
{
    [AutoMapFrom(typeof(worktype))]
    public   class WorkTypeDto : EntityDto
    {
        [Required]
        public string WorkTypeName { get; set; }
    }
}
