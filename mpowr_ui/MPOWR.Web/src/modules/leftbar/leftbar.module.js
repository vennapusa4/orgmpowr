/*
App module: left navigation bar module
Created by: Aamin Khan
Created at: 23/01/2017
Dependencies: bootstrap css, font-awesome
*/
(function () {
    'use strict';

    angular.module('leftbar.module', []);

    angular.module('leftbar.module').directive('leftBar', function(){
        // Runs during compile
        return {
            // name: '',
            // priority: 1,
            // terminal: true,
            // scope: {}, // {} = isolate, true = child, false/undefined = no change
            controller: function($scope, $element, $attrs, $transclude) {

            },
            // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
            // restrict: 'A', // E = Element, A = Attribute, C = Class, M = Comment
            // template: '',
            templateUrl: 'modules/leftbar/leftbar.html',
            // replace: true,
            transclude: true,
            // compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
            link: function($scope, iElm, iAttrs, controller) {
                
            }
        };
    });


})();
