(function () {
    angular.module('app').controller('app.views.category.Details', [
        '$scope', '$uibModalInstance', '$stateParams', 'abp.services.app.category', 'id',
        function ($scope, $uibModalInstance, $stateParams, categoryService, item) {
            var vm = this;
            vm.saving = false;
            vm.items = {};
            
            var init = function () {
                
                vm.saving = true;
                categoryService.getCategoryForDetail({ id: item })
                    .then(function (result) {
                        vm.items = result.data;
                        vm.items.teamname=vm.bindteamname(vm.items.teams);
                    });
            };

            vm.bindteamname = function (teamnamelist) {
                var teamname = "";
                angular.forEach(teamnamelist, function (v1, k1) {
                    teamname += v1 + ",";
                });
                return teamname.substring(0, teamname.length - 1);
            }

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            init();
        }
    ]);
})();