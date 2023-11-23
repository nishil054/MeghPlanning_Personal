
(function () {
    angular.module('app').controller('app.views.users.createModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'abp.services.app.role', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, userService, roleservice, masterListservice) {
            var vm = this;
            $scope.btndisable = false;
            vm.user = {
                isActive: true,
               teamname:""
            };
            //vm.user.next_Renewaldate.startDate = moment().add(3, 'month');
            vm.startDate = moment().add(1, 'year');
            //vm.user.next_Renewaldate = vm.startDate;

            //vm.minDate = moment().subtract(20, 'year');
            vm.maxDate = moment().add(1, 'month').subtract(20, 'year');

            //$("#dob").datepicker({ minDate: '-30Y', maxDate: '-Y' });
            vm.roles = [];
            vm.roleName = "Supervisor";
            vm.imm_sup = [];
            vm.companylist = [];
            vm.imm_Team = [];
            
            function getRoleData() {
                userService.getRoles()
                    .then(function (result) {
                        vm.roles = result.data;
                    });
            }

            //immediate supervisor

            function getSupervisor() {

                userService.getImmediateSupervisor(vm.roleName)
                    .then(function (result) {
                        vm.imm_sup = result.data;
                        vm.user.immediate_supervisorId = "0";
                    });
            }

            // Company Details


            function getCompanyData() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;

                    });
            }

            // Team

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
                    }
                    else {
                       // vm.user.teamname = "";
                    }
                }
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

                if (assingnedRoles.length > 0) {
                    vm.user.roleNames = assingnedRoles;

                    if (vm.user.immediate_supervisorId == "0") {
                        vm.user.immediate_supervisorId = null;
                    }
                    userService.create(vm.user)
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

            getRoleData();
            getSupervisor()
            getCompanyData();
            getTeamData();
            //  funallow_decimal("allow_decimal");
            //  funAllow_Alpha("allow_alpha");
        }
    ]);
})();