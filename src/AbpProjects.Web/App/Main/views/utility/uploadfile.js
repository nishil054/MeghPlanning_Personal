(function () {
    angular.module('app').controller('app.views.utility.uploadfile', [
        '$scope', '$http', '$uibModalInstance',
        function ($scope, $http, $uibModalInstance) {
            var vm = this;
            //vm.showcolumn = false;
            var maxsize = 2048000; //2MB Max

            vm.uploadFile = function (file) {

                $(".ladermain").show();
                vm.saving = true;
                var files = $('#filetoupload')[0].files[0];

                if ($('#filetoupload')[0].files.length != 0) {
                    vm.document = files.name;
                    var ext = vm.document.split('.').pop();
                    if (ext == 'pdf' || ext == 'jpg' || ext == 'jpeg' || ext == 'doc' || ext == 'docx' || ext == 'txt' || ext == 'xls' || ext == 'xlsx') {

                        if (files.size <= maxsize) {

                            var uploadUrl = "../ExportToExcel/UploadDocument";
                            var fd = new FormData();
                            fd.append('file', $('#filetoupload')[0].files[0]);

                            $http.post(uploadUrl, fd, {
                                transformRequest: angular.identity,
                                headers: { 'Content-Type': false }

                            }).then(function (data, status) {

                                if (data.data.success == false) {
                                     abp.notify.error("Error: ");
                                }
                                else {
                                    abp.notify.success("Upload successfully");
                                    $uibModalInstance.close();
                                }

                            }).finally(function () {
                                vm.saving = false;
                                $(".ladermain").hide();
                            });


                        }
                        else {
                            abp.notify.error(App.localize('FilesizeexceedsmaximumlimitMB'));
                        }

                    }   
                    else {
                        abp.notify.error("please upload correct file");
                        return;
                    }

                }
                else {
                    abp.notify.error("please upload document");
                    return;
                }
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

        }
    ]);
})();