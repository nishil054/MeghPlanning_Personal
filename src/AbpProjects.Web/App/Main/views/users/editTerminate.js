

(function () {
    angular.module('app').controller('app.views.users.editTerminate', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'id',
        function ($scope, $uibModalInstance, userService, id) {
            //debugger;
            var vm = this;
           /* $scope.btndisable = false;*/
            vm.user = {};
            vm.user.lastdate = moment();
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
                                vm.user = result.data;
                                vm.user.lastdate = moment(vm.user.lastdate);
                                setAssignedRoles(vm.user, vm.roles);
                            });
                    });
            }

            vm.save = function () {
             /*   $scope.btndisable = true;*/
                userService.updateUserTerminate(vm.user)
                    .then(function (result) {
                        //debugger;
                        console.log(result);
                        
                        abp.notify.success(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    }).finally(function () {
                        /*$scope.btndisable = false;*/
                    });

            };


            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();

        }
    ]);
})();