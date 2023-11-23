using Abp.Application.Services.Dto;
using AbpProjects.WorkType.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using AbpProjects.Authorization;

namespace AbpProjects.WorkType
{
    [AbpAuthorize(PermissionNames.Pages_DataVault, PermissionNames.Pages_DataVault_WorkType)]
    public class WorkTypeAppService : AbpProjectsApplicationModule, IWorkTypeAppService
    {
        private readonly IRepository<worktype> _worktypeRepository;
        public WorkTypeAppService(IRepository<worktype> worktypeRepository)
        {
            _worktypeRepository = worktypeRepository;
        }
        public async Task CreateWorkType(CreateWorkTypeDto input)
        {
            var result = input.MapTo<worktype>();
            await _worktypeRepository.InsertAsync(result);
        }

        public async Task DeleteWorkType(EntityDto input)
        {
            await _worktypeRepository.DeleteAsync(input.Id);
        }

        public async Task<WorkTypeDto> GetDataById(EntityDto input)
        {
            WorkTypeDto obj = new WorkTypeDto();
            var result = await _worktypeRepository.GetAsync(input.Id);
            obj.Id = input.Id;
            obj.WorkTypeName = result.WorkTypeName;
            return obj;
        }

      

        public List<WorkTypeDto> GetWorkType()
        {
            var result = (from a in _worktypeRepository.GetAll()
                          select new WorkTypeDto
                          {
                              Id = a.Id,
                              WorkTypeName = a.WorkTypeName,
                          }).ToList();
            return result;
        }

        public PagedResultDto<WorkTypeDto> GetWorkTypeData(GetWorkTypeDto input)
        {
            var Query = _worktypeRepository.GetAll();
            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<WorkTypeDto>(userCount, userData.MapTo<List<WorkTypeDto>>());
        }

        public PagedResultDto<WorkTypeDto> GetWorkTypeList(GetWorkTypeDto input)
        {
            var cc = _worktypeRepository.GetAll()
               .WhereIf(!input.WorkTypeName.IsNullOrEmpty(), p => p.WorkTypeName.ToLower().Contains(input.WorkTypeName.ToLower())
              );
            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            //return await Task.FromResult(cc.ToList());
            return new PagedResultDto<WorkTypeDto>(ccCount, ccData.MapTo<List<WorkTypeDto>>());
        }

        public async Task UpdateWorkType(EditWorkTypeDto input)
        {
            var Tests = await _worktypeRepository.GetAsync(input.Id);

            Tests.WorkTypeName = input.WorkTypeName;


            await _worktypeRepository.UpdateAsync(Tests);
        }

        public bool WorkTypeExsistence(WorkTypeDto input)
        {
            return _worktypeRepository.GetAll().Where(e => e.WorkTypeName == input.WorkTypeName).Any();
        }
        public bool WorkTypeExsistenceById(WorkTypeDto input)
        {
            return _worktypeRepository.GetAll().Where(e => e.WorkTypeName == input.WorkTypeName && e.Id != input.Id).Any();
        }
    }
}
