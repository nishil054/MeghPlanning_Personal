////(function () {
////    'use strict';

////    angular
////        .module('app')
////        .controller('switchButton', switchButton);

////    switchButton.$inject = ['$location'];

////    function switchButton($location) {
////        /* jshint validthis:true */
////        var vm = this;
////        vm.title = 'switchButton';

////        activate();

////        function activate() { }
////    }
////})();


var myApp = angular.module('myApp', []);

myApp.controller('myController', ['$scope',
    function ($scope) {

    }
]).directive('switchButton', function () {
    return {
        restrict: 'E',
        'template': '<input type="checkbox" class="bs-switch" name="my-checkbox1" checked>',
        'link': function (scope, element, attrs) {
            $(element).find('input').bootstrapSwitch();
        }
    }
});