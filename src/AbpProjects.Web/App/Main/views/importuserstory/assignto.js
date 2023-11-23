
(function () {
    angular.module('app').controller('app.views.importuserstory.assignto', [
        '$scope', '$filter', '$uibModal', '$uibModalInstance', '$stateParams', '$uibModalInstance', 'abp.services.app.importUserStoryDetails', 'abp.services.app.projectMilestone', 'abp.services.app.masterList', 'id', 'projectid', 'empName',
        function ($scope, $filter, $uibModal, $uibModalInstance, $stateParams, $uibModalInstance, userStoryService, projectMilestoneService, masterListservice, id, projectid, empName) {
            //debugger;
            var vm = this;
            vm.employeelist = [];
            vm.assign = {};
            vm.milestoneList = [];
            vm.milestone = {};
            vm.milestone.id = id;
            if (empName == " ") {
                vm.milestone.empName = null;
            } else {
                vm.milestone.empName = empName;
            }
            
            vm.milestone.projectId = parseInt(projectid);
            $scope.norecord = false;
            $scope.btndisable = false;

            function getEmployee() {

                masterListservice.getEmployee()
                    .then(function (result) {

                        vm.employeelist = result.data;
                    });
            }

            vm.save = function () {
                //debugger;
                vm.saving = true;
                vm.milestone.id = id;
                var UpdateStatusParams = {
                    id: id,
                    employeeId: vm.assign.employeeId
                };
                console.log(UpdateStatusParams);
                userStoryService.updateAssignUserstoryToEmployee($.extend({}, UpdateStatusParams)).then(function () {
                    abp.notify.success(App.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                });
            };

            vm.cancel = function () {
                //$uibModalInstance.dismiss({});
                $uibModalInstance.close();
            };

            vm.CheckNumber = function () {
                //console.log(event.keyCode);
                if (event.keyCode === 46) {

                }

                else if (isNaN(event.key) || event.key === ' ' || event.key === '') {
                    event.returnValue = '';
                }
            };

            vm.delete = function (milestone) {
                abp.message.confirm(
                    "Delete Data '" + milestone.title + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {
                            projectMilestoneService.deleteProjectMilestone({ id: milestone.id })
                                .then(function () {
                                    abp.notify.success("Deleted Successfully");

                                    getprojectMilestoneDetails();
                                    //$uibModalInstance.close();
                                });
                        }
                    });
            }


            var init = function () {
                getEmployee();
            };
            init();

        }
    ]);
})();