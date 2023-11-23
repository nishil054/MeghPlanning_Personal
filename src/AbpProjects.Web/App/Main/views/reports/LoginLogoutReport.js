(function () {
    angular.module('app').controller('app.views.reports.loginLogoutReport', [
        '$scope', '$state', 'abp.services.app.reports', 'uiGridConstants',
        function ($scope, $state, reportsService, uiGridConstants) {
            //debugger;
            var vm = this;
            vm.reportsData = {};
            vm.reportInput = {};
            vm.yearList = [];
            vm.monthList = [];
            $scope.projreport = false;

            var date = new Date();
            vm.currentYear = date.getFullYear();

            vm.currentMonth = date.getMonth() + 1;

            //vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            //vm.timesheet.fromDate = moment(vm.firstDay);
            //vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            //vm.timesheet.toDate = moment(vm.lastDay);

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "employeeName asc"
            };

            function yearList() {
                reportsService.getUniqueYear({}).then(function (result) {
                    vm.yearList = result.data;
                    console.log(vm.yearList);

                });
            }
            function monthList() {
                reportsService.getUniqueMonth({}).then(function (result) {
                    vm.monthList = result.data;
                    console.log(vm.monthList);

                });
            }

            function GetDate() {
                //alert();
                if ((vm.reportInput.yearId != null && vm.reportInput.yearId != undefined && vm.reportInput.yearId != "") || (vm.reportInput.monthId != null && vm.reportInput.monthId != undefined && vm.reportInput.monthId != "")) {
                    //alert(vm.reportInput.yearId);
                    //alert(vm.reportInput.monthId);
                    vm.firstDay = new Date(vm.reportInput.yearId, vm.reportInput.monthId, 1);
                    //alert(vm.firstDay);
                    vm.requestParams.fromDate = moment(vm.firstDay);

                    vm.lastDay = new Date(vm.reportInput.yearId, vm.reportInput.monthId, 0);
                    //alert(vm.lastDay);
                    vm.requestParams.toDate = moment(vm.lastDay);
                }
            };

            function getLoginLogoutReport() {
                abp.ui.setBusy();
                GetDate();
                reportsService.getLoginLogoutReportData($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.reportsData = result.data.items;
                        if (vm.reportsData.length > 0) {
                            $scope.noData = false;
                            vm.loginLogoutGridOptions.totalItems = result.data.totalCount;
                            vm.loginLogoutGridOptions.data = result.data.items;
                        } else {
                            $scope.noData = true;
                        }

                        abp.ui.clearBusy();
                    });
            }



            vm.clearAll = function () {
                vm.reportInput.yearId = null;
                vm.reportInput.monthId = null;
                vm.requestParams.skipCount = 0;
                vm.requestParams.maxResultCount = app.consts.grid.defaultPageSize;
                yearList();
                monthList();
                getLoginLogoutReport();
                vm.reportInput.yearId = vm.currentYear + "";
                vm.reportInput.monthId = vm.currentMonth + "";
            };


            vm.searchAll = function () {

                if (vm.reportInput.yearId == null || vm.reportInput.yearId == undefined || vm.reportInput.yearId == "") {
                    abp.notify.error("Please select year.");
                    return;
                }
                if (vm.reportInput.monthId == null || vm.reportInput.monthId == undefined || vm.reportInput.monthId == "") {
                    abp.notify.error("Please select month.");
                    return;
                }
                getLoginLogoutReport();
            }

            vm.viewData = null;

            vm.openDetailsViewModal = function (data) {
                if (data == null) {
                    vm.viewData = null;
                }
                else {
                    vm.viewData = data;
                    $state.go('inoutDetails', { id: data.userId, fromDate: vm.requestParams.fromDate, toDate: vm.requestParams.toDate });
                }
            };


            vm.loginLogoutGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{\'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',


                columnDefs: [
                    {
                        name: "Employee Name",
                        field: 'employeeName',
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' + '<a ng-if=row.entity.loginCount!=0 ng-click="grid.appScope.openDetailsViewModal(row.entity)" class="gridlink">{{row.entity.employeeName}}</a><span ng-if=row.entity.loginCount==0>{{row.entity.employeeName}}<span>' +
                            '</div>'
                    },

                    {
                        name: "Login Count",
                        field: 'loginCount',
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        width: 100,

                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.loginCount }}' +
                            '</div>'
                    },
                ],

                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "employeeName asc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }
                        getLoginLogoutReport();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getLoginLogoutReport();
                    });
                },
                data: []
            };


            var init = function () {
                yearList();
                monthList();
                vm.reportInput.yearId = vm.currentYear + "";
                vm.reportInput.monthId = vm.currentMonth + "";
                getLoginLogoutReport();
            };
            init();

        }
    ]);
})();