/// <reference path="D:\Mpower\MPOWR\MPOWR-Enhancement-GEO\MPOWR.Web\MPOWR.Web\components/Import/importProd.html" />
/// <reference path="D:\TFS_MPOWR\MPOWR\MPOWR-1.1\MPOWR.Web\MPOWR.Web\maintenance.html" />
/// <reference path="D:\TFS_MPOWR\MPOWR\MPOWR-1.1\MPOWR.Web\MPOWR.Web\maintenance.html" />
/*
App Route: All the rotes for the app are defined here
Created by: Aamin Khan
Created at: 19/01/2017
*/
(function () {
    'use strict';

    angular.module("hpe").config(config)

    function config($stateProvider, $urlRouterProvider, $localStorageProvider, $httpProvider, cfpLoadingBarProvider,IdleProvider,KeepaliveProvider) {
        //ngInject


        $localStorageProvider.setKeyPrefix('hpe-');
        cfpLoadingBarProvider.includeSpinner = false;


        IdleProvider.idle(1800);
        IdleProvider.timeout(5);
        KeepaliveProvider.interval(10);



        $httpProvider.interceptors.push('httpRequestInterceptor');
        var onlyLoggedIn = function ($q, adminFactory) {
            var deferred = $q.defer();
            if (adminFactory.IsAdmin()) {
                deferred.resolve();
            } else {
                $state.go('prebudget');
            }
            return deferred.promise;
        };
        
        
        $httpProvider.interceptors.push(function($templateCache) {
          return {
            request : function(request) {
                  if($templateCache.get(request.url) === undefined) { // cache miss
                      if(request.url.endsWith(".html")) {
                        request.url = request.url + '?v=' + version;   
                      }
                  }
                  return request;
              }
          }
        })
        
        
        $stateProvider
            //.state("login", {
            //    url: '/login',
            //    templateUrl: "maintenance.html",
            //    controller: 'loginCtrl'
            //})
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

            .state("feedback", {
                url: '/feedback',
                templateUrl: "Feedback.aspx",


            })

            .state('prebudget', {
                url: '/prebudget',
                templateUrl: 'components/budget/prebudget.html',
                controller: 'prebudgetCtrl'
            })

            .state('budget', {
                url: '/budget',
                abstract: true,
                templateUrl: 'components/budget/layout.html'
            })

            .state('budget.allocation', {
                url: '/allocation',
                templateUrl: 'components/budget/allocation/allocation.html',
                controller: 'budgetCtrl'
            })
            .state('budget.historical-perfomance', {
                url: '/historical-perfomance',
                templateUrl: 'components/budget/historical/historical-perfomance.html',
                controller: 'HistoricalCtrl'
            })
            .state('budget.model-parameter', {
                url: '/model-parameter',
                templateUrl: 'components/budget/model-parameters/model-parameters.html',
                controller: 'ModelParameterCtrl'
            })
            .state('budget.partner-budget', {
                url: '/partner-budget',
                templateUrl: 'components/budget/partner-budget/partner-budget.html',
                controller: 'partnerCreditCtrl'
            })
            .state('budget.partner-budget-expand', {
                url: '/partner-budget-expand/:currentBu',
                templateUrl: 'components/budget/partner-budget/full-screen.html',
                controller: 'partnerCreditCtrl'
            })
            .state('budget.partner-budget-round2', {
                url: '/partner-budget-round2',
                templateUrl: 'components/budget/partner-budget/partner-budget-round2.html',
                controller: 'partnerCreditRound2Ctrl'
            })
            .state('budget.final-summary', {
                url: '/final-summary',
                templateUrl: 'components/budget/final-summary/final-summary.html',
                controller: 'finalSummaryCtrl'
            })
            .state('budget.document', {
                url: '/document',
                templateUrl: 'components/budget/doc/document.html',
                controller: 'documentCtrl'
            })
             .state('budget.import', {
               url: '/import',
               templateUrl: 'components/import/importProd.html',
               controller: 'partnerCreditCtrl'
             })

            .state('admin', {
                url: '/admin',
                abstract: true,
                templateUrl: 'components/admin/admin-layout.html',
                resolve: { loggedIn: onlyLoggedIn }
            })
            .state('admin.user-management', {
                url: '/user-management',
                templateUrl: 'components/admin/user-management/user-management.html',
                controller: 'userManagementCtrl'
            })
            .state('admin.role-management', {
                url: '/role-management',
                templateUrl: 'components/admin/role-management/role-management.html',
                controller: 'roleCtrl'
            })
             .state('admin.Glossary-management', {
                 url: '/Glossary-management',
                 templateUrl: 'components/admin/Glossary-management/Glossary-management.html',
                 controller: 'GlossaryManagementCtrl'
             })
             .state('admin.bu-management', {
                 url: '/geo-management',
                 templateUrl: 'components/admin/geo-management/geo-management.html',
                 controller: 'geoManagementCtrl'
             })
             .state('admin.geo-management', {
                 url: '/bu-management',
                 templateUrl: 'components/admin/bu-management/bu-management.html',
                 controller: 'buManagementCtrl'
             })
            .state('admin.guardrial-setting', {
                url: '/guardrial-setting',
                templateUrl: 'components/admin/guardrial-setting/guardrial-setting.html',
                controller: 'guardrailCtrl'
            })
            .state('admin.model-parameter', {
                url: '/model-parameter',
                templateUrl: 'components/admin/model-parameter/model-parameter.html',
                controller: 'modelCtrl'
            })

            .state('admin.set-milestones', {
                url: '/set-milestones',
                templateUrl: 'components/admin/set-milestones/set-milestones.html',
                controller: 'setMilestoneCtrl'
            })
            .state('admin.expections-review', {
                url: '/expections-review',
                templateUrl: 'components/admin/expections-review/expections-review.html'
            })
           .state('budget.versioning', {
               url: '/versioning',
               templateUrl: 'components/versioning/versioning.html',
               controller: 'versioningctrl'

           })
           

        $urlRouterProvider.otherwise("/login");
    };




})();