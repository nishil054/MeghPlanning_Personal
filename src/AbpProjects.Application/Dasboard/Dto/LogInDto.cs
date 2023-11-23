using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.UserLoginDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Dasboard.Dto
{
    [AutoMapTo(typeof(UserLoginData))]
    public class LogInDto: EntityDto
    {
        public const int maxLengthName = 100;
        public virtual int UserId { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual DateTime? LoggedIn { get; set; }
        public virtual DateTime? LoggedOut { get; set; }
        //public virtual bool CheckTimesheet { get; set; }
        public virtual int? TimesheetComplete { get; set; }
        public int? Immediate_supervisorId { get; set; }
        public decimal LeaveBalance { get; set; }
        public virtual string UserName { get; set; }
        public virtual int Count { get; set; }
        public virtual DateTime? createTime { get; set; }

    }
}
