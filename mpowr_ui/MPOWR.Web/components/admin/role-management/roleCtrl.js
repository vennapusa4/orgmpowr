// Role controller
// Created By : Nikhil
// Created date: 23 Mar 2017
(function () {
    'use strict';
    angular.module("hpe").controller("roleCtrl", roleCtrlFn)
    function roleCtrlFn($scope, $http, $state, Notification, $localStorage, $rootScope) {
        //ngInject
        $scope.shortNameAdded = false;
        $scope.roleNameAdded = false;
        $scope.partnerTypeDisplay = false;
        $scope.role = {};
        $scope.partnerTypeArray = [];
        var RoleDetailsCopyCheck = {};

        //Code to Get Roles

        $http.get($rootScope.api + 'RoleFeatureMgnt/GetRoles').success(function (response) {
            $scope.channelStrgyRole = response;
            $scope.selectedItem = $scope.channelStrgyRole[0];
            $scope.selectedRole($scope.selectedItem.RoleID)
        });
       
        // Fetching Role details
        $scope.selectedRole = function (roleId) {
            

            $scope.roleId = roleId;
            
            $scope.partnerTypeDisplay = true;
            $http.get($rootScope.api + 'RoleFeatureMgnt/GetRoleFeatureActionActivity?roleID=' + roleId).success(function (response) {
  
                var count = response.RoleDetails[0].Feature.length;
                for (var j = 0; j < count; j++)
                    {
                        if (response.RoleDetails[0].Feature[j].options != null) {

                            if (response.RoleDetails[0].Feature[j].FeatureID == 2) {
                                delete response.RoleDetails[0].Feature[j].options[1] //removing Historical export option
                                response.RoleDetails[0].Feature[j].options.length = 1;
                            }

                            if (response.RoleDetails[0].Feature[j].FeatureID == 5) {
                                delete response.RoleDetails[0].Feature[j].options[1] //removing Portfolio summary option
                                response.RoleDetails[0].Feature[j].options.length = 1;
                            }
                        }
                 }
 
                response.RoleDetails[0].Feature.splice(count, 1);
                $scope.channelStrgyRoleFeatures = response.RoleDetails[0].Feature; //slice used to exclude exception review
                $scope.channelStrgyRoleFeatures.sort(function (a, b) {
                    return a.SortOrder - b.SortOrder
                })
                $scope.channelStrgyRolePartnerType = response.RoleDetails[0].PartnerType;
                $scope.partnerTypeArray = [];
                //if ($scope.partnerTypeArray.length <= 0) {
                    for (var i = 0; i < $scope.channelStrgyRolePartnerType.length; i++) {
                        $scope.partnerTypeArray.push({ "partnerTypeId": $scope.channelStrgyRolePartnerType[i].PartnerTypeID, "partnerName": $scope.channelStrgyRolePartnerType[i].PartnerTypeName, "partnerTypeStatus": $scope.channelStrgyRolePartnerType[i].RPT[0].IsChecked });
                    };
                //};
            });
        };

        // Code to save the Roles
        $scope.save = function () {
            var RoleDetailsFeaturesArray = []
            var RoleDetailsFeatures = {};
            var RoleDetailsPartnerArray = []
            var RoleDetailsPartner = {};
            for (var i = 0; i < $scope.channelStrgyRoleFeatures.length; i++) {
                for (var j = 0; j < $scope.channelStrgyRoleFeatures[i].options.length; j++) {
                    RoleDetailsFeatures = { "FeatureID": $scope.channelStrgyRoleFeatures[i].FeatureID, "FeatureActionID": $scope.channelStrgyRoleFeatures[i].options[j].FeatureActionID, "FeatureActionIsChecked": $scope.channelStrgyRoleFeatures[i].options[j].IsChecked }
                    RoleDetailsFeaturesArray.push(RoleDetailsFeatures);
                }
            }

            for (var i = 0; i < $scope.partnerTypeArray.length; i++) {
                RoleDetailsPartner = { "PartnerTypeID": $scope.partnerTypeArray[i].partnerTypeId, "PartnerTypeIsChecked": $scope.partnerTypeArray[i].partnerTypeStatus }
                RoleDetailsPartnerArray.push(RoleDetailsPartner);
            }
            var RoleDetails = {

                "RoleUser": [{ "RoleID": $scope.roleId, "UserID": $localStorage.user.UserID }], "RolePartnerType": RoleDetailsPartnerArray, "RoleFeatureActivity": RoleDetailsFeaturesArray
            }

            if (!angular.equals(RoleDetails, RoleDetailsCopyCheck)) {
                RoleDetailsCopyCheck = RoleDetails;
                $http.post($rootScope.api + '/RoleFeatureMgnt/AddUpdateRoleFeature', angular.toJson(RoleDetails)).success(function (response) {
                    Notification.success('Data saved successfully. These changes will be effective from next Login. User should logout and login to view the access change!');
                });
            };
        };

        //Creating New Role
        $scope.newRole = function () {
           
            if (($scope.role.newShortName == undefined || $scope.role.newShortName == "" || $scope.role.newRoleName == undefined || $scope.role.newRoleName == "")) {
                if ($scope.role.newShortName == undefined || $scope.role.newShortName == "")
                    $scope.shortNameAdded = true;
                if ($scope.role.newRoleName == undefined || $scope.role.newRoleName == "")
                    $scope.roleNameAdded = true;
            }
            else {
                var newRole = { "ShortName": $scope.role.newShortName, "RoleName": $scope.role.newRoleName, "UserID": $localStorage.user.UserID }
                $http.post($rootScope.api + 'RoleFeatureMgnt/CreateNewRole', angular.toJson(newRole)).success(function (response) {
                    if (response == "Record is not inserted because Role is already exist")
                        Notification.warning('Record is not inserted because Role is already exist');
                    else {
                        var newRoleArr = {
                            "RoleID": "",
                            "DisplayName": $scope.role.newRoleName
                        }
                        angular.element('#saveClose').click();
                        Notification.success('Role saved successfully!');
                        $state.reload();
                        }
                })
                angular.element('div').removeClass('modal-backdrop');
                
            }
        }

        // Validation to create new Role
        $scope.checkRoleName = function () {
            if ($scope.role.newRoleName == undefined || $scope.role.newRoleName == "")
                $scope.roleNameAdded = true;
            else
                $scope.roleNameAdded = false;
        }
        $scope.checkShortName = function () {
            if ($scope.role.newShortName == undefined || $scope.role.newShortName == "")
                $scope.shortNameAdded = true;
            else
                $scope.shortNameAdded = false;
        }

        //reset input data on click of close button in model popup
        $scope.reset = function(form){
            $scope.role.newShortName = null;
            $scope.role.newRoleName = null;
            form.$setPristine();
            form.$setUntouched();
        }

        $scope.switch = function (option, index, subindex) {
            if (index > 0) {
                if ($scope.channelStrgyRoleFeatures[index].options[0].IsChecked == false && option.FeatureActionDisplayName != 'View' && option.IsChecked) {
                    $scope.channelStrgyRoleFeatures[index].options[0].IsChecked = true;
                }

                if (option.FeatureActionDisplayName == 'View' && !option.IsChecked) {
                    angular.forEach($scope.channelStrgyRoleFeatures[index].options, function (action, key) {

                        $scope.channelStrgyRoleFeatures[index].options[key].IsChecked = false;
                    });

                }
            }
        }

    };
})();