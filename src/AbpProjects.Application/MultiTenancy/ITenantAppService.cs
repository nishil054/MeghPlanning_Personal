using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.MultiTenancy.Dto;

namespace AbpProjects.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
