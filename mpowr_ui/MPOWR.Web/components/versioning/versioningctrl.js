(function () {
    'use strict';
    angular.module("hpe").controller("versioningctrl", versioningctrlFn);

    function versioningctrlFn($scope, $rootScope, $sessionStorage, Notification,roleMappingService, $timeout, $localStorage, versioningservice, $state, $http, UtilityService) {


        var Delay = 4000;
        $scope.versioning = $sessionStorage.resellerPartner;
        $scope.allFinancialsYears = false;
        var data = $sessionStorage.currentVersion;
        $scope.currentYear = $localStorage.user.FinancialYear;
        
        if ($localStorage.user.FinancialYear.slice(5, 7) === '1H') {
            
            $scope.futurePeriod = "FY" + (parseInt($localStorage.user.FinancialYear.slice(2, 4))).toString() + ' 2H';
        } else {
            $scope.futurePeriod = "FY" + (parseInt($localStorage.user.FinancialYear.slice(2, 4)) + 1).toString() + ' 1H';
           
        }
        
        $scope.role = roleMappingService.apply();
        $scope.NOACCESS = $localStorage.NOACCESS;
        console.log("role ", $scope.role);
        $scope.$sessionstorage = $sessionStorage;
        $scope.$localstorage = $localStorage;
        var countryIDs ;
        angular.forEach($sessionStorage.resellerPartner.obj.countries, function (data) {
            countryIDs += data.CountryID;
        })
      
        var getdata = {
            PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
            MembershipGroupID: $sessionStorage.resellerPartner.membership.MembershipGroupID,
            GeoID: $sessionStorage.resellerPartner.obj.geo.GeoID,
            CountryID: $sessionStorage.resellerPartner.obj.countryID,
            DistrictID: $sessionStorage.resellerPartner.district.DistrictID,
            CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
            AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
            IsAllFinancialYear: $scope.allFinancialsYears,
            FinancialYearID: $localStorage.user.CurrentFinancialYearID
        }
        
       //get  version data
        $scope.versioningservice = function () {
            versioningservice.getData(getdata)
            .then(function (resp) {
                $scope.versioningData = resp.data;
                roleMappingService.getCheckIsActiveFlagByVersion(getdata).then(function onSuccess(response) {
                    $scope.NOACCESS = $localStorage.NOACCESS;
                    if ($scope.NOACCESS == true && $rootScope.previousPlan == false) {
                        Notification.error({ message: 'Plan is disabled due to Geo/Country/District/Business Unit has been made In-Active. Please Create a New Plan.', delay: Delay });
                    }
                    else {
                        $localStorage.NOACCESS = false;
                    }
                })
            },
            function onError(response) {

            });
    
        }
        $scope.versioningservice();
      //  versioningservice.getData(getdata);
        // Adding New  record  
        $scope.createdata = function () {
            var objdata = {
                PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
                CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
                AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
                FinancialYearID: $scope.drpdpwnvalue,
                VersionName: $scope.planname,
                UserID: $localStorage.user.UserID,
                MembershipGroupID: $sessionStorage.resellerPartner.membership.MembershipGroupID
            }

            versioningservice.createdata(objdata).success(function (resp) {
                var version = [];
                if ($sessionStorage.currentVersion != undefined && $sessionStorage.currentVersion != null && $sessionStorage.currentVersion.Version != undefined && $sessionStorage.currentVersion.Version != null && $sessionStorage.currentVersion.Version.length > 0)
                {
                    version = $sessionStorage.currentVersion.Version;
                }
                else {
                    version.push({
                        VersionID: resp.VersionID,
                        VersionNo: resp.VersionNo,
                        VersionName: resp.VersionName,
                        IsFinal: resp.IsFinal,
                        IsAllFinancialYear: resp.IsAllFinancialYear,
                        CreatedBy: resp.CreatedBy
                    });
                }
                $sessionStorage.currentVersion = resp;
                
                
                //var version  = {
                    
                //}
                $sessionStorage.currentVersion.Version = version;
                $rootScope.versionData = resp;
                $localStorage.selectedversion = resp;
                $scope.versioningservice();               
                $localStorage.user.FinancialYearID = resp.FinancialYearID
                $state.go('budget.allocation');
                Notification.success({
                    message: "Version created successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
            }, function () {
                Notification.error({
                    message: "Error successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
            });
            
        }

      //EditPlan
        $scope.editUserInfo = function (x) {
            $scope.editPlanData = angular.copy(x);
        }
        $scope.editdata = function () {
            $scope.editPlanData.UserID = $scope.$localstorage.user.UserID;
            versioningservice.editPlan($scope.editPlanData).success(function (res) {
                $scope.versioningservice();
                Notification.success({
                    message: "Plan name changed successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
            }, function () {
                Notification.error({
                    message: "Error successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
            })
        }
        
        //// Deleting record.  
        //$scope.deleteData = function (stu) {
        //    var retval = versioningservice.deleteData(stu.Id).success(function () {

        //        alert('data has been deleted successfully.');
        //    }).error(function () {
        //        alert('Oops! something went wrong.');
        //    });
        //}

        $scope.isfinalVersion = function (x) {
            $scope.finalVersionData = x;
        }

        $scope.finalVersion = function () {
            $scope.finalVersionData.UserID = $scope.$localstorage.user.UserID;
            versioningservice.setFinalVersion($scope.finalVersionData).success(function (res) {
                Notification.success({
                    message: "Final Version Updated successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
                $scope.versioningservice();
            });
            
        }

        $scope.getAllVersioingData = function () {
            getdata.IsAllFinancialYear = $scope.allFinancialsYears;
            $scope.versioningservice();
        }

        // copying record.  
        $scope.copydata = function () {
            
            var copyobj = {
                OldFinancialyearID: data.FinancialyearID,
                OldVersionNo: $scope.a.VersionNo,
                NewFinancialyearID: $scope.drpdpwnvalue1, // dest fy id
                PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
                CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
                AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
                UserID: $localStorage.user.UserID,
                VersionName: $scope.DestPlanvalue, // dest plan name
                VersionID: $scope.a.VersionID, // version id
                MembershipGroupID: $sessionStorage.resellerPartner.membership.MembershipGroupID
                
            }
          //  $('#mdf_loader').css('display', 'block');
            versioningservice.copydata(copyobj).success(function (resp) {
                $sessionStorage.currentVersion = resp;
                $rootScope.versionData = resp;
                $scope.versioningservice();
                Notification.success({
                    message: "Version Copied Successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
              //  $('#mdf_loader').css('display', 'none');
            })
        }

        // Delete Record
        $scope.deleteData = function (delobj) {
            // $('#mdf_loader').css('display', 'block');
            //var versionId = delobj.VersionID;
            versioningservice.deleteData(delobj.VersionID).success(function () {
                //$sessionStorage.currentVersion = resp;
                //$rootScope.versionData = resp;
                $scope.versioningservice();
               
                Notification.success({
                    message: $scope.a.VersionName + " is deleted Successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
                if ($localStorage.selectedversion != null && $localStorage.selectedversion != undefined && delobj.VersionID == $localStorage.selectedversion.VersionID)
                { delete $localStorage.selectedversion; }
                if ($sessionStorage.currentVersion != null && $sessionStorage.currentVersion != undefined && delobj.VersionID == $sessionStorage.currentVersion.VersionID)
                {
                    delete $sessionStorage.currentVersion;
                }
                //  $('#mdf_loader').css('display', 'none');
            });

        }


        //Declaring the function to load data from database
        var financial = {
            FinancialyearID: $localStorage.user.FinancialYearID,
            
        }
        $scope.PassData = function (x) {
            $scope.a = x;
            var fyid = x.FinancialYearID;
            var versionId = x.VersionID;
            var indexval = _.filter($scope.versioningData, function(o){
                return o.FinancialYearID == fyid
            });
            
            if(indexval.length <= 1){
                $scope.errorSingular = 'This is the only version available for this planning period. Do you really want to delete this?';
            }
            else
            {
                $scope.errorSingular = 'Are you sure you want to delete the plan : '+ x.VersionName+' ?';
                
            }
            
        }
        $scope.fillFinancialyearList = function (financial) {
          
            $scope.FinancialyearList = null;
            
          return  $http({
                method: 'POST',
                url: $rootScope.api + 'Version/GetFinancialYear',
                data: financial
            }).success(function (result) {
                $scope.FinancialyearList = result;
            });
        };
        $scope.fillFinancialyearList(financial);
        
        //on click routing to budget page
        $scope.gotoBudget = function (data) {
            $localStorage.user.FinancialYearID = data.FinancialYearID;
        
            var AllVersions;
            if ($sessionStorage.currentVersion != undefined) {
                if ($sessionStorage.currentVersion.Version != undefined)
                    if ($sessionStorage.currentVersion.Version.length == 0) {
                        AllVersions = $sessionStorage.currentVersion.Version;
                        AllVersions.push({
                            VersionID: data.VersionID,
                            VersionNo: data.VersionNo,
                            VersionName: data.VersionName,
                            IsFinal: data.IsFinal,
                            IsAllFinancialYear: data.IsAllFinancialYear,
                            CreatedBy: data.CreatedBy
                        });
                    }
                    else AllVersions = $sessionStorage.currentVersion.Version;
            }
            $localStorage.selectedversion = data;
            $localStorage.selectedversion.Financialyear = $localStorage.selectedversion.FinancialPeriod;
            $rootScope.versionData = data;
            $sessionStorage.currentVersion = data;
            $sessionStorage.currentVersion.Version = AllVersions;
            $localStorage.previousYear = data.Financialyear.replace(data.Financialyear.slice(2, 4), data.Financialyear.slice(2, 4) - 1);
            $state.go('budget.allocation');
        }

        // show all countries
        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                $scope.ctries = _.pluck(countryList, 'DisplayName').join(", ");
            }
        }

        // Sorting Users
        $scope.sort = function (key, elemId) { // key = PartnerName
            var elem = $("#" + elemId);
            
        //    elem.find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'PlanId')
                angular.element('#PlanId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'FinancialpId')
                angular.element('#FinancialpId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'VersionId')
                angular.element('#VersionId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'totalmdf')
                angular.element('#totalmdf').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'BaseLineId')
                angular.element('#BaseLineId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'ProgramId')
                angular.element('#ProgramId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'CountryId')
                angular.element('#CountryId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');

            var icon = elem.find("i").attr("class").split(" ")[2];
            var toSort = 'asc';
            if (icon === 'fa-caret-down') { // do descending sort
                var toSort = 'asc';
                elem.find("i").removeClass('fa-caret-down').addClass('fa-caret-up');

            } else { // do ascending sort
                var toSort = 'desc';
                elem.find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
                
            }
            $scope.versioningData = UtilityService.sortForVersionManagement($scope.versioningData, key, toSort);
            
        }



         //to clear the values after closing modal popup
        $('#myModalcreate,#copyModel').on('hidden.bs.modal', function () {
          //  $(this).find("input,textarea,select").val('').end();
            $scope.drpdpwnvalue = '';
            $scope.planname = '';
            $scope.drpdpwnvalue1 = '';
            $scope.DestPlanvalue = '';
        });
    }
     
    
       
})();