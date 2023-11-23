

(function () {
    angular.module('app').controller('app.views.users.editTerminate', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'id',
        function ($scope, $uibModalInstance, userService, id) {
            var vm = this;
            vm.user = {};
            vm.home.lastdate = moment();
            //  vm.startDate = moment().add(3, 'month');
            vm.user = {
                isActive: true
            };
            
            
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
                                vm.home.lastdate = moment(vm.home.lastdate);
                                setAssignedRoles(vm.user, vm.roles);
                            });
                    });
            }

            vm.save = function () {

                userService.updateUserTerminate(vm.home)
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

            init();

        }
    ]);
})();