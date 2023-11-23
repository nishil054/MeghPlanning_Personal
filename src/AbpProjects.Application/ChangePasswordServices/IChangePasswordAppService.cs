using Abp.Application.Services;
using AbpProjects.Authorization.Accounts.Dto;
using AbpProjects.ChangePasswordServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ChangePasswordServices
{
    public interface IChangePasswordAppService : IApplicationService
    {
        Task<string> ChangePassword(ChangePasswordDto inputs);
    }
}
