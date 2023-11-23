(function () {
    angular.module('app').controller('app.views.importftp.Details', [
        '$scope', '$uibModalInstance', 'abp.services.app.importFTPDetails', 'id',
        function ($scope, $uibModalInstance, importService, item) {
            var vm = this;
            vm.saving = false;
            vm.items = {};

            var init = function () {
                vm.saving = true;
                importService.getImportFTPDetail({ id: item })
                    .then(function (result) {
                        vm.items = result.data;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();