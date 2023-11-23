
(function () {
    angular.module('app').controller('app.views.project.createProject', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'abp.services.app.projectType', 'abp.services.app.project', 'abp.services.app.support', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, userService, projectTypeService, projectService, supportService, masterListservice) {
            var vm = this;
            vm.cname = [];
            vm.loading = false;
            vm.saving = false;
            vm.project = {};
            vm.imm_sup = [];
            vm.projecttypelist = [];

            function getClientName() {
                supportService.getClientName({}).then(function (result) {
                    vm.cname = result.data.items;
                    console.log(vm.cname);
                });
            }

            getClientName();
            vm.roleName = "Marketing Leader"
            function getMark_Leader() {

                userService.getImmediateSupervisor(vm.roleName)
                    .then(function (result) {

                        vm.imm_sup = result.data;
                        vm.project.marketing_LeaderId = "0";
                    });
            }

            function getProject_type() {

                masterListservice.getProjectType()
                    .then(function (result) {

                        vm.projecttypelist = result.data;
                    });
            }
            //Bind Company Details
            vm.companylist = [];
            function getCompanyList() {
                masterListservice.getCompany()
                    .then(function (result) {
                        vm.companylist = result.data;
                    });
            }


            $scope.ProArray = [];

            vm.addRow = function () {

                var i = $('#ddltype').find('option:selected').text();

                if ($scope.vm.projecttypelist.id != undefined && $scope.vm.project.typeprice != undefined && i != undefined) {
                    var a1 = [];
                    var keepGoing = true;

                    for (var im = 0; im < $scope.ProArray.length; im++) {
                        if ($scope.vm.projecttypelist.id == $scope.ProArray[0].projecttypeid) {

                            keepGoing = false;
                            break;
                        }
                    }
                    if (keepGoing) {
                        a1.projecttypeid = vm.projecttypelist.id;
                        a1.projectTypeName = i;//$scope.projecttypelist.projecttypename;
                        a1.typeprice = vm.project.typeprice;

                        // $scope.ProArray.push({ id: "", price: "" });
                        $scope.ProArray.push(a1);
                    }


                    $scope.projecttypeid = null;
                    $scope.typeprice = null;

                    $("#ddltype option:selected").each(function () {
                        $(this).removeAttr("selected");
                    });

                    i = 0;
                }
            }
            vm.removeRow = function () {

                var arr = [];
                angular.forEach($scope.ProArray, function (value) {

                    if (!value.remove) {
                        arr.push(value);
                    }
                });
                $scope.ProArray = arr;

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
                    //abp.notify.error("Please select client.");
                    //return;
                    vm.project.clientId = 0;
                }
                vm.project.projectDetail = [];
                for (var i = 0; i < $scope.emailList.length; i++) {
                    if ($scope.emailList[i].projecttypeId == null || $scope.emailList[i].projecttypeId == "" || $scope.emailList[i].projecttypeId == undefined) {
                        abp.notify.error("Please select project type.");
                        return;
                    }
                    else if ($scope.emailList[i].typeprice == null || $scope.emailList[i].typeprice == "" || $scope.emailList[i].typeprice == undefined) {
                        abp.notify.error("Please enter project price.");
                        return;
                    }
                    else if ($scope.emailList[i].hours == null || $scope.emailList[i].hours == "" || $scope.emailList[i].hours == undefined) {
                        abp.notify.error("Please enter project hours.");
                        return;
                    }
                    else {
                        vm.project.projectDetail.push({ projectType: $scope.emailList[i].projecttypeId, projectPrice: $scope.emailList[i].typeprice, projectPrice: $scope.emailList[i].typeprice, hours: $scope.emailList[i].hours });
                    }

                }

                // Vikas Change //Project price will be Zero and hours also zero.

                //if (vm.project.price != "" || vm.project.price != undefined || vm.project.price != null) {
                //    var price = Number(vm.project.price || 0);
                //    if (price <= 0) {
                //        abp.notify.error("Please enter price above zero.");
                //        return;
                //    }

                //}
                //if (vm.project.totalhours != "" || vm.project.totalhours != undefined || vm.project.totalhours != null) {
                //    var hour = Number(vm.project.totalhours || 0);
                //    if (hour <= 0) {
                //        abp.notify.error("Please enter hour above zero.");
                //        return;
                //    }

                //}

                if (vm.project.marketing_LeaderId == "0") {
                    vm.project.marketing_LeaderId = null;
                }
                //projecttypeId: "",
                //    typeprice: "",
                //        hours: ""
               
                vm.loading = true;
                projectService.projectExsistence(vm.project).then(function (result) {

                    if (!result.data) {
                        vm.project.status = 10;
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
                //}

            };
            vm.test = function () {
                vm.saving = true;
                test();
                projectServices.createProject(vm.project).then(function () {
                    abp.notify.success(App.localize('SavedSuccessfully'));
                    $uibModalInstance.dismiss({});

                }).final(function () {
                    vm.saving = false;
                });
            };
            vm.cancel = function () {
                $uibModalInstance.dismiss({});

            };

            $scope.emailList = [
                {
                    projecttypeId: "",
                    typeprice: "",
                    hours: ""
                }]
            $scope.addItem = function () {
                vm.error = false;
                $scope.emailList.push({
                    projecttypeId: "",
                    typeprice: "",
                    hours: ""
                });
            }

            $scope.removeItem = function (item, index) {
                $scope.emailList.splice($scope.emailList.indexOf(item), 1);
                document.getElementById('isShowH-' + index).style.display = 'none';
                document.getElementById('isShowP-' + index).style.display = 'none';
                vm.error = true;
                var price = 0, currentValue = 0, finalprice = 0;
                var value = 0, currentHour = 0, finalhours = 0;
                price = Number(vm.project.price || 0);
                currentValue = Number(item.typeprice || 0);
                finalprice = price - currentValue;
                vm.project.price = finalprice + "";
                //Hours Calculate
                value = Number(vm.project.totalhours || 0);
                currentHour = Number(item.hours || 0);
                finalhours = value - currentHour;
                vm.project.totalhours = finalhours + "";
            };

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
                for (var i = 0; i < $scope.emailList.length; i++) {
                    var inittot = 0;
                    tot = Number($scope.emailList[i].typeprice || 0);
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
                for (var i = 0; i < $scope.emailList.length; i++) {
                    var inittot = 0;
                    tot = Number($scope.emailList[i].hours || 0);
                    hour = tot + hour;
                }
                vm.project.totalhours = hour + "";

            }


            //if (event.charCode != 0) {
            //    if (event.charCode == 13) { return true; }
            //    else {
            //        var regex = new RegExp("^[0-9.]+$");
            //        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            //        var str = $(this).val();
            //        if ((str.indexOf('.') !== -1) && key == '.') {
            //            event.preventDefault();
            //            return false;
            //        }
            //        if (!regex.test(key)) {
            //            event.preventDefault();
            //            return false;
            //        }
            //    }
            //}

            getCompanyList();
            getMark_Leader();
            getProject_type();



        }
    ]);
})();
