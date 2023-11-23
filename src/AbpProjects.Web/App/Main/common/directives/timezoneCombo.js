(function () {
    var app = angular.module("app");
    app.directive('timezoneCombo', ['abp.services.app.timing',
        function (timingService) {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/App/Main/common/directives/timezoneCombo.cshtml',
                scope: {
                    defaultTimezoneScope: '=',
                    selectedTimezone: '=?'
                },
                link: function ($scope, element, attrs) {
                    $scope.timeZones = [];

                    $(element).addClass('edited');

                    timingService.getTimezones({
                        defaultTimezoneScope: $scope.defaultTimezoneScope
                    }).then(function (result) {
                        $scope.timeZones = result.data.items;
                    });
                }
            };
        }
    ]);
})();