using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.WorkType.Dto
{
  public  class GetWorkTypeDto : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string WorkTypeName { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "WorkTypeName";
            }
        }
    }
}
