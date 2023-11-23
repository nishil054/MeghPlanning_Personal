(function () {
    var app = angular.module("app");
    app.directive('inlineAutocomplete', function ($timeout) {
        return {
            scope: {

                handleSave: '&onSave',
                handleCancel: '&onCancel',
                model: '=inlineAutocomplete',
                searchAPI: '=searchAPI',
                titlename: '=titleName',
                minvalue: '=minValue',
                displayname: '=displayValue',
                initialvalue: '=initialvalue'
            },
            link: function (scope, elm, attr) {
                var previousValue;
                scope.editAutocomplete = function () {
                    scope.editMode = true;
                    previousValue = scope.model;
                    $timeout(function () {
                        elm.find('input')[0].focus();
                    }, 0, false);
                };
                scope.saveAutocomplete = function () {
                    if (scope.model > 0) {
                        scope.editMode = false;
                        //angular.forEach(scope.searchAPI, function (value, key) {
                        //    if (value.selectobject == scope.model) {
                        //        scope.titlename = value.titlename;
                        //    }
                        //});
                        scope.handleSave({ value: scope.model });
                    }
                    else {
                        alert("Please select Legal Form");
                        return false;
                    }
                };
                scope.cancelAutocomplete = function () {
                    scope.editMode = false;
                    scope.model = previousValue;
                    scope.handleCancel({ value: scope.model });
                };
            },
            templateUrl: "/App/Main/directives/inline-autocomplete.cshtml"
        };
    });
    app.directive('onEsc', function () {
        return function (scope, elm, attr) {
            elm.bind('keydown', function (e) {
                if (e.keyCode === 27) {
                    scope.$apply(attr.onEsc);
                }
            });
        };
    });
    app.directive('onEnter', function () {
        return function (scope, elm, attr) {
            elm.bind('keypress', function (e) {
                if (e.keyCode === 13) {
                    scope.$apply(attr.onEnter);
                }
            });
        };
    });
})();
