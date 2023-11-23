(function () {
    angular.module('app').controller('app.views.reports.detail', [
        '$scope', '$state', '$window', '$stateParams', 'abp.services.app.timeSheet',
        function ($scope, $state, $window,$stateParams, timeSheetService,) {
            var vm = this;
            vm.id = $stateParams.id;
            vm.pid = $stateParams.pid;
            vm.complainDetails = [];
            vm.saving = false;
            vm.faq = {};
          
          /*  $window.sessionStorage.setItem("SavedString", vm.pid);*/

            //$scope.step_two = function () {
            //    $window.history.back();
            //};

            vm.openRedirectModal = function (f) {

                $state.go('projectreport', { pid:vm.pid });

            };

            vm.get = function () {
                abp.ui.setBusy();
                timeSheetService.workdetail(vm.id, vm.pid).then(function (result) {
                    vm.faq = result.data.items;
                    
                    console.log(vm.faq);
                   
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }


          
         
            vm.refresh = function () {
                vm.get();
            };
            var init = function () {
                vm.get();

                
            }

            init();

        }

    ]);
})();