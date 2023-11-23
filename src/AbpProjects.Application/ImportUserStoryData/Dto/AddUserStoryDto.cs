using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportUserStoryData.Dto
{
    [AutoMapTo(typeof(ImportUserStoryDetails))]
    public class AddUserStoryDto:EntityDto
    {
        public virtual string UserStory { get; set; }
        public virtual decimal DeveloperHours { get; set; }
        public virtual decimal ExpectedHours { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int status { get; set; }
    }
}
