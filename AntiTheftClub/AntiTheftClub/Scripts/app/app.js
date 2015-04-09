// create the module and name it scotchApp
var scotchApp = angular.module('scotchApp', ['ngRoute', 'ngMessages', 'ui.bootstrap', 'preRegApp']);

// configure our routes
scotchApp.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider

        // route for the home page
        .state('preRegister', {
            url: '/preRegister',
            templateUrl: '/app-view/pre-reg/home.html',
            controller: 'mainController'
        })
         .state('about', {
             url: '/about',
             templateUrl: '/app-view/pre-reg/about.html',
             controller: 'aboutController'
         })
        .state('contact', {
            url: '/contact',
            templateUrl: '/app-view/pre-reg/contact.html',
            controller: 'contactController'
        })

});

// create the controller and inject Angular's $scope
scotchApp.controller('mainController', function ($scope) {
    // create a message to display in our view
    $scope.message = 'Everyone come and see how good I look!';
});

scotchApp.controller('aboutController', function ($scope) {
    $scope.message = 'Look! I am an about page.';
});

scotchApp.controller('contactController', function ($scope) {
    $scope.message = 'Contact us!. This is just a demo.';
});


scotchApp.directive('capitalizeFirst', function ($parse) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {
                if (inputValue === undefined) { inputValue = ''; }
                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                }
                return capitalized;
            }
            modelCtrl.$parsers.push(capitalize);
            capitalize($parse(attrs.ngModel)(scope)); // capitalize initial value
        }
    };
});


var notequalTo = function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=notequalTo"
        },
        link: function (scope, element, attributes, ngModel) {
            debugger;
            ngModel.$validators.notequalTo = function (modelValue) {
                return modelValue != scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
};

scotchApp.directive("notequalTo", notequalTo);



scotchApp.directive('validFile', function () {
    return {
        require: 'ngModel',
        link: function (scope, el, attrs, ngModel) {
            //change event is fired when file is selected
            el.bind('change', function () {
                scope.$apply(function () {
                    ngModel.$setViewValue(el.val());
                    ngModel.$render();
                });
            });
        }
    }
});


