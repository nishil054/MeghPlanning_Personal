

(function () {
    angular.module('app').controller('app.views.users.editModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'abp.services.app.masterList', 'id',
        function ($scope, $uibModalInstance, userService, masterListservice, id) {
            var vm = this;
            $scope.btndisable = false;
            //  vm.startDate = moment().add(3, 'month');
            vm.user = {
                isActive: true,
                teamname: ""
            };
            vm.user = {};
            vm.companylist = {};
            vm.imm_Team = {};
            vm.imm_sup = [];

            //var imp;
            //vm.user.id = $stateParams.id;
            vm.maxDate = moment().add(1, 'month').subtract(20, 'year');
            vm.roleName = "Supervisor";

            var setAssignedRoles = function (user, roles) {
                for (var i = 0; i < roles.length; i++) {
                    var role = roles[i];
                    role.isAssigned = $.inArray(role.name, user.roles) >= 0;
                }
            }
            function getSupervisor() {

                userService.getImmediateSupervisor(vm.roleName)
                    .then(function (result) {

                        vm.imm_sup = result.data;
                        //vm.user.immediate_supervisorId = "0";
                    });
            }
            function getCompanyData() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;

                    });
            }
            function getTeamData() {

                userService.getTeam()
                    .then(function (result) {

                        vm.imm_Team = result.data;

                    });
            }
            $scope.changeTeam = function (id) {

                for (var i = 0; i < vm.imm_Team.length; i++) {
                    if (vm.imm_Team[i].id == id) {
                        vm.user.teamname = vm.imm_Team[i].teamName;
                        if (vm.user.teamname != "Marketing") {
                            vm.user.targetAmount = "";
                        }
                    }

                }
            }
            var init = function () {
             


                userService.getRoles()
                    .then(function (result) {
                        vm.roles = result.data;
                        userService.get({ id: id })
                            .then(function (result) {
                                vm.user = result.data;
                                vm.user.companyId = vm.user.companyId + "";
                                vm.user.birthdate = moment(vm.user.birthdate);
                                vm.user.joiningdate = moment(vm.user.joiningdate);
                                vm.user.next_Renewaldate = moment(vm.user.next_Renewaldate);
                                if (vm.user.immediate_supervisorId != null) {
                                    vm.user.immediate_supervisorId = vm.user.immediate_supervisorId + "";
                                }

                                $scope.changeTeam(vm.user.teamId);
                                //if (vm.user.teamname)
                                if (vm.user.immediate_supervisorId == null || vm.user.immediate_supervisorId == "undefine") {
                                    vm.user.immediate_supervisorId = "0";

                                }
                                else {
                                    vm.user.immediate_supervisorId = vm.user.immediate_supervisorId + "";
                                }
                                // vm.user.immediate_supervisorId = vm.user.immediate_supervisorId + "";
                                vm.user.teamId = vm.user.teamId + "";
                                setAssignedRoles(vm.user, vm.roles);

                                getCompanyData();
                                getTeamData();
                                getSupervisor()
                            });
                    });
            }

            vm.save = function () {
                $scope.btndisable = true;
                var assingnedRoles = [];
              
                for (var i = 0; i < vm.roles.length; i++) {
                    var role = vm.roles[i];
                    if (!role.isAssigned) {
                        continue;
                    }

                    assingnedRoles.push(role.name);
                }

                var i = assingnedRoles.length;

                if (assingnedRoles.length > 0) {

                    vm.user.roleNames = assingnedRoles;

                    if (vm.user.immediate_supervisorId == "0") {
                        vm.user.immediate_supervisorId = null;
                    }
                    else {
                        vm.user.immediate_supervisorId = vm.user.immediate_supervisorId;
                    }
                    userService.update(vm.user)
                        .then(function () {
                            abp.notify.success(App.localize('SavedSuccessfully'));
                            $uibModalInstance.close();
                        });
                }
                else {
                    abp.notify.error(App.localize('UserRolesShouldbeSelected'));
                    $scope.btndisable = false;
                    vm.saving = false;
                }
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            vm.CheckNumber = function () {
                //console.log(event.keyCode);
                if (event.keyCode === 46) {

                }

                else if (isNaN(event.key) || event.key === ' ' || event.key === '') {
                    event.returnValue = '';
                }
            };

            init();

        }
    ]);
})();