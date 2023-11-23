(function () {
    angular.module('app').controller('app.views.users.lockuserdetails', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'id','f',
        function ($scope, $uibModalInstance, userService, id,f) {
            var vm = this;
            $scope.flag = f;
            function getDetails() {
                userService.getProjectServiceCount({ id: id })
                    .then(function (result) {
                        vm.projectservice = result.data;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            var init = function () {
                getDetails();
            }
            init();
        }
    ]);
})();