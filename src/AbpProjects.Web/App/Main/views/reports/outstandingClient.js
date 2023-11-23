(function () {
    angular.module('app').controller('app.views.outstandingClient', [
        '$scope', '$state', '$uibModal', 'abp.services.app.reports', 'uiGridConstants', 'abp.services.app.support',
        function ($scope, $state, $uibModal, reportsService, uiGridConstants, supportService) {
            /*ImportFTPDetailsService*/
            //debugger;
            var vm = this;
            vm.norecord = false;
            vm.clientId = 0;
            vm.cname = [];
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "ClientName desc",
                clientId: vm.clientId,
                fromDate: vm.fromDate,
                toDate: vm.toDate,
            };
            vm.getAll = function () {
                abp.ui.setBusy();
                vm.loading = true;
                reportsService.getOutStandingClient(vm.requestParams).then(function (result) {
                    //console.log(result.data);
                    vm.userGridOptions.data = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    if (vm.clientId == 0) {
                        vm.clientId = "";
                    }

                    if (result.data.totalCount == 0) {
                        vm.norecord = true;
                        $scope.noData = true;
                        //abp.notify.info(app.localize('NoRecordFound'));
                    } else {
                        vm.norecord = false;
                        $scope.noData = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                showColumnFooter: true,

                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: App.localize('Client Name'),
                        displayName: 'Client Name',
                        field: 'ClientName',
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
                            '      <a ng-click="grid.appScope.typeData(row.entity.clientId)" class=\"culserpoint\"> {{row.entity.clientName}} </a>' +
                            '  </div>' +
                            '</div>',

                    },
                    {
                        name: App.localize('Total Amount'),
                        displayName: 'Total Amount',
                        field: 'totalBillAmt',
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        enableColumnMenu: false,
                        width:150,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.totalBillAmt}}' +
                            '</div>',
                    },
                    {
                        name: App.localize('Total Collection'),
                        displayName: 'Total Collection',
                        /* field: 'DatabaseDetails',*/
                        field: 'totalCollection',
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        width:150,
                        enableColumnMenu: false,
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.totalCollection}}' +
                            '</div>',
                    },
                    {
                        name: App.localize('OutStanding Amount'),
                        displayName: 'OutStanding Amount',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        field: 'outStandingAmt',
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        width:150,
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.outStandingAmt}}' +
                            '</div>'
                    },
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "ClientName desc"
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

            var init = function () {
                vm.getAll();
                getClientName();

            };
            vm.typeData = function (data) {
                $state.go('outstandingInvoice', { id: data });
            };
            vm.refreshGrid = function (n) {
               
                if (vm.clientId == null || vm.clientId == "") {
                    vm.clientId = 0;
                }
                if (vm.fromDate != null || vm.fromDate != undefined) {
                    vm.requestParams.fromDate = vm.fromDate;
                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.toDate != null || vm.toDate != undefined) {
                    vm.requestParams.toDate = vm.toDate;
                } else {
                    vm.requestParams.toDate = null;
                }

                vm.requestParams = {
                    skipCount: 0,
                    maxResultCount: app.consts.grid.defaultPageSize,
                    sorting: "ClientName desc",
                    clientId: vm.clientId,
                    fromDate: vm.fromDate,
                    toDate: vm.toDate,
                };
                vm.getAll();
            };

            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                });
            }

            vm.search = function (filterText) {
                vm.filterText = filterText;
                vm.getAll();
            };

            vm.clearAll = function () {
                vm.clientId = null;
                vm.fromDate = null;
                vm.toDate = null;
                vm.cname = [];
                vm.refreshGrid();
                getClientName();
            };

            init();
        }
    ]);



})();