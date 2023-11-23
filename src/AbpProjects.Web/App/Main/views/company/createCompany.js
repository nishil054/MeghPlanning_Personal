(function () {
    angular.module('app').controller('app.views.company.createCompany', [
        '$scope', '$uibModalInstance', 'abp.services.app.company',
        function ($scope, $uibModalInstance, companyService) {
            var vm = this;
            $scope.btndisable = false;
            vm.loading = false;
            vm.save = function () {
                $scope.btndisable = true;
                vm.loading = true;
                companyService.companyExsistence(vm.company).then(function (result) {
                    if (!result.data) {
                        companyService.createCompany(vm.company)
                            .then(function () {
                                abp.notify.success(App.localize('Company Saved Successfully '));
                                $uibModalInstance.close();
                                $scope.btndisable = false;
                            }).finally(function () {
                                $scope.btndisable = false;
                            });
                    }
                    else {
                        abp.notify.error(App.localize('Company already Exist '));
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