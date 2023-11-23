(function () {
    angular.module('app').controller('app.views.notification.index', [
        '$scope', '$timeout', '$uibModal', '$http', '$state', '$stateParams', 'abp.services.app.masterList', 'abp.services.app.notification',
        function ($scope, $timeout, $uibModal, $http, $state, $stateParams, masterListService, notificationService) {
            var vm = this;
            vm.notification = {};
            vm.userList = [];
            vm.notificationList = [];
            vm.projectList = [];

            function getUsers() {
                masterListService.getUser()
                    .then(function (result) {
                        vm.userList = result.data;
                    });
               
            }

            function getAllProjects() {
                notificationService.getReminderProjectList()
                    .then(function (result) {
                        vm.projectList = result.data;
                    });
            }

            vm.getAll = function () {
                abp.ui.setBusy();
                notificationService.getNotificationDetails()
                    .then(function (result) {
                        vm.notification = result.data;
                        getUsers();
                    }).finally(function () {
                         abp.ui.clearBusy();
                });
            }

            vm.save = function () {
                vm.notification.notificationId = 1;
                vm.notification.projectDetails = vm.projectList;
                if (vm.notification.userId == null || vm.notification.userId == undefined || vm.notification.userId == "") {
                    abp.notify.error('Please select user');
                    return;
                }
                abp.ui.setBusy();


                notificationService.updateNotification(vm.notification).then(function () {
                    abp.notify.success(App.localize('SavedSuccessfully'));
                }).finally(function () {
                    vm.notification.userId = [];
                    vm.userList = [];
                    init();
                    abp.ui.clearBusy();
                });
            };

            var init = function () {
                vm.getAll();
                getAllProjects();
            }

            init();
        }
    ]);
})();