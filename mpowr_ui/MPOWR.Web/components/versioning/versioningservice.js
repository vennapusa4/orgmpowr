    (function () {
        'use strict';
        angular.module('hpe').service('versioningservice', versioningserviceFn);

        function versioningserviceFn($localStorage, $state, $rootScope, $http, $sessionStorage) {

        //making webapi call to get version data
            this.getData = function (getdata) {
                return $http({
                    method: 'POST',
                    url: $rootScope.api + 'Version/GetVersionData',
                    data: getdata
                })
            }

            // Adding Record  
            this.createdata = function (objdata) {

                return $http({
                    method: "POST",
                    url: $rootScope.api + 'Version/CreateVersion',
                    data: objdata

                });
            }

            // Deleting records  
            this.deleteData = function (Versionid) {             
                var versionData = { VersionID: Versionid };
                return $http({
                    method: 'POST',
                    url: $rootScope.api + 'Version/DeleteVersion',
                    data: versionData
                }).error(function (error, status) {
                    console.log(error);
                });
            }

            // Edit Plan  
            this.editPlan = function (editPlanData) {
                return $http({
                    method: 'POST',
                    url: $rootScope.api + 'Version/UpdateVersionName',
                    data: editPlanData
                }).error(function (error, status) {
                    console.log(error);
                });
            }


            //copying data from source to destination

            this.copydata = function (copyobj) {
               
                return $http({
                    method: 'POST',
                    url: $rootScope.api + 'Version/CopyVersion',
                    data: copyobj
                    
                }).error(function (error, status) {
                    console.log(error);
                });
            }, 
            this.setFinalVersion = function (obj) {
                var data = {};
                data = {
                    FinancialYearID: obj.FinancialYearID,
                    PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
                    AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
                    VersionID: obj.VersionID,
                    CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
                    UserID: $localStorage.user.UserID
                };
                return $http({
                    method: 'POST',
                    url: $rootScope.api + 'Version/UpdateIsFinal',
                    data: data
                    
                }).error(function (error, status) {
                    console.log(error);
                });
            }
        }
    })(); 