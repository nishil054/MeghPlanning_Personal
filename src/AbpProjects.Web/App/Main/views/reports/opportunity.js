(function () {
    angular.module('app').controller('app.views.reports.opportunity', [
        '$scope', '$state', '$uibModal', 'uiGridConstants', 'abp.services.app.opportunityService', '$http', 'abp.services.app.masterList',
        function ($scope, $state, $uibModal, uiGridConstants, opportunityService, $http, masterListservice) {
            var vm = this;
            vm.opportunity = [];
            vm.companylist = [];
            vm.companybeniflag = false;
            //vm.rolenamevalue = null;
           
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                calllCategoryId: vm.calllCategoryId,
                assignUserId: vm.assignUserId,
                companyName: vm.companyName,
                fromDate: vm.fromDate,
                toDate: vm.toDate,
                beneficiaryCompanyId: vm.beneficiaryCompanyId,
            };

            vm.getAll = function () {
                abp.ui.setBusy();
                vm.loading = true;
                
                opportunityService.getOpportunityReport($.extend({}, vm.requestParams))
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
                if (vm.rolenamevalue != null) {
                    if (vm.rolenamevalue == "DubaiManager") {
                        //vm.beneficiaryCompanyId = vm.companylist[0].id + "";
                        getCompanyList();
                    }
                }
                else {
                    vm.getrolename();
                    getCompanyList();
                }
                
                
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

            //Bind Company Details
            function getCompanyList() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;
                        if (vm.rolenamevalue != null) {
                            if (vm.rolenamevalue == "DubaiManager") {
                                //vm.beneficiaryCompanyId = vm.companylist[0].id + "";
                                //vm.requestParams.beneficiaryCompanyId = vm.beneficiaryCompanyId;
                                vm.companylist.forEach((element, index) => {
                                    if (element.beneficial_Company_Name == 'Megh Technologies LLC') {
                                        vm.beneficiaryCompanyId = vm.companylist[index].id + "";
                                        vm.requestParams.beneficiaryCompanyId = vm.beneficiaryCompanyId;
                                        $("#ddlcom").select2("val", vm.beneficiaryCompanyId);
                                    }
                                });
                            }
                        }
                        else {
                            vm.getrolename();
                            getCompanyList();
                        }
                       
                    });
                
            }

            vm.search = function (n) {
                vm.skipCount = n;
                if (vm.calllCategoryId == "") {
                    vm.requestParams.calllCategoryId = null;
                } else {
                    vm.requestParams.calllCategoryId = vm.calllCategoryId;
                }
                if (vm.rolenamevalue != null) {
                    if (vm.rolenamevalue == "DubaiManager") {
                        //if (vm.companylist != null) {
                        //    vm.requestParams.beneficiaryCompanyId = vm.companylist[0].id + "";
                        //}
                        //else {
                            getCompanyList();
                        //}
                    }
                }
                else {
                    vm.getrolename();
                    getCompanyList();
                }
                if (vm.beneficiaryCompanyId == "") {
                    vm.requestParams.beneficiaryCompanyId = null;
                }
                //else if (vm.rolenamevalue == "DubaiManager") {
                //    vm.requestParams.beneficiaryCompanyId = "1";
                //}
                else {
                    vm.requestParams.beneficiaryCompanyId = vm.beneficiaryCompanyId;
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

            vm.exportExcel = function () {
                var datefromdate = vm.fromDate;
                var datetodate = vm.toDate;
                //if (vm.calllCategoryId == "") {
                //    vm.requestParams.calllCategoryId = null;
                //} else {
                //    vm.requestParams.calllCategoryId = vm.calllCategoryId;
                //}
                //if (vm.assignUserId == "") {
                //    vm.requestParams.assignUserId = null;
                //} else {
                //    vm.requestParams.assignUserId = vm.assignUserId;
                //}
                //if (vm.companyName == undefined || vm.companyName == "") {
                //    vm.requestParams.companyName = null;
                //} else {
                //    vm.requestParams.companyName = vm.companyName;
                //}
                if (vm.fromDate != null) {
                    datefromdate = vm.fromDate._d;
                }
                if (vm.toDate != null) {
                    datetodate = vm.toDate._d;
                }

                var url = "../exportToExcel/OpportunityReportExport";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        calllCategoryId: vm.calllCategoryId,
                        assignUserId: vm.assignUserId,
                        companyName: vm.companyName,
                        fromDate: datefromdate,
                        toDate: datetodate,
                        beneficiaryCompanyId: vm.beneficiaryCompanyId,
                    },

                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });

            }

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
                vm.beneficiaryCompanyId = "";
                vm.requestParams.companyName = null;
                vm.requestParams.calllCategoryId = null;
                vm.requestParams.assignUserId = null;
                vm.requestParams.beneficiaryCompanyId = null;
                $("#ddlCategory").select2("val", null);
                $("#ddlassignUsers").select2("val", null);
                $("#ddlcom").select2("val", null);
                vm.toDate = null;
                vm.fromDate = null
                vm.requestParams.toDate = null;
                vm.requestParams.fromDate = null;
                vm.getAll();
                if (vm.rolenamevalue != null) {
                    if (vm.rolenamevalue == "DubaiManager") {
                        //if (vm.companylist != null) {
                        //    vm.requestParams.beneficiaryCompanyId = vm.companylist[0].id + "";
                        //    $("#ddlcom").select2("val", null);
                        //}
                        //else {
                            getCompanyList();
                        //}
                    }
                }
                else {
                    vm.getrolename();
                    getCompanyList();
                }
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
                        name: 'Company',
                        field: 'companyName',
                        width: 130,
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' + '<a ng-click="grid.appScope.openDetailsViewModal(row.entity)" class="gridlink">{{row.entity.companyName}}</a>' +
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
                        enableSorting: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{ grid.appScope.bindrolename(row.entity.projectType_Name) }}' +
                            '</div>'
                    },
                    {
                        name: 'Project Value',
                        field: 'projectValue',
                        enableColumnMenu: false,
                        width: 110,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectValue}}' +
                            '</div>'
                    },
                    //{
                    //    name: 'Call Category',
                    //    field: 'callCategoryName',
                    //    enableColumnMenu: false,
                    //    width: 170,
                    //    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                    //        '{{row.entity.callCategoryName}}' +
                    //        '</div>'
                    //},
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
                        width: 95,
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
                    //    name: 'Creation Date',
                    //    field: 'createDate',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    width: 105,
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

            vm.openDetailsViewModal = function (data) {
                $state.go('opportunityDetails', { id: data.id });
            };

            vm.getrolename = function () {
                masterListservice.getRoleName()
                    .then(function (result) {
                        vm.rolenamevalue = result.data;
                    });
            }

            init = function ()
            {
                vm.getrolename();
                vm.isChecked = true;
                getCallCategory();
                getCompanyList();
                getMarketingUsers();
                vm.getAll();

            }

            init();
        }
    ]);
})();