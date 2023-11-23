using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveApplication.Dto
{
    public class GetInputDto : PagedAndSortedResultRequestDto
    {
        public int? UserId { get; set; }
        public string Filter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? LeaveStatusId { get; set; }
    }
}
