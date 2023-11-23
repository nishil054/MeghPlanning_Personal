(function () {
    angular.module('app').controller('app.views.manageknowledgecenter.Create', [
        '$scope', '$uibModalInstance', 'abp.services.app.knowledgeCenter', '$http', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, knowledgeCenterService, $http, masterListservice) {
            var vm = this;
            vm.managekc = {};
            vm.showdoc = true;
            var docextension = false;
            var docsize = false;
            vm.loading = false;
            vm.close = function () {
                $uibModalInstance.dismiss();
            };
            var attachfiles = [];

            vm.sourceList = [
                { sourceId: 1, sourceName: "Document" },
                { sourceId: 0, sourceName: "Url" }
            ];

            function init() {
                getteams();
                //getcategories();
                //$("#select2-drop").select2("val", null);

                vm.managekc.isDocument = "1";
                if (vm.managekc.isDocument == "1") {
                    vm.showdoc = true;
                }
                else {
                    vm.showdoc = false;
                }
            }
            $scope.changeteam = function (teamid) {
                masterListservice.getCategoriesByTeam(teamid)
                    .then(function (result) {
                        vm.categoryList = result.data;
                    });
            }
            vm.save = function () {
                vm.loading = true;
                vm.saving = true;

                if (vm.managekc.isDocument == "1") {
                    var inputattachment = $scope.files;
                    if (inputattachment == "" || inputattachment == null || inputattachment == undefined || inputattachment == "null") {
                        abp.notify.error(App.localize('SelectDocument'));
                        return false;
                    }
                }
                else if (vm.managekc.isDocument == "0") {
                    if (vm.managekc.url == "" || vm.managekc.url == null || vm.managekc.url == undefined || vm.managekc.url == "null") {
                        abp.notify.error(App.localize('EnterUrl'));
                        return false;
                    }
                }

                vm.managekc.attachments = attachfiles;

                knowledgeCenterService.knowledgeCenterExsistence(vm.managekc).then(function (result) {
                    if (!result.data) {
                        knowledgeCenterService.createKnowledgeCenter(vm.managekc).then(function (response) {
                            abp.notify.success(App.localize('SavedSuccessfully'));
                            var knowlwdgecenterId = response.data;
                            if (knowlwdgecenterId) {
                                Multipeattachment(knowlwdgecenterId);
                            }
                            $uibModalInstance.close();
                            //$uibModalInstance.dismiss();
                            vm.getAll();
                        });
                    }
                    else {
                        abp.notify.error(App.localize('Title Already Exist!'));
                        vm.loading = false;
                    }
                });
            };
            vm.getAll = function () {
                vm.loading = true;

                knowledgeCenterService.getKnowledgeCenter($.extend({}, {}))
                    .then(function (result) {
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = result.data.items;
                        if (result.data.totalCount == 0) {
                            $scope.noData = true;
                            //vm.norecord = true;
                            //abp.notify.info(app.localize('NoRecordFound'));
                        } else {
                            $scope.noData = false;
                            //vm.norecord = false;
                        }
                    }).finally(function () {
                        vm.loading = false;

                    });

            }
            vm.isdocselected = function (value) {
                if (value == "1") {
                    vm.showdoc = true;
                }
                else {
                    vm.showdoc = false;
                }
            }



            vm.fileextensions = ["xls", "xlsx", "pdf", "doc", "docx", "txt", "XLS", "XLSX", "PDF", "DOC", "DOCX", "TXT",
                "jpg", "jpeg", "png", "JPG", "JPEG", "PNG", "zip", "ZIP"];
            $scope.getFileDetails = function (e) {
                $scope.files = [];
                var errfileList = [];
                var errfilesizeList = [];
                var maxsize = 2048000; //2MB Max

                $scope.$apply(function () {
                    //Attachmentpath = [];

                    // STORE THE FILE OBJECT IN AN ARRAY.
                    for (var i = 0; i < e.files.length; i++) {
                        $scope.files.push(e.files[i]);
                        //Attachmentpath.push(e.files[i]);

                    }
                    if ($scope.files.length == 1) {
                        vm.multidoc = $scope.files.length + " file";
                        vm.inputdoc = e.files[0].name;
                        var extn = vm.inputdoc.split(".").pop();
                        attachfiles.push(vm.inputdoc);
                        if (vm.fileextensions.includes(extn)) {
                            if (e.files[0].size <= maxsize) {
                                angular.element(document.getElementById('btnsave'))[0].disabled = false;
                                //Multipeattachment();
                            }
                            else {
                                abp.notify.error(App.localize(' File size should not be more than 2 MB!'));
                                angular.element(document.getElementById('btnsave'))[0].disabled = true;
                            }
                        }
                        else {
                            abp.notify.error(App.localize(vm.inputdoc + ' File is not Allowed!'));
                            //vm.multidoc = "";
                            angular.element(document.getElementById('btnsave'))[0].disabled = true;
                        }
                    }
                    if ($scope.files.length > 1) {
                        vm.multidoc = $scope.files.length + " files";
                        angular.forEach($scope.files, function (item, key) {
                            vm.inputdoc = item.name;
                            var extn = vm.inputdoc.split(".").pop();
                            attachfiles.push(vm.inputdoc);
                            if (vm.fileextensions.includes(extn)) {
                                if (item.size <= maxsize) {
                                    angular.element(document.getElementById('btnsave'))[0].disabled = false;
                                    //Multipeattachment();
                                }
                                else {
                                    //abp.notify.error(App.localize(' File size should not be more than 2 MB.'));
                                    angular.element(document.getElementById('btnsave'))[0].disabled = true;
                                    docsize = true;
                                    errfilesizeList.push(item.name);
                                }
                            }
                            else {
                                docextension = true;
                                errfileList.push(item.name);
                            }
                        });
                        if (docextension) {
                            var errorMessage = "";
                            if (errfileList != null) {
                                angular.forEach(errfileList, function (item) {
                                    if (errfileList.length > 1) {
                                        errorMessage += item + ",";
                                    }
                                    else {
                                        errorMessage += item;
                                    }
                                });
                                abp.notify.error(App.localize(errorMessage + ' File is not Allowed!'));
                            }
                            //vm.multidoc = "";
                            angular.element(document.getElementById('btnsave'))[0].disabled = true;
                        }
                        if (docsize) {
                            var errorsizeMessage = "";
                            if (errfilesizeList != null) {
                                angular.forEach(errfilesizeList, function (item) {
                                    if (errfilesizeList.length > 1) {
                                        errorsizeMessage += item + ",";
                                    }
                                    else {
                                        errorsizeMessage += item;
                                    }
                                });
                                abp.notify.error(App.localize('Size of the file ' + errorsizeMessage + ' is more than 2 MB!'));
                            }
                            //vm.multidoc = "";
                            angular.element(document.getElementById('btnsave'))[0].disabled = true;
                        }
                        //else {
                        //    Multipeattachment();
                        //}
                    }
                });
                //Multipeattachment();
            };

            function Multipeattachment(knowlwdgecenterId) {

                //FILL FormData WITH FILE DETAILS.
                var data = new FormData();
                //data.append("uploadedFile", files);
                for (var i in $scope.files) {
                    data.append("uploadedFile", $scope.files[i]);
                }

                //var knowlwdgecenterId = knowlwdgecenterId;
                data.append("knowlwdgecenterId", knowlwdgecenterId);

                // ADD LISTENERS.
                var uploadUrl = "../FileUpload/UploadMultipleAttachment";
                $http.post(uploadUrl, data, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (response, status) {
                    //var fileObject = response.data.Result.FileObject;

                }).finally(function () {
                    vm.saving = false;
                    $(".ladermain").hide();
                    //$window.history.back();
                    //SubmitDocument();
                });

            }

            vm.urlvalidation = function (url) {
                //var protocol = isValidHttpUrl(url);

                //var format = /^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/;
                var format = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/;

                if (!format.test(url)) {
                    //if (!format.test(url)) {
                    //alert("url error");
                    abp.notify.error(App.localize('Not a valid URL!'));
                    vm.managekc.url = "";
                    return false;
                }
            };

            function isValidHttpUrl(url) {
                let urls;
                let protocol;

                try {
                    urls = new URL(url);
                } catch (_) {
                    return false;
                }

                return protocol = urls.protocol === "http:" || urls.protocol === "https:";
            }

            function getteams() {
                knowledgeCenterService.getTeams()
                    .then(function (result) {
                        vm.teamList = result.data;
                        vm.categoryList = [];

                    });
            }
            function getcategories() {
                masterListservice.getCategories()
                    .then(function (result) {
                        vm.categoryList = result.data;
                    });
            }

            init();
        }
    ]);
})();