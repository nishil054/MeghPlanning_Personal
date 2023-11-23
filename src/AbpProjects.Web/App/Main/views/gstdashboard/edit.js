(function () {
    angular.module('app').controller('app.views.gstdashboard.edit', [
        '$scope', '$uibModalInstance', '$uibModal', 'abp.services.app.timeSheet', 'id', 'abp.services.app.masterList', 'abp.services.app.gSTDashboard',
        function ($scope, $uibModalInstance, $uibModal, timeSheetService, id, masterListservice, gSTDashboardService) {
            //debugger;
            var vm = this;
            vm.gstdashboard = [];
            vm.monthlist = [];
            //vm.worktypelist = [];
            //vm.userstorylist = [];
           
            $scope.btndisable = false;
            var months = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            function getCompanyData() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;

                    });
            }

            function getFinancialyearData() {
                masterListservice.getFinancialYear()
                    .then(function (result) {
                        vm.financialyearlist = result.data;

                    });
            }

            function getMonths() {
                gSTDashboardService.getMonth().then(function (result) {
                    vm.monthlist = result.data;
                });
            }

            $scope.financialyearChange = function (financialyearId) {

                vm.requestParams.financialyearId = financialyearId;
                vm.requestParams.companyId = vm.gstdashboard.companyId;
                getGstDataList();

            }
            
            var init = function () {
               
                getMonths();
                getCompanyData();
                getFinancialyearData();
                gSTDashboardService.getDataById({ id: id }).then(function (result) {
                    vm.gstdashboard = result.data;
                    vm.gstdashboard.companyId = vm.gstdashboard.companyId + "";
                    vm.gstdashboard.financialyearId = vm.gstdashboard.financialyearId + "";
                    vm.gstdashboard.monthId = vm.gstdashboard.monthId + "";
                    var month = parseInt(vm.gstdashboard.monthId);
                    //var date = new Date(2020, month, 21);
                    vm.gstdashboard.monthName = months[month];
                    vm.gstdashboard.status = vm.gstdashboard.status + "";
                    //vm.gstdashboard.status = "0";
                    angular.element(document.getElementById('drpteam'))[0].disabled = true;
                    
                    $scope.financialyearChange(vm.gstdashboard.financialyearId);
                  

                });
            }
            vm.CheckNumber = function () {
                
                if (event.keyCode === 46) {

                }

                else if (isNaN(event.key) || event.key === ' ' || event.key === '') {
                    event.returnValue = '';
                }
            };

            //$scope.datachange = function (gst) {
            //    if (gst == "") {
            //        vm.gstdashboard.totalPayableGST = "";
            //        vm.gstdashboard.totalPendingPayment = "";

            //    }
            //    else {
            //        vm.gstdashboard.inputGST = gst;

            //        var regdate = vm.gstdashboard.outputGST - vm.gstdashboard.inputGST;

            //        vm.gstdashboard.totalPayableGST = regdate;
            //        vm.gstdashboard.totalPendingPayment = regdate;
            //    }

            //}

            $scope.datachange = function (gst) {
                if (gst == "") {
                    vm.gstdashboard.totalPayableGST = "";
                    vm.gstdashboard.totalPendingPayment = "";

                }
                else {
                    vm.gstdashboard.inputGST = gst;

                    var regdate = vm.gstdashboard.outputGST - vm.gstdashboard.inputGST;
                    if (regdate >= 0) {
                        vm.gstdashboard.totalPayableGST = regdate;
                        vm.gstdashboard.totalPendingPayment = regdate;
                    }
                    else {
                        vm.gstdashboard.totalPayableGST = "";
                        vm.gstdashboard.totalPendingPayment = "";
                    }
                    if (vm.gstdashboard.totalPendingPayment != 0) {

                        vm.gstdashboard.status = "0";
                        angular.element(document.getElementById('drpteam'))[0].disabled = true;
                    }
                    else {
                        vm.gstdashboard.status = "1";
                        angular.element(document.getElementById('drpteam'))[0].disabled = true;
                    }
                }

            }
            vm.save = function () {
               
                $scope.btndisable = true;
                gSTDashboardService.updateGstData(vm.gstdashboard).then(function (result) {
                    console.log(result);
                    abp.notify.success(App.localize('Saved Successfully'));
                    $uibModalInstance.close();
                });
                $scope.btndisable = false;
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();