using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectType.Dto
{
public  class GetProjectTypeDto : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string ProjectTypeName { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "ProjectTypeName";
            }
        }
    }
}
