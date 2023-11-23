(function () {
    var myApp = angular.module('app');
    myApp.controller('app.views.client.editClient', [
        '$scope', '$state', '$uibModalInstance', '$stateParams', '$filter', '$http', 'abp.services.app.clients', 'id', 'edid',
        function ($scope, $state, $uibModalInstance, $stateParams, $filter, $http, clientsService, id, edid) {
            //debugger;
            var vm = this;
            $scope.btndisable = false;
            vm.saving = false;
            vm.editid = 0;
            vm.datafield = {
                //cName: null,
                //id: null
            };
            vm.clientAdd = [];
            vm.panno = "";
            vm.clientName = "";
            vm.emailFormat = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            vm.add = function () {
                //Add the new item to the Array. 

                var customer = {};
                customer.id = id;
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
                customer.flag = false;
                customer.isdefault = vm.isdefault;

                vm.clientAdd.push(customer);


            };

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

            vm.remove = function (index) {

                {

                    vm.clientAdd.splice(index, 1);
                }
            }

            vm.save = function () {
                $scope.btndisable = true;
                vm.saving = true;
                if (vm.clientName == "" || vm.clientName == null || vm.clientName == undefined) {
                    abp.notify.error(App.localize('Please Enter Client Name'));
                    $scope.btndisable = false;
                    return
                } else {
                    vm.datafield.cName = vm.clientName;
                    vm.datafield.id = vm.clientAdd[0].clientid;
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
                            vm.postedParams = {
                                ClientAdd: vm.clientAdd,
                                ClientName: vm.clientName,
                                pan_no: vm.panno
                            };
                            clientsService.editclient(vm.postedParams, id).then(function (result) {
                                abp.notify.success(App.localize('SavedSuccessfully'));
                                $uibModalInstance.close();
                                $scope.btndisable = false;
                            })
                        }
                    });
                }
            }


            vm.cancel = function () {
                $uibModalInstance.dismiss({});

            };
            vm.editdata = function () {

                if (vm.panno != "") {
                    if (vm.clientName != "") {
                        vm.saving = true;
                        clientsService.editclient(vm.clientAdd, id, vm.clientName, vm.panno).then(function (result) {
                            abp.notify.success(App.localize('SavedSuccessfully'));
                        }).finally(function () {
                            vm.saving = false;
                            // vm.addnewdata = 0;
                            //  vm.editid = 0;
                            $uibModalInstance.dismiss({});
                        });
                    } else { abp.notify.error(App.localize('Please Enter Client Name')); }
                } else { abp.notify.error(App.localize('Please Enter PANNO')); }
            };



            vm.edit = function () {
                // vm.addnewdata = 0;
                //  vm.editid = 1;
                clientsService.getClientdetail(id)
                    .then(function (result) {
                        vm.clientName = result.data.clientName;
                        vm.panno = result.data.pan_no;

                    });
                clientsService.getClientdetailList(id)
                    .then(function (result) {
                        if (result.data.items.length > 0) {
                            vm.clientAdd = result.data.items;
                            vm.addnewdata = 1;
                        } else {
                            vm.add();
                        }

                        //  vm.editid =1;
                    });
            }
            function init() {
                vm.editid = edid;
                vm.itemsPerPage = 10;
                vm.skipCount = 0;
                vm.currentdirection = "desc";
                vm.requestParams = {
                    skipCount: vm.skipCount,
                    maxResultCount: vm.itemsPerPage,
                    sorting: "Title"
                };
                vm.edit();

            }
            init();

        }
    ]);
})();