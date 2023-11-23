(function () {
    angular.module('app').controller('app.views.gstData', [
        '$scope', '$state', '$uibModal', '$stateParams', 'abp.services.app.reports', 'uiGridConstants', 'abp.services.app.financialYear',
        function ($scope, $state, $uibModal, $stateParams, reportsService, uiGridConstants, financialYearService) {
            /*ImportFTPDetailsService*/
            //debugger;
            var vm = this;
            vm.norecord = false;
            vm.financialyearId = 0;
            vm.finalamount = 0;
            vm.financialyearList = [];
            vm.GSTDataList;

            function getAll(financialyearId) {
                vm.loading = true;
                abp.ui.setBusy();
                reportsService.getGSTDataReport(financialyearId).then(function (result) {
                    vm.GSTDataList = result.data;
                    var trHTML = '';
                    var trfooterHTML = '';
                    $('#tblbody').empty();
                    $('#tblfooter').empty();
                    vm.finalamount = 0;

                    $.each(result.data, function (i, item) {
                        trHTML += "<tr>";
                        trHTML += "<td>" + result.data[i].monthyear + "</td>";
                        $.each(result.data[i].companynameList, function (j, item) {
                            trHTML += "<td style=text-align:right>" + item.gstAmount + "</td>";
                        });
                        trHTML += "<td style=text-align:right>" + result.data[i].gstAmount + "</td>";
                        trHTML += "</tr>";
                    });
                    $('#tblbody').append(trHTML);

                    trfooterHTML += "<tr>";
                    trfooterHTML += "<td>" + 'Total' + "</td>";
                    $.each(result.data[0].companynameList, function (i, item) {
                        trfooterHTML += "<td style=text-align:right>" + item.columnTotal + "</td>";
                        vm.finalamount +=item.columnTotal;
                    });
                    trfooterHTML += "<td style=text-align:right>" + vm.finalamount + "</td>";
                    trfooterHTML += "</tr>";

                    $('#tblfooter').append(trfooterHTML);

                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            var init = function () {
                vm.getFinancialYears();
            };

            vm.refreshGrid = function (n) {
                var selectedyr = $("#ddlcom :selected").val();
                getAll(selectedyr);
            };

            vm.getFinancialYears = function () {
                abp.ui.setBusy();
                financialYearService.getFinancialYearList(
                    $.extend({
                    }, vm.requestParams)
                ).then(function (result) {
                    vm.financialyearList = result.data;
                    vm.financialyearId = result.data[0].id
                    vm.id = result.data[0].id
                    $('#ddlcom').empty();
                    $.each(result.data, function (index, value) {
                        $('#ddlcom').append($('<option>').text(value.title).attr('value', value.id));
                    });

                    $('#ddlcom').val(result.data[0].id);
                    getAll(result.data[0].id);

                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });
            }
          
            init();
        }
    ]);
})();