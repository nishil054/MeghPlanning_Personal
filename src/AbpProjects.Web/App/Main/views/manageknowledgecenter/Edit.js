(function () {
    angular.module('app').controller('app.views.manageknowledgecenter.Edit', [
        '$scope', '$uibModalInstance', 'abp.services.app.knowledgeCenter', 'id', '$http', 'abp.services.app.masterList',
        function ($scope, $uibModalInstance, knowledgeCenterService, id, $http, masterListservice) {
            var vm = this;
            vm.saving = false;
            vm.items = {};
            vm.showdoc = true;
            vm.showdocgrid = false;
            var docextension = false;
            var docsize = false;
            var attachfiles = [];
            vm.loading = false;
          
            function editData() {
                knowledgeCenterService.getknowledgeCenterForEdit({ id: id })
                    .then(function (result) {
                        vm.items = result.data;
                        vm.items.isDocument = vm.items.isDocument + "";
                        getCategorybyId(vm.items.teamId);
                        if (vm.items.isDocument == "1") {
                            vm.oldDocValue = vm.items.isDocument;
                            vm.showdoc = true;
                            vm.showdocgrid = true;
                        }
                        else if (vm.items.isDocument == "0") {
                            vm.showdoc = false;
                            vm.showdocgrid = false;
                        }
                        vm.sourceList = [
                            { sourceId: 1, sourceName: "Document" },
                            { sourceId: 0, sourceName: "Url" }
                        ];
                        getteams();
                        getCategorybyId(teamid);
                    });
            }
            $scope.changeteam = function (teamid) {
                getCategorybyId(teamid);
              
            }
            function getCategorybyId(teamid) {
                masterListservice.getCategoriesByTeam(teamid)
                    .then(function (result) {
                        vm.categoryList = result.data;
                        vm.items.teamId = vm.items.teamId + "";
                        vm.items.categoryId = vm.items.categoryId + "";

                    });
            }
            vm.save = function () {
                vm.saving = true;
                vm.loading = true;
                if (vm.items.isDocument == "1") {
                    var inputattachment = $scope.files;
                    //if (!vm.knowledgeCenterAttachments.length > 0 && vm.oldDocValue == vm.items.isDocument) {
                    if (!vm.knowledgeCenterAttachments.length > 0 &&
                        (inputattachment == "" || inputattachment == null || inputattachment == undefined || inputattachment == "null")) {
                        abp.notify.error(App.localize('SelectDocument'));
                        return false;
                    }
                }
                else if (vm.items.isDocument == "0") {
                    if (vm.items.url == "" || vm.items.url == null || vm.items.url == undefined || vm.items.url == "null") {
                        abp.notify.error(App.localize('EnterUrl'));
                        return false;
                    }
                }
                if (vm.items.isDocument == "1") {
                    vm.items.url = null;
                }
                vm.items.attachments = attachfiles;

                knowledgeCenterService.knowledgeCenterExsistenceById(vm.items).then(function (result) {
                    if (!result.data) {
                        knowledgeCenterService.updateKnowledgeCenter(vm.items).then(function () {
                            abp.notify.success(App.localize('UpdatedSuccessfully'));
                            Multipeattachment(id);
                            $uibModalInstance.close();
                        });
                    } else {
                        abp.notify.error(App.localize('Title Already Exist.'));
                        vm.loading = false;
                    }
                });
            };

            vm.deletefile = function (items) {
                abp.message.confirm(
                    "Are you sure you want to delete this item?", "",
                    function (result) {
                        if (result) {
                            knowledgeCenterService.deleteknowledgeCenterFiles({ id: items.id })
                                .then(function () {
                                    abp.notify.success("Doument Deleted");
                                    getknowledgeCenterDocuments(vm.id);
                                    //vm.multidoc = "";
                                    //$scope.files = [];
                                });
                        }
                    });
            };

            vm.isdocselected = function (value) {
                if (value == "1") {
                    vm.showdoc = true;
                }
                else {
                    vm.showdoc = false;
                }
            }

            //vm.isdocselected = function (value) {
            //    if (value) {
            //        //vm.docmandatory = false;
            //        //if (vm.managekc.documentTitle != "" && vm.managekc.documentTitle != null && vm.managekc.documentTitle != undefined) {
            //        //    angular.element(document.getElementById('btnsave'))[0].disabled = false;
            //        //}
            //        //var inputattachment = $('#filetoupload')[0].files[0];
            //        vm.showdoc = true;
            //        if (vm.items.url == null || vm.items.url == "" || vm.items.url == undefined) {
            //            vm.showdocgrid = true;
            //            vm.showdoc = false;
            //        }
            //        else {
            //            vm.showdocgrid = false;
            //            vm.showdoc = true;
            //        }
            //    }
            //    else {
            //        vm.showdoc = false;
            //        vm.showdocgrid = false;
            //        //if ((inputattachment == "" || inputattachment == null || inputattachment == undefined)) {
            //        //    angular.element(document.getElementById('btnsave'))[0].disabled = true;
            //        //    vm.showdoc = true;
            //        //}
            //    }
            //};

            vm.fileextensions = ["xls", "xlsx", "pdf", "doc", "docx", "txt", "XLS", "XLSX", "PDF", "DOC", "DOCX", "TXT",
                "jpg", "jpeg", "png", "JPG", "JPEG", "PNG", "zip", "ZIP"];
            $scope.getFileDetails = function (e) {
                $scope.files = [];
                var errfileList = [];
                var errfilesizeList = [];
                var maxsize = 2048000; //2MB Max
                $scope.files.length = 0;
                vm.items.attachments = [];
                vm.items.attachmentpath = [];
                vm.items.newAttachments = [];
                $scope.$apply(function () {
                    // STORE THE FILE OBJECT IN AN ARRAY.
                    for (var i = 0; i < e.files.length; i++) {
                        $scope.files.push(e.files[i]);

                    }
                    if ($scope.files.length == 1) {
                        vm.multidoc = $scope.files.length + " file";
                        vm.inputdoc = e.files[0].name;
                        var extn = vm.inputdoc.split(".").pop();

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
                            abp.notify.error(App.localize(vm.inputdoc + ' File is not Allowed.'));
                            //vm.multidoc = "";
                            angular.element(document.getElementById('btnsave'))[0].disabled = true;
                        }
                    }
                    if ($scope.files.length > 1) {
                        vm.multidoc = $scope.files.length + " files";
                        angular.forEach($scope.files, function (item, key) {
                            vm.inputdoc = item.name;
                            var extn = vm.inputdoc.split(".").pop();
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
                                abp.notify.error(App.localize(errorMessage + ' File is not Allowed.'));
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
                        //    //Multipeattachment();
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
                data.append("knowlwdgecenterId", knowlwdgecenterId);

                // ADD LISTENERS.
                var uploadUrl = "../FileUpload/UploadMultipleAttachment";
                $http.post(uploadUrl, data, {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (response, status) {

                }).finally(function () {
                    vm.saving = false;
                    vm.loading = false;
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
                    vm.items.url = "";
                    return false;
                }
            };

            //function isValidHttpUrl(url) {
            //    let urls;
            //    let protocol;

            //    try {
            //        urls = new URL(url);
            //    } catch (_) {
            //        return false;
            //    }

            //    return protocol = urls.protocol === "http:" || urls.protocol === "https:";
            //}

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            vm.close = function () {
                $uibModalInstance.dismiss({});
            };

            vm.downloadFile = function (filepath) {
                var filepath = "userfiles/KnowledgeCenter/" + filepath;
                window.open(filepath, "_blank");
            };

            function getteams() {
                knowledgeCenterService.getTeams()
                    .then(function (result) {
                        vm.teamList = result.data;
                    });
            }

           

            function getknowledgeCenterDocuments() {
                $(".ladermain").show();

                knowledgeCenterService.getknowledgeCenterDocuments({ id: id }).then(function (result) {
                    vm.knowledgeCenterAttachments = result.data.items;
                    $(".ladermain").hide();
                });
            }

            var init = function () {
                getteams();
                editData();
                
                getknowledgeCenterDocuments(vm.id);
                //knowledgeCenterService.getknowledgeCenterForEdit({ id: id })
                //    .then(function (result) {
                //        debugger;
                //        vm.items = result.data;
                //        vm.items.teamId = vm.items.teamId + "";
                //        vm.items.categoryId = vm.items.categoryId + "";
                //        vm.items.isDocument = vm.items.isDocument + "";
                //        if (vm.items.isDocument == "1") {
                //            vm.oldDocValue = vm.items.isDocument;
                //            vm.showdoc = true;
                //            vm.showdocgrid = true;
                //        }
                //        else if (vm.items.isDocument == "0") {
                //            vm.showdoc = false;
                //            vm.showdocgrid = false;
                //        }
                //        vm.sourceList = [
                //            { sourceId: 1, sourceName: "Document" },
                //            { sourceId: 0, sourceName: "Url" }
                //        ];
                //    });

            };

            init();
        }
    ]);
})();