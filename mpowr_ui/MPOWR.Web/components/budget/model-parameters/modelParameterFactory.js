/*
modelParameter Factory: All the budget related functionality are written here
Created by: Rohit Kumar
Created at: 08/02/2017
*/
(function () {
    'use strict';
    angular.module('hpe').factory('modelParameterFactory', modelParameterFactoryFn);

    function modelParameterFactoryFn($http, $rootScope, $localStorage, $sessionStorage, $timeout) {

        return {
            getData: function (countryId, partnerTypeId, DistrictID, FinancialYearID, VersionID) {
                DistrictID = DistrictID === 0 ? 18 : DistrictID;
                var promise =
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'ModelParameter/GetModelParameterDefault?VersionID=' + VersionID + "&UserID=" + $localStorage.user.UserID
                }).then(function (resp) {
                    $timeout(function () {
                        var currentBus = resp.data[1];
                        for (var i = 0; i < currentBus.length; i++) {
                            if (($localStorage.user.BUs.indexOf(currentBus[i].BusinessUnitID) > -1)) {
                            }
                            else {
                                $(".businessUnit_" + currentBus[i].BusinessUnitID + " input").attr("disabled", "disabled")
                            }
                        }
                    }, 500)
                    return resp;
                });
                return promise;
            },
            setDefaultModalAttributeValues: function ($scope) {

                $scope.total = true;
                $scope.dcn = false;
                $scope.compute = false;
                $scope.aruba = false;
                $scope.storage = false;
                $scope.service = false;

                //load Set Boundaries for MDF Change from Last Period values
                if ($scope.data[0][0]) {

                    $scope.data[0][0].Max_Sellout_HighDecline_Min_MDF = $scope.data[0][0].Max_Sellout_HighDecline_Min_MDF + '%';
                    $scope.data[0][0].Max_Sellout_ModerateDecline_Min_MDF = $scope.data[0][0].Max_Sellout_ModerateDecline_Min_MDF + '%';
                    $scope.data[0][0].Max_Sellout_Steady_Max_MDF = $scope.data[0][0].Max_Sellout_Steady_Max_MDF + '%';
                    $scope.data[0][0].Max_Sellout_Steady_Min_MDF = $scope.data[0][0].Max_Sellout_Steady_Min_MDF + '%';
                    $scope.data[0][0].Max_Sellout_ModerateGrowth_Max_MDF = $scope.data[0][0].Max_Sellout_ModerateGrowth_Max_MDF + '%';
                    $scope.data[0][0].Max_Sellout_ModerateGrowth_Min_MDF = $scope.data[0][0].Max_Sellout_ModerateGrowth_Min_MDF + '%';
                    $scope.data[0][0].Max_Sellout_HighGrowth_Max = $scope.data[0][0].Max_Sellout_HighGrowth_Max + '%';
                    $scope.data[0][0].Max_Sellout_HighGrowth_Min = $scope.data[0][0].Max_Sellout_HighGrowth_Min + '%';


                    //load Set Boundaries for Advanced inputs values
                    $scope.data[0][0].Target_accomplish_HighPrecision_Score = $scope.data[0][0].Target_accomplish_HighPrecision_Score + '%';
                    $scope.data[0][0].Target_accomplish_MediumPrecision_Score = $scope.data[0][0].Target_accomplish_MediumPrecision_Score + '%';
                    $scope.data[0][0].Max_Target_Accomplish_percentage = $scope.data[0][0].Max_Target_Accomplish_percentage + '%';
                    $scope.data[0][0].Min_Target_Accomplish_percentage = $scope.data[0][0].Min_Target_Accomplish_percentage + '%';
                    $scope.data[0][0].Weights_applied_t_1H = $scope.data[0][0].Weights_applied_t_1H + '%';
                    $scope.data[0][0].Weights_applied_t_2H = $scope.data[0][0].Weights_applied_t_2H + '%';
                    $scope.data[0][0].Weights_applied_t_3H = $scope.data[0][0].Weights_applied_t_3H + '%';
                    $scope.data[0][0].Min_Target_Productivity = '$' + $scope.data[0][0].Min_Target_Productivity;

                    $scope.data[0][0].JPB_Max = $scope.data[0][0].JPB_Max + '%';
                    $scope.data[0][0].JPB_Min = $scope.data[0][0].JPB_Min + '%';
                    $scope.data[0][0].Prediction_High_Max = $scope.data[0][0].Prediction_High_Max + '%';
                    $scope.data[0][0].Prediction_High_Min = $scope.data[0][0].Prediction_High_Min + '%';
                    $scope.data[0][0].Prediction_Low_Max = $scope.data[0][0].Prediction_Low_Max + '%';
                    $scope.data[0][0].Prediction_Low_Min = $scope.data[0][0].Prediction_Low_Min + '%';

                    $scope.data[0][0].Last_Quarter_Sellout_Scale_Factor = $scope.data[0][0].Last_Quarter_Sellout_Scale_Factor + '%';

                }
            },
            getBusinessUnits: function () {
                var promise =
                  $http({
                      method: 'GET',
                      url: $rootScope.api + 'HistoricPerformance/GetBusinessUnitByVersion?VersionID=' + $localStorage.selectedversion.VersionID
                  }).then(function (resp) {
                      return resp;
                  });
                return promise;
            },
            getBUAccess: function () {
                var promise =
                  $http({
                      method: 'GET',
                      url: $rootScope.api + 'UserManagement/UserManagement_IsBUAccess?userid=' + $localStorage.user.UserID
                  }).then(function (resp) {
                      return resp;
                  });
                return promise;
            },
            updateUserModelValues: function (values) {
                values.Modelparameters.UserID = $localStorage.user.UserID;
                var promise =
               $http({
                   method: 'POST',
                   url: $rootScope.api + 'ModelParameter/AddModelParameters/',
                   data: values
               }).then(function (resp) {
                   return resp;
               });
                return promise;
            },
            getGraphData: function (buId, VersionID) {
                var promise =
               $http({
                   method: 'GET',
                   url: $rootScope.api + 'ModelParameter/GetGraphdetailsByBusinessUnit?BusinessUnitID=' + buId + '&VersionID=' + VersionID
               }).then(function (resp) {

                   return resp;
               });
                return promise;
            },
            getYOYGraphData: function (param) {
                var promise =
               $http({
                   method: 'POST',
                   url: $rootScope.api + 'ModelParameter/YOYGraphData',
                   data: param
               }).then(function (resp) {

                   return resp;
               });
                return promise;
            },
            getResetData: function (VersionID) {
                var promise =
               $http({
                   method: 'GET',
                   url: $rootScope.api + 'ModelParameter/ResetModelparam?VersionID=' + VersionID +'&UserID=' + $localStorage.user.UserID  
               }).then(function (resp) {

                   return resp;
               });
                return promise;
            },
            update: function ($http, $scope, $rootScope, $localStorage) {

                $http({
                    method: 'POST',
                    url: $rootScope.api + 'LastUpdated/inseretLastUpdated?userid=' + $localStorage.user.UserID + '&partnerid=' + $localStorage.PartnerTypeID

                }).then(function (resp) {

                });
            },
            getWarningdata: function (VersionID) {
               
                var promise =
               $http({
                   method: 'GET',
                   url: $rootScope.api + 'ModelParameter/DefaultCheckData?VersionID=' + VersionID + "&UserID=" + $localStorage.user.UserID
               }).then(function (resp) {

                   return resp;
               });
                return promise;
            }

        }
    }

})();