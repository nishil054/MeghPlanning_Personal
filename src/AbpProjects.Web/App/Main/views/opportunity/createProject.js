
(function () {
    angular.module('app').controller('app.views.opportunity.createProject', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'abp.services.app.projectType', 'abp.services.app.project', 'abp.services.app.support', 'abp.services.app.masterList', 'id', 'abp.services.app.opportunityService',
        function ($scope, $uibModalInstance, userService, projectTypeService, projectService, supportService, masterListservice, id, opportunityService) {
            var vm = this;
            vm.cname = [];
            vm.loading = false;
            vm.saving = false;
            vm.project = {};
            vm.imm_sup = [];
            vm.projecttypelist = [];
            vm.companylist = [];
            vm.roleName = "Marketing Leader"

            vm.getById = function () {
                abp.ui.setBusy();
                opportunityService.getProjectTypesDetails({ id: id }).then(function (result) {
                    vm.opportunity = result.data;
                    vm.project.marketing_LeaderId = vm.opportunity.creatorUserId + "";
                    $("#ddlmarket").select2("val", vm.project.marketing_LeaderId);
                    $scope.projectDetails = vm.opportunity.projectType_Name;
                    console.log($scope.projectDetails);
                }).finally(function () {
                    abp.ui.clearBusy();
                });;
            }

            $scope.filterValue = function ($event, index) {
                if (isNaN(String.fromCharCode($event.keyCode))) {
                    $event.preventDefault();
                    document.getElementById('isShowH-' + index).style.display = 'block';
                }
                else {
                    document.getElementById('isShowH-' + index).style.display = 'none';
                }

            };

            $scope.filterDecimalValue = function ($event, index) {

                if ($event.charCode != 0) {
                    if ($event.charCode == 13) { return true; }
                    else {
                        var regex = new RegExp("^[0-9.]+$");
                        var key = String.fromCharCode(!$event.charCode ? $event.which : $event.charCode);
                        /*var str = $event.val();*/
                        /*  var str = $event.key();*/
                        if ((key !== -1) && key == '.') {
                            $event.preventDefault();
                            return false;
                        }
                        if (!regex.test(key)) {
                            $event.preventDefault();
                            document.getElementById('isShowP-' + index).style.display = 'block';
                            return false;

                        }
                        else {
                            document.getElementById('isShowP-' + index).style.display = 'none';
                        }
                    }
                }
            }

            $scope.totalprice = function (value) {
                //debugger;
                var tot = 0, price = 0;
                /*tot = Number(value || 0);*/
                //var a = Number($scope.a || 0);
                //var b = Number($scope.b || 0);
                //$scope.sum = a + b;
                for (var i = 0; i < $scope.projectDetails.length; i++) {
                    var inittot = 0;
                    tot = Number($scope.projectDetails[i].typeprice || 0);
                    price = tot + price;
                }
                vm.project.price = price + "";

            }

            $scope.totalhours = function (value) {
                var tot = 0, hour = 0;
                /*tot = Number(value || 0);*/
                //var a = Number($scope.a || 0);
                //var b = Number($scope.b || 0);
                //$scope.sum = a + b;
                for (var i = 0; i < $scope.projectDetails.length; i++) {
                    var inittot = 0;
                    tot = Number($scope.projectDetails[i].hours || 0);
                    hour = tot + hour;
                }
                vm.project.totalhours = hour + "";

            }

            $scope.projectDetails = [
                {
                    projecttypeId: "",
                    projectTypeName: "",
                    typeprice: "",
                    hours: ""
                }]

            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                    console.log(vm.cname);
                });
            }


            function getMark_Leader() {

                userService.getImmediateSupervisor(vm.roleName)
                    .then(function (result) {

                        vm.imm_sup = result.data;
                    });
            }

            function getProject_type() {

                masterListservice.getProjectType()
                    .then(function (result) {

                        vm.projecttypelist = result.data;
                    });
            }


            //Bind Company Details

            function getCompanyList() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;
                    });
            }
            vm.save = function () {
                if (vm.project.beneficiaryCompanyId == "" || vm.project.beneficiaryCompanyId == undefined || vm.project.beneficiaryCompanyId == null) {
                    abp.notify.error("Please select company.");
                    return;
                }
                if (vm.project.projectName == "" || vm.project.projectName == undefined || vm.project.projectName == null) {
                    abp.notify.error("Please enter project name.");
                    return;
                }
                if (vm.project.description == "" || vm.project.description == undefined || vm.project.description == null) {
                    abp.notify.error("Please enter description.");
                    return;
                }
                if (vm.project.startDate == "" || vm.project.startDate == undefined || vm.project.startDate == null) {
                    abp.notify.error("Please enter start date.");
                    return;
                }
                if (vm.project.endDate == "" || vm.project.endDate == undefined || vm.project.endDate == null) {

                    vm.project.endDate == "";
                }
                if (vm.project.teamDeadline == "" || vm.project.teamDeadline == undefined || vm.project.teamDeadline == null) {
                    vm.project.teamDeadline == "";
                }
                if (vm.project.clientId == "" || vm.project.clientId == undefined || vm.project.clientId == null) {

                    vm.project.clientId = 0;
                }
                if (vm.project.marketing_LeaderId == "0") {
                    vm.project.marketing_LeaderId = null;
                }
                vm.project.projectDetail = [];
                for (var i = 0; i < $scope.projectDetails.length; i++) {
                    if ($scope.projectDetails[i].typeprice == null || $scope.projectDetails[i].typeprice == "" || $scope.projectDetails[i].typeprice == undefined) {
                        abp.notify.error("Please enter project price.");
                        return;
                    }
                    else if ($scope.projectDetails[i].hours == null || $scope.projectDetails[i].hours == "" || $scope.projectDetails[i].hours == undefined) {
                        abp.notify.error("Please enter project hours.");
                        return;
                    }
                    else {
                        vm.project.projectDetail.push({ projectType: $scope.projectDetails[i].id, projectPrice: $scope.projectDetails[i].typeprice, projectPrice: $scope.projectDetails[i].typeprice, hours: $scope.projectDetails[i].hours });
                    }

                }

                vm.loading = true;
                projectService.projectExsistence(vm.project).then(function (result) {

                    if (!result.data) {
                        vm.project.status = 10;
                        vm.project.opportunityid = id;
                        projectService.createProject(vm.project)
                            .then(function () {
                                abp.notify.success(App.localize('Project ' + vm.project.projectName + ' Saved Successfully '));
                                $uibModalInstance.close();
                            });
                    }
                    else {
                        abp.notify.error(App.localize('Project ' + vm.project.projectName + ' already Exist '));
                        vm.loading = false;
                    }
                });

            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});

            };

            init = function () {
                abp.ui.setBusy();
                getMark_Leader();
                getClientName();
                getCompanyList();
                getProject_type();
                vm.getById();
                abp.ui.clearBusy();
            }

            init();

        }
    ]);
})();
