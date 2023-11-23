(function () {
    angular.module('app').controller('app.views.importuserstory.addUserStory', [
        '$scope', '$uibModalInstance', 'abp.services.app.importUserStoryDetails', 'abp.services.app.masterList', 'id',
        function ($scope, $uibModalInstance, userStoryService, masterListservice, id) {
            var vm = this;
            $scope.btndisable = false;
            vm.importuserstory = {};
            vm.importuserstory.id = id;
            vm.close = function () {
                $uibModalInstance.dismiss();
            };

            function getEmployee() {

                masterListservice.getEmployee()
                    .then(function (result) {

                        vm.employeelist = result.data;
                    });
            }

            vm.CheckNumber = function () {
                //console.log(event.keyCode);
                if (event.keyCode === 46) {

                }

                else if (isNaN(event.key) || event.key === ' ' || event.key === '') {
                    event.returnValue = '';
                }
            };

            vm.save = function () {
                
                if (vm.importuserstory.userStory == "" || vm.importuserstory.userStory == undefined) {
                    abp.notify.error('Please enter user story.');
                    return;
                }
                if (vm.importuserstory.devHours == null || vm.importuserstory.devHours == undefined || vm.importuserstory.devHours == "") {
                    abp.notify.error('Please enter developer hours.');
                    return;
                }
                else {
                    vm.importuserstory.developerHours = vm.importuserstory.devHours;
                }
                if (vm.importuserstory.expHours == null || vm.importuserstory.expHours == undefined || vm.importuserstory.expHours == "") {
                    abp.notify.error('Please enter expected hours.');
                    return;
                }
                else {
                    vm.importuserstory.expectedHours = vm.importuserstory.expHours;
                }
                $scope.btndisable = true;
                userStoryService.addNewUserStory(vm.importuserstory)
                    .then(function () {
                        abp.notify.success('Saved Successfully');
                        $uibModalInstance.close();
                        $scope.btndisable = false;
                    });
            }

            var init = function () {
                getEmployee();
            };
            init();
        }
    ]);
})();