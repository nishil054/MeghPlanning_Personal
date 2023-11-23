(function () {
    angular.module('app').controller('app.views.nthlevelsubcategories', [
        '$scope', '$state', '$stateParams', '$timeout', '$uibModal', 'abp.services.app.nthCategory', 'uiGridConstants', '$window',
        function ($scope, $state, $stateParams, $timeout, $uibModal, nthCategoryService, uiGridConstants, $window) {
            var vm = this;
            vm.categoryList = [];
            vm.catid = $stateParams.id;
            vm.loading = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                filter: vm.searchBox,
                categoryId: vm.catid
            };
            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                vm.getUsers(vm.catid);
            };

            vm.openAddModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/nthlevelcategory/create.cshtml',
                    controller: 'app.views.nthlevelcategories.create as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return vm.catid;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    vm.getUsers(vm.catid);
                });
            };

            vm.openEditCategoryModal = function (data) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/nthlevelcategory/edit.cshtml',
                    controller: 'app.views.nthlevelcategories.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return data.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    vm.getUsers(vm.catid);
                });
            };


            vm.getUsers = function (catid) {
                if (catid == undefined) {
                    vm.loading = true;
                    if (vm.searchBox != null || vm.searchBox != "" || vm.searchBox != undefined) {
                        vm.requestParams.filter = vm.searchBox;
                    }

                    nthCategoryService.getCategoriesList($.extend({}, vm.requestParams)).then(function (result) {
                        vm.categoryList = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0) {
                            abp.notify.info(app.localize('NoRecordFound'));
                        }
                        else { vm.norecord = false; }

                    }).finally(function () {
                        vm.loading = false;
                    });
                }
                else {
                    if (vm.searchBox != null || vm.searchBox != "" || vm.searchBox != undefined) {
                        vm.requestParams.filter = vm.searchBox;
                    }
                    if (catid > 0) {
                        nthCategoryService.getAllParentByid(catid)
                            .then(function (result) {
                                vm.parentList = result.data.items;
                                vm.selectedType = "";
                                angular.forEach(vm.parentList, function (data, key) {
                                    if (key == 0) {
                                        vm.selectedType = data.name;
                                    }
                                    else {
                                        vm.selectedType = vm.selectedType + " => " + data.name;
                                    }
                                });
                            });
                    }
                    else {
                        vm.selectedType = "";
                        //clear();
                    }
                    nthCategoryService.getSubCategoriesListByParent($.extend({}, vm.requestParams)).then(function (result) {
                        vm.categoryList = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0) {
                            abp.notify.info(app.localize('NoRecordFound'));
                        }
                        else { vm.norecord = false; }

                    }).finally(function () {
                        vm.loading = false;
                    });
                }
            }

            vm.delete = function (data) {
                console.log(data);
                abp.message.confirm(
                    "Do you want to you delete this Category " + data.name + "?",
                    "",
                    function (result) {
                        if (result) {
                            nthCategoryService.delete({ id: data.id })
                                .then(function () {
                                    abp.notify.success("Deleted Category. ");
                                    vm.getUsers(vm.catid);
                                });
                        }
                    });
            };
            vm.refreshGrid = function (n) {

                vm.skipCount = n;
                vm.getUsers(vm.catid);
            };
            vm.openSubCategory = function (catdata) {
                //alert(catdata.id);
                $state.go('subcategory', { id: catdata.id });
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
                        name: App.localize('Actions'),
                        enableSorting: false,
                        enableColumnMenu: false,
                        enableScrollbars: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 80,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li ng-click="grid.appScope.openEditCategoryModal(row.entity)"><a> Edit </a></li>' +
                            '<li ng-click="grid.appScope.delete(row.entity)"><a> Delete </a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'

                    },
                    {
                        name: "Name",
                        enableColumnMenu: false,
                        field: 'name',
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        height: 180,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' + ' <a class="gridlink" ng-click="grid.appScope.openSubCategory(row.entity)" style="text-align:center"> {{row.entity.name}}</a>'
                            + '</div>'
                    },
                    {
                        name: "Sort Order",
                        enableColumnMenu: false,
                        field: 'sortOrder',
                        headerCellClass: 'leftalign',
                        cellClass: 'centeralign',
                        height: 180,
                        width: 110
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

                        vm.getUsers(vm.catid);
                    });

                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getUsers(vm.catid);
                    });
                },
                data: []
            };

            vm.cancel = function () {
                $window.history.back();
            };

            init = function () {
                vm.getUsers(vm.catid);
            }
            init();
        }
    ]);
})();