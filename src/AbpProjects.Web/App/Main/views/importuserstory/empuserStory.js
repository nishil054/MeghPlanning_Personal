(function () {
    angular.module('app').controller('app.views.importuserstory.empuserStory', [
        '$scope', '$uibModal', '$uibModalInstance', '$stateParams', 'abp.services.app.empUserStory', 'uiGridConstants', 'id',
        function ($scope, $uibModal, $uibModalInstance, $stateParams, empUserStoryService, uiGridConstants, id) {
           
            var vm = this;
            vm.data = {};
            vm.projectDetails = [];
            vm.userStoryList = [];
            vm.data.id = $stateParams.id;
            
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                id: id,
            };

            function getUserStoryDetails() {
                empUserStoryService.getUserStoryDetailsList($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.userStoryList = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0) {
                            $scope.noData = true;
                        } else {
                            $scope.noData = false;
                        }
                    });
            }

            vm.viewData = null;

            vm.details = function (timesheet) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/timesheet/details.cshtml',
                    controller: 'app.views.timesheet.details as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return timesheet.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });
                modalInstance.result.then(function () {
                    vm.getUserStoryDetails();
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
                        name: App.localize('Description'),
                        field: 'description',
                        enableColumnMenu: false,
                        //width: 500,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.description}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('UserName'),
                        field: 'userName',
                        enableColumnMenu: false,
                        width: 200,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.userName}}' +
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
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }
                        vm.getUserStoryDetails();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;
                        vm.getUserStoryDetails();

                    });
                },
                data: []
            };

            function init() {
                getUserStoryDetails();
            }

            init();

            vm.close = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();