(function(){
	'use strict';
	angular.module('hpe').service('loginService', loginServiceFn);

	function loginServiceFn($rootScope,$http){
		//ngInject
		this.login = function(user){
			var promise = 
			$http({
				method: 'POST',
				url: $rootScope.api + 'Users/checkUser',
				data: user
			}).then(function(resp){
				return resp;
			});
			return promise;
		}
	}

})();