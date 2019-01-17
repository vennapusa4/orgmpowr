/*
Pre Budget controller: The user selects the country and partner
Created by: Aamin Khan
Created at: 19/01/2017
*/
(function () {
    'use strict';
    angular.module("hpe").controller("prebudgetCtrl", prebudgetCtrlFn)


    function prebudgetCtrlFn($scope, $rootScope, $filter, countryParnerFactoy, Notification, $log, $timeout, $sessionStorage, $state, authService, $localStorage, roleMappingService, UtilityService, $http) {
        //ngInject
        authService.checkUser();
        var partnersClone = [];
        $scope.SC_GeoSelectedValues = [];
        $scope.example12settings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true
        };
        $scope.isAdmin = $localStorage.Admin;
        $scope.inactive = false; // Show InActive
        $scope.showDistrict = false;
        $scope.hideMembership = true;
        $scope.UserData = $localStorage.user;
        //$scope.searchCriteria = {
        //    SC_GeoSelectedValues: { GeoID: -1, DisplayName: 'Select' },
        //    SC_CountrySelectedValues: [],
        //    SC_BudgetSelectedValues: { PartnerTypeID: -1, PartnerName: 'Select' },
        //    SC_DistrictValues: [],
        //    SC_FinantialPeriodSelectedValues: {FinancialyearID:-1, Financialyear :'Select'}
        //};
        //$scope.SC_Partner=[{ PartnerTypeID: -1, PartnerName: 'Select' }];
        //$scope.SC_FinancialPeriod = [{ FinancialyearID: -1, Financialyear: 'Select' }];
        //$scope.SC_Districts = [];

        var init = function () {
            //$scope.SC_Geo = { GeoID: -1, DisplayName: 'Select' };
            $scope.partners = [{ PartnerTypeID: -1, PartnerName: 'Select' }];
            $scope.selectedPartner = { PartnerTypeID: -1, PartnerName: 'Select' };
            $scope.membershipsData = [{ MembershipGroupID: null, MembershipName: 'Select' }];
            $scope.selectedMembership = { MembershipGroupID: null, MembershipName : 'Select'};
            countryParnerFactoy.getGeoData().then(
            function onSuccess(res) {
                $scope.geo = res.data[0];
                $scope.SC_Geo = angular.copy($scope.geo);
                $scope.geo.splice(0, 0, { GeoID: -1, DisplayName: 'Select' });
                $scope.SC_Geo.splice(0, 0, { GeoID: -1, DisplayName: 'Select' });
                $scope.selectedGeo = $scope.geo[0];
                $scope.selectedCountry = [];
                $scope.countries = [];
            });
            countryParnerFactoy.getAllGeoData().then(
            function onSuccess(res) {
                $scope.IsActiveGeo = res.data[0];
                $scope.IsActiveGeo.splice(0, 0, {GeoID: -1, DisplayName: 'Select'});
                //$scope.SC_Geo = res.data[0];
                //$scope.SC_Geo.splice(0, 0, { GeoID: -1, DisplayName: 'Select' });
                //$scope.selectedCountry = [];
                //$scope.countries = [];
            });
            countryParnerFactoy.getData().then(
               function onSuccess(response) {
                   // Handle success

                   var data = response.data;
                   var status = response.status;
                   var statusText = response.statusText;
                   var headers = response.headers;
                   var config = response.config;
                   //$scope.countries = data.Countries;
                   $scope.partnersData = _.filter(data.Partners, function (x) { return x.IsActive == true; }); //data.Partners.filter(a => a.IsActive == true);
                   $scope.districtsData = _.filter(data.Districts, function (x) { return x.IsActive == true; });// data.Districts.filter(a => a.IsActive == true);
                   $scope.SC_Partner = $scope.partnersData;
                   $scope.SC_Partner.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                   $scope.districts = data.Districts;
                   $scope.inactivedistricts = data.Districts;
                   $scope.inactivepartners = data.Partners;
                   $scope.membershipsData = _.filter(data.Memberships, function (i) { return i.IsActive == true; });
                   
               },

             function onError(response) {
                 Notification.error({
                     message: response.data,
                     delay: null
                 });
             })
        }

        init();
        $scope.OnGeoChange = function (selectedGeo) {
            $scope.selectedCountry = [];
            $scope.countries = [];
            if (selectedGeo.GeoID !== -1) {

                countryParnerFactoy.getCountriesData(selectedGeo.GeoID).then(
            function onSuccess(res) {

                $scope.countries = res.data[0];
            })
                $scope.partners = angular.copy($scope.partnersData);
                $scope.districts = angular.copy($scope.districtsData);
                partnersClone = angular.copy($scope.partners);
                if ($scope.partners[0].PartnerTypeID !== -1) {
                    $scope.partners.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                }

                $scope.selectedPartner = $scope.partners[0];
                $scope.selectedDistrict = $scope.districts == null ? 0 : $scope.districts[0];
                if ($scope.selectedCountry.length === 0 && $scope.selectedGeo.DisplayName == 'North America') {
                    //   $scope.partners.splice(2, 1);
                    //   HSM-552
                    $scope.partners = $scope.partners.filter(function (c) {
                        return c.PartnerName != "Reseller";
                    });
                }

                $scope.getPartners();
                $scope.hideMembership = true;
                $scope.selectedMembership = { MembershipGroupID: null, MembershipName: 'Select' };
            }
        }

        $scope.changeCountryOrDistrict = function () {
            if ($scope.selectedCountry.length === 0 && $scope.selectedGeo.GeoID === -1) {
                $scope.partners = [{ PartnerTypeID: -1, PartnerName: 'Select' }];
                $scope.selectedPartner = { PartnerTypeID: -1, PartnerName: 'Select' };
                $scope.hideMembership = true;
                $scope.selectedMembership = { MembershipGroupID: null, MembershipName: 'Select' };
            } else {

                $scope.partners = angular.copy(partnersClone);
                if ($scope.partners[0].PartnerTypeID !== -1) {
                    $scope.partners.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                }
                var global = this;
                var onlyDistributor;
                if (!($scope.partners.length == 2)) {
                    onlyDistributor = _.find($scope.partners, function (o) {

                        if (o.PartnerName == "Distributor") {
                            global.onlyDistributor = false;

                        }
                        else {
                            global.onlyDistributor = true;
                        }
                    })
                    //onlyDistributor = onlyDistributor == undefined ? false : true;
                }
                else {
                    global.onlyDistributor = true;
                }
                $scope.selectedPartner = $scope.partners[0];
                angular.forEach($scope.selectedCountry, function (rec) {
                    angular.forEach($scope.countries, function (row) {
                        if (rec.CountryID === row.CountryID) {
                            rec.DisplayName = row.DisplayName;
                        }

                    });
                });
                if ($scope.selectedCountry.length === 0 && $scope.selectedGeo.DisplayName == 'North America') {
                    //   $scope.partners.splice(2, 1);
                    //   HSM-552
                    $scope.partners = $scope.partners.filter(function (c) {
                        return c.PartnerName != "Reseller";
                    });
                }
                if ($scope.selectedCountry.filter(function (c) {
                        return c.DisplayName == "UNITED STATES";
                }).length == 1) {
                    $scope.partners = $scope.partners.filter(function (c) {
                        return c.PartnerName != "Reseller";
                    });
                }
                $scope.getPartners();
                $scope.hideMembership = true;
                $scope.selectedMembership = { MembershipGroupID: null, MembershipName: 'Select' };
            }
        }

        $scope.collapse = false;
        $scope.collapse1 = true;
        $scope.expand = function (val) {
            if (val === 'collapse') {
                $scope.collapse = !$scope.collapse;
                if ($scope.collapse == false) {
                    $scope.collapse1 = true;
                }
            } else if (val === 'collapse1') {
                $scope.collapse1 = !$scope.collapse1;
                if ($scope.collapse1 == false) {
                    $scope.collapse = true;
                }
            }
        }

        $scope.getPartners = function () {

            if ($scope.selectedCountry.length === 1) {
                let data = [];
                angular.forEach($scope.selectedCountry, function (data) {

                    if (data.DisplayName == 'UNITED STATES') {

                        for (var i = 0; i < $scope.districts.length; i++) {
                            $scope.partners.push(
                                { PartnerTypeID: i + 2, PartnerName: "Reseller-" + $scope.districts[i].DistrictName }
                            )
                        }


                        var isDistributor = _.find($scope.partners, function (o) {
                            return o.PartnerName == "Distributor"
                        })
                        $scope.selectedPartner = { PartnerTypeID: -1, PartnerName: 'Select' };
                        if ($scope.partners[0].PartnerTypeID !== -1) {
                            $scope.partners.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                        }

                        //if (typeof isDistributor != 'undefined') {
                        //    if ($scope.partners[2].DisplayName == 'Reseller')
                        //        $scope.partners.splice(2, 1);
                        //} else {
                        //    $scope.partners.splice(1, 1);
                        //}
                    } else {
                        if ($scope.partners[0].PartnerTypeID !== -1) {
                            $scope.partners.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                            $scope.selectedPartner = { PartnerTypeID: -1, PartnerName: 'Select' };
                        }

                    }
                })
            }
            else if ($scope.selectedCountry.length === 2) {
                let countries = [];
                angular.forEach($scope.selectedCountry, function (data) {
                    if (data.DisplayName === 'UNITED STATES' || data.DisplayName === 'CANADA') {
                        countries.push(data.DisplayName);
                    }

                });
                if (countries.length === 2) {
                    $scope.partners.splice(2, 1);
                }
            }
        }
        $scope.OnPartnerChange = function (selectedPartner) {
            if (selectedPartner.PartnerTypeID >= 2) {
                $scope.memberships = angular.copy($scope.membershipsData);
                //$scope.memberships = angular.splice(0, 0, { MembershipGroupID: -1, MembershipName: 'Select' });
                
                if ($scope.memberships[0].MembershipGroupID !== null) {
                    $scope.memberships.splice(0, 0, { MembershipGroupID: null, MembershipName: 'Select' });
                }

                $scope.selectedMembership = _.filter($scope.memberships, function (o) { return o.MembershipName == 'Both'; })[0];


                $scope.hideMembership = false;
            }
            else {
                $scope.hideMembership = true;
                $scope.selectedMembership = { MembershipGroupID: null, MembershipName: 'Select' };
            }
        }
        $scope.procced = function () {


            var obj = {};
            let CountryIds = '';
            obj.geo = $scope.selectedGeo;
            obj.countries = $scope.selectedCountry;
            
            var country = "Country";
            $localStorage.user.FinancialYearID = $localStorage.user.CurrentFinancialYearID;
            var partner = $scope.selectedPartner;
            var membership = $scope.selectedMembership;

            var partner = getPartnerTypeId($scope.selectedPartner);

            $localStorage.PartnerTypeID = partner.PartnerTypeID

            var district = getDistrict($scope.selectedPartner);
            var CountryOrGeoOrDistrict = $scope.selectedCountry;

            let selectedValue = '';
            if ($scope.selectedCountry.length === 0) {
                selectedValue = $scope.selectedGeo.GeoID;
                obj.AllocationLevel = 'Geo';
            } else {
                $scope.items = $filter('orderBy')($scope.selectedCountry, 'CountryID');
                angular.forEach($scope.items, function (row, $index) {

                    if ($scope.items.length === $index + 1) {
                        CountryIds += row.CountryID;
                    } else {
                        CountryIds += row.CountryID + ',';
                    }

                })

                if (district.DistrictID === 0) {
                    obj.AllocationLevel = 'Country';
                    selectedValue = CountryIds;
                } else {
                    obj.AllocationLevel = 'District';
                    selectedValue = district.DistrictID;
                }

            }
            obj.countryID = CountryIds;
            var currentDate = new Date();
            var FinencialYear;
            if (currentDate.getMonth() >= 0 && currentDate.getMonth() <= 5) {
                FinencialYear = "FY" + (currentDate.getFullYear() - 2000).toString() + "_1H";
            } else {
                FinencialYear = "FY" + (currentDate.getFullYear() - 2000).toString() + "_2H";
            }


            $sessionStorage.resellerPartner = {
                obj: obj,
                partner: partner,
                membership : membership,
                district: district,
                // countryOrDistrict: country,
                CountryOrGeoOrDistrict: selectedValue,
                FinencialYear: $localStorage.user.FinancialYear.replace(" ", "_")
            }

            $http({
                url: $rootScope.api + "BUBudgets/GetFinancialYear",
                method: 'POST',
                data: {
                    Countries: CountryIds,
                    DistrictID: district.DistrictID,
                    FinancialYearID: $localStorage.user.FinancialYearID,
                    PartnerTypeID: partner.PartnerTypeID,
                    CountryOrGeoOrDistrict: selectedValue,
                    AllocationLevel: obj.AllocationLevel.charAt(0),
                    MembershipGroupID: membership.MembershipGroupID
                }
            }).then(function (resp) {

                var user = $localStorage.user;


                var versions = resp.data;
                var currentYear = _.find(versions, function (o) {
                    return o.FinancialyearID == $localStorage.user.FinancialYearID
                })

                var finalVersion = _.find(currentYear.Version, function (o) {
                    return o.IsFinal == true;
                })
                if (finalVersion === undefined) {
                    var currentVersion = currentYear.Version[currentYear.Version.length - 1];
                } else {
                    var currentVersion = finalVersion;
                }

                var obj = currentYear;
                if (currentVersion != undefined) {

                    obj.VersionID = currentVersion.VersionID;
                    obj.VersionName = currentVersion.VersionName;
                    obj.VersionNo = currentVersion.VersionNo;
                    obj.FinancialPeriod = currentYear.Financialyear;
                    obj.MembershipGroupID = currentVersion.MembershipGroupID;
                }

                $rootScope.versionData = obj;
                $sessionStorage.currentVersion = obj;
                delete $localStorage.selectedversion;
                UtilityService.getLastUpdatedTime();
                //delete obj.Version;
                $localStorage.selectedversion = obj;

                $sessionStorage.fyParams = {
                    CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
                    AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
                    FinancialYearID: $localStorage.user.FinancialYearID,
                    PartnerTypeID: partner.PartnerTypeID,
                    MembershipGroupID: membership.MembershipGroupID
                }

                if (currentYear.Version.length > 0) {
                    $state.go('budget.allocation');
                } else {
                    //$state.go(roleMappingService.nextMenu());
                    $state.go('budget.versioning');
                }
            })

        }

        function getPartnerTypeId(selectedPartnerType) {
            var name = selectedPartnerType.PartnerName.split("-")[0];
            var o = _.filter(partnersClone, function (o) { return o.PartnerName == name; });
            return o[0];
        }

        function getDistrict(selectedPartnerType) {
            var name = selectedPartnerType.PartnerName.split("-")[1];
            var o = _.filter($scope.districts, function (o) { return o.DistrictName == name; });
            return typeof o[0] == 'undefined' ? { DistrictID: 0, DistrictName: '' } : o[0];
        }
        function getMembership(selectedMembership) {
        
        }


        //Search Criteria(SC) on Geo change
        $scope.SC_geoChange = function () {

            $scope.SC_Districts = [];
            $scope.searchCriteria.SC_DistrictValues = [];
            if ($scope.inactive == true) { // IsActive Geo checked
                if ($scope.searchCriteria.SC_GeoSelectedValues.GeoID !== -1) {
                    countryParnerFactoy.getAllCountriesData($scope.searchCriteria.SC_GeoSelectedValues.GeoID).then(
               function onSuccess(res) {
                   $scope.SC_Countries = res.data[0];
                   $scope.searchCriteria.SC_CountrySelectedValues = [];
                   $scope.searchCriteria.SC_DistrictValues.DistrictID = [];
                   //$scope.SC_Partner = $scope.partnersData;
                   $scope.SC_Partner = $scope.inactivepartners;
                   if ($scope.SC_Partner[0].PartnerTypeID !== -1) {
                       $scope.SC_Partner.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                   }

               });
                }
                else {
                    $scope.SC_Countries = [];
                    $scope.searchCriteria.SC_CountrySelectedValues = [];
                }
            }
            else {
                if ($scope.searchCriteria.SC_GeoSelectedValues.GeoID !== -1) {

                    countryParnerFactoy.getCountriesData($scope.searchCriteria.SC_GeoSelectedValues.GeoID).then(
                function onSuccess(res) {

                    $scope.SC_Countries = res.data[0];
                    $scope.searchCriteria.SC_CountrySelectedValues = [];
                    $scope.searchCriteria.SC_DistrictValues.DistrictID = [];
                    //$scope.SC_Partner = $scope.partnersData;
                    //$scope.SC_Partner = $scope.partnersData;
                    if ($scope.SC_Partner[0].PartnerTypeID !== -1) {
                        $scope.SC_Partner.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                    }
                })
                    
                    
                }

            }
            
        }

        //Search Criteria(SC) on Country change
        $scope.SC_CountryChange = function () {
            console.log($scope.searchCriteria.SC_CountrySelectedValues);
            if ($scope.searchCriteria.SC_CountrySelectedValues.length > 0) {
                angular.forEach($scope.searchCriteria.SC_CountrySelectedValues, function (rec) {
                    angular.forEach($scope.SC_Countries, function (row) {
                        if (rec.CountryID === row.CountryID) {
                            rec.DisplayName = row.DisplayName;
                        }

                    });
                });
                if ($scope.searchCriteria.SC_CountrySelectedValues.length === 1 && $scope.searchCriteria.SC_CountrySelectedValues[0].DisplayName === 'UNITED STATES') {
                    //console.log($scope.districts);
                    //$scope.SC_Districts = $scope.districtsData;
                    if($scope.inactive)
                        $scope.SC_Districts = $scope.inactivedistricts;
                    else 
                        $scope.SC_Districts = $scope.districtsData;
                    // $scope.SC_Partner.splice(1, 1);
                } else {
                    $scope.SC_Districts = [];
                    $scope.searchCriteria.SC_DistrictValues = [];
                }
            }
            else {
                $scope.SC_Districts = [];
                $scope.searchCriteria.SC_DistrictValues = [];
            }
        }


        $scope.SC_DistrictChange = function () {
            angular.forEach($scope.searchCriteria.SC_DistrictValues, function (rec) {
                angular.forEach($scope.SC_Countries, function (row) {
                    if (rec.DistrictName === row.DistrictName) {
                        rec.DistrictName = row.DistrictName;
                    }

                });
            });
        }
        $scope.SC_OnPartnerChange = function () {
        }
        $scope.SC_FyChange = function () {
            console.log($scope.searchCriteria.SC_FinantialPeriodSelectedValues);
        }
        $scope.search = function () {

            let geoId = '';
            let countryIds = '';
            let districtId = '';
            let PartnerId = '';
            let financialYearId = '';
            if ($localStorage.user.PartnerTypes.length == 1) {
                PartnerId = $localStorage.user.PartnerTypes[0]
            }
            if ($scope.searchCriteria.SC_GeoSelectedValues.GeoID !== -1) {
                geoId = $scope.searchCriteria.SC_GeoSelectedValues.GeoID;
            }

            $scope.searchCriteria.SC_CountrySelectedValues = $filter('orderBy')($scope.searchCriteria.SC_CountrySelectedValues, 'CountryID');
            angular.forEach($scope.searchCriteria.SC_CountrySelectedValues, function (row, $index) {
                if ($scope.searchCriteria.SC_CountrySelectedValues.length === $index + 1) {
                    countryIds += row.CountryID;
                } else {
                    countryIds += row.CountryID + ',';
                }
            });

            if ($scope.searchCriteria.SC_DistrictValues.length > 0) {
                $scope.searchCriteria.SC_DistrictValues = $filter('orderBy')($scope.searchCriteria.SC_DistrictValues, 'DistrictID');
                angular.forEach($scope.searchCriteria.SC_DistrictValues, function (row, $index) {
                    if ($scope.searchCriteria.SC_DistrictValues.length === $index + 1) {
                        districtId += row.DistrictID;
                    } else {
                        districtId += row.DistrictID + ',';
                    }
                })
            }

            financialYearId = $scope.searchCriteria.SC_FinantialPeriodSelectedValues.FinancialyearID;
            if ($scope.searchCriteria.SC_BudgetSelectedValues.PartnerTypeID !== -1) {
                PartnerId = $scope.searchCriteria.SC_BudgetSelectedValues.PartnerTypeID;
            }

            var SC_data = {
                GeoID: geoId,
                CountryID: countryIds,
                DistrictID: districtId,
                PartnerTypeID: PartnerId,
                FinancialYearID: financialYearId,
                UserID: $localStorage.user.UserID
            };

            countryParnerFactoy.SC_search(SC_data).then(function onSuccess(res) {
                $scope.searchResults = res.data;
            })
        }



        $scope.SC_proceed = function (x) {
            console.log(x);

            var obj = {};
            let CountryIds = '';
            obj.geo = x.Geo[0];
            obj.countries = x.Country;
            var country = "Country";
            $localStorage.NOACCESS = false;

            $localStorage.previousYear = x.FinancialYear.replace(x.FinancialYear.slice(2, 4), x.FinancialYear.slice(2, 4) - 1);
            if ($localStorage.user.UserTypeID == 1) {
                var userCountries = $localStorage.user.Countries
                var common = obj.countries.filter(function (c) {
                    return userCountries.indexOf(c.CountryID) !== -1;
                });
                $localStorage.NOACCESS = !angular.equals(obj.countries, common);
            }

            //$localStorage.user.FinancialYearID = $localStorage.user.CurrentFinancialYearID;
            $localStorage.user.FinancialYearID = x.FinancialYearID; // Fixed for previos version Partner budget not disabled- 2-15-2018
            //var partner = $scope.selectedPartner;
            var partner = {};

            partner = x.PartnerType[0];

            var membership = {};
            if (x.Membership == null) {
                membership = { MembershipGroupID: null, MembershipName: 'Select' };
            }
            else {
                membership = x.Membership[0];
            }
            //partner.PartnerName = 'Reseller - ' + x.PartnerType[0].PartnerName;

            $localStorage.PartnerTypeID = partner.PartnerTypeID
            var district = [];
            if (x.District !== null) {
                district = x.District[0];
            }

            //district.DistrictID = '0';
            //district.DistrictName = '';
            // var CountryOrGeoOrDistrict = $scope.selectedCountry;

            let selectedValue = '';

            $scope.items = $filter('orderBy')(x.Country, 'CountryID');
            //CountryIds = x.CountryOrGeoOrDistrict;

            selectedValue = x.CountryOrGeoOrDistrict;
            if (x.AllocationLevel === 'C') {
                obj.AllocationLevel = 'Country';

            } else if (x.AllocationLevel === 'D') {
                obj.AllocationLevel = 'District';
                // 
            } else if (x.AllocationLevel === 'G') {
                obj.AllocationLevel = 'Geo';
                // selectedValue = district.DistrictID;
            }

            angular.forEach(x.Country, function (row, $index) {

                if (x.Country.length === $index + 1) {
                    CountryIds += row.CountryID;
                } else {
                    CountryIds += row.CountryID + ',';
                }

            })

            obj.countryID = CountryIds;

            var FinencialYear = x.FinancialYear;


            $sessionStorage.resellerPartner = {
                obj: obj,
                partner: partner,
                membership: membership,
                district: district,
                // countryOrDistrict: country,
                CountryOrGeoOrDistrict: x.CountryOrGeoOrDistrict,
                FinencialYear: FinencialYear.replace(" ", "_")
            }

            $http({
                url: $rootScope.api + "BUBudgets/GetFinancialYear",
                method: 'POST',
                data: {
                    Countries: CountryIds,
                    DistrictID: district.DistrictID,
                    FinancialYearID: x.FinancialYearID,
                    PartnerTypeID: partner.PartnerTypeID,
                    CountryOrGeoOrDistrict: selectedValue,
                    AllocationLevel: x.AllocationLevel,
                    MembershipGroupID: membership.MembershipGroupID
                }
            }).then(function (resp) {
                var user = $localStorage.user;
                //UtilityService.getLastUpdatedTime(); // moving this to end after updating  $sessionStorage.currentVersion

                var versions = resp.data;
                var currentYear = _.find(versions, function (o) {
                    return o.FinancialyearID == x.FinancialYearID
                })

                var finalVersion = _.find(currentYear.Version, function (o) {
                    return o.IsFinal == true;
                })
                if (finalVersion === undefined) {
                    var currentVersion = currentYear.Version[currentYear.Version.length - 1];
                } else {
                    var currentVersion = finalVersion;
                }
                //var currentVersion = currentYear.Version[currentYear.Version.length - 1];
                var obj = currentYear;
                if (currentVersion != undefined) {

                    obj.VersionID = currentVersion.VersionID;
                    obj.VersionName = currentVersion.VersionName;
                    obj.VersionNo = currentVersion.VersionNo;
                    obj.FinancialPeriod = currentYear.Financialyear;
                    obj.MembershipGroupID = currentVersion.MembershipGroupID;
                }

                $rootScope.versionData = obj;
                $sessionStorage.currentVersion = obj;
                UtilityService.getLastUpdatedTime();
                delete $localStorage.selectedversion;
                //delete obj.Version;
                $localStorage.selectedversion = obj;

                $sessionStorage.fyParams = {
                    CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
                    AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
                    FinancialYearID: x.FinancialYearID,
                    PartnerTypeID: partner.PartnerTypeID,
                    MembershipGroupID: membership.MembershipGroupID
                }

                if (currentYear.Version.length > 0) {
                    $state.go('budget.allocation');
                } else {
                    $state.go(roleMappingService.nextMenu());
                }
            })



        }

        $scope.exportBudget = function () {
            countryParnerFactoy.ExportBudgetReport();
        }
        $scope.exportCarveout = function () {
            countryParnerFactoy.ExportCarveoutReport();
        }
        $scope.showInActiveGeos = function(value) {
            $scope.reset_SC();
            $scope.inactive = value;
            if(value == true){
                $scope.SC_Geo = angular.copy($scope.IsActiveGeo);
                
                $scope.SC_Partner = angular.copy($scope.inactivepartners);
                $scope.SC_Partner.splice(0, 0, { PartnerTypeID: -1, PartnerName: 'Select' });
                
            }
            else {
                $scope.SC_Geo = angular.copy($scope.geo);
                $scope.SC_Partner = [{ PartnerTypeID: -1, PartnerName: 'Select' }];
                $scope.SC_Partner = angular.copy($scope.partnersData);
            }
        }
        // search criteria reseat
        $scope.reset_SC = function () {
            $scope.searchCriteria = {
                SC_GeoSelectedValues: { GeoID: -1, DisplayName: 'Select' },
                SC_CountrySelectedValues: [],
                SC_BudgetSelectedValues: { PartnerTypeID: -1, PartnerName: 'Select' },
                SC_DistrictValues: [],
                SC_FinantialPeriodSelectedValues: {}
            };

            // $scope.SC_Partner = [{ PartnerTypeID: -1, PartnerName: 'Select' }];
            //$scope.SC_FinancialPeriod = [{ FinancialyearID: -1, Financialyear: 'Select' }];
            $scope.SC_Districts = [];
            $scope.SC_Countries = [];
            $scope.inactive = false;
            $scope.SC_Geo = angular.copy($scope.geo);
            //$scope.SC_Partner = [{ PartnerTypeID: -1, PartnerName: 'Select' }];
            $scope.SC_Partner = angular.copy($scope.partnersData);
            countryParnerFactoy.getFinancialYear().then(
           function onSuccess(res) {

               $scope.SC_FinancialPeriod = res.data;
               let currentFinancialYearData = _.find($scope.SC_FinancialPeriod, function (o) {
                   return o.Financialyear == $localStorage.user.FinancialYear;
               })
               $scope.searchCriteria.SC_FinantialPeriodSelectedValues = currentFinancialYearData;
               $scope.search();
           })

        }
        $scope.reset_SC();
    }



})();
