(function () {
    angular.module('app').controller('app.views.opportunity.importExcel', [
        '$scope', '$http', '$uibModalInstance', 'name',
        function ($scope, $http, $uibModalInstance, name) {
            var vm = this;
            vm.showcolumn = false;
            vm.saving = false;
            vm.opportunity = "";
            $("#ddlCategory").select2("val", vm.opportunity);
            vm.sampleFile = function () {
                window.location.href = "/userFiles/SampleFiles/OpportunityData_Format.xls";
            }

            vm.permissions = {
                opportunityOpportunity_Leader: abp.auth.hasPermission('Pages.Opportunity_Leader'),
                opportunitySales_Import: abp.auth.hasPermission('Pages.Sales_Import')
            };

            $scope.checkfiletypevalidation = function (e) {
                $scope.$apply(function () {
                    if (e.files.length > 0) {
                        vm.logo = e.files[0].name;
                        var extn = vm.logo.split(".").pop();
                        if (extn == "xls" || extn == "XLS" || extn == "xlsx" || extn == "XLSX") {
                            angular.element(document.getElementById('btnupload'))[0].disabled = false;
                            vm.filename = e.files[0].name;
                        }
                        else {
                            abp.notify.error(App.localize('You can only upload files of .xls and .xlsx type'));
                            vm.logo = "";
                            angular.element(document.getElementById('btnupload'))[0].disabled = true;
                            vm.filename = null;
                        }
                    }
                    else {
                        vm.logo = "";
                        angular.element(document.getElementById('btnupload'))[0].disabled = true;
                        vm.filename = null;
                    }
                });
            }

            vm.fileextensions = ["xls", "xlsx"];
            vm.uploadFile = function (file) {
                abp.ui.setBusy();
                $(".ladermain").show();
                vm.saving = true;
                var files = $('#filetoupload')[0].files[0];

                if ($('#filetoupload')[0].files.length != 0) {
                    vm.document = files.name;
                    var ext = vm.document.split('.').pop();
                    // if (ext == 'xls' || ext == 'xlsx') {

                    //var uploadUrl = "../ExportToExcel/SaveBulkData";
                    var uploadUrl = "../FileUpload/SaveOpportunityData";


                    var fd = new FormData();
                    fd.append('file', files);

                    if (name != "General") {
                        if (vm.permissions.opportunitySales_Import == true) {
                            fd.append('opportunity', 0);
                        }
                    }
                    if (vm.opportunity == 0) {
                        fd.append('opportunity', vm.opportunity);
                    }
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
                            vm.saving = false;
                        }
                        else {
                            abp.notify.success("Upload successfully");
                            $uibModalInstance.close();

                        }

                    }).finally(function () {
                        vm.saving = false;
                        $(".ladermain").hide();
                        abp.ui.clearBusy();
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