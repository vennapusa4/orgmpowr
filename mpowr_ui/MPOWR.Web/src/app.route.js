/*
App Route: All the rotes for the app are defined here
Created by: Aamin Khan
Created at: 19/01/2017
*/
(function () {
    'use strict';

    angular.module("hpe").config(config)

    function config($stateProvider, $urlRouterProvider, $localStorageProvider) {
        //ngInject
        $localStorageProvider.setKeyPrefix('hpe-');
        $stateProvider
            .state("login", {
                url: '/login',
                templateUrl: "components/login/login.html",
                controller: 'loginCtrl'
            })
            .state("dashboard", {
                url: '/dashboard',
                templateUrl: "components/dashboard/dashboard.html",
                controller: 'dashboardCtrl'
            })
            .state('budget',{
                url: '/budget',
                abstract: true,
                templateUrl: 'components/budget/layout.html'
            })
            .state('budget.prebudget',{
                url: '/prebudget',
                templateUrl: 'components/budget/prebudget.html',
                controller: 'prebudgetCtrl'
            })
            .state('budget.allocation',{
                url: '/allocation',
                templateUrl: 'components/budget/allocation/allocation.html',
                controller: 'budgetCtrl'
            })
            .state('budget.historical-perfomance',{
                url: '/historical-perfomance',
                templateUrl: 'components/budget/historical/historical-perfomance.html',
                controller: 'HistoricalCtrl'
            })
            .state('budget.model-parameter', {
                url: '/model-parameter',
                templateUrl: 'components/budget/model-parameters/model-parameters.html',
                controller: 'ModelParamCtrl'
            })

        $urlRouterProvider.otherwise("/login");
    };

    
    

})();