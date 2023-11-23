(function () {
    angular.module('app').controller('app.views.users.adminChangePassword', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'id',
        function ($scope, $uibModalInstance, userService, id) {
            var vm = this;
            vm.user = {};
            vm.user.id = id;
          /*  $scope.btndisable = false;*/
            vm.save = function () {
             /*   $scope.btndisable = true;*/
                userService.updateChangePassword(vm.user)
                    .then(function () {
                        abp.notify.success(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    }).finally(function () {
                       /* $scope.btndisable = false;*/
                    });
            };


            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };


        }
    ]);
})();