(function () {
    angular.module('app').controller('app.views.holiday.edit', [
        '$scope', '$http', '$state', '$uibModalInstance', 'abp.services.app.holiday', 'id',
        function ($scope, $http, $state, $uibModalInstance, holidayservice, id) {
            var vm = this;
            vm.loading = false;
            vm.saving = false;
            vm.holiday = {};


            function init() {
                holidayservice.getHolidayEdit({
                    id: id
                }).then(function (result) {
                    //debugger;
                    

                    vm.holiday = result.data;
                    vm.holiday.type = vm.holiday.type + "";
                    //if (vm.holiday.type == 1) {
                    //    vm.holiday.type == "1";

                    //} else {
                    //    vm.holiday.type == "0"

                    //}
                    vm.holiday.startDate = moment(vm.holiday.startDate);
                    vm.holiday.endDate = moment(vm.holiday.endDate);
                    console.log(result);



                });


            }
            init();

            vm.save = function () {
                vm.loading = true;
                console.log(vm.holiday);
                if (vm.holiday.endDate < vm.holiday.startDate) {
                    abp.notify.error("EndDate should not be a less than StartDate");
                    return;

                } else {
                    holidayservice.updateHoliday(vm.holiday)
                        .then(function () {
                            abp.notify.success(App.localize('SavedSuccessfully'));
                            $uibModalInstance.close();
                        }).finally(function () {
                        });

                }
                
            };

            

            

            //funAllow_Alpha("allow_alpha");
            //funallow_decimal("allow_decimal");

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
                //$state.go('invoices');
            };
        }
    ]);
})();