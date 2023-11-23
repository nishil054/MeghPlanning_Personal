(function() {
    angular.module('app').controller('app.views.importuserstory', [
        '$scope', '$state', '$stateParams', '$uibModal', 'abp.services.app.importUserStoryDetails', 'abp.services.app.project', 'abp.services.app.masterList', 'uiGridConstants',
        function ($scope, $state, $stateParams, $uibModal, userStoryService, projectService, masterListservice, uiGridConstants) {
            //debugger;
            var vm = this;
            vm.norecord = false;
            $scope.noData = false;
            vm.importuserstory = {};
            vm.projectTask = [];
            vm.obj = {};
            vm.projectName = "";
            vm.importuserstory.status = "0";
            vm.obj.id = $stateParams.id;
            //vm.import = abp.auth.isGranted('Pages.Import');
            //vm.importexcel = abp.auth.isGranted('Pages.ImportExcel');
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                id: $stateParams.id,
                status: vm.importuserstory.status,
                EmployeeId: vm.importuserstory.employeeId,
                //FilterText: vm.filterText
            };

            $scope.statusChange = function () {
                
                vm.getUserStoryData();
            }
            $scope.getEmployeeUserStory = function () {

                vm.getUserStoryData();
            }
            function getEmployee() {

                masterListservice.getEmployee()
                    .then(function (result) {

                        vm.employeelist = result.data;
                    });
            }

            vm.getDataView = function () {
                projectService.getProjectName({
                    id: $stateParams.id
                    
                }).then(function (result) {
                    vm.projectName = result;
                    console.log(result);
                });
            }

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };
            vm.clear = function () {
                getProjectList();
                //vm.searchBox = "";
                vm.importuserstory.projectId = "";
                vm.getUserStoryData();
            };
            function getProjectList() {
                projectService.getProjectData({}).then(function (result) {
                    vm.projectTask = result.data.items;
                });
            }
            vm.getUserStoryData = function () {
                abp.ui.setBusy();
                //if (vm.filterText != null) {
                //    vm.requestParams.FilterText = vm.filterText;

                //}
                vm.loading = true;
                if (vm.importuserstory.projectId != "" || vm.importuserstory.projectId != undefined) {
                    vm.requestParams.projectId = vm.importuserstory.projectId;
                }
                if (vm.importuserstory.status != "0") {
                    vm.requestParams.status = vm.importuserstory.status;
                } else {
                    vm.requestParams.status = "0";
                }
                if (vm.importuserstory.employeeId != "0") {
                    vm.requestParams.employeeId = vm.importuserstory.employeeId;
                } else {
                    vm.requestParams.employeeId = "0";
                }
                userStoryService.getImportUserStoryData($.extend({}, vm.requestParams))
                    .then(function (result) {
                    vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        //debugger;
                        if (result.data.totalCount == 0) {
                        //vm.norecord = true;
                        $scope.noData = true;
                        //abp.notify.info(app.localize('NoRecordFound'));
                    } else {
                        //vm.norecord = false;
                        $scope.noData = false;

                        for (var i = 0; i < vm.userGridOptions.data.length; i++) {
                            // vm.userGridOptions.data[i].price = vm.userGridOptions.data[i].price.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
                            vm.userGridOptions.data[i].status = vm.userGridOptions.data[i].status + "";
                        }
                    }

                }).finally(function() {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            vm.openUserStoryCreationModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/importuserstory/addUserStory.cshtml',
                    controller: 'app.views.importuserstory.addUserStory as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return $stateParams.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getUserStoryData();
                });
            };

            vm.importExcelUserStory = function () {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/importuserstory/Create.cshtml',
                    controller: 'app.views.importuserstory.Create as vm',
                    backdrop: 'static', 
                    resolve: {
                        id: function () {
                            return $stateParams.id;

                        },
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    vm.getUserStoryData();
                });
            };

            vm.userStoryList = function (data) {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/importuserstory/userStory.cshtml',
                    controller: 'app.views.importuserstory.userStory as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        id: function () {
                            return data.id;

                        },
                        employeeId: function () {
                            return vm.importuserstory.employeeId;

                        },
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    //vm.getAll();
                });
            };

            vm.delete = function (data) {
                abp.message.confirm(
                    "Delete User Story ?",
                    "Delete?",
                    function (result) {
                        if (result) {
                            userStoryService.delete({ id: data.id })
                                .then(function () {
                                    abp.notify.success("User Story Deleted");
                                    vm.getUserStoryData();
                                });
                        }
                    });
            }

            vm.typeData = function (data) {
                if (data == null) {
                    vm.viewData = null;
                } else {
                    vm.viewData = data;
                    $state.go('userStory', { id: data.id });
                }
            };

            vm.updateStatusUserstory = function (id, status) {
                var postparam = {};

                if (status != "") {
                    postparam.id = id;
                    postparam.status = status;
                    try {
                        userStoryService.updateUserstoryStatus(postparam)
                            .then(function () {
                                abp.notify.success("Status updated successfully.");
                                vm.getUserStoryData();
                            });
                    } catch (exp) { }
                } else {
                    abp.notify.error("Please Select Status!");
                    status = 0;
                }

            }

            vm.assignEmployee = function (milestone) {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/importuserstory/assignto.cshtml',
                    controller: 'app.views.importuserstory.assignto as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return milestone.id;
                        },
                        projectid: function () {
                            return $stateParams.id;
                        },
                        empName: function () {
                            return milestone.userName;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    vm.getUserStoryData();
                    //vm.getAll();
                    //getUsers();

                });
            };

            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                vm.getUserStoryData();
            };

            var init = function () {
                getEmployee();
            };
            init();

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
                        name: App.localize('Actions'),
                        enableSorting: false,
                        enableColumnMenu: false,
                        enableScrollbars: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 100,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span><i ng-click="grid.appScope.Details(row.entity)" class="fa fa-eye" aria-hidden="true"></i></span>' +
                            '</div>',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                            '    <ul uib-dropdown-menu>' +
                           /* '      <li><a ng-click="grid.appScope.Details(row.entity)">' + App.localize('Details') + '</a></li>' +*/
                            '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('UserStory'),
                        field: 'UserStory',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        //width: 500,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.userStory}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('ProjectName'),
                        field: 'ProjectName',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        //width: 200,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectName}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('DevelopersHours'),
                        //displayName: 'DB Type',
                        field: 'DevelopersHours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 150,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.developerHours}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('ExpectedHours'),
                        //displayName: 'Storage Container',
                        field: 'ExpectedHours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 120,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.expectedHours}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('ActualHours'),
                        //displayName: 'Storage Container',
                        field: 'ActualHours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 100,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '<a ng-click="grid.appScope.userStoryList(row.entity)"> {{row.entity.actualHours}} </a>' +
                            /*'<ng-click="grid.appScope.userStoryList(row.entity)" ng-show="grid.appScope.userStoryList(row.entity.actualHours == 0)"> {{row.entity.actualHours}}' +*/
                            /*'{{row.entity.actualHours}}' +*/
                            '</div>'
                    },
                    {
                        name: "Assign To",
                        enableColumnMenu: false,
                        field: 'userName',
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        cellTemplate:
                            '<div  class=\"ui-grid-cell-contents div_center\" style=\"display:flex; flex-direction:column;align-items: flex-start;\"><span ng-if="row.entity.employeeId!=0">{{row.entity.userName}} </span> ' +
                            '  <a ng-if="row.entity.employeeId==0" ng-click="grid.appScope.assignEmployee(row.entity)"> <span"> Assign </span></a>'
                            + ' <a ng-if="row.entity.employeeId!=0" ng-click="grid.appScope.assignEmployee(row.entity)"> <span> Reassign  </span></a>' +
                            '</div>',
                       
                        width: 200,
                    },
                    {
                        name: "Status",
                        /*result.data.items[0].status//*/
                        /* field: 'projectStatusId',*/
                        enableColumnMenu: false,
                        width: 120,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        /*visible: vm.ishown == true,*/
                        //type: uiGridConstants.filter.SELECT,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
                            '<select id=\"ddlcom-{{row.entity.id}}\" class="form-control" ng-model="row.entity.status" required ng-change="grid.appScope.updateStatusUserstory(row.entity.id, row.entity.status)" >' +
                            '<option value="0">Pending</option>' +
                            '<option value="1">Complete</option>' +
                            '</select>' +
                            /*'<span ng-if="grid.appScope.chckprojectadminpermission()==false ">{{row.entity.projectStatus}}</span>' +*/
                            '</div>',
                        //visible: vm.permissions.adminwrites == true ? true : false,
                        //visible: vm.permissions.employeewrites == false ? true : false,
                    },
                ],
                onRegisterApi: function(gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function(grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getUserStoryData();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function(pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getUserStoryData();
                    });
                },
                data: []
            };

            var init = function () {
                vm.getDataView();
                vm.getUserStoryData();
                
                getProjectList();
            };

            //vm.Details = function (timesheet) {
            //    var modalInstance = $uibModal.open({
            //        templateUrl: '/App/Main/views/importftp/Details.cshtml',
            //        controller: 'app.views.importftp.Details as vm',
            //        backdrop: 'static',
            //        resolve: {
            //            id: function () {
            //                return timesheet.id;
            //            }
            //        }
            //    });

            //    modalInstance.rendered.then(function () {
            //        $timeout(function () {
            //            $.AdminBSB.input.activate();
            //        }, 0);
            //    });

            //    modalInstance.result.then(function () {
            //        vm.getAll();
            //    });
            //};

            //vm.search = function (filterText) {
            //    vm.filterText = filterText;
            //    vm.getAll();
            //};

            //vm.importExcel = function () {
            //    var modalInstance = $uibModal.open({
            //        templateUrl: '/App/Main/views/importftp/Create.cshtml',
            //        controller: 'app.views.importftp.Create as vm',
            //        backdrop: 'static'
            //    });

            //    modalInstance.rendered.then(function () {
            //        $.AdminBSB.input.activate();
            //    });

            //    modalInstance.result.then(function () {
            //        vm.getAll();
            //    });
            //};

            //vm.checkimportExcelBtnpermission = function () {
            //    if (vm.import && vm.importexcel) {
            //        return true;
            //    } else {
            //        return false;
            //    }

            //}

            init();

            vm.cancel = function () {
                $state.go('project');
            };
        }
    ]);



})();