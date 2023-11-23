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
    [AutoMapFrom(typeof(UserLoginData))]
    public class LogOutMissingDto: EntityDto
    {
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime? LoggedOut { get; set; }
        public virtual DateTime? LoggedIn { get; set; }
        public virtual int TimesheetComplete { get; set; }
    }
}
