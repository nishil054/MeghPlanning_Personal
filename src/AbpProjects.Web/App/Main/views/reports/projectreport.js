(function () {
    angular.module('app').controller('app.views.reports.projectreport', [
        '$scope', '$state','$http', '$window', '$stateParams', '$timeout', '$uibModal', 'abp.services.app.timeSheet', 'uiGridConstants',
        function ($scope, $state,  $http,$window,$stateParams, $timeout, $uibModal, timeSheetService, uiGridConstants) {
            var vm = this;

            vm.supports = [];
            $scope.record = true;
            
            vm.task = {};
            
            vm.tname = [];
            vm.isChecked = true;
          
            
          /*  $scope.cancelflag = "false";*/
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
            };
            function getdata() {
                abp.ui.setBusy();
                timeSheetService.getProjectReport($.extend({}, vm.requestParams)
                ).then(function (result) {
                    //debugger;
                    vm.supports = result.data.items;

                    vm.totalRecord = result.data.totalCount;
                    if (result.data.totalCount > 0) {
                        vm.isChecked = false;
                    }
                    else {
                        vm.isChecked = true;
                    }
                    vm.userGridOptions.totalItems = result.data.totalCount;


                    vm.userGridOptions.data = result.data.items;
                    vm.userGridOptions.data.push({ userName: 'total', hours: $scope.getTotal(), efforts: $scope.getamountTotal(), });


                    console.log(vm.supports);
                    //debugger;
                    if (result.data.totalCount > 0) {
                        $scope.record = false;
                    }
                    else {
                        $scope.record = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            $scope.getProjectReport = function (id) {
                vm.requestParams.projectId = id;
                
                getdata();
            }
            $scope.getTotal = function () {
                var total = 0;
                for (var i = 0; i < $scope.vm.supports.length; i++) {
                    var product = $scope.vm.supports[i].hours;
                    total = product + total;
                }
                return total.toFixed(2);
            }
            $scope.getamountTotal = function () {

                var total = 0;
                for (var i = 0; i < $scope.vm.supports.length; i++) {
                    var product = $scope.vm.supports[i].efforts;
                    total = product + total;
                }
                return total;
            }

           
            function getProject() {
                timeSheetService.getProject({}).then(function (result) {
                   
                    vm.tname = result.data;
                    console.log(vm.tname);
                });
            }
           
            
            vm.openDetailModal = function (f) {

                $state.go('detailproject', { id: f.userId, pid: f.projectId });
               
            };

            vm.exportExcel = function () {

                var url = "../exportToExcel/ExportProjectReportToExcel/projectId";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        /*userId: vm.task.userId,*/
                       
                        projectId: vm.requestParams.projectId,
                        //t:vm.userGridOptions.data.push({ userName: 'total', hours: $scope.getTotal(), efforts: $scope.getamountTotal(), })
                        

                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });
            }
            vm.refreshGrid = function (n) {
                skipCount = n;
                getServiceMgt();
                getProjectReport();
            };
            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enablePaginationControls: false,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',

                columnDefs: [
                  

                   
                    {
                        name: 'UserName',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'userName',

                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\" style=\"float: left\">' +
                            ' <span ng-if=row.entity.userName!="total"><a class=\"culserpoint\" ng-click="grid.appScope.openDetailModal(row.entity)">{{row.entity.userName}}</a></span>' +
                            '<span ng-if=row.entity.userName=="total">{{row.entity.userName}}</span>'+
                           
                            '</div>',
                       
                     
                    },
             
                    {
                        name: 'No of hours',
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        field: 'hours',

                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\" >' +
                            ' <span>{{row.entity.hours}}</span>' +

                            '</div>',
                        width: 130,
                       
                    },
                    {
                        name: 'Amount',
                        enableColumnMenu: false,
                        headerCellClass: 'rightalign',
                        cellClass: 'rightalign',
                        field: 'efforts',

                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\" >' +
                            ' <span>{{row.entity.efforts}}</span>' +

                            '</div>',
                        width: 130,
                        
                    },
                    {
                        name: '',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'test',
                      
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.test}}</span>' +

                            '</div>',


                    },
                    {
                        name: 'userId',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'userId',
                        visible: false,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.userId}}</span>' +

                            '</div>',


                    },
                    {
                        name: 'projectId',
                        enableColumnMenu: false,
                        headerCellClass: 'leftalign',
                        cellClass: 'leftalign',
                        field: 'projectId',
                        visible:false,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            ' <span>{{row.entity.projectId}}</span>' +

                            '</div>',


                    },
                    //{
                    //    name: 'Date',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'rightalign',
                    //    cellClass: 'rightalign',
                    //    field: 'date',

                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\" >' +
                    //        ' <span>{{row.entity.date| date:dd/ MM / yyyy}}</span>' +

                    //        '</div>',
                    //    width: 130,

                    //},
                 
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        getdata();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getdata();
                    });
                },
                data: []
            };

           
           
            var init = function () {
               
                if ($stateParams != null) {
                    vm.task.id = $stateParams.pid;
                    vm.requestParams.projectId = vm.task.id;
                    getdata();
                }
                
                getProject();
                
            }
            init();
           
        }
    ]);
})();