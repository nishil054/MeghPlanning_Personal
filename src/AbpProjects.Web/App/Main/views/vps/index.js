(function() {
    angular.module('app').controller('app.views.vps.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.vPS', 'uiGridConstants',
        function($scope, $timeout, $uibModal, vPSService, uiGridConstants) {
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
                abp.ui.setBusy();
                vm.loading = true;
                // debugger;
                vPSService.getVPSData($.extend({}, vm.requestParams)).then(function(result) {
                    //  debugger;
                    vm.vps = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                        $scope.noData = true;
                    } else { $scope.noData = false; }
                }).finally(function() {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }
            vm.openVPSCreationModal = function() {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/vps/createVPS.cshtml',
                    controller: 'app.views.vps.createVPS as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function() {
                    vm.getAll();
                });
            };

            function getVPSSearch() {
                vm.loading = true;
                abp.ui.setBusy();
                vPSService.getVPSList($.extend({ title: vm.searchBox }, vm.requestParams)).then(function(result) {
                    vm.vps = result.data;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                    } else { $scope.noData = false; }
                }).finally(function() {
                    abp.ui.clearBusy();
                    vm.loading = false;
                });
            }
            vm.refreshGrid = function(n) {
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
                            '      <li><a ng-click="grid.appScope.openVPSEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('Title'),
                        field: 'title',
                        enableColumnMenu: false,
                        //width: 150,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.title}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('IP'),
                        displayName: 'IP',
                        field: 'ip',
                        enableColumnMenu: false,
                        width:430,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.ip}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('UserName'),
                        field: 'userName',
                        enableColumnMenu: false,
                        width:280
                    },
                    {
                        name: App.localize('Password'),
                        field: 'password',
                        enableColumnMenu: false,
                        width:230
                    },
                    //{
                    //    name: App.localize('Comment'),
                    //    field: 'comment',
                    //    enableColumnMenu: false,
                    //    width: 150
                    //},

                ],
                onRegisterApi: function(gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function(grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getAll();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function(pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getAll();
                    });
                },
                data: []
            };
            vm.openVPSEditModal = function(vps) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/vps/editVPS.cshtml',
                    controller: 'app.views.vps.editVPS as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return vps.id;
                        }
                    }
                });

                modalInstance.rendered.then(function() {
                    $timeout(function() {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function() {
                    vm.getAll();
                });
            };
            vm.delete = function(vps) {
                abp.message.confirm(
                    "Delete VPS '" + vps.title + "'?",
                    "Delete?",
                    function(result) {
                        if (result) {
                            vPSService.deleteVPS({ id: vps.id })
                                .then(function() {
                                    abp.notify.success("VPS Deleted Successfully");
                                    vm.getAll();
                                });
                        }
                    });
            }
            vm.getAll();
        }
    ]);
})();