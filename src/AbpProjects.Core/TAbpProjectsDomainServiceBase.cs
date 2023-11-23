using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects
{
    public class AbpProjectsDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected AbpProjectsDomainServiceBase()
        {
            LocalizationSourceName = AbpProjectsConsts.LocalizationSourceName;
        }
    }
}
