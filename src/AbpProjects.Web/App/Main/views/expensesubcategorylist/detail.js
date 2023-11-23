(function () {
    angular.module('app').controller('app.views.expensesubcategorylist.detail', [
        '$scope', '$uibModalInstance', '$timeout', '$uibModal', 'abp.services.app.expSubCategory', 'id',
        function ($scope, $uibModalInstance, $timeout, $uibModal, expSubCategoryService, item) {

            var vm = this;
            $scope.record = true;
            vm.history = {};
            vm.loading = false;

            vm.getAll = function () {
                vm.loading = true;
                expSubCategoryService.getExpenseCategoryForDetail({ id: item }).then(function (result) {
                    vm.history = result.data;

                });
            }
            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            vm.getAll();

        }
    ]);
})();