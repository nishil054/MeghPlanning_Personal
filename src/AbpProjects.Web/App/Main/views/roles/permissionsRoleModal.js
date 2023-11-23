(function () {
    angular.module('app').controller('app.views.roles.permissionsroleModal', [
        '$scope', '$stateParams', '$state', '$uibModalInstance', 'abp.services.app.role', 'id',
        function ($scope, $stateParams, $state, $uibModalInstance, roleService, id) {
            //'$scope', '$uibModalInstance', 'abp.services.app.user', 'user',
            //function ($scope, $uibModalInstance, roleService, user) {
            var vm = this;

            vm.saving = false;
            vm.resettingPermissions = false;
            //vm.user = user;
            //user = null;
            vm.permissionEditData = null;
            //vm.fileredpermissionEditData = [];
            vm.roleid = id;

            vm.save = function () {
                vm.saving = true;
                vm.permissionEditData.id = vm.roleid;
                vm.permissionEditData.grantedPermissionNames = vm.permissionEditData.grantedPermissionNames;
                roleService.updaterolePermissions(vm.permissionEditData).then(function () {
                    $uibModalInstance.dismiss();
                }).finally(function () {
                    vm.saving = false;
                    //$uibModalInstance.dismiss();
                });
            };

            vm.cancel = function () {
                //$state.go('users');
                $uibModalInstance.dismiss();
            };
           
            function loadPermissions(searchValue) {
                roleService.getRolePermissionsForEdit({
                    id: vm.roleid
                }).then(function (result) {
                    //vm.fileredpermissionEditData = [];
                    vm.permissionEditData = result.data;
                    vm.role = result.data.Rolename;
                    //if (searchValue != null && searchValue != "") {
                    //    var data = result.data.permissions.filter(c => c.displayName.toLowerCase().includes(searchValue.toLowerCase()));
                      
                    //    angular.forEach(data, function (item, key) {
                    //        if (item.parentName == null) {
                    //            vm.fileredpermissionEditData.push(item);
                    //        }
                    //        else {
                    //            angular.forEach(result.data.permissions, function (itemList, key) {
                    //                if (itemList.displayName == item.displayName) {
                    //                    vm.fileredpermissionEditData.push(item);
                    //                }
                    //                if (itemList.name == item.parentName) {
                    //                    vm.fileredpermissionEditData.push(itemList);
                    //                }
                    //            });
                    //        }
                    //    });

                    //    //console.log('vm.fileredpermissionEditData', vm.fileredpermissionEditData);
                    //    vm.permissionEditData = result.data;
                    //    vm.permissionEditData.permissions = vm.fileredpermissionEditData;
                    //    vm.role = result.data.Rolename;
                    //}
                    //else {
                    //    vm.permissionEditData = result.data;
                    //    vm.role = result.data.Rolename;
                    //}
                });

            }

            loadPermissions("");

            //vm.refreshGrid = function (val) {
            //    loadPermissions(val);
            //}
        }
    ]);
})();