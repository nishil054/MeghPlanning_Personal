(function () {
    angular.module('app').controller('app.views.document.editDocument', [
        '$scope', '$http', '$uibModalInstance', 'abp.services.app.document', 'id',
        function ($scope, $http, $uibModalInstance, documentService, id) {
            var vm = this;
            vm.loading = false;
            vm.saving = false;
            vm.document = {};
            vm.downloadPDF = function (document) {
                window.open(abp.appPath + 'UserFiles/Documents/' + document + '?v=' + new Date().valueOf(), '_blank');
            };

            vm.uploadFile = function (file) {
                vm.saving = true;
                if ($('#filetoupload')[0].files.length != 0) {
                    var files = $('#filetoupload')[0].files[0];
                    if ($('#filetoupload')[0].files.length == 0) {

                        abp.notify.error(App.localize('pleaseuploaddoc'));
                        return;
                    }
                    var uploadUrl = "../FileUpload/UploadDocumentAttachments";
                    var fd = new FormData();
                    fd.append('file', $('#filetoupload')[0].files[0]);
                }
                else {
                    var uploadUrl = "../FileUpload/UploadDocumentAttachments";
                    var fd = new FormData();
                    fd.append('file', vm.document.attachment);
                }
                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (data, status) {
                    if (data.statusText == "OK") {
                        vm.document.attachment = data.data.Result.fileName;
                        //vm.saveAs();
                        //console.log(data);
                    }



                    else {
                        alert("somethingsiswrong");
                    }

                }).finally(function () {
                    vm.saving = false;
                    vm.saveAs();
                })


            };



            function init() {

                documentService.getDataById({ id: id }).then(function (result) {
                    vm.document = result.data;
                });


            }


            vm.saveAs = function () {
                vm.loading = true;
                vm.saving = true;
                documentService.documentExsistenceById(vm.document).then(function (result) {
                    if (!result.data) {
                        documentService.updateDocument(vm.document)
                            .then(function () {
                                abp.notify.success(App.localize('DocumentSavedSuccessfully'));
                                $uibModalInstance.close();

                            });
                    }

                    else {
                        abp.notify.error(App.localize('Document already Exist '));
                        vm.loading = false;
                    }
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.save = function () {
                vm.loading = true;

                if ($('#filetoupload')[0].files.length != 0) {
                    var files = $('#filetoupload')[0].files[0];
                    vm.document.attachment = files.name;
                    var ext = vm.document.attachment.split('.').pop();


                    if (ext == 'pdf' || ext == 'jpg' || ext == 'jpeg' || ext == 'doc' || ext == 'docx' || ext == 'txt' || ext == 'xls' || ext == 'xlsx') {

                        vm.uploadFile();


                    }

                    else {
                        abp.notify.error(App.localize('pleaseuploadcorrectfile'));
                        //return;
                        vm.loading = false;
                    }
                }
                else {
                    //abp.notify.error(App.localize('pleaseuploaddocument'));
                    //return;
                    vm.document.attachment = vm.document.attachment;
                    vm.uploadFile();
                }
            }



            vm.cancel = function () {
                $uibModalInstance.dismiss({});

            };
            init();
        }
    ]);
})();