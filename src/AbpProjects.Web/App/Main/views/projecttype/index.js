(function() {
    angular.module('app').controller('app.views.projecttype.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.projectType', 'uiGridConstants',
        function($scope, $timeout, $uibModal, projectTypeService, uiGridConstants) {
            var vm = this;
            $scope.record = true;
            vm.projecttype = {};
            vm.loading = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };
            vm.getAll = function () {
                abp.ui.setBusy();
                vm.loading = true;
                //debugger;
                projectTypeService.getProjectTypeData($.extend({}, vm.requestParams)).then(function(result) {
                    //  debugger;
                    vm.projecttype = result.data.items;
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
            vm.openProjectTypeCreationModal = function() {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/projecttype/createProjectType.cshtml',
                    controller: 'app.views.projecttype.createProjectType as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function() {
                    vm.getAll();
                });
            };

            function getProjectTypeSearch() {
                //debugger;
                abp.ui.setBusy();
                vm.loading = true;
                projectTypeService.getProjectTypeList($.extend({ projectTypeName: vm.searchBox }, vm.requestParams)).then(function(result) {
                    vm.projecttype = result.data;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                    } else { $scope.noData = false; }
                }).finally(function() {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            vm.refreshGrid = function(n) {
                vm.skipCount = n;
                getProjectTypeSearch();
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
                            '      <li><a ng-click="grid.appScope.openProjectTypeEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('ProjectTypeName'),
                        field: 'projectTypeName',
                        enableColumnMenu: false,
                        width: 250,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectTypeName}}' +
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
            vm.openProjectTypeEditModal = function(projecttype) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/projecttype/editProjectType.cshtml',
                    controller: 'app.views.projecttype.editProjectType as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return projecttype.id;
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
            vm.delete = function(projecttype) {
                abp.message.confirm(
                    "Delete Project Type '" + projecttype.projectTypeName + "'?",
                    "Delete?",
                    function(result) {
                        if (result) {
                            projectTypeService.deleteProjectType({ id: projecttype.id })
                                .then(function() {
                                    abp.notify.success("Project Type Deleted Successfully");
                                    vm.getAll();
                                });
                        }
                    });
            }
            vm.getAll();
        }
    ]);
})();