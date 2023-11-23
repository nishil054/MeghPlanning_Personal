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
    [AutoMapFrom(typeof(ImportUserStoryDetails))]
    public class UserStoryDto: EntityDto
    {
        public virtual int ProjectId { get; set; }
        public virtual string UserStory { get; set; }
        public virtual int status { get; set; }
    }
}
