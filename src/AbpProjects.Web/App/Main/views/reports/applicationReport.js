(function () {
    angular.module('app').controller('app.views.reports.applicationReport', [
        '$scope', '$timeout', '$uibModal', '$http', 'abp.services.app.leaveApplication', 'uiGridConstants',  'abp.services.app.masterList',
        function ($scope, $timeout, $uibModal, $http, leaveApplicationService, uiGridConstants, masterListservice) {
            var vm = this;
            $scope.record = false;
            vm.loading = false;
            vm.leaveapplication = {};
            vm.leaveapplicationlist = [];
            vm.showfilter = false;
            var date = new Date();
            vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            vm.toDate = moment(vm.lastDay);
            vm.fromDate = moment(vm.firstDay);

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                userId: vm.leaveapplication.userId,
                fromDate: vm.fromDate,
                toDate: vm.toDate,
                leaveStatusId: vm.leaveapplication.leaveStatusId,
            };

            if (abp.auth.hasPermission('Pages.Reports.LeaveApplicationReport')) {
                vm.showfilter = true;
            }

            vm.clearAll = function () {
                vm.userlist = [];
                vm.leavestatuslist = [];
                getUsers();
                getLeaveStatusData();
                var date = new Date();
                vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                vm.fromDate = moment(vm.firstDay);
                vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                vm.toDate = moment(vm.lastDay);
                vm.leaveapplication.fromDate = vm.fromDate;
                vm.leaveapplication.toDate = vm.toDate;
                vm.leaveapplication.leaveStatusId = null;
                vm.leaveapplication.userId = null;
                $("#ddlemp").select2("val", null);
                $("#ddlleavestatus").select2("val", null);
                vm.requestParams.fromDate = null;
                vm.requestParams.toDate = null;
                vm.searchAll();
            };

            function getUsers() {
                masterListservice.getUser().then(function (result) {
                    vm.userlist = result.data;
                });
            }

            vm.searchAll = function (n) {
                //if (vm.searchBox != null) {
                //    vm.requestParams.filter = vm.searchBox;
                //}
                //if (vm.leaveapplication.fromDate != null) {
                //    vm.requestParams.fromDate = vm.leaveapplication.fromDate;
                //}
                //else {
                //    vm.requestParams.fromDate = null;
                //}
                //if (vm.leaveapplication.toDate != null) {
                //    vm.requestParams.toDate = vm.leaveapplication.toDate;
                //} else {
                //    vm.requestParams.toDate = null;
                //}
                //if (vm.leaveapplication.leaveStatusId != "") {
                //    vm.requestParams.leaveStatusId = vm.leaveapplication.leaveStatusId;

                //} else {
                //    vm.requestParams.leaveStatusId = "";
                //}

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
                //vm.leaveapplication.fromDate = vm.fromDate;
                //vm.leaveapplication.toDate = vm.toDate;
                if (vm.fromDate != null) {
                    vm.requestParams.fromDate = vm.fromDate;
                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.toDate != null) {
                    vm.requestParams.toDate = vm.toDate;
                } else {
                    vm.requestParams.toDate = null;
                }
                if (vm.leaveapplication.leaveStatusId != "") {
                    vm.requestParams.leaveStatusId = vm.leaveapplication.leaveStatusId;

                } else {
                    vm.requestParams.leaveStatusId = "";
                }
                if (vm.leaveapplication.userId != "") {
                    vm.requestParams.userId = vm.leaveapplication.userId;
                }
                else {
                    vm.requestParams.userId = "";
                }
                abp.ui.setBusy();
                leaveApplicationService.getLeaveDataReport($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.leaveapplicationlist = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                            $scope.noData = true; vm.isChecked = true;
                        } else {
                            $scope.noData = false; vm.isChecked = false;
                        }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }
            vm.openLeaveApplicationCreationModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/leaveApplication/createLeaveApp.cshtml',
                    controller: 'app.views.leaveApplication.createLeaveApp as vm',
                    backdrop: 'static',
                });
                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });
                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };
          
            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.exportExcel = function () {

                if (vm.requestParams.leaveStatusId == null || vm.requestParams.leaveStatusId == "") {
                    vm.leaveStatusId = null;
                }

                if (vm.requestParams.fromDate != null || vm.requestParams.fromDate != undefined) {
                    vm.fromDate = vm.requestParams.fromDate._d;
                    //vm.fromDate = null;
                } else {
                    vm.fromDate = null;
                }
                if (vm.requestParams.toDate != null || vm.requestParams.toDate != undefined) {
                    vm.toDate = vm.leaveapplication.toDate._d;
                    //vm.toDate = null;
                } else {
                    vm.toDate = null;
                }
                if (vm.requestParams.userId == null || vm.requestParams.userId == "") {
                    vm.userId = vm.requestParams.filter;
                }
                //} else {
                //    vm.filter = '';
                //}

                
        
                var url = "../exportToExcel/GetLeaveDataReportExport";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        leaveStatusId: vm.leaveStatusId,
                        fromDate: vm.fromDate,
                        toDate: vm.toDate,
                        userId: vm.userId,
                        

                        //leaveStatusId:1,
                        //fromDate:null,
                        //toDate: null,
                        //filter: 'ch',

                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

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
                columnDefs: [
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
            
          

            vm.viewRequest = function (leave) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/leaveapplication/details.cshtml',
                    controller: 'app.views.leaveapplication.details as vm',
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
                getUsers();
                vm.getAll();
                getLeaveStatusData();
            }
            init();
        }

    ]);
})();