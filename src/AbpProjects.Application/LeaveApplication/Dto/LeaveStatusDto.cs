using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.LeaveStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveApplication.Dto
{
    [AutoMapFrom(typeof(Leavestatus))]
   public class LeaveStatusDto : EntityDto
    {
        public virtual string Status { get; set; }
    }
}
