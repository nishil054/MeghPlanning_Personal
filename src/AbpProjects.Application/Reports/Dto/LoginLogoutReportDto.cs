using Abp.AutoMapper;
using AbpProjects.UserLoginDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    [AutoMapFrom(typeof(UserLoginData))]
    public class LoginLogoutReportDto
    {
        public virtual int userId { get; set; }
        public virtual string EmployeeName { get; set; }
        public virtual int LoginCount { get; set; }
    }

    [AutoMapFrom(typeof(UserLoginData))]
    public class EmployeeInOutReportDetailsDto
    {
        public virtual int userId { get; set; }
        public virtual string EmployeeName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime LogInTime { get; set; }
        public virtual DateTime? LogOutTime { get; set; }
    }
}
