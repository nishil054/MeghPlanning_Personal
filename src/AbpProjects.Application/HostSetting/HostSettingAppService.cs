using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.UI;
using AbpProjects.Authorization;
using AbpProjects.Configuration;
using AbpProjects.HostSetting.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.HostSetting
{
    [AbpAuthorize(PermissionNames.Pages_Settings)]
    public class HostSettingAppService : IHostSettingAppService
    {
        private readonly IEmailSender _emailSender;
        private readonly ISettingManager _SettingManager;
        private readonly IRepository<Setting, long> _abpSettingsRepository;
        public HostSettingAppService(IEmailSender emailSender, ISettingManager SettingManager, IRepository<Setting, long> abpSettingsRepository)
        {
            _emailSender = emailSender;
            _SettingManager = SettingManager;
            _abpSettingsRepository = abpSettingsRepository;
        }
        public async Task<HostSettingsDto> GetAllSettings()
        {
            try
            {
                return new HostSettingsDto
                {
                    Email = await GetEmailSettingsAsync()
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<EmailSettingsDto> GetEmailSettingsAsync()
        {
            try
            {
                var smtpPassword = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
                //smtpPassword = smtpPassword.IsNullOrEmpty() ? null : SimpleStringCipher.Instance.Decrypt(smtpPassword).ToString();

                return new EmailSettingsDto
                {
                    DefaultFromAddress = await _SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                    DefaultFromDisplayName = await _SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                    SmtpHost = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                    SmtpPort = await _SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                    SmtpUserName = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                    SmtpPassword = smtpPassword,
                    SmtpDomain = await _SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                    SmtpEnableSsl = await _SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                    SmtpUseDefaultCredentials = await _SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials),
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task SendTestEmail(SendTestEmailInput input)
        {
            try
            {
                _emailSender.Send(input.EmailAddress,"TEst","testbody", true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Ooppps! There is a problem!", ex.Message);
            };
        }

        public async Task UpdateAllSettings(HostSettingsDto input)
        {
            await UpdateEmailSettingsAsync(input.Email);
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

    }
}
