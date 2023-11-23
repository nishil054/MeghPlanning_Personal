(function () {
    var myApp = angular.module('app');
    myApp.controller('app.views.client.addClient', [
        '$scope', '$state', '$uibModalInstance', '$stateParams', '$filter', '$http', 'abp.services.app.clients',
        function ($scope, $state, $uibModalInstance, $stateParams, $filter, $http, clientsService) {
            var vm = this;
            $scope.btndisable = false;
            vm.saving = false;

            vm.clientAdd = [];
            vm.datafield = {
                //cName: null,
                //id: null
            };
            vm.panno = "";
            vm.clientName = "";
            vm.emailFormat = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            vm.add = function () {
                //Add the new item to the Array. 

                var customer = {};
                customer.id = vm.id;
                customer.clientid = vm.clientid;
                customer.contactno = vm.contactno;
                customer.contactname = vm.contactname;
                customer.email = vm.email;
                customer.clientaddress = vm.clientaddress;
                customer.city = vm.city;
                customer.state = vm.state;
                customer.statecodeid = vm.statecodeid;
                customer.pincode = vm.pincode;
                customer.gstno = vm.gstno;
                customer.countryName = vm.countryName;
                customer.isdefault = vm.isdefault;
                customer.flag = false;
                vm.clientAdd.push(customer);


            };

            vm.remove = function (index) {
                {
                    vm.clientAdd.splice(index, 1);
                }
            }

            $scope.isDefaultcheck = function (ischeck, index) {
                angular.forEach(vm.clientAdd, function (value, key) {
                    if (index == key) {
                        if (ischeck == true) {
                            vm.clientAdd[key].isdefault = true;
                        } else {
                            vm.clientAdd[key].isdefault = false;
                        }
                    }
                    else {
                        vm.clientAdd[key].isdefault = false;
                    }
                    console.log('value', value);
                    console.log('key', key);
                });
            }

            vm.save = function () {
                var flag = false;
                $scope.btndisable = true;
                if (vm.clientName == "" || vm.clientName == null || vm.clientName == undefined) {
                    abp.notify.error(App.localize('Please Enter Client Name'));
                    $scope.btndisable = false;
                    return
                } else {
                    var clName = vm.clientName;
                    vm.datafield.cName = clName;
                    clientsService.clientExsistence(vm.datafield).then(function (result) {
                        if (result.data) {
                            abp.notify.error("Client Name already exsist.");
                            $scope.btndisable = false;
                            return;
                        } else {
                            for (var i = 0; i < vm.clientAdd.length; i++) {
                                if (vm.clientAdd[i].contactname == null || vm.clientAdd[i].contactname == "" || vm.clientAdd[i].contactname == undefined) {
                                    abp.notify.error(App.localize('Please Enter Contact Name.'));
                                    $scope.btndisable = false;
                                    return;
                                }
                            }

                            var pannoval = ""
                            if (vm.panno == "") {
                                pannoval = null;
                            }
                            else {
                                pannoval = vm.panno;
                            }

                            vm.saving = true;
                            vm.postedParams = {
                                ClientAdd: vm.clientAdd,
                                ClientName: vm.clientName,
                                pan_no: pannoval
                            };
                            console.log(vm.postedParams);
                            clientsService.createClient(vm.postedParams)
                                .then(function (result) {
                                    abp.notify.success(App.localize('SavedSuccessfully'));
                                    $uibModalInstance.close();
                                    $scope.btndisable = false;
                                })
                        }
                    });
                }
            };


            vm.cancel = function () {
                $uibModalInstance.dismiss({});

            };

            function init() {
                //debugger;
                vm.itemsPerPage = 10;
                vm.skipCount = 0;
                vm.currentdirection = "desc";
                vm.requestParams = {
                    skipCount: vm.skipCount,
                    maxResultCount: vm.itemsPerPage,
                    sorting: "Title"
                };
                vm.add();

            }
            init();

        }
    ]);
})();