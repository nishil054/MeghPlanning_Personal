(function() {
    angular.module('app').controller('app.views.invoicerequestlists.projectinvoicelist', [
        '$scope', '$state', '$timeout', '$uibModal', 'abp.services.app.project', 'uiGridConstants',
        function($scope, $state, $timeout, $uibModal, projectService, uiGridConstants) {
            var vm = this;



            vm.x = [];
            vm.supports = [];
            $scope.record = true;
            vm.task = {};

            vm.task.status = "0";

            /*$scope.cancelflag = "false";*/
            vm.requestParams = {

                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                status: vm.task.status,
                projectName: vm.task.projectName,


            };

            vm.save = function(t) {
                abp.message.confirm(
                    "Are you sure you want to cancel?",
                    "Cancel?",
                    function(isConfirmed) {
                        if (isConfirmed) {
                            supportService.updateDashboardService(t.id)
                                .then(function() {
                                    abp.notify.info("Cancel: " + t.domainName);
                                    /* getDomainDate();*/
                                    // init();
                                    getServiceMgt();
                                    getEmployeeNameActive();
                                    //getHostingDate();
                                    //getStorageDate();
                                    //getemailDate();
                                });
                        }
                    });
                //supportService.updateDashboardService(t.id).then(function () {

                //    abp.notify.info(App.localize('SavedSuccessfully'));
                //    getDomainDate();
                //    getHostingDate();
                //    getStorageDate();
                //    getemailDate();
                //    /*$uibModalInstance.close();*/
                //    console.log(vm.task);




                //});

            };

            function getInvoicerequestListByPrject() {
                abp.ui.setBusy();
                if (vm.task.status != "0") {
                    vm.requestParams.status = vm.task.status;
                } else {
                    vm.requestParams.status = "0";
                }
                if (vm.task.projectName != null) {
                    vm.requestParams.projectName = vm.task.projectName;
                } else {
                    vm.requestParams.projectName = null;

                }
                projectService.getInvoicerequestListByPrject(
                    $.extend({
                        Filter: vm.nameFilter

                    }, vm.requestParams)
                ).then(function(result) {
                    vm.supports = result.data.items;

                    vm.totalRecord = result.data.totalCount;

                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    console.log(vm.supports);
                    if (vm.supports == 0) {
                        $scope.record = false;
                    } else {
                        $scope.record = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            $scope.clearSearch = function() {
                vm.task.projectName = "";
                vm.task.status = "0";

                getInvoicerequestListByPrject();

            }

            vm.search = function() {
                    getInvoicerequestListByPrject();

                }




            vm.refreshGrid = function(n) {
                skipCount = n;
                getInvoicerequestListByPrject();

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
                    //{
                    //   // name: App.localize('Actions'),
                    //    enableSorting: false,
                    //    enableColumnMenu: false,
                    //    enableScrollbars: false,
                    //    headercellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    width: 85,
                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                    //        '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                    //        '    <ul uib-dropdown-menu>' +
                    //        '      <li><a ng-click="grid.appScope.openServiceEditModal(row.entity)" ng-show="row.entity.cancelflag==0 && row.entity.status==0">' + App.localize('Edit') + '</a></li>' +
                    //        '      <li><a ng-click="grid.appScope.openServiceDetailModal(row.entity)" ng-show="row.entity.cancelflag==1">' + App.localize('Detail') + '</a></li>' +
                    //        '      <li><a ng-click="grid.appScope.openServiceRenewModal(row.entity)" ng-show="row.entity.cancelflag==1">' + App.localize('Renew') + '</a></li>' +
                    //        '      <li><a ng-click="grid.appScope.openServiceAdjustmentModal(row.entity)" ng-show="row.entity.cancelflag==0">' + App.localize('Adjustment') + '</a></li>' +
                    //        '      <li><a ng-click="grid.appScope.save(row.entity)" ng-show="row.entity.cancelflag==0">' + App.localize('Cancel') + '</a></li>' +
                    //        '      <li><a ng-click="grid.appScope.openServiceHistoryModal(row.entity)" ng-show="row.entity.cancelflag==0">' + App.localize('History') + '</a></li>' +
                    //        /*   ng - show="row.entity.cancelflag==1"*/
                    //        '    </ul>' +
                    //        '  </div>' +
                    //        '</div>'
                    //},

                    //{
                    //    name: 'Service',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    field: 'serviceName',

                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        ' <span>{{row.entity.serviceName}}</span><br/>' +

                    //        '</div>',
                    //     width: 90,
                    //    height: 180,
                    //},
                    {
                        name: 'Project Name',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'projectName',

                        cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"float: left\">' +
                            ' <span>{{row.entity.projectName}}</span>' +

                            '</div>',
                        width: 180,
                        /*  cellClass: 'centeralign'*/
                    },
                    {
                        name: 'Marketing Person',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'marketingPerson',

                        cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"float: left\">' +
                            ' <span>{{row.entity.marketingPerson}}</span>' +

                            '</div>',
                        width: 150,
                        /*  cellClass: 'centeralign'*/
                    },
                    {
                        name: 'Client Name',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'clientName',

                        cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"float: left\">' +
                            ' <span>{{row.entity.clientName}}</span>' +

                            '</div>',
                        width: 240,
                        /*cellClass: 'centeralign'*/
                    },
                    {
                        name: 'Amount',
                        enableColumnMenu: false,
                        headerCellClass: 'rightalign',
                        cellClass: 'rightalign',
                        field: 'amount',
                        cellTemplate: '<div class=\"ui-grid-cell-contents specal1 gridcolumn\">' +
                            ' <div><span>{{row.entity.amount}} </span></div>' +
                            '</div>',
                        width: 70,
                    },
                    //{
                    //    name: 'Price',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'rightalign',
                    //    field: 'price',

                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents pecal1 gridcolumn\" style=\"float: right\">' +
                    //        ' <div><span>{{row.entity.price}}</span></div>' +

                    //        '</div>',
                    //    width: 90,
                    //    cellClass: 'centeralign'
                    //},
                    {
                        name: ' Comment',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'comment',
                        /* cellFilter: 'momentFormat: \'DD/MM/YYYY\'',*/
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span class=\"wordbreak\">{{row.entity.comment}}</span>' +
                            '</div>',
                        width: 220,
                    },
                    {
                        name: ' Created By',
                        enableColumnMenu: false,
                        headerCellClass: 'leftlign',
                        cellClass: 'leftalign',
                        field: 'creatorName',
                        /* cellFilter: 'momentFormat: \'DD/MM/YYYY\'',*/
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.creatorName}}</span>' +
                            '</div>',
                        width: 110,
                    },

                    {
                        name: 'Created On',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        field: 'creationTime',

                        //cellTemplate:
                        //    '<div class=\"ui-grid-cell-contents\">' +
                        //    ' <span>{{row.entity.creationTime}}</span>' +

                        //    '</div>',
                        width: 100,
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                    },
                    {
                        name: 'Status',

                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 100,
                        field: 'status',

                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +

                            ' <span ng-if="row.entity.status==0" class=\"badge badge-warning"\>Pending </span>' +
                            ' <span ng-if="row.entity.status==1" class=\"badge badge-success"\> Complete  </span>' +
                            ' <span ng-if="row.entity.status==2" class=\"badge badge-danger"\> Cancel  </span>' +
                            '</div>',

                        cellClass: 'centeralign'
                    },
                    //{
                    //    name: 'HostingSpace',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    field: 'hostingSpace',

                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        ' <span>{{row.entity.hostingSpace}}</span>' +

                    //        '</div>',
                    //    width: 130,
                    //    cellClass: 'centeralign'
                    //},
                ],
                onRegisterApi: function(gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function(grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        getInvoicerequestListByPrject();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function(pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getInvoicerequestListByPrject();
                    });
                },
                data: []
            };

            var init = function() {
                getInvoicerequestListByPrject();
            }
            init();


        }
    ]);
})();