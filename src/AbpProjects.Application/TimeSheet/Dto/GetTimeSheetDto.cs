using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TimeSheet.Dto
{
   public class GetTimeSheetDto : PagedAndSortedResultRequestDto
    {
        public virtual int? ProjectId { get; set; }
        public virtual int? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public virtual int? UserStoryId { get; set; }
        public virtual int? Id { get; set; }
        public string Filter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public virtual int? WorkTypeId { get; set; }
      
    }
}
