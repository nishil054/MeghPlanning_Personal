using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ProjectMilestone.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectMilestone
{
    public interface IProjectMilestoneAppService: IApplicationService
    {
        List<GetProjectMilestoneDto> GetprojectMilestoneList(EntityDto input);
        Task CreateProjectMilestone(CreateProjectMilestoneDto input);
        bool ProjectMilestoneExsistence(ProjectMilestoneDto input);
        bool ProjectMilestoneExsistenceById(ProjectMilestoneDto input);
        Task UpdateProjectMilestone(EditProjectMilestoneDto input);
        Task DeleteProjectMilestone(EntityDto input);
    }
}
