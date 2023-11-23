(function () {
    var app = angular.module("app");
    app.directive('fullHeightGrid', ['$timeout', '$window',
        function ($timeout, $window) {
            return {
                restrict: 'A',
                scope: false,
                require: 'uiGrid',
                link: function (scope, element, attrs, uiGridCtrl) {
                    var setGridHeight = function () {
                        var windowHeight = window.innerHeight;
                        var gridTop = getGridTop();
                        var gridHeight = windowHeight - gridTop - 40;
                        if (gridHeight < 380) gridHeight = 380;
                        //alert(gridHeight);
                        $(element).height(gridHeight);
                        uiGridCtrl.grid.api.core.handleWindowResize();
                        $('.ui-grid-canvas').css('height', 'auto');
                        $('.ui-grid').css('height', 'auto');
                        $('.ui-grid-viewport').css('overflow', 'visible');
                        $('.ui-grid-viewport').css('overflow-anchor', 'none');
                    };
                    var getGridTop = function () {
                        return $(element)[0].getBoundingClientRect().top + document.documentElement.scrollTop;
                    };
                    $timeout(setGridHeight);
                    angular.element($window).bind('resize', setGridHeight);
                    scope.$watch(function () {
                        return getGridTop();
                    }, function (gridTop) {
                        setGridHeight();
                    });

                    scope.$watch('isDisplayed', function (newValue, oldValue) {
                        var viewport = element.find('.ui-grid-render-container');

                        ['touchstart', 'touchmove', 'touchend', 'keydown', 'wheel', 'mousewheel', 'DomMouseScroll', 'MozMousePixelScroll'].forEach(function (eventName) {
                            viewport.unbind(eventName);
                        });
                    });

                }
            };
        }
    ]);
})();