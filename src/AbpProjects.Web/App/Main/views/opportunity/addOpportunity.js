(function () {
    angular.module('app').controller('app.views.opportunity.addOpportunity', [
        '$scope', '$uibModalInstance', 'abp.services.app.opportunityService', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, opportunityService, masterListservice) {
            var vm = this;
            $scope.btndisable = false;
            vm.loading = false;
            vm.projectTypes = [];
            vm.companylist = [];

            vm.save = function () {
                if (vm.opportunity.beneficiaryCompanyId == "" || vm.opportunity.beneficiaryCompanyId == undefined || vm.opportunity.beneficiaryCompanyId == null) {
                    abp.notify.error("Please select company.");
                    return;
                }
                $scope.btndisable = true;
                vm.loading = true;
                var assingnedprojectType = [];
                for (var i = 0; i < vm.projectTypes.length; i++) {
                    var projectType = vm.projectTypes[i];
                    if (!projectType.isAssigned) {
                        continue;
                    }
                    projectType.id = projectType.id + "";
                    assingnedprojectType.push(projectType.id);
                } projectType.id

                if (assingnedprojectType.length > 0) {
                    vm.opportunity.projectType = assingnedprojectType;
                    opportunityService.mobileOrEmailExsistence(vm.opportunity).then(function (result) {
                        if (!result.data) {
                            opportunityService.opportunityCreate(vm.opportunity)
                                .then(function () {
                                    abp.notify.success(App.localize('SavedSuccessfully'));
                                    $uibModalInstance.close();
                                    vm.loading = false;
                                });
                        }
                        else {
                            abp.notify.error('Mobile or EmailId already Exist ');
                            vm.loading = false;
                        }
                    });
                }
                else {
                    abp.notify.error(App.localize('InterestedInSelected'));
                    $scope.btndisable = false;
                    vm.saving = false;
                    vm.loading = false;
                }
            };

            //Bind Company Details
            function getCompanyList() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;
                    });
            }

            function getProjectType() {
                opportunityService.getProjectType({}).then(function (result) {
                    vm.projectTypes = result.data.items;
                });
            }
            function getCallCategory() {
                opportunityService.getCallCategoryInq({}).then(function (result) {
                    vm.callCategory = result.data.items;
                    //vm.callCategory.forEach((element, index) => {
                    //    if (element.id == 1 || element.id == 5 || element.id == 6)
                    //        vm.callCategory.splice(index,1);
                    //});
                });
            }
            function getMarketingUsers() {
                opportunityService.getMarketingUsers({}).then(function (result) {
                    vm.assignUsers = result.data.items;
                });
            }
            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            //$scope.filterValue = function ($event) {
            //    if (isNaN(String.fromCharCode($event.keyCode))) {
            //        $event.preventDefault();

            //        $scope.numberShow = true;
            //    }
            //    else {
            //        $scope.numberShow = false;
            //    }
            //};

            init = function () {
                getCallCategory();
                getProjectType();
                //getMarketingUsers();
                getCompanyList();
            }

            init();


        }
    ]);
})();