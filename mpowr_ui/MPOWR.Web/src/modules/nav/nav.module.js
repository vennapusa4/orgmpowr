/*
App module: Top panel module
Created by: Aamin Khan
Created at: 23/01/2017
Dependencies: bootstrap css, font-awesome
*/
(function () {
    'use strict';

    angular.module('navBar.module', ['ngStorage']);

    angular.module('navBar.module').directive('navBar', function(){
        // Runs during compile
        return {
            // name: '',
            // priority: 1,
            // terminal: true,
            // scope: {}, // {} = isolate, true = child, false/undefined = no change
            controller: function($scope, $element, $attrs, $transclude,$timeout,$localStorage,$state) {
                $timeout(function(){
                $(".top-navbar").click(function(e){
                        if( $(e.target).closest(".icon .fa-bars").length > 0 ) {
                            return false;
                        }
                        $(".leftbar").css("width",'81px');
                        $(".menu-items > ul > li > a > h5").hide();
                    })  
                },100);

                //logout

                $scope.logout = function(){
                    delete $localStorage.user;
                    $state.go('login');
                }

                $scope.expandBut = true;
                if($state.current.name == 'dashboard'){
                    $scope.expandBut = false;
                }

                $scope.userName = $localStorage.user.FirstName;
            },
            // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
            // restrict: 'A', // E = Element, A = Attribute, C = Class, M = Comment
            // template: '',
            templateUrl: 'modules/nav/nav.html',
            // replace: true,
            transclude: true,
            // compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
            link: function($scope, iElm, iAttrs, controller) {

                

                $scope.toggle = function(){
                        $(".leftbar").css("width",'245px');
                        $(".leftbar").css("z-index",'11');
                        $(".menu-items > ul > li > a > h5").show();
                }



            }
        };
    });


})();
