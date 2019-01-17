// Role controller
// Created By : Nikhil
// Created date: 24 Mar 2017
(function () {
    'use strict';
    angular.module("hpe").controller('setMilestoneCtrl', setMilestoneCtrlFn)
    function setMilestoneCtrlFn($scope, $http, $state, $timeout, $rootScope, Notification, setMilestoneFactory) {

        setMilestoneFactory.getRoles($scope);

        getData();
        function getData(){
            $http.get($rootScope.api + 'SetModelParameters/GetRegionsandYears').success(function (response) {
                $scope.regions = response.RegionFinancialYear[0].Regions;
                $scope.years = response.RegionFinancialYear[0].FinancialYear;
                $scope.activeMenu = $scope.regions[0].Region.RegionId;

                $scope.activeDummyMenu = $scope.regions[0]
                $scope.errorValue = false;
                
                var fYear = JSON.parse(localStorage['hpe-user']).FinancialYear;
                angular.forEach($scope.years, function (obj) {
                    if (obj.year.ShortName === fYear) {
                        $scope.selectedYear = obj;
                        $scope.selectedYearForCopy = $scope.selectedYear;
                    }
                })

                //$scope.selectedYear = $scope.years[0];
                

                $scope.setActive(
                    $scope.activeDummyMenu.Region.RegionId,
                    $scope.regions[0].Region.RegionId,
                    $scope.selectedYear.year.FinancialYearID
                );

            });
        }


        $scope.setActive = function (menuItem, RegionId, FinancialYearId) {
            $scope.activeMenu = menuItem;//.Region.RegionId;

            $scope.activeDummyMenu = menuItem;
            $scope.api_url = $rootScope.api + "SetModelParameters/GetExpectedProductivity";

            $scope.RegionId = RegionId;
            $scope.FinancialYearId = FinancialYearId;

            setMilestoneFactory.getMilestone($scope.FinancialYearId, $scope.activeMenu,$scope)
        }
        
        $scope.getMileStoneDetails = function (FinancialYearId, RegionId) {
            
            
        };

        //Fake the datepicker click while click on icon
        

        $scope.newYearSelected = function (year) {
            $scope.selectedYear = year;
            var requestRegionandyearData = {
                "FinancialYearDetails": $scope.selectedYear,
                "RegionDetails": $scope.selectedRegion
            }
            $scope.getMileStoneDetails(requestRegionandyearData);
        }
        $scope.getRegionDetails = function (region) {
            $scope.selectedRegion = region;
            var requestRegionandyearData = {
                "FinancialYearDetails": $scope.selectedYear,
                "RegionDetails": $scope.selectedRegion
            }
            $scope.getMileStoneDetails(requestRegionandyearData);
        }

        $scope.addNewMilestone = function (form) {
            var obj = {
                Name:               $scope.newMilestone,
                RegionId:           $scope.activeMenu,
                FinancialYearID:    $scope.selectedYear.year.FinancialYearID
            }

            $http.post($rootScope.api + 'SetMileStone/CreateMileStoneDetails',obj).then(function(resp){
                
                Notification.success({message: '<i class="fa fa-check" aria-hidden="true"></i> Milestone created successfully!'});
                $('.modal').modal('hide');
                $scope.setActive(obj.RegionId,obj.RegionId,obj.FinancialYearID);
                $scope.reset(form);
            })

            
        };

        $scope.fetchRoleList = function(){  
            $http.get($rootScope.api + '').success(function (response) {

            });
        };

        $scope.copyYearData = function (milestones,year) {
            
            for(var i=0;i<milestones.length;i++){
                milestones[i].FinancialYearId = year.year.FinancialYearID
            }
            $scope.save(milestones,'copy');
            $('.modal').modal('hide');
        };



        $scope.save = function (milestones,operation) {
            $http.put($rootScope.api + 'SetMileStone/SetMileStoneDetails', milestones).success(function (response) {
                if(operation == 'save')
                    Notification.success({message: '<i class="fa fa-check" aria-hidden="true"></i> Milestone details saved successfully!'});
                else if(operation == 'copy')
                    Notification.success({ message: '<i class="fa fa-check" aria-hidden="true"></i> Milestone details copied successfully!' });

            });
        };
        
        $scope.reset = function(form){
            $scope.newMilestone = "";
            form.$setPristine();
            form.$setUntouched();
        }

        $scope.validate = function () {
            var err1,err2;
            err1=err2=false;
            try {
                $(".send-to").each(function () {
                    var arr = $(this).attr("class").split(" ");
                    var last = arr[arr.length - 1];
                    if (last == 'has-error' || last == 'has-empty') {
                        err1 = true;
                    }
                });
                //added piece of code for making copyto properties required
                $(".copy-to").each(function () {
                    var arr = $(this).attr("class").split(" ");
                    var last = arr[arr.length - 1];
                    if (last == 'has-error' || last == 'has-empty') {
                        err1 = true;
                    }
                });
                return err1 || err2;
                
            } catch (e) {
                return false;
            }

        }
        //added code for getting datepicker on click of date icon
        $scope.datepicker = function (id) {
            var ref = '#milestone_' + id;           
            $timeout(function () {
                $(ref).trigger("click");                
            }, 0, false, ref);
        }
        //added code for comparing sentTo and copyTo Selections
        $scope.$watch('milestones', function (newValue, oldValue, scope) {
            if (newValue && oldValue && newValue.length > 0 && oldValue.length > 0) {
                angular.forEach(newValue, function (obj, index) {
                    obj.flag = false;
                    angular.forEach(obj.CopyTo, function (object) {
                        obj.flag = obj.flag === false ? _.some(obj.SendTo, { RoleID: object.RoleID }) : obj.flag;
                    })
                })
            } 
        },true);       
    };
})();