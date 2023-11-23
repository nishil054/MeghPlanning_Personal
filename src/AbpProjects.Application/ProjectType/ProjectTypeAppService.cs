using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AbpProjects.ProjectType.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Extensions;
using Abp.Authorization;
using AbpProjects.Authorization;

namespace AbpProjects.ProjectType
{
    [AbpAuthorize(PermissionNames.Pages_DataVault, PermissionNames.Pages_DataVault_ProjectType)]
    public class ProjectTypeAppService : AbpProjectsApplicationModule, IProjectTypeAppService
    {
        private readonly IRepository<projecttype> _projecttypeRepository;
        public ProjectTypeAppService(IRepository<projecttype> projecttypeRepository)
        {
            _projecttypeRepository = projecttypeRepository;
        }
        public async Task CreateProjectType(CreateProjectTypeDto input)
        {
            var result = input.MapTo<projecttype>();
            await _projecttypeRepository.InsertAsync(result);
        }

        public List<ProjectTypeDto> GetProjectType()
        {
            var result = (from a in _projecttypeRepository.GetAll()
                          select new ProjectTypeDto
                          {
                              Id = a.Id,
                              ProjectTypeName = a.ProjectTypeName,
                          }).ToList();
            return result;
        }

        public PagedResultDto<ProjectTypeDto> GetProjectTypeData(GetProjectTypeDto input)
        {
            var Query = _projecttypeRepository.GetAll();
            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<ProjectTypeDto>(userCount, userData.MapTo<List<ProjectTypeDto>>());
        }

        public async Task<ProjectTypeDto> GetDataById(EntityDto input)
        {
            var c = (await _projecttypeRepository.GetAsync(input.Id)).MapTo<ProjectTypeDto>();
            return c;
        }

        public bool ProjectTypeExsistence(ProjectTypeDto input)
        {
            return _projecttypeRepository.GetAll().Where(e => e.ProjectTypeName == input.ProjectTypeName).Any();
        }
        public bool ProjectTypeExsistenceById(ProjectTypeDto input)
        {
            return _projecttypeRepository.GetAll().Where(e => e.ProjectTypeName == input.ProjectTypeName && e.Id != input.Id).Any();
        }

        public async Task UpdateProjectType(EditProjectTypeDto input)
        {
            var Tests = await _projecttypeRepository.GetAsync(input.Id);

            Tests.ProjectTypeName = input.ProjectTypeName;


            await _projecttypeRepository.UpdateAsync(Tests);
        }
        public async Task DeleteProjectType(EntityDto input)
        {
            await _projecttypeRepository.DeleteAsync(input.Id);
        }
        public PagedResultDto<ProjectTypeDto> GetProjectTypeList(GetProjectTypeDto input)
        {
            var cc = _projecttypeRepository.GetAll()
                .WhereIf(!input.ProjectTypeName.IsNullOrEmpty(), p => p.ProjectTypeName.ToLower().Contains(input.ProjectTypeName.ToLower())
               );
            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            //return await Task.FromResult(cc.ToList());
            return new PagedResultDto<ProjectTypeDto>(ccCount, ccData.MapTo<List<ProjectTypeDto>>());
        }

        
    }
}
