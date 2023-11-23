(function () {
    angular.module('app').controller('app.views.timesheet.details', [
        '$scope', '$uibModalInstance', 'abp.services.app.timeSheet', 'abp.services.app.masterList', 'id',
        function ($scope, $uibModalInstance, timeSheetService, masterListservice, id) {
            var vm = this;
            vm.timesheet = {};
            vm.projectlist = [];
            vm.worktypelist = [];
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
            var init = function () {
                // getProjects();
                // getWorkTypes();

                timeSheetService.getTimeSheet({ id: id }).then(function (result) {
                    vm.timesheet = result.data;
                    vm.timesheet.projectName = vm.timesheet.projectName;
                    vm.timesheet.workTypeName = vm.timesheet.workTypeName;
                    vm.timesheet.date = vm.timesheet.date;
                    vm.timesheet.hours = vm.timesheet.hours;
                    vm.timesheet.description = vm.timesheet.description;
                    vm.timesheet.userStory
                });
            }

          

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();