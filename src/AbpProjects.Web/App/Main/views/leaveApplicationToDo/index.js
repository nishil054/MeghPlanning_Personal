(function () {
    angular.module('app').controller('app.views.leaveapplicationtodo.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.leaveApplication', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, leaveApplicationService, uiGridConstants) {
            var vm = this;
            $scope.record = false;
            vm.loading = false;
            vm.leaveapplicationtodo = {};
            vm.leaveapplicationlist = [];
            vm.showfilter = false;

            var date = new Date();
            vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            vm.leaveapplicationtodo.fromDate = moment(vm.firstDay);
            vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            vm.leaveapplicationtodo.toDate = moment(vm.lastDay);

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                filter: vm.searchBox,
                fromDate: vm.leaveapplicationtodo.fromDate,
                toDate: vm.leaveapplicationtodo.toDate,
                leaveStatusId: vm.leaveapplicationtodo.leaveStatusId,
            };

            if (abp.auth.hasPermission('Pages.Admin.LeaveApplicationToDo')) {
                vm.showfilter = true;
            }

            vm.clearAll = function () {
                vm.searchBox = null;
                vm.requestParams.filter = null;
                vm.leaveapplicationtodo.fromDate = null;
                vm.leaveapplicationtodo.toDate = null;
                vm.leaveapplicationtodo.leaveStatusId = null;
                $("#ddlleavestatus").select2("val", null);
                vm.requestParams.fromDate = null;
                vm.requestParams.toDate = null;
                vm.leaveapplicationtodo.fromDate = moment(vm.firstDay);
                vm.leaveapplicationtodo.toDate = moment(vm.lastDay);
                vm.searchAll();
            };

            vm.searchAll = function (n) {
                if (vm.searchBox != null) {
                    vm.requestParams.filter = vm.searchBox;
                }
                if (vm.leaveapplicationtodo.fromDate != null) {
                    vm.requestParams.fromDate = vm.leaveapplicationtodo.fromDate;
                }
                else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.leaveapplicationtodo.toDate != null) {
                    vm.requestParams.toDate = vm.leaveapplicationtodo.toDate;
                } else {
                    vm.requestParams.toDate = null;
                }
                if (vm.leaveapplicationtodo.leaveStatusId != "") {
                    vm.requestParams.leaveStatusId = vm.leaveapplicationtodo.leaveStatusId;

                } else {
                    vm.requestParams.leaveStatusId = "";
                }

                getLeaveStatusData();
                vm.getAll();
            };

            function getLeaveStatusData() {
                leaveApplicationService.getLeaveStatus().then(function (result) {
                    vm.leavestatuslist = result.data;
                });
            }

            vm.getAll = function () {
                vm.loading = true;
                abp.ui.setBusy();
                leaveApplicationService.getLeaveToDoData($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.leaveapplicationlist = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                            $scope.noData = true;
                        } else {
                            $scope.noData = false;
                        }
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
                        '      <li><a ng-click="grid.appScope.viewRequest(row.entity)">' + App.localize('View') + '</a></li>' +
                        //'      <li><a ng-click="grid.appScope.cancelRequest(row.entity)" ng-if="(row.entity.leaveStatus==2)">' + App.localize('Cancel Request') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.approveRequest(row.entity)" ng-if="(row.entity.leaveStatus==1) || (row.entity.leaveStatus==4)">' + App.localize('Approve') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.rejectRequest(row.entity)" ng-if="(row.entity.leaveStatus==1) || (row.entity.leaveStatus==4)">' + App.localize('Reject') + '</a></li>' +

                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },
                {
                    name: App.localize('Name'),
                    field: 'Name',
                    enableColumnMenu: false,
                    width: 250,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.userName}}' +
                        '</div>'
                },
                {
                    name: App.localize('LeaveType'),
                    field: 'leaveTypeName',
                    enableColumnMenu: false,
                    width: 200,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.leaveTypeName}}' +
                        '</div>'
                },
                {
                    name: App.localize('FromDate'),
                    field: 'fromDate',
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    width: 120,
                    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                },
                {
                    name: App.localize('ToDate'),
                    field: 'toDate',
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    width: 120,
                    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                },
                {
                    name: App.localize('Reason'),
                    field: 'reason',
                    enableColumnMenu: false,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.reason}}' +
                        '</div>'
                },
                {
                    name: App.localize('LeaveStatus'),
                    field: 'leaveStatusName',
                    enableColumnMenu: false,
                    width: 200,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.leaveStatusName}}' +
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

            vm.approveRequest = function (leave) {
                abp.message.confirm(
                    "Are you sure want to approve the leave application ?", " ",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            leaveApplicationService.approveLeaveRequest({ id: leave.id })
                                .then(function (result) {
                                    abp.notify.success("Leave Approved Successfully");
                                    vm.getAll();
                                });
                        }
                    });
            }

            vm.rejectRequest = function (leave) {
                abp.message.confirm(
                    "Are you sure want to reject the leave application ?", " ",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            leaveApplicationService.rejectLeaveRequest({ id: leave.id })
                                .then(function (result) {
                                    abp.notify.success("Leave Rejected Successfully");
                                    vm.getAll();
                                });
                        }
                    });
            }

            vm.viewRequest = function (leave) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/leaveApplicationToDo/details.cshtml',
                    controller: 'app.views.leaveapplicationtodo.details as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return leave.id;
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

            init = function () {
                vm.getAll();
                getLeaveStatusData();
            }
            init();
        }

    ]);
})();