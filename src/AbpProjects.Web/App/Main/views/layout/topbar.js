(function () {
    var controllerId = 'app.views.layout.topbar';
    angular.module('app').controller(controllerId, [
        '$rootScope', '$state', 'appSession', 'appUserNotificationHelper', 'abp.services.app.notifications',
        function ($rootScope, $state, appSession, appUserNotificationHelper, notificationService) {
            var vm = this;
            vm.languages = [];
            vm.currentLanguage = {};

            vm.notifications = [];
            vm.unreadNotificationCount = 0;
            vm.unreadChatMessageCount = 0;
            vm.recentlyUsedLinkedUsers = [];
            vm.tenant = appSession.tenant;


            //taking from side bar
            vm.userEmailAddress = appSession.user.emailAddress;
            /* vm.userName = appSession.user.userName;*/
            vm.userName = appSession.user.name + " " + appSession.user.surname;

            function init() {
                vm.languages = abp.localization.languages;
                vm.currentLanguage = abp.localization.currentLanguage;
                vm.loadNotifications();
            }

            vm.changeLanguage = function (languageName) {
                location.href = abp.appPath + 'AbpLocalization/ChangeCulture?cultureName=' + languageName + '&returnUrl=' + window.location.pathname + window.location.hash;
            }


            vm.loadNotifications = function () {
                notificationService.getUserNotifications({
                    maxResultCount: 3
                }).then(function (result) {
                    vm.unreadNotificationCount = result.data.unreadCount;
                    vm.notifications = [];
                    $.each(result.data.items, function (index, item) {
                        vm.notifications.push(appUserNotificationHelper.format(item));
                        if (vm.notifications[index].data.message.includes("for domain")) {
                            vm.notifications[index].url = "#/serviceinvoicerequest";
                        } else {
                            vm.notifications[index].url = "#/projectinvoicerequest";
                        }
                    });
                });
            }

            vm.setAllNotificationsAsRead = function () {
                appUserNotificationHelper.setAllAsRead();
            };

            vm.setNotificationAsRead = function (userNotification) {
                appUserNotificationHelper.setAsRead(userNotification.userNotificationId);
            }

            vm.openNotificationSettingsModal = function () {
                appUserNotificationHelper.openSettingsModal();
            };

            // 3 times call notification bug
            abp.event.on('abp.notifications.received', function (userNotification) {
                //appUserNotificationHelper.show(userNotification);
                vm.loadNotifications();
            });

            abp.event.on('app.notifications.refresh', function () {
                vm.loadNotifications();
            });

            abp.event.on('app.notifications.read', function (userNotificationId) {
                for (var i = 0; i < vm.notifications.length; i++) {
                    if (vm.notifications[i].userNotificationId == userNotificationId) {
                        vm.notifications[i].state = 'READ';
                    }
                }

                vm.unreadNotificationCount -= 1;
            });

            //Chat
            abp.event.on('app.chat.unreadMessageCountChanged', function (messageCount) {
                vm.unreadChatMessageCount = messageCount;
            });
            init();

        }
    ]);
})();