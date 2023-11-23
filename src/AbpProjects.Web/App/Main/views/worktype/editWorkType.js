(function () {
    angular.module('app').controller('app.views.worktype.editWorkType', [
        '$scope', '$uibModalInstance', 'abp.services.app.workType', 'id',
        function ($scope, $uibModalInstance, workTypeService, id) {
            var vm = this;
            vm.loading = false;
            vm.worktype = [];
            var init = function () {
                workTypeService.getDataById({ id: id }).then(function (result) {
                    vm.worktype = result.data;

                });
            }



            vm.save = function () {
                vm.loading = true;
                workTypeService.workTypeExsistenceById(vm.worktype).then(function (result) {
                    if (!result.data) {
                        workTypeService.updateWorkType(vm.worktype).then(function () {
                            abp.notify.success(App.localize('Work Type Saved Successfully '));
                            $uibModalInstance.close();
                        });

                    } else {
                        abp.notify.error(App.localize('Work Type already Exist '));
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