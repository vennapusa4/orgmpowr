/*
Budget Factory: All the budget related functionality are written here
Created by: Aamin Khan
Created at: 25/01/2017
*/
(function () {
    'use strict';
    angular.module('hpe').factory('userManagementFactory', function ($http, $rootScope, $localStorage, $sessionStorage, $timeout) {

        return {
            //load User Management Data
            loadUserManagementData: function () {
                var promise =
                      $http({
                          method: 'GET',
                          url: $rootScope.api + 'UserManagement/UserManagement_Read'
                      }).then(function (resp) {
                          return resp.data;
                      });
                return promise;
            },

            //load User Acess Level Data
            loadUserAcessLevelData: function (isAdmin) {
                var promise =
                      $http({
                          method: 'GET',
                          url: $rootScope.api + 'UserManagement/UserManagement_IfUser_IsAdmin?isadmin='+isAdmin
                      }).then(function (resp) {
                          return resp.data;
                      });
                return promise;
            },

            //delete User Management Data
            deleteUserManagementData: function (param) {
                    var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_Delete',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                    return promise;

            },
            //loadUserManagementUserOptionsDetails: function () {
            //    var promise =
            //          $http({
            //              method: 'GET',
            //              url: $rootScope.api + 'UserManagement/UserManagement_Select_User_Details'
            //          }).then(function (resp) {
            //              return resp.data;
            //          });
            //    return promise;
            //},

            //load Select Options Data
            loadSelectOptionssData: function (param) {
                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_Edit_User_firstDetails',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },

            //load Geo Data
            loadGeoData: function (param) {
                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_AccessDPClick_Details',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },

            //edit User Management Data
            editUserManagementData: function (param1) {
               
                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_Create_Update',
                           data: param1
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },

            //load SubRegion Options
            loadSubRegionOptions: function (param) {
                console.log(param);
                  
                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_RegionDPClick_Details',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },

            //load Country Details
            loadCountryDetails: function (param) {
               
                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_GeoDPClick_Details',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },

            //load BU Details for GEO level users
            loadBUForGeoLevelUser: function (param) {

                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_GeoDPClick_Details',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },

            //load BU Details
            loadBuDetails: function (param) {
                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_CountryDPClick_Details',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },
            //loadDsitrictsDetails: function (param) {
            //    var promise =
            //           $http({
            //               method: 'POST',
            //               url: $rootScope.api + 'UserManagement/UserManagement_CountryDPClick_Details',
            //               data: param
            //           }).then(function (resp) {
            //               return resp;
            //           });
            //    return promise;
            //},

            //reset User Password
            resetUserPassword: function (param) {
                var promise =
                       $http({
                           method: 'POST',
                           url: $rootScope.api + 'UserManagement/UserManagement_ResetPassword',
                           data: param
                       }).then(function (resp) {
                           return resp;
                       });
                return promise;
            },

            //load New User Options Details
            loadNewUserOptionsDetails: function () {
                var promise =
                      $http({
                          method: 'GET',
                          url: $rootScope.api + 'UserManagement/UserManagement_New_User_Dropdowns'
                      }).then(function (resp) {
                          return resp.data;
                      });
                return promise;
            }
        }
    })
})();