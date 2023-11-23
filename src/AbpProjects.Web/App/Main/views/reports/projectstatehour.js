(function () {
    angular.module('app').controller('app.views.reports.projectstatehour', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.dashboard', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, dashboardService, uiGridConstants) {
            //debugger;
            var vm = this;
            vm.statuslist = [];
            vm.projecthour = [];
            vm.reports = {};
            /*vm.projectstat = {};*/

            $scope.projreport = false;

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "hourPercentage desc"
            };

            //vm.refreshGrid = function (n) {
            //    vm.skipCount = n;
            //    getProjectStatesHourreport();

            //};

            function statusList() {
                dashboardService.statusList({}).then(function (result) {
                    vm.statuslist = result.data.items;
                    console.log(vm.statuslist);
                });
            }

            function getProjectStatesHourreport() {
                abp.ui.setBusy();
                dashboardService.getProjectStatesHourreport($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.projecthour = result.data.items;
                        if (result.data.totalCount > 0) {
                            $scope.projreport = false;
                            vm.userGridOptions.totalItems = result.data.totalCount;
                            vm.userGridOptions.data = result.data.items;
                        }
                        else {
                            $scope.projreport = true;
                        }

                        console.log(vm.projecthour);
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }


            vm.refresh = function () {
                getProjectStatesHourreport();
                statusList();
            };
            vm.searchAll = function (n) {
                vm.skipCount = n;
                if (vm.reports.projectStatusId == null || vm.reports.projectStatusId == undefined || vm.reports.projectStatusId == "") {
                    vm.requestParams.projectStatusId = null;
                }
                else {
                    vm.requestParams.projectStatusId = vm.reports.projectStatusId;
                }
                getProjectStatesHourreport();

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

                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{\'oldred-people\':(row.entity.hourPercentage>=100),\'oldyellow-people-selected\':(row.entity.hourPercentage<100 && row.entity.hourPercentage>=80),\'oldgreen-people-selected\':(row.entity.hourPercentage<80), \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
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
                        name: "Total hour",
                        field: 'totalhours',
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.totalhours | number : 2}}' +
                            '</div>'
                    },
                    {
                        name: "Actual hour",
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        field: 'actualhours',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.actualhours}}' +
                            '</div>'
                    },
                    {
                        name: "Project cost",
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        field: 'projectCost',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectCost | number : 2}}' +
                            '</div>'
                    },
                    {
                        name: "%",
                        enableColumnMenu: false,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        field: 'hourPercentage',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.hourPercentage | number : 2}}' +
                            '</div>'
                    },
                    {
                        name: "Status",
                        enableColumnMenu: false,
                        cellClass: 'centeralign statuswidth',
                        headerCellClass: 'centeralign statuswidth',
                        field: 'projectStatus',
                        width: 80
                    },

                ],

                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "hourPercentage desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }
                        getProjectStatesHourreport();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;
                        getProjectStatesHourreport();
                    });
                },
                data: []
            };


            var init = function () {
                statusList();
                getProjectStatesHourreport();
            };
            init();

        }
    ]);
})();