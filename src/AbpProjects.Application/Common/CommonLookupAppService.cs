using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Net.Mail;
using AbpProjects.Common.Dto;
using AbpProjects.Configuration;
using AbpProjects.Configuration.HostSetting.Dto;
using AbpProjects.Editions;

namespace AbpProjects.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : AbpProjectsAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;

        public CommonLookupAppService(EditionManager editionManager)
        {
            _editionManager = editionManager;
        }


        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query =  UserManager.Users
                            .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                            u => u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    );

                var userCount =  query.Count();
                var users =  query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToList();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }

        public string GetDefaultEditionName()
        {
           return EditionManager.DefaultEditionName;
        }

        public async Task<ListResultDto<ComboboxItemDto>> GetEditionsForCombobox()
        {
            var editions =  _editionManager.Editions.ToList();
            return new ListResultDto<ComboboxItemDto>(
                editions.Select(e => new ComboboxItemDto(e.Id.ToString(), e.DisplayName)).ToList()
                );
        }

    }
}
