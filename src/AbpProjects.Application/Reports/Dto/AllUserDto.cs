using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    [AutoMapFrom(typeof(User))]
    public class AllUserDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

    }
}


