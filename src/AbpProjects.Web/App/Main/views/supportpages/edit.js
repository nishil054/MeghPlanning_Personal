(function () {
    angular.module('app').controller('app.views.supportpages.edit', [
        '$scope', '$uibModalInstance', 'abp.services.app.support', 'id', 'fid', 'famount', 'fdash','abp.services.app.masterList',
        function ($scope, $uibModalInstance, supportService, id, fid, famount, fdash, masterListservice) {
            var vm = this;
            var f;
            vm.loading = false;
            var today = new Date();
            vm.date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
            console.log(vm.date);
            vm.sname = [];
            vm.cname = [];
            vm.ename = [];
            vm.tname = [];
            vm.server = [];
            vm.persons1 = [];
            vm.fvalue = fid;
            vm.fadjust = famount;
            vm.dashboard = fdash;
            /*vm.editpopup = fedit;*/
            //vm.renew = frenew;
           
            

            vm.saving = false;
            /*vm.maintask = {};*/
            vm.task = {};

            vm.selecteddomainname = function (selected, addActivity) {
                if (selected) {
                    vm.task.domainName = selected.originalObject.domainName;
                }
            };
            vm.searchAPI = function (userInputString, timeoutPromise) {
                vm.task.domainName = userInputString;
                return supportService.getDomainNameList(userInputString).then(function (result) {
                    return result.data;
                });
            };
            
            var init = function () {
                //getServiceName();
                //getClientName();
                //getEmployeeName();
                //getServerName();
                //getTypeName();
                supportService.getServiceForEdit(id)
                    .then(function (result) {
                        vm.task = result.data;
                        vm.task.serviceId = result.data.serviceId + "";
                       /* $scope.getServiceClearField(vm.task.serviceId);*/
                        vm.task.serviceName = result.data.serviceName + "";
                       /* vm.task.f = result.data.f;*/
                        $scope.term = vm.task.term;

                        vm.task.tprice = vm.task.price;
                        if (vm.task.term != 0) {
                            vm.task.price = vm.task.tprice / vm.task.term;
                        }
                        else {
                            $scope.term = 1;
                        }

                        if (result.data.clientId == 0) {
                            vm.task.clientId = "0";
                        }
                        else {
                            vm.task.clientId = result.data.clientId + "";
                        }
                        vm.task.clientName = result.data.clientName + "";
                        if (vm.task.typeName != null) {
                            vm.task.typeName = result.data.typeName + "";
                            
                        }

                        if (vm.task.typeofssl != null) {
                            vm.task.typeofssl = result.data.typeofssl + "";

                        }
                       
                        if (vm.task.title != null) {
                            vm.task.title = result.data.title + "";

                        }
                        vm.task.displayTypename = result.data.displayTypename + "";
                        vm.task.employeeId = result.data.employeeId + "";
                        vm.task.employeeName = result.data.employeeName + "";
                        if (vm.task.serverType != null) {
                            vm.task.serverType = result.data.serverType + "";
                        }
                        

                        vm.task.serverName = result.data.serverName + "";
                        
                        
                        vm.task.nextRenewalDate = moment(vm.task.nextRenewalDate);
                        vm.task.registrationDate = moment(vm.task.registrationDate);
                        console.log(vm.task);
                        getServiceName();
                        getClientName();
                        getEmployeeName();
                        getServerName();
                        getTypeName();
                    });
            }

            if (vm.fvalue == false && vm.dashboard == false && vm.fadjust == false) {
                $scope.datechange = function (registrationDate) {
                    var regdate = moment.utc(registrationDate);
                    if ($scope.term == 1) {
                        var nextrenewaldate = moment(regdate).add(1, 'years');
                    }
                    else {
                        var y = $scope.term;
                        var nextrenewaldate = moment(regdate).add(y, 'years');
                    }

                    vm.task.nextRenewalDate = nextrenewaldate;
                
                    vm.loading = false;
            }
            }

            $scope.datechange = function (registrationDate) {
                vm.loading = false;
                var regdate = moment.utc(registrationDate);
                if ($scope.term == 1) {
                    var nextrenewaldate = moment(regdate).add(1, 'years');
                }
                else {
                    var y = $scope.term;
                    var nextrenewaldate = moment(regdate).add(y, 'years');
                }

                vm.task.nextRenewalDate = nextrenewaldate;

                
            }

            $scope.termchange = function (value) {
                if (value != null) {
                    var ans = vm.task.price * value;
                    vm.task.tprice = ans;
                    var regdate = moment.utc(vm.task.registrationDate);
                    //if (vm.fvalue == false && vm.dashboard == false && vm.fadjust == false) {
                        if (vm.task.registrationDate != null) {
                            if ($scope.term == 1) {
                                var nextrenewaldate = moment(regdate).add(1, 'years');
                            }
                            else {
                                var y = $scope.term;
                                var nextrenewaldate = moment(regdate).add(y, 'years');
                            }

                            vm.task.nextRenewalDate = nextrenewaldate;
                        }
                    }
                //}

            }

            $scope.pricechange = function (value) {
                vm.task.tprice = $scope.term * value;
            }
           
            $scope.divdisable = function () {
                adjustamount= true;
            }

            vm.save = function (price) {
                vm.loading = false;
                if (vm.task.serviceId != 7) {
                    vm.task.credits = null;
                }
                if (vm.fadjust == true) {
                    if (vm.task.invoiceNote == null || vm.task.invoiceNote == undefined || vm.task.invoiceNote == "") {
                        abp.notify.error("Please enter Invoice Note.");
                        return;
                    }
                }
                vm.task.checkadjustment = vm.fadjust;
                vm.task.checkrenew = vm.fvalue;
                vm.task.checkrenewDashboard = vm.dashboard;
                if (vm.task.term != 0) {
                    vm.task.price = vm.task.term * vm.task.price;
                }
                var newprice = Number(vm.task.price) + Number(price);
                vm.task.price = newprice;
                supportService.updateService(vm.task).then(function (result) {
                        abp.notify.info(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                        console.log(vm.task);
                });
            };

            function getServiceName() {
                supportService.getServiceName({}).then(function (result) {
                    vm.sname = result.data.items;
                    console.log(vm.sname);
                });
            }
            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                    console.log(vm.cname);
                });
            }
            function getTypeName() {
                masterListservice.getTypeName({}).then(function (result) {
                    vm.tname = result.data.items;
                    console.log(vm.tname);
                });
            }
            function getEmployeeName() {
                //supportService.getEmployeeName({}).then(function (result) {
                //    vm.ename = result.data.items;
                //    console.log(vm.ename);
                //});
                supportService.getUserName().then(function (result) {
                    vm.ename = result.data;
                    //console.log(vm.ename);
                });
            }
            function getServerName() {
                supportService.getServerName({}).then(function (result) {
                    vm.server = result.data.items;
                    console.log(vm.server);
                });
            }

            //vm.datechange = function (registrationDate) {

            //    var regdate = moment.utc(registrationDate);
            //    var nextrenewaldate = moment(regdate).add(1, 'years');
            //    vm.task.nextRenewalDate = nextrenewaldate;

            //};
            //var init = function () {
            //    getServiceName();
            //    getClientName();
            //    getEmployeeName();
            //}
            init();
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();