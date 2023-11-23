using System.Collections.Generic;
using System.Configuration;
using Abp.Configuration;
using Abp.Json;
using Abp.Zero.Configuration;
using AbpProjects.Security;

namespace AbpProjects.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            context.Manager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled).DefaultValue = false.ToString().ToLowerInvariant();
            var defaultPasswordComplexitySetting = new PasswordComplexitySetting
            {
                MinLength = 6,
                MaxLength = 10,
                UseNumbers = true,
                UseUpperCaseLetters = false,
                UseLowerCaseLetters = true,
                UsePunctuations = false,
            };
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
            
                //Host settings
                new SettingDefinition(AppSettings.TenantManagement.AllowSelfRegistration,ConfigurationManager.AppSettings[AppSettings.TenantManagement.UseCaptchaOnRegistration] ?? "true"),
                new SettingDefinition(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault,ConfigurationManager.AppSettings[AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault] ??"false"),
                new SettingDefinition(AppSettings.TenantManagement.UseCaptchaOnRegistration,ConfigurationManager.AppSettings[AppSettings.TenantManagement.UseCaptchaOnRegistration] ?? "true"),
                new SettingDefinition(AppSettings.TenantManagement.DefaultEdition,ConfigurationManager.AppSettings[AppSettings.TenantManagement.DefaultEdition] ?? ""),
                new SettingDefinition(AppSettings.Security.PasswordComplexity, defaultPasswordComplexitySetting.ToJsonString(),scopes: SettingScopes.Application | SettingScopes.Tenant),

                //Tenant settings
                new SettingDefinition(AppSettings.UserManagement.AllowSelfRegistration, ConfigurationManager.AppSettings[AppSettings.UserManagement.AllowSelfRegistration] ?? "true", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault, ConfigurationManager.AppSettings[AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault] ?? "false", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettings.UserManagement.UseCaptchaOnRegistration, ConfigurationManager.AppSettings[AppSettings.UserManagement.UseCaptchaOnRegistration] ?? "true", scopes: SettingScopes.Tenant),

                new SettingDefinition(AppSettings.CDNSettingNames.CDNUrl, ConfigurationManager.AppSettings[AppSettings.CDNSettingNames.CDNUrl] ?? "true"),
                new SettingDefinition(AppSettings.CDNSettingNames.CDNFolderName, ConfigurationManager.AppSettings[AppSettings.CDNSettingNames.CDNFolderName] ?? "true"),
                new SettingDefinition(AppSettings.CDNSettingNames.CDNUserName, ConfigurationManager.AppSettings[AppSettings.CDNSettingNames.CDNUserName] ?? "true"),
                new SettingDefinition(AppSettings.CDNSettingNames.CDNKey, ConfigurationManager.AppSettings[AppSettings.CDNSettingNames.CDNKey] ?? "true"),
                new SettingDefinition(AppSettings.CDNSettingNames.CDNContainer, ConfigurationManager.AppSettings[AppSettings.CDNSettingNames.CDNContainer] ?? "true"),
                new SettingDefinition(AppSettings.CDNSettingNames.CDNFlag, ConfigurationManager.AppSettings[AppSettings.CDNSettingNames.CDNFlag] ?? "true"),
            };
        }
    }
}