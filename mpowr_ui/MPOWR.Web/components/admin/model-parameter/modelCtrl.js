// Role controller
// Created By : Nikhil
// Created date: 23 Mar 2017
(function () {
    'use strict';
    angular.module("hpe").controller('modelCtrl', setModelCtrlFn)

    function setModelCtrlFn($scope, $http, $state, Notification, $localStorage, $rootScope) {
        $scope.example14settings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true
        };

        // Initially Fetching Model Parameter data
        $http.get($rootScope.api + 'SetModelParameters/GetRegionsandYears').success(function (response) {
            $scope.regions = response.RegionFinancialYear[0].Regions;
            $scope.years = response.RegionFinancialYear[0].FinancialYear;
            $scope.activeMenu = $scope.regions[0].Region.RegionId;

            $scope.activeDummyMenu = $scope.regions[0]

            

            //$scope.selectedYear = $scope.years[0];
            //$scope.selectedYearForCopy = $scope.selectedYear;
            var fYear = $localStorage.user.FinancialYear;
                angular.forEach($scope.years, function (obj) {
                    if (obj.year.ShortName === fYear) {
                        $scope.selectedYear = obj;
                        $scope.selectedYearForCopy = obj;
                    }
            })

            $scope.setActive(
                $scope.activeDummyMenu,
                $scope.regions[0].Region.RegionId,
                $scope.selectedYear.year.FinancialYearID
            );
        });

        // Fetching model parameter data on selecitng Region and finalitial year
        $scope.setActive = function (menuItem, RegionId, FinancialYearId) {
            $scope.activeMenu = menuItem.Region.RegionId;

            $scope.activeDummyMenu = menuItem;
            $scope.api_url = $rootScope.api + "SetModelParameters/GetExpectedProductivity";

            $scope.RegionId = RegionId;
            $scope.FinancialYearId = FinancialYearId;

            var data = {
                "FinancialYearId": $scope.FinancialYearId,
                "RegionId": $scope.activeMenu
            }

            //console.log(angular.toJson(data));

            $http.post($rootScope.api + '/SetModelParameters/GetExpectedProductivity', angular.toJson(data)).success(function (response) {
                $scope.HighPerformerProductivity = response[0]['HighPerformerProductivity'];
                $scope.MediumPerformerProductivityRatio = response[0]['MediumPerformerProductivityRatio'];
                $scope.LowPerformerProductivityRatio = response[0]['LowPerformerProductivityRatio'];

                //console.log("HighPerformerProductivity: " + $scope.HighPerformerProductivity + " MediumPerformerProductivityRatio: " + $scope.MediumPerformerProductivityRatio + " LowPerformerProductivityRatio: " + $scope.LowPerformerProductivityRatio);
             
            });
        }

        //Saving model paramerter date
        $scope.save = function () {          

            var data = {
                "FinancialYearId": $scope.FinancialYearId,
                "RegionId": $scope.activeMenu, // activeMenu = regionID
                "HighPerformerProductivity": parseFloat($scope.HighPerformerProductivity),
                "MediumPerformerProductivityRatio": parseFloat($scope.MediumPerformerProductivityRatio),
                "LowPerformerProductivityRatio": parseFloat($scope.LowPerformerProductivityRatio),
                "UserID": $localStorage.user.UserID
            }


            $http.post($rootScope.api + '/SetModelParameters/SetModelDetails', angular.toJson(data)).success(function (response) {
                console.log(data);
                Notification.success('Model Parameter Settings Saved Successfully');
            });
        };
        
        $scope.copyYearData = function (RegionId, copy_year) {
            
            
            $scope.copy_year = copy_year;
           
            var data = {
                "FinancialYearId": $scope.copy_year,
                "RegionId": RegionId,
                "HighPerformerProductivity": $scope.HighPerformerProductivity,
                "MediumPerformerProductivityRatio": $scope.MediumPerformerProductivityRatio,
                "LowPerformerProductivityRatio": $scope.LowPerformerProductivityRatio,
                "UserID": $localStorage.user.UserID
            }
            $http.post($rootScope.api + '/SetModelParameters/SetModelDetails', angular.toJson(data)).success(function (response) {
                console.log(data);
                $('.modal').modal('hide');
                Notification.success('Model Parameter Settings Copied Successfully');
                
            });

        };

        


    };

})();


