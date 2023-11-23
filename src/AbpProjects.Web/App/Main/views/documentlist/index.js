(function () {
    angular.module('app').controller('app.views.documentlist.index', [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.document', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, documentService, uiGridConstants) {
            var vm = this;
            $scope.record = true;
            vm.document = {};
            vm.loading = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Title asc"
            };
            vm.getAll = function () {
                vm.loading = true;
                abp.ui.setBusy();
                // debugger;
                documentService.getDocumentData($.extend({}, vm.requestParams)).then(function (result) {
                    //  debugger;
                    vm.document = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                        $scope.noData = true;
                    }
                    else { $scope.noData = false;}
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }
            vm.downloadPDF = function (document) {
                window.open(abp.appPath + 'UserFiles/Documents/' + document + '?v=' + new Date().valueOf(), '_self');
            };
            function getDocumentSearch() {
                vm.loading = true;
                abp.ui.setBusy();
                documentService.getDocumentList($.extend({ title: vm.searchBox }, vm.requestParams)).then(function (result) {
                    vm.document = result.data;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                    }
                    else { $scope.noData = false;}
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                getDocumentSearch();
            };
        /*{{row.entity.title}}*/
            //pull - right
            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                //enableSorting: false,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                  
                    {
                        name: App.localize('Title'),
                        field: 'title',
                        enableColumnMenu: false,
                        enableSorting: true,
                        //headercellClass: 'centeralign',
                        //cellClass: 'centeralign',
                        
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.title}} </span>' +
                            '</div>',  
                        
                    },
                    {
                        
                        name: App.localize('Download'),
                        field: 'attachment',
                        enableColumnMenu: false,
                        enableSorting: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 80,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' + '<a class="gridlink" ng-click="grid.appScope.downloadPDF(row.entity.attachment)"  style="text-align:center"><img src="/images/icon_download.svg" class="btndownloadicon"/></a>'
                            + '</div>',

                    },

                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Title asc"
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
            vm.getAll();
        }
    ]);
})();