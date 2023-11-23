
(function () {
    angular.module('app').controller('app.views.reports.projectwisetimesheet', [
        '$scope', '$timeout', '$uibModal', '$http', 'abp.services.app.reports', 'abp.services.app.masterList', 'uiGridConstants',
        function ($scope, $timeout, $uibModal, $http, reportsService, masterListService, uiGridConstants) {
            var vm = this;
            vm.resultlist = [];
            vm.userlist = [];
            vm.reports = {};
            vm.isChecked = true;
            vm.btndisable = true;

            function getProjects() {
                masterListService.getProject().then(function (result) {
                    vm.projectlist = result.data;
                });
            }
            function getMonths() {
                reportsService.getMonth().then(function (result) {
                    vm.monthlist = result.data;
                });
            }
            function getYears() {
                reportsService.getYear().then(function (result) {
                    vm.yearlist = result.data;
                });
            }
            
            vm.bindproject = function () {

                vm.reports.projectId = 0;
              
                if (vm.reports.year == null || vm.reports.year == undefined || vm.reports.year == "") {
                    abp.notify.error("Please select year.");
                    return;
                }
                if (vm.reports.month == null || vm.reports.month == undefined || vm.reports.month == "") {
                    abp.notify.error("Please select month.");
                    return;
                }
                abp.ui.setBusy();
                masterListService.getTimesheetwise_ProjectList(vm.reports)
                    .then(function (result) {
                    if (result.data != null) {
                        vm.projectlist = result.data;
                    }
                    else {
                        vm.projectlist = null;
                    }
                }).finally(function () {
                    vm.loading = false;
                    abp.ui.clearBusy();
                });

            }

            vm.projectchange = function () {
                if ((vm.reports.year != null && vm.reports.year != undefined && vm.reports.year != "") && (vm.reports.month != null && vm.reports.month != undefined && vm.reports.month != "") && (vm.reports.projectId != null && vm.reports.projectId != undefined && vm.reports.projectId != "")) {
                    vm.btndisable = false;
                } else {
                    vm.btndisable = true;
                }
            }

            vm.searchAll = function () {
                
                if (vm.reports.year == null || vm.reports.year == undefined || vm.reports.year == "") {
                    abp.notify.error("Please select year.");
                    return;
                }
                if (vm.reports.month == null || vm.reports.month == undefined || vm.reports.month == "") {
                    abp.notify.error("Please select month.");
                    return;
                }
                if (vm.reports.projectId == null || vm.reports.projectId == undefined || vm.reports.projectId == "") {
                    abp.notify.error("Please select project.");
                    return;
                }
                abp.ui.setBusy();
                reportsService.getProjectWiseTimesheetReport(vm.reports).then(function (result) {
                    if (result.data.length > 0) {
                        vm.resultlist = result.data;
                        vm.userlist = result.data[0].timesheetData;

                        $scope.noData = false;
                        vm.isChecked = false;
                    }
                    else {
                        $scope.noData = true;
                        vm.isChecked = true;
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

            vm.exportExcel = function () {

                tableToExcel('divexport', 'test', 'DownloadTimesheet');

                //$scope.divexport.table2excel({
                //    filename: "Table.xls"
                //});
            }


            init = function () {
                getYears();
                var year = new Date().getFullYear();
                vm.reports.year = year + "";
                getMonths();

                //var month = moment(new Date().getMonth(), 'MM').format('MMM');
                vm.reports.month = new Date().getMonth() + "";
                $scope.noData = true;
                vm.bindproject();
                //vm.reports.year = new Date().getYear();
                //vm.reports.month = new Date().getMonth();
            }


            init();

            function tableToExcel(table, sheetName, fileName) {


                var ua = window.navigator.userAgent;
                var msie = ua.indexOf("MSIE ");
                if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
                {
                    return fnExcelReport(table, fileName);
                }

                var uri = 'data:application/vnd.ms-excel;base64,',
                    templateData = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>',
                    base64Conversion = function (s) { return window.btoa(unescape(encodeURIComponent(s))) },
                    formatExcelData = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

                $("tbody > tr[data-level='0']").show();

                if (!table.nodeType)
                    table = document.getElementById(table)

                var ctx = { worksheet: sheetName || 'Worksheet', table: table.innerHTML }

                var element = document.createElement('a');
                element.setAttribute('href', 'data:application/vnd.ms-excel;base64,' + base64Conversion(formatExcelData(templateData, ctx)));
                element.setAttribute('download', fileName);
                element.style.display = 'none';
                document.body.appendChild(element);
                element.click();
                document.body.removeChild(element);

                $("tbody > tr[data-level='0']").hide();
            }

            function fnExcelReport(table, fileName) {

                var tab_text = "<table border='2px'>";
                var textRange;

                if (!table.nodeType)
                    table = document.getElementById(table)

                $("tbody > tr[data-level='0']").show();
                tab_text = tab_text + table.innerHTML;

                tab_text = tab_text + "</table>";
                tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
                tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
                tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(tab_text);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", false, fileName + ".xls");
                $("tbody > tr[data-level='0']").hide();
                return (sa);
            }
        }
    ]);
})();