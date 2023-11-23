(function () {
    angular.module('app').controller('app.views.reports.inOutDetailsReport', [
        '$scope', '$stateParams', 'abp.services.app.reports', 'uiGridConstants',
        function ($scope, $stateParams, reportsService, uiGridConstants) {
            var vm = this;
            vm.reportsData = {};
            vm.reportInput = {};
            $scope.projreport = false;

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "date asc"
            };
            function getInOutDetailsReport() {
                abp.ui.setBusy();
                reportsService.getInOutDetailsReportData($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.reportsData = result.data.items;
                        if (vm.reportsData.length > 0) {
                            $scope.noData = false;
                            vm.inoutGridOptions.totalItems = result.data.totalCount;
                            vm.inoutGridOptions.data = result.data.items;
                        } else {
                            $scope.noData = true;
                        }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }

            vm.inoutGridOptions = {
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
                        name: "Date",
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        field: 'date',
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        width: 120
                    },
                    {
                        name: "LogIn",
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        field: 'logInTime',
                        cellFilter: 'momentFormat: \'HH:mm\'',
                        width: 120
                    },

                    {
                        name: "LogOut",
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        field: 'logOutTime',
                        cellFilter: 'momentFormat: \'HH:mm\'',
                        width: 120
                    },
                ],

                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "date desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }
                        getInOutDetailsReport();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getInOutDetailsReport();
                    });
                },
                data: []
            };

            var init = function () {
              
                if ($stateParams.id != "") {
                    vm.requestParams.userId = parseInt($stateParams.id);
                    vm.requestParams.fromDate = moment($stateParams.fromDate);
                    vm.requestParams.toDate = moment($stateParams.toDate);
                    getInOutDetailsReport();

                }
            };
            init();

        }
    ]);
})();