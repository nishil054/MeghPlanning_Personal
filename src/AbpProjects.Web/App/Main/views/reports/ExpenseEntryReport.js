
(function () {
    angular.module('app').controller('app.views.reports.expenseEntryReport', [
       '$scope', '$state', 'abp.services.app.reports', 'abp.services.app.expcategory',
        function ($scope,$state, reportsService, expcategoryService) {
            var vm = this;

            vm.yearId;
            
            vm.fddinancyear = '';

            vm.years1 = [];
            
            //$scope.IsAllCollapsed = false;
            //$scope.data = vm.expenseData;
            //$scope.collapseAll = function () {
            //    $scope.IsAllCollapsed = !$scope.IsAllCollapsed;
            //    $scope.data.forEach(function (item) {
            //        item.isCollapsed = $scope.IsAllCollapsed;
            //    })
            //}

            


            
            vm.get = function (yearId) {
                vm.yearId = yearId;
                reportsService.getExpenseEntryReport(vm.yearId).then(function (result) {
                    vm.expenseData = result.data;
                    $scope.IsAllCollapsed = false;
                    $scope.data = vm.expenseData;
                    $scope.collapseAll = function () {
                        $scope.IsAllCollapsed = !$scope.IsAllCollapsed;
                        $scope.data.forEach(function (item) {
                            item.isCollapsed = $scope.IsAllCollapsed;
                        })
                    }
                    //console.log(vm.expenseData);
                    var sum = 0;
                    vm.financyear = vm.expenseData[0].financialYear;
                    vm.monthyear = vm.expenseData[0].monthYear;

                    //console.log(vm.monthyear);
                    var array = vm.expenseData[0].financialYear.split("-");

                    var currentfinYear = vm.expenseData[0].financialYear.substr(vm.expenseData[0].financialYear.indexOf("-") + 1);
                    $('#ddlcom1').empty();
                    $.each(vm.expenseData[0].mFiancialYearDropdownList, function (index, value) {
                        $('#ddlcom1').append($('<option>').text(value.text).attr('value', value.value));
                    });

                    $('#ddlcom1').val(array[0]); 
                    if (result.data.length == 0) {
                        $scope.noData = true;
                    }
                    else {
                        $scope.noData = false;
                    }
                   
                });
                reportsService.getExpenseEntryReportTotal(vm.yearId).then(function (result) {
                    vm.totalFinancialYrsales = result.data;
                    console.log(vm.totalFinancialYrsales);
                });
            }

            vm.refreshGrid = function () {
                var selectedyr = $("#ddlcom1 :selected").val();
                vm.get(parseInt(selectedyr) + 1);
            };
            vm.clear = function () {
                var selectedyr = new Date().getFullYear();
                vm.get(selectedyr);
            };
            var init = function () {

            }
            init();
                vm.get(new Date().getFullYear());
              
        }

    ]);
})();