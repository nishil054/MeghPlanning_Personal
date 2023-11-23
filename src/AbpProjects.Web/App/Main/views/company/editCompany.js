(function () {
    angular.module('app').controller('app.views.company.editCompany', [
        '$scope', '$uibModalInstance', 'abp.services.app.company', 'id',
        function ($scope, $uibModalInstance, companyService, id) {
            var vm = this;
            vm.loading = false;
            vm.company = {};
            var init = function () {
                companyService.getDataById({ id: id }).then(function (result) {
                    vm.company = result.data;

                });
            }

            vm.save = function () {
                vm.loading = true;
                companyService.companyExsistenceById(vm.company).then(function (result) {
                    if (!result.data) {
                        companyService.updateCompany(vm.company).then(function () {
                            abp.notify.success(App.localize('Company Saved Successfully '));
                            $uibModalInstance.close();
                        });
                    } else {
                        abp.notify.error(App.localize('Company already Exist '));
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