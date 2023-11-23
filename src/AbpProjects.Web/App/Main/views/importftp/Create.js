(function () {
    angular.module('app').controller('app.views.importftp.Create', [
        '$scope', '$uibModalInstance', '$http',
        function ($scope, $uibModalInstance, $http) {
            var vm = this;
            vm.reasons = {};
            vm.close = function () {
                $uibModalInstance.dismiss();
            };

            vm.uploadFile = function (file) {
                var inputattachment = $('#filetoupload')[0].files[0];
                if ((inputattachment == "" || inputattachment == null || inputattachment == undefined)) {
                    abp.notify.error(App.localize('Please Select File!'));
                    return false;
                }

                $(".ladermain").show();
                vm.saving = true;
                var files = $('#filetoupload')[0].files[0];
                var uploadUrl = "../FileUpload/ImportExcelData";
                var fd = new FormData();
                fd.append('file', files);

                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (data, status) {
                    console.log('File Response', data.data.success);
                    if (data.data.success == true) {
                        $uibModalInstance.close();
                    }
                    else {
                        abp.notify.error(App.localize('Please Upload Valid File Format'));
                        vm.logo = "";
                    }
                }).finally(function () {
                    vm.saving = false;
                    $(".ladermain").hide();
                    //$window.history.back();
                });

            };

            vm.downloadsampleFile = function () {
                window.location.href = "/userFiles/SampleFiles/FTPDetail_Format.xls";
            };

            $scope.checkfiletypevalidation = function (e) {
                $scope.$apply(function () {
                    if (e.files.length > 0) {
                        vm.logo = e.files[0].name;
                        var extn = vm.logo.split(".").pop();
                        if (extn == "xls" || extn == "XLS" || extn == "xlsx" || extn == "XLSX") {
                            angular.element(document.getElementById('btnupload'))[0].disabled = false;
                        }
                        else {
                            abp.notify.error(App.localize('You can only upload files of .xls and .xlsx type'));
                            vm.logo = "";
                            angular.element(document.getElementById('btnupload'))[0].disabled = true;
                        }
                    }
                    else {
                        vm.logo = "";
                        angular.element(document.getElementById('btnupload'))[0].disabled = true;
                    }
                });
            }
        }
    ]);
})();