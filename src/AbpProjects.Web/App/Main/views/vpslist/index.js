(function () {
    angular.module('app').controller('app.views.vpslist.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.vPS', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, vPSService, uiGridConstants) {
            var vm = this;
            $scope.record = true;
            vm.vps = {};
            vm.loading = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };
            vm.getAll = function () {
                vm.loading = true;
                abp.ui.setBusy();
                // debugger;
                vPSService.getVPSData($.extend({}, vm.requestParams)).then(function (result) {
                    //  debugger;
                    vm.vps = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                        $scope.noData = true;
                    }
                    else { $scope.noData = false;}
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }
           
           
            function getVPSSearch() {
                vm.loading = true;
                abp.ui.setBusy();
                vPSService.getVPSList($.extend({ title: vm.searchBox }, vm.requestParams)).then(function (result) {
                    vm.vps = result.data;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                    }
                    else { $scope.noData = false;}
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                getVPSSearch();
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
                        name: App.localize('Title'),
                        field: 'title',
                        enableColumnMenu: false,
                        //width: 150,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.title}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('IP'),
                        field: 'ip',
                        displayName: 'IP',
                        enableColumnMenu: false,
                        width: 450,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.ip}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('UserName'),
                        field: 'userName',
                        enableColumnMenu: false,
                        width: 250
                    },
                    {
                        name: App.localize('Password'),
                        field: 'password',
                        enableColumnMenu: false,
                        width: 200
                    },
                    //{
                    //    name: App.localize('Comment'),
                    //    field: 'comment',
                    //    enableColumnMenu: false,
                    //    width: 150
                    //},

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
           
           
            vm.getAll();
        }
    ]);
})();