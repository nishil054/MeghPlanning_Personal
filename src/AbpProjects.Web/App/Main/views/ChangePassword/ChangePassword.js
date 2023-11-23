(function () {
    'use strict';
    angular.module('app').controller('app.views.changePassword.changePassword', [
        '$scope', '$state', 'abp.services.app.changePassword',
        function ($scope, $state, changePasswordServices) {
            var vm = this;
            vm.user = {

            };
            vm.errormessage = "";
            vm.error = false;
            vm.save = function () {
                abp.ui.setBusy();
                changePasswordServices.changePassword(vm.user)
                    .then(function (result) {
                        if (result.data == "Correct") {
                            abp.notify.info(App.localize('SavedSuccessfully'));
                            vm.reset();
                            $state.go('passwordChange');
                        }
                        else {
                            vm.error = true;
                            if (result.data == "OldPasswordIncorrect") {
                                vm.errormessage = "Old Password is Incorrect.";
                            }
                            else if (result.data == "OldPasswordNewPasswordSame") {
                                vm.errormessage = "Old Password and New Password is same.";
                            }
                            else if (result.data == "NewPasswordConfirmNewPasswordNotMatch") {
                                vm.errormessage = "New Password and Confirm New Password is not match.";
                            }
                            else {
                                vm.errormessage = "Incorrect Password.";
                            }
                        }
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                    });
            };

            vm.cancel = function () {
                $state.go('passwordChange');
            };
            vm.reset = function () {
                vm.user.oldpassword = "";
                vm.user.password = "";
                vm.user.confirmPassword = "";
                vm.errormessage = "";
                vm.error = false;
            };
        }
    ]);
})();