(function () {
    angular.module('app').controller('app.views.opportunity.addFollow', [
        '$scope', '$state', '$uibModal', '$stateParams', 'uiGridConstants', 'abp.services.app.opportunityService', '$window', 'abp.services.app.masterList',
        function ($scope, $state, $uibModal, $stateParams, uiGridConstants, opportunityService, $window, masterListservice) {
            var vm = this;
            vm.data = {};
            vm.opportunity = {};
            vm.followuptypelist = [];
            vm.data.id = $stateParams.id;
            vm.pageName = $stateParams.name;
            vm.callCategId = $stateParams.catid;
            vm.save = function () {
                abp.ui.setBusy();

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
                    if (vm.opportunity.calllCategoryId == 9) {
                        vm.opportunity.opporutnityid = vm.data.id;
                        if (vm.opportunity.assignUserId == undefined || vm.opportunity.assignUserId == null || vm.opportunity.assignUserId == "") {
                            abp.notify.error("Please select marketing leader");
                            $scope.btndisable = false;
                            vm.saving = false;
                            abp.ui.clearBusy();
                            return;
                        }
                    }
                    else {
                        vm.opportunity.assignUserId = 0;
                    }
                    $scope.btndisable = true;
                    vm.opportunity.projectType = assingnedprojectType;
                    opportunityService.updateFollowUp(vm.opportunity)
                        .then(function () {
                            abp.notify.success(App.localize('SavedSuccessfully'));
                            getFollowUpHistory();
                            getFollowUpDetail();
                            cleardata();
                            $state.go('myopportunity');
                            // $state.go('inquiry');
                        });
                }
                else {
                    if (vm.opportunity.calllCategoryId == 8) {
                        vm.opportunity.assignUserId = 0;
                        $scope.btndisable = true;
                        vm.opportunity.opporutnityid = vm.data.id;
                        opportunityService.updateFollowUp(vm.opportunity)
                            .then(function () {
                                abp.notify.success(App.localize('SavedSuccessfully'));
                                getFollowUpHistory();
                                getFollowUpDetail();
                                cleardata();
                                $state.go('myopportunity');
                            });
                    }
                    else if (vm.opportunity.calllCategoryId == 7 || vm.opportunity.calllCategoryId == 10) {
                        vm.opportunity.assignUserId = 0;
                        vm.opportunity.opporutnityid = vm.data.id;
                        $scope.btndisable = true;
                        opportunityService.updateFollowUp(vm.opportunity)
                            .then(function () {
                                abp.notify.success(App.localize('SavedSuccessfully'));
                                cleardata();
                                $window.location.reload();
                                //$state.go('addFollow', { id: $stateParams.id, name: 'myopportunity' });
                            });
                    }
                    else if (vm.opportunity.calllCategoryId == 6) {
                        vm.opportunity.assignUserId = 0;
                        vm.opportunity.opporutnityid = vm.data.id;
                        $scope.btndisable = true;
                        opportunityService.updateFollowUp(vm.opportunity)
                            .then(function () {
                                abp.notify.success(App.localize('SavedSuccessfully'));
                                cleardata();
                                $window.location.reload();
                            });
                    }
                    else {
                        abp.notify.error(App.localize('InterestedInSelected'));
                        $scope.btndisable = false;
                        vm.saving = false;
                    }
                }
                abp.ui.clearBusy();
            }

            function getFollowUpHistory() {
                opportunityService.followUpHistoryData(vm.data.id).then(function (result) {
                    vm.followuphistory = result.data.items;
                })
            }

            function cleardata() {
                vm.opportunity.calllCategoryId = "";
                vm.projectTypes = [];
                vm.callCategory = [];
                getCallCategory();
                getProjectType();
                vm.opportunity.comment = null;
                vm.opportunity.nextactiondate = null;
                vm.opportunity.expectedclosingdate = null;

            }

            vm.bindprojecttypename = function (ptlist) {
                var name = "";
                angular.forEach(ptlist, function (v1, k1) {
                    name += v1.name + ",";
                });
                return name.substring(0, name.length - 1);
            }

            $scope.closedCallCategory = function (callCategoryId, data) {
                if (callCategoryId == "6") {
                    vm.ddlmarketingList = null;
                    vm.opportunity.marketingleadId = null;
                    ///----hide popup
                    //abp.message.confirm(
                    //    "Are you sure want to close the Inquiry?", " ",
                    //    function (result) {
                    //        if (result) {
                    //            opportunityService.updateFollowUpClosed({ id: data.opporutnityid })
                    //                .then(function () {
                    //                    $state.go('inquiry');
                    //                });
                    //        }
                    //    });
                    //-------end
                    //var modalInstance = $uibModal.open({
                    //    templateUrl: '/App/Main/views/opportunity/createProject.cshtml',
                    //    controller: 'app.views.opportunity.createProject as vm',
                    //    backdrop: 'static',
                    //    resolve: {
                    //        id: function () {
                    //            return data.opporutnityid;
                    //        }
                    //    }
                    //});
                    //modalInstance.rendered.then(function () {
                    //    $.AdminBSB.input.activate();
                    //});
                    //modalInstance.result.then(function () {
                    //opportunityService.updateFollowUpClosed({ id: data.opporutnityid })
                    //    .then(function () {
                    //        $state.go('inquiry');
                    //    });
                    //});
                }
                else if (callCategoryId == "9") {
                    getMarketingUser();
                    getProjectType();
                } else {
                    vm.ddlmarketingList = null;
                    vm.opportunity.marketingleadId = null;
                }
            }

            function getFollowUpDetail() {
                abp.ui.setBusy();
                opportunityService.getFollowUpDetail({ id: vm.data.id }).then(function (result) {
                    vm.opportunity = result.data;
                    vm.opportunity.opporutnityid = result.data.opporutnityid;
                    setAssignedRoles(vm.opportunity, vm.projectTypes);
                    if (vm.opportunity.expectedclosingdate == null) {
                        vm.opportunity.expectedclosingdate = null;
                    }
                    else {
                        vm.opportunity.expectedclosingdate = moment(vm.opportunity.expectedclosingdate);
                    }
                    $("#ddlCategory").select2("val", vm.opportunity.calllCategoryId);
                    if (vm.opportunity.calllCategoryId=="9") {
                        getMarketingUser();
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
            var setAssignedRoles = function (user, protype) {
                for (var i = 0; i < protype.length; i++) {
                    var projectTypes = protype[i];
                    projectTypes.isAssigned = $.inArray(projectTypes.id, user.projectTypeName) >= 0;
                }
            }


            function getProjectType() {
                opportunityService.getProjectType({}).then(function (result) {
                    vm.projectTypes = result.data.items;
                    if (vm.pageName == 'myopportunity') {
                        opportunityService.getOpportunityDetails({ id: vm.data.id }).then(function (result) {
                            vm.opportunity.companyName = result.data.companyName;
                            vm.opportunity.beneficiaryCompany = result.data.beneficiaryCompany;
                            vm.opportunity.personName = result.data.personName;
                            vm.opportunity.emailId = result.data.emailId;
                            vm.opportunity.mobileNumber = result.data.mobileNumber;
                        })
                    }
                    else if (vm.pageName == 'inquiry') {
                    }
                    else {
                        getFollowUpDetail();
                    }

                });
            }

            function getFollowUpType() {
                masterListservice.getFollowUpType({}).then(function (result) {
                    vm.followuptypelist = result.data;
                    console.log("getFollowUpType");
                });
            }

            function getMarketingUser() {
                opportunityService.getUserMarketingLead()
                    .then(function (result) {
                        vm.ddlmarketingList = result.data.items;
                    });

            }
            function getCallCategory() {
                opportunityService.getCallCategoryInq({}).then(function (result) {
                    vm.callCategory = result.data.items;
                });
            }
            function getCallCategoryOpportunity() {
                opportunityService.getCallCategoryOpportunity({}).then(function (result) {
                    vm.callCategory = result.data.items;

                });
            }

            vm.cancel = function () {
                $window.history.back();
            };
            init();
            if (vm.pageName == 'myopportunity') {
                abp.ui.setBusy();
                //vm.opportunity.calllCategoryId = 7;
                vm.opportunity.calllCategoryId = vm.callCategId;
                abp.ui.clearBusy();

            }
            if (vm.pageName == 'inquiry') {
                abp.ui.setBusy();
                vm.opportunity.calllCategoryId = 9;
                angular.element(document.getElementById('ddlCategory'))[0].disabled = true;
                abp.ui.clearBusy();
            }
            if (vm.pageName == 'generalopportunity') {
                abp.ui.setBusy();
                vm.opportunity.calllCategoryId = 7;
                abp.ui.clearBusy();

            }

            function init() {
                abp.ui.setBusy();
                getFollowUpType();
                getProjectType();
                if (vm.pageName == 'myopportunity') {
                    getCallCategoryOpportunity();
                    getFollowUpHistory();

                }
                else if (vm.pageName == 'inquiry') {
                    getCallCategoryOpportunity();

                }
                else {
                    getCallCategory();
                    getFollowUpHistory();
                }

                abp.ui.clearBusy();
            }
        }
    ]);
})();