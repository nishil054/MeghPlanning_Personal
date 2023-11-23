(function () {
    angular.module('app').controller('app.views.document.createDocument', [
        '$scope', '$uibModalInstance', '$http', 'abp.services.app.document',
        function ($scope, $uibModalInstance, $http, documentService) {
            var vm = this;
            vm.saving = false;
            vm.loading = false;
            var maxsize = 2048000; //2MB Max
            vm.document = {};
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc"
            };
            vm.getAll = function () {
                vm.loading = true;
                // debugger;
                documentService.getDocumentData($.extend({}, vm.requestParams)).then(function (result) {
                    //  debugger;
                    vm.document = result.data.items;
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        //vm.norecord = true;
                        abp.notify.info(app.localize('NoRecordFound'));
                    }
                    else { vm.norecord = false; }
                }).finally(function () {
                    vm.loading = false;
                });

            }

            vm.uploadFile = function (file) {

                vm.saving = true;
                var files = $('#filetoupload')[0].files[0];
                //console.log(files);
                if ($('#filetoupload')[0].files.length == 0) {

                    abp.notify.error(App.localize('pleaseuploaddoc'));

                    return;
                }

                var uploadUrl = "../FileUpload/UploadDocumentAttachments";
                var fd = new FormData();
                fd.append('file', $('#filetoupload')[0].files[0]);

                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (data, status) {
                    console.log(data);
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
            vm.saveAs = function () {
                vm.loading = true;
                vm.saving = true;

                documentService.documentExsistence(vm.document).then(function (result) {
                    if (!result.data) {
                        documentService.createDocument(vm.document).then(function (result) {
                            vm.document = result.data;
                            abp.notify.success(App.localize('DocumentSavedSuccessfully'));

                            $uibModalInstance.close();
                            vm.getAll();

                        }).finally(function () {
                            vm.saving = false;
                        });
                    }
                    else {
                        abp.notify.error(App.localize('Document already Exist '));
                        vm.loading = false;
                    }
                });
            };
            vm.save = function () {
                vm.loading = true;
                var files = $('#filetoupload')[0].files[0];


                if ($('#filetoupload')[0].files.length != 0) {
                    vm.document.attachment = files.name;
                    var ext = vm.document.attachment.split('.').pop();
                    if (ext == 'pdf' || ext == 'jpg' || ext == 'jpeg' || ext == 'doc' || ext == 'docx' || ext == 'txt' || ext == 'xls' || ext == 'xlsx') {
                        //var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
                        //var i = parseInt(Math.floor(Math.log(files.size) / Math.log(1024)));
                        //var sz = Math.round(files.size / Math.pow(1024, i), 2) + ' ' + sizes[i];
                        if (files.size <= maxsize) {
                            vm.uploadFile();
                        }
                        else {
                            abp.notify.error(App.localize('FilesizeexceedsmaximumlimitMB'));
                        }
                    }

                    else {
                        abp.notify.error(App.localize('pleaseuploadcorrectfile'));

                        // return;
                    }

                }
                else {
                    abp.notify.error(App.localize('pleaseuploaddoc'));
                    //return;
                    vm.loading = false;
                }
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };


        }
    ]);
})();