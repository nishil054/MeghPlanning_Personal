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
    [AutoMapTo(typeof(worktype))]
    public class CreateWorkTypeDto : EntityDto
    {
        [Required]
        public virtual string WorkTypeName { get; set; }
    }
}
