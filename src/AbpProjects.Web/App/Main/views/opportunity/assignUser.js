(function () {
    angular.module('app').controller('app.views.opportunity.assignUser', [
        '$scope', '$uibModalInstance', 'abp.services.app.opportunityService', 'id',
        function ($scope, $uibModalInstance, opportunityService, id) {
            var vm = this;
            vm.loading = false;
            vm.opportunity = {};
            vm.opportunity.id = id;

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };
            function getMarketingUser() {
                opportunityService.getUserMarketingLead()
                    .then(function (result) {
                        vm.ddlmarketingList = result.data.items;
                    });

            }
            vm.assignUser = function () {
                abp.ui.setBusy();
                opportunityService.assignUser(vm.opportunity)
                    .then(function () {
                        abp.notify.success(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    });
                abp.ui.clearBusy();
            };

            init = function () {
                getMarketingUser()
            }
            init();
        }
    ]);
})();