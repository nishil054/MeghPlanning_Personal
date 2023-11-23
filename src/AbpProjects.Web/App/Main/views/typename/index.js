(function() {
    angular.module('app').controller('app.views.typename.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.typename', 'uiGridConstants',
        function($scope, $timeout, $uibModal, typeNameService, uiGridConstants) {
            var vm = this;
            $scope.record = true;
            vm.typename = {};
            vm.loading = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                filterText: vm.searchBox,
            };
            vm.getAll = function () {
                abp.ui.setBusy();
                if (vm.searchBox != null) {
                    vm.requestParams.filterText = vm.searchBox;

                }
                vm.loading = true;
                //debugger;
                typeNameService.getTypenameList($.extend({}, vm.requestParams)).then(function(result) {
                    //  debugger;
                    vm.typename = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                    } else {
                        $scope.noData = false;
                    }
                }).finally(function() {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }
            vm.openTypenameCreationModal = function() {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/typename/createTypeName.cshtml',
                    controller: 'app.views.typename.createTypeName as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function() {
                    vm.getAll();
                });
            };

            vm.refreshGrid = function(searchName) {
                vm.searchBox = searchName;
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
                            '      <li><a ng-click="grid.appScope.openTypenameEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('TypeName'),
                        field: 'name',
                        enableColumnMenu: false,
                        width: 250,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.name}}' +
                            '</div>'
                    },

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
            vm.openTypenameEditModal = function(typename) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/typename/editTypeName.cshtml',
                    controller: 'app.views.typename.editTypeName as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return typename.id;
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
            vm.delete = function(typename) {
                abp.message.confirm(
                    "Delete  '" + typename.name + "'?",
                    "Delete?",
                    function(result) {
                        if (result) {
                            typeNameService.deleteTypename({ id: typename.id })
                                .then(function() {
                                    abp.notify.success(App.localize('TypeNameDeletedSuccessfully'));
                                    vm.getAll();
                                });
                        }
                    });
            }
            vm.getAll();
        }
    ]);
})();