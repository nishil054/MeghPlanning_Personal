
(function () {
    angular.module('app').controller('app.views.utility.index', [
        '$scope', '$timeout', '$uibModal', '$http', '$state', '$stateParams', 'abp.services.app.document',
        function ($scope, $timeout, $uibModal, $http, $state, $stateParams, documentService) {
            var vm = this;
            $scope.btndisable = false;
            //file upload by directive
            vm.files = [];      
            vm.fileextensions = ["jpg", "pdf", "jpeg", "png", "gif", "doc", "docx", "xls", "xlsx", "xlsm", "txt"];
            vm.folder = "Upload";
            vm.uploadUrl = "../FileUpload/UploadFiles";
            vm.filesize = 2048000; //2MB Max
            vm.document = [];
            vm.doc = {};
            vm.doc.fileName = [];
            var uploadattachfiles = [];

            vm.sampleFile = function () {
                window.location.href = "/userFiles/SampleFiles/ImportBulkData_Format.xls";
            }

            vm.uploadFilePopup = function (file) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/utility/importExcel.cshtml',
                    controller: 'app.views.utility.importExcel as vm',
                    backdrop: 'static',
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    //	vm.getCompanies('Unprocessed');
                    //$state.go('companyDataGrid');
                });
            };

            vm.uploadAttachment = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/utility/uploadfile.cshtml',
                    controller: 'app.views.utility.uploadfile as vm',
                    backdrop: 'static',
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                });
            }


            $scope.fileChange = function (e) {
                $scope.files = [];
                vm.document = [];

                $scope.$apply(function () {
                    // STORE THE FILE OBJECT IN AN ARRAY.
                    for (var i = 0; i < e.length; i++) {
                        $scope.files.push(e[i]);
                    }
                    angular.forEach($scope.files, function (item, key) {
                    vm.document.push(item.name);
                });
                    vm.files =vm.document;
                });
            }
        
            vm.save = function () {
                //$scope.btndisable = true;
                if (vm.files == null || vm.files == undefined || vm.files == "") {
                    abp.notify.error("Please upload file.");
                    //$scope.btndisable = false;
                    return;
                }
                vm.SaveAs();
                
            };

            vm.SaveAs = function () {
               
                    vm.doc.documentComment = "Test";
                    documentService.createFileUploadDocument(vm.doc).then(function (result) {   //insert into parent table
                        vm.doc.id = result.data;
                        if (vm.doc.id) {
                            vm.uploadMultipleFiles();
                        }
                       
                    });
                
               
            }

            vm.uploadMultipleFiles = function() {
                //FILL FormData WITH FILE DETAILS.
                var fd = new FormData();
                for (var i in $scope.files) {
                    fd.append("file", $scope.files[i]);
                }
                
                var documentCode = vm.doc.id;
                fd.append('documentCode', documentCode);

                var folderName = vm.folder;
                fd.append('folderName', folderName);

                // ADD LISTENERS.
                var uploadUrl = "../FileUpload/UploadFiles";
                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (response, status) {
                    var filObject = response.data.Result.fileObject;
                    if (response.statusText == "OK") {

                        abp.notify.success(App.localize('DocumentSavedSuccessfully'));
                    }


                    //else {
                    //    alert("somethingsiswrong");
                    //}
                   
                }).finally(function () {
                    vm.saving = false;
                    $(".ladermain").hide();
                    vm.cleardata();
                });
            }

            vm.cleardata = function () {
                vm.files = [];
                vm.document = [];
                vm.doc = {};
                //vm.doc.fileName = [];
                $("#filetoupload").val('');
                $("#file_upload").val('');
            }
        }
    ]);
})();