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
    [AutoMapTo(typeof(ImportUserStoryDetails))]
    public class AssignToUserstoryDto: EntityDto
    {
        public virtual int EmployeeId { get; set; }
    }
}
