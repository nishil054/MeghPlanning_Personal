(function () {
    angular.module('app').controller('app.views.financialyear.edit', [
        '$scope', '$uibModalInstance', '$uibModal', 'abp.services.app.timeSheet', 'abp.services.app.financialYear', 'id', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, $uibModal, timeSheetService, financialYearService, id, masterListservice) {
            //debugger;
            var vm = this;
            vm.financialyear = [];
            vm.yearlist = [];
            vm.loading = false;

            function getYears() {
                financialYearService.getFinancialYear().then(function (result) {
                    vm.yearlist = result.data;
                });
            }

            var init = function () {

                financialYearService.getFinancialDataById({ id: id }).then(function (result) {
                    vm.financialyear = result.data;
                    //vm.financialyear.startyear = vm.financialyear.startyear
                    vm.financialyear.startYear = vm.financialyear.startYear + "";
                    vm.financialyear.endYear = vm.financialyear.endYear + "";
                    $scope.startyear(vm.financialyear.startYear);
                    $scope.endyear(vm.financialyear.endYear);
                    getYears();

                });
            }

            $scope.startyear = function (startyear) {
                //debugger;
                vm.financialyear.startYear = startyear;

                //if (vm.financialyear.endYear != undefined) {
                //    vm.financialyear.title = vm.financialyear.startYear + "-" + vm.financialyear.endYear;

                //} else {
                //    vm.financialyear.title = vm.financialyear.startYear;
                //}


            }

            $scope.endyear = function (endyear) {
                //debugger;
                vm.financialyear.endYear = endyear;
                if (vm.financialyear.startYear != undefined) {
                    vm.financialyear.title = vm.financialyear.startYear + "-" + vm.financialyear.endYear;

                } else {
                    vm.financialyear.title = vm.financialyear.endYear;
                }
                //vm.financialyear.title = vm.financialyear.startYear + "-" + vm.financialyear.endYear;

            }

            vm.save = function () {
                //debugger;
                // var date1 = vm.timesheet.date;

                console.log(vm.financialyear);
                if (vm.financialyear.endYear <= vm.financialyear.startYear) {
                    abp.notify.error("EndYear should be a greater than StartYear");
                    return;

                }
                else {
                    vm.loading = true;

                    financialYearService.financialYearExsistenceByid(vm.financialyear).then(function (result) {
                        if (!result.data) {

                            financialYearService.updateFinancialYear(vm.financialyear).then(function (result) {
                                console.log(result);
                                abp.notify.success(App.localize('Saved Successfully'));
                                $uibModalInstance.close();
                            });


                        }
                        else {
                            abp.notify.error(App.localize('Financial Year already Exist '));
                            vm.loading = false;
                        }
                    });
                }

            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();