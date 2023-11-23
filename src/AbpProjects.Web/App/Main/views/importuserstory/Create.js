(function () {
    angular.module('app').controller('app.views.importuserstory.Create', [
        '$scope', '$uibModalInstance', '$http', 'abp.services.app.masterList','id',
        function ($scope, $uibModalInstance, $http, masterListservice,id) {
            //debugger;
            var vm = this;
            vm.reasons = {};
            //vm.data = {};
            vm.importuserstory = {};
            vm.downloadfile = "/userFiles/SampleFiles/UserStoryDetail_Format.xls";
            vm.close = function () {
                $uibModalInstance.dismiss();
            };


            function getProjects() {

                masterListservice.getProject()
                    .then(function (result) {

                        vm.projectlist = result.data;
                    });
            }
            vm.uploadFile = function (file) {
                var projectid = id;
                if ((projectid == "" || projectid == null || projectid == undefined)) {
                    abp.notify.error(App.localize('Please Select Project!'));
                    return false;
                }
                var inputattachment = $('#filetoupload')[0].files[0];
                if ((inputattachment == "" || inputattachment == null || inputattachment == undefined)) {
                    abp.notify.error(App.localize('Please Select File!'));
                    return false;
                }

                $(".ladermain").show();
                vm.saving = true;
                var files = $('#filetoupload')[0].files[0];
                var uploadUrl = "../FileUpload/ImportUserStoryExcelData";
                var fd = new FormData();
                fd.append('file', files);
                fd.append('projectid', projectid);
                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (data, status) {
                    //debugger;
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

            //vm.downloadsampleFile = function () {
            //    debugger;
            //   // window.location.href = "/userFiles/SampleFiles/UserStoryDetail_Format.xls";
            //};

            $scope.checkfiletypevalidation = function (e) {
                //debugger;
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

            var init = function () {
                getProjects();



            };
            init();
        }
    ]);
})();