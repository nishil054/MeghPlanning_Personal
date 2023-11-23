(function () {
    angular.module('app').controller('app.views.supportpages.servicesearch', [
        '$scope', '$state', '$timeout', '$http', 'abp.services.app.support',
        function ($scope, $state, $timeout, $http, supportService) {
            var vm = this;
            vm.domainName = "ha";
            vm.selecteddomainname = function (selected, addActivity) {
                if (selected) {
                    vm.domainName = selected.originalObject.domainName;
                }
            };
            vm.searchAPI = function (userInputString, timeoutPromise) {
                return supportService.getDomainNameList(userInputString).then(function (result) {
                    return result.data;
                });
            };
        }
    ]);
})();