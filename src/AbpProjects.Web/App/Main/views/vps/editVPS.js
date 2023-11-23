(function () {
    angular.module('app').controller('app.views.vps.editVPS', [
        '$scope', '$uibModalInstance', 'abp.services.app.vPS', 'id',
        function ($scope, $uibModalInstance, vPSService, id) {
            var vm = this;
            vm.loading = false;
            vm.vps = [];
            var init = function () {
                vPSService.getDataById({ id: id }).then(function (result) {
                    vm.vps = result.data;

                });
            }

            vm.save = function () {
                vm.loading = true;
                vPSService.vPSExsistenceById(vm.vps).then(function (result) {
                    if (!result.data) {
                        vPSService.updateVPS(vm.vps).then(function () {
                            abp.notify.success(App.localize('VPS Saved Successfully '));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('VPS already Exist '));
                        vm.loading = false;
                    }
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();