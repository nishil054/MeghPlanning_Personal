using System.Threading.Tasks;
using Abp.Application.Services;
using AbpProjects.Configuration.Dto;

namespace AbpProjects.Configuration
{
    public interface IConfigurationAppService: IApplicationService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}