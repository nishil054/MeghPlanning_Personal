(function () {
    var myApp = angular.module('app');
    myApp.controller('app.views.holiday.create', [
        '$scope', '$state', '$uibModalInstance', '$stateParams', '$filter', '$http', 'abp.services.app.holiday',
        function ($scope, $state, $uibModalInstance, $stateParams, $filter, $http, holidayservice) {
            //debugger;
            var vm = this;
            $scope.btndisable = false;
            vm.loading = false;
            vm.saving = false;
            vm.holiday = {};
            vm.holiday.id = $stateParams.id;
            vm.holiday.type = "1";



            vm.save = function () {
                $scope.btndisable = true;
                vm.loading = true;
                console.log(vm.holiday);
                if (vm.holiday.endDate < vm.holiday.startDate) {
                    abp.notify.error("EndDate should not be a less than StartDate");
                    return;

                } else {

                    vm.saving = true;
                    holidayservice.createHoliday(vm.holiday).then(function (result) {
                        //debugger;
                        console.log(result);
                        abp.notify.success(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                        $scope.btndisable = false;

                    }).finally(function () {
                        vm.saving = false;
                    });

                }

            };


            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            function init() {
                vm.itemsPerPage = 10;
                vm.skipCount = 0;
                vm.currentdirection = "desc";
                vm.requestParams = {
                    skipCount: vm.skipCount,
                    maxResultCount: vm.itemsPerPage,
                    sorting: "Title"
                };


            }
            init();
            
        }
    ]);
})();