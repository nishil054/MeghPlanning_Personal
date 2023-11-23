(function () {
    var app = angular.module("app");
    app.directive('morrisbarchart', function () {
        return {
            templateUrl: "/App/Main/directives/morrisChartDirective.cshtml",
            scope: {
                chartname: "@",
                xdata: "=info"
            },
            link: function (scope, element, attrs) {
                var init = function () {
                    scope.ctx = document.getElementById("myChart");
                    Morris.Bar({
                        element: scope.ctx,
                        data: scope.xdata,
                        xkey: 'y',
                        ykeys: ['a'],
                        labelTop: true,
                        labels: ['No of Invoice'],
                        xLabelAngle: 45
                    });
                };
                init();
            }
        };
    });
})();