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
    [AutoMapTo(typeof(projectMilestone))]
    public class CreateProjectMilestoneDto: EntityDto
    {
        [Required]
        public virtual string Title { get; set; }
        
        [Required]
        public virtual decimal Amount { get; set; }

        public virtual string Description { get; set; }
        public virtual int ProjectTypeId { get; set; }
    }
}
