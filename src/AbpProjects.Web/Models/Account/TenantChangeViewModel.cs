using Abp.AutoMapper;
using AbpProjects.Sessions.Dto;

namespace AbpProjects.Web.Models.Account
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}