
(function () {
    angular.module('app').controller('app.views.projecttype.createProjectMilestone', [
        '$scope', '$filter', '$uibModal', '$uibModalInstance', '$stateParams', '$uibModalInstance', 'abp.services.app.projectMilestone', 'id', 'projectid',
        function ($scope, $filter, $uibModal, $uibModalInstance, $stateParams, $uibModalInstance, projectMilestoneService, id, projectid) {
            //debugger;
            var vm = this;
            vm.milestoneList = [];
            vm.milestone = {};
            vm.milestone.id = id;
            vm.milestone.projectId = parseInt(projectid);
            $scope.norecord = false;
            vm.loading = false;


            function getprojectMilestoneDetails() {
                projectMilestoneService.getprojectMilestoneList({ id })
                    .then(function (result) {
                        vm.milestoneList = result.data;
                        if (vm.milestoneList.length > 0) {
                            $scope.norecord = true;
                        }
                        else { $scope.norecord = false; }
                    });
            }

            vm.save = function () {
                vm.loading = true;


                projectMilestoneService.projectMilestoneExsistence(vm.milestone).then(function (result) {
                    if (!result.data) {
                        vm.milestone.projectTypeId = id;
                        //vm.milestone.projectTypeId = id;
                        projectMilestoneService.createProjectMilestone(vm.milestone)
                            .then(function () {
                                abp.notify.success(App.localize('Project Milestone Saved Successfully '));
                                getprojectMilestoneDetails();
                                //$uibModalInstance.close();
                            });
                    }
                    else {
                        abp.notify.error(App.localize('Project Milestone already Exist '));
                        vm.loading = false;
                    }
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
                getprojectMilestoneDetails();
            };
            init();

        }
    ]);
})();