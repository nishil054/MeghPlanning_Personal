using System.Threading.Tasks;
using Abp.Application.Services;
using AbpProjects.Sessions.Dto;

namespace AbpProjects.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
