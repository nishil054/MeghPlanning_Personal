using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TimeSheet.Dto
{
    [AutoMapFrom(typeof(User))]
 public  class UserDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string CurrentUser { get; set; }
        public int? Immediate_supervisorId { get; set; }
    }
}
