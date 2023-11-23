using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageLeaves.Dto
{
    //[AutoMapTo(typeof(User))]
    public class EmployeeLeaveDto : EntityDto
    {
        public decimal LeaveBalance { get; set; }
        public virtual decimal PendingLeaves { get; set; }
    }
}
