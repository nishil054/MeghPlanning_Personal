(function () {
    angular.module('app').controller('app.views.financialyear.index', [
        '$scope', '$uibModal', 'abp.services.app.financialYear', 'uiGridConstants',
        function ($scope, $uibModal, financialYearService, uiGridConstants) {
            var vm = this;

            vm.financialyearList = [];
            //$scope.record = true;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };

            function getFinancialYear() {
                //debugger;
                abp.ui.setBusy();
                financialYearService.getFinancialYearList($.extend({}, vm.requestParams)
                ).then(function (result) {
                    //vm.financialyearList = result.data;
                    vm.userGridOptions.data = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.totalRecord = result.data.totalCount;

                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                    } else { $scope.noData = false; }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            vm.search = function () {
                skipCount = 0;
                currentPage = 0;
                getFinancialYear();
            }

            vm.openFinancialYearCreationModal = function () {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/financialyear/create.cshtml',
                    controller: 'app.views.financialyear.create as vm',
                    backdrop: 'static'
                });


                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    init();
                    //getFinancialYear();
                });
            };

            vm.openFinancialYearEditModal = function (financialyear) {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/financialyear/edit.cshtml',
                    controller: 'app.views.financialyear.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return financialyear.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    getFinancialYear();
                });
            };

            vm.refreshGrid = function (n) {
                skipCount = n;
                getFinancialYear();
            };
            var init = function () {
                vm.requestParams = {
                    skipCount: 0,
                    maxResultCount: app.consts.grid.defaultPageSize,
                    sorting: "Id desc"
                };
                getFinancialYear();
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
                columnDefs: [{
                    name: App.localize('Actions'),
                    enableSorting: false,
                    enableColumnMenu: false,
                    enableScrollbars: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    width: 70,
                    cellTemplate: '<div class=\"ui-grid-cell-contents padd0\">' +
                        '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                        '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                        '    <ul uib-dropdown-menu>' +
                        '      <li><a ng-click="grid.appScope.openFinancialYearEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                        /*'      <li><a ng-click="grid.appScope.openPermissionModal(row.entity)">' + App.localize('Change Permissions') + '</a></li>' +*/
                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },
                {
                    name: App.localize('Duration'),
                    enableColumnMenu: false,
                    headerCellClass: 'leftalign',
                    cellClass: 'leftalign',
                    field: 'startYear',
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        ' <span>{{row.entity.startYear}} To {{row.entity.endYear}}</span>' +
                        '</div>',
                    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                    width: 100,
                },

                {
                    name: 'Title',
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    field: 'title',
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        ' <span>{{row.entity.title}} </span>' +
                        '</div>',
                    width: 150,
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

                        getFinancialYear();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getFinancialYear();
                    });
                },
                data: []
            };
            init();

            vm.delete = function (financialyear) {
                //debugger;
                abp.message.confirm(
                    "Delete Title '" + financialyear.title + "'?",
                    "Delete?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            financialYearService.deleteFinancialYear({ id: financialyear.id })
                                .then(function () {
                                    abp.notify.info("Deleted Title: " + financialyear.title);
                                    getFinancialYear();
                                });
                        }
                    });
            }


            getFinancialYear();
        }
    ]);
})();