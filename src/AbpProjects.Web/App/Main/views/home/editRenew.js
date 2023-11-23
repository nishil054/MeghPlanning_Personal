

(function () {
    angular.module('app').controller('app.views.home.editRenew', [
        '$scope', '$http', '$uibModalInstance', 'abp.services.app.user', 'id',
        function ($scope, $http, $uibModalInstance, userService, id) {
            //debugger;
            var vm = this;
            //  vm.startDate = moment().add(3, 'month');
            vm.user = {
                isActive: true
            };
            vm.home = {};
            vm.roleName = "Supervisor";

            vm.roles = [];

            var setAssignedRoles = function (user, roles) {
                for (var i = 0; i < roles.length; i++) {
                    var role = roles[i];
                    role.isAssigned = $.inArray(role.name, user.roles) >= 0;
                }
            }


            var init = function () {
                userService.getRoles()
                    .then(function (result) {
                        vm.roles = result.data;
                        userService.get({ id: id })
                            .then(function (result) {
                                vm.home = result.data;
                                vm.home.next_Renewaldate = moment(vm.home.next_Renewaldate);
                                setAssignedRoles(vm.user, vm.roles);
                            });
                    });
            }

            vm.save = function () {
                //debugger;

                userService.updateUserRenew(vm.home)
                    .then(function (result) {
                        //debugger;
                        console.log(result);
                        
                        abp.notify.success(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    }).finally(function () {
                    });

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