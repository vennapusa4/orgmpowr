(function () {
    'use strict';
    angular.module("hpe").controller('buManagementCtrl', buManagementCtrlFn)
    function buManagementCtrlFn($scope, $http, $state, $timeout, $rootScope, $localStorage, Notification) {
        var Template = '<p style="padding-left: 25%;" class="checkbox-text admin"><input type="checkbox" id="{{row.uid}}--{{col.uid}}" ng-checked="row.entity.IsCountryLevel" ng-click="row.entity.IsCountryLevel = !row.entity.IsCountryLevel;grid.appScope.isDirty()"> <label class ="check" for="{{row.uid}}--{{col.uid}}"><span class ="check-label"></span></label></p>';
        var AllocationTemplate = '<p style="padding-left: 30%;" class="checkbox-text admin"> <input type="checkbox" id="{{row.uid}}--{{col.uid}}" ng-checked="row.entity.IsOverAllocation" ng-click="row.entity.IsOverAllocation = !row.entity.IsOverAllocation;grid.appScope.isDirty()"> <label class ="check" for="{{row.uid}}--{{col.uid}}"><span class ="check-label"></span></label></p>';
        var columnDefs = [
         { field: 'GeoName',displayName: 'BU Name', width: '20%', enableHiding: false},
         { field: 'IsApplicable', width: '20%', displayName: 'Applicable', type: 'boolean', cellTemplate: Template, enableHiding: false },
         { field: 'IsOverAllocation', width: '15%', displayName: 'Over Allocation', type: 'boolean', cellTemplate: AllocationTemplate, enableHiding: false },
         { field: 'dummy', width: '45%', displayName: ' ', enableColumnMenu: false },
        ]
    
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableSorting: false,
            columnDefs: columnDefs
        };
        $http.get($rootScope.api + 'GeoSettings/GetBuLevelData').success(function (response) {
            $scope.Data = response;
            $scope.gridOptions.data = response;
        });
    
       
        $scope.SaveData = function (data) {
            $http.post($rootScope.api + 'GeoSettings/UpdateGeodata?User=' + $localStorage.user.UserID, data).success(function (response) {
                $rootScope.Saved = true;
                Notification.success({
                    message: "Saved successfully!" + $rootScope.closeNotify,
                    delay: null

                });
            }).error(function () {
                Notification.error({
                    message: "something went wrong!" + $rootScope.closeNotify,
                    delay: null
                });
            });
        }
        $scope.isDirty=function () {
            $rootScope.Saved = false;
        }
        
    };
    
})();