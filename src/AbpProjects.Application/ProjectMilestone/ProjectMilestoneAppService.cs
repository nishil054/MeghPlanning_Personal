using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AbpProjects.Authorization;
using AbpProjects.ProjectMilestone.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectMilestone
{
    // [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Project)]
    public class ProjectMilestoneAppService : AbpProjectsApplicationModule, IProjectMilestoneAppService
    {
        private readonly IRepository<projectMilestone> _projectmilestoneRepository;
        public ProjectMilestoneAppService(IRepository<projectMilestone> projectmilestoneRepository)
        {
            _projectmilestoneRepository = projectmilestoneRepository;
        }

        public async Task CreateProjectMilestone(CreateProjectMilestoneDto input)
        {
            var result = input.MapTo<projectMilestone>();
            await _projectmilestoneRepository.InsertAsync(result);
        }

        public async Task DeleteProjectMilestone(EntityDto input)
        {
            await _projectmilestoneRepository.DeleteAsync(input.Id);
        }

        public List<GetProjectMilestoneDto> GetprojectMilestoneList(EntityDto input)
        {
            var result = (from a in _projectmilestoneRepository.GetAll()
                          where a.ProjectTypeId == input.Id
                          select new GetProjectMilestoneDto
                          {
                              Id = a.Id,
                              ProjectTypeId = a.ProjectTypeId,
                              Title = a.Title,
                              Amount = a.Amount,
                              Description = a.Description
                          })
                          .OrderByDescending(x => x.Id)
                          .ToList();
            return result;
        }

        public bool ProjectMilestoneExsistence(ProjectMilestoneDto input)
        {
            return _projectmilestoneRepository.GetAll().Where(e => e.Title == input.Title && e.ProjectTypeId == input.Id).Any();
        }

        public bool ProjectMilestoneExsistenceById(ProjectMilestoneDto input)
        {
            return _projectmilestoneRepository.GetAll().Where(e => e.Title == input.Title && e.Id != input.Id).Any();
        }

        public async Task UpdateProjectMilestone(EditProjectMilestoneDto input)
        {
            var Tests = await _projectmilestoneRepository.GetAsync(input.Id);

            Tests.Title = input.Title;
            Tests.Amount = input.Amount;
            Tests.Description = input.Description;


            await _projectmilestoneRepository.UpdateAsync(Tests);
        }
    }
}
