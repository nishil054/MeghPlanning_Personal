(function() {
    angular.module('app').controller('app.views.timesheet.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.timeSheet', 'uiGridConstants',
        function($scope, $timeout, $uibModal, timeSheetService, uiGridConstants) {
            var vm = this;
            $scope.record = false;
            vm.timesheetlist = [];
            vm.timesheet = {};
            vm.roleName = "";

            var date = new Date();
            vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            vm.timesheet.fromDate = moment(vm.firstDay);
            vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            vm.timesheet.toDate = moment(vm.lastDay);

            vm.loading = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Date desc",
                filter: vm.searchBox,
                fromDate: vm.timesheet.fromDate,
                toDate: vm.timesheet.toDate,
                userId: vm.timesheet.userId,
            };
           
            function getUsers() {
                timeSheetService.getUser().then(function(result) {
                    vm.userlist = result.data;
                    try {
                        vm.roleName = result.data[0].roleName;
                    } catch (e) {

                    }
                });
            }
            vm.getAll = function() {
                vm.loading = true;
                abp.ui.setBusy();
                timeSheetService.getTimeSheetData($.extend({}, vm.requestParams))
                    .then(function(result) {
                        vm.timesheetlist = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                            //$scope.record = false;
                            $scope.noData = true;
                            //abp.notify.info(app.localize('NoRecordFound'));
                        } else {
                            $scope.noData = false;
                        }
                    }).finally(function() {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });


            }
            vm.openTimeSheetCreationModal = function() {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/timesheet/createTimeSheet.cshtml',
                    controller: 'app.views.timesheet.createTimeSheet as vm',
                    backdrop: 'static',
                    resolve: {
                        date: null
                    }
                });
                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });
                modalInstance.result.then(function() {
                    vm.getAll();
                    // getUsers();
                });
            };
            vm.clearAll = function() {
                vm.timesheet.fromDate = moment(vm.firstDay);
                vm.timesheet.toDate = moment(vm.lastDay);
                
                getUsers();
                vm.searchBox = "";
                vm.userlist = [];
                vm.timesheet.userId = null;
                //vm.requestParams.userId = null;
                vm.requestParams.fromDate = null;
                vm.requestParams.toDate = null;
                //vm.requestParams.userId = null;
                vm.requestParams.filter = null;
                getTimeSheetSearch();
                vm.getAll();
                getUsers();
               
                
            };

            function getTimeSheetSearch() {
                vm.loading = true;
                abp.ui.setBusy();
                if (vm.searchBox != null) {
                    vm.requestParams.filter = vm.searchBox;

                }
                if (vm.timesheet.fromDate != null) {
                    vm.requestParams.fromDate = vm.timesheet.fromDate;
                    //vm.requestParams.fromDate = moment(vm.timesheet.fromDate).toGMTString(true);
                   
                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.timesheet.toDate != null) {
                    vm.requestParams.toDate = vm.timesheet.toDate;
                    //vm.requestParams.toDate = moment(vm.timesheet.toDate).toGMTString(true);
                } else {
                    vm.requestParams.toDate = null;
                }
                if (vm.timesheet.userId != "") {
                    vm.requestParams.userId = vm.timesheet.userId;

                } else {
                    vm.requestParams.userId = null;
                }
                timeSheetService.getTimeSheetData($.extend({}, vm.requestParams))
                    .then(function(result) {
                        vm.timesheetlist = result.data.items;
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

            vm.searchAll = function(n) {
                //debugger;
                vm.skipCount = n;
                getTimeSheetSearch();
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
                            '      <li><a ng-click="grid.appScope.openTimeSheetEditModal(row.entity)" ng-if="(row.entity.isEdit==true)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.details(row.entity)">' + App.localize('Details') + '</a></li>' +

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('ProjectName'),
                        field: 'projectName',
                        enableColumnMenu: false,
                        //width: 500,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectName}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('Type'),
                        field: 'workTypeName',
                        enableColumnMenu: false,
                        width: 200,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.workTypeName}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('Date'),
                        field: 'date',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 120,
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        //cellTemplate:
                        //'<div class=\"ui-grid-cell-contents\">' +
                        //'{{row.entity.date}}' +
                        //'</div>'

                    },
                    {
                        name: App.localize('Hours'),
                        field: 'hours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.hours}}' +
                            '</div>'
                    },

                ],
                onRegisterApi: function(gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function(grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Date desc"
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
            vm.openTimeSheetEditModal = function (timesheet) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/timesheet/editTimeSheet.cshtml',
                    controller: 'app.views.timesheet.editTimeSheet as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return timesheet.id;
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
            vm.delete = function(timesheet) {
                abp.message.confirm(
                    "Delete Data '" + timesheet.projectName + "'?",
                    "Delete?",
                    function(result) {
                        if (result) {
                            timeSheetService.deleteTimeSheet({ id: timesheet.id })
                                .then(function() {
                                    abp.notify.success("Deleted Successfully");

                                    vm.getAll();
                                });
                        }
                    });
            }
            vm.details = function(timesheet) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/timesheet/details.cshtml',
                    controller: 'app.views.timesheet.details as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return timesheet.id;
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
            init = function() {
                getUsers();
                vm.getAll();

            }
            init();
        }

    ]);
})();