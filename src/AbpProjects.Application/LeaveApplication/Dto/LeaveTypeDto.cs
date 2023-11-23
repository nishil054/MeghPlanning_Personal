using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.LeaveType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveApplication.Dto
{
    [AutoMapFrom(typeof(Leavetype))]
    public class LeaveTypeDto : EntityDto
    {
        public virtual string Type { get; set; }
    }
}
