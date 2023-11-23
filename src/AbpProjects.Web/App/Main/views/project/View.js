(function () {
    angular.module('app').controller('app.views.project.view', [
        '$scope', '$state', '$uibModal', '$stateParams', 'abp.services.app.project',
        function ($scope, $state, $uibModalInstance, $stateParams, projectService) {
            var vm = this;
            vm.data = {};
            vm.projectDetails = [];
            vm.data.id = $stateParams.id;

            vm.getDataView = function () {
                projectService.getProjectViewById({
                    id: vm.data.id
                }).then(function (result) {
                    vm.data = result.data;
                    console.log(result);
                });
            }

          
            function getprojectDetails() {
                abp.ui.setBusy();
                projectService.getprojectDetailsList({ id: vm.data.id })
                    .then(function (result) {
                        vm.projectDetails = result.data.items;
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            } 
            
           
            function init() {
                vm.getDataView();
                getprojectDetails();
            }

            init();

            vm.cancel = function () {
                $state.go('project');
            };
        }
    ]);
})();