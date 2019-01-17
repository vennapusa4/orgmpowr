(function(){
	'use strict';
	angular.module('hpe').directive('breadcrumbBar', function(){
		// Runs during compile
		return {
			// name: '',
			// priority: 1,
			// terminal: true,
			// scope: {}, // {} = isolate, true = child, false/undefined = no change
			controller: function($scope, $element, $attrs, $transclude,$state) {
				var pointer = 0;
				$scope.menus = [
					{
						name: 'Budget Allocation',
						url: 'budget.allocation',
						active: false
					},
					{
						name: 'Historical Perfomance',
						url: 'budget.historical-perfomance',
						active: false
					},
					{
						name: 'Model Parameters',
						url: 'budget.model-parameter',
						active: false
					},
					{
						name: 'Partner Credit',
						url: '',
						active: false
					},
					{
						name: 'Final summary',
						url: '',
						active: false
					}

				];
				//$scope.menus[pointer].active = false;

				switch($state.current.name){
					case 'budget.allocation':
						   pointer = 0;	
						   $scope.menus[pointer].active = true;
						   break
					case 'budget.historical-perfomance':
							pointer = 1;
							$scope.menus[pointer].active = true;
							break
					case 'budget.model-parameter':
							pointer = 2;
							$scope.menus[pointer].active = true;
							break
				}

				$scope.goto = function(menu){
					$state.go(menu.url);
				}

			},
			// require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
			 restrict: 'AE', // E = Element, A = Attribute, C = Class, M = Comment
			// template: '',
			 templateUrl: 'directives/breadcrumb/breadcrumb.html',
			// replace: true,
			// transclude: true,
			// compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
			link: function($scope, iElm, iAttrs, controller) {

			}

		};
	});
})();
