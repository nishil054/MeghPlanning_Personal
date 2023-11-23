(function () {
    angular.module('app').controller('app.views.project.createProjectType', [
        '$scope', '$state', '$uibModalInstance', '$stateParams', 'abp.services.app.project', 'abp.services.app.projectType', 'abp.services.app.masterList', 'id',
        function ($scope, $state, $uibModalInstance, $stateParams, projectService, projectTypeService, masterListservice, id) {
            var vm = this;
            $scope.btndisable = false;
            vm.data = {};
            vm.projecttypelist = [];
            vm.project = {};
            vm.project.isActive = false;
            function getProject_type() {

                masterListservice.getProjectType()
                    .then(function (result) {
                        vm.projecttypelist = result.data;
                    });
            }

            vm.getprojectDetails = function () {
                projectService.getProjectEdit({ id: vm.data.projectId }).then(function (result) {
                    vm.project = result.data;
                });
            }

            $scope.filterValue = function ($event, index) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                    document.getElementById('isShowH').style.display = 'block';
                }
                else {
                    document.getElementById('isShowH').style.display = 'none';
                }

            };
            $scope.filterValuehours = function ($event, index) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                    document.getElementById('isShowHours').style.display = 'block';
                }
                else {
                    document.getElementById('isShowHours').style.display = 'none';
                }

            };
            $scope.filterValueCost = function ($event, index) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                    document.getElementById('costforCompany').style.display = 'block';
                }
                else {
                    document.getElementById('costforCompany').style.display = 'none';
                }

            };
            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            vm.save = function () {
                $scope.btndisable = true;
                if (vm.data.projecttypeId == "" || vm.data.projecttypeId == undefined) {
                    abp.notify.error('Please select project type.');
                    return;
                }
                if (vm.data.price == null || vm.data.price == undefined || vm.data.price == "") {
                    abp.notify.error('Please enter price.');
                    return;
                }
                if (vm.data.hours == null || vm.data.hours == undefined || vm.data.hours == "") {
                    abp.notify.error('Please enter hours.');
                    return;
                }
                if (vm.data.projecttypeId == "") {
                    abp.notify.error('Please select project type.');
                    return;
                }
                if (vm.data.isActive == true) {
                    vm.data.isOutSource = true;
                    if (vm.data.costforCompany == "" || vm.data.costforCompany == undefined || vm.data.costforCompany == null) {
                        abp.notify.error('Please enter company for cost.');
                        return;
                    }
                }
                else {
                    vm.data.isOutSource = null;
                    vm.data.costforCompany = null;
                }
                console.log(vm.data);
                projectService.createProjectType(vm.data)
                    .then(function () {
                        abp.notify.success('Saved Successfully');
                        $uibModalInstance.close();
                        $scope.btndisable = false;
                    });

            }


            function init() {
                /*vm.data.projectId = $stateParams.id;*/
                vm.data.projectId = id;
                getProject_type();
                vm.getprojectDetails();

            }
            init();


        }
    ]);
})();