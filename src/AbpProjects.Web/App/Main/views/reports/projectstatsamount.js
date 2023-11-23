(function () {
    angular.module('app').controller('app.views.reports.projectstatsamount', [
        '$scope', '$timeout', '$uibModal', '$http', 'abp.services.app.reports', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, $http, reportsService, uiGridConstants) {
            var vm = this;
            vm.statuslist = [];
            vm.resultlist = [];
            vm.reports = {};
            vm.isChecked = true;
            $scope.noData = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "costPercentage desc"
            };

            function getAllStatus() {
                reportsService.getStatus().then(function (result) {
                    if (result.data.length > 0) {
                        vm.statuslist = result.data;
                        $scope.noData = false;
                        vm.isChecked = false;
                    }
                    else {
                        $scope.noData = true;
                        vm.isChecked = true;
                    }
                });
            }

            vm.getAll = function () {
                abp.ui.setBusy();
                reportsService.getProjectStatsAmount($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.resultlist = result.data.items;
                        if (result.data.totalCount > 0) {
                            vm.userGridOptions.totalItems = result.data.totalCount;
                            vm.userGridOptions.data = result.data.items;
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


            vm.clearAll = function () {
                getAllStatus();
                vm.requestParams.status = null;
                vm.requestParams.skipCount = 0;
                vm.requestParams.maxResultCount = app.consts.grid.defaultPageSize;
                vm.reports.status = "";
                $("#s2id_ddlstatus").select2("val", null);
                vm.getAll();
            };
            vm.searchAll = function () {
                if (vm.reports.status == null || vm.reports.status == undefined || vm.reports.status == "") {
                    vm.requestParams.status = null;
                } else {
                    vm.requestParams.status = vm.reports.status;
                }
                vm.getAll();
            }

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.exportExcel = function () {

                var url = "../exportToExcel/ExportProjectStateAmountToExcel";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        status: vm.reports.status,
                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });

            }


            vm.getStyle = function (status) {
                if (status > 100)
                    return { 'background-color': '#ff0000' };
                else if (status < 100 && status <= 80)
                    return { 'background-color': '#FFFF00' };
                else
                    return { 'background-color': '#00FF00' };
            };

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                /*enablePaginationControls: false,*/
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" ng-style=\"grid.appScope.getStyle(row.entity.costPercentage)\" class="ui-grid-cell" ng-class="{\'oldred-people\':(row.entity.costPercentage>=100), \'oldyellow-people-selected\':(row.entity.costPercentage<100 && row.entity.costPercentage>=80),\'oldgreen-people-selected\':(row.entity.costPercentage<80), \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: "Project Name",
                        field: 'projectName',
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectName}}' +
                            '</div>'
                    },

                    {
                        name: "Project Cost",
                        field: 'projectCost',
                        enableColumnMenu: false,
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectCost | number : 2}}' +
                            '</div>',
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                    },
                    {
                        name: "Company Cost",
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        field: 'companyCost',
                        width: 120,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.companyCost | number : 2}}' +
                            '</div>'
                    },
                    {
                        name: "%",
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        field: 'costPercentage',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.costPercentage | number : 2}}' +
                            '</div>'
                    },
                    {
                        name: "profit",
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        field: 'profit',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.profit | number : 2}}' +
                            '</div>'
                    },
                    {
                        name: "Profit (%)",
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        field: 'profitPercentage',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.profitPercentage | number : 2}}' +
                            '</div>'
                    },
                    {
                        name: "Status",
                        enableColumnMenu: false,
                        cellClass: 'centeralign statuswidth',
                        headerCellClass: 'centeralign statuswidth',
                        field: 'status',
                        width: 80
                    },

                ],

                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "costPercentage desc"
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
                getAllStatus();
                vm.getAll();
                $scope.noData = false;
            }
            init();
        }
    ]);
})();