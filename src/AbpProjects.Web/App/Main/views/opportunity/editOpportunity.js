(function () {
    angular.module('app').controller('app.views.opportunity.editOpportunity', [
        '$scope', '$uibModalInstance', 'abp.services.app.opportunityService', 'id', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, opportunityService, id, masterListservice) {
            var vm = this;
            vm.loading = false;
            $scope.btndisable = false;
            vm.companylist = [];

            vm.save = function () {
                if (vm.opportunity.beneficiaryCompanyId == "" || vm.opportunity.beneficiaryCompanyId == undefined || vm.opportunity.beneficiaryCompanyId == null) {
                    abp.notify.error("Please select company.");
                    return;
                }
                $scope.btndisable = true;
                var assingnedprojectType = [];
                for (var i = 0; i < vm.projectTypes.length; i++) {
                    var projectType = vm.projectTypes[i];
                    if (!projectType.isAssigned) {
                        continue;
                    }
                    projectType.id = projectType.id + "";
                    assingnedprojectType.push(projectType.id);
                } 

                if (assingnedprojectType.length > 0) {
                    vm.opportunity.projectType = assingnedprojectType;

                    opportunityService.mobileOrEmailExsistenceById(vm.opportunity).then(function (result) {
                        if (!result.data) {
                            opportunityService.updateOpportunity(vm.opportunity)
                                .then(function () {
                                    abp.notify.success(App.localize('SavedSuccessfully'));
                                    $uibModalInstance.close();
                                    $scope.btndisable = false;
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
                        $scope.btndisable = false;
                     }
            };

            vm.getById = function () {
                abp.ui.setBusy();
                opportunityService.getOpportunityEdit({ id: id }).then(function (result) {
                    vm.opportunity = result.data;
                    if (vm.opportunity.nextactiondate == null) {
                        vm.opportunity.nextactiondate = null;
                    }
                    else {
                        vm.opportunity.nextactiondate = moment(vm.opportunity.nextactiondate);
                    }
                    setAssignedRoles(vm.opportunity, vm.projectTypes);

                    getCallCategory();
                    getCompany();
                    //getMarketingUsers();
                   // vm.opportunity.calllCategoryId = vm.opportunity.calllCategoryId + "";
                    $("#ddlCategory").select2("val", vm.opportunity.calllCategoryId);
                    $("#ddlcom").select2("val", vm.opportunity.beneficiaryCompanyId);
                    //vm.opportunity.assignUserId = vm.opportunity.assignUserId + "";
                }).finally(function () {
                    abp.ui.clearBusy();
                });;
            }

            var setAssignedRoles = function (user, protype) {
                for (var i = 0; i < protype.length; i++) {
                    var projectTypes = protype[i];
                    projectTypes.isAssigned = $.inArray(projectTypes.id, user.projectTypeName) >= 0;
                }
            }

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            function getCompany() {
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
                    //        vm.callCategory.splice(index, 1);
                    //});
                });
            }
            function getMarketingUsers() {
                opportunityService.getMarketingUsers({}).then(function (result) {
                    vm.assignUsers = result.data.items;
                });
            }

            init = function () {
                getCallCategory();
                getProjectType();
                //getMarketingUsers();
                vm.getById();
                getCompany();
            }

            init();
        }
    ]);
})();