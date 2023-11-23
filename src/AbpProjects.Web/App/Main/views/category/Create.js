(function () {
    angular.module('app').controller('app.views.category.Create', [
        '$scope','$uibModalInstance', 'abp.services.app.knowledgeCenter','abp.services.app.category',
        function ($scope,$uibModalInstance, knowledgeCenterService, categoryService) {
            var vm = this;
            vm.categories = {};
            vm.maxlength = 500;
            vm.loading = false;

            vm.close = function () {
                $uibModalInstance.dismiss();
            };

            function getTeams() {
                knowledgeCenterService.getTeams()
                    .then(function (result) {
                        vm.teamList = result.data;
                    });
            }

            var init = function () {
                getTeams();
            }

            vm.save = function () {
                vm.loading = true;
                vm.saving = true;
                categoryService.categoryExsistence(vm.categories).then(function (result) {
                    if (!result.data) {
                        categoryService.createCategory(vm.categories).then(function () {
                            abp.notify.success(App.localize('SavedSuccessfully'));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('Category Already Exist'));
                        vm.loading = false;
                    }
                });
            };

            init();
        }
    ]);
})();