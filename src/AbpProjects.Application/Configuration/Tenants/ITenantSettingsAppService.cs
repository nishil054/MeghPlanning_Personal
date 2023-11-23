using Abp.Application.Services;
using AbpProjects.Configuration.Tenants.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsDto> GetAllSettings();
        Task UpdateAllSettings(TenantSettingsDto input);
    }
}
