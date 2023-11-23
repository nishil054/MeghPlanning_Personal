using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class UpdateRenewUserDto: EntityDto<long>
    {
        public DateTime? Next_Renewaldate { get; set; }
        public decimal Salary_Hour { get; set; }
        public decimal Salary_Month { get; set; }
        public DateTime? Resigndate { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}
