
(function () {
    'use strict';
    angular.module("hpe").controller("guardrailCtrl", guardFn)

    function guardFn($scope, $http, $state, Notification, $localStorage, $rootScope) {

        $scope.rev1 = true;
        $scope.rev2 = true;
        $scope.rev3 = true;
        $scope.rev4 = true;
       

        //Initially loading Reginal and Finantial Year Data

        $http.get($rootScope.api + 'SetModelParameters/GetRegionsandYears').success(function (response) {
            $scope.regions = response.RegionFinancialYear[0].Regions;
            $scope.years = response.RegionFinancialYear[0].FinancialYear;
            $scope.activeMenu = $scope.regions[0].Region.RegionId;

            $scope.activeDummyMenu = $scope.regions[0]

            var fYear = JSON.parse(localStorage['hpe-user']).FinancialYear;
            angular.forEach($scope.years, function (obj) {
                if (obj.year.ShortName === fYear) {
                    $scope.selectedYear = obj;
                    $scope.selectedYearForCopy = $scope.selectedYear;
                }
            })

            
            getData($scope.activeMenu, $scope.selectedYear.year.FinancialYearID)
        });



        //Getting Data on change of Region and finantial Year

        $scope.setActive = function (menuItem, RegionId, FinancialYearId) {

            $scope.activeMenu = menuItem.Region.RegionId;

            $scope.activeDummyMenu = menuItem;


            getData($scope.activeMenu, FinancialYearId)

            $scope.RegionId = RegionId;
            $scope.FinancialYearId = FinancialYearId;

            var data = {
                "FinancialYearId": $scope.FinancialYearId,
                "RegionId": $scope.activeMenu
            }


        }

        $http.get($rootScope.api + '/RoleFeatureMgnt/GetRoles').success(function (response) {

            $scope.reviewer = response;
            $scope.reviewer1 = response;
            $scope.reviewer2 = response;
            $scope.reviewer3 = response;
            $scope.reviewer4 = response;


        });


        var prevSelectedReviewer1 = null;
        var prevSelectedReviewer2 = null;
        var prevSelectedReviewer3 = null;
        var prevSelectedReviewer4 = null;

        

        $scope.AllowOverAllocation = 'Y';

        // Fetching GuardRail Data From API

        function getData(regionId, financialYearID) {

            $http.get($rootScope.api + 'GuardRail/GetGuardRailDetail?regionID=' + regionId + '&financialYearID=' + financialYearID).success(function (response) {
                $scope.guardrailData = response[0];
                console.log(response[0]);
                $scope.ProgramCarveOutComplaint = $scope.guardrailData.ProgramCarveOutComplaintValue;
                $scope.ProgramCarveOutNonComplaint = $scope.guardrailData.ProgramCarveOutNonComplaintValue;
               
                $scope.BorderlineMin = $scope.guardrailData.ActualMDFAllocationComplaintValue;
                $scope.BorderlineMax = $scope.guardrailData.ActualMDFAllocationNonComplaintValue;
              
                $scope.BorderlineMin1 = $scope.guardrailData.MDFSelloutIndexComplaintValue;
                $scope.BorderlineMax1 = $scope.guardrailData.MDFSelloutIndexNonComplaintValue;
                
                $scope.OverAllocationComplaint = $scope.guardrailData.OverAllocationComplaintValue;
                $scope.OverAllocationNonComplaint = $scope.guardrailData.OverAllocationNonComplaintValue;
               
                $scope.UnderAllocationComplaint = $scope.guardrailData.UnderAllocationComplaintValue;
                $scope.UnderAllocationNonComplaint = $scope.guardrailData.UnderAllocationNonComplaintValue;
               
                $scope.GuardRailConfigID = $scope.guardrailData.GuardRailConfigID;
                $scope.GuardRailFYConfigID = $scope.guardrailData.GuardRailFYConfigID;
                $scope.AllowOverAllocation = $scope.guardrailData.AllowOverAllocation == 'Y' ? true : false;
                $scope.SetGuardrailValue($scope.AllowOverAllocation);

             

                $scope.selectedReviewer1 = _.find($scope.reviewer, function (o) {
                    return o.RoleID == $scope.guardrailData.ReviewerRoleID1;
                })

                $scope.selectedReviewer2 = _.find($scope.reviewer, function (o) {
                    return o.RoleID == $scope.guardrailData.ReviewerRoleID2;
                })
                console.log($scope.guardrailData.ReviewerRoleID2);
                $scope.selectedReviewer3 = _.find($scope.reviewer, function (o) {
                    return o.RoleID == $scope.guardrailData.ReviewerRoleID3;
                })

                $scope.selectedReviewer4 = _.find($scope.reviewer, function (o) {
                    return o.RoleID == $scope.guardrailData.ReviewerRoleID4;
                })
                

                $scope.reqReview($scope.selectedReviewer1, $scope.selectedReviewer2, $scope.selectedReviewer3, $scope.selectedReviewer4);

            });

        }


        //Code to Select Unique Reviews

        $scope.reqReview = function (selectedReviewer1, selectedReviewer2, selectedReviewer3, selectedReviewer4) {
            if (typeof selectedReviewer1 != 'undefined' || typeof selectedReviewer2 != 'undefined' || typeof selectedReviewer3 != 'undefined' || typeof selectedReviewer4 != 'undefined') {
                $scope.rev1 = false;
                $scope.rev2 = false;
                $scope.rev3 = false;
                $scope.rev4 = false;
            } else {
                $scope.rev1 = true;
                $scope.rev2 = true;
                $scope.rev3 = true;
                $scope.rev4 = true;
            }
        }
       

        $scope.selectReviewer1 = function (sr) { // 2,3,4
            checkCondition();
            $scope.rev2 = false;
            $scope.rev3 = false;
            $scope.rev4 = false;
            
            
        }

        
        $scope.selectReviewer2 = function (sr) { //1,3,4
            checkCondition();
            $scope.rev1 = false;
            $scope.rev3 = false;
            $scope.rev4 = false;
            
        }

        $scope.selectReviewer3 = function (sr) { //1,2,4
            checkCondition();
            $scope.rev1 = false;
            $scope.rev2 = false;
            $scope.rev4 = false;
            
        }

        $scope.selectReviewer4 = function (sr) { //1,2,3
            checkCondition();
            $scope.rev1 = false;
            $scope.rev2 = false;
            $scope.rev3 = false;
            
        }


        // Reviews Validation

        $scope.rev_msg = 'Please Enter Unique Review';
        $scope.condition = false;
        function checkCondition() {
            $scope.condition = ($scope.selectedReviewer1 == $scope.selectedReviewer2 && typeof $scope.selectedReviewer1 != 'undefined')
                || ($scope.selectedReviewer1 == $scope.selectedReviewer3 && typeof $scope.selectedReviewer3 != 'undefined')
                || ($scope.selectedReviewer1 == $scope.selectedReviewer4 && typeof $scope.selectedReviewer1 != 'undefined')
            || ($scope.selectedReviewer2 == $scope.selectedReviewer3 && typeof $scope.selectedReviewer2 != 'undefined')
            || ($scope.selectedReviewer2 == $scope.selectedReviewer4 && typeof $scope.selectedReviewer2 != 'undefined')
            || ($scope.selectedReviewer4 == $scope.selectedReviewer3 && typeof $scope.selectedReviewer4 != 'undefined')

            
        }
       

        // Code to set OverAllocation Values Based on AllowOverAllocation checkbox
        $scope.SetGuardrailValue = function (AllowOverAllocation) {
            if (AllowOverAllocation == false) {

                $scope.OverAllocationComplaint = 0;
                $scope.OverAllocationNonComplaint = 0;
             
            } else if (AllowOverAllocation == true) {

                $scope.OverAllocationComplaint = $scope.guardrailData.OverAllocationComplaintValue;
                $scope.OverAllocationNonComplaint = $scope.guardrailData.OverAllocationNonComplaintValue;
              
            }
        }


        $scope.SetOverAllocation = function (BorderlineMin1, BorderlineMax1) {
            if (BorderlineMin1 === BorderlineMax1) {
                $scope.OverAllocationMin = null;
                $scope.OverAllocationMax = null;
            } else {
                $scope.OverAllocationMin = BorderlineMin1;
                $scope.OverAllocationMax = BorderlineMax1;
            }
        }


        // Code to save tha Guardrail data
        $scope.save = function (RegionId) {

            if ($scope.AllowOverAllocation == true) {
                $scope.AllowOverAllocation = 'Y'
            }
            if ($scope.AllowOverAllocation == false) {
                $scope.AllowOverAllocation = 'F'
            }
            if (typeof $scope.selectedReviewer1 == 'undefined') {
                $scope.selectedReviewer1 = {
                    RoleID: 0
                }
            }
            if (typeof $scope.selectedReviewer2 == 'undefined') {
                $scope.selectedReviewer2 = {
                    RoleID: 0
                }
            }
            if (typeof $scope.selectedReviewer3 == 'undefined') {
                $scope.selectedReviewer3 = {
                    RoleID: 0
                }
            }
            if (typeof $scope.selectedReviewer4 == 'undefined') {
                $scope.selectedReviewer4 = {
                    RoleID: 0
                }
            }
           

                var data = {

                    "RegionID": RegionId,
                    "FinancialYearID": $scope.selectedYear.year.FinancialYearID,
                    "user": $localStorage.user.UserID,
                    "GuardRailDetails": [
                      {
                          "GuardRailConfigID": $scope.GuardRailConfigID,
                          "GuardRailFYConfigID": $scope.GuardRailFYConfigID,
                          "ProgramCarveOutComplaintValue": $scope.ProgramCarveOutComplaint,
                          "ProgramCarveOutNonComplaintValue": $scope.ProgramCarveOutNonComplaint,
                          "ActualMDFAllocationComplaintValue": $scope.BorderlineMin,
                          "ActualMDFAllocationNonComplaintValue": $scope.BorderlineMax,
                          "MDFSelloutIndexComplaintValue": $scope.BorderlineMin1,
                          "MDFSelloutIndexNonComplaintValue": $scope.BorderlineMax1,
                          "OverAllocationComplaintValue": $scope.OverAllocationComplaint,
                          "OverAllocationNonComplaintValue": $scope.OverAllocationNonComplaint,
                          "UnderAllocationComplaintValue": $scope.UnderAllocationComplaint,
                          "UnderAllocationNonComplaintValue": $scope.UnderAllocationNonComplaint,
                          "AllowOverAllocation": $scope.AllowOverAllocation,
                          "ReviewerRoleID1": $scope.selectedReviewer1.RoleID,
                          "ReviewerRoleID2": $scope.selectedReviewer2.RoleID,
                          "ReviewerRoleID3": $scope.selectedReviewer3.RoleID,
                          "ReviewerRoleID4": $scope.selectedReviewer4.RoleID
                      }
                    ]
                }

                $http.post($rootScope.api + '/GuardRail/AddUpdateGuardRail', angular.toJson(data)).success(function (response) {
                    console.log(data);
                    Notification.success('Guardrail Settings Saved Successfully');
                   
                });
                if ($scope.AllowOverAllocation == 'Y') {
                    $scope.AllowOverAllocation = true
                }
                if ($scope.AllowOverAllocation == 'F') {
                    $scope.AllowOverAllocation = false

                }
            }
            
        
        // Code to copy the Guardrail data
        $scope.copyYearData = function (RegionId, copy_year) {


            $scope.copy_year = copy_year;

            if ($scope.AllowOverAllocation == true) {
                $scope.AllowOverAllocation = 'Y'
            }
            if ($scope.AllowOverAllocation == false) {
                $scope.AllowOverAllocation = 'F'
            }
            if (typeof $scope.selectedReviewer1 == 'undefined') {
                $scope.selectedReviewer1 = {
                    RoleID: 0
                }
            }
            if (typeof $scope.selectedReviewer2 == 'undefined') {
                $scope.selectedReviewer2 = {
                    RoleID: 0
                }
            }
            if (typeof $scope.selectedReviewer3 == 'undefined') {
                $scope.selectedReviewer3 = {
                    RoleID: 0
                }
            }
            if (typeof $scope.selectedReviewer4 == 'undefined') {
                $scope.selectedReviewer4 = {
                    RoleID: 0
                }
            }
           
                var data = {
                    "RegionID": RegionId,
                    "FinancialYearID": $scope.copy_year,
                    "user": $localStorage.user.UserID,
                    "GuardRailDetails": [
                      {
                          "GuardRailConfigID": $scope.GuardRailConfigID,
                          "GuardRailFYConfigID": $scope.GuardRailFYConfigID,
                          "ProgramCarveOutComplaintValue": $scope.ProgramCarveOutComplaint,
                          "ProgramCarveOutNonComplaintValue": $scope.ProgramCarveOutNonComplaint,
                          "ActualMDFAllocationComplaintValue": $scope.BorderlineMin,
                          "ActualMDFAllocationNonComplaintValue": $scope.BorderlineMax,
                          "MDFSelloutIndexComplaintValue": $scope.BorderlineMin1,
                          "MDFSelloutIndexNonComplaintValue": $scope.BorderlineMax1,
                          "OverAllocationComplaintValue": $scope.OverAllocationComplaint,
                          "OverAllocationNonComplaintValue": $scope.OverAllocationNonComplaint,
                          "UnderAllocationComplaintValue": $scope.UnderAllocationComplaint,
                          "UnderAllocationNonComplaintValue": $scope.UnderAllocationNonComplaint,
                          "AllowOverAllocation": $scope.AllowOverAllocation,
                          "ReviewerRoleID1": $scope.selectedReviewer1.RoleID,
                          "ReviewerRoleID2": $scope.selectedReviewer2.RoleID,
                          "ReviewerRoleID3": $scope.selectedReviewer3.RoleID,
                          "ReviewerRoleID4": $scope.selectedReviewer4.RoleID
                      }
                    ]
                }
                console.log(data);
                $http.post($rootScope.api + '/GuardRail/AddUpdateGuardRail', angular.toJson(data)).success(function (response) {

                    Notification.success('Guardrial Settings Copied Successfully');
                    $('.modal').modal('hide');
                   
                });
                if ($scope.AllowOverAllocation == 'Y') {
                    $scope.AllowOverAllocation = true
                }
                if ($scope.AllowOverAllocation == 'F') {
                    $scope.AllowOverAllocation = false
                }
        }

    }


})();