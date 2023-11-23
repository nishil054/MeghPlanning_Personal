(function () {
    angular.module('app').controller('app.views.outstandingInvoice', [
        '$scope', '$state', '$uibModal', '$http', '$stateParams', 'abp.services.app.reports', 'uiGridConstants', 'abp.services.app.support',
        function ($scope, $state, $uibModal, $http, $stateParams, reportsService, uiGridConstants, supportService) {
            /*ImportFTPDetailsService*/
            //debugger;
            var vm = this;
            vm.norecord = false;
            vm.clientId = 0;
            vm.cname = [];
            vm.id = $stateParams.id;

            $("#btnBack").hide();

            if (vm.id != "") {
                vm.clientId = vm.id;
                $("#btnBack").show();
            }

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
                reportsService.getOutStandingInvoice(vm.requestParams).then(function (result) {
                    console.log(result.data.items);
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

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.exportExcel = function () {
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
                var url = "../exportToExcel/ExportApplicationReportToExcel";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        clientId: vm.clientId,
                        fromDate: vm.fromDate,
                        toDate: vm.toDate,
                        //type: 1,
                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });

            }

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                showColumnFooter: true,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: 'Invoice No',
                        field: 'InvoiceNo',
                        enableColumnMenu: false,
                        width: 170,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.invoiceNo}}' +
                            '</div>'
                    },
                   
                    {
                        name: App.localize('Invoice Date'),
                        field: 'billDate',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 120,
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        //cellTemplate:
                        //'<div class=\"ui-grid-cell-contents\">' +
                        //'{{row.entity.date}}' +
                        //'</div>'

                    },
                    {
                        name: App.localize('ClientName'),
                        displayName: 'Client Name',
                        field: 'ClientName',
                        enableColumnMenu: false,
                        

                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.clientName}}' +
                            '</div>',
                    },
                    {
                        name: App.localize('Total Amount'),
                        displayName: 'Total Amount',
                        field: 'totalBillAmt',
                        cellClass: 'rightalign',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        width:150,
                        headerCellClass: 'rightalign',
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.totalBillAmt}}' +
                            '</div>',
                    },
                    {
                        name: App.localize('Total Collection'),
                        displayName: 'Total Collection',
                        /* field: 'DatabaseDetails',*/
                        field: 'totalCollection',
                        headerCellClass: 'rightalign',
                        cellClass: 'rightalign',
                        enableColumnMenu: false,
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        width:150,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.totalCollection}}' +
                            '</div>',
                    },
                    {
                        name: App.localize('OutStanding Amount'),
                        displayName: 'OutStanding Amount',
                        field: 'outStandingAmt',
                        enableColumnMenu: false,
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        width:150,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign ',
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
            vm.back = function (n) {
                $state.go('outstandingClient');
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
                    console.log(result.data);
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