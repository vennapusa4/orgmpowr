
(function () {
    'use strict';
    angular.module("hpe").controller("userManagementCtrl", userManagementFn);

    function userManagementFn($scope, $timeout, $sessionStorage, userManagementFactory, Notification, $rootScope, $localStorage, $http, UtilityService) {
        //ngInject

        $scope.ShowDataUpload = $localStorage.user.DataUploadAccess;
        $scope.example14model = [];
        $scope.gioSelectedValues = [];
        $scope.subRegionSelectedValues = [];
        $scope.example12settings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true
        };

        // Sorting Users
        //dgdfg

        $scope.sort = function (key, elemId) { // key = PartnerName
            
            var elem = $("#" + elemId);
            if (elemId !== 'UemailId')
                angular.element('#UemailId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UnameId')
                angular.element('#UnameId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UroleId')
                angular.element('#UroleId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UaccessId')
                angular.element('#UaccessId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UregionId')
                angular.element('#UregionId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UgeoId')
                angular.element('#UgeoId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UcountryId')
                angular.element('#UcountryId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UDistrictId')
                angular.element('#UDistrictId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            if (elemId !== 'UStatusId')
                angular.element('#UStatusId').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            
            var icon = elem.find("i").attr("class").split(" ")[2];
            var toSort = 'asc';
            if (icon === 'fa-caret-down') { // do descending sort
                var toSort = 'asc';
                elem.find("i").removeClass('fa-caret-down').addClass('fa-caret-up');
            } else { // do ascending sort
                var toSort = 'desc';
                elem.find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            }
           
            $scope.uMData = UtilityService.sortForUserManagement($scope.uMData, key, toSort);
            
            //partnerCreditFactory.applyFilter($scope);
        }

        //Disable model data on close of model popup
        var disableRegionAndCountries = function () {
            angular.element('.example81 button.dropdown-toggle').attr("disabled", "disabled");
            angular.element('#example42 button.dropdown-toggle').attr("disabled", "disabled");
            angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
            angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
            angular.element('#example91 button.dropdown-toggle').attr("disabled", "disabled");
            angular.element('#example92 button.dropdown-toggle').attr("disabled", "disabled");
            angular.element('#example93 button.dropdown-toggle').attr("disabled", "disabled");
            angular.element('#example94 button.dropdown-toggle').attr("disabled", "disabled");
        }
        

        $scope.reset = function (form, isNew) {
            $scope.subRegionSelectedValues = [];
            if (isNew) {
                
                $scope.addObj['user'].EmailID = "";
                $scope.addObj['user'].FirstName = ""
                $scope.addObj['user'].LastName = "";
            } else {
                $scope.editObj['user'].EmailID = "";
                $scope.editObj['user'].FirstName = ""
                $scope.editObj['user'].LastName = "";
            }
            
            form.$setPristine();
            form.$setUntouched();
        }

        $scope.acceessLevelOptions = ['Active', 'InActive'];
        var copiedUnits = [];
        // Create New User
        $scope.addNewUser = function () {
            //disable the Region, sub-region, country and districts
            $timeout(function () {
                angular.element('#example91 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example92 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example93 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example94 button.dropdown-toggle').attr("disabled", "disabled");
            }, 0)
            $scope.example14model = [];
            $scope.example13model = [];
            $scope.example12model = [];
            $scope.example11model = [];
            $scope.gioSelectedValues = [];
            userManagementFactory.loadNewUserOptionsDetails().then(
                function onSuccess(response) {
                    $scope.newUData = [];
                    $scope.defaultData = response;
                    $scope.reseller = $scope.defaultData[3];
                    $scope.reseller = $scope.reseller.filter(function (e) {
                                return e.DisplayName == "Reseller"
                            });
                    copiedUnits = angular.copy(response[2]);
                    angular.copy($scope.defaultData, $scope.newUData);
                    //$scope.example12model = $scope.defaultData[2];
                    angular.forEach($scope.defaultData[2], function (SelectedBUs) {
                        $scope.example12model.push(SelectedBUs);
                    });
                    $scope.addObj['user'].BUAccess = true;
                    
                },
                function onError(response) {

                }
            );



            $scope.addObj = [];
            $scope.distElem = {

            }

            $scope.contryElem = {

            }

            var addObjScope = {
                "user": {
                    "EmailID": '',
                    "FirstName": '',
                    "LastName": '',
                    "IsAdmin": false,
                    "role": {
                        "RoleID": null,
                        "displayname": null
                    },
                    "userTypeDetails": {
                        "UserTypeID": null,
                        "DisplayName": null
                    },
                   // "RegionDetails":[],
                    //    {
                    //    "RegionID": null,
                    //    "DisplayName": null
                    //},
                    "GeoDetails":[],
                    //    {
                    //    "SubRegionID": null,
                    //    "DisplayName": null
                    //},
                    "IsActive": true,
                    "BusinessUnitDetails": [

                    ],
                    "districts": [

                    ],
                    "countries": [

                    ],
                    "partnerType": [
                    ]
                }
            }

            angular.copy(addObjScope, $scope.addObj);
        }

        //
        var loadAccessValues = function (isAdmin, isNewUser) {
            userManagementFactory.loadUserAcessLevelData(isAdmin).then(
                function onSuccess(response) {
                    if (isNewUser) {
                        $scope.defaultData[1] = response;
                    }
                    else {
                        $scope.userOptionsData[1] = response;
                    }
                },
                function onError(response) {

                }
            );
        }

        //Update Access level while adding new user 
        $scope.updateAccessLevelForAdmin = function (obj, isNewUser) {
            
            //load Access level values
            loadAccessValues(obj['user'].IsAdmin, isNewUser);
            //reset Region,SR data field
            disableRegionAndCountries();
            if (obj['user'].IsAdmin == false) {
                obj['user'].GlossaryApprover = false;
            }
            //obj.user.RegionDetails = [];
            //$scope.gioSelectedValues = [];
            
                if (obj['user']['userTypeDetails'].DisplayName === "Geo") {
                    angular.element('#example92 button.dropdown-toggle').removeAttr('disabled');
                    angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                } else if (obj['user']['userTypeDetails'].DisplayName === "World Wide") {
                    obj.user.GeoDetails = [];
                    $scope.subRegionSelectedValues = [];
                } else {
                    obj['user']['userTypeDetails'].DisplayName = null;
                    obj.user.GeoDetails = [];
                    $scope.subRegionSelectedValues = [];
                    obj['user']['userTypeDetails'].DisplayName = null;
                }
            
            $scope.example13data = [];
            $scope.example13model = [];
            $scope.example14data = [];
            $scope.example14model = [];
            
        }

        //$scope.enableSave = false; validation of user form
        $scope.validateAdd = function (addObj, example12model, example13model, example14model, example11model) {
            $scope.enableSave = false;
            if (addObj['user'].IsAdmin) {

            }
            if (!angular.isUndefined(addObj)) {
                var mand = (addObj['user']['role'].displayname !== null && addObj['user']['userTypeDetails'].DisplayName !== null && example12model.length !== 0 && example11model.length !== 0);
                if (addObj['user']['userTypeDetails'].DisplayName !== null) {
                    if (addObj['user']['userTypeDetails'].DisplayName === 'World Wide') {
                        $scope.enableSave = mand;
                    } else if (addObj['user']['userTypeDetails'].DisplayName === 'Geo') {
                        $scope.enableSave = mand && ( addObj['user']['GeoDetails'].DisplayName !== null);
                    } else if (addObj['user']['userTypeDetails'].DisplayName === 'Country') {
                        $scope.enableSave = mand && (addObj['user']['GeoDetails'].DisplayName !== null && example14model.length !== 0);
                    } else {
                        $scope.enableSave = mand && (addObj['user']['GeoDetails'].DisplayName !== null && example14model.length !== 0 && example13model.length !== 0);
                    }
                }
            }
        }

        //load The user-Management default data
        var loadDefaultData = function () {
            userManagementFactory.loadUserManagementData().then(
                function onSuccess(response) {
                   
                    $scope.uMData = response['users'];
                    //initializeUMLib();
                    $(document).ready(function(){
                        $('[data-toggle="popover"]').popover({
                            placement : 'top',
                            trigger: 'hover',
                            container: 'body'
                        });
                    })
                },
                function onError(response) {

                }
            );
        }

        loadDefaultData();

        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                 $scope.ctries = _.pluck(countryList, 'DisplayName').join(",");
            }
        }

        
        //Edit user details
        var setEditFieldsForUM = function (accessObj, obj, isNew) {
            $scope.example14model = [];
            $scope.example13model = [];
            //$scope.gioSelectedValues = [];
            $scope.subRegionSelectedValues = [];
            obj.user.countries = [];
            obj.user.districts = [];
            //obj.user.RegionDetails = [];
            obj.user.GeoDetails = [];
            //logic to disable the text-field
            
            if (accessObj.DisplayName === 'World Wide') {
                $timeout(function () {
                    disableRegionAndCountries();

                }, 0)
                userManagementFactory.loadGeoData(obj.user.userTypeDetails).then(
                    function onSuccess(response) {
                        if (isNew) {
                            $scope.defaultData[2] = response.data[0];

                        } else {
                            $scope.userOptionsData[4] = response.data[0];
                        }
                    })
            } else if (accessObj.DisplayName === 'District') {
                if (!isNew) {
                    $scope.example11model = $scope.example11model.filter(function (e) {
                        return e.DisplayName == "Reseller";
                    });
                }
                else {
                    $scope.example11model = $scope.example11model.filter(function (e) {
                        return e.PartnerTypeID == 2;
                    });
                }
                angular.element('.example81 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example40 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example39 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example91 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example92 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example93 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example94 button.dropdown-toggle').removeAttr('disabled');

                userManagementFactory.loadGeoData(obj.user.userTypeDetails).then(
                    function onSuccess(response) {
                        response.data[3] = [response.data[3]];
                        $scope.districtData = response.data;
                        delete $scope.districtData[2].RegionID;
                        delete $scope.districtData[3][0].SubRegionID;

                        //obj.user.RegionDetails = [$scope.districtData[1]];
                        obj.user.GeoDetails = [$scope.districtData[2]];
                        obj.user.countries = [$scope.districtData[2]];
                        var ctry = [];
                        $scope.ctry = [];
                        $scope.district = [];
                        if (isNew) {
                            $scope.defaultData[2] = $scope.districtData[0];
                            //$scope.example12model = $scope.districtData[0];
                            $scope.example12model = [];
                            angular.forEach($scope.districtData[0], function (SelectedBUs) {
                                $scope.example12model.push(SelectedBUs);
                            });
                        } else {
                            
                            $scope.userOptionsData[4] = $scope.districtData[0];
                            var BUs = $scope.editObj.user.BusinessUnitDetails;
                            angular.forEach($scope.editObj.user.BusinessUnitDetails, function (SelectedBUs) {
                                if (SelectedBUs.DisplayName == "Compute Value")
                                {
                                    BUs.splice(BUs.indexOf(SelectedBUs), 1);
                                }
                            });
                            angular.forEach($scope.editObj.user.BusinessUnitDetails, function (SelectedBUs) {
                                if (SelectedBUs.DisplayName == "Compute Volume")
                                {
                                    BUs.splice(BUs.indexOf(SelectedBUs), 1);
                                }
                            });
                            $scope.example12model = BUs;
                        }
                        //$scope.example12model = [];
                        $scope.example14data = [$scope.districtData[2]];
                        //$scope.gioSelectedValues = $scope.districtData[1];
                        $scope.subRegionSelectedValues = [$scope.districtData[2]];
                        $scope.example14model = [$scope.districtData[2]];
                        $scope.example13data = $scope.districtData[3][0];
                        $scope.example13model = obj.user.districts;
                        $scope.exampleCountrydata = [$scope.districtData[1]];
                        
                       
                        //$scope.exampleCountrydata = obj['user']['GeoDetails'];
                        angular.copy($scope.districtData[3], $scope.district);
                        angular.copy($scope.example14model, $scope.ctry);
                        console.log($scope.example13model, $scope.example13data, obj, $scope.districtData);
                    },
                    function onError(response) {

                    }

                )
                
            }
            else if (accessObj.DisplayName === 'Country') {
                angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example92 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example94 button.dropdown-toggle').attr("disabled", "disabled");
                //angular.element('#example42 button.dropdown-toggle').attr("disabled", "disabled");
                //angular.element('#example92 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example93 button.dropdown-toggle').attr("disabled", "disabled");
                userManagementFactory.loadGeoData(obj.user.userTypeDetails).then(
                    function onSuccess(response) {
                        if (isNew) {
                            $scope.defaultData[2] = response.data[0];
                            $scope.example12model =[];
                            angular.forEach($scope.defaultData[2], function (SelectedBUs) {
                                $scope.example12model.push(SelectedBUs);
                            });

                        } else {
                            $scope.userOptionsData[4] = response.data[0];
                        }
                        $scope.exampleCountrydata = response.data[1];
                    })
                if (isNew) {
                

                
                    //$scope.defaultData[3] = $scope.newUData[3];
                    $scope.example13data = [];
                    $scope.example13model = [];
                    $scope.example14data = [];
                    $scope.example14model = [];
                    //$scope.exampleCountrydata = [];
                } else {
                    
                    //$scope.subRegionSelectedValues = $scope.editOpt[2];
                    
                    $scope.example13data = [];
                    $scope.example13model = [];
                 }
            } else if (accessObj.DisplayName === 'Geo')
            {
                userManagementFactory.loadGeoData(obj.user.userTypeDetails).then(
                   function onSuccess(response) {
                       if (isNew) {
                           $scope.defaultData[2] = response.data[0];
                           //$scope.example12model = response.data[0];
                           $scope.example12model = [];
                            angular.forEach($scope.defaultData[2], function (SelectedBUs) {
                                $scope.example12model.push(SelectedBUs);
                            });
                       } else {
                           $scope.userOptionsData[4] = response.data[0];
                       }
                       $scope.exampleCountrydata = response.data[1];
                       
                   })
                angular.element('#example92 button.dropdown-toggle').removeAttr('disabled');
                angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                //angular.element('#example42 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                
                //angular.element('#example92 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example93 button.dropdown-toggle').attr("disabled", "disabled");
                angular.element('#example94 button.dropdown-toggle').attr("disabled", "disabled");

                if ($scope.districtData !== undefined) {
                    $scope.example14data = $scope.districtData[3];
                }
                $scope.example14model = [];
                //load Region values
                if (isNew) {
                    //$scope.defaultData[3] = $scope.newUData[3];
                } else {
                    // $scope.userOptionsData[2] = $scope.editOpt[2];
                    //$scope.subRegionSelectedValues = $scope.editOpt[2];
                }
                
            }
        }

        //check If US Selected
        var checkIfUsSelected = function(editObj){
            var isUs = false;
            angular.forEach(editObj.user.countries, function (row) {
                if (row.DisplayName === 'UNITED STATES') {
                    isUs = true;
                }
            })

            return isUs;
        }

        //To load Select Options For Edit
        var loadSelectOptionsForEdit = function (editObj) {
            //$scope.gioSelectedValues = editObj.user.RegionDetails;
            $scope.subRegionSelectedValues = editObj.user.GeoDetails;
            userManagementFactory.loadSelectOptionssData(editObj).then(
                function onSuccess(response) {
                    $scope.editOpt = [];
                    $scope.userOptionsData = response.data;
                    $scope.reseller = $scope.userOptionsData[5];
                    $scope.reseller = $scope.reseller.filter(function (e) {
                        return e.DisplayName == "Reseller"
                    });
                    angular.copy($scope.userOptionsData, $scope.editOpt);
                    copiedUnits = angular.copy(response.data[6]);
                    //$scope.example14model = editObj.user.countries;
                    $scope.example14model = _.filter(editObj.user.countries, function (o) { return o.DisplayName !== ''; }); 
                    $scope.example13model = _.filter(editObj.user.districts, function (o) { return o.DisplayName !== ''; });
                    $scope.example12model = _.filter(editObj.user.BusinessUnitDetails, function (o) { return o.DisplayName !== ''; });
                    
                    $scope.example11model = _.filter(editObj.user.partnerType, function (o) { return o.DisplayName !== ''; });
                   
                    $scope.example14data = $scope.userOptionsData[2];
                    $scope.example13data = $scope.userOptionsData[3];
                    $scope.exampleCountrydata = $scope.userOptionsData[6];
                    
                    if (editObj['user']['userTypeDetails'].DisplayName === 'World Wide') {
                        angular.element('.example81 button.dropdown-toggle').attr("disabled", "disabled");
                        angular.element('#example42 button.dropdown-toggle').attr("disabled", "disabled");
                        angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
                        angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                        editObj.user.countries = [];
                        editObj.user.districts = [];
                        editObj.user.GeoDetails = [];
                        $scope.subRegionSelectedValues = [];
                        //editObj.user.RegionDetails=[];
                    } else if (editObj['user']['userTypeDetails'].DisplayName === 'Geo') {
                        angular.element('.example81 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
                        angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                        //$scope.exampleCountrydata = editObj.user.GeoDetails;
                        editObj.user.countries = [];
                        editObj.user.districts = [];
                    }
                    //else if (editObj['user']['userTypeDetails'].DisplayName === 'Regional') {
                    //    angular.element('#example42 button.dropdown-toggle').attr("disabled", "disabled");
                    //    angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
                    //    angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                    //    editObj.user.countries = [];
                    //    editObj.user.districts = [];
                    //    editObj.user.GeoDetails = [];
                        
                    //}
                    else if (editObj['user']['userTypeDetails'].DisplayName === 'Country') {
                        angular.element('.example81 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example40 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                        editObj.user.districts = [];
                    } else {
                        angular.element('.example81 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example40 button.dropdown-toggle').removeAttr('disabled');
                        angular.element('#example39 button.dropdown-toggle').removeAttr('disabled');
                        $scope.example14model = editObj.user.countries;
                        $scope.example13model = editObj.user.districts;
                        $scope.example14data = editObj.user.countries;
                        //$scope.userOptionsData[2] = editObj.user.RegionDetails;
                        $scope.exampleCountrydata = editObj.user.GeoDetails;
                    }

                    editObj.user.countries = _.filter(editObj.user.countries, function (o) { return o.DisplayName !== ''; });
                    editObj.user.districts = _.filter(editObj.user.districts, function (o) { return o.DisplayName !== ''; });
                    editObj.user.BusinessUnitDetails = _.filter(editObj.user.BusinessUnitDetails, function (o) { return o.DisplayName !== ''; });
                    editObj.user.partnerType = _.filter(editObj.user.partnerType, function (o) { return o.DisplayName !== ''; });
                    loadAccessValues(editObj.user.IsAdmin, false);
                    $scope.validateAdd(editObj, editObj.user.BusinessUnitDetails, editObj.user.districts, editObj.user.countries, editObj.user.partnerType);
                },
                function onError(response) {

                }
            )
            
        }

        //load SubRegion Options
        var loadSubRegionOptions = function (regObj, ob) {
            
            userManagementFactory.loadSubRegionOptions(regObj).then(
                function onSuccess(response) {
                    $scope.exampleCountrydata = response.data[0];
                    //disable Country options and District options
                    $scope.example14model = [];
                    $scope.example14data = [];
                    angular.element('#example93 button.dropdown-toggle').attr("disabled", "disabled");
                    angular.element('#example94 button.dropdown-toggle').attr("disabled", "disabled");
                    angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
                    angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                    ob['user'].districts = [];
                    ob['user'].countries = [];
                    ob['user'].GeoDetails = [];
                },
                function onError(response) {

                }
            )
        }
        
        //edit user
        $scope.editUserInfo = function (obj) {
            $scope.DefaultGeo = obj.user.GeoDetails;
            //angular.element('.addExUserSave').attr("disabled", "disabled");
            angular.copy(obj, $scope.editObj);
            loadSelectOptionsForEdit($scope.editObj);
        }

        //To close one dropdown on select of other dropdown
        $scope.closeDropDown = function () {
            angular.element('div.dropdown-multiselect ul.dropdown-menu').css("display", "none");
        }

        //update Role Data On Edit
        $scope.updateRoleDataOnEdit = function (roleSelected, obj) {
                obj['user']['role'] = {};
                obj['user']['role'].RoleID = roleSelected.RoleID;
                obj['user']['role'].displayname = roleSelected.DisplayName;
        }

        //update Acess Data On Edit
        $scope.updateAcessDataOnEdit = function (accessSelected, obj, isNew) {
            obj['user']['userTypeDetails'] = {};
            obj['user']['userTypeDetails'].UserTypeID = accessSelected.UserTypeID;
            obj['user']['userTypeDetails'].DisplayName = accessSelected.DisplayName;
            

            setEditFieldsForUM(accessSelected, obj, isNew);

                   }

       // update Region Data On Edit
        $scope.updateRegionDataOnEdit = function (obj, newUser) {
           
            //obj.user.RegionDetails = [];
            //obj['user']['RegionDetails'] = [];
            obj['user']['GeoDetails'] = [];
            $scope.subRegionSelectedValues = [];
            obj.user.countries = [];
            obj.user.districts = [];
            //enable SubRegion options
            //if (regionSelected.DisplayName !== '') {
            //    if (obj.user.userTypeDetails.DisplayName === 'Regional')
            //    {
            //        obj.user.countries = [];
            //        obj.user.districts = [];
            //    }
            //    else {
            //        angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
            //        angular.element('#example92 button.dropdown-toggle').removeAttr('disabled');
            //        loadSubRegionOptions(regionSelected, obj);
            //    }
            //}

            if ($scope.gioSelectedValues.length != 0) {
                var geoData = [];
                let seletedRegiondata = [];
                if (newUser) {
                    geoData = $scope.defaultData[3];
                } else {
                    geoData = $scope.userOptionsData[2];
                }
                angular.forEach($scope.gioSelectedValues, function (rec) {
                    angular.forEach(geoData, function (row) {
                        if (rec.RegionID === row.RegionID) {
                            rec.DisplayName = row.DisplayName;
                            seletedRegiondata.push({
                                'RegionID': rec.RegionID,
                                'DisplayName': row.DisplayName
                            });
                        }
                    });
                });
               
                //obj['user'].RegionDetails = $scope.gioSelectedValues;

                if (obj.user.userTypeDetails.DisplayName === 'Regional')
                {
                    obj.user.countries = [];
                    obj.user.districts = [];
                }
                else {
                    angular.element('#example42 button.dropdown-toggle').removeAttr('disabled');
                    angular.element('#example92 button.dropdown-toggle').removeAttr('disabled');
                    loadSubRegionOptions(seletedRegiondata, obj);
                }
            }
            
            //angular.forEach($scope.gioSelectedValues, function (rec) {
            //    angular.forEach($scope.defaultData[3], function (row) {
            //        if (rec.RegionID === row.RegionID) {
            //                rec.DisplayName = row.DisplayName;
            //            }
            //        });
            //    });
            //    obj['user'].RegionDetails = _.filter($scope.defaultData[3], function (o) { return o.DisplayName !== ''; });
              
            
        }

        //Delete User-Management Data
        $scope.setDeleteUser = function (obj) {
            $scope.elementsToDelete = obj;
           
        }

        $scope.deleteUser = function () {
            
            $scope.elementsToDelete.user.loggedUser = $localStorage.user.UserID;
            userManagementFactory.deleteUserManagementData($scope.elementsToDelete.user).then(
                function onSuccess(response) {
                    angular.element('#myModalNorm3').modal('hide');
                    Notification.success("User Deleted Successfully");
                    loadDefaultData();
                },
                function onError(response) {
                    Notification.error("Error while Deleting a User");
                }
            );
        }

        //Reset Password
        $scope.resetUserPassword = function (obj) {
            $scope.passResetForReset = obj;
        }

        $scope.resetPassword = function () {
            angular.element('.resetUser').attr("disabled", "disabled");
            $scope.passResetForReset.user.loggedUser = $localStorage.user.UserID;
            userManagementFactory.resetUserPassword($scope.passResetForReset.user).then(
                function onSuccess(response) {
                    angular.element('.resetUser').removeAttr('disabled');
                    angular.element('#myModalNorm2').modal('hide');
                    Notification.success("Password Reset Successfully. Email has been sent to User.");

                },
                function onError(response) {
                    angular.element('.resetUser').removeAttr('disabled');
                    Notification.error("Password Not Reset Successfully");
                }
            );
        }

        // Email Validation
        function validateEmail(email) {
            var re = /\S+@\S+\.\S+/;
            //var re = /^\"?[\w-_\.]*\"?@hpe\.com$/;
            return re.test(email);
        }

        //update Edited User Data in User table list
        var updateEditedUserData = function (editObj) {
            editObj.user.countries = _.filter(editObj.user.countries, function (o) { return o.DisplayName !== ''; });
            editObj.user.districts = _.filter(editObj.user.districts, function (o) { return o.DisplayName !== ''; });
            editObj.user.BusinessUnitDetails = _.filter(editObj.user.BusinessUnitDetails, function (o) { return o.DisplayName !== ''; });
            editObj.user.partnerType = _.filter(editObj.user.partnerType, function (o) { return o.DisplayName !== ''; });
            
            editObj.user.loggedUser = $localStorage.user.UserID;
        }



        // Code to Save Edited User Data
        $scope.saveEditedUserData = function (editObj, isNew) {
            updateEditedUserData(editObj);
            if (isNew) {
                angular.element('.addNewUserSave').attr("disabled", "disabled");
                angular.element('.addNewUserCancel').attr("disabled", "disabled");
                editObj['user'].BusinessUnitDetails = $scope.example12model;
                
            }
            else
            {
                angular.element('.addExUserSave').attr("disabled", "disabled");
                angular.element('.addExUserCancel').attr("disabled", "disabled");
                editObj['user'].BusinessUnitDetails = $scope.example12model;
            }
                userManagementFactory.editUserManagementData(editObj.user).then(
                    function onSuccess(response) {
                        
                        if (response.data === 'User exists') {
                            Notification.warning("User Already Exists. Cannot Create Duplicate User.");
                            if (isNew) {
                                angular.element('.addNewUserSave').removeAttr('disabled');
                                angular.element('.addNewUserCancel').removeAttr('disabled');
                            } else {
                                angular.element('.addExUserSave').removeAttr('disabled');
                                angular.element('.addExUserCancel').removeAttr('disabled');
                            }
                        }
                        else
                        {
                            if (isNew) {
                                angular.element('.addNewUserSave').removeAttr('disabled');
                                angular.element('.addNewUserCancel').removeAttr('disabled');
                                Notification.success("User added successfully");
                            } else {
                                angular.element('.addExUserSave').removeAttr('disabled');
                                angular.element('.addExUserCancel').removeAttr('disabled');
                                Notification.success("Data saved successfully. These changes will be effective from next Login. User should logout and login to view the access change");
                            }
                            angular.element('#myModalNorm1').modal('hide');
                            angular.element('#myModalNorm').modal('hide');
                            loadDefaultData();
                        }
                        
                    },
                    function onError(response) {
                        if (isNew) {
                            angular.element('.addNewUserSave').removeAttr('disabled');
                            angular.element('.addNewUserCancel').removeAttr('disabled');
                            Notification.error("User Information Not Added.");
                        } else {
                            angular.element('.addExUserSave').removeAttr('disabled');
                            angular.element('.addExUserCancel').removeAttr('disabled');
                            Notification.error("User Information Not updated successfully");
                        }
                        console.log("Error while Deleting a User Data");
                    }
                );
        }

        //update Countries Data On Edit
        $scope.updateCountriesDataOnEdit = function (obj) {
           
            if (obj.user.userTypeDetails.DisplayName === 'District') {
                $scope.example14data = $scope.districtData[3];
                $scope.example14model = $scope.districtData[3];
                $scope.example13data = $scope.districtData[4];
                $scope.example13model = obj.user.districts;
            } else
            {
                angular.forEach($scope.example14model, function(rec) {
                    angular.forEach($scope.example14data, function (row) {
                        if (rec.CountryID === row.CountryID) {
                            rec.DisplayName = row.DisplayName;
                        }
                    });
                });
                obj['user'].countries = _.filter($scope.example14model, function (o) { return o.DisplayName !== ''; });
                obj['user'].districts = [];
                $scope.example13model = [];
                $scope.example13data = [];
            }
            //if ($scope.example14model.length != 0) {
            userManagementFactory.loadBuDetails($scope.example14model).then(
                            function onSuccess(response) {
                                console.log($scope.userOptionsData);
                                //$scope.example12model = [];
                                if ($scope.defaultData != undefined) {
                                    $scope.defaultData[2] = response.data[1];
                                }
                                //console.log($scope.userOptionsData);
                            });
           // }
            

        }

        //update Districts On Edit
        $scope.updateDistrictsOnEdit = function (obj) {
                angular.forEach($scope.example13model, function (row) {
                    angular.forEach($scope.example13data, function (arr) {
                        if (row.DistrictID === arr.DistrictID) {
                            row.DisplayName = arr.DisplayName;
                        }
                    });
                });
                obj['user'].districts = _.filter($scope.example13model, function (o) { return o.DisplayName !== ''; });
            
        }

        //update BUs On Edit
        $scope.updateBUsOnEdit = function (obj, isNewUser) {
            if (isNewUser) {
                angular.forEach($scope.example12model, function (row) {
                    angular.forEach($scope.defaultData[2], function (arr) {
                        if (row.BusinessUnitID === arr.BusinessUnitID) {
                            row.DisplayName = arr.DisplayName;
                        }
                    });
                });
            } else {
                angular.forEach($scope.example12model, function (row) {
                    angular.forEach($scope.userOptionsData[6], function (arr) {
                        if (row.BusinessUnitID === arr.BusinessUnitID) {
                            row.DisplayName = arr.DisplayName;
                        }
                    });
                });
                
            }
            
            
            obj['user'].BusinessUnitDetails = _.filter($scope.example12model, function (o) { return o.DisplayName !== ''; });

            if (isNewUser) {
                //  if (obj.user.BusinessUnitDetails.length == 5) 
                if ($scope.example12model.length === $scope.defaultData[2].length) {
                    $scope.addObj['user'].BUAccess = true;
                }
                else {
                    $scope.addObj['user'].BUAccess = false;
                }
            }
            else {
                // if (obj.user.BusinessUnitDetails.length == 5) {
                if ($scope.userOptionsData[4].length === $scope.example12model.length) {
                    $scope.editObj['user'].BUAccess = true;
                }
                else
                    $scope.editObj['user'].BUAccess = false;
               
            }
           
        }
        
        //update Partners On Edit
        $scope.updatePartnersOnEdit = function (obj, isNewUser) {
            if (isNewUser) {
                angular.forEach($scope.example11model, function (row) {
                    angular.forEach($scope.defaultData[4], function (arr) {
                        if (row.PartnerTypeID === arr.PartnerTypeID) {
                            row.DisplayName = arr.DisplayName;
                        }
                    });
                });

            } else {
                angular.forEach($scope.example11model, function (row) {
                    angular.forEach($scope.userOptionsData[7], function (arr) {
                        if (row.PartnerTypeID === arr.PartnerTypeID) {
                            row.DisplayName = arr.DisplayName;
                        }
                    });
                });
            }
            
            
            obj['user'].partnerType = _.filter($scope.example11model, function (o) { return o.DisplayName !== ''; });
        }

        //update SubRegion Data On Edit
        $scope.updateSubRegionDataOnEdit = function (obj, IsNewUser) {
            if (IsNewUser) {
                obj.user.countries = [];
                obj.user.districts = [];
                $scope.example14data = [];
                //$scope.example14model = [];
            }
           // $scope.gioSelectedValues = [];
            if ($scope.subRegionSelectedValues.length != 0) {

                let seletedSubRegiondata = [];

                angular.forEach($scope.subRegionSelectedValues, function (rec) {
                    angular.forEach($scope.exampleCountrydata, function (row) {
                        if (rec.GeoID === row.GeoID) {
                            rec.DisplayName = row.DisplayName;
                            seletedSubRegiondata.push({
                                'GeoID': rec.GeoID,
                                'DisplayName': row.DisplayName
                            });
                        }
                    });
                });
                obj.user.GeoDetails = seletedSubRegiondata;
                if (obj.user.userTypeDetails.DisplayName === 'Geo') {
                    angular.element('#example40 button.dropdown-toggle').attr("disabled", "disabled");
                    angular.element('#example39 button.dropdown-toggle').attr("disabled", "disabled");
                    angular.element('#example93 button.dropdown-toggle').attr("disabled", "disabled");
                    angular.element('#example94 button.dropdown-toggle').attr("disabled", "disabled");
                    // angular.element('#example95 button.dropdown-toggle').attr("disabled", "disabled");
                    obj.user.countries = [];
                    obj.user.districts = [];
                    //accessObj.user.GeoDetails = obj;
                    userManagementFactory.loadBUForGeoLevelUser(seletedSubRegiondata, IsNewUser).then(
                           function onSuccess(response) {
                               var flag = false;
                               if (IsNewUser == true) {
                                   $scope.defaultData[2] = null;
                                   $scope.defaultData[2] = response.data[1];
                                   //$scope.example12model = response.data[1];
                                   $scope.example12model = [];
                                   angular.forEach($scope.defaultData[2], function (SelectedBUs) {
                                       $scope.example12model.push(SelectedBUs);
                                   });
                               }
                               else {
                                   $scope.userOptionsData[4] = response.data[1];
                                   var newBUs = [];
                                   angular.forEach($scope.editObj.user.BusinessUnitDetails, function (SelectedBUs, index) {
                                       angular.forEach($scope.userOptionsData[4], function (BUs, index1) {
                                           if (BUs.DisplayName == SelectedBUs.DisplayName) {
                                               newBUs.push(BUs)
                                           }
                                       })
                                   })
                                   $scope.editObj.user.BusinessUnitDetails = newBUs;
                                   $scope.example12model = $scope.editObj.user.BusinessUnitDetails;;
                               }
                           },
                           function onError(response) {
                               console.log("Error while loading BU");
                           }
                       );
                }
                else {
                    $scope.example14data = [];
                    userManagementFactory.loadCountryDetails(seletedSubRegiondata).then(
                            function onSuccess(response) {
                                angular.element('#example40 button.dropdown-toggle').removeAttr('disabled');
                                angular.element('#example93 button.dropdown-toggle').removeAttr('disabled');
                                $scope.example14data = response.data[0];
                                //angular.forEach($scope.DefaultGeo, function (Geo) {
                                //    delete Geo['UserGeoID'];
                                //});
                                //if (JSON.stringify($scope.DefaultGeo) != JSON.stringify(seletedSubRegiondata)) {
                                //    //$scope.example14model = [];
                                //}
                                var countrymodel = [];
                                angular.forEach($scope.example14data, function (Countries) {
                                    angular.forEach($scope.example14model, function (SelectedCountries) {
                                        if (SelectedCountries.CountryID == Countries.CountryID){
                                            countrymodel.push(SelectedCountries);
                                        }
                                    });
                                });
                                $scope.example14model = countrymodel;
                            },
                            function onError(response) {
                                console.log("Error while loading countrty");
                            }
                        );

                    userManagementFactory.loadBUForGeoLevelUser(seletedSubRegiondata, IsNewUser).then(
                          function onSuccess(response) {
                              var flag = false;
                              if (IsNewUser == true) {
                                  $scope.defaultData[2] = null;
                                  $scope.defaultData[2] = response.data[1];
                                  $scope.example12model = response.data[1];
                              }
                              else {
                                    $scope.userOptionsData[4] = response.data[1];
                                    var newBUs = [];
                                    angular.forEach($scope.editObj.user.BusinessUnitDetails, function (SelectedBUs, index) {
                                        angular.forEach($scope.userOptionsData[4], function (BUs, index1) {
                                            if (BUs.DisplayName == SelectedBUs.DisplayName) {
                                                newBUs.push(BUs)
                                            }
                                        })
                                    })
                                    $scope.editObj.user.BusinessUnitDetails = newBUs;
                                    $scope.example12model = $scope.editObj.user.BusinessUnitDetails;
                                }
                            },
                            function onError(response) {
                                console.log("Error while loading BU");
                            }
                      );
                }
            }
            else {
                $scope.example14data = [];
                $scope.example14model = [];
            }
        }

    }
})();