

(function () {
    var app = angular.module("app")
        .directive('uploadfiles', ['$timeout', '$parse', '$http',
            function ($timeout, $parse, $http) {
                return {

                    restrict: 'A', //the directive can be used as an attribute only
                    templateUrl: "/App/Main/directives/fileuploader.cshtml",
                    scope: {
                        extlst: '=extensionlist',
                        maxsize: '@',
                        foldername: '@',
                        uploadurl: '@',
                        model: '=filedata'
                    },

                    link: function (scope, element, attrs) {
                        ////Bind change event on the element
                        element.bind('change', function (e) {
                            //Call apply on scope, it checks for value changes and reflect them on UI

                            scope.files = e.target.files;


                            if (scope.files.length > 0) {
                                var i = 0;
                                angular.forEach(scope.files, function (item, key) {
                                    var filename = item.name;
                                    if (filename != null) {
                                        var extn = filename.split(".").pop();
                                        if (scope.extlst.includes(extn)) {
                                            if (item.size <= scope.maxsize) {
                                                i++;
                                            }
                                            else {
                                                abp.notify.error('File size exceeds maximum limit MB');
                                                $("#filetoupload").val('');
                                                $("#textlogo").val('');
                                            }
                                        }
                                        else {
                                            abp.notify.error("Incorrect " + filename + " formate.");
                                            $("#filetoupload").val('');
                                            $("#textlogo").val('');
                                        }
                                    }
                                });
                            }
                            e.preventDefault();
                            scope.$apply(function () {

                                var resetHandler = function () {
                                    scope.$apply(function () {
                                        scope.files.length = 0;
                                        
                                    });
                                };

                                if (element[0].form) {
                                    angular.element(element[0].form).bind('reset', resetHandler);
                                }

                                // Watch the files so we can reset the input if needed
                                //scope.$watchCollection('files', function () {
                                //    if (scope.files.length === 0) {
                                //        element.val(null);
                                //    }
                                //})


                            });
                        });

                    }
                }
            }
        ])

})();







