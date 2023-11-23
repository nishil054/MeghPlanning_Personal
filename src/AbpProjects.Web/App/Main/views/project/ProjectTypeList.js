(function () {
    angular.module('app').controller('app.views.project.projectTypeList', [
        '$scope', '$state', '$uibModal', '$stateParams', 'abp.services.app.project',
        function ($scope, $state, $uibModal, $stateParams, projectService) {
            var vm = this;
            vm.data = {};
            vm.projectDetails = [];
            vm.data.id = $stateParams.id;
            vm.projectId = $stateParams.id;
           // vm.projectTypeId = $stateParams.id;

            vm.getDataView = function () {
                projectService.getProjectViewById({
                    id: vm.data.id
                }).then(function (result) {
                    vm.data = result.data;
                    console.log(result);
                });
            }

            function getprojectDetails() {
                abp.ui.setBusy();
                projectService.getprojectDetailsList({ id: vm.data.id })
                    .then(function (result) {
                        vm.projectDetails = result.data.items;
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            }
            vm.openEdit = function (projectList) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/project/editProjectType.cshtml',
                    controller: 'app.views.project.editProjectType as vm',
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
                    init();
                });
            };

            vm.viewData = null;

            //vm.openEdit = function (data) {
            //    if (data == null) {
            //        vm.viewData = null;
            //    }
            //    else {
            //        vm.viewData = data;
            //        $state.go('editProjectType', { id: data.id });
            //    }
            //};
            vm.openProjectCreationModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/project/createProjectType.cshtml',
                    controller: 'app.views.project.createProjectType as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return vm.projectId;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    init();
                });
            };

            vm.openMilestoneCreationModal = function (milestone) {
                //debugger;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/projecttype/createProjectMilestone.cshtml',
                    controller: 'app.views.projecttype.createProjectMilestone as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return milestone.id;
                        },
                        projectid: function () {
                            return $stateParams.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {

                    getprojectDetails();
                    //vm.getAll();
                    //getUsers();

                });
            };

            //vm.openProjectCreationModal = function (data) {
            //    $state.go('createProjectType', { id: vm.projectId });
            //};

            vm.openDelete = function (data) {
                abp.message.confirm(
                    "Delete Project Type '" + data.projectType + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {
                            projectService.deleteProjectType({ id: data.id })
                                .then(function () {
                                    abp.notify.success("Deleted");
                                    init();
                                });
                        }
                    });
            }
            function init() {
                vm.getDataView();
                getprojectDetails();
                
            }

            init();

            vm.cancel = function () {
                $state.go('project');
            };
        }
    ]);
})();