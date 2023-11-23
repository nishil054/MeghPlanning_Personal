using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects
{
  public  class AbpProjectsServiceBase : Abp.AbpServiceBase
    {
        protected AbpProjectsServiceBase()
        {
            LocalizationSourceName = AbpProjectsConsts.LocalizationSourceName;
        }
    }
}
