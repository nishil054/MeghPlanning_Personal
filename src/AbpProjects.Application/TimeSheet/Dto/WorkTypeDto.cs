using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.WorkType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TimeSheet.Dto
{
    [AutoMapFrom(typeof(worktype))]
    public class WorkTypeDto : EntityDto
    {
        public string WorkTypeName { get; set; }
    }
}
