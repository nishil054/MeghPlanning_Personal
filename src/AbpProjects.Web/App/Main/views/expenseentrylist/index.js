(function () {
    angular.module('app').controller('app.views.expenseentrylist.index', [
        '$scope', '$state', '$timeout', '$uibModal', 'abp.services.app.expenseEntryForm', 'uiGridConstants',
        function ($scope, $state, $timeout, $uibModal, expenseEntryFormService, uiGridConstants) {
            
            var vm = this;
            vm.norecord = false;
            vm.categories = [];
            vm.requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: "Id desc",
                monthYear: vm.categories.monthYear
            };
            var today = new Date();
            var regdatee = moment.utc(today);
            vm.date = regdatee;
            vm.monthyr;
            vm.getAll = function (monthYeardata) {
                vm.monthyr = monthYeardata;
                vm.requestParams.monthYear = moment(monthYeardata).format("MM/DD/YYYY");

                expenseEntryFormService.getExpenseEntry(vm.monthyr)
                    .then(function (result) {
                        vm.categories = result.data.items;
                            vm.categories.monthYear = monthYeardata;
                    }).finally(function () {
                        vm.loading = false;
                    });


            }
            $scope.datechange = function (monthYeardata) {
                vm.getAll(monthYeardata);
            }
          
            vm.save = function () {
                vm.saving = true;
               
                expenseEntryFormService.createExpenseEntry(vm.categories, vm.categories.monthYear).then(function () {
                    abp.notify.info(App.localize('SavedSuccessfully'));
                    vm.getAll(vm.categories.monthYear);
                        });
            };

            vm.userGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enablePaginationControls: false,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
    
                {
                    name: App.localize('Category'),
                    field: 'Category',
                    enableColumnMenu: false,
                    width: 250,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                        '{{row.entity.category}}' +
                        '</div>'
                },
                {
                    name: App.localize('SubCategory'),
                    field: 'SubCategory',
                    enableColumnMenu: false,
                    width: 250,
                    cellTemplate: '<div class=\"ui-grid-cell-contents\" style=\"text-align:left\">' +
                        '{{row.entity.subCategory}}' +
                        '</div>'
                    },
                     
                    {
                        name: App.localize('Date'),
                        field: 'Expense',
                        enableColumnMenu: false,
                        width: 250,
                        cellTemplate: '<div class=\"ui-grid-cell-contents\" >' +
                            '<input class="form-control"  min="0" maxlength="12" type="number" name="priority" ng-model=" row.entity.expense" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength)">' +
                            '</div>',

                       
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
                        $scope.datechange();
                        
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {

                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        $scope.datechange();
                    });
                },
                data: []
            };

            var init = function () {
                let date = new Date()
                let day = date;
                var regdate = moment.utc(day);
                monthYeardata = regdate;
                vm.categories.monthYear = regdate;
                vm.getAll(vm.categories.monthYear);
            };

            init();
        }
    ]);



})();