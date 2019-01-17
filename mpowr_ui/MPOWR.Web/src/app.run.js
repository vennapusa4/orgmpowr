(function () {
    'use strict';

    angular.module("hpe").run(rn);

    function rn($rootScope,$timeout,$localStorage,authService) {
        $rootScope.api = "http://localhost:54961/api/";
	    

	    $rootScope.$on('$stateChangeStart', function(event, toState, toStateParams) {
	        
	        if(typeof $localStorage.user == 'undefined'){
				// Here you can take the control and call your own functions:
		    	window.location.href= '#/login';
		    }

	    });
	    $rootScope.labels = {
		    	Login:{
		    		username: 'Username',
		    		password: 'Password',
		    		ErrMsg: {
		    			auth: 'Please provide valid username / password',
		    			invalidUsername: 'Invalid username.',
		    			requireUsername: 'Please enter username.',
		    			requirePassword: 'Please enter password.'
		    		},
		    		button:{
		    			Login: 'Login'
		    		}
		    	}
		    }
    }

})();

