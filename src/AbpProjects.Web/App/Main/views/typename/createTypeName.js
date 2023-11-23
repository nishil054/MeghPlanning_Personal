(function () {
    angular.module('app').controller('app.views.typename.createTypeName', [
        '$scope', '$uibModalInstance', 'abp.services.app.typename',
        function ($scope, $uibModalInstance, typeNameService) {
            var vm = this;
            $scope.btndisable = false;
            vm.loading = false;
            vm.save = function () {
                $scope.btndisable = true;
                vm.loading = true;
                typeNameService.typenameExsistence(vm.typename).then(function (result) {
                    if (!result.data) {
                        typeNameService.createTypename(vm.typename)
                            .then(function () {
                                abp.notify.success(App.localize('TypeNameSavedSuccessfully'));
                                $uibModalInstance.close();
                                $scope.btndisable = false;
                            });
                    }
                    else {
                        abp.notify.error(App.localize('TypeNamealreadyExist'));
                        $scope.btndisable = false;
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