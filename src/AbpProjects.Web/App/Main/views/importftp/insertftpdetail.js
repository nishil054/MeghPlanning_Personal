(function () {
    angular.module('app').controller('app.views.importftp.insertftpdetail', [
        '$scope', '$uibModalInstance', 'abp.services.app.importFTPDetails',
        function ($scope, $uibModalInstance, importFTPDetailsService) {
            var vm = this;
            vm.loading = false;
            vm.saving = false;
            vm.ftp = {};

            vm.save = function () {
                vm.loading = true;
                importFTPDetailsService.checkExist(vm.ftp).then(function (result) {

                    if (!result.data) {
                        importFTPDetailsService.createFTPDetails(vm.ftp).then(function () {
                            abp.notify.info(App.localize('Saved Successfully'));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error('DomainName is already exists');
                        vm.loading = false;
                    }
                });
            };



            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();