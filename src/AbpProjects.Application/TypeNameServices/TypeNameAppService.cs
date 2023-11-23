using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.TypenameServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization;
using AbpProjects.Authorization;

namespace AbpProjects.TypenameServices
{
    [AbpAuthorize(PermissionNames.Pages_DataVault, PermissionNames.Pages_DataVault_TypeName)]
    public class TypenameAppService : AbpProjectsApplicationModule, ITypenameAppService
    {
        private readonly IRepository<Typename> _TypenameRepository;
        public TypenameAppService(IRepository<Typename> TypenameRepository)
        {
            _TypenameRepository = TypenameRepository;
        }
        public async Task DeleteTypename(EntityDto input)
        {
            await _TypenameRepository.DeleteAsync(input.Id);
        }

        public async Task<TypenameDto> GetDataById(EntityDto input)
        {
            var c = (await _TypenameRepository.GetAsync(input.Id)).MapTo<TypenameDto>();
            return c;
        }

        public List<TypenameDto> GetTypename()
        {
            var result = (from a in _TypenameRepository.GetAll()
                          select new TypenameDto
                          {
                              Id = a.Id,
                              Name = a.Name,
                          }).ToList();
            return result;
        }

        public PagedResultDto<TypenameDto> GetTypenameList(GetTypenameDto input)
        {
            var cc = _TypenameRepository.GetAll()
               .WhereIf(!input.FilterText.IsNullOrEmpty(), p => p.Name.ToLower().Contains(input.FilterText.ToLower())
              );
            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            return new PagedResultDto<TypenameDto>(ccCount, ccData.MapTo<List<TypenameDto>>());
        }

        public async Task CreateTypename(CreateTypenameDto input)
        {
            var result = input.MapTo<Typename>();
            await _TypenameRepository.InsertAsync(result);
        }

        public bool TypenameExsistence(TypenameDto input)
        {
            return _TypenameRepository.GetAll().Where(e => e.Name == input.Name).Any();
        }

        public bool TypenameExsistenceById(TypenameDto input)
        {
            return _TypenameRepository.GetAll().Where(e => e.Name == input.Name && e.Id != input.Id).Any();
        }

        public async Task UpdateTypename(EditTypenameDto input)
        {
            var Tests = await _TypenameRepository.GetAsync(input.Id);
            Tests.Name = input.Name;
            await _TypenameRepository.UpdateAsync(Tests);
        }
    }
}
