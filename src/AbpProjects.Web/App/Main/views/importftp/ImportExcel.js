(function () {
    angular.module('app').controller('app.views.ongoings.ImportExcel', [
        '$scope', '$stateParams', '$uibModalInstance', '$window', '$http', '$state', 'id',
        function ($scope, $stateParams, $uibModalInstance, $window, $http, $state, id) {
            var vm = this;

            vm.items = {};
            //vm.id = $stateParams.id;
            vm.id = id;
            vm.showcolumn = false;

            var init = function () {
                //alert("Pratik");
            };

            //vm.cancel = function () {
            //    $window.history.back();
            //};

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };

            vm.downloadsampleFile = function () {
                //<a href="~/Temp/SampleFiles/ImportCompanyData_Format.xlsx">~/Temp/SampleFiles/ImportCompanyData_Format.xlsx</a>
                window.location.href = "/userfiles/SampleFiles/ImportDMLData_Format_Copy.XLSX";
                //window.location.href = "/Temp/SampleFiles/ImportDML_Format.XLSX";
                //window.location.href = "/images/user.png";
            };

            vm.uploadFile = function (file) {
                $(".ladermain").show();
                vm.saving = true;
                var files = $('#filetoupload')[0].files[0];
                var uploadUrl = "../FileUpload/ImportExcelData";
                var fd = new FormData();
                fd.append('file', files);

                projectid = vm.id;
                fd.append('projectid', vm.id);
                var reqobject = { fd: fd, projectid: vm.id };

                $http.post(uploadUrl, fd, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (data, status) {
                    if (data.data.success === false) {
                        if (data.data.excel.isnullcolumn === true) {
                            vm.errorresult = data.data.excel.errorresult;
                            vm.rowcount = data.data.excel.rowcount;
                            vm.successrowcount = data.data.excel.successrowcount;
                            vm.nullcolumnscount = data.data.excel.nullcolumnscount;
                            vm.notexsistcolumnscount = data.data.excel.notexsistcolumnscount;

                            for (var k = 0; k < data.data.excel.errorresult.length; k++) {
                                data.data.excel.errorresult[k];
                                vm.showcolumn = true;

                            }
                        }
                        //abp.notify.info("Error: " + "Invalid File Format.");
                    }
                    else
                    {
                        abp.notify.info(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                        $state.go('Import', { id: vm.id });
                        //$window.history.back();
                    }
                    //$uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                    $(".ladermain").hide();
                    //$window.history.back();
                });

                //promise = fileUploadService.uploadFileToUrl(reqobject, uploadUrl, 0);
                //promise.then(function (data, status) {
                //    if (data.data.success == false) {
                //        abp.notify.info("Error: " + "Invalid File Format.");
                //    }
                //    $uibModalInstance.close();
                //}).finally(function () {
                //    vm.saving = false;
                //    $(".ladermain").hide();
                //});


            };

            init();

        }
    ]);
})();