(function () {
   
    angular.module('app').controller('app.views.importftp.editFtpdetails', [
        '$scope', '$uibModalInstance', 'abp.services.app.importFTPDetails', 'id',
        function ($scope, $uibModalInstance, importFTPDetailsService, id) {
            
            var vm = this;
            vm.ftp = {};
            vm.loading = false;
            /*vm.ftpdata = {};*/
            var init = function () {
                
                importFTPDetailsService.getServiceForEdit(id)
                
                    .then(function (result) {
                        
                        vm.ftp = result.data;
                        if (vm.ftp.hostingProvider == undefined) {
                            vm.ftp.hostingProvider = "";
                        }
                        if (vm.ftp.ftpUserName == undefined) {
                            vm.ftp.ftpUserName = "";
                        }
                        if (vm.ftp.ftpPassword == undefined) {
                            vm.ftp.ftpPassword = "";
                        }
                        if (vm.ftp.dbType == undefined) {
                            vm.ftp.dbType = "";
                        }
                        if (vm.ftp.onlineManager == undefined) {
                            vm.ftp.onlineManager = "";
                        }
                        if (vm.ftp.onlineManagerHostName == undefined) {
                            vm.ftp.onlineManagerHostName = "";
                        }

                        if (vm.ftp.databaseName == undefined) {
                            vm.ftp.databaseName = "";
                        }
                        if (vm.ftp.dataBaseUserName == undefined) {
                            vm.ftp.dataBaseUserName = "";
                        }
                        if (vm.ftp.dataBasePassword == undefined) {
                            vm.ftp.dataBasePassword = "";
                        }
                        if (vm.ftp.storagecontainer == undefined) {
                            vm.ftp.storagecontainer = "";
                        }

                        if (vm.ftp.mailProvider_Host == undefined) {
                            vm.ftp.mailProvider_Host = "";
                        }
                        if (vm.ftp.mailProvider_User == undefined) {
                            vm.ftp.mailProvider_User = "";
                        }
                        if (vm.ftp.mailProvider_Password == undefined) {
                            vm.ftp.mailProvider_Password = "";
                        }
                        vm.ftp.domainName = result.data.domainName + "";
                        vm.ftp.hostName = result.data.hostName + "";
                        vm.ftp.hostingProvider = result.data.hostingProvider + "";
                        vm.ftp.ftpUserName = result.data.ftpUserName + "";
                        vm.ftp.ftpPassword = result.data.ftpPassword + "";
                       

                       
                        vm.ftp.dbType = result.data.dbType + "";
                        vm.ftp.onlineManager = result.data.onlineManager + "";
                        vm.ftp.onlineManagerHostName = result.data.onlineManagerHostName + "";
                       

                        vm.ftp.databaseName = result.data.databaseName + "";
                        vm.ftp.dataBaseUserName = result.data.dataBaseUserName + "";
                        vm.ftp.dataBasePassword = result.data.dataBasePassword + "";
                        vm.ftp.storagecontainer = result.data.storagecontainer + "";
                        vm.ftp.mailProvider_Host = result.data.mailProvider_Host + "";
                        vm.ftp.mailProvider_User = result.data.mailProvider_User + "";
                        vm.ftp.mailProvider_Password = result.data.mailProvider_Password + "";
                        
                        
                       
                        console.log(vm.ftp);

                    });
            }


            vm.save = function () {
                vm.loading = true;
                importFTPDetailsService.ftpExsistenceById(vm.ftp).then(function (result) {
                    debugger;

                    if (!result.data) {
                        importFTPDetailsService.updateService(vm.ftp).then(function () {
                            abp.notify.info(App.localize('FTP Details Updated Successfully'));
                            $uibModalInstance.close();
                        });
                    }
                    else {
                        abp.notify.error('Domain Name is already exists');
                        vm.loading = false;
                    }
                });
            };





            //vm.save = function () {
            //    importFTPDetailsService.updateService(vm.ftp)
            //        .then(function () {
            //            abp.notify.success(App.localize('FTP Details Saved Successfully '));

            //            $uibModalInstance.close();
            //        });

            //};
            init();
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

        }
    ]);
})();