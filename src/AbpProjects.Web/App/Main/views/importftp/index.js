(function () {
    angular.module('app').controller('app.views.importftp', [
        '$scope', '$state', '$uibModal', 'abp.services.app.importFTPDetails', 'uiGridConstants',
        function ($scope, $state, $uibModal, importService, uiGridConstants) {
            /*ImportFTPDetailsService*/
            //debugger;
            var vm = this;
            vm.norecord = false;
            vm.import = abp.auth.isGranted('Pages.Import');
            vm.importexcel = abp.auth.isGranted('Pages.ImportExcel');
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                FilterText: vm.filterText
            };
            vm.permissions = {
                adminwrites: abp.auth.hasPermission('Pages.ImportExcel'),

            };



            vm.getAll = function () {
                abp.ui.setBusy();
                if (vm.filterText != null) {
                    vm.requestParams.FilterText = vm.filterText;

                }
                vm.loading = true;
                importService.getImportdata($.extend({}, vm.requestParams)).then(function (result) {
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    console.log(result.data.items);
                    //debugger;
                    if (result.data.totalCount == 0) {
                        vm.norecord = true;
                        $scope.noData = true;
                        $scope.nodata = true;
                        //abp.notify.info(app.localize('NoRecordFound'));
                    } else {
                        vm.norecord = false;
                        $scope.noData = false;
                        $scope.nodata = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            $scope.hoverIn = function () {
                $scope.hoverEdit = true;
            };

            $scope.hoverOut = function () {
                $scope.hoverEdit = false;
            };

            vm.createFTP = function () {
                $state.go('createftp');
            };
            vm.openFtpCreateModal = function (support) {
                //debugger;
                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/importftp/insertftpdetail.cshtml',
                    controller: 'app.views.importftp.insertftpdetail as vm',
                    backdrop: 'static',

                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };
            vm.openFtpEditModal = function (support) {

                var modalInstance = $uibModal.open({

                    templateUrl: '/App/Main/views/importftp/editFtpdetails.cshtml',
                    controller: 'app.views.importftp.editFtpdetails as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return support.id;

                        },


                    }
                });

                modalInstance.rendered.then(function () {

                    $.AdminBSB.input.activate();

                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            vm.delete = function (person) {
                //debugger;
                abp.message.confirm(
                    "Delete Domain '" + person.domainName + "'?",
                    "Delete?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            importService.deleteFtp(person.id)
                                .then(function () {
                                    abp.notify.info("Deleted Domain: " + person.domainName);
                                    vm.getAll();
                                });
                        }
                    });
            }

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: App.localize('Actions'),
                        enableSorting: false,
                        visible: vm.permissions.adminwrites == true ? true : false,
                        enableColumnMenu: false,
                        enableScrollbars: false,
                        headerCellClass: 'centeralign',
                        cellClass: 'centeralign',
                        width: 90,
                        //cellTemplate:
                        //    '<div class=\"ui-grid-cell-contents\">' +
                        //    '  <span><i ng-click="grid.appScope.edit(row.entity)"  aria-hidden="true"></i>Edit</span>' +
                        //    '  <span><i ng-click="grid.appScope.Details(row.entity)"  aria-hidden="true"></i>Delete</span>' +
                        //    '</div>',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-click="grid.appScope.openFtpEditModal(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.delete(row.entity)">' + App.localize('Delete') + '</a></li>' +

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('DomainName'),
                        field: 'DomainName',
                        enableColumnMenu: false,
                        width: 150,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.domainName}}' +
                            '</div>'
                    },
                    {
                        name: "Hosting Provider",
                        field: 'hostingProvider',
                        enableColumnMenu: false,
                        width: 170,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.hostingProvider}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('FtpUserName'),
                        displayName: 'FTP Details',
                        field: 'FtpUserName',
                        enableColumnMenu: false,
                        width: 240,

                        cellTemplate: '<div class=\"ui-grid-cell-contents \">' +
                            //'{{row.entity.title}} &nbsp;&nbsp;&nbsp;' +
                            //'<div><p ng-if="row.entity.hostName != null && row.entity.hostName !=\'\'"><b>Host Name:</b><span>{{row.entity.hostName}} </span></p><p ng-if="row.entity.hostingProvider != null && row.entity.hostingProvider !=\'\'"><b>Hosting Provider:</b><span>{{row.entity.hostingProvider}} </span></p><p ng-if="row.entity.ftpUserName != null && row.entity.ftpUserName !=\'\'"><b>User Name:</b><span>{{row.entity.ftpUserName}} </span></p><p ng-if="row.entity.ftpPassword != null && row.entity.ftpPassword !=\'\'"><b>Password:</b><span>{{row.entity.ftpPassword}} </span></p></div>' +
                            //'</div>' +
                            '<div><p ng-if="row.entity.hostName != null && row.entity.hostName !=\'\'"><b>Host Name : </b><span>{{row.entity.hostName}} </span></p><p ng-if="row.entity.hostingProvider != null && row.entity.hostingProvider !=\'\'"></p><p ng-if="row.entity.ftpUserName != null && row.entity.ftpUserName !=\'\'"><b>User Name : </b><span>{{row.entity.ftpUserName}} </span></p><p ng-if="row.entity.ftpPassword != null && row.entity.ftpPassword !=\'\'"><b>Password : </b><span>{{row.entity.ftpPassword}} </span></p></div>' +
                            '</div>' +
                            ' <div class=\" text-center\" ng-if="(row.entity.hostName == null || row.entity.hostName ==\'\') && (row.entity.ftpUserName == null || row.entity.ftpUserName ==\'\') && (row.entity.ftpPassword == null || row.entity.ftpPassword ==\'\')">-</div>' +
                            '</div>',
                    },
                    {
                        name: App.localize('Database Details'),
                        displayName: 'Database Details',
                        /* field: 'DatabaseDetails',*/
                        field: 'databaseName',
                        enableColumnMenu: false,
                        width: 230,
                        cellTemplate: '<div class=\"ui-grid-cell-contents \">' +
                            '<div><p ng-if="row.entity.onlineManager!=null && row.entity.onlineManager !=\'\'"><b>Online Manager : </b><span>{{row.entity.onlineManager}} </span></p><p ng-if="row.entity.onlineManagerHostName != null && row.entity.onlineManagerHostName !=\'\'"><b>Host Name : </b><span>{{row.entity.onlineManagerHostName}} </span></p><p ng-if="row.entity.databaseName!=null && row.entity.databaseName !=\'\'"><b>Database Name : </b><span>{{row.entity.databaseName}} </span></p><p ng-if="row.entity.dataBaseUserName!=null && row.entity.dataBaseUserName !=\'\'"><b>DB User Name : </b><span>{{row.entity.dataBaseUserName}} </span></p><p ng-if="row.entity.dataBasePassword != null && row.entity.dataBasePassword !=\'\'"><b>DB Password : </b><span>{{row.entity.dataBasePassword}} </span></p>'+
                            '<p ng-if="row.entity.dbType!=null && row.entity.dbType !=\'\'" ><b>DB Type : </b><span>{{row.entity.dbType}} </span></p></div > ' +
                            '</div>' +
                            ' <div class=\" text-center\" ng-if="(row.entity.onlineManager == null || row.entity.onlineManager ==\'\') && (row.entity.onlineManagerHostName == null || row.entity.onlineManagerHostName ==\'\') && (row.entity.databaseName == null || row.entity.databaseName ==\'\') && (row.entity.dataBaseUserName == null || row.entity.dataBaseUserName ==\'\') && (row.entity.dataBasePassword == null || row.entity.dataBasePassword ==\'\')">-</div>' +
                            '</div>',
                    },
                    {
                        name: App.localize('StorageContainer'),
                        displayName: 'Storage Container',
                        field: 'StorageContainer',
                        enableColumnMenu: false,
                        width: 180,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{row.entity.storagecontainer}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('Mail Provider'),
                        displayName: 'Mail Provider',
                        field: 'mailProvider_User',
                        /* field: 'MailProvider',*/
                        enableColumnMenu: false,
                        width: 140,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '<div><p ng-if="row.entity.mailProvider_Host != null && row.entity.mailProvider_Host != \'\'" ><b>Host : </b><span>{{row.entity.mailProvider_Host}} </span></p>' +
                            '<p ng-if="row.entity.mailProvider_User != null && row.entity.mailProvider_User !=\'\'" ><b>User : </b><span>{{row.entity.mailProvider_User}} </span></p>' +
                            '<p ng-if="row.entity.mailProvider_Password!=null && row.entity.mailProvider_Password !=\'\'" ><b>Password : </b><span>{{ row.entity.mailProvider_Password }} </span></p>' +
                            '</div > ',
                    },
                    
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getAll();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getAll();
                    });
                },
                data: []
            };

            var init = function () {
                vm.getAll();
            };

            vm.Details = function (timesheet) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/importftp/Details.cshtml',
                    controller: 'app.views.importftp.Details as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return timesheet.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            vm.onSubmit = function ($event, filterText) {
                if ($event.keyCode === 13) {
                    vm.filterText = filterText;
                    vm.getAll();
                }
            };

            vm.search = function (filterText) {
                vm.filterText = filterText;
                vm.getAll();
            };

            vm.importExcel = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/importftp/Create.cshtml',
                    controller: 'app.views.importftp.Create as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            vm.checkimportExcelBtnpermission = function () {
                if (vm.import && vm.importexcel) {
                    return true;
                } else {
                    return false;
                }

            }

            init();
        }
    ]);



})();