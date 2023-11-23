(function () {
    angular.module('app').controller('app.views.worktype.createWorkType', [
        '$scope', '$uibModalInstance', 'abp.services.app.workType',
        function ($scope, $uibModalInstance, workTypeService) {
            var vm = this;
            vm.loading = false;
            vm.save = function () {
                vm.loading = true;
                workTypeService.workTypeExsistence(vm.worktype).then(function (result) {
                    if (!result.data) {
                        workTypeService.createWorkType(vm.worktype)
                            .then(function () {
                                abp.notify.success(App.localize('Work Type Saved Successfully '));
                                $uibModalInstance.close();
                            });
                    }
                    else {
                        abp.notify.error(App.localize('Work Type already Exist '));
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