
(function () {
    angular.module('app').controller('app.views.auditLogs.index', [
        '$scope', '$uibModal', 'uiGridConstants', 'abp.services.app.auditLog',
        function ($scope, $uibModal, uiGridConstants, auditLogService) {
            var vm = this;
            /*vm.task = [];*/
            vm.audit = [];
            var today = new Date();
            var regdatee = moment.utc(today);
            vm.date = regdatee;
            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            $scope.noData = false;
            vm.loading = false;
            vm.advancedFiltersAreShown = false;

            vm.requestParams = {
                userName: vm.audit.userName,
                serviceName: vm.audit.serviceName,
                methodName: vm.audit.methodName,
                browserInfo: vm.audit.browserInfo,
                hasException: vm.audit.hasException,
                skipCount: 0,
                startDate: vm.audit.startDate,
                endDate: vm.audit.endDate,
                hasException: vm.audit.hasException,
                minExecutionDuration: vm.audit.minExecutionDuration,
                maxExecutionDuration: vm.audit.maxExecutionDuration,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };
            //vm.createdtPickerOptions = function () {
            //         var options = {
            //        locale: {
            //            format: 'L',
            //            applyLabel:  ('Apply'),
            //            cancelLabel:  ('Cancel'),
            //            customRangeLabel:  ('CustomRange')
            //        },
            //        min: moment('2015-05-01'),
            //        minDate: moment('2015-05-01'),
            //        max: moment(),
            //        maxDate: moment(),
            //        ranges: {}
            //    };

            //    options.ranges[ ('Today')] = [moment().startOf('day'), moment().endOf('day')];
            //    options.ranges[ ('Yesterday')] = [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')];
            //    options.ranges[ ('Last7Days')] = [moment().subtract(6, 'days').startOf('day'), moment().endOf('day')];
            //    options.ranges[ ('Last30Days')] = [moment().subtract(29, 'days').startOf('day'), moment().endOf('day')];
            //    options.ranges[ ('ThisMonth')] = [moment().startOf('month'), moment().endOf('month')];
            //    options.ranges[ ('LastMonth')] = [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')];

            //    return options;
            ////};

            ////vm.createdtPickerOptions();
            //vm.dateRangeModel = {
            //    startDate: moment().startOf('day'),
            //    endDate: moment().endOf('day')
            //};


            //vm.createDateRangePickerOptions = function () {
            //    var options = {
            //        locale: {
            //            format: 'L',
            //            applyLabel:  ('Apply'),
            //            cancelLabel:  ('Cancel'),
            //            customRangeLabel:  ('CustomRange')
            //        },
            //        min: moment('2015-05-01'),
            //        minDate: moment('2015-05-01'),
            //        max: moment(),
            //        maxDate: moment(),
            //        ranges: {}
            //    };

            //    options.ranges[ ('Today')] = [moment().startOf('day'), moment().endOf('day')];
            //    options.ranges[ ('Yesterday')] = [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')];
            //    options.ranges[ ('Last7Days')] = [moment().subtract(6, 'days').startOf('day'), moment().endOf('day')];
            //    options.ranges[ ('Last30Days')] = [moment().subtract(29, 'days').startOf('day'), moment().endOf('day')];
            //    options.ranges[ ('ThisMonth')] = [moment().startOf('month'), moment().endOf('month')];
            //    options.ranges[ ('LastMonth')] = [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')];

            //    return options;
            //};
            vm.search = function () {

                abp.ui.setBusy();
                if (vm.audit.userName == null || vm.audit.userName == "" || vm.audit.userName == undefined) {
                    vm.requestParams.userName = null;
                } else {
                    vm.requestParams.userName = vm.audit.userName;

                }
                if (vm.audit.serviceName == null || vm.audit.serviceName == "" || vm.audit.serviceName == undefined) {
                    vm.requestParams.serviceName = null;
                } else {

                    vm.requestParams.serviceName = vm.audit.serviceName;
                }
                if (vm.audit.methodName == null || vm.audit.methodName == "" || vm.audit.methodName == undefined) {
                    vm.requestParams.methodName = null;
                } else {
                    vm.requestParams.methodName = vm.audit.methodName;
                }
                if (vm.audit.browserInfo == null || vm.audit.browserInfo == "" || vm.audit.browserInfo == undefined) {
                    vm.requestParams.browserInfo = null;
                } else {
                    vm.requestParams.browserInfo = vm.audit.browserInfo;
                }
                if (vm.audit.hasException == null || vm.audit.hasException == "" || vm.audit.hasException == undefined) {
                    vm.requestParams.hasException = null;
                } else {
                    vm.requestParams.hasException = vm.audit.hasException;
                }
                if (vm.audit.startDate == null || vm.audit.startDate == "" || vm.audit.startDate == undefined) {
                    vm.requestParams.startDate = null;
                } else {
                    vm.requestParams.startDate = vm.audit.startDate;
                }
                if (vm.audit.endDate == null || vm.audit.endDate == "" || vm.audit.endDate == undefined) {
                    vm.requestParams.endDate = null;
                } else {
                    vm.requestParams.endDate = vm.audit.endDate;
                }
                if (vm.audit.hasException == null || vm.audit.hasException == "" || vm.audit.hasException == undefined) {
                    vm.requestParams.hasException = null;
                } else {
                    vm.requestParams.hasException = vm.audit.hasException;
                }
                if (vm.audit.minExecutionDuration == null || vm.audit.minExecutionDuration == "" || vm.audit.minExecutionDuration == undefined) {
                    vm.requestParams.minExecutionDuration = null;
                } else {
                    vm.requestParams.minExecutionDuration = vm.audit.minExecutionDuration;
                }
                if (vm.audit.maxExecutionDuration == null || vm.audit.maxExecutionDuration == "" || vm.audit.maxExecutionDuration == undefined) {
                    vm.requestParams.maxExecutionDuration = null;
                } else {
                    vm.requestParams.maxExecutionDuration = vm.audit.maxExecutionDuration;
                }
                vm.getAuditLogs();
                if ($scope.toggle) {
                    $scope.toggle = !$scope.toggle;

                }
            }
           
            vm.clearSearch = function () {
                vm.requestParams.userName = null;
                vm.requestParams.serviceName = null;
                vm.requestParams.methodName = null;
                vm.requestParams.browserInfo = null;
                vm.requestParams.hasException = null;
                vm.requestParams.minExecutionDuration = null;
                vm.requestParams.maxExecutionDuration = null;
                vm.requestParams.startDate = null;
                vm.requestParams.endDate = null;



                vm.audit.userName = null;
                vm.audit.serviceName = null;
                vm.audit.methodName = null;
                vm.audit.browserInfo = null;
                vm.audit.hasException = null;
                vm.audit.minExecutionDuration = null;
                vm.audit.maxExecutionDuration = null;
                vm.audit.startDate = null;
                vm.audit.endDate = null;


                vm.getAuditLogs();
                if ($scope.toggle) {
                    $scope.toggle = !$scope.toggle;

                }
            }
            vm.gridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                columnDefs: [
                    {
                        name: 'Actions',
                        enableSorting: false,
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        width: 50,
                        headerCellTemplate: '<span></span>',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents text-center\">' +
                            '<button class="btneye" ng-click="grid.appScope.showDetails(row.entity)"><i class="fa fa-eye"></i></button>' +
                            '</div>'
                    },
                    {
                        field: 'exception',
                        enableSorting: false,
                        enableColumnMenu: false,
                        width: 30,
                        headerCellTemplate: '<span></span>',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents text-center\">' +
                            '<i class="fa fa-check-circle font-green" ng-if="!row.entity.exception"></i>' +
                            '<i class="fa fa-warning font-yellow-gold" ng-if="row.entity.exception"></i>' +
                            '</div>'
                    },
                    {
                        name: 'Time',
                        field: 'executionTime',
                        enableColumnMenu: false,
                        cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                        width: 150
                    },
                    {
                        name: 'UserName',
                        enableColumnMenu: false,
                        field: 'userName',
                        width: 110
                    },
                    {
                        name: 'Service',
                        enableColumnMenu: false,
                        enableSorting: false,
                        field: 'serviceName',
                        width: 190
                    },
                    {
                        name: 'Action',
                        enableColumnMenu: false,
                        enableSorting: false,
                        field: 'methodName',
                        width: 190
                    },
                    {
                        name: 'Duration',
                        enableColumnMenu: false,
                        field: 'executionDuration',
                        width: 100,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            ('Xms', '{{COL_FIELD CUSTOM_FILTERS}}') +
                            '</div>'
                    },
                    {
                        name: 'IpAddress',
                        enableColumnMenu: false,
                        field: 'clientIpAddress',
                        enableSorting: false,
                        width: 110
                    },
                    {
                        name: 'Client',
                        enableColumnMenu: false,
                        field: 'clientName',
                        enableSorting: false,
                        width: 90
                    },
                    {
                        name: 'Browser',
                        enableColumnMenu: false,
                        field: 'browserInfo',
                        enableSorting: false,
                        /*minWidth: 200*/
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = null;
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getAuditLogs();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getAuditLogs();
                    });
                },
                data: []
            };





            vm.getAuditLogs = function () {
                vm.loading = true;
                auditLogService.getAuditLogs($.extend({}, vm.requestParams/*, vm.dateRangeModel*/))
                    .then(function (result) {
                        /* vm.audit = result.data;*/

                        vm.gridOptions.totalItems = result.data.totalCount;
                        vm.gridOptions.data = result.data.items;
                        if (vm.gridOptions.data == 0) {
                            $scope.noData = true;

                        } else {
                            $scope.noData = false;

                        }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            };

            //vm.exportToExcel = function () {
            //    auditLogService.getAuditLogsToExcel($.extend({}, vm.requestParams, vm.dateRangeModel))
            //        .then(function (result) {
            //            app.downloadTempFile(result.data);
            //        });
            //};

            vm.showDetails = function (auditLog) {
                $uibModal.open({
                    templateUrl: '/App/Main/views/auditLogs/detailModal.cshtml',
                    controller: 'app.views.auditLogs.detailModal as vm',
                    backdrop: 'static',
                    resolve: {
                        auditLog: function () {
                            return auditLog;
                        }
                    }
                });
            };

            vm.getAuditLogs();
        }
    ]);
})();