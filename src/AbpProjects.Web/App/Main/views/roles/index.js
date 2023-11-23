(function() {
    angular.module('app').controller('app.views.roles.index', [
        '$scope', '$uibModal', 'abp.services.app.role', 'uiGridConstants',
        function($scope, $uibModal, roleService, uiGridConstants) {
            //debugger;
            var vm = this;

            vm.roles = [];
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };

            function getRoles() {
                //debugger;
                abp.ui.setBusy();
                roleService.getAllRoles($.extend({}, vm.requestParams))
                    .then(function (result) {
                    //debugger;
                    vm.roles = result.data.items;
                    vm.totalRecord = result.data.totalCount;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    }).finally(function ()
                    {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }
            vm.openRoleCreationModal = function() {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/roles/createModal.cshtml',
                    controller: 'app.views.roles.createModal as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function() {
                    getRoles();
                });
            };

            vm.openRoleEditModal = function(role) {

                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/roles/editModal.cshtml',
                    controller: 'app.views.roles.editModal as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return role.id;
                        }
                    }
                });

                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function() {
                    getRoles();
                });
            };

            vm.openPermissionModal = function(role) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/roles/permissionsModal.cshtml',
                    controller: 'app.views.roles.permissionsroleModal as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return role.id;
                        }
                    }
                });
            }

            vm.refreshGrid = function(n) {
                skipCount = n;
                getRoles();
            };
            var init = function() {

                getRoles()
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
                            '      <li><a ng-click="grid.appScope.openRoleEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.openPermissionModal(row.entity)">' + App.localize('Permissions') + '</a></li>' +

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('RoleName'),
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',    
                        cellClass: 'leftalign',
                        field: 'name',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.name}}</span>' +
                            '</div>',
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        width: 250,
                    },
                    //{
                    //    name: App.localize('EndDate'),
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    field: 'endDate',
                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        ' <span>{{row.entity.endDate |date:"dd/MM/yy"}} </span>' +
                    //        '</div>',
                    //    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                    //    //width: 180,
                    //},

                    {
                        name: 'DisplayName',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'displayName',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.displayName}} </span>' +
                            '</div>',
                        //width: 150,
                    }
                    //{
                    //    name: 'Title',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'leftalign',
                    //    cellClass: 'leftalign',
                    //    field: 'title',
                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        ' <span>{{row.entity.title}} </span>' +
                    //        '</div>',
                    //    //width: 180,
                    //}
                    //{
                    //    name: App.localize('Title'),
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'rightalign',
                    //    field: 'title',
                    //    cellTemplate: ' <a class="gridlink" ng-click="grid.appScope.downloadPDF(row.entity.documentUpload)" tooltip-placement="top" uib-tooltip="Download" style="text-align:center"> {{row.entity.title}}  <img src="/images/icon_download.svg" class="btndownloadicon"/></a>',
                    //    //'<div class=\"ui-grid-cell-contents\">' +
                    //    //' <span>{{row.entity.docTitle}} </span>' +
                    //    //'</div>',
                    //    //width: 120,
                    //}

                    //{
                    //    name: 'Document Upload',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    field: 'documentUpload',
                    //    cellTemplate: ' <a class="gridlink" ng-click="grid.appScope.downloadPDF(row.entity.documentUpload)" tooltip-placement="top" uib-tooltip="Download" style="text-align:center"> {{row.entity.documentUpload}}  <img src="/images/icon_download.svg" class="btndownloadicon"/></a>',
                    //    //'<div class=\"ui-grid-cell-contents\">' +
                    //    //' <span>{{row.entity.documentUpload}} </span>' +
                    //    //'</div>',
                    //    //width: 180,
                    //    cellClass: 'centeralign'
                    //}

                    //{
                    //    name: App.localize('DocumentTitle'),
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'rightalign',
                    //    field: 'docTitle',
                    //    cellTemplate: 
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        ' <span>{{row.entity.docTitle}} </span>' +
                    //        '</div>',
                    //    //width: 120,
                    //}

                ],
                onRegisterApi: function(gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function(grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        getRoles();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function(pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getRoles();
                    });
                },
                data: []
            };
            init();

            vm.delete = function(role) {
                //debugger;
                abp.message.confirm(
                    "Delete role '" + role.name + "'?", "",
                    function(isConfirmed) {
                        if (isConfirmed) {
                            roleService.delete({ id: role.id })
                                .then(function() {
                                    abp.notify.info("Deleted role: " + role.name);
                                    getRoles();
                                });
                        }
                    });
            }
            getRoles();
        }
    ]);
})();