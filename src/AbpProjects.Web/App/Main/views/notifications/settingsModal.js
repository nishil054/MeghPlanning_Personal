(function () {
    angular.module('app').controller('views.notifications.settingsModal', [
        '$scope','$state', '$uibModalInstance', 'abp.services.app.notifications',
        function ($scope, $state, $uibModalInstance, notificationAppService) {
            var vm = this;
          
            vm.settings = null;

            vm.saving = false;

            vm.save = function () {
                vm.saving = true;
                notificationAppService.updateNotificationSettings(vm.settings)
                    .then(function () {
                        if (vm.settings.receiveNotifications=="true") {
                            abp.notify.success("Notifucations is On.");
                        } else {
                            abp.notify.success("Notifucations is Off.");
                        }
                        $uibModalInstance.dismiss('cancel');
                   // $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                notificationAppService.getNotificationSettings({}).then(function (result) {
                    vm.settings = result.data;
                    vm.settings.receiveNotifications = vm.settings.receiveNotifications + "";
                });
            }

            init();
        }
    ]);
})();