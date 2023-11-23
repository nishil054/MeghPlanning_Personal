(function () {
    angular.module('app').controller('app.views.project.invoiceRequest', [
        '$scope', '$uibModalInstance', 'abp.services.app.project', 'id',
        function ($scope, $uibModalInstance, projectService,id) {
            var vm = this;
            
            
           
            vm.task = {};
            vm.invoice = [];
            $scope.inv = false;
            var init = function () {
                
                projectService.getInvoiceRequest(id)
                    .then(function (result) {
                        vm.invoice = result.data.items;

                        vm.invoice.amount = result.data.amount + "";
                        
                        vm.invoice.comment = result.data.comment + "";
                        vm.invoice.creationTime = result.data.creationTime + "";
                        vm.invoice.status = result.data.status + "";
                        if (vm.invoice != 0) {
                            $scope.inv = true;
                        }
                        else {
                            $scope.inv = false;
                        }

                        
                        console.log(vm.invoice);

                    });
            }
            vm.save = function () {
              
                abp.ui.setBusy();
                vm.task.id = id;
                projectService.createInvoiceRequest(vm.task).then(function () {
                    abp.notify.success(App.localize('SavedSuccessfully'));
                    $uibModalInstance.close();

                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            };
           
            //vm.delete = function (obj) {
            //    console.log(obj.id);
            //    projectService.deleteInvoiceRequest(obj).then(function () {
            //        abp.notify.success(App.localize('DeletedSuccessfully'));
            //        $uibModalInstance.close();
            //    });
                
            //}
            vm.delete = function (obj) {
                abp.message.confirm(
                    "Delete Invoice Request for Project '" + obj.projectName + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {
                            projectService.deleteInvoiceRequest(obj).then(function () {
                                abp.notify.success(App.localize('DeletedSuccessfully'));
                                $uibModalInstance.close();
                            });
                        }
                    });
            }
           
            init();
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();