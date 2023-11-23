(function () {
    angular.module('app').controller('app.views.expcategorylist.detailpage', [
        '$scope', '$uibModalInstance', '$timeout', '$uibModal', 'abp.services.app.expcategory', 'id',
        function ($scope, $uibModalInstance, $timeout, $uibModal, expcategoryService, item) {

            var vm = this;
            $scope.record = true;
            vm.history = {};
            vm.loading = false;

            vm.getAll = function () {
                vm.loading = true;
                expcategoryService.getExpenseCategoryForDetail({ id: item }).then(function (result) {
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