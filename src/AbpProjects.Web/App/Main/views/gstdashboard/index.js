(function () {
    angular.module('app').controller('app.views.gstdashboard.index', [
        '$scope', '$state', '$timeout', '$uibModal', 'abp.services.app.project', 'abp.services.app.masterList', 'uiGridConstants', 'abp.services.app.gSTDashboard',
        function ($scope, $state, $timeout, $uibModal, projectService, masterListservice, uiGridConstants, gSTDashboardService) {
            //debugger;
            var vm = this;
            //var month = date.getMonth();
            $scope.nodata = true;
            vm.gstdashboard = {};
            vm.gstdashboard.status = "0";
            //vm.gstdashboard.month = "April";
            /* vm.gstdashboard.financialyearId = 3;*/
            var months = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""];
            vm.financialyearlist = [];
            vm.companylist = [];
            vm.financialyearlist = [];
            vm.loading = false;
            vm.searchBox = "";
            vm.showteamColumn = "false";
            vm.ishown = false;
            //vm.userGridOptions.enablePaginationControls = false;

            vm.label = "";
            var year = (new Date().getFullYear()).toString();
            var perviousyear = year - 1;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };


            function getCompanyData() {
                gSTDashboardService.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;
                        vm.gstdashboard.companyId = vm.companylist[0].companyId.toString();
                        vm.requestParams.financialyearId = vm.gstdashboard.financialyearId;
                        vm.requestParams.companyId = vm.gstdashboard.companyId;
                        getGstDataList();
                    });
            }

            function getMonths() {
                gSTDashboardService.getMonth().then(function (result) {
                    vm.monthlist = result.data;
                });
            }

            function getFinancialyearData() {
                getCompanyData();
                masterListservice.getFinancialYear()
                    .then(function (result) {
                        vm.financialyearlist = result.data;
                        angular.forEach(vm.financialyearlist, function (value, key) {
                            if (value.endYear == year) {
                                vm.gstdashboard.previousyear = value.endYear - 1;
                                vm.gstdashboard.financialyearId = value.id.toString();

                                vm.requestParams.financialyearId = vm.gstdashboard.financialyearId;
                                vm.requestParams.companyId = vm.gstdashboard.companyId;
                                getyear(vm.requestParams.financialyearId);
                                getGstDataList();
                            }
                        });

                    });
            }
            vm.save = function () {
                abp.ui.setBusy();

                gSTDashboardService.createService(vm.gstdashboard).then(function () {


                    abp.notify.info(App.localize('SavedSuccessfully'));
                    /*$uibModalInstance.close();*/
                    

                    vm.gstdashboard.inputGST = "";
                    vm.gstdashboard.totalPayableGST = "";
                    vm.gstdashboard.totalPendingPayment = "";


                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                    getGstDataList();
                });
                //}

            };
            function getGstDataList() {
                //debugger;
                abp.ui.setBusy();
                gSTDashboardService.getGstDataList($.extend({}, vm.requestParams)
                ).then(function (result) {
                    //debugger;

                    vm.gstDataList = result.data;
                    vm.totalRecord = result.data.totalCount;
                    vm.userGridOptions.totalItems = result.data;



                    if (result.data.length != 0) {

                        vm.gstdashboard.outputGST = vm.gstDataList[0].totalgst;
                        vm.gstdashboard.monthName = vm.gstDataList[0].monthName;
                        if (vm.gstdashboard.outputGST == null /*&& vm.gstdashboard.outputGST == 0*/) {
                            vm.gstdashboard.outputGST = 0;
                        }
                        else {
                            vm.gstdashboard.outputGST = vm.gstDataList[0].totalgst;
                        }
                    }
                    else {
                        vm.gstdashboard.outputGST = 0;
                        /*vm.gstdashboard.monthName = "";*/
                    }

                    vm.userGridOptions.data = result.data;
                    for (var i = 0; i < vm.userGridOptions.data.length; i++) {

                        vm.userGridOptions.data[i].status = vm.userGridOptions.data[i].status + "";

                    }
                    //getyear(vm.requestParams.financialyearId);
                    if (result.data.length == 0) {
                        if (vm.gstdashboard.endYear > year) {
                            $scope.nodata = false;
                        }
                        else {
                            vm.gstdashboard.monthId = "4";
                            var month = parseInt(vm.gstdashboard.monthId);
                            var date = new Date(2020, month, 21);
                            vm.gstdashboard.monthName = months[date.getMonth()];
                            vm.gstdashboard.startYear = vm.gstdashboard.startYear;
                            var a = vm.gstdashboard.monthName;
                            var b = vm.gstdashboard.startYear;

                            vm.gstdashboard.monthName = a.concat("-", b);
                            getTotalgst(month, vm.requestParams.companyId, vm.requestParams.financialyearId);

                            $scope.nodata = true;
                        }


                        //vm.norecord = true;

                    }
                    else {

                        var monthid = (result.data[result.data.length - 1].monthId);
                        var month = (new Date().getMonth() + 1).toString().slice(-2);
                        if (monthid == 12) {
                            monthid = 1;
                        }
                        else {
                            monthid = monthid;
                        }
                        if (monthid == month && result.data[result.data.length - 1].endYear == parseInt(year)) {
                            $scope.nodata = false;

                        }
                        else {
                            monthid = parseInt(result.data[result.data.length - 1].monthId);

                            if (monthid == 12 /*&& result.data[result.data.length - 1].endYear == year*/) {
                                vm.gstdashboard.monthId = "1";
                                var month = parseInt(vm.gstdashboard.monthId);
                                var date = new Date(2020, month, 21);
                                vm.gstdashboard.monthName = months[date.getMonth()];
                                vm.gstdashboard.startYear = vm.gstdashboard.startYear;

                                vm.gstdashboard.endYear = vm.gstdashboard.endYear;
                                var a = vm.gstdashboard.monthName;
                                var b = vm.gstdashboard.startYear;
                                var c = vm.gstdashboard.endYear;
                                if (month >= 4) {
                                    vm.gstdashboard.monthName = a.concat("-", b);
                                }
                                else {
                                    vm.gstdashboard.monthName = a.concat("-", c);
                                }
                                getTotalgst(month, vm.requestParams.companyId, vm.requestParams.financialyearId);
                                $scope.nodata = true;
                            }
                            else {
                                if (monthid == 3 && result.data[result.data.length - 1].endYear != year) {
                                    $scope.nodata = false;
                                }
                                else {
                                    monthid = monthid + 1;
                                    vm.gstdashboard.monthId = monthid + "";
                                    var month = parseInt(vm.gstdashboard.monthId);
                                    /*var date = new Date(2020, month, 21);*/
                                    vm.gstdashboard.monthName = months[month];
                                    vm.gstdashboard.startYear = vm.gstdashboard.startYear;
                                    vm.gstdashboard.endYear = vm.gstdashboard.endYear;
                                    var a = vm.gstdashboard.monthName;
                                    var b = vm.gstdashboard.startYear;
                                    var c = vm.gstdashboard.endYear;
                                    if (month >= 4) {
                                        vm.gstdashboard.monthName = a.concat("-", b);
                                    }
                                    else {
                                        vm.gstdashboard.monthName = a.concat("-", c);
                                    }

                                    /*vm.gstdashboard.monthName = months[date.getMonth()];*/
                                    getTotalgst(month, vm.requestParams.companyId, vm.requestParams.financialyearId);
                                    $scope.nodata = true;
                                }

                            }

                        }


                    }

                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            function getTotalgst(month, company, finacialyearid) {

                gSTDashboardService.getSum(month, company, finacialyearid)
                    .then(function (result) {
                        if (result.data != null) {
                            vm.gstdashboard.outputGST = result.data;
                        }
                        else {
                            vm.gstdashboard.outputGST = 0;
                        }
                    });
            }


            function getyear(finacialyearid) {

                gSTDashboardService.getStartYear(finacialyearid)
                    .then(function (result) {

                        /* vm.gstdashboard.endyr = result.data*/
                        vm.gstdashboard.yr = result.data;
                        var str = vm.gstdashboard.yr;
                        var yearComma = str.split("-");
                        vm.gstdashboard.startYear = yearComma[0];
                        vm.gstdashboard.endYear = yearComma[1];




                    });
            }

            $scope.getPendingTotal = function () {

                var total = 0;
                for (var i = 0; i < $scope.vm.getGstDataList.length; i++) {
                    var product = $scope.vm.getGstDataList[i].totalPayableGst;
                    total = product + total;
                }
                return total;
            }
            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                getGstDataList();

            };

            $scope.financialyearChange = function (financialyearId) {

                vm.requestParams.financialyearId = financialyearId;
                vm.requestParams.companyId = vm.gstdashboard.companyId;
                getyear(vm.requestParams.financialyearId);

                getGstDataList();
            }
            $scope.comapneynameChange = function (companyId) {

                vm.requestParams.financialyearId = vm.gstdashboard.financialyearId;
                vm.requestParams.companyId = companyId;

                getGstDataList();


            }
            $scope.datachange = function (gst) {
                if (gst == "") {
                    vm.gstdashboard.totalPayableGST = "";
                    vm.gstdashboard.totalPendingPayment = "";

                }
                else {
                    vm.gstdashboard.inputGST = gst;

                    var regdate = vm.gstdashboard.outputGST - vm.gstdashboard.inputGST;
                    if (regdate >= 0) {
                        vm.gstdashboard.totalPayableGST = regdate;
                        vm.gstdashboard.totalPendingPayment = regdate;
                    }
                    else {
                        vm.gstdashboard.totalPayableGST = "";
                        vm.gstdashboard.totalPendingPayment = "";
                    }
                    if (vm.gstdashboard.totalPendingPayment != 0) {

                        vm.gstdashboard.status = "0";
                        angular.element(document.getElementById('drpteam'))[0].disabled = true;
                    }
                    else {
                        vm.gstdashboard.status = "1";
                        angular.element(document.getElementById('drpteam'))[0].disabled = true;
                    }
                }

            }
            vm.updateStatusProject = function (id, status) {

                var postparam = {};

                if (status != "") {
                    postparam.id = id;
                    postparam.updateStatusId = status;
                    try {
                        gSTDashboardService.updateStatuslist(postparam)
                            .then(function () {
                                abp.notify.success("Status updated successfully.");
                                getGstDataList();
                            });
                    } catch (exp) { }
                } else {
                    abp.notify.error("Please Select Status!");
                    status = 0;
                }

            }
            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                /*enablePaginationControls: false,*/
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                enableSorting: false,
                showGridFooter: true,
                showColumnFooter: true,
                useExternalPagination: true,
                useExternalSorting: true,
                enablePaginationControls: false,
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
                        width: 70,
                        cellTemplate: '<div class=\"ui-grid-cell-contents padd0\">' +
                            '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body ng-show="row.entity.status==0">' +
                            '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                            '    <ul uib-dropdown-menu ng-show="row.entity.status==0">' +
                            '      <li><a ng-click="grid.appScope.openGstDashboardEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            /* '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +*/

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },

                    {
                        name: App.localize('Month'),
                        field: 'month',
                        enableColumnMenu: false,
                        width: 170,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.month}}' +
                            '</div>'
                    },

                    {
                        name: "Marketing Lead",
                        field: 'marketingLeadName',
                        enableColumnMenu: false,
                        width: 120,
                        visible: vm.ishown == true,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.marketingLeadName}}' +
                            '</div>'
                    },
                    {

                        name: App.localize('OutputGST'),
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        field: 'outputGST',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        //cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        width: 150
                    },
                    {
                        name: App.localize('InputGST'),
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        field: 'inputGST',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        //cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        width: 150
                    },
                    {
                        name: App.localize('TotalPayableGST'),
                        field: 'totalPayableGST',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        enableColumnMenu: false,
                        width: 150,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',

                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.totalPayableGST}}' +
                            '</div>'
                    },

                    {
                        name: "TotalPendingAmount",
                        field: 'totalPendingPayment',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        enableColumnMenu: false,
                        width: 200,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.totalPendingPayment}}' +
                            '</div>'
                        //cellTemplate: '<div class=\"ui-grid-cell-contents\" ng-if="row.entity.invoiceamount != null || row.entity.pricesum != null ">' +
                        //    '<span ng-if="row.entity.invoiceamount==null">0</span>{{row.entity.invoiceamount}}/<span ng-if="row.entity.pricesum==null">0</span>{{row.entity.pricesum}}' +
                        //    '</div>'
                    },
                    {
                        name: "Status",
                        field: 'status',
                        enableColumnMenu: false,
                        width: 120,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',

                        type: uiGridConstants.filter.SELECT,


                        cellTemplate: '<span class=\"ui-grid-cell-contents\" ng-if="row.entity.status==1">Paid</span>' + '<div class=\"ui-grid-cell-contents\" ng-if="row.entity.status==0">' +
                            '<select id=\"ddlstat-{{row.entity.id}}\"  ng-model="row.entity.status" class="form-control" ng-change="grid.appScope.updateStatusProject(row.entity.id, row.entity.status)" >' +
                            '<option value="">-Select status -</option>' +
                            '<option value="0">Pending </option>' +
                            '<option value="1">Paid</option>' +
                            '</div>'





                    },
                ],

                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    /* $scope.pagination = false;*/
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }
                        getGstDataList();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;
                        getGstDataList();
                    });
                },
                data: []
            };




            vm.openGstDashboardEditModal = function (gstDataList) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/gstdashboard/edit.cshtml',
                    controller: 'app.views.gstdashboard.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return gstDataList.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    getGstDataList();
                });
            };
            vm.delete = function (gstDataList) {
                abp.message.confirm(
                    "Delete Record '" + gstDataList.month + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {
                            gSTDashboardService.deleteGstData({ id: gstDataList.id })
                                .then(function () {
                                    abp.notify.success("Record : " + gstDataList.month + "Deleted");
                                    getGstDataList();
                                });
                        }
                    });
            }

            vm.CheckNumber = function () {
                //console.log(event.keyCode);
                if (event.keyCode === 46) {

                }

                else if (isNaN(event.key) || event.key === ' ' || event.key === '') {
                    event.returnValue = '';
                }
            };

            vm.onChangeStatus = function () {

            }

            init = function () {

                getCompanyData();
                getFinancialyearData();
                getMonths();
                vm.gstdashboard.status = "0";
                angular.element(document.getElementById('drpteam'))[0].disabled = true;
                //vm.requestParams.financialyearId = vm.gstdashboard.financialyearId
                //getyear(vm.requestParams.financialyearId);
            }
            init();
        }
    ]);
})();