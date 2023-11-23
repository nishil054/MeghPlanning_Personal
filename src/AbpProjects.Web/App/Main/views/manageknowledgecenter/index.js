(function () {
    angular.module('app').controller('app.views.manageknowledgecenter', [
        '$scope', '$uibModal', 'abp.services.app.knowledgeCenter', 'uiGridConstants', 'abp.services.app.masterList',
        function ($scope, $uibModal, knowledgeCenterService, uiGridConstants, masterListservice) {
           
            var vm = this;
            vm.showteamColumn = "false";
            vm.categoryId = null;
            vm.knowledgecenter = abp.auth.isGranted('Pages.KnowledgeCenter');
            vm.knowledgecrud = abp.auth.isGranted('Pages.KnowledgeCenterCrud');
            vm.status = false;
            vm.loading = false;
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                Filter: vm.filterText,
                TeamId: vm.searchBox,
                CategoryId: vm.categoryId
            };

            vm.getAll = function () {
                abp.ui.setBusy();
                if (vm.searchBox != null || vm.searchBox != undefined || vm.searchBox != "") {
                    vm.requestParams.TeamId = vm.searchBox;
                }
                
                if (vm.categoryId != null || vm.categoryId != undefined || vm.categoryId != "") {
                    vm.requestParams.CategoryId = vm.categoryId;
                }
                if (vm.filterText != null || vm.filterText != undefined || vm.filterText != "") {
                    vm.requestParams.Filter = vm.filterText;
                }
                vm.loading = true;
                $("#s2id_drpcat").select2("val", null);

                knowledgeCenterService.getKnowledgeCenter($.extend({}, vm.requestParams)).then(function (result) {
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    $("#s2id_drpcat").select2("val", vm.categoryId);

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
                    abp.ui.clearBusy();
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
                columnDefs: [{
                    name: App.localize('Actions'),
                    enableSorting: false,
                    enableColumnMenu: false,
                    enableScrollbars: false,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign verticalcenter',

                    width: 70,
                    visible: vm.knowledgecrud,
                    //cellTemplate:
                    //    '<div class=\"ui-grid-cell-contents\">' +
                    //    '  <span><i ng-click="grid.appScope.Details(row.entity)" class="fa fa-eye" aria-hidden="true"></i></span>' +
                    //    '</div>',
                    cellTemplate: '<div class=\"ui-grid-cell-contents padd0\" ng-if="grid.appScope.checkknowledgeCenterBtnpermission()==true">' +
                        '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                        '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                        '    <ul uib-dropdown-menu>' +
                        '      <li><a  ng-click="grid.appScope.Edit(row.entity)">' + App.localize('Edit') + '</a></li>' +
                        //'      <li><a ng-click="grid.appScope.Details(row.entity)">' + App.localize('Details') + '</a></li>' +
                        '      <li><a ng-click="grid.appScope.Delete(row.entity)">' + App.localize('Delete') + '</a></li>' +
                        '    </ul>' +
                        '  </div>' +
                        '</div>'
                },
                {
                    name: App.localize('Team'),
                    field: 'Team',
                    enableColumnMenu: false,
                    width: 150,
                    visible: vm.knowledgecrud,
                    //visible: getteamsFun(),
                    headerCellClass: 'leftalign',
                    cellClass: 'verticalcenter ',

                    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                        '{{row.entity.team}}' +
                        '</div>'
                },
                {
                    name: App.localize('Category'),
                    field: 'Category',
                    enableColumnMenu: false,
                    width: 180,
                    headerCellClass: 'leftalign',
                    cellClass: 'verticalcenter ',

                    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                        '{{row.entity.category}}' +
                        '</div>'
                },
                {
                    name: App.localize('Title'),
                    field: 'Title',
                    enableColumnMenu: false,
                    /* width: 450,*/
                    innerHeight: 450,
                    headerCellClass: 'leftalign',
                    cellClass: 'verticalcenter',
                    cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                        //'{{row.entity.title}} &nbsp;&nbsp;&nbsp;' +
                        '<div><p ng-if="row.entity.title!=null && row.entity.title !=\'\'">{{row.entity.title}}</p><p><b>Comment: </b><span ng-if="row.entity.comment!=null && row.entity.comment !=\'\'">{{row.entity.comment}} </span></p></div>' +
                        '</div>'
                },
                {
                    name: App.localize('Url / Document'),
                    field: 'Url',
                    enableColumnMenu: false,
                    width: 215,
                    headerCellClass: 'centeralign',
                    cellClass: 'centeralign',
                    enableSorting: false,
                    cellTemplate: '<div class=\"ui-grid-cell-contents breakall\ " ng-show="row.entity.isDocument == 0" >' +
                        //'<a href="' + "http://" + '{{row.entity.url}}" target="_blank"> {{row.entity.url}}</a>' +
                        '<a ng-click="grid.appScope.downloaduRL(row.entity.url)"> {{row.entity.url}}</a>' +
                        '</div>' +
                        '<div class=\"ui-grid-cell-contents\" ng-show="row.entity.isDocument == 1">' +
                        //'<a href="javascript:void(0);" class="downloadDoc" id="{{data.id}}"  data-filename="{{data.documentName}}" ><i class="fa fa-download" style="font-size:large" aria-hidden="true"></i></a>' +
                        '<a ng-click="grid.appScope.downloadFile(row.entity.knowledgeDocuments[0].documentName, row.entity.id)" ><i class="fa fa-download" style="font-size:large" aria-hidden="true"></i></a>' +
                        '</div>'
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

            vm.refresh = function () {
                vm.getAll();
            };



            vm.Details = function (data) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/manageknowledgecenter/Details.cshtml',
                    controller: 'app.views.manageknowledgecenter.Details as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return data.id;
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

            vm.Edit = function (data) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/manageknowledgecenter/Edit.cshtml',
                    controller: 'app.views.manageknowledgecenter.Edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return data.id;
                        }
                    }
                });

                modalInstance.rendered.then(function () {
                    $timeout(function () {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function (res) {
                    //alert(1);
                    //console.log(res);
                    vm.getAll();
                });
            };



            vm.downloadFile = function (filepath, id) {
                //debugger;
                if (filepath != null && filepath != "" && filepath != undefined) {
                    var filepath = "userfiles/KnowledgeCenter/" + filepath;
                    window.open(filepath, "_blank");
                }
                else {
                    knowledgeCenterService.getknowledgeDocuments({ id: id })
                        .then(function (result) {
                            debugger;
                            var documentName = result.data.documentName;
                            var filepath = "userfiles/KnowledgeCenter/" + documentName;
                            window.open(filepath, "_blank");
                        });
                }
            };

            vm.downloaduRL = function (urllink) {
                var url = urllink;
                window.open(url, "_blank");
            };

            vm.Delete = function (item) {
                abp.message.confirm(
                    "Delete Knowledge Center'" + item.title + "'?",
                    "Delete?",
                    function (result) {
                        if (result) {

                            knowledgeCenterService.deleteKnowledgeCenter({ id: item.id })
                                .then(function () {
                                    //abp.notify.success("Deleted Knowledge Center : " + item.title);
                                    abp.notify.success("Deleted Successfully");
                                    vm.getAll();
                                });
                        }
                    });
            };

            vm.search = function (filterText) {
                vm.loading = true;
                vm.filterText = filterText;
                vm.getAll();
            };

            vm.clearAll = function () {
                vm.filterText = "";
                vm.teamList = [];
                vm.categoryList = [];
                vm.searchBox = null;
                vm.categoryId = null;
                vm.getAll();
                getteams();
                //getcategories();
                
            };

            vm.openCreateModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/manageknowledgecenter/Create.cshtml',
                    controller: 'app.views.manageknowledgecenter.Create as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function () {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    //alert(JSON.stringify(res));
                    //console.log(res);
                    vm.getAll();
                });
            };

            $scope.changeteam = function (teamid) {
                if (teamid != null) {
                    masterListservice.getCategoriesByTeam(teamid)
                        .then(function (result) {
                            vm.categoryList = result.data;
                        });
                }
                else {
                    
                    vm.categoryId = "";
                    
                }

            }

            function getteams() {
                knowledgeCenterService.getTeams()
                    .then(function (result) {
                        vm.teamList = result.data;
                        if (result.data.length == 1) {
                            vm.searchBox = vm.teamList[0].id + "";
                            angular.element(document.getElementById('drpteam'))[0].disabled = true;
                            vm.showteamColumn = false;
                        } else {
                            vm.teamList = result.data;
                            vm.showteamColumn = true;
                        }

                    }).finally(function () {
                        vm.loading = false;
                        vm.getAll();
                    });

                //alert(vm.showteamColumn);
            }

            function getcategories() {
                masterListservice.getCategories()
                    .then(function (result) {
                        vm.categoryList = result.data;
                    });
            }

            vm.checkknowledgeCenterBtnpermission = function () {
                if (vm.knowledgecenter && vm.knowledgecrud) {
                    vm.status = true;
                    return true;
                } else {
                    vm.status = false;
                    return false;
                }

            }

            var init = function () {
                vm.requestParams = {
                    skipCount: 0,
                    maxResultCount: app.consts.grid.defaultPageSize,
                    sorting: "Id desc",
                    Filter: vm.filterText,
                    TeamId: vm.searchBox,
                    CategoryId: vm.categoryId
                };
                vm.getAll();
                getteams();
               // getcategories();
            };

            init();
        }
    ]);



})();