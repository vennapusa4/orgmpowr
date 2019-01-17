/*
Dashboard Controller: This is the controller for default landing page
Created by: Aamin Khan
Created at: 19/01/2017
*/
angular.module("hpe").controller("dashboardCtrl", dashboardCtrl);

function dashboardCtrl($scope,$state,$sessionStorage,$location,authService) {
    //ngInject

    authService.checkUser();
    
    $scope.goto = function(){
    	$state.go("prebudget");
    }
}

