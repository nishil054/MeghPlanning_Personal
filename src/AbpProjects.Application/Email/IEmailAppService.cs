using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Email
{
    public interface IEmailAppService: IApplicationService
    {
        Task sendmail();
    }
}
