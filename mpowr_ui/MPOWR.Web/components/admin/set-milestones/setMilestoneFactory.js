/*
    Name:       Set Milestone Factory
    Create at:  04 Apr 2017
    Created by: Aamin Khan
*/

(function () {
    'use strict';
    angular.module("hpe").factory("setMilestoneFactory", setMilestoneFactory);

    function setMilestoneFactory($rootScope, $http, $localStorage, $timeout) {
        //ngInject
        return {
            getMilestone: function (financialYearId, regionId,$scope) {
                $http.get($rootScope.api + 'SetMileStone/MileStoneDetails?FinancialYearId=' + financialYearId + "&RegionId=" + regionId).success(function (response) {
                    console.log(response)

                    $scope.milestones = response
                    $timeout(function () {
                        $(".datepicker").datepicker({
                            todayHighlight: true,
                            autoclose: true,
                            dateFormat: 'M dd, yy'
                        })

                        $(".fa-calendar").click(function () {
                            
                            $(this).parent().prev().trigger("click");
                        })
                    },2000)
                });
            },
            getRoles: function ($scope) {
                $http.get($rootScope.api + 'RoleFeatureMgnt/GetRoles').success(function (response) {
                    $scope.roles = response
                    
                    
                });
            }

        }

    }
})();


