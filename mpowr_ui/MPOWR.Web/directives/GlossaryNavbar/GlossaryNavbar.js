

(function () {
	'use strict';
	angular.module('hpe').directive('glossarynav', function () {
		// Runs during compile
		return {
			// name: '',
			// priority: 1,
			// terminal: true,
			// scope: {}, // {} = isolate, true = child, false/undefined = no change
			controller: function ($scope, $element, $window, $attrs, $transclude, $state, $localStorage, $http, $rootScope, Notification, $timeout) {

				$http.get($rootScope.api + 'Glossary/GetGlossaryDetails').success(function (response) {
					$scope.menus = response;
					$scope.selectedPage = $scope.Data[0];
					$scope.gridOptions.data = $scope.selectedPage.ParameterDetails;
				});
				
			},
			// require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
			restrict: 'AE', // E = Element, A = Attribute, C = Class, M = Comment
			// template: '',
			templateUrl: 'directives/GlossaryNavbar/GlossaryNavbar.html',
			// replace: true,
			// transclude: true,
			// compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
			link: function ($scope, iElm, iAttrs, controller) {

			}

		};
	});
})();
