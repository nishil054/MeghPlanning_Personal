﻿(function () {
    angular.module('app').controller('app.views.roles.editModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.role', 'id',
        function ($scope, $uibModalInstance, roleService, id) {
            var vm = this;
            $scope.btndisable = false;
            vm.role = {};
            vm.permissions = [];

            var setAssignedPermissions = function (role, permissions) {
                for (var i = 0; i < permissions.length; i++) {
                    var permission = permissions[i];
                    permission.isAssigned = $.inArray(permission.name, role.grantedPermissions) >= 0;
                }
            }

            function init() {
                roleService.getAllPermissions()
                    .then(function (result) {
                        vm.permissions = result.data.items;
                    }).then(function () {
                        roleService.get({ id: id })
                            .then(function (result) {
                                vm.role = result.data;
                                setAssignedPermissions(vm.role, vm.permissions);
                            });
                    });
            }

            vm.save = function () {
                $scope.btndisable = true;
                var assignedPermissions = [];

                for (var i = 0; i < vm.permissions.length; i++) {
                    var permission = vm.permissions[i];
                    if (!permission.isAssigned) {
                        continue;
                    }

                    assignedPermissions.push(permission.name);
                }

                vm.role.grantedPermissions = assignedPermissions;
                roleService.update(vm.role)
                    .then(function () {
                        abp.notify.info(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                        $scope.btndisable = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();