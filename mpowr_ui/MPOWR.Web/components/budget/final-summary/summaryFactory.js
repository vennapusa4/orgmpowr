/*
Final Summary Service
Created by: Avinash Kumar
Created at: 20/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").factory("summaryFactory", summaryFunction);

    function summaryFunction($http, $q, $rootScope) {
        //ngInject

        return {
            getData: function (data) {
                var deft = $q.defer();
                $http({ method: 'POST',url: $rootScope.api + 'FinalSummary/GetFinalSummayData', data: data}).then(function (result) {
                    deft.resolve(JSON.parse(result.data));
                });
                return deft.promise;
            },
            getGraphData: function (data) {
                var promise =
                $http({
                    method: 'POST',
                    url: $rootScope.api + 'FinalSummary/FinalSummaryGraphData',
                    data: data
                }).then(function (resp) {
                    return resp;
                });
                return promise;
            }
        }
    }
})();
