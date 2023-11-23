using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Application.Services.Dto;
using AbpProjects.VPS.Dto;
using Abp.AutoMapper;
using Abp.Authorization;
using AbpProjects.Authorization;

namespace AbpProjects.VPS
{
    [AbpAuthorize(PermissionNames.Pages_VPS)]
    public  class VPSAppService : AbpProjectsApplicationModule, IVPSAppService
    {
        private readonly IRepository<vps> _VPSRepository;
        public VPSAppService(IRepository<vps> VPSRepository)
        {
            _VPSRepository = VPSRepository;
        }


        public async Task CreateVPS(CreateVPSDto input)
        {
            var result = input.MapTo<vps>();
            await _VPSRepository.InsertAsync(result);
        }
        public async Task DeleteVPS(EntityDto input)
        {
            await _VPSRepository.DeleteAsync(input.Id);
        }

          public List<VPSDto> GetVPS()
        {
            var result = (from a in _VPSRepository.GetAll()
                          select new VPSDto
                          {
                              Id = a.Id,
                              Title = a.Title,
                              IP=a.IP,
                              UserName=a.UserName,
                              Password=a.Password,
                              Comment=a.Comment,
                          }).ToList();
            return result;
        }

        public PagedResultDto<VPSDto> GetVPSData(GetVPSDto input)
        {
            var Query = _VPSRepository.GetAll();
            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<VPSDto>(userCount, userData.MapTo<List<VPSDto>>());
        }

        public PagedResultDto<VPSDto> GetVPSList(GetVPSDto input)
        {
            var cc = _VPSRepository.GetAll()
                .WhereIf(!input.Title.IsNullOrEmpty(), p => p.Title.ToLower().Contains(input.Title.ToLower())
               );
            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            //return await Task.FromResult(cc.ToList());
            return new PagedResultDto<VPSDto>(ccCount, ccData.MapTo<List<VPSDto>>());
        }
        public async Task UpdateVPS(EditVPSDto input)
        {
            var Tests = await _VPSRepository.GetAsync(input.Id);

            Tests.Title = input.Title;
            Tests.IP = input.IP;
            Tests.UserName = input.UserName;
            Tests.Password = input.Password;
            Tests.Comment = input.Comment;
            await _VPSRepository.UpdateAsync(Tests);
        }

        public bool VPSExsistence(VPSDto input)
        {
            return _VPSRepository.GetAll().Where(e => e.Title == input.Title).Any();
        }

        public bool VPSExsistenceById(VPSDto input)
        {
            return _VPSRepository.GetAll().Where(e => e.Title == input.Title && e.Id != input.Id).Any();
        }

        public async Task<VPSDto> GetDataById(EntityDto input)
        {
            var c = (await _VPSRepository.GetAsync(input.Id)).MapTo<VPSDto>();
            return c;
        }
    }
}
