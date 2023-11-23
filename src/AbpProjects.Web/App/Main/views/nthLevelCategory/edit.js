(function () {
    angular.module('app').controller('app.views.nthlevelcategories.edit', [
        '$scope', '$uibModalInstance', 'abp.services.app.nthCategory', 'id',
        function ($scope, $uibModalInstance, nthCategoryService, id) {
            var vm = this;
            vm.category = {};
            vm.exsist = 1;
            $scope.btndisable = false;
            vm.save = function () {
                $scope.btndisable = true;
                if (vm.category.name == null && vm.category.name == "" && vm.category.name == undefined) {

                    abp.notify.error("Please enter category name.");
                    $scope.btndisable = false;
                }
                nthCategoryService.update(vm.category).then(function (result) {
                    vm.exsist = result.data;
                    if (vm.exsist == 0) {
                        abp.notify.error("Category name " + vm.category.name + " is already exsist.");
                        $scope.btndisable = false;
                    }
                    else {
                        abp.notify.success(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    }
                   /* abp.notify.info(App.localize('SavedSuccessfully'));*/

                    
                });

            };

            $scope.filterValue = function ($event) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();

                    $scope.numberShow = true;
                }
                else {
                    $scope.numberShow = false;
                }
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            vm.getByIdData = function () {
                nthCategoryService.getCategoryById({ id: id })
                    .then(function (result) {
                        vm.category = result.data;
                    });
            };

            init = function () {
                vm.getByIdData();
            }
            init();

        }
    ]);
})();