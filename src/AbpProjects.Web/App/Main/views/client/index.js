(function () {
    angular.module('app').controller('app.views.client.index', [
        '$scope', '$uibModal', 'abp.services.app.clients', 'uiGridConstants',
        function ($scope, $uibModal, clientsService, uiGridConstants) {
            var vm = this;
            vm.addnewdata = 0;
            vm.holidayList = [];
            vm.clientAdd = [];
            vm.panno = "";
            vm.clientName = "";
            vm.editid = 0;
            vm.id = 0;
            vm.emailFormat = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;


            function getClientlist() {
                abp.ui.setBusy();
                clientsService.getClientsList($.extend({ search: vm.searchBox }, vm.requestParams)).then(function (result) {
                    console.log(result.data.items);
                    vm.userGridOptions.data = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.cname = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                    } else { $scope.noData = false; }
                    vm.totalresult = result.data.totalCount;
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
                        width: 70,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-click="grid.appScope.edit(row.entity)"  ng-if=" (row.entity.createdby==1 || row.entity.isadmin==1 ) ">' + App.localize('Edit') + '</a></li>' +
                            '      <li ng-if="row.entity.isDeleteEnable==false"><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.view(row.entity)"> View </a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'

                    },
                    {
                        name: App.localize('ClientName'),
                        field: 'clientName',
                        enableColumnMenu: false,

                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.clientName}}' +
                            '</div>'
                    }
                    ,

                ],

                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }
                        getClientlist();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getClientlist();
                    });
                },
                data: []
            };
            vm.search = function () {
                skipCount = 0;
                currentPage = 0;
                getClientlist();
            }

            vm.openClientCreation = function () {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/client/addClient.cshtml',
                    controller: 'app.views.client.addClient as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    /*getClientlist();*/
                    init();

                });

                getClientlist();
            }
            vm.openProjectEditModal = function (projectList) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/client/editClient.cshtml',
                    controller: 'app.views.client.editClient as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return projectList.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    getClientlist();
                });
                getClientlist();
            };

            vm.refreshGrid = function (n) {
                skipCount = n;
                getClientlist();
            };
            vm.clear = function () {
                vm.addnewdata = 0;
                vm.editid = 0;
                vm.clientAdd = [];
                vm.clientName = "";
                vm.panno = "";
                vm.searchBox = "";
                init();
            };

            vm.delete = function (holiday) {
                clientsService.checkProject(holiday.id).then(function (result) {

                    if (!result.data) {
                        abp.message.confirm(
                            "Delete Client '" + holiday.clientName + "'?",
                            "Delete?",
                            function (isConfirmed) {
                                if (isConfirmed) {
                                    clientsService.deleteClients(holiday.id)
                                        .then(function () {
                                            abp.notify.success("Deleted Client: " + holiday.clientName);
                                            getClientlist();
                                        });
                                }
                            });
                    }
                    else {
                        abp.notify.error('This client already assigned, so can\'t\ delete.');
                    }
                });
            };

            vm.edit = function (holiday) {
                clientsService.checkProject(holiday.id)
                    .then(function (result) {
                        if (!result.data) {
                            var modalInstance = $uibModal.open({
                                templateUrl: '/App/Main/views/client/editClient.cshtml',
                                controller: 'app.views.client.editClient as vm',
                                backdrop: 'static',
                                resolve: {
                                    id: function () {
                                        return holiday.id;
                                    },
                                    edid: function () {
                                        return 0;
                                    }
                                }
                            });

                            modalInstance.rendered.then(function () {
                                $timeout(function () {
                                    $.AdminBSB.input.activate();
                                }, 0);
                            });

                            modalInstance.result.then(function () {
                                getClientlist();
                            });
                        }
                        else {

                            abp.notify.error('This client already assigned, so can\'t\ edit.');
                        }
                    });
                /*class=\"ui-grid-cell-contents\"*/
            };
            vm.view = function (holiday) {

                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/client/editClient.cshtml',
                    controller: 'app.views.client.editClient as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return holiday.id;
                        },
                        edid: function () {
                            return 4;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    getClientlist();
                });

            };
            vm.editdata = function () {
                if (vm.panno != "") {
                    if (vm.clientName != "") {
                        vm.saving = true;
                        clientsService.editclient(vm.clientAdd, vm.id, vm.clientName, vm.panno).then(function (result) {
                            abp.notify.success(App.localize('SavedSuccessfully'));
                        }).finally(function () {
                            vm.saving = false;
                            vm.addnewdata = 0;
                            vm.editid = 0;
                        });
                    } else { abp.notify.error(App.localize('Please Enter Client Name')); }
                } else { abp.notify.error(App.localize('Please Enter PANNO')); }
            };
            // getClientlist();
            var init = function () {
                vm.requestParams = {
                    skipCount: 0,
                    maxResultCount: app.consts.grid.defaultPageSize,
                    sorting: "Id desc"
                };
                getClientlist()
            };
            init();
        }
    ]);
})();