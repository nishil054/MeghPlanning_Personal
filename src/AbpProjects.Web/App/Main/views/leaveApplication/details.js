(function () {
    angular.module('app').controller('app.views.leaveapplication.details', [
        '$scope', '$uibModalInstance', 'abp.services.app.leaveApplication', 'id',
        function ($scope, $uibModalInstance, leaveApplicationService, id) {
            var vm = this;
            vm.leaveapplication = {};
           
            var init = function () {

                leaveApplicationService.getLeaveDataById({ id: id }).then(function (result) {
                    vm.leaveapplication = result.data.items;
                });
            }

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();