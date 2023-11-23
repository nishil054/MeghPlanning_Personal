(function () {
    angular.module('app').controller('app.views.supportpages.history', [
        '$scope', '$uibModalInstance', '$timeout', '$uibModal', 'abp.services.app.support', 'id',
        function ($scope,  $uibModalInstance, $timeout, $uibModal, supportService, id) {
            
            var vm = this;
            $scope.record = true;
            vm.history = {};
            vm.loading = false;
           
            vm.getAll = function () {
                vm.loading = true;
                supportService.getAllServiceHistory(id).then(function (result) {
                    vm.history = result.data.items;
                    
                });
            }
            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };
           
            vm.getAll();
            
        }
    ]);
})();