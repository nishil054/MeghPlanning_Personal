using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Authorization.Users.Dto
{
   public class ProjectDataDto : EntityDto
    {
        public virtual string ProjectName { get; set; }
        public virtual decimal hourPercentage { get; set; }
        public virtual decimal totalhours { get; set; }
        public virtual decimal actualhours { get; set; }
    }
}
