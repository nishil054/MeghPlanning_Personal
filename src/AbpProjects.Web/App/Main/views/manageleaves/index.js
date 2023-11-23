(function () {
    angular.module('app').controller('app.views.manageleaves', [
        '$scope', '$uibModal', 'abp.services.app.manageLeaves', 'uiGridConstants',
        function ($scope, $uibModal, manageLeavesService, uiGridConstants) {

            var vm = this;
            vm.dataobject = [];
            vm.norecord = false;
            //vm.knowledgecenter = abp.auth.isGranted('Pages.KnowledgeCenter');
            //vm.knowledgelist = abp.auth.isGranted('Pages.KnowledgeCenterList');
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                CategoryFilter: vm.filterText
            };
            vm.getAll = function () {
                abp.ui.setBusy();
                if (vm.filterText != null || vm.filterText != undefined || vm.filterText != "") {
                    vm.requestParams.CategoryFilter = vm.filterText;
                }
                vm.loading = true;
                manageLeavesService.getUserdata($.extend({}, vm.requestParams)).then(function (result) {
                    //vm.userGridOptions.totalItems = result.data.totalCount;
                    vm.userGridOptions.totalItems = result.data.length;
                    vm.userGridOptions.data = result.data;
                    if (result.data.totalCount == 0) {
                        vm.norecord = true;
                        //abp.notify.info(app.localize('NoRecordFound'));
                    }
                    else {
                        vm.norecord = false;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enablePaginationControls: false,
                //paginationPageSizes: app.consts.grid.defaultPageSizes,
                //paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: false,
                useExternalSorting: true,
                enableSorting: false,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: App.localize('NameSurname'),
                        field: 'FullName',
                        enableColumnMenu: false,
                        width: 400,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                            '{{row.entity.name}} {{row.entity.surname}}' +
                            '</div>'
                    },
                    {
                        name: App.localize('LeaveBalance'),
                        field: 'leaveBalance',
                        enableColumnMenu: false,
                        width: 250,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                            /*'{{row.entity.leaveBalance}}' +*/
                            '<input type="text" name="leavebalance" style="width:65px;" ng-blur="grid.appScope.CheckNumber(row.entity)" step="0.01" ng-model="row.entity.leaveBalance" required class="validate form-control">' +
                            '</div>'
                    },
                    {
                        name: 'Pending Leaves',
                        field: 'pendingLeaves',
                        enableColumnMenu: false,
                        width: 250,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                            /*'{{row.entity.leaveBalance}}' +*/
                            '<input type="text" name="pendingLeaves" style="width:65px;" ng-blur="grid.appScope.CheckNumber(row.entity)" step="0.01" ng-model="row.entity.pendingLeaves" required class="validate form-control">' +
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
                    //gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {

                    //    vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                    //    vm.requestParams.maxResultCount = pageSize;

                    //    vm.getAll();
                    //});
                },
                data: []
            };

            var init = function () {
                vm.getAll();
            };

            vm.search = function (filterText) {
                vm.filterText = filterText;
                vm.getAll();
            };

            vm.CheckNumber = function (data) {
                var format = /^\d{0,4}(\.\d{0,3})?$/;
                var charformat = /[a-zA-Z]/g;;

                if (!charformat.test(data.leaveBalance)) {
                    if (format.test(data.leaveBalance)) {
                        angular.element(document.getElementById('btnsave'))[0].disabled = false;
                    }
                    else {
                        abp.notify.error(App.localize('You must include three decimal places!'));
                        data.leaveBalance = null;
                        angular.element(document.getElementById('btnsave'))[0].disabled = true;
                    }
                }
                else {
                    abp.notify.error(App.localize('Characters are not allowed!'));
                    data.leaveBalance = null;
                    angular.element(document.getElementById('btnsave'))[0].disabled = true;
                }


                if (!charformat.test(data.pendingLeaves)) {
                    if (format.test(data.pendingLeaves)) {
                        angular.element(document.getElementById('btnsave'))[0].disabled = false;
                    }
                    else {
                        abp.notify.error(App.localize('You must include three decimal places!'));
                        data.pendingLeaves = null;
                        angular.element(document.getElementById('btnsave'))[0].disabled = true;
                    }
                }
                else {
                    abp.notify.error(App.localize('Characters are not allowed!'));
                    data.pendingLeaves = null;
                    angular.element(document.getElementById('btnsave'))[0].disabled = true;
                }

                //ng-keypress
                //if (event.keyCode === 46) {

                //}

                //else if (isNaN(event.key) || event.key === ' ' || event.key === '') {
                //    event.returnValue = '';
                //}
            };

            init();

            vm.save = function () {
                abp.ui.setBusy();
                angular.forEach(vm.userGridOptions.data, function (item) {
                    vm.dataobject.push({
                        id: item.id,
                        leaveBalance: item.leaveBalance,
                        pendingLeaves: item.pendingLeaves
                    });
                });
                console.log('vm.dataobject', vm.dataobject);
                manageLeavesService.updateBulkLeaves(vm.dataobject)
                    .then(function ()
                    {
                    abp.notify.info(App.localize('SavedSuccessfully'));
                    
                    }).finally(function () {
                        vm.loading = false;
                        abp.ui.clearBusy();
                        vm.getAll();
                    });
            };
        }
    ]);



})();