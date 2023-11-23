
(function () {
    angular.module('app').controller('app.views.financialyear.create', [
        '$scope', '$timeout', '$uibModal', '$uibModalInstance', '$http', 'abp.services.app.financialYear', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, $uibModalInstance, $http, financialYearService, uiGridConstants) {
            var vm = this;
            vm.resultlist = [];
            vm.yearlist = [];
            vm.reports = {};
            vm.financialyear = {};
            vm.isChecked = true;
            vm.loading = false;



            function getYears() {
                financialYearService.getFinancialYear().then(function (result) {
                    vm.yearlist = result.data;
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
                //vm.loading = true;
                console.log(vm.financialyear);
                if (vm.financialyear.endYear <= vm.financialyear.startYear) {
                    abp.notify.error("EndYear should be a greater than StartYear");
                    return;

                }
                else {
                    vm.loading = true;
                    financialYearService.financialYearExsistence(vm.financialyear).then(function (result) {
                        if (!result.data) {

                            financialYearService.createFinancialYear(vm.financialyear)
                                .then(function () {
                                    abp.notify.success(App.localize('Financial Year Saved Successfully '));
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

            init = function () {
                getYears();
            }
            init();
        }
    ]);
})();