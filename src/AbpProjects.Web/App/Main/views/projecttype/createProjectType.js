(function () {
    angular.module('app').controller('app.views.projecttype.createProjectType', [
        '$scope', '$uibModalInstance', 'abp.services.app.projectType',
        function ($scope, $uibModalInstance, projectTypeService) {
            var vm = this;
            vm.loading = false;
            vm.save = function () {
                vm.loading = true;


                projectTypeService.projectTypeExsistence(vm.projecttype).then(function (result) {
                    if (!result.data) {
                        projectTypeService.createProjectType(vm.projecttype)
                            .then(function () {
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


        }
    ]);
})();