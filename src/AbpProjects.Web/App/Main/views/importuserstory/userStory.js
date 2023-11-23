(function () {
    angular.module('app').controller('app.views.importuserstory.userStory', [
        '$scope', '$state', '$uibModal', '$uibModalInstance', '$stateParams', 'abp.services.app.project', 'abp.services.app.timeSheet', 'uiGridConstants', 'id','employeeId',
        function ($scope, $state, $uibModal, $uibModalInstance, $stateParams, projectService, timeSheetService, uiGridConstants, id, employeeId) {
            //debugger;
            //alert("1");
            var vm = this;
            vm.data = {};
            vm.projectDetails = [];
            vm.userStoryList = [];
            vm.data.id = $stateParams.id;
            if (employeeId == "") {
                vm.data.employeeId = null;
            } else {
                vm.data.employeeId = employeeId;
            }
            //vm.importuserstory.employeeId = employeeId;
            //vm.projectId = $stateParams.id;

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                id: id,
                employeeId: vm.data.employeeId,
                //filter: vm.searchBox,
                //fromDate: vm.timesheet.fromDate,
                //toDate: vm.timesheet.toDate,
                //userId: vm.timesheet.userId,
            };

            //vm.getDataView = function () {
            //    projectService.getProjectViewById({
            //        id: vm.data.id
            //    }).then(function (result) {
            //        vm.data = result.data;
            //        console.log(result);
            //    });
            //}

            //function getprojectDetails() {
            //    projectService.getprojectDetailsList({ id: vm.data.id })
            //        .then(function (result) {
            //            vm.projectDetails = result.data.items;
            //        });
            //}
            function getUserStoryDetails() {
                //debugger;
                abp.ui.setBusy();
                timeSheetService.getUserStoryDetailsList($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.userStoryList = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0) {
                            $scope.noData = true;
                        } else {
                            $scope.noData = false;
                        }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }


            vm.openEdit = function (projectList) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/project/editProjectType.cshtml',
                    controller: 'app.views.project.editProjectType as vm',
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
                    getprojectDetails();
                });
            };

            vm.viewData = null;

            //vm.openEdit = function (data) {
            //    if (data == null) {
            //        vm.viewData = null;
            //    }
            //    else {
            //        vm.viewData = data;
            //        $state.go('editProjectType', { id: data.id });
            //    }
            //};
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
            vm.openProjectCreationModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/project/createProjectType.cshtml',
                    controller: 'app.views.project.createProjectType as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return vm.projectId;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    getprojectDetails();
                });
            };

            //vm.openProjectCreationModal = function (data) {
            //    $state.go('createProjectType', { id: vm.projectId });
            //};

            vm.openDelete = function (data) {
                abp.message.confirm(
                    "Delete Project Type '" + data.projectType + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {
                            projectService.deleteProjectType({ id: data.id })
                                .then(function () {
                                    abp.notify.success("Deleted");
                                    init();
                                });
                        }
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
                //    {
                //    name: App.localize('Actions'),
                //    enableSorting: false,
                //    enableColumnMenu: false,
                //    enableScrollbars: false,
                //    headerCellClass: 'centeralign',
                //    cellClass: 'centeralign',
                //    width: 70,
                //    cellTemplate: '<div class=\"ui-grid-cell-contents padd0\">' +
                //        '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                //        '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                //        '    <ul uib-dropdown-menu>' +
                //        /*'      <li><a ng-click="grid.appScope.openTimeSheetEditModal(row.entity)" ng-if="(row.entity.isEdit==true)">' + App.localize('Edit') + '</a></li>' +*/
                //        '      <li><a ng-click="grid.appScope.details(row.entity)">' + App.localize('Details') + '</a></li>' +

                //        '    </ul>' +
                //        '  </div>' +
                //        '</div>'
                //},
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
                //vm.getDataView();
                //getprojectDetails();
                getUserStoryDetails();
            }

            init();

            vm.close = function () {
                $uibModalInstance.dismiss();
            };

            //vm.cancel = function () {
            //    $state.go('project');
            //};
        }
    ]);
})();