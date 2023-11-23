using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Notifications;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Notifications
{
    public class AppNotifier : AbpProjectsDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<User, long> _userRepository;

        public AppNotifier(INotificationPublisher notificationPublisher,
            IRepository<Tenant> tenantRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<User, long> userRepository
            )
        {
            _notificationPublisher = notificationPublisher;
            _tenantRepository = tenantRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewUserRegisteredNotificationMessage",
                    AbpProjectsConsts.LocalizationSourceName
                    )
                );

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData, tenantIds: new[] { user.TenantId });
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewTenantRegisteredNotificationMessage",
                    AbpProjectsConsts.LocalizationSourceName
                    )
                );

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            //var data = (from u in _userRepository.GetAll()
            //            join ur in _userRoleRepository.GetAll()
            //            on u.Id equals ur.UserId
            //            join r in _roleRepository.GetAll()
            //            on ur.RoleId equals r.Id
            //            where r.DisplayName == "Employee"
            //            select new
            //            {
            //                TenantId = u.TenantId,
            //                UserId = u.Id
            //            }).ToArray();
            //string [] aa = new 
            
            //foreach (var item in data)
            //{
            //    userId: new[] {new UserIdentifier(item.TenantId,item.UserId)};
            //}
            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { new UserIdentifier(1,6),
                                 new UserIdentifier(1,8),
                                 new UserIdentifier(1,2)
                               }
                );
        }
        public async Task SendUserAddCountry(User user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(AppNotificationNames.AddCountry,
                new MessageNotificationData(message), severity: severity, userIds: new[] { user.ToUserIdentifier() });
        }

        //public async Task WelcomeInstallerAsync(Installer installer)
        //{
        //    var notificationData = new LocalizableMessageNotificationData(
        //        new LocalizableString(
        //            "NewTenantRegisteredNotificationMessage",
        //            TechnoFormsConsts.LocalizationSourceName
        //            )
        //        );
        //    var TenantName = _tenantRepository.GetAll().Where(T => T.Id == installer.TenantId).Select(T => T.TenancyName).FirstOrDefault();
        //    notificationData["tenancyName"] = TenantName;
        //    await _notificationPublisher.PublishAsync(AppNotificationNames.NewInstaller, notificationData);

        //    //await _notificationPublisher.PublishAsync(
        //    //    AppNotificationNames.NewInstaller,
        //    //    new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
        //    //    severity: NotificationSeverity.Success,
        //    //    userIds: null
        //    //    );
        //}


        public async Task NewProjectRegisteredAsync()
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "New Project Creation From admin",
                    AbpProjectsConsts.LocalizationSourceName
                    )
                );

            //notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }
        public async Task Publish_AddCountry(string message)
        {
            //Example "LowDiskWarningMessage" content for English -> "Attention! Only {remainingDiskInMb} MBs left on the disk!"
            var data = new LocalizableMessageNotificationData(new LocalizableString("LowDiskWarningMessage", "MyLocalizationSourceName"));
            data["remainingDiskInMb"] = message;

            await _notificationPublisher.PublishAsync("System.LowDisk", data, severity: NotificationSeverity.Warn);
        }

        public async Task SendInvoiceRequestMessageAsync(User[] user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            foreach(var ur in user) { 
                await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { ur.ToUserIdentifier() }
                );

            }

        }
    }
}
