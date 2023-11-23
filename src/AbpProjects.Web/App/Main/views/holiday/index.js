(function() {
    angular.module('app').controller('app.views.holiday.index', [
        '$scope', '$uibModal', 'abp.services.app.holiday', 'uiGridConstants',
        function($scope, $uibModal, holidayService, uiGridConstants) {
            var vm = this;

            vm.holidayList = [];
            //$scope.record = true;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };

            function getHoliday() {
                //debugger;
                abp.ui.setBusy();
                holidayService.getHolidayList(
                    $.extend({
                        Title: vm.searchBox
                    }, vm.requestParams)
                ).then(function (result) {
                    vm.holidayList = result.data.items;
                    vm.totalRecord = result.data.totalCount;
                    for (var i = 0; i < result.data.items.length; i++) {
                        if (result.data.items[i].type == 0) {
                            result.data.items[i].type = "Half Day";
                        } else {
                            result.data.items[i].type = "Full Day";
                        }
                    }
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                        //$scope.record = false;
                        //vm.norecord = true;
                        //abp.notify.info(app.localize('NoRecordFound'));
                    } else { $scope.noData = false; }
                }).finally(function ()
                {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            vm.search = function() {
                skipCount = 0;
                currentPage = 0;
                getHoliday();
            }

            vm.downloadPDF = function(documentUpload) {
                //debugger;
                window.open(abp.appPath + 'userfiles/InvoicesDocuments/' + documentUpload + '?v=' + new Date().valueOf(), '_blank');
            };

            vm.openHolidayCreationModal = function() {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/holiday/create.cshtml',
                    controller: 'app.views.holiday.create as vm',
                    backdrop: 'static'
                });


                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function() {
                    getHoliday();
                });
            };

            vm.openHolidayEditModal = function(holiday) {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/holiday/edit.cshtml',
                    controller: 'app.views.holiday.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return holiday.id;
                        }
                    }
                });

                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function() {
                    getHoliday();
                });
            };

            vm.refreshGrid = function(n) {
                skipCount = n;
                getHoliday();
            };
            var init = function() {

                getHoliday()
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
                            '      <li><a ng-click="grid.appScope.openHolidayEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                            /*'      <li><a ng-click="grid.appScope.openPermissionModal(row.entity)">' + App.localize('Change Permissions') + '</a></li>' +*/
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('Date'),
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'startDate',
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.startDate |date:"dd/MM/yy"}} To {{row.entity.endDate |date:"dd/MM/yy"}}</span>' +
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
                        name: 'Type',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        field: 'type',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.type}} </span>' +
                            '</div>',
                        width: 150,
                    },
                    {
                        name: 'Title',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'title',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.title}} </span>' +
                            '</div>',
                        //width: 180,
                    }
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
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        getHoliday();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function(pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getHoliday();
                    });
                },
                data: []
            };
            init();

            vm.delete = function(holiday) {
                abp.message.confirm(
                    "Delete Title '" + holiday.title + "'?",
                    "Delete?",
                    function(isConfirmed) {
                        if (isConfirmed) {
                            holidayService.deleteHoliday({ id: holiday.id })
                                .then(function() {
                                    abp.notify.info("Deleted TenancyName: " + holiday.title);
                                    getHoliday();
                                });
                        }
                    });
            }

            //vm.refresh = function () {
            //    getInvoices();
            //};

            getHoliday();
        }
    ]);
})();