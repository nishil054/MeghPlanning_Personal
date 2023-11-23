(function () {
    angular.module('app').controller('app.views.project.invoicecollectionprojects', [
        '$scope', '$state','$http', '$timeout', '$uibModal', 'abp.services.app.project', 'uiGridConstants', 'abp.services.app.support', 'abp.services.app.user',
        function ($scope, $state,$http, $timeout, $uibModal, projectService, uiGridConstants, supportService, userService) {
            var vm = this;
            vm.ddlProjectStatus = [];
            vm.ddlmarketingList = [];
            vm.project = {};
            vm.projectList = [];
            vm.projectTask = [];
            vm.cname = [];
            vm.loading = false;
            vm.searchBox = "";
            vm.showteamColumn = "false";
            vm.ishown = false;
            vm.defaultStatus = []; //[5, 6, 7];
            vm.permissions = {
                adminwrites: abp.auth.hasPermission('Pages.Project.Admin'),
                employeewrites: abp.auth.hasPermission('Pages.Project.Employee'),
                typeshow: abp.auth.hasPermission('Pages.Project.Type'),
                marketLeads: abp.auth.hasPermission('Pages.Project.Marketing'),
            };

            vm.employeewritespermission = function () {
                if (vm.permissions.employeewrites) {
                    return true;
                } else {
                    return false;
                }
            }

            if (abp.auth.hasPermission('Pages.Project.Admin')) {
                vm.ishown = true;
            }

            vm.invoice = abp.auth.isGranted('Projects.Generate.Invoice.Request');
            vm.userstory = abp.auth.isGranted('Pages.UserStory');
            vm.projectadmin = abp.auth.isGranted('Pages.Project.Admin');
            vm.projectMarketingLeaders = abp.auth.isGranted('Pages.Project.Marketing');
            vm.projectmarketingleader = abp.auth.isGranted('Pages.Project');
            vm.projPriority = abp.auth.isGranted('Pages.Update.Project.Priority');
            vm.isEnableDisable = abp.auth.isGranted('Pages.Project.TimeSheetEnableDisable');
            vm.chkdisabledenablepermission = function () {
                if (vm.isEnableDisable) {
                    return true;
                } else {
                    return false;
                }
            }
            /**/
            /*vm.updateStatus = [];*/

            vm.exportExcel = function () {

                var url = "../exportToExcel/ExportReportToExcelProjectInvoice";

                $http({
                    method: 'Post',
                    url: url,
                    params: {
                        clientId: vm.project.clientId,
                        marketingleadId: vm.project.marketingleadId,
                        searchBy: vm.searchBox
                    },
                }).then(function (res) {
                    window.location.href = "/Temp/" + res.data.fileName;

                });

            }

            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };
            vm.chckinvoicerequestpermission = function () {
                if (vm.invoice) {
                    return true;
                } else {
                    return false;
                }
            }

            vm.chckuserstoryrequestpermission = function () {
                if (vm.userstory) {
                    return true;
                } else {
                    return false;
                }
            }
            vm.chckprojectMarketingLeaderpermission = function () {
                if (vm.projectMarketingLeaders) {
                    return true;
                } else {
                    return false;
                }
            }

            vm.chckprojectadminpermission = function () {
                if (vm.projectadmin) {
                    return true;
                } else {
                    return false;
                }
            }
            vm.chckprojectPriorityUpdatePermission = function () {
                if (vm.projPriority) {
                    return true;
                } else {
                    return false;
                }

            }

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };


            function getMarketingUser() {
                userService.getUserMarketingLead()
                    .then(function (result) {
                        vm.ddlmarketingList = result.data.items;
                        if (result.data.items.length == 1) {
                            vm.project.marketingleadId = vm.ddlmarketingList[0].id + "";
                            angular.element(document.getElementById('drpteam'))[0].disabled = true;
                            vm.showteamColumn = false;
                            vm.requestParams.marketingleadId = vm.ddlmarketingList[0].id;
                            getAll();
                            //getProjectSearch();
                        } else {
                            vm.teamList = result.data;
                            vm.showteamColumn = true;
                            vm.project.marketingleadId = ""; // change vikas
                            // getProjectSearch();
                            getAll();
                        }
                        console.log(vm.ddlmarketingList);
                    });
            }

            function getProjectStatus() {
                projectService.getInvoiceCollectionProjectStatus()
                    .then(function (result) {
                        vm.ddlProjectStatus = result.data.items;
                        vm.project.projectStatusId = vm.defaultStatus;
                        vm.requestParams.projectStatusId = vm.defaultStatus;
                        vm.project.projectStatusId = vm.ddlProjectStatus[0].id + "";
                    });
            }

            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                });
            }

            function getProjectList() {
                projectService.getProjectData({}).then(function (result) {
                    vm.projectTask = result.data.items;
                });
            }
            function getAll() {
                vm.loading = true;
                abp.ui.setBusy();
                $("#drpteam").select2("val", null);
                projectService.getInvoiceCollectionProjectList($.extend({ projectName: vm.searchBox }, vm.requestParams))
                    .then(function (result) {
                        vm.projectList = result.data.items;
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        $("#drpteam").select2("val", vm.project.marketingleadId);
                        if (vm.project.marketingleadId != "" || vm.project.marketingleadId == undefined) {
                            vm.requestParams.marketingleadId = vm.project.marketingleadId;
                        }
                       
                        if (result.data.totalCount == 0 || vm.userGridOptions.data.length == 0) {
                            //vm.norecord = true;
                            $scope.noData = true;
                            /*abp.notify.info(app.localize('NoRecordFound'));*/
                        } else {
                            if (vm.userGridOptions.data[1].username == "Marketing Leader") {
                                vm.ishown = false;
                            }
                            for (var i = 0; i < vm.userGridOptions.data.length; i++) {
                                // vm.userGridOptions.data[i].price = vm.userGridOptions.data[i].price.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
                                vm.userGridOptions.data[i].projectStatusId = vm.userGridOptions.data[i].projectStatusId + "";
                            }

                            $scope.noData = false;
                        }

                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }

            //vm.openProjectCreationModal = function () {
            //    var modalInstance = $uibModal.open({
            //        templateUrl: '/App/Main/views/project/createProject.cshtml',
            //        controller: 'app.views.project.createProject as vm',
            //        backdrop: 'static'
            //    });
            //    modalInstance.rendered.then(function () {
            //        $.AdminBSB.input.activate();
            //    });
            //    modalInstance.result.then(function () {
            //        //vm.getAll();
            //        getAll();
            //    });
            //};

            vm.openInvoiceCreationModal = function (support) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/project/invoiceRequest.cshtml',
                    controller: 'app.views.project.invoiceRequest as vm',
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
                    /*  vm.getAll();*/
                    getAll();
                });
            };



            vm.viewData = null;

            vm.view = function (data) {
                if (data == null) {
                    vm.viewData = null;
                } else {
                    vm.viewData = data;
                    $state.go('projectView', { id: data.id });
                }
            };

            vm.typeData = function (data) {
                if (data == null) {
                    vm.viewData = null;
                } else {
                    vm.viewData = data;
                    $state.go('projectTypeList', { id: data.id });
                }
            };

            vm.userStoryList = function (obj) {
                if (obj == null) {
                    vm.viewData = null;
                } else {
                    vm.viewData = obj;
                    $state.go('importuserstory', { id: obj.id });
                }
            };


            function getProjectSearch() {
                abp.ui.setBusy();
                vm.loading = true;
                if (vm.project.clientId != "" || vm.project.clientId != undefined) {
                    vm.requestParams.clientId = vm.project.clientId;
                }
                if (vm.project.projectId != "" || vm.project.projectId != undefined) {
                    vm.requestParams.projectId = vm.project.projectId;
                }
                if (vm.project.marketingleadId != "" || vm.project.marketingleadId == undefined) {
                    vm.requestParams.marketingleadId = vm.project.marketingleadId;
                }
                else {
                    vm.requestParams.marketingleadId = null; // change vikas
                }
                if (vm.project.projectStatusId.length > 0) {
                    vm.requestParams.projectStatusId = vm.project.projectStatusId;
                }
                else {
                    vm.requestParams.projectStatusId = [];
                }
                projectService.getInvoiceCollectionProjectList($.extend({ projectName: vm.searchBox }, vm.requestParams))
                    .then(function (result) {
                        vm.projectList = result.data;
                        vm.userGridOptions.totalItems = result.data.totalCount;

                        vm.userGridOptions.data = result.data.items;

                        if (result.data.totalCount == 0) {
                            $scope.noData = true;
                            /* abp.notify.info(app.localize('NoRecordFound'));*/
                        } else {
                            for (var i = 0; i < vm.userGridOptions.data.length; i++) {
                                // vm.userGridOptions.data[i].price = vm.userGridOptions.data[i].price.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
                                vm.userGridOptions.data[i].projectStatusId = vm.userGridOptions.data[i].projectStatusId + "";
                                /* var v = vm.userGridOptions.data[i].price.toString("#,###");*/
                                /*vm.userGridOptions.data[i].price=$filter('number')(vm.userGridOptions.data[i].price, 2)*/
                            }
                            $scope.noData = false;
                            /*vm.norecord = false;*/

                        }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }
            $scope.getTotal = function () {
                var total = 0;
                for (var i = 0; i < $scope.vm.projectList.length; i++) {
                    var product = $scope.vm.projectList[i].price;
                    total = product + total;
                }
                return total;
            }
            $scope.getPendingTotal = function () {
                var total = 0;
                for (var i = 0; i < $scope.vm.projectList.length; i++) {
                    var product = $scope.vm.projectList[i].pendingAmount;
                    total = product + total;
                }
                return total;
            }
            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                getProjectSearch();

            };
            vm.clear = function () {
                getProjectStatus();
                vm.requestParams.projectStatusId = vm.defaultStatus;
                vm.searchBox = "";
                vm.project.projectId = "";
                vm.requestParams.clientId = "";
                vm.requestParams.marketingleadId = "";
                vm.cname = [];
                vm.ddlmarketingList = [];
                vm.project.clientId = null;
                vm.project.marketingleadId = null;
                //getProjectSearch();
                getAll();
                vm.ddlProjectStatus = [];
                getClientName();
                getMarketingUser();
            };
            vm.updateStatusProject = function (id, projectStatusId) {
                var postparam = {};
                if (projectStatusId != "") {
                    postparam.id = id;
                    postparam.updateStatusId = projectStatusId;
                    try {
                        projectService.updateProjectStatus(postparam)
                            .then(function () {
                                abp.notify.success("Status updated successfully.");
                                getAll();
                            });
                    } catch (exp) { }
                } else {
                    abp.notify.error("Please Select Status!");
                    projectStatusId = 0;
                }
            }
            vm.updateProjectPriority = function (id, priority) {
                //var updateStatus = [];
                var postparam = {};
                if (event.keyCode === 8) {

                    postparam.id = id;
                    postparam.updatePrio = null;
                }
                else {
                    postparam.id = id;
                    postparam.updatePrio = priority;
                }
                if (priority != "") {
                    if (priority == undefined) {
                        abp.notify.error("Please Enter Valid Priority!");
                    }
                    else {
                        postparam.id = id;
                        postparam.updatePrio = priority;
                        try {
                            projectService.updateProjectPriority(postparam)
                                .then(function () {
                                    abp.notify.success("Priority updated successfully.");
                                    getAll();
                                });
                        } catch (exp) { }
                    }

                } else {
                    priority = null;
                }
            }

            vm.enabledisable = function (projectList) {
                if (projectList.enableStatus == 0) {
                    abp.message.confirm(
                        "Are you sure you want to Disable Timesheet for project " + projectList.projectName + "?",
                        " ",
                        function (result) {
                            if (result) {
                                projectService.enabledisable(projectList.id)
                                    .then(function () {
                                        abp.notify.success("Enabled Timesheet for project." + projectList.projectName + ".");
                                        getAll();
                                    });
                            }
                        });
                } else {
                    abp.message.confirm(
                        "Are you sure you want to Enable Timesheet for project " + projectList.projectName + "?",
                        " ",
                        function (result) {
                            if (result) {
                                projectService.enabledisable(projectList.id)
                                    .then(function () {
                                        abp.notify.success("Enabled Timesheet for project." + projectList.projectName + ".");
                                        getAll();
                                    });
                            }
                        });
                }

            }

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                showColumnFooter: vm.permissions.adminwrites == true ? true : false,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" /*ng-style=\"grid.appScope.getStyle(row.entity.costPercentage)\"*/ class="ui-grid-cell" ng-class="{\'oldred-people\':(row.entity.hourPercentage>=100),\'oldyellow-people-selected\':(row.entity.hourPercentage<100 && row.entity.hourPercentage>=80),\'oldgreen-people-selected\':(row.entity.hourPercentage<80 && row.entity.hourPercentage>=60), \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                /*  rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',*/
                columnDefs: [
                    {
                        name: App.localize('Actions'),
                        enableSorting: false,
                        enableColumnMenu: false,
                        enableScrollbars: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 70,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li ng-if= "grid.appScope.chckprojectadminpermission()==true || grid.appScope.chckprojectMarketingLeaderpermission()==true"><a ng-click="grid.appScope.openProjectEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li ng-if= "grid.appScope.chckprojectadminpermission()==true && grid.appScope.chckprojectMarketingLeaderpermission()==false"><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                            '      <li  ng-if= "grid.appScope.chckinvoicerequestpermission()==true"><a ng-click="grid.appScope.openInvoiceCreationModal(row.entity)"> Generate Invoice Request </a></li>' +
                            '      <li><a ng-click="grid.appScope.view(row.entity)"> View </a></li>' +
                            '      <li ng-if= "grid.appScope.chckuserstoryrequestpermission()==true"><a ng-click="grid.appScope.userStoryList(row.entity)"> User Story </a></li>' +
                            '      <li ng-if= "grid.appScope.chkdisabledenablepermission()==true"><a ng-click="grid.appScope.enabledisable(row.entity)"> <span ng-if=row.entity.enableStatus==1>Enable Timesheet</span> <span ng-if=row.entity.enableStatus==0>Disable Timesheet</span> </a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'

                    },
                    {
                        name: App.localize('ProjectName'),
                        field: 'projectName',
                        enableColumnMenu: false,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.projectName}}' +
                            '</div>'
                    },
                    {
                        name: "Marketing Lead",
                        field: 'marketingLeadName',
                        enableColumnMenu: false,
                        width: 140,
                        visible: vm.ishown == true,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.marketingLeadName}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('StartDate'),
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        field: 'startDate',
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        width: 80
                    },
                    {
                        name: App.localize('EndDate'),
                        enableColumnMenu: false,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        field: 'endDate',
                        cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                        width: 80
                    },
                    {
                        name: "Priority",
                        field: 'priority',
                        enableColumnMenu: false,
                        width: 70,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        //visible: vm.permissions.updatePriority == true ? true : false,
                        visible: vm.permissions.adminwrites == true || vm.permissions.marketLeads == true,
                        type: uiGridConstants.filter.SELECT,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
                            '<input class="form-control" ng-keyup="grid.appScope.updateProjectPriority(row.entity.id, row.entity.priority)"  ng-if="grid.appScope.chckprojectPriorityUpdatePermission()==true" min="0" maxlength="2" type="number" name="priority" ng-model=" row.entity.priority" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength)">' +

                            '<span ng-if="grid.appScope.chckprojectPriorityUpdatePermission()==false ">{{row.entity.priority}}</span>' +
                            '</div>'
                    },
                    {
                        name: App.localize('Price'),
                        field: 'price',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        enableColumnMenu: false,
                        width: 100,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        //visible: vm.permissions.employeewrites == false ? true : false,
                        //visible: vm.permissions.adminwrites == true ? true : false,
                        visible: vm.permissions.adminwrites == true || vm.permissions.marketLeads == true,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.price}}' +
                            '</div>'
                    },
                    {
                        name: "Invoice Amount",
                        field: 'invoiceamount',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        enableColumnMenu: false,
                        width: 120,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        visible: vm.permissions.adminwrites == true || vm.permissions.marketLeads == true,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" ng-if="row.entity.invoiceamount != null || row.entity.pricesum != null ">' +
                            '<span ng-if="row.entity.invoiceamount==null">0</span>{{row.entity.invoiceamount}}/<span ng-if="row.entity.pricesum==null">0</span>{{row.entity.pricesum}}' +
                            '</div>',
                        //  visible: vm.permissions.employeewrites == false ? true : false,
                    },
                    {
                        name: "Pending amount",
                        field: 'pendingAmount',

                        enableColumnMenu: false,
                        width: 120,
                        cellClass: 'rightalign',
                        headerCellClass: 'rightalign',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.pendingAmount}}' +
                            '</div>',
                        visible: vm.permissions.adminwrites == true || vm.permissions.marketLeads == true,
                    },
                    {
                        name: "ActualHours",
                        field: 'actualhours',
                        enableColumnMenu: false,
                        width: 100,
                        cellClass: 'centeralign',
                        headerCellClass: 'centeralign',
                        /*visible: vm.userDashboardPermission == true ? false : true,*/
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" ng-if= "grid.appScope.chckprojectadminpermission()==true">' +
                            '{{row.entity.actualhours}}/{{row.entity.totalhours}}' +
                            '</div>',
                        /*visible: vm.ishown == true*/  //change vikas
                        visible: vm.permissions.adminwrites == true || vm.permissions.marketLeads == true,
                    },
                    {
                        name: "Type",
                        enableSorting: false,
                        enableColumnMenu: false,
                        enableScrollbars: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 50,
                        cellTemplate: '<div class=\"ui-grid-cell-contents hidesh\" >' +
                            '      <a ng-click="grid.appScope.typeData(row.entity)" class=\"culserpoint\"> Type </a>' +
                            '  </div>' +
                            '</div>',
                        //visible: vm.permissions.adminwrites == true || vm.ishownmkt == true ? true : false
                        visible: (vm.permissions.adminwrites == true || vm.permissions.typeshow == true || vm.permissions.marketLeads == true) ? true : false
                    },
                    {
                        name: "Status",
                        field: 'projectStatusId',
                        enableColumnMenu: false,
                        width: 100,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        type: uiGridConstants.filter.SELECT,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
                            '<select id=\"ddlstat-{{row.entity.id}}\" ng-if="grid.appScope.chckprojectadminpermission()==true || grid.appScope.chckprojectMarketingLeaderpermission()==true" ng-model="row.entity.projectStatusId" class="form-control" ng-change="grid.appScope.updateStatusProject(row.entity.id, row.entity.projectStatusId)" >' +
                            '<option value="">-Select status -</option>' +
                            '<option ng-repeat="us in row.entity.objProjectStatusList" value="{{us.statusId}}">{{us.statusname}}</option>' +
                            '</select>' +
                            '<span ng-if="grid.appScope.chckprojectadminpermission()==false && grid.appScope.chckprojectMarketingLeaderpermission()==false">{{row.entity.projectStatus}}</span>' +
                            '</div>',
                        visible: vm.permissions.adminwrites == true || vm.permissions.marketLeads == true,
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }
                        getAll();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;
                        getAll();
                    });
                },
                data: []
            };

            //vm.userGridOptions = {
            //    enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
            //    enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
            //    /*enablePaginationControls: false,*/
            //    paginationPageSizes: app.consts.grid.defaultPageSizes,
            //    paginationPageSize: app.consts.grid.defaultPageSize,
            //    //showGridFooter: vm.permissions.adminwrites == true ? true : false,
            //    showColumnFooter: vm.permissions.adminwrites == true ? true : false,
            //    useExternalPagination: true,
            //    useExternalSorting: true,
            //    appScopeProvider: vm,
            //    rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{\'oldred-people\':(row.entity.hourPercentage>=100),\'oldyellow-people-selected\':(row.entity.hourPercentage<100 && row.entity.hourPercentage>=80),\'oldgreen-people-selected\':(row.entity.hourPercentage<80 && row.entity.hourPercentage>=60), \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
            //    /*rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',*/
            //    columnDefs: [
            //        {
            //            name: App.localize('Actions'),
            //            enableSorting: false,
            //            enableColumnMenu: false,
            //            enableScrollbars: false,
            //            headerCellClass: 'centeralign',
            //            cellClass: 'centeralign',
            //            width: 70,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
            //                '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
            //                '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
            //                '    <ul uib-dropdown-menu>' +
            //                 // uncomment code -------
            //                '      <li ng-if= "grid.appScope.chckprojectadminpermission()==true"><a ng-click="grid.appScope.openProjectEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
            //                '      <li ng-if= "grid.appScope.chckprojectadminpermission()==true && grid.appScope.chckprojectMarketingLeaderpermission()==false"><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
            //                '      <li  ng-if= "grid.appScope.chckinvoicerequestpermission()==true"><a ng-click="grid.appScope.openInvoiceCreationModal(row.entity)"> Generate Invoice Request </a></li>' +
            //                // -----------
            //                '      <li><a ng-click="grid.appScope.view(row.entity)"> View </a></li>' +
            //                '      <li ng-if= "grid.appScope.chckuserstoryrequestpermission()==true"><a ng-click="grid.appScope.userStoryList(row.entity)"> User Story </a></li>' +
            //                '      <li ng-if= "grid.appScope.chkdisabledenablepermission()==true"><a ng-click="grid.appScope.enabledisable(row.entity)"> <span ng-if=row.entity.enableStatus==1>Enable Timesheet</span> <span ng-if=row.entity.enableStatus==0>Disable Timesheet</span> </a></li>' +
            //                '    </ul>' +
            //                '  </div>' +
            //                '</div>'

            //        },
            //        {
            //            name: App.localize('ProjectName'),
            //            field: 'projectName',
            //            enableColumnMenu: false,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
            //                '{{row.entity.projectName}}' +
            //                '</div>'
            //        },
            //        {
            //            name: "Marketing Lead",
            //            field: 'marketingLeadName',
            //            enableColumnMenu: false,
            //            width: 140,
            //            visible: vm.ishown == true,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
            //                '{{row.entity.marketingLeadName}}' +
            //                '</div>'
            //        },
            //        {
            //            name: App.localize('StartDate'),
            //            enableColumnMenu: false,
            //            cellClass: 'centeralign',
            //            headerCellClass: 'centeralign',
            //            field: 'startDate',
            //            cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
            //            width: 80
            //        },
            //        {
            //            name: App.localize('EndDate'),
            //            enableColumnMenu: false,
            //            cellClass: 'centeralign',
            //            headerCellClass: 'centeralign',
            //            field: 'endDate',
            //            cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
            //            width: 80
            //        },
            //        {
            //            name: "Priority",
            //            field: 'priority',
            //            enableColumnMenu: false,
            //            width: 70,
            //            headerCellClass: 'centeralign',
            //            cellClass: 'centeralign',
            //            /*visible: vm.ishown == true,*/
            //            type: uiGridConstants.filter.SELECT,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
            //                '<input class="form-control" ng-keyup="grid.appScope.updateProjectPriority(row.entity.id, row.entity.priority)"  ng-if="grid.appScope.chckprojectPriorityUpdatePermission()==true" min="0" maxlength="2" type="number" name="priority" ng-model=" row.entity.priority" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength)">' +

            //                '<span ng-if="grid.appScope.chckprojectPriorityUpdatePermission()==false ">{{row.entity.priority}}</span>' +
            //                '</div>'
            //        },
            //        {
            //            name: App.localize('Price'),
            //            field: 'price',
            //            aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
            //            enableColumnMenu: false,
            //            width: 100,
            //            cellClass: 'rightalign',
            //            headerCellClass: 'rightalign',
            //            //visible: vm.permissions.employeewrites == false ? true : false,
            //            visible: vm.permissions.adminwrites == true ? true : false,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
            //                '{{row.entity.price}}' +
            //                '</div>'
            //        },
            //        {
            //            name: "Invoice Amount",
            //            field: 'invoiceamount',
            //            aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
            //            enableColumnMenu: false,
            //            width: 120,
            //            cellClass: 'rightalign',
            //            headerCellClass: 'rightalign',
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\" ng-if="row.entity.invoiceamount != null || row.entity.pricesum != null ">' +
            //                '<span ng-if="row.entity.invoiceamount==null">0</span>{{row.entity.invoiceamount}}/<span ng-if="row.entity.pricesum==null">0</span>{{row.entity.pricesum}}' +
            //                '</div>',
            //            visible: vm.permissions.adminwrites == true ? true : false,
            //            //  visible: vm.permissions.employeewrites == false ? true : false,
            //        },
            //        {
            //            name: "Pending amount",
            //            field: 'pendingAmount',

            //            enableColumnMenu: false,
            //            width: 120,
            //            cellClass: 'rightalign',
            //            headerCellClass: 'rightalign',
            //            aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
            //                '{{row.entity.pendingAmount}}' +
            //                '</div>',
            //            visible: vm.permissions.adminwrites == true ? true : false
            //        },
            //        {
            //            name: "ActualHours",
            //            field: 'actualhours',
            //            enableColumnMenu: false,
            //            width: 100,
            //            cellClass: 'centeralign',
            //            headerCellClass: 'centeralign',
            //            /*visible: vm.userDashboardPermission == true ? false : true,*/
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\" ng-if= "grid.appScope.chckprojectadminpermission()==true">' +
            //                '{{row.entity.actualhours}}/{{row.entity.totalhours}}' +
            //                '</div>',
            //            visible: vm.ishown == true
            //        },
            //        {
            //            name: "Type",
            //            enableSorting: false,
            //            enableColumnMenu: false,
            //            enableScrollbars: false,
            //            headerCellClass: 'centeralign',
            //            cellClass: 'centeralign',
            //            width: 50,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
            //                '      <a ng-click="grid.appScope.typeData(row.entity)" class=\"culserpoint\"> Type </a>' +
            //                '  </div>' +
            //                '</div>',
            //            //visible: vm.permissions.adminwrites == true ? true : false
            //            visible: (vm.permissions.adminwrites == true || vm.permissions.typeshow == true) ? true : false
            //        },
            //        {
            //            name: "Status",
            //            field: 'projectStatusId',
            //            enableColumnMenu: false,
            //            width: 100,
            //            headerCellClass: 'centeralign',
            //            cellClass: 'centeralign',
            //            type: uiGridConstants.filter.SELECT,
            //            cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
            //                '<select id=\"ddlstat-{{row.entity.id}}\" ng-if="grid.appScope.chckprojectadminpermission()==true " ng-model="row.entity.projectStatusId" class="form-control" ng-change="grid.appScope.updateStatusProject(row.entity.id, row.entity.projectStatusId)" >' +
            //                '<option value="">-Select status -</option>' +
            //                '<option ng-repeat="us in row.entity.objProjectStatusList" value="{{us.statusId}}">{{us.statusname}}</option>' +
            //                '</select>' +
            //                '<span ng-if="grid.appScope.chckprojectadminpermission()==false ">{{row.entity.projectStatus}}</span>' +
            //                '</div>',
            //            visible: vm.permissions.adminwrites == true ? true : false
            //        }
            //    ],
            //    onRegisterApi: function (gridApi) {
            //        $scope.gridApi = gridApi;
            //        $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
            //            if (!sortColumns.length || !sortColumns[0].field) {
            //                vm.requestParams.sorting = "Id desc"
            //            } else {
            //                vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
            //            }
            //            getAll();
            //        });
            //        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
            //            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
            //            vm.requestParams.maxResultCount = pageSize;
            //            getAll();
            //        });
            //    },
            //    data: []
            //};

            vm.openProjectEditModal = function (projectList) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/project/editProject.cshtml',
                    controller: 'app.views.project.editProject as vm',
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
                    //vm.getAll();
                    getAll();
                });
            };

            vm.delete = function (projectList) {
                abp.message.confirm(
                    "Delete Project '" + projectList.projectName + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {
                            projectService.deleteProject({ id: projectList.id })
                                .then(function () {
                                    abp.notify.success("Project " + projectList.projectName + "Deleted");
                                    //vm.getAll();
                                    getAll();
                                });
                        }
                    });
            }

            vm.onChangeStatus = function () {

            }

            init = function () {
                //getcurrentuserrole();
                getProjectStatus();
                getClientName();
                getMarketingUser();
                //vm.getAll();
                vm.requestParams.projectStatusId = vm.defaultStatus;
                
            }

            init();
        }
    ]);
})();