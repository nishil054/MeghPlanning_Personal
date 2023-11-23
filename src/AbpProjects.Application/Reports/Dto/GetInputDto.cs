using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
  public class GetInputDto : PagedAndSortedResultRequestDto
    {
        public virtual int UserId { get; set; }
        
        public virtual int Month { get; set; }
      
        public virtual int Year { get; set; }

        public virtual int Type { get; set; }

        public virtual int? Status { get; set; }
        public virtual int ProjectId { get; set; }
    }
}
