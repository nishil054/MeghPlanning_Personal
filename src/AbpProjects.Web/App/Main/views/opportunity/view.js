(function () {
    angular.module('app').controller('app.views.opportunity.reasonView', [
        '$scope', '$uibModalInstance', 'abp.services.app.opportunityService', 'id',
        function ($scope, $uibModalInstance, opportunityService, id) {
            var vm = this;
            vm.loading = false;
            vm.comma = ",";

            vm.getById = function () {
                abp.ui.setBusy();
                opportunityService.getOpportunityDetails({ id: id }).then(function (result) {
                    vm.opportunity = result.data;
                }).finally(function () {
                    abp.ui.clearBusy();
                });
            }
            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };
            

            init = function () {
                vm.getById();
            }
           $scope.inter = function (rolenamelist) {
                var projectType_Name = "";
                angular.forEach(rolenamelist, function (v1, k1) {
                    projectType_Name += v1 + ",";
                });
                return projectType_Name.substring(0, projectType_Name.length - 1);
            }

            init();
        }
    ]);
})();