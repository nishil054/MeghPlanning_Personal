(function () {
    angular.module('app').controller('app.views.leaveapplicationtodo.details', [
        '$scope', '$uibModalInstance', 'abp.services.app.leaveApplication', 'id',
        function ($scope, $uibModalInstance, leaveApplicationService, id) {
            var vm = this;
            vm.leaveapplicationtodo = {};

            var init = function () {

                leaveApplicationService.getLeaveDataById({ id: id }).then(function (result) {
                    vm.leaveapplicationtodo = result.data.items;
                });
            }

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();