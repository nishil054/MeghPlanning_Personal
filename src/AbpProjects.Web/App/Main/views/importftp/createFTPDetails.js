(function () {
    angular.module('app').controller('app.views.importftp.createFTPDetails', [
        '$scope', '$state', '$uibModal', '$stateParams', 'abp.services.app.importFTPDetails',
        function ($scope, $state, $uibModal, $stateParams, importFTPDetailsService) {
            var vm = this;
            vm.data = {};

            vm.save = function () {

                importFTPDetailsService.checkExist(vm.ftp).then(function (result) {

                    if (!result.data) {
                        importFTPDetailsService.createFTPDetails(vm.ftp).then(function () {
                            abp.notify.info(App.localize('FTP Details Saved Successfully'));
                            $uibModalInstance.close();
                        });
                    }
                    else {

                        abp.notify.error('DomainName is alredy exists');
                    }
                });
            };

            //vm.save = function () {
            //            importFTPDetailsService.createFTPDetails(vm.ftp)
            //                .then(function () {
            //                    abp.notify.success(App.localize('FTP Details Saved Successfully '));
                               
            //                    $state.go('importftp');
            //                });
                
            //};

            vm.cancel = function () {
                $state.go('importftp');
            };
        }
    ]);
})();