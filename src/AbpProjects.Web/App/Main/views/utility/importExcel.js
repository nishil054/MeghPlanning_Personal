(function () {
    angular.module('app').controller('app.views.utility.importExcel', [
        '$scope', '$http', '$uibModalInstance',
        function ($scope, $http, $uibModalInstance) {
            var vm = this;
            vm.showcolumn = false;
            vm.uploadFile = function (file) {
                $(".ladermain").show();
                vm.saving = true;
                var files = $('#filetoupload')[0].files[0];

                if ($('#filetoupload')[0].files.length != 0) {
                    vm.document = files.name;
                    var ext = vm.document.split('.').pop();
                   // if (ext == 'xls' || ext == 'xlsx') {

                        //var uploadUrl = "../ExportToExcel/SaveBulkData";
                    var uploadUrl = "../ExportToExcel/SaveLoginPunchData";
                    
                        var fd = new FormData();
                        fd.append('file', files);
                        $http.post(uploadUrl, fd, {
                            transformRequest: angular.identity,
                            headers: { 'Content-Type': undefined }
                   
                        }).then(function (data, status) {

                            if (data.data.success == false) {

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
                                        // abp.notify.error("Error: " + "Invalid File Format.");
                            }
                            else {
                                abp.notify.success("Upload successfully");
                                $uibModalInstance.close();
                                 }
                   
                        }).finally(function () {
                        vm.saving = false;
                        $(".ladermain").hide();
                        });

                    //}
                    //else {
                    //    abp.notify.error("please upload correct file");
                    //    return;
                    //}

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