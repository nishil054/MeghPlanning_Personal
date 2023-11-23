using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    public class GetStatusDto:EntityDto
    {
        public string Status { get; set; }
    }
}
