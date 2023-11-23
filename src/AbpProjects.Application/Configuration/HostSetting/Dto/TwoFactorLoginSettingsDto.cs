using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.HostSetting.Dto
{
   public class TwoFactorLoginSettingsDto
    {
        public bool IsEnabledForApplication { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsEmailProviderEnabled { get; set; }

        public bool IsSmsProviderEnabled { get; set; }

        public bool IsRememberBrowserEnabled { get; set; }
    }
}
