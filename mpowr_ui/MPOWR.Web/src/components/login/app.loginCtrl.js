/*
Login Controller: This controller is responsible for doing login actions
Created by: Aamin Khan
Created at: 19/01/2017
*/
angular.module("hpe").controller("loginCtrl", loginCtrl);

function loginCtrl($scope,$state,loginService,$localStorage,$rootScope) {
    //ngInject
    $scope.loginErr = 'hidden';
    $scope.spin = false;
    $scope.login = function (user) {
    	//call login service
    	$scope.spin = true;
        loginService.login(user).then(function onSuccess(response) {
		    // Handle success
		    var data = response.data;
		    var status = response.status;
		    var statusText = response.statusText;
		    var headers = response.headers;
		    var config = response.config;
		    
		    $localStorage.user = data[0];
		    $scope.spin = false;
			$state.go("dashboard");
		  }, 

		  function onError(response) {
		    // Handle error
		    var data = response.data;
		    var status = response.status;
		    var statusText = response.statusText;
		    var headers = response.headers;
		    var config = response.config;
		    $scope.loginErr = 'visible';
		    $scope.spin = false;
		  })
        
    }
}

