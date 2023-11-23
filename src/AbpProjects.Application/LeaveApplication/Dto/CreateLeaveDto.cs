using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveApplication.Dto
{
    [AutoMapTo(typeof(Leaveapplication))]
    public class CreateLeaveDto : EntityDto
    {
        public virtual int UserId { get; set; }
        public virtual int? Immediate_supervisorId { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual string Reason { get; set; }
        public virtual int LeaveType { get; set; }
        public virtual int LeaveStatus { get; set; }
        public virtual decimal LeaveBalance { get; set; }
        public virtual decimal PendingLeave { get; set; }
    }
}
