(function () {
    angular.module('app').controller('app.views.home.editServiceWithoutClient', [
        '$scope', '$uibModalInstance', 'abp.services.app.support', 'id', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, supportService, id, masterListservice) {
            var vm = this;
            var f;
            vm.loading = false;
            vm.sname = [];
            vm.cname = [];
            vm.ename = [];
            vm.tname = [];
            vm.server = [];
            vm.persons1 = [];
            
            vm.saving = false;
            vm.task = {};

            vm.selecteddomainname = function (selected, addActivity) {
                if (selected) {
                    vm.task.domainName = selected.originalObject.domainName;
                }
            };
            vm.searchAPI = function (userInputString, timeoutPromise) {
                vm.task.domainName = userInputString;
                return supportService.getDomainNameList(userInputString).then(function (result) {
                    return result.data;
                });
            };

            var init = function () {
                supportService.getServiceForEdit(id)
                    .then(function (result) {
                        vm.task = result.data;
                        vm.task.serviceId = result.data.serviceId + "";
                        /* $scope.getServiceClearField(vm.task.serviceId);*/
                        vm.task.serviceName = result.data.serviceName + "";
                        
                        if (result.data.clientId == 0) {
                            vm.task.clientId = "0";
                        }
                        else {
                            vm.task.clientId = result.data.clientId + "";
                        }
                        vm.task.clientName = result.data.clientName + "";
                        
                        vm.task.displayTypename = result.data.displayTypename + "";
                        vm.task.employeeId = result.data.employeeId + "";
                        vm.task.employeeName = result.data.employeeName + "";
                        vm.task.serverName = result.data.serverName + "";
                        getServiceName();
                        getClientName();
                        getEmployeeName();
                        getServerName();
                        getTypeName();
                    });
                }

            vm.save = function () {
                vm.loading = false;
                supportService.updateServiceWithoutClient(vm.task).then(function (result) {
                    abp.notify.info(App.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                    console.log(vm.task);
                });
            };

            function getServiceName() {
                supportService.getServiceName({}).then(function (result) {
                    vm.sname = result.data.items;
                    console.log(vm.sname);
                });
            }
            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                    console.log(vm.cname);
                });
            }
            function getEmployeeName() {
                supportService.getUserName().then(function (result) {
                    vm.ename = result.data;
                });
            }
            init();

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();