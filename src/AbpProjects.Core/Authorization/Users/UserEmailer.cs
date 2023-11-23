using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using AbpProjects.Authorization.Users.Dto;
using AbpProjects.EmailUtility;
using AbpProjects.MultiTenancy;
using AbpProjects.Net.Emailing;
using AbpProjects.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mail;

namespace AbpProjects.Authorization.Users
{
    public class UserEmailer : AbpProjectsServiceBase, IUserEmailer, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IEmailUtility _emailUtility;

        public UserEmailer(IEmailTemplateProvider emailTemplateProvider,
        IEmailSender emailSender,
        IWebUrlService webUrlService,
        IRepository<Tenant> tenantRepository,
        ICurrentUnitOfWorkProvider unitOfWorkProvider,
        IEmailUtility emailUtility)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _webUrlService = webUrlService;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
            _emailUtility = emailUtility;
        }

        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>



        [UnitOfWork]
        public virtual async Task SendEmailActivationLinkAsync(User user, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new ApplicationException("EmailConfirmationCode should be set in order to send email activation link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);

            var link = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/EmailConfirmation" +
            "?userId=" + Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.Id.ToString())) +
            "&tenantId=" + (user.TenantId == null ? "" : Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.TenantId.Value.ToString()))) +
            "&confirmationCode=" + Uri.EscapeDataString(user.EmailConfirmationCode);

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(user.TenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", L("EmailActivation_Title"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("EMAIL_USER_Register"));

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
            }

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("EmailActivation_ClickTheLinkBelowToVerifyYourEmail") + "<br /><br />");
            mailMessage.AppendLine("<a href=\"" + link + "\">" + link + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
            await _emailSender.SendAsync(user.EmailAddress, L("EmailActivation_Subject"), emailTemplate.ToString());
        }

        public async Task SendEmailForNewInvoiceRequest(GetInvoiceRequestDto obj)
        {
            
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(1));
            emailTemplate.Replace("{EMAIL_TITLE}", obj.EmailTitle);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", obj.Emailsubtitle);
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine("Below are the details of new invoice request : " + "<br />");
            if (obj.ProjectName != null)
            {
                mailMessage.AppendLine("<b>" + "Project Name " + "</b>: " + obj.ProjectName + "<br />");
            }
            else
            {
                mailMessage.AppendLine("<b>" + "Domain Name " + "</b>: " + obj.DomainName + "<br />");
            }
            mailMessage.AppendLine("<b>" + "Amount " + "</b>: " + Math.Round(obj.Amount, 2) + "<br />");
            if(obj.Comment != null) { 
                    mailMessage.AppendLine("<b>" + "Comment " + "</b>: " + obj.Comment + "<br />");
            }

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
            try
            {
                await _emailSender.SendAsync(obj.toEmail, "Notification from Megh Planning", emailTemplate.ToString());
            }
            catch (Exception ex)
            {

                //throw;
            }
            
        }

        public async Task SendEmailReminderProjectList(GetProjectListDto obj)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(1));
            emailTemplate.Replace("{EMAIL_TITLE}", obj.EmailTitle);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", obj.Emailsubtitle);
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("Following is the list of project details which are  in loss : " + "<br /><br />");
            mailMessage.AppendLine("<table border=\"1\" cellpadding=\"10\" style=\"width:100%; border-collapse:collapse; padding:10px;\">");
            mailMessage.AppendLine("<tr>");
            mailMessage.AppendLine("<th>Project Name</th>");
            mailMessage.AppendLine("<th>Total Hours</th>");
            mailMessage.AppendLine("<th>Actual Hours</th>");
            mailMessage.AppendLine("<th>Hour(%)</th>");
            mailMessage.AppendLine("</tr>");
            foreach (var item in obj.ProjectDetails)
            {
                if (item.hourPercentage >= 100)
                {
                    mailMessage.AppendLine("<tr style=\"background-color:#ffdede\">");
                }
                else if (item.hourPercentage < 100 && item.hourPercentage >= 80)
                {
                    mailMessage.AppendLine("<tr style=\"background-color:#fdffde\">");
                }
                else
                {
                    mailMessage.AppendLine("<tr style=\"background-color:#d4fecf\">");
                }

                mailMessage.AppendLine("<td>" + item.ProjectName + "</td>");
                mailMessage.AppendLine("<td>" + item.totalhours + "</td>");
                mailMessage.AppendLine("<td>" + item.actualhours + "</td>");
                mailMessage.AppendLine("<td>" + Math.Round(item.hourPercentage, 2) + "</td>");
                mailMessage.AppendLine("</tr>");
            }
            mailMessage.AppendLine("</table>");
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
            await _emailSender.SendAsync(obj.toEmail, "Notification from Megh Planning", emailTemplate.ToString());
        }

        [UnitOfWork]
        public virtual async Task SendEmailUserCreationkAsync(User user, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new ApplicationException("EmailConfirmationCode should be set in order to send email activation link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(user.TenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", L("EmailActivation_Title"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("EMAIL_USER_Register"));

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
            }

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            await _emailSender.SendAsync(user.EmailAddress, L("EMAIL_USER_Register_Message"), emailTemplate.ToString());
        }

        public async Task SendLeaveApplicationApprovalEmail(GetUserListDto obj)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(1));
            emailTemplate.Replace("{EMAIL_TITLE}", obj.EmailTitle);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", obj.Emailsubtitle);
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + "Employee Name " + "</b>: " + obj.UserName + "<br />");
            mailMessage.AppendLine("<b>" + "Type of leave " + "</b>: " + obj.LeaveType + "<br />");
            if (obj.FromDate == obj.ToDate)
            {
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + "<br />");
            }
            else
            {
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + " to " + obj.ToDate + "<br />");
            }
            mailMessage.AppendLine("<b>" + "Reason of leave " + "</b>: " + obj.Reason + "<br />");
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            _emailUtility.Send(obj.toEmail, obj.CC, "Leave Application Approved", emailTemplate.ToString());
           
        }

        public async Task SendLeaveApplicationCancellationEmail(GetUserListDto obj)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(1));
            emailTemplate.Replace("{EMAIL_TITLE}", obj.EmailTitle);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", obj.Emailsubtitle);
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + "Employee Name " + "</b>: " + obj.UserName + "<br />");
            mailMessage.AppendLine("<b>" + "Type of leave " + "</b>: " + obj.LeaveType + "<br />");
            if (obj.FromDate == obj.ToDate)
            {
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + "<br />");
            }
            else
            {
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + " to " + obj.ToDate + "<br />");
            }
            mailMessage.AppendLine("<b>" + "Reason of leave " + "</b>: " + obj.Reason + "<br />");
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            _emailUtility.Send(obj.toEmail, obj.CC, "Leave Application Cancellation", emailTemplate.ToString()); 
        }

        public async Task SendLeaveApplicationEmail(GetUserListDto obj)
        {
           
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(1));
            emailTemplate.Replace("{EMAIL_TITLE}", obj.EmailTitle);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", obj.Emailsubtitle);
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            var mailMessage = new StringBuilder();
            //mailMessage.AppendLine("Below are the details of leave request : " + "<br />");
           
            mailMessage.AppendLine("<b>" + "Employee Name " + "</b>: " + obj.UserName + "<br />");
            mailMessage.AppendLine("<b>" + "Type of leave " + "</b>: " + obj.LeaveType + "<br />");
            if(obj.FromDate == obj.ToDate)
            {
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + "<br />");
            }
            else { 
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + " to "+ obj.ToDate + "<br />");
            }
            mailMessage.AppendLine("<b>" + "Reason of leave " + "</b>: " + obj.Reason + "<br />");
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());


            //try
            //{
            _emailUtility.Send(obj.toEmail, obj.CC, "Request for Leave Application", emailTemplate.ToString());
            //await _emailSender.SendAsync(obj.toEmail, "Request for Leave Application", emailTemplate.ToString());
            //}
            //catch(Exception ex)
            //{

            //}
        }

        public async Task SendLeaveApplicationRejectionEmail(GetUserListDto obj)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(1));
            emailTemplate.Replace("{EMAIL_TITLE}", obj.EmailTitle);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", obj.Emailsubtitle);
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + "Employee Name " + "</b>: " + obj.UserName + "<br />");
            mailMessage.AppendLine("<b>" + "Type of leave " + "</b>: " + obj.LeaveType + "<br />");
            if (obj.FromDate == obj.ToDate)
            {
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + "<br />");
            }
            else
            {
                mailMessage.AppendLine("<b>" + "Date " + "</b>: " + obj.FromDate + " to " + obj.ToDate + "<br />");
            }
            mailMessage.AppendLine("<b>" + "Reason of leave " + "</b>: " + obj.Reason + "<br />");
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());

            _emailUtility.Send(obj.toEmail, obj.CC, "Leave Application Rejected", emailTemplate.ToString());
        }

        public async Task SendMailtouser(string Email)
            {
            try
            {
                await _emailSender.SendAsync(Email, "Test", "Test");
            }

            catch (Exception ex) {
                throw ex;
            }

        }

        public async Task SendPasswordResetLinkAsync(User user)
        {
            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new ApplicationException("PasswordResetCode should be set in order to send password reset link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);

            var link = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/ResetPassword" +
            "?userId=" + Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.Id.ToString())) +
            "&tenantId=" + (user.TenantId == null ? "" : Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.TenantId.Value.ToString()))) +
            "&resetCode=" + Uri.EscapeDataString(user.PasswordResetCode);

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(user.TenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", L("PasswordResetEmail_Title"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("PasswordResetEmail_SubTitle"));

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            //if (!tenancyName.IsNullOrEmpty())
            //{
            //    mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            //}

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("PasswordResetEmail_ClickTheLinkBelowToResetYourPassword") + "<br /><br />");
            mailMessage.AppendLine("<a href=\"" + link + "\">" + link + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            emailTemplate.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
            await _emailSender.SendAsync(user.EmailAddress, L("PasswordResetEmail_Subject"), emailTemplate.ToString());
        }

        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }
    }
}
