using AbpProjects.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Authorization.Users
{
    public interface IUserEmailer
    {
        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        Task SendEmailActivationLinkAsync(User user, string plainPassword = null);

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        Task SendPasswordResetLinkAsync(User user);
        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        Task SendEmailUserCreationkAsync(User user, string plainPassword = null);

        Task SendMailtouser(string Email);

        //send email for Project Reminder 
        Task SendEmailReminderProjectList(GetProjectListDto obj);

        //send email for Invoice Creation
        Task SendEmailForNewInvoiceRequest(GetInvoiceRequestDto obj);

        //send email for Leave Application
        Task SendLeaveApplicationEmail(GetUserListDto obj);

        //send email for Leave Approval
        Task SendLeaveApplicationApprovalEmail(GetUserListDto obj);

        //send mail for Leave Rejection
        Task SendLeaveApplicationRejectionEmail(GetUserListDto obj);

        //send mail for Cancel Leave
        Task SendLeaveApplicationCancellationEmail(GetUserListDto obj);
    }
}
