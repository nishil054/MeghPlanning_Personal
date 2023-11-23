(function () {
    angular.module('app').controller('app.views.empuserStoryReport', [
        '$scope', '$state', '$stateParams', '$uibModal', 'abp.services.app.empUserStory', 'abp.services.app.masterList','uiGridConstants',
        function ($scope, $state, $stateParams, $uibModal, empUserStoryService,  masterListservice, uiGridConstants) {
            //debugger;
            var vm = this;
            $scope.noData = false;
            vm.norecord = false;
            vm.empimportuserstory = {};
            //vm.userStoryList = [];
            vm.projectTask = [];
            vm.obj = {};
            vm.projectName = "";
            vm.obj.id = $stateParams.id;
            
            vm.userstrory = abp.auth.isGranted('Pages.Employeewise.UserStory');
            
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                fromDate: vm.empimportuserstory.fromDate,
                toDate: vm.empimportuserstory.toDate,
                projectId: vm.empimportuserstory.projectId,
                userStory: vm.empimportuserstory.userStory,
                status: vm.empimportuserstory.status,
                
            };
            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };
            vm.clear = function () {
                getProjectList();
                vm.empimportuserstory.projectId = "";
                vm.getUserStoryData();
            };
            function getProjectList() {
                masterListservice.getProjectsByCurUser().then(function (result) {
                    vm.projectTask = result.data;
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
                vm.loading = true;
                if (vm.empimportuserstory.projectId != "" || vm.empimportuserstory.projectId != undefined) {
                    vm.requestParams.projectId = vm.empimportuserstory.projectId;
                }
                if (vm.empimportuserstory.fromDate != null) {
                    vm.requestParams.fromDate = vm.empimportuserstory.fromDate;
                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.empimportuserstory.toDate != null) {
                    vm.requestParams.toDate = vm.empimportuserstory.toDate;
                } else {
                    vm.requestParams.toDate = null;
                }
                if (vm.empimportuserstory.status != null) {
                    vm.requestParams.status = vm.empimportuserstory.status;
                }
                else {
                    vm.requestParams.status = null;
                }
                if (vm.empimportuserstory.userStory != null) {
                    vm.requestParams.userStory = vm.empimportuserstory.userStory;
                }
                empUserStoryService.getEmployeeUserStoryReport($.extend({}, vm.requestParams)).then(function (result) {
                    
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                        
                    } else {
                        $scope.noData = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                });

            }

            vm.searchAll = function () {
                if ((vm.empimportuserstory.projectId == null || vm.empimportuserstory.projectId == "") && (vm.empimportuserstory.userStory == null || vm.empimportuserstory.userStory == "") && (vm.empimportuserstory.status == "" || vm.empimportuserstory.status == undefined) && vm.empimportuserstory.toDate == null && vm.empimportuserstory.fromDate == null) {
                    $scope.noData = true;
                    vm.isChecked = true;
                }
                else {
                vm.getUserStoryData();
                }
            }

            $scope.getProjectUserStory = function (id) {
                vm.requestParams.projectId = id;
                vm.getUserStoryData();
            }

            vm.clearAll = function () {
                getProjectList();
                vm.empimportuserstory.userStory = null;
                vm.requestParams.userStory = null;
                vm.requestParams.fromDate = null;
                vm.requestParams.projectId = null;
                vm.empimportuserstory.fromDate = null;
                vm.empimportuserstory.toDate = null;
                vm.requestParams.toDate = null;
                vm.empimportuserstory.projectId = null;
                vm.empimportuserstory.status = "";
                vm.requestParams.status = "";
                $("#ddlcom").select2("val", null);
                $("#ddlstatus").select2("val", "");
                $scope.noData = true;
            };

            vm.userStoryList = function (data) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/importuserstory/empuserStory.cshtml',
                    controller: 'app.views.importuserstory.empuserStory as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        id: function () {
                            return data.id;

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
                    $state.go('empuserStory', { id: data.id });
                }
            };

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
                  
                    {
                        name: App.localize('UserStory'),
                        field: 'userStory',
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
                        field: 'projectName',
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
                        field: 'actualHours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 120,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '<a ng-click="grid.appScope.userStoryList(row.entity)"> {{row.entity.actualHours}} </a>' +
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
                $scope.noData = true;
            };
            init();
        }
    ]);



})();