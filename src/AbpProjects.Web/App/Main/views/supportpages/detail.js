(function () {

    angular.module('app').controller('app.views.supportpages.detail', [
        '$scope', '$stateParams', 'abp.services.app.support',
        function ($scope, $stateParams, supportService,) {
            var vm = this;
            vm.id = $stateParams.id;
          /*  vm.complainDetails = [];*/
            vm.saving = false;
            vm.faq = {};
           
                   
              
            var init = function () {
                vm.get();
                //complaintService.getCommentDetailsById({ id: $stateParams.id })
                //    .then(function (result) {
                //        vm.complainDetails = result.data.items;
                        
                //        console.log(vm.complainDetails);
                //    });
            }
            vm.get = function () {
                supportService.getServiceForEdit(vm.id).then(function (result) {
                    vm.faq = result.data;

                });
            }
           
            vm.refresh = function () {
                vm.get();
            };

            init();

        }

    ]);
})();