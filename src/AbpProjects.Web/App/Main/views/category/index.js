(function() {
    angular.module('app').controller('app.views.category', [
        '$scope', '$uibModal', 'abp.services.app.knowledgeCenter', 'abp.services.app.category', 'uiGridConstants',
        function ($scope, $uibModal, knowledgeCenterService,categoryService, uiGridConstants) {

            var vm = this;
            vm.norecord = false;
            vm.categories = {};
            //vm.knowledgecenter = abp.auth.isGranted('Pages.KnowledgeCenter');
            //vm.knowledgelist = abp.auth.isGranted('Pages.KnowledgeCenterList');
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                CategoryFilter: vm.filterText,
                teamFilter: vm.categories.teamId
            };
            vm.getAll = function () {

                abp.ui.setBusy();
                if (vm.filterText != null || vm.filterText != undefined || vm.filterText != "") {
                    vm.requestParams.CategoryFilter = vm.filterText;
                }
                if (vm.categories.teamId != undefined) {
                    vm.requestParams.teamFilter = vm.categories.teamId;
                }
                else {
                    vm.categories.teamId = null;
                }
                vm.loading = true;
                categoryService.getCategory($.extend({}, vm.requestParams)).then(function(result) {
                    vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.data = result.data.items;
                    if (result.data.totalCount == 0) {
                        vm.norecord = true;
                        //abp.notify.info(app.localize('NoRecordFound'));
                    } else {
                        vm.norecord = false;
                    }
                }).finally(function() {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
               
            }
            function getTeams() {
                knowledgeCenterService.getTeams()
                    .then(function (result) {
                        vm.teamList = result.data;
                    });
            }

            vm.bindteamname = function (teamnamelist) {
                var teamname = "";
                angular.forEach(teamnamelist, function (v1, k1) {
                    teamname += v1 + ",";
                });
                return teamname.substring(0, teamname.length - 1);
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
                        cellClass: 'centeralign',
                        // headerCellClass: 'centeralign',
                        // cellClass: 'centeralign',
                        width: 70,
                        //cellTemplate:
                        //    '<div class=\"ui-grid-cell-contents\">' +
                        //    '  <span><i ng-click="grid.appScope.Details(row.entity)" class="fa fa-eye" aria-hidden="true"></i></span>' +
                        //    '</div>',
                        cellTemplate: '<div class=\"ui-grid-cell-contents padd0\">' +
                            '  <div class="btn-group dropdown bg_none" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue btn_none" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="material-icons">more_vert</i> ' + ' <!--<span class="caret"></span>--></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-click="grid.appScope.Edit(row.entity)">' + App.localize('Edit') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.Details(row.entity)">' + App.localize('Details') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.Delete(row.entity)">' + App.localize('Delete') + '</a></li>' +

                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: App.localize('Category'),
                        field: 'Category',
                        enableColumnMenu: false,
                        //width: 250,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                            '{{row.entity.category}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('Team'),
                        enableColumnMenu: false,
                        field: 'teamName',
                        enableSorting: false,
                        //width: 180,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\">' +
                            '{{ grid.appScope.bindteamname(row.entity.teams) }}' +
                            '</div>'
                    },
                ],
                onRegisterApi: function(gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function(grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = "Id desc"
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getAll();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function(pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getAll();
                    });
                },
                data: []
            };

            var init = function () {
                vm.requestParams = {
                    skipCount: 0,
                    maxResultCount: app.consts.grid.defaultPageSize,
                    sorting: "Id desc",
                    CategoryFilter: vm.filterText,
                    teamFilter: vm.categories.teamId
                };
                getTeams()
                vm.getAll();
            };

            vm.clear = function () {
                vm.filterText = "";
                vm.teamList = [];
                vm.categories.teamId = null;
                vm.requestParams.CategoryFilter = null;
                vm.requestParams.teamFilter = null;
                getTeams();
                vm.getAll();
            };

            vm.Details = function(category) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/category/Details.cshtml',
                    controller: 'app.views.category.Details as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return category.id;
                        }
                    }
                });

                modalInstance.rendered.then(function() {
                    $timeout(function() {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function() {
                    vm.getAll();
                });
            };

            vm.Edit = function(category) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/category/Edit.cshtml',
                    controller: 'app.views.category.Edit as vm',
                    backdrop: 'static',
                    resolve: {
                        id: function() {
                            return category.id;
                        }
                    }
                });

                modalInstance.rendered.then(function() {
                    $timeout(function() {
                        $.AdminBSB.input.activate();
                    }, 0);
                });

                modalInstance.result.then(function() {
                    vm.getAll();
                });
            };

            vm.Delete = function(item) {
                abp.message.confirm(
                    "Delete Category '" + item.category + "'?",
                    "Delete?",
                    function(result) {
                        if (result) {

                            categoryService.deleteCategory({ id: item.id })
                                .then(function() {
                                    //abp.notify.info("Deleted Category : " + item.category);
                                    abp.notify.success("Deleted Successfully");
                                    vm.getAll();
                                });
                        }
                    });
            };

            vm.search = function(filterText) {
                vm.filterText = filterText;
                vm.getAll();
            };

            //vm.clearAll = function () {
            //    vm.searchBox = null;
            //    vm.filterText = "";
            //    vm.getAll();
            //};

            vm.openCreateModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/category/Create.cshtml',
                    controller: 'app.views.category.Create as vm',
                    backdrop: 'static'
                });

                modalInstance.rendered.then(function() {
                    $.AdminBSB.input.activate();
                });

                modalInstance.result.then(function () {
                    init();
                    //vm.getAll();
                });
            };

            init();
        }
    ]);



})();