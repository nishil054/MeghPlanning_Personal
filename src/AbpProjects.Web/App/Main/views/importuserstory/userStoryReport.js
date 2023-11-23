(function () {
    angular.module('app').controller('app.views.userStoryReport', [
        '$scope', '$state','$http', '$stateParams', '$uibModal', 'abp.services.app.importUserStoryDetails', 'abp.services.app.masterList', 'uiGridConstants',
        function ($scope, $state,$http, $stateParams, $uibModal, userStoryService,masterListservice, uiGridConstants) {
            //debugger;
            var vm = this;
            $scope.noData = true;
            vm.norecord = false;
            vm.importuserstory = {};
            //vm.userStoryList = [];
            vm.projectTask = [];
            vm.obj = {};
            vm.projectName = "";
            vm.obj.id = $stateParams.id;
            vm.userstorylist = abp.auth.isGranted('Pages.UserStoryReport');
            vm.isChecked = true;
            //vm.import = abp.auth.isGranted('Pages.Import');
            //vm.importexcel = abp.auth.isGranted('Pages.ImportExcel');
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                fromDate: vm.importuserstory.fromDate,
                toDate: vm.importuserstory.toDate,
                projectId: vm.importuserstory.projectId,
                employeeId: vm.importuserstory.employeeId,
                status: vm.importuserstory.status,
                //id: $stateParams.id,
                //FilterText: vm.filterText
            };


            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.clearAll = function () {
                getProjectList();
                getEmployee();
                vm.requestParams.fromDate = null;
                vm.requestParams.projectId = null;
                vm.requestParams.employeeId = null;
                vm.importuserstory.fromDate = null;
                vm.importuserstory.toDate = null;
                vm.requestParams.toDate = null;
                vm.importuserstory.projectId = null;
                vm.importuserstory.employeeId = null;
                vm.importuserstory.status = "";
                vm.requestParams.status = "";
                $("#ddlcom").select2("val", null);
                $("#ddlemployee").select2("val", null);
                $("#ddlstatus").select2("val", "");
                $scope.noData = true;
                vm.isChecked = true;
            };
            
            function getProjectList() {
                //projectService.getProjectData({}).then(function (result) {
                masterListservice.getProject().then(function (result) {
                    //vm.projectTask = result.data;
                    if (result.data.length > 0) {
                        vm.projectTask = result.data;
                        vm.isChecked = true;
                    }
                    else {
                        vm.isChecked = false;
                    }
                });
            }
            function getEmployee() {

                masterListservice.getEmployee()
                    .then(function (result) {
                        //vm.employeelist = result.data;
                        if (result.data.length > 0) {
                            vm.employeelist = result.data;
                            vm.isChecked = true;
                        }
                        else {
                            vm.isChecked = false;
                        }
                    });
            }

            vm.chckuserstorylistrequestpermission = function () {

                if (vm.userstorylist) {

                    return true;
                } else {

                    return false;

                }

            }
            vm.getUserStoryData = function () {
                
                abp.ui.setBusy();
                vm.loading = true;
                if (vm.importuserstory.projectId != "" || vm.importuserstory.projectId != undefined) {
                    vm.requestParams.projectId = vm.importuserstory.projectId;
                }
                if (vm.importuserstory.employeeId != "" || vm.importuserstory.employeeId != undefined) {
                    vm.requestParams.employeeId = vm.importuserstory.employeeId;
                }
                if (vm.importuserstory.fromDate != null) {
                    vm.requestParams.fromDate = vm.importuserstory.fromDate;
                    //vm.requestParams.fromDate = moment(vm.timesheet.fromDate).toGMTString(true);

                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.importuserstory.toDate != null) {
                    vm.requestParams.toDate = vm.importuserstory.toDate;
                    //vm.requestParams.toDate = moment(vm.timesheet.toDate).toGMTString(true);
                } else {
                    vm.requestParams.toDate = null;
                }
                if (vm.importuserstory.status != null) {
                    vm.requestParams.status = vm.importuserstory.status;
                }
                else {
                    vm.requestParams.status = null;
                }
                userStoryService.getImportUserStoryReport($.extend({}, vm.requestParams)).then(function (result) {
                    //debugger;
                    //vm.userStoryList = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        //vm.norecord = true;
                        $scope.noData = true;
                        vm.isChecked = true;
                        //$scope.record = false;
                        //abp.notify.info(app.localize('NoRecordFound'));
                    } else {
                        //vm.norecord = false;
                        $scope.noData = false;
                        vm.isChecked = false;
                        //$scope.record = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            vm.exportExcel = function ()
            {
                var datefromdate = vm.importuserstory.fromDate;
                var datetodate = vm.importuserstory.toDate;

                if (vm.importuserstory.fromDate != null) {
                    datefromdate = vm.importuserstory.fromDate._d;
                }
                if (vm.importuserstory.toDate != null) {
                    datetodate = vm.importuserstory.toDate._d;
                }

                var url = "../exportToExcel/ExportUserStoryReportToExcel";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        fromDate: datefromdate,
                        toDate: datetodate,
                        projectId: vm.importuserstory.projectId,
                        employeeId: vm.importuserstory.employeeId,
                        status: vm.importuserstory.status,
                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });

            }
            

            $scope.getProjectUserStory = function (id) {
                //debugger;
                vm.requestParams.projectId = id;
                vm.getUserStoryData();
            }

            $scope.getEmployeeUserStory = function (id) {
                //debugger;
                vm.requestParams.employeeId = id;
                vm.getUserStoryData();
            }

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
                    //vm.getAll();
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

            vm.typeData = function (data) {
                if (data == null) {
                    vm.viewData = null;
                } else {
                    vm.viewData = data;
                    $state.go('userStory', { id: data.id });
                }
            };

            vm.searchAll = function () {
                if ((vm.importuserstory.projectId == null || vm.importuserstory.projectId == "") && (vm.importuserstory.employeeId == null || vm.importuserstory.employeeId == "") && (vm.importuserstory.status == "" || vm.importuserstory.status == undefined) && vm.importuserstory.toDate == null && vm.importuserstory.fromDate == null) {
                    $scope.noData = true;
                    vm.isChecked = true;
                }
                else {
                    vm.getUserStoryData();
                } 
            }

            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                vm.getUserStoryData();
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
                    //    name: App.localize('Actions'),
                    //    enableSorting: false,
                    //    enableColumnMenu: false,
                    //    enableScrollbars: false,
                    //    headercellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    width: 100,
                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        '  <span><i ng-click="grid.appScope.Details(row.entity)" class="fa fa-eye" aria-hidden="true"></i></span>' +
                    //        '</div>',
                    //    //cellTemplate:
                    //    //    '<div class=\"ui-grid-cell-contents\">' +
                    //    //    '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                    //    //    '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                    //    //    '    <ul uib-dropdown-menu>' +
                    //    //    '      <li><a ng-click="grid.appScope.Details(row.entity)">' + App.localize('Details') + '</a></li>' +
                    //    //    //'      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +

                    //    //    '    </ul>' +
                    //    //    '  </div>' +
                    //    //    '</div>'
                    //},
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
                        width: 200,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectName}}' +
                            '</div>'
                    },
                    {
                        name: 'Creation Date',
                        field: 'creationDate',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 120,
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                    },
                    
                    {
                        name: App.localize('DevelopersHours'),
                        //displayName: 'DB Type',
                        field: 'developerHours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 150,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.developerHours}}' +
                            '</div>'
                    },
                    //{
                    //    name: App.localize('Database Details'),
                    //    displayName: 'Database Details',
                    //    /* field: 'DatabaseDetails',*/
                    //    field: 'databaseName',
                    //    enableColumnMenu: false,
                    //    width: 210,
                    //    cellTemplate: '<div class=\"ui-grid-cell-contents \">' +
                    //        '<div><p ng-if="row.entity.onlineManager!=null && row.entity.onlineManager !=\'\'"><b>Online Manager:</b><span>{{row.entity.onlineManager}} </span></p><p ng-if="row.entity.onlineManagerHostName != null && row.entity.onlineManagerHostName !=\'\'"><b>Host Name:</b><span>{{row.entity.onlineManagerHostName}} </span></p><p ng-if="row.entity.databaseName!=null && row.entity.databaseName !=\'\'"><b>Database Name:</b><span>{{row.entity.databaseName}} </span></p><p ng-if="row.entity.dataBaseUserName!=null && row.entity.dataBaseUserName !=\'\'"><b>DB User Name:</b><span>{{row.entity.dataBaseUserName}} </span></p><p ng-if="row.entity.dataBasePassword != null && row.entity.dataBasePassword !=\'\'"><b>DB Password:</b><span>{{row.entity.dataBasePassword}} </span></p></div>' +
                    //        '</div>' +
                    //        ' <div class=\" text-center\" ng-if="(row.entity.onlineManager == null || row.entity.onlineManager ==\'\') && (row.entity.onlineManagerHostName == null || row.entity.onlineManagerHostName ==\'\') && (row.entity.databaseName == null || row.entity.databaseName ==\'\') && (row.entity.dataBaseUserName == null || row.entity.dataBaseUserName ==\'\') && (row.entity.dataBasePassword == null || row.entity.dataBasePassword ==\'\')">-</div>' +
                    //        '</div>',
                    //},
                    {
                        name: App.localize('ExpectedHours'),
                        //displayName: 'Storage Container',
                        field: 'expectedHours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 150,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.expectedHours}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('ActualHours'),
                        //displayName: 'Storage Container',
                        field: 'actualHours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 120,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '<a ng-click="grid.appScope.userStoryList(row.entity)" ng-show= "grid.appScope.chckuserstorylistrequestpermission()==true"> {{row.entity.actualHours}} </a>' +
                            '<ng-click="grid.appScope.userStoryList(row.entity)" ng-show= "grid.appScope.chckuserstorylistrequestpermission()==false">{{row.entity.actualHours}}' +
                            /*'{{row.entity.actualHours}}' +*/
                            /*'{{row.entity.actualHours}}' +*/
                            '</div>'
                    },
                    {
                        name: App.localize('Status'),
                        field: 'status',
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        enableColumnMenu: false,
                        width: 90,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '<div><span ng-if="row.entity.status==0">Pending</span><span ng-if="row.entity.status==1">Completed</span>' +
                            '</div>',
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

                        vm.getUserStoryData();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getUserStoryData();
                    });
                },
                data: []
            };

            var init = function () {
                //vm.getUserStoryData();
                getProjectList();
                getEmployee();
            };
            init();

            vm.cancel = function () {
                $state.go('project');
            };
        }
    ]);



})();