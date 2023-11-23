(function () {
    angular.module('app').controller('app.views.projecttype.editProjectType', [
        '$scope', '$uibModalInstance', 'abp.services.app.projectType', 'id',
        function ($scope, $uibModalInstance, projectTypeService, id) {
            var vm = this;
            vm.loading = false;
            vm.projecttype = [];
            var init = function () {
                projectTypeService.getDataById({ id: id }).then(function (result) {
                    vm.projecttype = result.data;

                });
            }

            vm.save = function () {
                vm.loading = true;
                projectTypeService.projectTypeExsistenceById(vm.projecttype).then(function (result) {
                    if (!result.data) {
                        projectTypeService.updateProjectType(vm.projecttype).then(function () {
                            abp.notify.success(App.localize('Project Type Saved Successfully '));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('Project Type already Exist '));
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