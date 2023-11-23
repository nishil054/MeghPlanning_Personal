(function () {
    angular.module('app').controller('app.views.category.Edit', [
        '$scope','$uibModalInstance', 'abp.services.app.knowledgeCenter','abp.services.app.category', 'id',
        function ($scope,$uibModalInstance, knowledgeCenterService, categoryService, id) {
            var vm = this;
            vm.saving = false;
            vm.items = {};
            vm.maxlength = 500;
            vm.loading = false;
            var init = function () {

                vm.saving = true;
                categoryService.getCategoryForEdit({ id: id })
                    .then(function (result) {
                        vm.items = result.data;
                        getTeam();
                    });
            };
            function getTeam() {
                knowledgeCenterService.getTeams()
                    .then(function (result) {
                        vm.teamList = result.data;
                    });
            }
            vm.save = function () {
                vm.saving = true;
                vm.loading = true;
                categoryService.categoryExsistenceById(vm.items).then(function (result) {
                    if (!result.data) {
                        categoryService.updateCategory(vm.items).then(function () {
                            abp.notify.success(App.localize('UpdatedSuccessfully'));
                            $uibModalInstance.close();
                        });
                    } else {
                        abp.notify.error(App.localize('Category Already Exist.'));
                        vm.loading = false;
                    }
                });
            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };
            init();
        }
    ]);
})();