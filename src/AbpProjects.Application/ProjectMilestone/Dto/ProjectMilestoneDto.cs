using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectMilestone.Dto
{
    [AutoMapFrom(typeof(projectMilestone))]
    public class ProjectMilestoneDto: EntityDto
    {
        [Required]
        public string Title { get; set; }
        public int ProjectTypeId { get; set; }
        public int ProjectId { get; set; }
    }
}
