using Abp.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.HostSetting.Dto
{
   public class EmailSettingsDto
    {
        public string DefaultFromAddress { get; set; }

        public string DefaultFromDisplayName { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUserName { get; set; }

        [DisableAuditing]
        public string SmtpPassword { get; set; }

        public string SmtpDomain { get; set; }

        public bool SmtpEnableSsl { get; set; }

        public bool SmtpUseDefaultCredentials { get; set; }
    }
}
