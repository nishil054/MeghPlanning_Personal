(function () {
    angular.module('app')
        .directive('barchart', ['$timeout',
            function ($timeout) {
                return {
                    templateUrl: "/App/Main/directives/commonchart.cshtml",
                    scope: {
                        //chartname: "@",
                        chartid: "@",
                        xdata: "=info",
                        ydata: "=data",
                        lableprint: "=printdata",
                        xlabel: "@",
                        ylabel: "@"
                    },
                    link: function (scope, element, attrs) {


                        var lst = [];
                        var cnt = scope.xdata.length; 
                        angular.forEach(scope.xdata, function (value, key) {
                             lst.push(value);

                        });

                        $('#barchart').append('<div id=' + scope.chartid + '></div>');
                        var cd = document.getElementById(scope.chartid);
                        Morris.Bar({
                            element: cd,
                            data: lst,
                            xkey: 'y',
                            ykeys: scope.ydata,
                            labels: scope.ydata,
                            gridTextSize: 12,
                            resize: true,
                            xLabelAngle: 60,
                            barColors: ['#3DA5D9', '#218380'],
                             
                            hideHover: true
                        });

                        // $('#chartbar').append('<canvas id=' + scope.chartid + '></canvas>');
                        //var cd = document.getElementById(scope.chartid);
                        //ctx = cd.getContext('2d');
                        //var myChart = new Chart(ctx, {
                        //    type: 'bar',
                        //    data: {
                        //        labels: scope.lableprint,
                        //        datasets: [{
                        //            label: scope.xlabel,
                        //            data: scope.xdata,
                        //            backgroundColor: '#3DA5D9',
                        //            fill: false
                        //        }, {
                        //            label: scope.ylabel,
                        //            data: scope.ydata,
                        //            backgroundColor: '#218380',
                        //            fill: false
                        //        }]
                        //    }
                        //});
                        scope.$watchCollection("data", function (newArray) {
                        });
                        Controller.$inject = ['$scope'];
                        function Controller($scope) {

                        }
                    },
                };
            }
        ]);
    //app.directive('piedoughnut', function () {
    //    return {
    //        templateUrl: "/App/Main/directives/commonchart.cshtml",
    //        scope: {
    //            chartid: "@",
    //            charttype: "@",
    //            xdata: "=info",
    //            lableprint: "=printdata"
    //        },
    //        link: function (scope, element, attrs) {
    //            scope.color = [];
    //            var randomColorGenerator = function () {
    //                return '#' + (Math.random().toString(16) + '0000000').slice(2, 8);
    //            };
    //            for (i = 0; i < scope.xdata.length; i++) {
    //                var colour = randomColorGenerator();
    //                scope.color.push(colour);
    //            }
    //            $('#canvaschart').append('<canvas id=' + scope.chartid + '></canvas>');
    //            var cd = document.getElementById(scope.chartid);
    //            ctx = cd.getContext('2d');
    //            var myChart = new Chart(ctx, {
    //                type: scope.charttype,
    //                data: {
    //                    labels: scope.lableprint,
    //                    datasets: [{
    //                        backgroundColor: scope.color,
    //                        data: scope.xdata
    //                    }]
    //                }
    //            });
    //        }
    //    };
    //});
    //app.directive('donutchart', function () {
    //    return {
    //        templateUrl: "/App/Main/directives/commonchart.cshtml",
    //        scope: {
    //            piechartname: "@",
    //            chartid: "@",
    //            xdata: "=info",
    //            lableprint: "=printdata"
    //        },
    //        link: function (scope, element, attrs) {
    //            scope.color = [];
    //            var randomColorGenerator = function () {
    //                return '#' + (Math.random().toString(16) + '0000000').slice(2, 8);
    //            };
    //            for (i = 0; i < scope.xdata.length; i++) {
    //                var colour = randomColorGenerator();
    //                scope.color.push(colour);
    //            }
    //            var ctx = document.getElementById("mydonutChart").getContext('2d');
    //            var myChart = new Chart(ctx, {
    //                type: 'doughnut',
    //                data: {
    //                    labels: scope.lableprint,
    //                    datasets: [{
    //                        backgroundColor: scope.color,
    //                        data: scope.xdata
    //                    }]
    //                }
    //            });
    //        }
    //    };
    //});
    //app.directive('singledatadonutchart', function () {
    //    return {
    //        templateUrl: "/App/Main/directives/commonchart.cshtml",
    //        scope: {
    //            piechartname: "@",
    //            xdata: "=info",
    //            lableprint: "=printdata"
    //        },
    //        link: function (scope, element, attrs) {

    //            const percent = 84;
    //            const color = '#01713c';
    //            const canvas = 'chartCanvas';
    //            const container = 'chartContainer';
    //            const percentValue = percent; // Sets the single percentage value
    //            const colorGreen = color, // Sets the chart color
    //                animationTime = '1400'; // Sets speed/duration of the animation
    //            const chartCanvas = document.getElementById(canvas), // Sets canvas element by ID
    //                chartContainer = document.getElementById(container), // Sets container element ID
    //                divElement = document.createElement('div'), // Create element to hold and show percentage value in the center on the chart
    //                domString = '<div class="chart__value"><p>' + percentValue + '%</p></div>'; // String holding markup for above created element
    //            // Create a new Chart object
    //            const doughnutChart = new Chart(chartCanvas, {
    //                type: 'doughnut', // Set the chart to be a doughnut chart type
    //                data: {
    //                    datasets: [
    //                        {
    //                            data: [percentValue, 100 - percentValue], // Set the value shown in the chart as a percentage (out of 100)
    //                            backgroundColor: [colorGreen], // The background color of the filled chart
    //                            borderWidth: 0 // Width of border around the chart
    //                        }
    //                    ]
    //                },
    //                options: {
    //                    cutoutPercentage: 84, // The percentage of the middle cut out of the chart
    //                    responsive: false, // Set the chart to not be responsive
    //                    tooltips: {
    //                        enabled: false // Hide tooltips
    //                    }
    //                }
    //            });
    //            Chart.defaults.global.animation.duration = animationTime; // Set the animation duration
    //            divElement.innerHTML = domString; // Parse the HTML set in the domString to the innerHTML of the divElement
    //            chartContainer.appendChild(divElement.firstChild); // Append the divElement within the chartContainer as it's child
    //        }
    //    };
    //});
    //app.directive('singlebarchart', function () {
    //    return {
    //        templateUrl: "/App/Main/directives/commonchart.cshtml",
    //        scope: {
    //            chartid: "@",
    //            xdata: "=info"
    //        },
    //        link: function (scope, element, attrs) {
    //            var data = scope.xdata,
    //                config = {
    //                    data: data,
    //                    xkey: 'y',
    //                    ykeys: ['a'],
    //                    labels: ['Total'],
    //                    fillOpacity: 0.6,
    //                    hideHover: 'auto',
    //                    behaveLikeLine: true,
    //                    resize: true,
    //                    pointFillColors: ['#ffffff'],
    //                    pointStrokeColors: ['black'],
    //                    lineColors: ['gray']
    //                };
    //            config.element = 'bar-chart';
    //            Morris.Bar(config);
    //        }
    //    };
    //});
})();