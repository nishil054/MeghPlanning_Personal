(function () {
    angular.module('app').controller('app.views.opportunity.inquiry', [
        '$scope', '$state', '$http', '$timeout', '$uibModal', 'uiGridConstants', 'abp.services.app.opportunityService',
        function ($scope, $state, $http, $timeout, $uibModal, uiGridConstants, opportunityService) {
            var vm = this;
            vm.opportunity = [];
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };
            vm.openOpportunityCreationModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/opportunity/addOpportunity.cshtml',
                    controller: 'app.views.opportunity.addOpportunity as vm',
                    backdrop: 'static'
                });
                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });
                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            vm.openOpportunityEditModal = function (opportunity) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/opportunity/editOpportunity.cshtml',
                    controller: 'app.views.opportunity.editOpportunity as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return opportunity.id;
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

            vm.getAll = function () {
                abp.ui.setBusy();
                vm.loading = true;
                opportunityService.getOpportunityList($.extend({}, vm.requestParams))
                    .then(function (result) {
                        vm.opportunity = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                            $scope.noData = true;
                        } else { $scope.noData = false; }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });

            }
            function getCallCategory() {
                opportunityService.getCallCategoryInq({}).then(function (result) {
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
                vm.getAll();
            };

            vm.clear = function () {
                vm.companyName = null;
                vm.calllCategoryId = "";
                vm.assignUserId = "";
                vm.requestParams.companyName = null;
                vm.requestParams.calllCategoryId = null;
                vm.requestParams.assignUserId = null;
                $("#ddlCategory").select2("val", null);
                //$("#ddlassignUsers").select2("val", null);
                vm.getAll();
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

            vm.viewData = null;

            vm.addFollowUp = function (data) {
                if (data == null) {
                    vm.viewData = null;
                } else {
                    vm.viewData = data;
                    $state.go('addFollow', { id: data.id });
                }
            };

            //vm.addFollowUp = function (data) {
            //    var modalInstance = $uibModal.open({
            //        templateUrl: '/App/Main/views/opportunity/addFollow.cshtml',
            //        controller: 'app.views.opportunity.addFollow as vm',
            //        backdrop: 'static',
            //        resolve: {
            //            id: function () {
            //                return data.id;
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
                        '      <li ng-show=\"row.entity.calllCategoryId!=1 && row.entity.calllCategoryId!=5\"><a ng-click="grid.appScope.openOpportunityEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.details(row.entity)">' + App.localize('Details') + '</a></li>' +
                        '      <li ng-show=\"row.entity.calllCategoryId!=1 && row.entity.calllCategoryId!=5 && row.entity.calllCategoryId!=6\"><a ng-click="grid.appScope.addFollowUp(row.entity)">' + App.localize('AddFollowup') + '</a></li>' +
                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },
                {
                    name: 'Company',
                    field: 'companyName',
                    width: 180,
                    enableColumnMenu: false,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.companyName}}' +
                        '</div>'
                },
                {
                    name: 'Company Beneficiary',
                    field: 'beneficiaryCompany',
                    width: 180,
                    enableColumnMenu: false,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.beneficiaryCompany}}' +
                        '</div>'
                },
                {
                    name: 'Person Details',
                    field: 'personName',
                    enableColumnMenu: false,
                    width: 230,
                    //cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                    //    '{{row.entity.personName}}' +
                    //    '</div>'
                    cellTemplate: '<div><p ng-if="row.entity.personName != null && row.entity.personName !=\'\'"><b>Person Name : </b><span>{{row.entity.personName}} </span></p><p ng-if="row.entity.emailId != null && row.entity.emailId !=\'\'"></p><p><b>Email Id : </b><span>{{row.entity.emailId}} </span></p><p ng-if="row.entity.mobileNumber != null && row.entity.mobileNumber !=\'\'"><b>Mobile : </b><span>{{row.entity.mobileNumber}} </span></p></div>' +
                        '</div>',
                },
                {
                    name: App.localize('Interested in'),
                    enableColumnMenu: false,
                    field: 'projectType_Name',
                    width: 125,
                    enableSorting: false,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{ grid.appScope.bindrolename(row.entity.projectType_Name) }}' +
                        '</div>'
                },
                {
                    name: 'Project Value',
                    field: 'projectValue',
                    enableColumnMenu: false,
                    width: 117,
                    cellClass: 'rightalign',
                    headerCellClass: 'rightalign',
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.projectValue}}' +
                        '</div>'
                },
                //{
                //    name: 'Email Id ',
                //    field: 'emailId',
                //    width: 130,
                //    enableColumnMenu: false,
                //    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                //        '{{row.entity.emailId}}' +
                //        '</div>'
                //},
                //{
                //    name: 'Mobile Number',
                //    field: 'mobileNumber',
                //    width: 135,
                //    enableColumnMenu: false,
                //    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                //        '{{row.entity.mobileNumber}}' +
                //        '</div>'
                //},
                {
                    name: 'Call Category',
                    field: 'callCategoryName',
                    enableColumnMenu: false,
                    width: 120,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '<a ng-show=\"row.entity.calllCategoryId==1 || row.entity.calllCategoryId==5\" ng-click="grid.appScope.reasonView(row.entity)" class=\"culserpoint\"> {{ row.entity.callCategoryName }}</a>' +
                        '<span ng-show=\"row.entity.calllCategoryId!=1 && row.entity.calllCategoryId!=5\">{{ row.entity.callCategoryName }}</span>' +
                        '</div>'
                },

                {
                    name: 'Closed Amount',
                    field: 'closedAmount',
                    enableColumnMenu: false,
                    width: 110,
                    cellClass: 'rightalign',
                    headerCellClass: 'rightalign',
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '<span ng-show=\"row.entity.calllCategoryId==6\">{{ row.entity.closedAmount }}</span>' +
                        '<span ng-show=\"row.entity.calllCategoryId!=6\"> </span>' +
                        '</div>'
                },
                {
                    name: 'Closing Date',
                    field: 'expectedClosingDate',
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    width: 100,
                    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                },
                    //{
                    //    name: 'Reason',
                    //    field: 'reason',
                    //    enableColumnMenu: false,
                    //    width: 110,
                    //    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                    //        '{{row.entity.reason}}' +
                    //        '</div>'
                    //},

                    //{
                    //    name: 'Creation On',
                    //    field: 'createdOn',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    width: 90,
                    //    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
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

            vm.bindrolename = function (rolenamelist) {
                var projectType_Name = "";
                angular.forEach(rolenamelist, function (v1, k1) {
                    projectType_Name += v1 + ",";
                });
                return projectType_Name.substring(0, projectType_Name.length - 1);
            }

            init = function () {
                getCallCategory();
                //getMarketingUsers();
                vm.getAll();
            }

            init();
        }
    ]);
})();