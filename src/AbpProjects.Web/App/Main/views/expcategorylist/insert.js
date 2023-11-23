
(function () {
    angular.module('app').controller('app.views.expcategorylist.insert', [
        '$scope', '$uibModalInstance', 'abp.services.app.expcategory',
        function ($scope, $uibModalInstance, expcategoryService) {
            var vm = this;
            vm.categories = {};
           /* vm.catego = [];*/

            vm.saving = false;
            $scope.btndisable = false;
           

            vm.save = function () {
                $scope.btndisable = true;
                vm.saving = true;

                expcategoryService.expenseCategoryExsistence(vm.categories).then(function (result) {
                    if (!result.data) {
                        expcategoryService.expenseCreateCategory(vm.categories).then(function () {
                            abp.notify.info(App.localize('SavedSuccessfully'));
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
        
        }
    ]);
})();