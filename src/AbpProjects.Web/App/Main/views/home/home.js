(function () {
    var controllerId = 'app.views.home.home';
    angular.module('app').controller(controllerId, [
        '$scope', '$state', '$timeout', '$uibModal', 'abp.services.app.dashboard', 'abp.services.app.masterList', 'uiGridConstants','abp.services.app.opportunityService',
        function ($scope, $state, $timeout, $uibModal, dashboardService, masterListservice, uiGridConstants, opportunityService) {
            //debugger;
            var vm = this;
            $scope.sal = false;
            $scope.pamt = false;
            $scope.dom = false;
            $scope.sto = false;
            $scope.hos = false;
            $scope.em = false;
            $scope.salesreport = false;
            $scope.servicerepo = false;
            $scope.out = false;
            $scope.diff = false;
            $scope.invodis = false;
            $scope.today = new Date();
            //$scope.norecord = false;
            $scope.renewlist = false;
            $scope.missingtimesheet = false;
            $scope.assigneuserstory = false;
            vm.tasks = [];
            vm.sales = [];
            vm.servicewi = [];
            vm.projecthour = [];
            vm.monthlysales = [];
            vm.monthlysalesdata = [];
            vm.pamountstats = [];
            vm.loginUserList = [];
            vm.salesdata = [];
            vm.target = [];
            vm.extrasales = [];
            vm.months = ["Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "March"];
            vm.lname = ["Total Invoice", "Total Collection", "Total Outstanding"];
            vm.chartid = "firstchart";
            vm.chartid2 = "secondchart";
            vm.chartid3 = "donut";
            vm.totaldata = [];
            vm.dt = [];
            vm.invo = [];
            vm.timesheetList = [];
            vm.t1 = [];
            vm.t2 = [];
            vm.t3 = [];
            vm.task = {};
            $scope.record = true;
            vm.home = [];
            vm.logoutmissing = [];
            vm.followCount = [];
            vm.timesheetmissing = [];
            vm.assignuserstoryList = [];
            vm.usermissingtimesheet = [];
            vm.monthlysales = [];
            vm.login = {};
            vm.loginuserlist = {};
            vm.logout = {};
            vm.support = abp.auth.isGranted('Pages.SupportManageService');
            vm.sales7days = abp.auth.isGranted('Pages.SevenDays.Sales');
            vm.currentmonthsales = abp.auth.isGranted('Pages.Monthly.Sales');
            vm.totaloutst = abp.auth.isGranted('Pages.TotalOutstanding');

            vm.projectstatshours = abp.auth.isGranted('Pages.ProjectStats.Hour');
            vm.projectstatsamount = abp.auth.isGranted('Pages.ProjectStats.Amount');
            vm.employeeRenewal = abp.auth.hasPermission('Section.Renewal');
            vm.chartname = "Target Vs. Achievement";
            vm.loading = false;
            vm.loginloading = false;
            $scope.timeshow = false;
            $scope.timeshowlogout = false;
            $scope.logintime = false;
            $scope.logouttime = false;
            $scope.projreport = false;
            vm.totaldiff = [];
            vm.itemsPerPage = 10;
            /* vm.dt = [300, 400, 500];*/
            vm.pc = [];
            vm.skipCount = 0;
            vm.leaveBalance = 0.0;
            vm.pendingLeavecount = 0.0;
            var t1 = new Date();
            var year = t1.getFullYear();

            vm.currentyear = year;
            vm.leaveDate = null;
            vm.requestParams = {
                //skipCount: 0,
                //maxResultCount: app.consts.grid.defaultPageSize,
                //sorting: "Id desc"
                skipCount: vm.skipCount,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "name"
            };

            var date = new Date();
            vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            vm.followfromDate = moment(date);
            vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            vm.followtoDate = moment(date);

            vm.permissions = {
                userAdmin: abp.auth.hasPermission('Section.Renewal'),
                missingTimsheetEmployee: abp.auth.hasPermission('Pages.MissingTimesheetEmployee.Count'),
                missingTimsheetDatewise: abp.auth.hasPermission('Pages.MissingTimesheet.Datewise'),
                salesTargetChart: abp.auth.hasPermission('Pages.SalesTargetChart'),
                serviceWithoutClient: abp.auth.hasPermission('Pages.ServiceWithoutClient'),
                supportadminwrites: abp.auth.hasPermission('Pages.Support.Admin'),
                assignToUserstoryEmployee: abp.auth.hasPermission('Pages.AssignToUserstory.Employee'),
                opportunityFollowUp: abp.auth.hasPermission('Pages.Opportunity'),
                employeeHoliday: abp.auth.hasPermission('Pages.Employee.Holiday'),
                myOpportunity: abp.auth.hasPermission('Pages.MyOpportunity')
            };
            
            function getCurntYear() {
                masterListservice.getFinYear().then(function (result) {
                    vm.financialyear = result.data;
                });
            }

            vm.opportunity = function () {
                $state.go('inquiry')

            }

            function leaveBalanceData() {
                abp.ui.setBusy();
                dashboardService.leaveBalance({})
                    .then(function (result) {
                        vm.leaveBalance = result.data;
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
                dashboardService.pendingLeave({})
                    .then(function (result) {
                        vm.pendingLeavecount = result.data;
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }
            //function leaveBalanceData() {
            //    abp.ui.setBusy();

            //}

            function leaveUpdateDate() {
                abp.ui.setBusy();
                dashboardService.leaveUpteDate()
                    .then(function (result) {
                        vm.leaveDate = result.data;
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }
            vm.chcksupportpermission = function () {
                if (vm.support) {
                    return true;
                } else {
                    return false;
                }

            }
            vm.chcksales1permission = function () {
                if (vm.sales7days) {
                    return true;
                } else {
                    return false;
                }

            }

            vm.chckmonthlysales1permission = function () {
                if (vm.currentmonthsales) {
                    return true;
                } else {
                    return false;
                }

            }

            vm.chcktotaloutstandingpermission = function () {

                if (vm.totaloutst) {
                    return true;
                } else {
                    return false;
                }

            }
            vm.checkDate = function (dateVAr) {

                return moment(dateVAr).isAfter(moment());

            }
            vm.chckprojectstatshourspermission = function () {
                if (vm.projectstatshours) {
                    return true;
                } else {
                    return false;
                }

            }

            vm.chckprojectstatsamountpermission = function () {
                if (vm.projectstatsamount) {
                    return true;
                } else {
                    return false;
                }

            }
            function getServiceNameWithoutClient() {
                abp.ui.setBusy();
                dashboardService.getServiceNameWithoutClient({}).then(function (result) {
                    vm.servicewi = result.data.items;
                    if (vm.servicewi != 0) {
                        $scope.servicerepo = true;
                    } else {
                        $scope.servicerepo = false;
                    }
                    //console.log(vm.servicewi);
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            vm.bindprojecttypename = function (ptlist) {
                var name = "";
                angular.forEach(ptlist, function (v1, k1) {
                    name += v1 + ",";
                });
                return name.substring(0, name.length - 1);
            }

            vm.viewData = null;

            vm.addFollowUp = function (data) {
                if (data == null) {
                    vm.viewData = null;
                } else {
                    vm.viewData = data;
                    $state.go('addFollow', { id: data.masterOpporutnityid });
                }
            };

            function getFollowUp() {
                abp.ui.setBusy();
                dashboardService.getFollowUp({}).then(function (result) {
                    vm.followUp = result.data.items;
                    if (result.data.items.length == 0) {
                        $scope.shownFollowUp = false;
                    } else {
                        $scope.shownFollowUp = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            function getSalesReport() {
                abp.ui.setBusy();
                dashboardService.getSalesReport({}).then(function (result) {

                    vm.sales = result.data.items;
                    if (vm.sales != 0) {
                        $scope.salesreport = true;
                    } else {
                        $scope.salesreport = false;
                    }

                    //console.log(vm.sales);
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            function getProjectStatesHour() {
                abp.ui.setBusy();
                dashboardService.getProjectStatesHour({}).then(function (result) {
                    vm.projecthour = result.data.items;
                    if (vm.projecthour != 0) {
                        $scope.projreport = true;
                    } else {
                        $scope.projreport = false;
                    }

                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            vm.getUsers = function () {
                vm.loading = true;
                abp.ui.setBusy();
                dashboardService.getUserRenewdata($.extend({}, vm.requestParams)).then(function (result) {
                    vm.dashboard = result.data.items;
                    if (vm.dashboard != 0) {
                        $scope.renewlist = true;
                    } else {
                        $scope.renewlist = false;
                    }
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    /*else { vm.norecord = false; }*/
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            vm.openUserRenewEditModal = function (home) {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/home/editRenew.cshtml',
                    controller: 'app.views.home.editRenew as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return home.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getUsers();
                });
            };

            vm.openUserResignEditModal = function (home) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/home/editResign.cshtml',
                    controller: 'app.views.home.editResign as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return home.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getUsers();
                });
            };


            // first
            vm.loginf = function () {
                abp.ui.setBusy();
                vm.loginloading = true;
                dashboardService.createLogInTime()
                    .then(function (result) {
                        //debugger;
                        vm.login = result.data;
                        $scope.timeshow = false;
                        $scope.timeshowlogout = true;
                        $scope.logintime = true;
                        $scope.logouttime = false;
                        vm.loginloading = false;
                        abp.notify.success(App.localize('SavedSuccessfully'));
                    }).finally(function () {
                        vm.loginloading = false;
                        abp.ui.clearBusy();
                    });

            };
            //second
            vm.getLoginDetails = function () {
                abp.ui.setBusy();
                //debugger;
                dashboardService.getLoginUserList({

                }).then(function (result) {
                    //debugger;
                    vm.login = result.data;
                    if (vm.login.loggedIn == null) {
                        $scope.timeshow = true;
                        $scope.timeshowlogout = false;
                        $scope.logintime = false;
                        $scope.logouttime = false;

                    }
                    else if (vm.login.loggedIn != null && vm.login.loggedOut != null) {
                        $scope.timeshow = false;
                        $scope.timeshowlogout = false;
                        $scope.logintime = true;
                        $scope.logouttime = true;

                    }
                    else if (vm.login.loggedIn != null) {
                        // vm.login.loggedIn = moment.utc(vm.login.loggedIn).format('HH:mm');
                        $scope.timeshow = false;

                        $scope.logintime = true;
                        $scope.logouttime = false
                        $scope.timeshowlogout = true;

                    }
                    vm.totalRecord = result.data.totalCount;
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            vm.logout = function () {
                //debugger;
                abp.ui.setBusy();
                dashboardService.updateLogOutTime({ id: vm.login.id })
                    .then(function (result) {
                        //debugger;
                        vm.login = result.data;
                        //vm.login.loggedIn = moment.utc(vm.login.loggedIn).format('HH:mm');
                        $scope.timeshow = false;
                        $scope.timeshowlogout = false;
                        $scope.logintime = true;
                        $scope.logouttime = true;
                        abp.notify.success(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });

            };

            vm.getMissingLogOutData = function () {
                //debugger;
                abp.ui.setBusy();
                dashboardService.getMissingLogOutList({

                }).then(function (result) {
                    //debugger;
                    vm.logoutmissing = result.data.items;
                    vm.totalRecord = result.data.totalCount;
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            function getfollowUpCountData () {
                abp.ui.setBusy();
                vm.datefilter = {};
                vm.datefilter.fromDate = vm.followfromDate;
                vm.datefilter.toDate = vm.followtoDate;
                opportunityService.getfollowCountLists($.extend({}, vm.datefilter)).then(function (result) {
                    vm.followCount = result.data.items;
                    if (vm.followCount == 0) {
                        $scope.noData = true;
                    } else { $scope.noData = false; }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            vm.searchAll = function(){
                getfollowUpCountData()
            }

            vm.clearAll = function () {
                var date = new Date();
                vm.firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                vm.followfromDate = moment(date);
                vm.lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                vm.followtoDate = moment(date);
                vm.datefilter.fromDate = vm.followfromDate;
                vm.datefilter.toDate = vm.followtoDate;
                getfollowUpCountData();
              
            };

            vm.getMissingTimesheet = function () {
                //debugger;  
                abp.ui.setBusy();
                dashboardService.getMissingTimesheet({}).then(function (result) {
                    //debugger;
                    vm.timesheetmissing = result.data;
                    //console.log(vm.timesheetmissing+"1");
                    if (vm.timesheetmissing != null && vm.timesheetmissing != 0) {
                        $scope.missingtimesheet = true;
                    } else {
                        $scope.missingtimesheet = false;
                    }
                    vm.totalRecord = result.data.length;
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            vm.getAssignUserstory = function () {
                //debugger;  
                abp.ui.setBusy();
                dashboardService.getAssignUserstoryEmployeewise({}).then(function (result) {
                    //debugger;
                    vm.assignuserstoryList = result.data.items;
                    //console.log(vm.timesheetmissing+"1");
                    if (vm.assignuserstoryList != null && vm.assignuserstoryList != 0) {
                        $scope.assigneuserstory = true;
                    } else {
                        $scope.assigneuserstory = false;
                    }
                    vm.totalRecord = result.data.length;
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }


            vm.getUserMissingTimesheetCount = function () {
                //debugger;
                abp.ui.setBusy();
                dashboardService.getUserMissingTimesheetCount({})
                    .then(function (result) {
                        vm.usermissingtimesheet = result.data.items;
                        if (vm.usermissingtimesheet != 0) {
                            $scope.norecord = true;
                        } else {
                            $scope.norecord = false;
                        }
                        vm.totalRecord = result.data.items.length;
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }



            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                vm.getemployeetargetsalesreport();
            };

            vm.openTimeSheetCreationModal = function (obj) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/timesheet/createTimeSheet.cshtml',
                    controller: 'app.views.timesheet.createTimeSheet as vm',
                    backdrop: 'static',
                    resolve: {
                        date: function () {
                            return obj.date;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {

                    vm.getMissingTimesheet();
                    //vm.getAll();
                    //getUsers();

                });
            };



            vm.save = function (t) {
                abp.message.confirm(
                    "Are you sure you want to cancel?",
                    "Cancel?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            abp.ui.setBusy();
                            dashboardService.updateDashboardService(t.id)
                                .then(function () {
                                    abp.notify.info("Cancel: " + t.domainName);
                                    getDomainDate();
                                }).finally(function () {
                                    vm.loading = false;
                                    abp.ui.clearBusy();
                                });
                        }
                    });
            };
            vm.opensaveAsConfirmModal = function (support) {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/home/markasconfirm.cshtml',
                    controller: 'app.views.home.markasconfirm as vm',
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
                    getDomainDate();
                });
            };


            vm.done = function (home) {
                //debugger;
                abp.message.confirm(
                    "Are you sure you want to change this status to done?",
                    "Done?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            //debugger;
                            abp.ui.setBusy();
                            dashboardService.updateTimesheetStatus(home)
                                .then(function (result) {
                                    vm.home = result.data;
                                    vm.getMissingTimesheet();

                                }).finally(function () {
                                    vm.loading = false;
                                    abp.ui.clearBusy();
                                });
                        }
                    });
            };

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
                            // return support.a = true;
                        },
                        fdash: function () {
                            return true;
                            // return support.a = true;
                        },
                        fid: function () {
                            return false;
                            // return support.a = true;
                        },
                        feditdashboard: function () {
                            return false;
                        }
                    }
                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    getDomainDate();

                });
            };

            vm.openServiceWithoutClientEditModal = function (support) {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/home/editServiceWithoutClient.cshtml',
                    controller: 'app.views.home.editServiceWithoutClient as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return support.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    getServiceNameWithoutClient();
                    getDomainDate();
                });
            };

            function getDomainDate() {
                abp.ui.setBusy();

                dashboardService.getDomainDate({}).then(function (result) {
                    vm.tasks = result.data.items;
                    if (vm.tasks != 0) {
                        $scope.dom = true;
                    } else {
                        $scope.dom = false;
                    }

                    //console.log(vm.tasks);
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });;
            }
            function getMonthlySales() {
                abp.ui.setBusy();
                dashboardService.getMonthlySales({}).then(function (result) {
                    vm.monthlysalesdata = result.data.items;
                    if (vm.monthlysalesdata != 0) {
                        $scope.sal = true;

                    }
                    else {
                        $scope.sal = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            function getProjectAmountStats() {
                abp.ui.setBusy();
                dashboardService.getProjectStatsAmount({}).then(function (result) {
                    if (result.data.items.length > 0) {
                        vm.pamountstats = result.data.items;
                        $scope.pamt = true;
                    }
                    else {
                        $scope.pamt = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            function getemployeemorrisreport() {
                dashboardService.getSalestargetmoriss({}).then(function (result) {

                    vm.morissdata = result.data.items;
                    //vm.ct = document.getElementById("myChart");
                    //Morris.Bar({
                    //    element: vm.ct,
                    //    data: vm.morissdata,
                    //    xkey: 'y',
                    //    ykeys: ['a'],
                    //    labelTop: true,
                    //    labels: ['No of Invoice'],
                    //    xLabelAngle: 45

                    //});



                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }
            function getemployeetargetsalesreport() {
                abp.ui.setBusy();
                dashboardService.getSalestarget({}).then(function (result) {

                    vm.monthlysales = result.data.items;
                    vm.target = ['amount', 'targetAmount']

                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            function getpiechart() {

                abp.ui.setBusy();
                dashboardService.getCollectionSum({}).then(function (result) {
                    vm.pc = result.data;

                    if (vm.pc != 0) {
                        $scope.out = true;
                    } else {
                        $scope.out = false;
                    }

                    //console.log(vm.totaldata);


                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            function gettotaloust() {

                abp.ui.setBusy();
                dashboardService.getTotalOutstanding({}).then(function (result) {
                    vm.totaldiff = result.data;

                    if (vm.totaldiff != 0) {
                        $scope.diff = true;
                    } else {
                        $scope.diff = false;
                    }

                    //console.log(vm.totaldiff);


                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            function gettotalinvo() {

                abp.ui.setBusy();
                dashboardService.getTotalInvoice({}).then(function (result) {
                    vm.invo = result.data;

                    if (vm.invo != 0) {
                        $scope.invodis = true;
                    } else {
                        $scope.invodis = false;
                    }

                    //console.log(vm.invodis);


                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            vm.refresh = function () {
                getDomainDate();
                getemployeetargetsalesreport();
            };
            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                enablePaginationControls: false,
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
                    cellClass: 'centeralign verticalalign',
                    width: 100,
                    cellTemplate: '<div class=\"ui-grid-cell-contents padd0\">' +
                        '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                        '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                        '    <ul uib-dropdown-menu>' +
                        '      <li><a ng-click="grid.appScope.openUserRenewEditModal(row.entity)">' + App.localize('Mark As Renew') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.openUserResignEditModal(row.entity)">' + App.localize('Mark As Resign') + '</a></li>' +
                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },
                //{
                //    name: App.localize('UserName'),
                //    field: 'userName',
                //    enableColumnMenu: false,
                //    width: 150
                //},
                {
                    name: App.localize('Name'),
                    enableColumnMenu: false,
                    field: 'name',
                    // width: 100,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        '{{row.entity.name}} {{row.entity.surname}}' +
                        '</div>'
                },
                //{
                //    name: App.localize('EmailAddress'),
                //    enableColumnMenu: false,
                //    field: 'emailAddress'
                //    //width: 250
                //},
                //{
                //    name: App.localize('Active'),
                //    enableColumnMenu: false,
                //    headerCellClass: 'centeralign',
                //    cellClass: 'centeralign',
                //    field: 'isActive',
                //    cellTemplate:
                //        '<div class=\"ui-grid-cell-contents\">' +
                //        '  <span ng-show="row.entity.isActive" class="label label-success">' + App.localize('Yes') + '</span>' +
                //        '  <span ng-show="!row.entity.isActive" class="label label-default">' + App.localize('No') + '</span>' +
                //        '</div>',
                //    width: 80,
                //    cellClass: 'centeralign'
                //},
                {
                    name: App.localize('RenewalDate'),
                    enableColumnMenu: false,
                    cellClass: 'centeralign',
                    headerCellClass: 'centeralign',
                    field: 'next_Renewaldate',
                    cellFilter: 'momentFormat: \'DD/MM/YYYY\'',
                    width: 140
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

                        vm.getUsers();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getUsers();
                    });
                },
                data: []
            };
            var init = function () {
                if (vm.permissions.opportunityFollowUp) {
                    getfollowUpCountData();
                }
                if (vm.permissions.employeeHoliday) {
                    leaveBalanceData();
                    leaveUpdateDate();
                    vm.getLoginDetails();
                }
                //getemployeemorrisreport();
                if (vm.chcktotaloutstandingpermission()) {
                    gettotaloust();
                    getCurntYear();
                    gettotalinvo();
                    getpiechart();
                }
                // HR
                if (vm.employeeRenewal) {
                    vm.getUsers();
                }
                //Accountant // Support 
                if (vm.support) {
                    //getUsersSearch();
                    getDomainDate();
                }
                if (vm.currentmonthsales) {
                    getMonthlySales();

                }
                if (vm.projectstatshours) {
                    getProjectStatesHour();
                }

                if (vm.projectstatsamount) {
                    getProjectAmountStats();
                }
                if (vm.sales7days) {
                    getSalesReport();
                }
                if (vm.permissions.opportunityFollowUp) {
                    getFollowUp();
                }
                if (vm.permissions.salesTargetChart) {
                    getemployeetargetsalesreport();
                }
                if (vm.permissions.serviceWithoutClient) {
                    getServiceNameWithoutClient();
                }
                if (vm.permissions.missingTimsheetEmployee) {
                    vm.getUserMissingTimesheetCount();
                }
                if (vm.permissions.missingTimsheetDatewise) {
                    vm.getMissingTimesheet(); 
                }
                if (vm.permissions.assignToUserstoryEmployee) {
                    vm.getAssignUserstory();
                }
            };
            init();

        }
    ]);
})();


