using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Json;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Zero.Configuration;
using AbpProjects.Configuration.HostSetting.Dto;
using AbpProjects.Configuration.Tenants.Dto;
using AbpProjects.Security;
using AbpProjects.Timing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Configuration.Tenants
{
    [AbpAuthorize]
    public class TenantSettingsAppService : AbpProjectsAppServiceBase, ITenantSettingsAppService
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        //private readonly IAbpZeroLdapModuleConfig _ldapModuleConfig;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ISettingManager _SettingManager;
        

        public TenantSettingsAppService(
           IMultiTenancyConfig multiTenancyConfig,
           //IAbpZeroLdapModuleConfig ldapModuleConfig,
           ITimeZoneService timeZoneService,
           ISettingManager SettingManager
            )
        {
            _timeZoneService = timeZoneService;
            _multiTenancyConfig = multiTenancyConfig;
            //_ldapModuleConfig = ldapModuleConfig;
            _SettingManager = SettingManager;
            
        }
        public async Task<TenantSettingsDto> GetAllSettings()
        {
            var settings = new TenantSettingsDto
            {
                UserManagement = await GetUserManagementSettingsAsync(),
                Security = await GetSecuritySettingsAsync()
            };

            if (!_multiTenancyConfig.IsEnabled || Clock.SupportsMultipleTimezone)
            {
                settings.General = await GetGeneralSettingsAsync();
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                settings.Email = await GetEmailSettingsAsync();

                //if (_ldapModuleConfig.IsEnabled)
                //{
                //    settings.Ldap = await GetLdapSettingsAsync();
                //}
                //else
                //{
                //    settings.Ldap = new LdapSettingsDto { IsModuleEnabled = false };
                //}
            }

            return settings;
        }
        private async Task<LdapSettingsDto> GetLdapSettingsAsync()
        {
            return new LdapSettingsDto
            {
                IsModuleEnabled = true,
                //IsEnabled = await _SettingManager.GetSettingValueAsync<bool>(LdapSettingNames.IsEnabled),
                //Domain = await _SettingManager.GetSettingValueAsync(LdapSettingNames.Domain),
                //UserName = await _SettingManager.GetSettingValueAsync(LdapSettingNames.UserName),
                //Password = await _SettingManager.GetSettingValueAsync(LdapSettingNames.Password),
            };
        }
        private async Task<EmailSettingsDto> GetEmailSettingsAsync()
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsDto
            {
                DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                //SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpPassword= smtpPassword,
                SmtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }

        private async Task<GeneralSettingsDto> GetGeneralSettingsAsync()
        {
            var settings = new GeneralSettingsDto();

            if (Clock.SupportsMultipleTimezone)
            {
                var timezone = await SettingManager.GetSettingValueForTenantAsync(TimingSettingNames.TimeZone, AbpSession.GetTenantId());

                settings.Timezone = timezone;
                settings.TimezoneForComparison = timezone;
            }

            var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Tenant, AbpSession.TenantId);

            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        private async Task<TenantUserManagementSettingsDto> GetUserManagementSettingsAsync()
        {
            return new TenantUserManagementSettingsDto
            {
                AllowSelfRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.AllowSelfRegistration),
                IsNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault),
                IsEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin),
                UseCaptchaOnRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration)
            };
        }

        private async Task<SecuritySettingsDto> GetSecuritySettingsAsync()
        {
            var passwordComplexitySetting = await SettingManager.GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
            var defaultPasswordComplexitySetting = await SettingManager.GetSettingValueForApplicationAsync(AppSettings.Security.PasswordComplexity);

            var settings = new SecuritySettingsDto
            {
                UseDefaultPasswordComplexitySettings = passwordComplexitySetting == defaultPasswordComplexitySetting,
                PasswordComplexity = JsonConvert.DeserializeObject<PasswordComplexitySetting>(passwordComplexitySetting),
                DefaultPasswordComplexity = JsonConvert.DeserializeObject<PasswordComplexitySetting>(defaultPasswordComplexitySetting),
                UserLockOut = await GetUserLockOutSettingsAsync()
            };

            settings.TwoFactorLogin = await GetTwoFactorLoginSettingsAsync();

            return settings;
        }

        private async Task<UserLockOutSettingsDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds)
            };
        }

        private Task<bool> IsTwoFactorLoginEnabledForApplicationAsync()
        {
            return SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled);
        }

        private async Task<TwoFactorLoginSettingsDto> GetTwoFactorLoginSettingsAsync()
        {
            var settings = new TwoFactorLoginSettingsDto
            {
                IsEnabledForApplication = await IsTwoFactorLoginEnabledForApplicationAsync()
            };

            if (_multiTenancyConfig.IsEnabled && !settings.IsEnabledForApplication)
            {
                return settings;
            }

            settings.IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled);
            settings.IsRememberBrowserEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled);

            if (!_multiTenancyConfig.IsEnabled)
            {
                settings.IsEmailProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled);
                settings.IsSmsProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled);
            }

            return settings;
        }
        public async Task UpdateAllSettings(TenantSettingsDto input)
        {
            await UpdateUserManagementSettingsAsync(input.UserManagement);
            await UpdateSecuritySettingsAsync(input.Security);

            //Time Zone
            if (Clock.SupportsMultipleTimezone)
            {
                if (input.General.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Tenant, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), TimingSettingNames.TimeZone, input.General.Timezone);
                }
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                input.ValidateHostSettings();

                await UpdateEmailSettingsAsync(input.Email);

                //if (_ldapModuleConfig.IsEnabled)
                //{
                //    await UpdateLdapSettingsAsync(input.Ldap);
                //}
            }
        }
        private async Task UpdateLdapSettingsAsync(LdapSettingsDto input)
        {
            //await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.IsEnabled, input.IsEnabled.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
            //await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.Domain, input.Domain.IsNullOrWhiteSpace() ? null : input.Domain);
            //await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.UserName, input.UserName.IsNullOrWhiteSpace() ? null : input.UserName);
            //await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), LdapSettingNames.Password, input.Password.IsNullOrWhiteSpace() ? null : input.Password);
        }

        private async Task UpdateEmailSettingsAsync(EmailSettingsDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, input.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, input.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, input.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, input.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, input.SmtpPassword);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, input.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, input.SmtpEnableSsl.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, input.SmtpUseDefaultCredentials.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
        }

        private async Task UpdateUserManagementSettingsAsync(TenantUserManagementSettingsDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.UserManagement.AllowSelfRegistration,
                settings.AllowSelfRegistration.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault,
                settings.IsNewRegisteredUserActiveByDefault.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );

            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );
            await SettingManager.ChangeSettingForTenantAsync(
                AbpSession.GetTenantId(),
                AppSettings.UserManagement.UseCaptchaOnRegistration,
                settings.UseCaptchaOnRegistration.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );
        }

        private async Task UpdateSecuritySettingsAsync(SecuritySettingsDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await SettingManager.ChangeSettingForTenantAsync(
                    AbpSession.GetTenantId(),
                    AppSettings.Security.PasswordComplexity,
                    settings.DefaultPasswordComplexity.ToJsonString()
                );
            }
            else
            {
                await SettingManager.ChangeSettingForTenantAsync(
                    AbpSession.GetTenantId(),
                    AppSettings.Security.PasswordComplexity,
                    settings.PasswordComplexity.ToJsonString()
                );
            }

            await UpdateUserLockOutSettingsAsync(settings.UserLockOut);
            await UpdateTwoFactorLoginSettingsAsync(settings.TwoFactorLogin);
        }

        private async Task UpdateUserLockOutSettingsAsync(UserLockOutSettingsDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled, settings.IsEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds, settings.DefaultAccountLockoutSeconds.ToString());
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout, settings.MaxFailedAccessAttemptsBeforeLockout.ToString());
        }

        private async Task UpdateTwoFactorLoginSettingsAsync(TwoFactorLoginSettingsDto settings)
        {
            if (_multiTenancyConfig.IsEnabled &&
                !await IsTwoFactorLoginEnabledForApplicationAsync()) //Two factor login can not be used by tenants if disabled by the host
            {
                return;
            }

            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, settings.IsEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, settings.IsRememberBrowserEnabled.ToString(CultureInfo.InvariantCulture).ToLower());

            if (!_multiTenancyConfig.IsEnabled)
            {
                //These settings can only be changed by host, in a multitenant application.
                await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled, settings.IsEmailProviderEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
                await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled, settings.IsSmsProviderEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
            }
        }
    }
}
