(function(){
	'use strict';
	angular.module('hpe').service('authService', authFn);

	function authFn($localStorage,$state,$sessionStorage,ExitService){
		//ngInject
		this.checkUser =  function(){
			if(typeof $localStorage.user == 'undefined'){
				
		    	window.location.href= '#';
		    }
		}

		this.logout = function(){
			var user = {};
            // user.Username = $scope.userName;
            user.Username = $localStorage.user.UserID;
            
            ExitService.Exit(user);
           
		    // Delete Session
            delete $sessionStorage.fyParams;
            delete $sessionStorage.resellerPartner;
            delete $sessionStorage.lastUpdatedDate;
            delete $localStorage.selectedversion;
            delete $sessionStorage.currentVersion;
		    // Delete localStorage
            delete $localStorage.showSpartlines;
            delete $localStorage.user;
            delete $localStorage.preYear;
            delete $localStorage.prePeriod;
            delete $localStorage.previousYear;
            delete $localStorage.previoustoPreviousYear;
            delete $localStorage.businessUnits;
            delete $localStorage.Admin;
            delete $localStorage.PartnerTypeID;
            $state.go('login');
		}
	}
})();