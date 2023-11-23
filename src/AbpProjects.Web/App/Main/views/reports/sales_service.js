
(function () {
    angular.module('app').controller('app.views.sales.service', [
        '$state', 'abp.services.app.reports',
        function ($state, reportsService) {
            var vm = this;

            vm.yearId;
            vm.totalFinancialYrsales = 0;
            vm.fddinancyear = '';

            vm.years1 = [];

            vm.get = function (yearId) {
                abp.ui.setBusy();
                vm.yearId = yearId;
                reportsService.getSalesReport_Service(vm.yearId).then(function (result) {
                    vm.salesData = result.data;
                    console.log(result.data);
                    var sum = 0;
                    vm.financyear = vm.salesData[0].financialYear;
                    var array = vm.salesData[0].financialYear.split("-");
                    var currentfinYear = vm.salesData[0].financialYear.substr(vm.salesData[0].financialYear.indexOf("-") + 0);
                    $('#ddlcom1').empty();
                    $.each(vm.salesData[0].mFiancialYearDropdownList, function (index, value) {
                        $('#ddlcom1').append($('<option>').text(value.text).attr('value', value.value));
                    });

                    $('#ddlcom1').val(array[0]);
                    //$("#HowYouKnow option:contains(" + theText + ")").attr('selected', 'selected');
                    $.each(vm.salesData, function (index, value) {
                        var capacity = parseInt(value.totalYearsales);
                        sum += capacity;
                    });
                    vm.totalFinancialYrsales = sum;
                    //console.log(result.data);
                    if (result.data.length == 0) {
                        //vm.resultlist = result.data;
                        $scope.noData = true;
                        //vm.isChecked = false;
                    }
                    else {
                        $scope.noData = false;
                        //vm.isChecked = true;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }

            vm.refreshGrid = function () {
                var selectedyr = $("#ddlcom1 :selected").val();
                vm.get( parseInt(selectedyr));
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