(function () {
    'use strict';
    angular.module('hpe').directive('glossary', function () {
        // Runs during compile
        return {
            // name: '',
            // priority: 1,
            // terminal: true,
            // scope: {}, // {} = isolate, true = child, false/undefined = no change
            controller: function ($http, $scope, $element, $rootScope, $attrs, $transclude, $state, $localStorage, $sessionStorage) {

                $scope.$on('refreshGlossary', function (event, data) {
                    if (data == undefined || data.length == 0) {
                        $http.get($rootScope.api + 'Glossary/GetGlossaryDetails').success(function (response) {
                            $scope.Data = response;
                            SetScreenNumber();
                        });
                    }
                    else {
                        $scope.Data = data;
                        SetScreenNumber();
                    }

                });

                function SetScreenNumber()
                {
                    var count = 0;
                    angular.forEach($scope.Data, function (e) {
                        if (!e.IsChild) {
                            e.prefix = count;
                            var childScreen = $scope.Data.filter(function (screen) { return screen.IsChild && screen.ParentScreenID == e.ID });
                            var childCount = 1;
                            angular.forEach(childScreen, function (screen) {
                                screen.prefix = count + "." + childCount;
                                childCount++;
                            })
                            count++;
                        }
                    })
                }

                $scope.DataRefreshModel = function (x) {
                    $scope.searchResults = $sessionStorage.searchResultss;
                    $('.modal').removeClass('modal-backdrop.in');
                }


            },
            // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
            restrict: 'AE', // E = Element, A = Attribute, C = Class, M = Comment
            // template: '',
            templateUrl: 'directives/glossary/glossary.html',
            // replace: true,
            // transclude: true,
            // compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
            link: function ($scope, iElm, iAttrs, controller) {

            }

        };
    });
})();
