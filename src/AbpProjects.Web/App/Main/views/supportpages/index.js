(function () {
    angular.module('app').controller('app.views.supportpages.index', [
        '$scope', '$state', '$timeout', '$uibModal', 'abp.services.app.support', 'uiGridConstants', 'abp.services.app.masterList',
        function ($scope, $state, $timeout, $uibModal, supportService, uiGridConstants, masterListservice) {
            var vm = this;

            vm.x = [];
            vm.supports = [];
            vm.task = {};
            vm.sname = [];
            vm.tname = [];
            vm.client = [];
            vm.emp = [];



            vm.permissions = {
                adminwrites: abp.auth.hasPermission('Pages.Support.Admin'),
                employeewrites: abp.auth.hasPermission('Pages.Support.Employee'),

            };
            vm.invoice = abp.auth.isGranted('Pages.Service.Generate.Invoice.Request');

            vm.chckinvoicerequestpermission = function () {

                if (vm.invoice) {

                    return true;
                } else {

                    return false;

                }

            }
            vm.save = function (t) {
                abp.message.confirm(
                    "Are you sure you want to cancel?",
                    "Cancel?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            supportService.updateDashboardService(t.id)
                                .then(function () {
                                    abp.notify.info("Cancel: " + t.domainName);
                                    getServiceMgt();
                                    getEmployeeNameActive();
                                });
                        }
                    });


            };

            vm.selecteddomainname = function (selected, addActivity) {
                
                if (selected) {
                    vm.domainName = selected.originalObject.domainName;
                }
                else {
                    vm.domainName = "";
                    //console.log(vm.domainName);
                }
            };
            vm.searchAPI = function (userInputString, timeoutPromise) {
                vm.domainName = userInputString;
              
                return supportService.getDomainNameList(userInputString).then(function (result) {
                    return result.data;
                    
                });
               
            };

            function getServiceMgt() {
                
                abp.ui.setBusy();
                supportService.getServiceMgt($.extend({ Filter: vm.nameFilter }, vm.requestParams))
                    .then(function (result) {
                        vm.supports = result.data.items;
                        for (i = 0; i < result.data.items.length; i++) {
                            var d = result.data.items[i].price.toLocaleString('en-IN');
                            vm.price = d;

                            //console.log(d);
                        }


                        vm.totalRecord = result.data.totalCount;

                        

                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        //console.log(vm.supports);
                        if (vm.supports == 0) {
                            $scope.noData = true;
                        } else {
                            $scope.noData = false;
                        }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }
          
            vm.search = function () {
                abp.ui.setBusy();
                if (vm.task.serviceId == null || vm.task.serviceId == "" || vm.task.serviceId == undefined) {
                    vm.requestParams.serviceId = null;
                } else {
                    vm.requestParams.serviceId = vm.task.serviceId;
                }
                
                //vm.domainName
                if (vm.domainName == null || vm.domainName == "" || vm.domainName == undefined) {
                    vm.requestParams.domainName = "";
                } else {
                    vm.requestParams.domainName = vm.domainName;
                }
                if (vm.task.clientId == null || vm.task.clientId == "" || vm.task.clientId == undefined) {
                    vm.requestParams.clientId = null;
                } else {
                    vm.requestParams.clientId = vm.task.clientId;
                }
                if (vm.task.employeeId == null || vm.task.employeeId == "" || vm.task.employeeId == undefined) {
                    vm.requestParams.employeeId = null;
                } else {
                    vm.requestParams.employeeId = vm.task.employeeId;
                }
                if (vm.task.cancelflag == null || vm.task.cancelflag == "" || vm.task.cancelflag == undefined) {
                    vm.requestParams.cancelflag = null;
                } else {
                    vm.requestParams.cancelflag = vm.task.cancelflag;
                }
                if (vm.task.fromdate == null || vm.task.fromdate == "" || vm.task.fromdate == undefined) {
                    vm.requestParams.fromDate = null;
                } else {
                    vm.requestParams.fromDate = vm.task.fromdate;
                }
                if (vm.task.todate == null || vm.task.todate == "" || vm.task.todate == undefined) {
                    vm.requestParams.toDate = null;
                } else {
                    vm.requestParams.toDate = vm.task.todate;
                }
                if (vm.task.typeName == null || vm.task.typeName == "" || vm.task.typeName == undefined) {
                    vm.requestParams.typeName = null;
                } else {
                    vm.requestParams.typeName = vm.task.typeName;
                }
                supportService.getServiceMgt(
                    $.extend({ domainName: vm.domainName}, vm.requestParams)
                ).then(function (result) {
                    vm.supports = result.data.items;
                    vm.totalRecord = result.data.totalCount;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    //console.log(result);
                    if (result.data.totalCount == 0) {
                        $scope.noData = true;
                        //$scope.record = false;
                    } else {
                        $scope.noData = false;
                        //$scope.record = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            function getServiceName() {
                supportService.getServiceName({}).then(function (result) {
                    vm.sname = result.data.items;
                    //console.log(vm.sname);
                });
            }

            function getTypeName() {
                masterListservice.getTypeName({}).then(function (result) {
                    vm.tname = result.data.items;
                    //console.log(vm.tname);
                });
            }

            function getClientNameActive() {
                supportService.getClientNameActive({}).then(function (result) {
                    vm.client = result.data.items;
                    //console.log(vm.client);
                });
            }

            function getEmployeeNameActive() {
                supportService.getUserName().then(function (result) {
                    vm.emp = result.data;
                });
            }


            $scope.clearSearch = function () {
                
                vm.requestParams.fromDate = null;
                vm.requestParams.toDate = null;
                vm.requestParams.serviceId = null;
                vm.requestParams.typeName = null;
                vm.requestParams.clientId = null;
                vm.requestParams.employeeId = null;
                vm.requestParams.cancelflag = false;
                vm.requestParams.domainName = null;
                vm.task.fromdate = null;
                vm.task.todate = null;
                $("#s2id_ddlcom").select2("val", null);
                $("#s2id_ddlcomType").select2("val", null);
                $("#s2id_ddlcom1").select2("val", null);
                $("#s2id_ddlcomEmp").select2("val", null);
                
                vm.task.cancelflag = "false";
                vm.task.domainName = "";
                $("#DomainName_value").val("");
                vm.domainName = "";
                getServiceName();
                getTypeName();
                getClientNameActive();
                getEmployeeNameActive();
                getServiceMgt();
            }


            vm.openCreateServiceModal = function () {

                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/supportpages/insert.cshtml',
                    controller: 'app.views.supportpages.insert as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    init();
                });
            };
            vm.openServiceHistoryModal = function (history) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/supportpages/history.cshtml',
                    controller: 'app.views.supportpages.history as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return history.id;

                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {


                });
            }


            vm.openServiceEditModal = function (support) {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/supportpages/edit.cshtml',
                    controller: 'app.views.supportpages.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return support.id;

                        },

                        famount: function () {
                            return false;

                        },
                        fdash: function () {
                            return false;
                            // return support.a = true;
                        },
                        fid: function () {
                            return true;

                        }
                    }
                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    getServiceMgt();
                });
            };

            vm.openServiceRenewModal = function (support) {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/supportpages/edit.cshtml',
                    controller: 'app.views.supportpages.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return support.id;

                        },
                        famount: function () {
                            return false;
                            // return support.a = true;
                        },
                        fdash: function () {
                            return false;
                            // return support.a = true;
                        },
                        fid: function () {
                            return true;
                            // return support.a = true;
                        }
                    }
                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    vm.task.cancelflag = "false";
                    getServiceMgt();
                });
            };

            vm.openServiceAdjustmentModal = function (support) {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/supportpages/edit.cshtml',
                    controller: 'app.views.supportpages.edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return support.id;

                        },
                        famount: function () {
                            return true;
                            // return support.a = true;
                        },
                        fdash: function () {
                            return false;
                            // return support.a = true;
                        },
                        fid: function () {
                            return false;
                            // return support.a = true;
                        }
                    }
                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    vm.task.cancelflag = "false";
                    getServiceMgt();
                });
            };

            vm.openServiceDetailModal = function (support) {

                $state.go('detail', { id: support.id });

            };

            vm.openInvoiceCreationModal = function (support) {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/supportpages/generateinvoice.cshtml',
                    controller: 'app.views.supportpages.generateinvoice as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return support.id;

                        },


                    }
                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    vm.getAll();
                    //init();
                });
            };

            vm.delete = function (support) {
                abp.message.confirm(
                    "Delete Service '" + support.domainName + "'?",
                    "Delete?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            supportService.deleteService(support.id)
                                .then(function () {
                                    abp.notify.info("Deleted Service: " + support.domainName);
                                    getServiceMgt();
                                    getEmployeeNameActive();
                                });
                        }
                    });
            }

            vm.refreshGrid = function (n) {
                skipCount = n;
                vm.task.serviceId = null;
                getServiceMgt();
                getEmployeeNameActive();

            };
            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: [50,100,150,200],
                paginationPageSize: 50,
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
                    visible: vm.permissions.adminwrites == true ? true : false,
                    width: 65,
                    cellTemplate: '<div class=\"ui-grid-cell-contents padd0\">' +
                        '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                        '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                        '    <ul uib-dropdown-menu>' +
                        '      <li><a ng-click="grid.appScope.openServiceEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' + 
                        '      <li><a ng-click="grid.appScope.openServiceDetailModal(row.entity)" ng-show="row.entity.cancelflag==1">' + App.localize('Detail') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.openServiceRenewModal(row.entity)" ng-show="row.entity.cancelflag==1">' + App.localize('Renew') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.openServiceAdjustmentModal(row.entity)" ng-show="row.entity.cancelflag==0">' + App.localize('Adjustment') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.save(row.entity)" ng-show="row.entity.cancelflag==0">' + App.localize('Cancel') + '</a></li>' +
                        '      <li  ng-if= "grid.appScope.chckinvoicerequestpermission()==true"><a ng-click="grid.appScope.openInvoiceCreationModal(row.entity)"> Generate Invoice Request </a></li>' +
                        '      <li><a ng-click="grid.appScope.openServiceHistoryModal(row.entity)" ng-show="row.entity.cancelflag==0">' + App.localize('History') + '</a></li>' +
                        /*   ng - show="row.entity.cancelflag==1"*/
                    /*&& row.entity.statusName==1*/
                    /*ng-show="row.entity.cancelflag==0 && row.entity.status==0 "*/
                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },

                //{
                //    name: 'Service',
                //    enableColumnMenu: false,
                //    headerCellClass: 'centeralign',
                //    cellClass: 'centeralign',
                //    field: 'serviceName',

                //    cellTemplate:
                //        '<div class=\"ui-grid-cell-contents\">' +
                //        ' <span>{{row.entity.serviceName}}</span><br/>' +

                //        '</div>',
                //     width: 90,
                //    height: 180,
                //},
                //{
                //    name: 'Client',
                //    enableColumnMenu: false,
                //    headerCellClass: 'leftalign',
                //    cellClass: 'leftalign',
                //    field: 'clientName',

                //    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"float: left\">' +
                //        ' <span>{{row.entity.clientName}}</span>' +

                //        '</div>',
                //},
                {
                    name: 'Domain Name',
                    enableColumnMenu: false, 
                    //headerCellClass: 'centeralign',
                    //cellClass: 'centeralign',
                    field: 'domainName',

                    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"float: left\">' +
                        /*' <span>{{row.entity.domainName}}</span>' +*/
                        ' <div><p ng-if="row.entity.domainName!=null"><b>Domain Name:&nbsp;<i ng-if="row.entity.isAutoRenewal==true" style="color:green" class="fa fa-refresh" aria-hidden="true"></i></b>&nbsp;&nbsp;<span>{{row.entity.domainName}} </span></p><p ng-if="row.entity.clientName!=null"><b>Client Name:</b><span ng-if="row.entity.clientName!=null">{{row.entity.clientName}} </span></p><p ng-if ="row.entity.comment != null"><b>Comment:</b><span style="white-space: pre-wrap; text-indent: 50px;" ng-if="row.entity.comment!=null">{{row.entity.comment}} </span></p></div>' +
                        '</div>',
                    width: 390,
                },
                //{
                //    name: 'Service Detail',
                //    enableColumnMenu: false,
                //    headerCellClass: 'leftalign',
                //    cellClass: 'leftalign',
                //    field: 'domainName',
                //    cellTemplate: '<div class=\"ui-grid-cell-contents \">' +
                //        ' <div><span>{{row.entity.domainName}} </span></div><div ng-if="row.entity.serviceName!=null"><b>Service Type:</b><span>{{row.entity.serviceName}} </span></div><div ng-if="row.entity.hostingSpace!=null"><b>Hosting Space:</b><span ng-if="row.entity.hostingSpace!=null">{{row.entity.hostingSpace}} </span></div><div ng-if="row.entity.displayTypename != null"><b>Type Name:</b><span>{{row.entity.displayTypename}} </span></div><div ng-if="row.entity.noOfEmail!=null"><b>No of email:</b><span>{{row.entity.noOfEmail}} </span></div><div ng-if="row.entity.serverName!=null"><b>Server Type:</b><span>{{row.entity.serverName}} </span></div>' +
                //        '</div>',
                //    //width: 180,
                //},
                {
                    name: 'Service Detail',
                    enableColumnMenu: false,
                    headerCellClass: 'leftalign',
                    cellClass: 'leftalign',
                    field: 'domainName',
                    cellTemplate: '<div class=\"ui-grid-cell-contents \">' +
                            ' <div><p ng-if="row.entity.serviceName!=null"><b>Service Type:</b><span>{{row.entity.serviceName}} </span></p>' 
                        + '<p ng-if= "row.entity.hostingSpace!=null" > <b>Storage Space:</b><span ng-if="row.entity.hostingSpace!=null">{{ row.entity.hostingSpace }} </span></p>' +
                        ' <p ng-if="row.entity.databaseSpace != null"><b>Database Space:</b><span ng-if="row.entity.databaseSpace!=null">{{ row.entity.databaseSpace }} </span></p>'
                        + '<p ng-if="row.entity.displayTypename != null"><b>Type Name:</b><span>{{ row.entity.displayTypename }} </span></p>'
                        + '<p ng-if="row.entity.noOfEmail!=null"><b>No of email:</b><span>{{ row.entity.noOfEmail }} </span></p>'
                        + '<p ng-if="row.entity.serverName!=null"><b>Server Type:</b><span>{{ row.entity.serverName }} </span></p>'
                        + '<p ng-if="row.entity.typeofssl!=null"><b>Type Of SSL:</b><span ng-if="row.entity.typeofssl!=null">{{ row.entity.typeofssl }} </span></p>'
                        + '<p ng-if="row.entity.title!=null"><b>Title:</b><span ng-if="row.entity.title!=null">{{ row.entity.title }} </span></p>'
                        + '<p ng-if="row.entity.credits!=null"><b>Credits:</b><span>{{ row.entity.credits }} </span></p>' +
                        '</div>',
                    
                    width: 180,
                },
                {
                    name: 'Price',
                    enableColumnMenu: false,
                    headerCellClass: 'rightalign',
                    cellClass: 'rightalign',
                    field: 'price',

                    cellTemplate: '<div class=\"ui-grid-cell-contents pecal1 gridcolumn\" style=\"float: right\">' +
                        ' <div><span>{{row.entity.price}}</span></div>' +

                        '</div>',
                    width: 80,

                },
                {
                    name: ' Registration Date',
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    field: 'registrationDate',
                    cellFilter: 'momentFormat: \'DD/MMM/YYYY\'',
                    //cellTemplate:
                    //    '<div class=\"ui-grid-cell-contents\">' +
                    //    ' <span>{{row.entity.createtime}}</span>' +
                    //    '</div>',
                    width: 135,
                },
                {
                    name: ' Renewal Date',
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    field: 'nextRenewalDate',
                    cellFilter: 'momentFormat: \'DD/MMM/YYYY\'',
                    //cellTemplate:
                    //    '<div class=\"ui-grid-cell-contents\">' +
                    //    ' <span>{{row.entity.createtime}}</span>' +
                    //    '</div>',
                    width: 110,
                },

                {
                    name: 'Account Manager',
                    enableColumnMenu: false,
                    //headerCellClass: 'leftalign',
                    //cellClass: 'centeralign',
                    field: 'employeeName',

                    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"float: left\">' +
                        ' <span>{{row.entity.employeeName}}</span>' +

                        '</div>',
                    width: 150,
                },
                //{
                //    name: 'Action Name',
                //    enableColumnMenu: false,
                //    headerCellClass: 'centeralign',
                //    cellClass: 'centeralign',
                //    field: 'cancelflag',

                //    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +

                //        ' <span>{{row.entity.actionName}}</span>' +

                //        '</div>',
                //    width: 30,
                //    cellClass: 'centeralign'
                //},
                {
                    name: 'Status',
                    enableColumnMenu: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    field: 'cancelflag',

                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +

                        ' <span ng-if="row.entity.cancelflag==0" class=\"badge badge-success"\>Active </span>' +
                        ' <span ng-if="row.entity.cancelflag==1" class=\"badge badge-danger"\> Cancel  </span>' +
                        '</div>',
                    width: 70,
                    cellClass: 'centeralign'
                },
                    //{
                    //    name: 'HostingSpace',
                    //    enableColumnMenu: false,
                    //    headerCellClass: 'centeralign',
                    //    cellClass: 'centeralign',
                    //    field: 'hostingSpace',

                    //    cellTemplate:
                    //        '<div class=\"ui-grid-cell-contents\">' +
                    //        ' <span>{{row.entity.hostingSpace}}</span>' +

                    //        '</div>',
                    //    width: 130,
                    //    cellClass: 'centeralign'
                    //},
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "nextRenewalDate asc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        getServiceMgt();
                        getEmployeeNameActive();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        getServiceMgt();
                        getEmployeeNameActive();
                    });
                },
                data: []
            };

            var init = function () {
                vm.requestParams = {

                    skipCount: 0,
                    maxResultCount: 50,
                    sorting: "nextRenewalDate asc",
                    serviceId: vm.task.serviceId,
                    domainName: vm.task.domainName,
                    clientId: vm.task.clientId,
                    employeeId: vm.task.employeeId,
                    cancelflag: vm.task.cancelflag,
                    fromDate: vm.task.fromdate,
                    toDate: vm.task.todate,
                    cancelflag: "false",
                    typeName: vm.task.typeName

                };
                vm.task.cancelflag = "false";
                getServiceName();
                getClientNameActive();
                getEmployeeNameActive();
                getTypeName();
                getServiceMgt();
            }
            init();

        }
    ]);
})();