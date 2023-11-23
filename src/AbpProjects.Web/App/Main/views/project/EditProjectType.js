(function () {
    angular.module('app').controller('app.views.project.editProjectType', [
        '$scope', '$state', '$uibModalInstance', '$stateParams', 'abp.services.app.project', 'abp.services.app.projectType', 'abp.services.app.masterList', 'id',
        function ($scope, $state, $uibModalInstance, $stateParams, projectService, projectTypeService, masterListservice, id) {
            var vm = this;
            $scope.btndisable = false;
            vm.data = {};
            vm.projectDetails = [];
            vm.projecttypelist = [];
            vm.inputs = {};
            vm.project = {};

            function getProject_type() {
                masterListservice.getProjectType()
                    .then(function (result) {
                        vm.projecttypelist = result.data;
                    });
            }
            vm.getEditProjectType = function () {
                projectService.getProjectTypeByProjectEdit(vm.inputs).then(function (result) {
                    vm.data = result.data;
                    vm.data.price = vm.data.price + "";
                    vm.data.isActive = vm.data.isOutSource;
                    vm.data.projecttypeId = vm.data.projecttypeId + "";
                    console.log(result);
                    vm.getprojectDetails();

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
            $scope.filterValueCost = function ($event, index) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                    document.getElementById('costforCompany').style.display = 'block';
                }
                else {
                    document.getElementById('costforCompany').style.display = 'none';
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

            vm.save = function () {
                /* vm.data.price = vm.data.price + "";*/
                //vm.data.isOutSource = false;
                $scope.btndisable = true;
                if (vm.data.price == null || vm.data.price == undefined || vm.data.price == "") {
                    abp.notify.error('Please enter price');
                    return;
                }
                // vm.data.hours = vm.data.hours + "";
                if (vm.data.hours == null || vm.data.hours == "undefined" || vm.data.hours == "") {
                    abp.notify.error('Please enter hours');
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
                projectService.updateProjectType(vm.data)
                    .then(function () {
                        abp.notify.success('Saved Successfully');
                        $uibModalInstance.close();
                        $scope.btndisable = false;
                        /*$state.go('project');*/
                    });

            }
            //vm.cancel = function () {
            //    $uibModalInstance.dismiss({});
            //    /*href = "javascript:history.back()"*/
            //  /*  $state.go('project');*/
            //}
            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            function init() {
                /* vm.inputs.id = $stateParams.id;*/
                vm.inputs.id = id;
                /*vm.inputs.projectTypeId = $stateParams.projectType;*/
                //vm.data.id = $stateParams.id;
                //vm.data.projectType = $stateParams.projectType;
                getProject_type();
                vm.getEditProjectType();

                /* getprojectDetails();*/
            }

            init();


        }
    ]);
})();