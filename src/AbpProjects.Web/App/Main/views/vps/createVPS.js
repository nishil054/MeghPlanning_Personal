(function () {
    angular.module('app').controller('app.views.vps.createVPS', [
        '$scope', '$uibModalInstance', 'abp.services.app.vPS',
        function ($scope, $uibModalInstance, vPSService) {
            var vm = this;
            vm.loading = false;
            vm.save = function () {
                vm.loading = true;
                vPSService.vPSExsistence(vm.vps).then(function (result) {
                    if (!result.data) {
                        vPSService.createVPS(vm.vps)
                            .then(function () {
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


        }
    ]);
})();