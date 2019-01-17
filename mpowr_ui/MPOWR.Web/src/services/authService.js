(function(){
	'use strict';
	angular.module('hpe').service('authService', authFn);

	function authFn($localStorage,$state){
		//ngInject
		this.checkUser =  function(){
			if(typeof $localStorage.user == 'undefined'){
				
		    	window.location.href= '#';
		    }
		}
	}
})();