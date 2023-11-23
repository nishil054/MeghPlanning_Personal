using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupportServices.Dto
{
    [AutoMapFrom(typeof(User))]
    public  class ListDto : EntityDto<long>
    {
        public virtual int EmployeeId { get; set; }
        public virtual string EmployeeName { get; set; }
    }
}
