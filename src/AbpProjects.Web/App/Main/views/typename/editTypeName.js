(function () {
    angular.module('app').controller('app.views.typename.editTypeName', [
        '$scope', '$uibModalInstance', 'abp.services.app.typename', 'id',
        function ($scope, $uibModalInstance, typeNameService, id) {
            var vm = this;
            vm.loading = false;
            vm.typename = [];
            var init = function () {
                typeNameService.getDataById({ id: id }).then(function (result) {
                    vm.typename = result.data;

                });
            }

            vm.save = function () {
                vm.loading = true;
                typeNameService.typenameExsistenceById(vm.typename).then(function (result) {
                    if (!result.data) {
                        typeNameService.updateTypename(vm.typename).then(function () {
                            abp.notify.success(App.localize('TypeNameSavedSuccessfully'));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('TypeNamealreadyExist'));
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