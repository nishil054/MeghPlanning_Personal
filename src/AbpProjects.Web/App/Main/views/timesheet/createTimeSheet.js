(function () {
    angular.module('app').controller('app.views.timesheet.createTimeSheet', [
        '$scope', '$filter', '$uibModal', '$uibModalInstance', 'abp.services.app.timeSheet', 'abp.services.app.dashboard', 'date', 'abp.services.app.masterList',
        function ($scope, $filter, $uibModal, $uibModalInstance, timeSheetService, dashboardService, date, masterListservice) {
            //debugger;
            var vm = this;
            vm.projectlist = [];
            vm.worktypelist = [];
            vm.timesheetList = [];
            vm.userstorylist = [];
            vm.timesheet = {};
            vm.obj = {};
            $scope.norecord = false;
            vm.timesheet.date = moment();
            $scope.btndisable = false;
            function getProjects() {

                timeSheetService.getProject()
                    .then(function (result) {

                        vm.projectlist = result.data;
                    });
            }
            function getWorkTypes() {

                masterListservice.getWorkType()
                    .then(function (result) {

                        vm.worktypelist = result.data;
                    });
            }
            $scope.projectChange = function (projectId) {
                masterListservice.getUserStory({ projectId }).then(function (result) {
                    console.log(result);
                    vm.userstorylist = result.data.items;
                });

            }

            vm.CheckNumber = function () {
                if (event.keyCode === 46) {

                }

                else if (isNaN(event.key) || event.key === ' ' || event.key === '') {
                    event.returnValue = '';
                }
            };

            vm.getTimeSheetData = function () {
                dashboardService.getTimeSheetData({ date: date }).then(function (result) {
                    vm.timesheetList = result.data.items;
                 
                    if (date != null) {
                        vm.timesheet.date = moment(date);

                    } else {
                        vm.timesheet.date = moment(Date.now());

                    }
                    if (vm.timesheetList.length > 0) {
                        $scope.norecord = true;
                    }
                    else { $scope.norecord = false; }
                });
            }

            vm.openTimeSheetEditModal = function (timesheet) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/timesheet/editTimeSheet.cshtml',
                    controller: 'app.views.timesheet.editTimeSheet as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return timesheet.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getTimeSheetData();
                });
            };



            vm.save = function () {
                $scope.btndisable = true;
                timeSheetService.createTimeSheet(vm.timesheet)
                    .then(function (result) {
                        console.log(result);
                        abp.notify.success(App.localize('Saved Successfully'));
                        $uibModalInstance.close();
                        $scope.btndisable = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };
            var init = function () {
                getProjects();
                getWorkTypes();
                vm.getTimeSheetData();
                
            };
          
            init();
            
        }
    ]);
})();