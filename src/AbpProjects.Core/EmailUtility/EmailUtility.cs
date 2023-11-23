using Abp.Configuration;
using Abp.Dependency;
using Abp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.EmailUtility
{
    public class EmailUtility : IEmailUtility, ITransientDependency
    {
        readonly ISettingManager _settingManager;
        public EmailUtility(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }
        public virtual void Send(string to, string cc, string subject, string body)
        {
            string DefaultFromAddress = _settingManager.GetSettingValue(EmailSettingNames.DefaultFromAddress);
            string DefaultFromDisplayName = _settingManager.GetSettingValue(EmailSettingNames.DefaultFromDisplayName);
            string SmtpHost = _settingManager.GetSettingValue(EmailSettingNames.Smtp.Host);
            string SmtpPort = _settingManager.GetSettingValue(EmailSettingNames.Smtp.Port);
            string SmtpUserName = _settingManager.GetSettingValue(EmailSettingNames.Smtp.UserName);
            string smtpPassword = _settingManager.GetSettingValue(EmailSettingNames.Smtp.Password);
            string SmtpDomain = _settingManager.GetSettingValue(EmailSettingNames.Smtp.Domain);
            string SmtpEnableSsl = _settingManager.GetSettingValue(EmailSettingNames.Smtp.EnableSsl);
            string SmtpUseDefaultCredentials = _settingManager.GetSettingValue(EmailSettingNames.Smtp.UseDefaultCredentials);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Set up the email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress(DefaultFromAddress, DefaultFromDisplayName);
            message.To.Add(to);
            message.CC.Add(cc);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            //bool isBodyHtml = true

            // Set up the SMTP client
            SmtpClient smtpClient = new SmtpClient(SmtpHost);
            smtpClient.Port = Convert.ToInt32(SmtpPort);
            smtpClient.EnableSsl = Convert.ToBoolean(SmtpEnableSsl);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = Convert.ToBoolean(SmtpUseDefaultCredentials);
            smtpClient.Credentials = new NetworkCredential(SmtpUserName, smtpPassword);
            smtpClient.Send(message);
        }
    }
}
