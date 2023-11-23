using Abp.Application.Services;
using AbpProjects.Configuration.HostSetting.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.HostSetting
{
  public  interface IHostSettingAppService : IApplicationService
    {
        Task<HostSettingsDto> GetAllSettings();
        Task UpdateAllSettings(HostSettingsDto input);
        Task SendTestEmail(SendTestEmailInput input);
    }
}
