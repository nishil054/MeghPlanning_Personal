using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration
{
    public static class AppSettings
    {
        public static class General
        {
            //no setting yet
        }

        public static class TenantManagement
        {
            public const string AllowSelfRegistration = "App.TenantManagement.AllowSelfRegistration";
            public const string IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
            public const string DefaultEdition = "App.TenantManagement.DefaultEdition";
        }

        public static class UserManagement
        {
            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
        }

        public static class Security
        {
            public const string PasswordComplexity = "App.Security.PasswordComplexity";
        }

        public static class CDNSettingNames
        {
            public const string CDNUrl = "CDN Url";
            public const string CDNFolderName = "CDN Folder Name";
            public const string CDNUserName = "CDN User Name";
            public const string CDNKey = "CDN Key";
            public const string CDNContainer = "CDN Container";
            public const string CDNFlag = "CDN Flag";

        }
    }
}
