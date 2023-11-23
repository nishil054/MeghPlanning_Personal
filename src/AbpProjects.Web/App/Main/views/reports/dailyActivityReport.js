(function () {
    angular.module('app').controller('app.views.reports.dailyActivityReport', [
        '$scope', '$state', '$uibModal', 'uiGridConstants', 'abp.services.app.reports', '$http', 'abp.services.app.masterList',
        function ($scope, $state, $uibModal, uiGridConstants, reportsService, $http, masterListservice) {
            var vm = this;
            vm.dailydata = [];
            var date = new Date();
            vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            vm.toDate = moment(vm.lastDay);
            vm.fromDate = moment(vm.firstDay);

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                //assignUserId: vm.assignUserId,
                fromDate: vm.fromDate,
                toDate: vm.toDate,
            };

            vm.getAll = function () {
                abp.ui.setBusy();
                vm.loading = true;
                reportsService.getDailyActivityReport($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.dailydata = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                            $scope.noData = true;
                            vm.isChecked = true;
                        } else { $scope.noData = false; vm.isChecked = false; }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });

            }

            function getMarketingUsers() {
                opportunityService.getMarketingUsers({}).then(function (result) {
                    vm.assignUsers = result.data.items;
                    console.log(vm.assignUsers);
                });
            }

            vm.search = function (n) {
                vm.skipCount = n;
                //if (vm.assignUserId == "") {
                //    vm.requestParams.assignUserId = null;
                //} else {
                //    vm.requestParams.assignUserId = vm.assignUserId;
                //}
                if (vm.fromDate != null) {
                    vm.requestParams.fromDate = vm.fromDate;

                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.toDate != null) {
                    vm.requestParams.toDate = vm.toDate;
                } else {
                    vm.requestParams.toDate = null;
                }
                vm.getAll();
            };

            vm.exportExcel = function () {
                var datefromdate = vm.fromDate;
                var datetodate = vm.toDate;
                if (vm.fromDate != null) {
                    datefromdate = vm.fromDate._d;
                }
                if (vm.toDate != null) {
                    datetodate = vm.toDate._d;
                }

                var url = "../exportToExcel/DailyActivityReportExport";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        //assignUserId: vm.assignUserId,
                        fromDate: datefromdate,
                        toDate: datetodate,
                    },

                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });

            }

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.clear = function () {
                //vm.assignUserId = "";
                //vm.requestParams.assignUserId = null;
                //$("#ddlassignUsers").select2("val", null);
                vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                vm.fromDate = moment(vm.firstDay);
                vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                vm.toDate = moment(vm.lastDay);
                vm.toDate = null;
                vm.fromDate = null
                vm.requestParams.toDate = null;
                vm.requestParams.fromDate = null;
                vm.getAll();
            };

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: 'Date',
                        field: 'date',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 120,
                        //cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                    },
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getAll();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getAll();
                    });
                },
                data: []
            };


            init = function () {
                vm.isChecked = true;
                //getMarketingUsers();
                vm.getAll();
            }

            init();
        }
    ]);
})();