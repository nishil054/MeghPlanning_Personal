using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.VPS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.VPS
{
 public  interface IVPSAppService : IApplicationService
    {
        Task CreateVPS(CreateVPSDto input);
        bool VPSExsistence(VPSDto input);
        bool VPSExsistenceById(VPSDto input);
        List<VPSDto> GetVPS();
        Task<VPSDto> GetDataById(EntityDto input);
        Task UpdateVPS(EditVPSDto input);
        Task DeleteVPS(EntityDto input);
        PagedResultDto<VPSDto> GetVPSList(GetVPSDto input);
        PagedResultDto<VPSDto> GetVPSData(GetVPSDto input);
    }
}
