(function () {
    angular.module('app').controller('app.views.leaveApplication.createLeaveApp', [
        '$scope', '$filter', '$uibModal', '$uibModalInstance', 'abp.services.app.leaveApplication',
        function ($scope, $filter, $uibModal, $uibModalInstance, leaveApplicationService ) {
            //debugger;
            var vm = this;
            vm.leavetypelist = [];
            vm.leaveapplication = {};
            vm.obj = {};
            $scope.norecord = false;
            vm.leaveapplication.fromDate = moment();
            vm.leaveapplication.toDate = moment();
            $scope.btndisable = false;


            function getLeaveTypes() {
                leaveApplicationService.getLeaveType()
                    .then(function (result) {
                        vm.leavetypelist = result.data;
                    });
            }


            vm.save = function () {
                $scope.btndisable = true;
                abp.ui.setBusy();
                leaveApplicationService.createLeave(vm.leaveapplication)
                    .then(function (result) {
                        abp.notify.success(App.localize('Saved Successfully'));
                        $uibModalInstance.close();
                        $scope.btndisable = false;
                    }).finally(function () {
                        abp.ui.clearBusy();
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            var init = function () {
                getLeaveTypes();
            };

            init();

        }
    ]);
})();