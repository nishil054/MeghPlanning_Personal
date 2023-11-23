
(function () {
    angular.module('app').controller('app.views.expcategorylist.edit', [
        '$scope', '$uibModalInstance', 'abp.services.app.expcategory', 'id',
        function ($scope, $uibModalInstance, expcategoryService,id) {
            var vm = this;
            vm.items = {};
            /* vm.catego = [];*/

            vm.saving = false;
            $scope.btndisable = false;
            var init = function () {

                vm.saving = true;
                expcategoryService.getExpenseCategoryForEdit({ id: id })
                    .then(function (result) {
                        vm.items = result.data;
                    });
            };

            vm.save = function () {
                $scope.btndisable = true;
                vm.saving = true;

                expcategoryService.expenseCategoryExsistenceById(vm.items).then(function (result) {
                    if (!result.data) {
                        expcategoryService.updateExpenseCategory(vm.items).then(function () {
                            abp.notify.info(App.localize('UpdatedSuccessfully'));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('Category Already Exist'));
                        $scope.btndisable = false;
                    }
                });

            };
            vm.close = function () {
                $uibModalInstance.dismiss();
            };
            init();
        }
    ]);
})();