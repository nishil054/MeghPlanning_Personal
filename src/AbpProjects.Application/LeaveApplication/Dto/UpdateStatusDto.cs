using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveApplication.Dto
{
  public  class UpdateStatusDto : EntityDto
    {
        public int LeaveStatus { get; set; }
    }
}
