
(function () {
    angular.module('app').controller('app.views.users.lockuser', [
        '$scope', '$uibModalInstance', '$uibModal', 'abp.services.app.user', 'abp.services.app.support', 'id',
        function ($scope, $uibModalInstance, $uibModal, userService, supportService, id) {

            var vm = this;
            vm.project = {};
            var init = function () {
                getMark_Leader();
                getEmployeeName();
                getAll();
                getCount(); 
               
            }//vm.projectservice[0].projectCount
            function getCount() {
                userService.getProjectServiceCount({ id:id })
                    .then(function (result) {
                        vm.projectservice = result.data;
                        });
            };
            function getAll() {
                userService.get({ id: id })
                    .then(function (result) {
                        vm.user = result.data;
                    });
            }
           
            function getMark_Leader() {
                userService.getImmediateSupervisorById(id)
                    .then(function (result) {
                        vm.imm_sup = result.data;
                       
                    });
            }
            function getEmployeeName() {
                supportService.getUserNameById(id).then(function (result) {
                    vm.ename = result.data;
                   
                });
            } 

            vm.show = function (f) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/users/lockuserdetails.cshtml',
                    controller: 'app.views.users.lockuserdetails as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return id;
                        },
                        f: function () {
                            return f;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    getAll();
                });
            }

            vm.save = function () {
                
                if (vm.projectservice.length > 0) {
                    if (vm.projectservice[0].projectCount > 0 && vm.projectservice[0].serviceCount > 0) {

                        if (vm.project.marketing_LeaderId == undefined) {
                            abp.notify.error("Select Marketing Leader");
                            return;
                        }

                        if (vm.project.employeeId == undefined) {
                            abp.notify.error("Select Account Manager");
                            return;
                        }

                        userService.deactiveUserProjectUpdate({ id: id, marketing_LeaderId: vm.project.marketing_LeaderId })
                            .then(function () {
                                //abp.notify.success(App.localize('SavedSuccessfully'));
                            })
                        userService.deactiveUserServiceUpdate({ id: id, employeeId: vm.project.employeeId })
                            .then(function () {
                                //abp.notify.success(App.localize('SavedSuccessfully'));

                            })

                    }
                    else if (vm.projectservice[0].serviceCount > 0) {
                        if (vm.project.employeeId == undefined) {
                            abp.notify.error("Select Account Manager");
                            return;
                        }

                        userService.deactiveUserServiceUpdate({ id: id, employeeId: vm.project.employeeId })
                            .then(function () {
                                //abp.notify.success(App.localize('SavedSuccessfully'));

                            })
                    }
                    else {
                        if (vm.project.marketing_LeaderId == undefined) {
                            abp.notify.error("Select Marketing Leader");
                            return;
                        }
                        userService.deactiveUserProjectUpdate({ id: id, marketing_LeaderId: vm.project.marketing_LeaderId })
                            .then(function () {
                                //abp.notify.success(App.localize('SavedSuccessfully'));
                            })
                    }
                }
               
                userService.deactiveUser({ id: id })
                    .then(function (result) {
                        if (result.data == true) {
                            abp.notify.success("User lock successfully");
                            $uibModalInstance.close();
                        }
                    });
                
            };


            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            init();
           
        }
    ]);
})();