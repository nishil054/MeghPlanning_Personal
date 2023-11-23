using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Json;
using Abp.Net.Mail;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using AbpProjects.Authorization;
using AbpProjects.Configuration.HostSetting.Dto;
using AbpProjects.Editions;
using AbpProjects.Security;
using AbpProjects.Timing;
using Newtonsoft.Json;
using System;
using Abp.Collections.Extensions;
using System.Globalization;
using System.Threading.Tasks;
using Abp.Runtime.Session;

namespace AbpProjects.Configuration.HostSetting
{
    [AbpAuthorize(PermissionNames.Pages_Settings)]
    public class HostSettingAppService : IHostSettingAppService
    {
        private readonly IEmailSender _emailSender;
        private readonly EditionManager _editionManager;
        private readonly ITimeZoneService _timeZoneService;
        readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _SettingManager;
        private readonly IAbpSession _AbpSession;
        public HostSettingAppService(IEmailSender emailSender,
            EditionManager editionManager,
            ITimeZoneService timeZoneService,
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager SettingManager,
            IAbpSession AbpSession
            )
        {
            _emailSender = emailSender;
            _editionManager = editionManager;
            _timeZoneService = timeZoneService;
            _settingDefinitionManager = settingDefinitionManager;
            _SettingManager = SettingManager;
            _AbpSession = AbpSession;
        }
        public async Task<HostSettingsDto> GetAllSettings()
        {
            return new HostSettingsDto
            {
                General = await GetGeneralSettingsAsync(),
                TenantManagement = await GetTenantManagementSettingsAsync(),
                UserManagement = await GetUserManagementAsync(),
                Email = await GetEmailSettingsAsync(),
                Security = await GetSecuritySettingsAsync(),
                CDNDoc = await GetAbpSettingsListAsync()
            };

        }
        private async Task<GeneralSettingsDto> GetGeneralSettingsAsync()
        {
            var timezone = await _SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            var settings = new GeneralSettingsDto
            {
                Timezone = timezone,
                TimezoneForComparison = timezone
            };

            var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, _AbpSession.TenantId);
            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        private async Task<TenantManagementSettingsDto> GetTenantManagementSettingsAsync()
        {
            
            var settings = new TenantManagementSettingsDto
            {
                AllowSelfRegistration = await _SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.AllowSelfRegistration),
                IsNewRegisteredTenantActiveByDefault = await _SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault),
                UseCaptchaOnRegistration = await _SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration)
            };

            var defaultEditionId = await _SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.DefaultEdition);
            if (!string.IsNullOrEmpty(defaultEditionId) && (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionId)) != null))
            {
                settings.DefaultEditionId = Convert.ToInt32(defaultEditionId);
            }

            return settings;
           
        }

        private async Task<HostUserManagementSettingsDto> GetUserManagementAsync()
        {
            return new HostUserManagementSettingsDto
            {
                IsEmailConfirmationRequiredForLogin = await _SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin)
            };
        }

        private async Task<EmailSettingsDto> GetEmailSettingsAsync()
        {
            var smtpPassword = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsDto
            {
                DefaultFromAddress = await _SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = await _SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await _SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                //SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpPassword = smtpPassword,
                SmtpDomain = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await _SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials = await _SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials),

            };
        }

        private async Task<SecuritySettingsDto> GetSecuritySettingsAsync()
        {
            var passwordComplexitySetting = await _SettingManager.GetSettingValueAsync(AppSettings.Security.PasswordComplexity);
            var defaultPasswordComplexitySetting = _settingDefinitionManager.GetSettingDefinition(AppSettings.Security.PasswordComplexity).DefaultValue;

            return new SecuritySettingsDto
            {
                UseDefaultPasswordComplexitySettings = passwordComplexitySetting == defaultPasswordComplexitySetting,
                PasswordComplexity = JsonConvert.DeserializeObject<PasswordComplexitySetting>(passwordComplexitySetting),
                DefaultPasswordComplexity = JsonConvert.DeserializeObject<PasswordComplexitySetting>(defaultPasswordComplexitySetting),
                UserLockOut = await GetUserLockOutSettingsAsync(),
                TwoFactorLogin = await GetTwoFactorLoginSettingsAsync()
            };
        }

        private async Task<AbpSettingsDto> GetAbpSettingsListAsync()
        {
            try
            {
                return new AbpSettingsDto
                {
                    CDNUrl = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNUrl),
                    CDNFolderName = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNFolderName),
                    CDNUserName = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNUserName),
                    CDNKey = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNKey),
                    CDNContainer = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNContainer),
                    CDNFlag = await _SettingManager.GetSettingValueAsync<bool>(AppSettings.CDNSettingNames.CDNFlag)
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<UserLockOutSettingsDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsDto
            {
                IsEnabled = await _SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout = await _SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds = await _SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds)
            };
        }

        private async Task<TwoFactorLoginSettingsDto> GetTwoFactorLoginSettingsAsync()
        {
            return new TwoFactorLoginSettingsDto
            {
                IsEnabled = await _SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                IsEmailProviderEnabled = await _SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled),
                IsSmsProviderEnabled = await _SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled),
                IsRememberBrowserEnabled = await _SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled),
            };
        }
        public async Task SendTestEmail(SendTestEmailInput input)
        {
            try
            {
                _emailSender.Send(input.EmailAddress, "TEst", "testbody", true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Ooppps! There is a problem!", ex.Message);
            };
        }

        public async Task UpdateAllSettings(HostSettingsDto input)
        {
            await UpdateGeneralSettingsAsync(input.General);
            await UpdateTenantManagementAsync(input.TenantManagement);
            await UpdateUserManagementSettingsAsync(input.UserManagement);
            await UpdateSecuritySettingsAsync(input.Security);
            await UpdateEmailSettingsAsync(input.Email);
            await UpdateAbpSettingsAsync(input.CDNDoc);
        }

        private async Task UpdateGeneralSettingsAsync(GeneralSettingsDto settings)
        {
            if (Clock.SupportsMultipleTimezone)
            {
                if (settings.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, _AbpSession.TenantId);
                    await _SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await _SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, settings.Timezone);
                }
            }
        }

        private async Task UpdateTenantManagementAsync(TenantManagementSettingsDto settings)
        {
            await _SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.AllowSelfRegistration,
                settings.AllowSelfRegistration.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );
            await _SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault,
                settings.IsNewRegisteredTenantActiveByDefault.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );

            await _SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.UseCaptchaOnRegistration,
                settings.UseCaptchaOnRegistration.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );

            await _SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.DefaultEdition,
                settings.DefaultEditionId?.ToString() ?? ""
            );
        }

        private async Task UpdateUserManagementSettingsAsync(HostUserManagementSettingsDto settings)
        {
            await _SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)
            );
        }

        private async Task UpdateSecuritySettingsAsync(SecuritySettingsDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await _SettingManager.ChangeSettingForApplicationAsync(
                    AppSettings.Security.PasswordComplexity,
                    settings.DefaultPasswordComplexity.ToJsonString()
                );
            }
            else
            {
                await _SettingManager.ChangeSettingForApplicationAsync(
                    AppSettings.Security.PasswordComplexity,
                    settings.PasswordComplexity.ToJsonString()
                );
            }

            await UpdateUserLockOutSettingsAsync(settings.UserLockOut);
            await UpdateTwoFactorLoginSettingsAsync(settings.TwoFactorLogin);
        }

        private async Task UpdateUserLockOutSettingsAsync(UserLockOutSettingsDto settings)
        {
            await _SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled, settings.IsEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
            await _SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds, settings.DefaultAccountLockoutSeconds.ToString());
            await _SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout, settings.MaxFailedAccessAttemptsBeforeLockout.ToString());
        }

        private async Task UpdateTwoFactorLoginSettingsAsync(TwoFactorLoginSettingsDto settings)
        {
            await _SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, settings.IsEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
            await _SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled, settings.IsEmailProviderEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
            await _SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled, settings.IsSmsProviderEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
            await _SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, settings.IsRememberBrowserEnabled.ToString(CultureInfo.InvariantCulture).ToLower());
        }

        private async Task UpdateEmailSettingsAsync(EmailSettingsDto settings)
        {
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, settings.DefaultFromAddress);
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, settings.DefaultFromDisplayName);
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, settings.SmtpHost);
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, settings.SmtpUserName);
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, settings.SmtpPassword);
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, settings.SmtpEnableSsl.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
            await _SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, settings.SmtpUseDefaultCredentials.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
        }

        private async Task UpdateAbpSettingsAsync(AbpSettingsDto settings)
        {
            await _SettingManager.ChangeSettingForApplicationAsync(AppSettings.CDNSettingNames.CDNUrl, settings.CDNUrl);
            await _SettingManager.ChangeSettingForApplicationAsync(AppSettings.CDNSettingNames.CDNFolderName, settings.CDNFolderName);
            await _SettingManager.ChangeSettingForApplicationAsync(AppSettings.CDNSettingNames.CDNUserName, settings.CDNUserName);
            await _SettingManager.ChangeSettingForApplicationAsync(AppSettings.CDNSettingNames.CDNKey, settings.CDNKey);
            await _SettingManager.ChangeSettingForApplicationAsync(AppSettings.CDNSettingNames.CDNContainer, settings.CDNContainer);
            await _SettingManager.ChangeSettingForApplicationAsync(AppSettings.CDNSettingNames.CDNFlag, settings.CDNFlag.ToString());
        }
    }
}
