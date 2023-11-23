(function () {
    angular.module('app').controller('app.views.users.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.user', 'abp.services.app.email', 'uiGridConstants', 'abp.services.app.masterList',
        function ($scope, $timeout, $uibModal, userService, emailservice, uiGridConstants, masterListservice) {
            var vm = this;
            vm.data = {};
            vm.users = [];
            vm.teamList = [];
            vm.comapnyList = [];
            vm.loading = false;
            vm.itemsPerPage = 10;
            vm.skipCount = 0;
            vm.requestParams = {
                skipCount: vm.skipCount,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "name"
            };
            vm.data.activeStatus = "";

            function getTeam() {
                userService.getTeam()
                    .then(function (result) {
                        vm.teamList = result.data;
                    });
            }

            function getCompanyData() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.comapnyList = result.data;
                    });
            }

            function getRoles() {
                masterListservice.getRoles()
                    .then(function (result) {
                        vm.roleList = result.data;
                    });
            }

            vm.getUsers = function () {
                vm.loading = true;
                abp.ui.setBusy();
                userService.getUserList($.extend({}, vm.requestParams)).then(function (result) {
                    vm.users = result.data.items;
                    //angular.forEach(values, function (value, key) {
                    //    console.log(key + ': ' + value);
                    //});

                    // { { grid.appScope.bindrolename(row.entity.date) } }

                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        //vm.norecord = true;
                        abp.notify.error(app.localize('NoRecordFound'));
                    } else { vm.norecord = false; }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            vm.bindrolename = function (rolenamelist) {
                var rolenamevalue = "";
                angular.forEach(rolenamelist, function (v1, k1) {
                    rolenamevalue += v1 + ",";
                    //this is nested angular.forEach loop
                });
                return rolenamevalue.substring(0, rolenamevalue.length - 1);
            }

            vm.openUserCreationModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/users/createModal.cshtml',
                    controller: 'app.views.users.createModal as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    vm.getUsers();
                });
            };

            vm.openUserEditModal = function (user) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/users/editModal.cshtml',
                    controller: 'app.views.users.editModal as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return user.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getUsers();
                });
            };
            vm.openPermissionModal = function (user) {

                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/users/permissionsUserModal.cshtml',
                    controller: 'app.views.users.permissionsUserModal as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return user.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    getUsers();
                });
            };
            vm.openUserChangePasswordModal = function (user) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/users/adminChangePassword.cshtml',
                    controller: 'app.views.users.adminChangePassword as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return user.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getUsers();
                });
            };


            vm.openUserTerminateEditModal = function (user) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/users/editTerminate.cshtml',
                    controller: 'app.views.users.editTerminate as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return user.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getUsers();
                });
            };

            vm.delete = function (user) {
                abp.message.confirm(
                    "Delete user '" + user.userName + "'?", "Delete",
                    function (result) {
                        if (result) {
                            userService.delete({ id: user.id })
                                .then(function () {
                                    abp.notify.info("Deleted user: " + user.userName);
                                    vm.getUsers();
                                });
                        }
                    });
            }



            function getUsersSearch() {
                vm.loading = true;
                abp.ui.setBusy();
                if (vm.data.roleId == "" || vm.data.roleId == undefined) {
                    vm.requestParams.roleId = null;
                } else {
                    vm.requestParams.roleId = vm.data.roleId;
                }
                if (vm.data.teamId == "" || vm.data.teamId == undefined) {
                    vm.requestParams.teamId = null;
                } else {
                    vm.requestParams.teamId = vm.data.teamId;
                }
                if (vm.data.companyId == "" || vm.data.companyId == undefined) {
                    vm.requestParams.companyId = null;
                } else {
                    vm.requestParams.companyId = vm.data.companyId;
                }

                if (vm.data.activeStatus == "" || vm.data.activeStatus == undefined) {
                    vm.requestParams.activeStatus = null;
                } else {
                    vm.requestParams.activeStatus = vm.data.activeStatus;
                }
                userService.getUserList($.extend({ name: vm.searchBox }, vm.requestParams)).then(function (result) {
                    vm.users = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                       
                    } else {
                        vm.norecord = false;
                        $scope.noData = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                //vm.getUsers();
                getUsersSearch();
            };
            vm.clear = function () {
                vm.searchBox = null;
                vm.data.activeStatus = "";
                vm.roleList = [];
                vm.comapnyList = [];
                vm.teamList = [];
                vm.data.roleId = null;
                vm.data.teamId = null;
                vm.data.companyId = null;
                vm.getUsers();
                getUsersSearch();
                getRoles();
                getTeam();
                getCompanyData();
                
            };

            vm.openDeActiveModal = function (user) {
                abp.message.confirm(
                    "Are you sure you want to lock '" + user.userName + "'?", " ",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            userService.getProjectServiceCount({ id: user.id })
                                .then(function (result) {
                                    if (result.data.length > 0) {

                                        var modalInstance = $uibModal.open({
                                            templateUrl: '/App/Main/views/users/lockuser.cshtml',
                                            controller: 'app.views.users.lockuser as vm',
                                            backdrop: 'static',
                                            resolve: {
                                                id: function () {
                                                    return user.id;
                                                }
                                            }
                                        });

                                        modalInstance.rendered.then(function () {
                                            $timeout(function () {
                                                $.AdminBSB.input.activate();
                                            }, 0);
                                        });

                                        modalInstance.result.then(function () {
                                            vm.getUsers();
                                        });
                                    }
                                    else {
                                        userService.deactiveUser({ id: user.id })
                                            .then(function (result) {
                                                if (result.data == true) {
                                                    abp.notify.success("User lock successfully");
                                                }
                                                vm.getUsers();
                                            });
                                    }
                                }
                                )
                        }

                    });
            };

            vm.openActiveModal = function (user) {
                abp.message.confirm(
                    "Are you sure you want to unlock '" + user.userName + "'?", " ",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            userService.activeUser({ id: user.id })
                                .then(function (result) {
                                    if (result.data == true) {
                                        abp.notify.success("User unlock successfully");
                                    }
                                    vm.getUsers();
                                });
                        }
                    });
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
                        '      <li><a ng-click="grid.appScope.openUserEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.openUserTerminateEditModal(row.entity)" ng-if="row.entity.isActive == true">' + App.localize('Mark As Terminate') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                        '<li><a ng-click="grid.appScope.openUserChangePasswordModal(row.entity)">Reset Password</a></li>' +
                        '      <li><a ng-click="grid.appScope.openPermissionModal(row.entity)">' + App.localize('Change Permissions') + '</a></li>' +
                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },
                {
                    name: App.localize('UserName'),
                    field: 'userName',
                    enableColumnMenu: false,
                    width: 110
                },
                {
                    name: App.localize('Name'),
                    enableColumnMenu: false,
                    field: 'name',
                    width: 170,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.name}} {{row.entity.surname}}' +
                        '</div>'
                },
                {
                    name: App.localize('RoleName'),
                    enableColumnMenu: false,
                    field: 'name',
                    width: 180,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{ grid.appScope.bindrolename(row.entity.role_Name) }}' +
                        '</div>'
                },
                {
                    name: App.localize('EmailAddress'),
                    enableColumnMenu: false,
                    field: 'emailAddress',
                    headerCellClass: 'emailaddressbox',
                    cellClass: 'emailaddressbox',
                    //   width: 230,
                },
                {
                    name: App.localize('Active'),
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    field: 'isActive',
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        ' <a ng-click="grid.appScope.openDeActiveModal(row.entity)" <span ng-show="row.entity.isActive" class="label label-success">' + App.localize('Yes') + '</span></a>' +
                        ' <a ng-click="grid.appScope.openActiveModal(row.entity)" <span ng-show="!row.entity.isActive" class="label label-default">' + App.localize('No') + '</span></a>' +
                        '</div>',
                    width: 80,
                    cellClass: 'centeralign'
                },
                //{
                //    name: App.localize('CreationTime'),
                //    enableColumnMenu: false,
                //    cellClass: 'centeralign',
                //    headerCellClass: 'centeralign',
                //    field: 'creationTime',
                //    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                //    width: 125
                //},
                {
                    name: "Next Renewal Date",
                    enableColumnMenu: false,
                    cellClass: 'centeralign',
                    headerCellClass: 'centeralign',
                    field: 'next_Renewaldate',
                    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                    width: 150
                },
                {
                    name: "Salary",
                    enableColumnMenu: false,
                    cellClass: 'rightalign',
                    headerCellClass: 'rightalign',
                    field: 'salary_Month',
                    width: 100,
                },
                {
                    name: "Salary/Hour",
                    enableColumnMenu: false,
                    cellClass: 'rightalign',
                    headerCellClass: 'rightalign',
                    field: 'salary_Hour',
                    width: 100,
                }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getUsers();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getUsers();
                    });
                },
                data: []
            };


            var init = function () {
                getRoles();
                getTeam();
                getCompanyData();
                vm.getUsers();
                /* emailservice.sendmail();*/
            }
            init();
        }
    ]);
})();