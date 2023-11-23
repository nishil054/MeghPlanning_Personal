using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NotificationServices.Dto
{
    public class ProjectListDto : EntityDto
    {
        public virtual string ProjectName { get; set; }
        public virtual decimal hourPercentage { get; set; }
        public virtual decimal totalhours { get; set; }
        public virtual decimal actualhours { get; set; }
    }
}
