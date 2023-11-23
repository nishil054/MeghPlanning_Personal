(function () {
    angular.module('app').controller('app.views.reports.opportunityDetails', [
        '$scope', '$state', '$stateParams', 'abp.services.app.opportunityService', 'uiGridConstants',
        function ($scope, $state, $stateParams, opportunityService, uiGridConstants) {
            var vm = this;
            vm.opportunity = {};
            vm.datahistory = [];
            vm.opportunity.id = $stateParams.id;
            $scope.isvisible = false;

            function getFollowUpDetail() {
                abp.ui.setBusy();
                opportunityService.getOpportunityDetails({ id: vm.opportunity.id }).then(function (result) {
                    vm.opportunity = result.data;
                    vm.opportunity.opporutnityid = result.data.opporutnityid;

                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            function getFollowUpHistory() {
                opportunityService.followUpHistoryData(vm.opportunity.id).then(function (result) {
                    vm.followuphistory = result.data.items;
                })
            }

            $scope.inter = function (rolenamelist) {
                var projectType_Name = "";
                angular.forEach(rolenamelist, function (v1, k1) {
                    projectType_Name += v1 + ",";
                });
                return projectType_Name.substring(0, projectType_Name.length - 1);
            }

            vm.bindprojecttypename = function (ptlist) {
                var name = "";
                angular.forEach(ptlist, function (v1, k1) {
                    name += v1.name + ",";
                });
                return name.substring(0, name.length - 1);
            }

            function init() {
                getFollowUpDetail();
                getFollowUpHistory();
            }

            init();

        }
    ]);
})();
       