using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Users.Dto
{
  public  class ProjectServiceDto 
    {
        public virtual int? ProjectCount { get; set; }
        public virtual string[] ProjectName { get; set; }
        public virtual int? ServiceCount { get; set; }
        public virtual string[] DomainName { get; set; }
    }
}
