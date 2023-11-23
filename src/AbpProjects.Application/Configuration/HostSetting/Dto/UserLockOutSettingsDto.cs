using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.HostSetting.Dto
{
   public class UserLockOutSettingsDto
    {
        public bool IsEnabled { get; set; }

        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        public int DefaultAccountLockoutSeconds { get; set; }
    }
}
