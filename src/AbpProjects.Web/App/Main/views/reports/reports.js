
(function () {
    angular.module('app').controller('app.views.reports.reports', [
        '$scope', '$timeout', '$uibModal',  '$http', 'abp.services.app.reports', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, $http, reportsService, uiGridConstants) {
            var vm = this;
            vm.resultlist = [];
            vm.reports = {};
            vm.isChecked = true;
            vm.btndisable = true;

            function getUsers() {
                reportsService.getUser().then(function (result) {
                    vm.userlist = result.data;
                });
            }
            function getMonths() {
                reportsService.getMonth().then(function (result) {
                    vm.monthlist = result.data;
                });
            }
            function getYears() {
                reportsService.getYear().then(function (result) {
                    vm.yearlist = result.data;
                });
            }

            $scope.useridchange = function () {
                if ((vm.reports.userId != null && vm.reports.userId != undefined && vm.reports.userId != "") && (vm.reports.month != null && vm.reports.month != undefined && vm.reports.month != "") && (vm.reports.year != null && vm.reports.year != undefined && vm.reports.year != "")) {
                    vm.btndisable = false;
                } else {
                    vm.btndisable = true;
                }
            }

            $scope.monthchange = function () {
                if ((vm.reports.userId != null && vm.reports.userId != undefined && vm.reports.userId != "") && (vm.reports.month != null && vm.reports.month != undefined && vm.reports.month != "") && (vm.reports.year != null && vm.reports.year != undefined && vm.reports.year != "")) {
                    vm.btndisable = false;
                } else {
                    vm.btndisable = true;
                }
            }

            $scope.yearchange = function () {
                if ((vm.reports.userId != null && vm.reports.userId != undefined && vm.reports.userId != "") && (vm.reports.month != null && vm.reports.month != undefined && vm.reports.month != "") && (vm.reports.year != null && vm.reports.year != undefined && vm.reports.year != "")) {
                    vm.btndisable = false;
                } else {
                    vm.btndisable = true;
                }
              
            }

            vm.searchAll = function () {
                abp.ui.setBusy();
                reportsService.getProjects(vm.reports).then(function (result) {
                    if (result.data.length > 0) {
                        vm.projectlist = result.data;
                        $scope.noData = false;
                        vm.isChecked = false;
                    }
                    else {
                        $scope.noData = true;
                        vm.isChecked = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
                abp.ui.setBusy();
                reportsService.getAllData(vm.reports).then(function (result) {
                    if (result.data.length > 0) {
                        vm.resultlist = result.data;
                        $scope.noData = false;
                        vm.isChecked = false;
                    }
                    else {
                        $scope.noData = true;
                        vm.isChecked = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.exportExcel = function () {
                
                var url = "../exportToExcel/ExportReportToExcel";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        userId: vm.reports.userId,
                        month: vm.reports.month,
                        year: vm.reports.year,
                        type:1,
                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;
                    
                });

            }


            init = function () {
                getUsers();
                getMonths();
                getYears();
                $scope.noData = true;
            }
            init();
        }
    ]);
})();