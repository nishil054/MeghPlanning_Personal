using Abp;
using Abp.Notifications;
using AbpProjects.Authorization.Users;
using AbpProjects.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);

        //push notification for invoice request
        Task SendInvoiceRequestMessageAsync(User[] user, string message, NotificationSeverity severity = NotificationSeverity.Info);

        //Task WelcomeInstallerAsync(Installer installer);
        Task NewProjectRegisteredAsync();
        Task Publish_AddCountry(string message);
    }
}
