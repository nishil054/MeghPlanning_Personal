(function () {
    angular.module('app').controller('app.views.reason.Edit', [
        '$uibModalInstance', 'abp.services.app.reason', 'id',
        function ($uibModalInstance, reasonService, id) {
            var vm = this;
            vm.saving = false;
            vm.items = {};
            var init = function () {

                vm.saving = true;
                reasonService.getReasonForEdit({ id: id })
                    .then(function (result) {
                        vm.items = result.data;
                    });
            };

            vm.save = function () {
                vm.saving = true;
                reasonService.reasonExsistenceById(vm.items).then(function (result) {
                    if (!result.data) {
                        reasonService.updateReason(vm.items).then(function () {
                            abp.notify.info(App.localize('UpdatedSuccessfully'));
                            $uibModalInstance.close();
                        });
                    } else {
                        abp.notify.error(App.localize('Reason Already Exist.'));
                    }
                });
            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();