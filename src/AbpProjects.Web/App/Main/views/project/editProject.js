(function () {
    angular.module('app').controller('app.views.project.editProject', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'abp.services.app.projectType', 'abp.services.app.project', 'id', 'abp.services.app.support', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, userService, projectTypeService, projectService, id, supportService, masterListservice) {
            var vm = this;
            vm.loading = false;
            vm.project = {};
            vm.imm_sup = [];
            vm.roleName = "Marketing Leader";
            vm.companylist = [];
            vm.projecttypelist = [];
            vm.cname = [];
            vm.exsistProject = {};
            $scope.editDetails = true;

            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                    console.log(vm.cname);
                });
            }
            function edit() {
                projectService.getProjectEdit({ id: id }).then(function (result) {
                    vm.project = result.data;

                    vm.project.beneficiaryCompanyId = vm.project.beneficiaryCompanyId + "";
                    if (vm.project.clientId == "" || vm.project.clientId == undefined || vm.project.clientId == 0) {

                        vm.project.clientId = 0;
                    }
                    else {
                        vm.project.clientId = vm.project.clientId + "";
                    }
                    

                    if (vm.project.marketing_LeaderId == null || vm.project.marketing_LeaderId == "undefined") {
                        vm.project.marketing_LeaderId = "0";
                    }
                    else {
                        vm.project.marketing_LeaderId = vm.project.marketing_LeaderId + "";
                    }
                    vm.project.startDate = moment(vm.project.startDate);
                    if (vm.project.endDate == null) {
                        vm.project.endDate = null;
                    }
                    else {
                        vm.project.endDate = moment(vm.project.endDate);
                    }
                    if (vm.project.teamDeadline == null) {
                        vm.project.teamDeadline = null;
                    }
                    else {
                        vm.project.teamDeadline = moment(vm.project.teamDeadline);
                    }
                    if (vm.project.actualEndDate == null) {
                        vm.project.actualEndDate = null;
                    }
                    else {
                        vm.project.actualEndDate = moment(vm.project.actualEndDate);
                    }
                    getClientName();
                    getprojectDetails();
                    getCompany();
                    getMark_Leader();
                    getProject_type();
                });
            }



            function getProject_type() {

                masterListservice.getProjectType()
                    .then(function (result) {
                        vm.projecttypelist = result.data;
                    });
            }
            $scope.projectDetails = [
                {
                    projecttypeId: "",
                    typeprice: "",
                    hours: ""
                }]
            function getprojectDetails() {
                projectService.getprojectDetailsList({ id: id })
                    .then(function (result) {
                        $scope.projectDetails = result.data.items;
                        for (var i = 0; i < $scope.projectDetails.length; i++) {
                            $scope.projectDetails[i].projecttypeId = $scope.projectDetails[i].projecttypeId + "";

                        }

                    });
            }

            function getMark_Leader() {

                userService.getImmediateSupervisor(vm.roleName)
                    .then(function (result) {

                        vm.imm_sup = result.data;
                        //vm.project.marketing_LeaderId = "0";
                    });
            }

            function getCompany() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;
                    });
            }
            vm.save = function () {
                if (vm.project.marketing_LeaderId == "") {
                    vm.project.marketing_LeaderId = null;
                }
                if (vm.project.beneficiaryCompanyId == "" || vm.project.beneficiaryCompanyId == undefined || vm.project.beneficiaryCompanyId == null) {
                    abp.notify.error("Please select company.");
                    return;
                }
                if (vm.project.projectName == "" || vm.project.projectName == undefined || vm.project.projectName == null) {
                    abp.notify.error("Please enter project name.");
                    return;
                }
                if (vm.project.description == "" || vm.project.description == undefined || vm.project.description == null) {
                    abp.notify.error("Please enter description.");
                    return;
                }
                if (vm.project.startDate == "" || vm.project.startDate == undefined || vm.project.startDate == null) {
                    abp.notify.error("Please enter start date.");
                    return;
                }
                if (vm.project.endDate == "" || vm.project.endDate == undefined || vm.project.endDate == null) {
                    vm.project.endDate == "";
                }
                if (vm.project.teamDeadline == "" || vm.project.teamDeadline == undefined || vm.project.teamDeadline == null) {
                    vm.project.teamDeadline == "";
                }
                if (vm.project.clientId == "" || vm.project.clientId == undefined || vm.project.clientId == null) {
                    vm.project.clientId = 0;
                }
               
                vm.loading = true;
                vm.exsistProject.id = vm.project.id;
                vm.exsistProject.projectName = vm.project.projectName;
                projectService.projectExsistence(vm.exsistProject).then(function (result) {

                    if (!result.data) {
                        projectService.updateProject(vm.project).then(function () {
                            abp.notify.success(App.localize('Project ' + vm.project.projectName + ' Saved Successfully '));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('Project ' + vm.project.projectName + ' already Exist '));
                        vm.loading = false;
                    }
                });

            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };
            var init = function () {
                //getClientName();
                //getProject_type();
                //getMark_Leader();
                //getCompany();
                edit();
            }
            init();
        }
    ]);
})();