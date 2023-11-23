using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.Tenants.Dto
{
   public class TenantUserManagementSettingsDto
    {
        public bool AllowSelfRegistration { get; set; }

        public bool IsNewRegisteredUserActiveByDefault { get; set; }

        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        public bool UseCaptchaOnRegistration { get; set; }
    }
}
