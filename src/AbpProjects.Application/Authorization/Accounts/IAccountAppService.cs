using System.Threading.Tasks;
using Abp.Application.Services;
using AbpProjects.Authorization.Accounts.Dto;

namespace AbpProjects.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
