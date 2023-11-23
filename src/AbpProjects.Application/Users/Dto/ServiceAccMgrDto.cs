using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.MeghPlanningSupports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Users.Dto
{
    [AutoMapFrom(typeof(ManageService))]
    public class ServiceAccMgrDto : EntityDto
    {
        public virtual int EmployeeId { get; set; }
    }
}
