(function () {
    var controllerId = 'app.views.supportpages.servicerenewal';
    angular.module('app').controller(controllerId, [
        '$scope', '$timeout', '$uibModal', 'abp.services.app.dashboard', 'abp.services.app.masterList', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, dashboardService, masterListservice, uiGridConstants) {
            
            var vm = this;
            $scope.dom = false;
            $scope.hos = false;
            $scope.em = false;
            $scope.servicerepo = false;
            $scope.out = false;
            $scope.diff = false;
            $scope.invodis = false;
            $scope.today = new Date();
            $scope.renewlist = false;
            $scope.missingtimesheet = false;
            $scope.assigneuserstory = false;
            vm.tasks = [];
            vm.sales = [];
            vm.servicewi = [];
            vm.months = ["Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "March"];
            vm.lname = ["Total Invoice", "Total Collection", "Total Outstanding"];
            vm.totaldata = [];
            vm.dt = [];
            vm.invo = [];
            vm.t1 = [];
            vm.t2 = [];
            vm.t3 = [];
            vm.task = {};
            $scope.record = true;
            vm.home = [];
            vm.support = abp.auth.isGranted('Pages.SupportManageService');
            
            vm.employeeRenewal = abp.auth.hasPermission('Section.Renewal');
            
            vm.loading = false;

            vm.checkDate = function (dateVAr) {
                return moment(dateVAr).isAfter(moment());
            }
           
            vm.requestParams = {
                skipCount: vm.skipCount,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "name"
            };

            vm.permissions = {
                userAdmin: abp.auth.hasPermission('Section.Renewal'),
                supportadminwrites: abp.auth.hasPermission('Pages.Support.Admin'),
            };

          
           
            vm.chcksupportpermission = function () {
                if (vm.support) {
                    return true;
                } else {
                    return false;
                }
            }
           
            vm.refreshGrid = function (n) {
                vm.skipCount = n;
                vm.getemployeetargetsalesreport();
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

            function getDomainDate() {
                abp.ui.setBusy();
                dashboardService.getDomainDate({}).then(function (result) {
                    vm.tasks = result.data.items;
                    if (vm.tasks != 0) {
                        $scope.dom = true;
                    } else {
                        $scope.dom = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });;
            }
          
            vm.refresh = function () {
                getDomainDate();
                //getemployeetargetsalesreport();
            };
            
            var init = function () {
                if (vm.support) {
                    getDomainDate();
                }
            };
            init();

        }
    ]);
})();


