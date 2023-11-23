using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.BackgroundJob
{
   public interface IBackgroundAppService : IApplicationService
    {
        Task ProjectNotification();
    }
}
