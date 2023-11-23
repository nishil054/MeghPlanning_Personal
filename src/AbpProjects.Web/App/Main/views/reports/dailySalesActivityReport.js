(function () {
    angular.module('app').controller('app.views.reports.dailySalesActivityReport', [
        '$scope', '$state', '$http','$uibModal', 'uiGridConstants', 'abp.services.app.opportunityService',
        function ($scope, $state, $http, $uibModal,uiGridConstants, opportunityService) {
            var vm = this;
            vm.opportunity = [];
            vm.companyName = "";

            var date = new Date();
            vm.toDate = moment(date);
            vm.fromDate = moment(date);

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                toDate: vm.toDate,
                fromDate: vm.fromDate,
            };
          
            //vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            //vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            //vm.toDate = moment(vm.lastDay);
            //vm.fromDate = moment(vm.firstDay);

           

            vm.getAll = function () {
                abp.ui.setBusy();
                vm.loading = true;
                opportunityService.dailySalesActivityReport($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.opportunity = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                            $scope.noData = true;
                            vm.isChecked = true;
                        } else { $scope.noData = false; vm.isChecked = false; }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });

            }
            function getCallCategory() {
                opportunityService.getCallCategory({}).then(function (result) {
                    vm.callCategory = result.data.items;
                });
            }
            function getMarketingUsers() {
                opportunityService.getMarketingUsers({}).then(function (result) {
                    vm.assignUsers = result.data.items;
                });
            }

            vm.search = function (n) {
                vm.skipCount = n;
                if (vm.calllCategoryId == "") {
                    vm.requestParams.calllCategoryId = null;
                } else {
                    vm.requestParams.calllCategoryId = vm.calllCategoryId;
                }
                if (vm.assignUserId == "") {
                    vm.requestParams.assignUserId = null;
                } else {
                    vm.requestParams.assignUserId = vm.assignUserId;
                }
                if (vm.companyName == undefined || vm.companyName == "") {
                    vm.requestParams.companyName = null;
                } else {
                    vm.requestParams.companyName = vm.companyName;
                }
                if (vm.fromDate != null) {
                    vm.requestParams.fromDate = vm.fromDate;

                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.toDate != null) {
                    vm.requestParams.toDate = vm.toDate;
                } else {
                    vm.requestParams.toDate = null;
                }
                vm.getAll();
            };
            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.clear = function () {
                vm.companyName = null;
                vm.calllCategoryId = "";
                vm.assignUserId = "";
                vm.requestParams.companyName = null;
                vm.requestParams.calllCategoryId = null;
                vm.requestParams.assignUserId = null;
                $("#ddlCategory").select2("val", null);
                $("#ddlassignUsers").select2("val", null);
                vm.toDate = null;
                vm.fromDate = null
                vm.requestParams.toDate = null;
                vm.requestParams.fromDate = null;
                vm.toDate = moment(date);
                vm.fromDate = moment(date);
                if (vm.fromDate != null) {
                    vm.requestParams.fromDate = vm.fromDate;

                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.toDate != null) {
                    vm.requestParams.toDate = vm.toDate;
                } else {
                    vm.requestParams.toDate = null;
                }
                vm.getAll();
            };
            vm.reasonView = function (data) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/opportunity/reasonView.cshtml',
                    controller: 'app.views.opportunity.reasonView as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return data.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });
                modalInstance.result.then(function () {
                    vm.getAll();
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
                        name: 'Lead Owner',
                        field: 'assignUserName',
                        enableColumnMenu: false,
                        width: 120,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.assignUserName}}' +
                            '</div>'
                    },
                                 
                    {
                        name: 'Person Name ',
                        field: 'personName',
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.personName}}' +
                            '</div>'
                    },
                    {
                        name: 'Company Name',
                        field: 'companyName',
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' + '<a ng-click="grid.appScope.openDetailsViewModal(row.entity)" class="gridlink">{{row.entity.companyName}}</a>' +
                            '</div>'
                    },
                    {
                        name: 'Email Id ',
                        field: 'emailId',

                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.emailId}}' +
                            '</div>'
                    },
                    {
                        name: 'Mobile Number',
                        field: 'mobileNumber',
                        width: 135,
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.mobileNumber}}' +
                            '</div>'
                    },
                    {

                        name: 'Total FollowUp',
                        field: 'followupCount',
                        width: 135,
                        enableColumnMenu: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '<div><a ng-show="row.entity.followupCount != 0" ng-click="grid.appScope.details(row.entity)"> {{row.entity.followupCount}} </a>' +
                            '<div><span ng-show="row.entity.followupCount == 0">{{row.entity.followupCount}}</span>' +
                            //'<div><a ng-show="row.entity.followUpCount == 0"> {{row.entity.followUpCount}} </a>' +
                            //'<a ng-show="row.entity.followUpCount != 0" ng-click="grid.appScope.details(row.entity)"> {{row.entity.followUpCount}} </a>' +
                            //'<ng-show="row.entity.followUpCount == 0"> {{row.entity.followUpCount}}' +
                            '</div>'
                    },
                    {
                        name: 'Call Category',
                        field: 'callCategoryName',
                        enableColumnMenu: false,
                        width: 170,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.callCategoryName}}' +
                            '</div>'
                    },
                    //{
                    //    name: 'Call Category',
                    //    field: 'callCategoryName',
                    //    enableColumnMenu: false,
                    //    width: 120,
                    //    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                    //        '<a ng-show=\"row.entity.calllCategoryId==1 || row.entity.calllCategoryId==5\" ng-click="grid.appScope.reasonView(row.entity)" class=\"culserpoint\"> {{ row.entity.callCategoryName }}</a>' +
                    //        '<span ng-show=\"row.entity.calllCategoryId!=1 && row.entity.calllCategoryId!=5\">{{ row.entity.callCategoryName }}</span>' +
                    //        '</div>'
                    //},
                    {
                        name: App.localize('Interested in'),
                        enableColumnMenu: false,
                        field: 'companyName',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{ grid.appScope.bindrolename(row.entity.projectType_Name) }}' +
                            '</div>'
                    },
                    //{
                    //    name: "Followup Count",
                    //    enableColumnMenu: false,
                    //    field: 'followupCount',
                    //    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                    //        '{{ row.entity.followupCount}}' +
                    //        '</div>'
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


            vm.details = function (data) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/opportunity/view.cshtml',
                    controller: 'app.views.opportunity.view as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return data.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });
                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.exportExcel = function () {

                if (vm.calllCategoryId == "" || vm.calllCategoryId == undefined) {
                    vm.requestParams.calllCategoryId = null;
                } else {
                    vm.requestParams.calllCategoryId = vm.calllCategoryId;
                }
                if (vm.assignUserId == "" || vm.assignUserId == undefined) {
                    vm.requestParams.assignUserId = null;
                } else {
                    vm.requestParams.assignUserId = vm.assignUserId;
                }
                if (vm.companyName == undefined || vm.companyName == "") {
                    vm.requestParams.companyName = null;
                } else {
                    vm.requestParams.companyName = vm.companyName;
                }
                if (vm.fromDate != null) {
                    vm.requestParams.fromDate = vm.fromDate._d;

                } else {
                    vm.requestParams.fromDate = null;
                }
                if (vm.toDate != null) {
                    vm.requestParams.toDate = vm.toDate._d;
                } else {
                    vm.requestParams.toDate = null;
                }



                var url = "../exportToExcel/DailySalesActivityReportExport";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        calllCategoryId: vm.requestParams.calllCategoryId,
                        assignUserId: vm.requestParams.assignUserId,
                        toDate: vm.requestParams.toDate,
                        fromDate: vm.requestParams.fromDate,

                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });


            };




            vm.bindrolename = function (rolenamelist) {
                var projectType_Name = "";
                angular.forEach(rolenamelist, function (v1, k1) {
                    projectType_Name += v1 + ",";
                });
                return projectType_Name.substring(0, projectType_Name.length - 1);
            }

            vm.openDetailsViewModal = function (data) {
                $state.go('opportunityDetails', { id: data.id});
             };

            init = function () {
                vm.isChecked = true;
                getCallCategory();
                getMarketingUsers();
                vm.getAll();
            }

            init();
        }
    ]);
})();