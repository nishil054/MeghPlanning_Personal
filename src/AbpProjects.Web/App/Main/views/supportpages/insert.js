(function () {
    angular.module('app').controller('app.views.supportpages.insert', [
        '$scope', '$uibModalInstance', 'abp.services.app.support', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, supportService, masterListservice) {
            var vm = this;
            vm.loading = false;
            //var today = moment();
            //vm.mindate=today.subtr
            var today = new Date();
            vm.date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
            //console.log(vm.date);
            vm.sname = [];
            vm.cname = [];
            vm.tname = [];

            vm.ename = [];
            vm.server = [];
            $scope.date = new Date();
            $scope.term = 1;
            vm.saving = false;
            vm.maintask = {};
            vm.task = {};
            $scope.solution = "Y";
            /*$scope.IsVisible = true;*/
            //$scope.ShowRevert = function (value) {
            //    $scope.IsVisible = value == "Y";
            //}
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
            vm.save = function () {
                if ($scope.solution == 'Y') {
                    if (vm.task.invoiceNote == null || vm.task.invoiceNote == undefined || vm.task.invoiceNote == "") {
                        abp.notify.error("Please enter Invoice Note.");
                        return;
                    }
                }
                if (vm.task.serviceId != 7) {
                    vm.task.credits = null;
                }
                else if (vm.task.serviceId == 7) {
                    vm.task.typeName = null;
                    vm.task.noOfEmail = null;
                    vm.task.hostingSpace = null;
                    vm.task.title = null;
                    vm.task.typeofssl = null;
                    if (vm.task.credits == null || vm.task.credits == undefined || vm.task.credits == "") {
                        abp.notify.error("Please enter credits.");
                        return;
                    }
                }
                /* $scope.change*/
                vm.task.solution = $('input[name="radiobtn"]:checked').val();
                if (vm.task.clientId == "") {
                    vm.task.clientId = "0";
                }
                else {
                    vm.task.clientId = vm.task.clientId;
                }

                vm.task.term = $scope.term;
                if (vm.task.term != null) {
                    vm.task.price = vm.task.term * vm.task.price;

                }
                vm.loading = true;

                supportService.createService(vm.task).then(function () {
                    abp.notify.info(App.localize('SavedSuccessfully'));
                    $uibModalInstance.close();

                });
            };

            $scope.termchange = function (value) {
                if (value != null) {
                    var ans = vm.task.price * value
                    vm.task.tprice = ans;
                    if (vm.task.registrationDate != null) {
                        $scope.datechange(vm.task.registrationDate);
                    }}
                
            }

            $scope.pricechange = function (value) {
                vm.task.tprice = $scope.term * value;
            }
           
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

            }
            //$scope.getServiceClearField = function (id) {

            //    supportService.getServiceClearField(id).then(function (result) {
            //        vm.persons1 = result.data.items;

            //        console.log(vm.persons1);
            //    });

            //}
            function getServiceName() {
                supportService.getServiceName({}).then(function (result) {
                    vm.sname = result.data.items;
                    //console.log(vm.sname);
                });
            }
            function getTypeName() {
                masterListservice.getTypeName({}).then(function (result) {
                    vm.tname = result.data.items;
                    //console.log(vm.tname);
                });
            }
            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                    //console.log(vm.cname);
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
                    //console.log(vm.server);
                });
            }
            var init = function () {
                getServiceName();
                getClientName();
                getEmployeeName();
                getServerName();
                getTypeName();
            }
            init();
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();