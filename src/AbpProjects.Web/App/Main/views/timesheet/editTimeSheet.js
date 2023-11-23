(function () {
    angular.module('app').controller('app.views.timesheet.editTimeSheet', [
        '$scope', '$uibModalInstance', '$uibModal', 'abp.services.app.timeSheet', 'id', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, $uibModal, timeSheetService, id, masterListservice) {
            //debugger;
            var vm = this;
            vm.timesheet = [];
            vm.projectlist = [];
            vm.worktypelist = [];
            vm.userstorylist = [];
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
            //function getUserStory() {

            //    masterListservice.getUserStory()
            //        .then(function (result) {

            //            vm.userstorylist = result.data;
            //        });
            //}
            $scope.projectChange = function (projectId) {
                //debugger;
                masterListservice.getUserStory({ projectId }).then(function (result) {
                    //params: {
                    //    countryId: countryId
                    //}
                    vm.userstorylist = result.data.items;
                    
                });

            }
            var init = function () {
                
                
                timeSheetService.getDataById({ id: id }).then(function (result) {
                    vm.timesheet = result.data;
                    vm.timesheet.projectId = vm.timesheet.projectId + "";
                    vm.timesheet.workTypeId = vm.timesheet.workTypeId + "";
                  
                    vm.timesheet.date = moment(vm.timesheet.date);
                    $scope.projectChange(vm.timesheet.projectId);
                    if (vm.timesheet.userStoryId != null) {
                        vm.timesheet.userStoryId = vm.timesheet.userStoryId + "";
                    }
                    getProjects();
                    getWorkTypes();
                    getUserStory();
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

            vm.save = function () {
                //debugger;
                // var date1 = vm.timesheet.date;
                $scope.btndisable = true;
                console.log(vm.timesheet);
               
                timeSheetService.updateTimeSheet(vm.timesheet).then(function (result) {
                    console.log(result);
                  
                    abp.notify.success(App.localize('Saved Successfully'));
                    $uibModalInstance.close();
                });
                $scope.btndisable = false;
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();