(function () {
    angular.module('app').controller('app.views.home.markasconfirm', [
        '$scope', '$uibModalInstance', 'abp.services.app.support', 'id',
        function ($scope, $uibModalInstance, supportService, id) {
            var vm = this;
           // $scope.isdata =  true;

            vm.saving = false;
            
            vm.task = {};

            var init = function () {
                /*getClientName();*/
                supportService.getServiceForEdit(id)
                    .then(function (result) {
                        vm.task = result.data;
                        vm.task.serviceId = result.data.serviceId + "";
                        vm.task.serviceName = result.data.serviceName + "";
                        vm.task.domainName = result.data.domainName + "";
                        //vm.task.nextRenewalDate = moment(result.data.nextRenewalDate).add(1, 'years');
                        vm.task.nextRenewalDate = moment(result.data.nextRenewalDate);
                        vm.task.olddate = vm.task.nextRenewalDate;
                        vm.task.tprice = vm.task.price;
                        vm.task.price = result.data.price;
                        $scope.term = vm.task.term;
                        if (vm.task.term == 0) {
                            $scope.term = 1;
                        }
                        else {
                                vm.task.price = vm.task.tprice / vm.task.term;
                        }
                        if (result.data.clientId == 0) {
                            vm.task.clientId = "0";
                        }
                        else {
                            vm.task.clientId = result.data.clientId + "";
                        }
                        if (vm.task.comment == null) {
                           
                            vm.task.comment = "";
                            
                        }
                        else {
                            vm.task.comment = result.data.comment + "";
                        }
                        vm.task.hostingSpace = result.data.hostingSpace + "";
                        vm.task.databaseSpace = result.data.databaseSpace + "";
                        if (vm.task.noOfEmail != null) {
                            vm.task.noOfEmail = result.data.noOfEmail ;
                        }
                        console.log(vm.task);

                    });
            }

            $scope.termchange = function (value) {
                if (value != null) {
                    var ans = vm.task.price * value;
                    vm.task.tprice = ans;
                    var regdate = moment.utc(vm.task.olddate);
                    //if (vm.fvalue == false && vm.dashboard == false && vm.fadjust == false) {
                    
                        if ($scope.term == 1) {
                            var nextrenewaldate = moment(regdate).add(1, 'years');
                        }
                        else {
                            var y = $scope.term;
                            var nextrenewaldate = moment(regdate).add(y, 'years');
                        }

                        vm.task.nextRenewalDate = nextrenewaldate;
                    
                }
                //}

            }

            $scope.pricechange = function (value) {
                vm.task.tprice = $scope.term * value;
            }

            //function getClientName() {
            //    supportService.getClientName({}).then(function (result) {
            //        vm.cname = result.data.items;
            //        console.log(vm.cname);
            //    });
            //}
            vm.save = function () {
                vm.task.price = vm.task.tprice;
                vm.task.term = $scope.term;
                if (vm.task.invoiceNote == null || vm.task.invoiceNote == undefined || vm.task.invoiceNote == "") {
                    abp.notify.error("Please enter Invoice Note.");
                    return;
                }
                supportService.dashboardMarkAsConfirm(vm.task).then(function () {
                    abp.notify.info(App.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                    console.log(vm.task);

                });
                
            };

            init();
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();