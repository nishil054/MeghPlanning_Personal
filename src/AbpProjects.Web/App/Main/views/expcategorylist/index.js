(function () {
    angular.module('app').controller('app.views.expcategorylist.index', [
        '$scope', '$state', '$timeout', '$uibModal', 'abp.services.app.expcategory', 'uiGridConstants',
        function ($scope, $state, $timeout, $uibModal, expcategoryService, uiGridConstants) {

            var vm = this;
            vm.norecord = false;
            vm.task = {};
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                CategoryFilter: vm.task.category
            };
            vm.getAll = function () {
                abp.ui.setBusy();
                if (vm.task.category != null || vm.task.category != undefined || vm.task.category != "") {
                    vm.requestParams.CategoryFilter = vm.task.category;
                }
                else {
                    vm.requestParams.CategoryFilter = null;
                }
                vm.loading = true;
                expcategoryService.getExpenseCategory($.extend({}, vm.requestParams)).then(function (result) {
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        vm.norecord = true;
                    } else {
                        vm.norecord = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }
            vm.search = function (filterText) {
                vm.task.category = filterText;
                vm.getAll();

            }
            $scope.clearSearch = function () {
                vm.task.category = null;
                vm.getAll();

            }
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
                        '      <li><a ng-click="grid.appScope.Edit(row.entity)">' + App.localize('Edit') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.opendetailModal(row.entity)">' + App.localize('Details') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.Delete(row.entity)">' + App.localize('Delete') + '</a></li>' +

                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },
                {
                    name: App.localize('Category'),
                    field: 'Category',
                    enableColumnMenu: false,
                    width: 250,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                        '{{row.entity.category}}' +
                        '</div>'
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

            var init = function () {
                vm.requestParams = {
                    skipCount: 0,
                    maxResultCount: app.consts.grid.defaultPageSize,
                    sorting: "Id desc",
                    CategoryFilter: vm.task.category
                };
                vm.getAll();
            };

           
            vm.opendetailModal = function (history) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/expcategorylist/detailpage.cshtml',
                    controller: 'app.views.expcategorylist.detailpage as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return history.id;

                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {


                });
            }

            vm.Edit = function (category) {

                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/expcategorylist/edit.cshtml',
                    controller: 'app.views.expcategorylist.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return category.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };



            vm.Delete = function (item) {
                abp.message.confirm(
                    "Delete Category '" + item.category + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {

                            expcategoryService.deleteExpenseCategory({ id: item.id })
                                .then(function () {
                                    abp.notify.info("Deleted Category : " + item.category);
                                    vm.getAll();
                                });
                        }
                    });
            };





            vm.openCreateModal = function () {

                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/expcategorylist/insert.cshtml',
                    controller: 'app.views.expcategorylist.insert as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    init();
                    //vm.getAll();
                });
            };



            init();
        }
    ]);



})();