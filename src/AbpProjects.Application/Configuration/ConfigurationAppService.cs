using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using AbpProjects.Configuration.Dto;

namespace AbpProjects.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AbpProjectsAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
