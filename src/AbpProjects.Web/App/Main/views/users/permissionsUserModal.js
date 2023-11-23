(function () {
    angular.module('app').controller('app.views.users.permissionsUserModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'id',
        function ($scope, $uibModalInstance, userService, user) {

            var vm = this;

            vm.saving = false;

            vm.permissionEditData = null;
            vm.userid = user;


            vm.save = function () {
                vm.saving = true;
                userService.updateUserPermissions({
                    id: user,
                    //id: vm.user.id,
                    grantedPermissionNames: vm.permissionEditData.grantedPermissionNames
                }).then(function () {
                    //$state.go('users');
                    //  abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.dismiss();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                //$state.go('users');
                $uibModalInstance.dismiss();
            };

            function loadPermissions() {

                userService.getUserPermissionsForEdit({
                    id: user
                    // id: vm.user.id
                }).then(function (result) {

                    vm.permissionEditData = result.data;
                });

                userService.getUserById({
                    Id: user
                }).then(function (result) {
                    console.log(result.data);
                    vm.user = result.data;
                });
            }

            loadPermissions();
        }
    ]);
})();