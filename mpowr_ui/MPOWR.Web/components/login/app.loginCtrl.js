/*
Login Controller: This controller is responsible for doing login actions
Created by: Aamin Khan
Created at: 19/01/2017
*/

(function () {
    'use strict';
    angular.module("hpe").controller("loginCtrl", loginCtrl);

    function loginCtrl($scope, $state, loginService, $localStorage, $rootScope, HistoricalFactory, adminFactory,Idle) {
        //ngInject
        $scope.loginErr = 'hidden';
        $scope.spin = false;


        $scope.login = function (user) {

             
            //call login service
            $scope.spin = true;
            loginService.login(user).then(function onSuccess(response) {
              //  $rootScope.$broadcast('LoginTrigger');
                // Handle success
                var data = response.data;
                var status = response.status;
                var statusText = response.statusText;
                var headers = response.headers;
                var config = response.config;

                //set Admin Field
                adminFactory.setAdmin(data[0].IsAdmin);
                if (data[0].FinancialYear.slice(5, 7) === '1H') {
                    $scope.previousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 1H';
                    $scope.previoustoPreviousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 2).toString() + ' 1H';
                    $localStorage.preYear = 'FY' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 1H';
                    $localStorage.prePeriod = 'FY' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 2H';
                } else {
                    $scope.previousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 2H';
                    $scope.previoustoPreviousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 2).toString() + ' 2H';
                    $localStorage.preYear = 'FY' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 2H';
                    $localStorage.prePeriod = 'FY' + (parseInt(data[0].FinancialYear.slice(2, 4))).toString() + ' 1H';
                }
                $localStorage.previousYear = $scope.previousYear;
                $localStorage.previoustoPreviousYear = $scope.previoustoPreviousYear;

                if (data[0].FinancialYear.slice(5, 7) === '1H') {
                    $scope.previousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 1H';
                    $scope.previoustoPreviousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 2).toString() + ' 1H';
                    $localStorage.preYear = 'FY' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 1H';
                } else {
                    $scope.previousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 2H';
                    $scope.previoustoPreviousYear = 'FY_' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 2).toString() + ' 2H';
                    $localStorage.preYear = 'FY' + (parseInt(data[0].FinancialYear.slice(2, 4)) - 1).toString() + ' 2H';
                }
                $localStorage.previousYear = $scope.previousYear;
                $localStorage.previoustoPreviousYear = $scope.previoustoPreviousYear;
                

                $localStorage.user = data[0];
                $scope.spin = false;
                HistoricalFactory.getBu();
                Idle.watch();
                //$state.go("dashboard");
                $state.go("prebudget");
                $localStorage.showSpartlines = false;
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

            $scope.setYearValues = function () {

            }

        }

        if (typeof $localStorage.user !== 'undefined') {
            $state.go("prebudget");
        }

    }
})();