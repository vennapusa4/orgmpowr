/*
Budget Factory: All the budget related functionality are written here
Created by: Aamin Khan
Created at: 25/01/2017
*/
(function () {
    'use strict';
    angular.module('hpe').factory('budgetFactory', function ($http, $rootScope, $localStorage, $sessionStorage, $timeout) {

        var budgetFactory = {
            
            budgets: {},
            // setter method

            setBudget: function (budget) {
                this.budget = budget;
            },
            
           
            // getter method: get's the data from api
            getBudget: function ($scope) {
                var curObj = this;
                var countries;
                angular.forEach($sessionStorage.resellerPartner.obj.countries, function (rec) {
                    if (countries == undefined)
                        countries = rec.CountryID;
                    else
                        countries = ',' + rec.CountryID;
                });
                var promise = 
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'BUBudgets/GetBUBudgets?VersionID=' + $sessionStorage.currentVersion.VersionID + '&GeoID=' + $sessionStorage.resellerPartner.obj.geo.GeoID
                }).then(function (resp) {
                    $timeout(function () {
                        var currentBus = resp.data[0].BuUnits;
                        $localStorage.user.IsOverAllocation = resp.data[0].IsOverAllocation;
                        for (var i = 0; i < currentBus.length; i++) {
                            if (($localStorage.user.BUs.indexOf(currentBus[i].BusinessUnitID) > -1)) {
                            }
                            else {
                                $(".businessUnit_" + currentBus[i].BusinessUnitID + " input").attr("disabled", "disabled")
                                $(".businessUnit_" + currentBus[i].BusinessUnitID + " .close-button").attr("disabled", "disabled")
                            }
                        }
                    }, 500)
                    return resp;
                });
                return promise;
            },
            getModelParam: function () {
                var bool = true;
                var promise =
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'ModelParameter/GetModelParameterDefault?VersionID=' + $localStorage.selectedversion.VersionID + '&UserID=' + $localStorage.user.UserID + '&frmStep1=' + bool
                }).then(function (resp) {
                    $timeout(function () {
                        var currentBus = resp.data[1];
                        for (var i = 0; i < currentBus.length; i++) {
                            if (($localStorage.user.BUs.indexOf(currentBus[i].BusinessUnitID) > -1)) {
                            }
                            else {
                                //$(".businessUnit_" + currentBus[i].BusinessUnitID + " input").attr("disabled", "disabled")
                            }
                        }
                    }, 500)
                    return resp;
                });

                return promise;
            },
            getBudgetOnSelect: function ($scope, versionId, financialyearID) {
                var curObj = this;
                var countries;
                angular.forEach($sessionStorage.resellerPartner.obj.countries, function (rec) {
                    if (countries == undefined)
                        countries = rec.CountryID;
                    else
                        countries = ',' + rec.CountryID;
                });
                
                var promise =
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'BUBudgets/GetBUBudgets?VersionID=' + versionId + '&GeoID=' + $sessionStorage.resellerPartner.obj.geo.GeoID
                }).then(function (resp) {
                    $timeout(function () {
                        var currentBus = resp.data[0].BuUnits;
                        $localStorage.user.IsOverAllocation = resp.data[0].IsOverAllocation;
                        for (var i = 0; i < currentBus.length; i++) {
                            if (($localStorage.user.BUs.indexOf(currentBus[i].BusinessUnitID) > -1)) {                             
                            }
                            else {
                                $(".businessUnit_" + currentBus[i].BusinessUnitID + " input").attr("disabled", "disabled");
                                $(".businessUnit_" + currentBus[i].BusinessUnitID + " .close-button").attr("disabled", "disabled");
                            }                         
                        }   
                    },500)
                    return resp;
                });
                return promise;
            },
            //
            List : function () {
            },

            //    $scope.FinancialyearList = null;

            //    return $http({
            //        method: 'POST',
            //        url: $rootScope.api + 'BUBudgets/GetFinancialYear',
            //        data: $localStorage.user.FinancialYearID
            //    }).success(function (result) {
            //        $scope.FinancialyearList = result;
                   
            //    });
            //},

            // decorates the object
            generateJSON: function(obj){
                var newObj = {};
                newObj = obj[0];
                var afterCalculationObj = this.performInitCalculation(newObj);
                return afterCalculationObj;
            },

            // performs calcula
            performInitCalculation: function (obj) {
                var curObj = this;

                for(var i=0;i<obj.BuUnits.length;i++){ // BU Unit loop
                    obj.BuUnits[i].curvePer = curObj.formatPercentage((obj.BuUnits[i].ProgramMDF / obj.BuUnits[i].TotalMDF)*100);
                    obj.BuUnits[i].countryPer = curObj.formatPercentage((obj.BuUnits[i].CountryReserveMDF / obj.BuUnits[i].TotalMDF)*100);
                    obj.BuUnits[i].baslinePer = curObj.formatPercentage((obj.BuUnits[i].BaselineMDF / obj.BuUnits[i].TotalMDF)*100);
                    for(var j=0;j<obj.BuUnits[i].CarveProjects.length;j++){
                        obj.BuUnits[i].CarveProjects[j].per = curObj.formatPercentage((obj.BuUnits[i].CarveProjects[j].ProjectMDF/obj.BuUnits[i].ProgramMDF)*100)
                    }
                }
                $timeout(function(){
                    // to comma seperated format
                    $(".specialText").priceFormat();
                },100)
                return obj;
            },
            // converts comma seperated text back to number format
            toFormat: function (val) {
                if (val == "") {
                    return 0;
                }
                if (typeof val == 'string') {
                    val = val.replace(/,/g, ''); // remove commas
                    var regex = /(?=.)^\$?(([1-9][0-9]{0,2}(,[0-9]{3})*)|[0-9]+)?(\.[0-9]{1,2})?$/;
                    if (regex.test(val)) {
                        let arrayInput = val.split('');
                        if (isNaN(arrayInput[0])) {
                            arrayInput.shift();
                            val = arrayInput.join('');
                        }
                        //val = val.replace(/,/g, ''); // remove commas

                    }
                    else {
                        return false;
                    }
                    //val = val.replace(/,/g, ''); // remove commas
                }
                return parseInt(val);
            },

            // convert the budgetObj to the readable(int) format
            toBudgetFormat: function (budgetObj) {
                budgetObj.ProgramMDF = budgetFactory.toFormat(budgetObj.ProgramMDF);
                budgetObj.TotalMDF = budgetFactory.toFormat(budgetObj.TotalMDF);
                budgetObj.CountryReserveMDF = budgetFactory.toFormat(budgetObj.CountryReserveMDF);
                budgetObj.BaselineMDF = budgetFactory.toFormat(budgetObj.BaselineMDF);
                return budgetObj;
            },
            // converts infinity back to zero or returns value
            formatPercentage: function (data) {
                var val = Math.round(data.toFixed(1)) || 0;
                val = isFinite(val) ? val : 0;
                return val;
            },

            // thorughs data back to the api
            persist: function(obj){
                var promise = 
                $http({
                    method: 'POST',
                    url: $rootScope.api + 'BUBudgets/AddUpdateBUBudget',
                    data: obj
                }).then(function(resp){
                    return resp;
                });
                return promise;
            },

            // updates JSON depending on the data 
            updateJSON: function (obj) {
                var units = obj.BuUnits;
                for (var i = 0; i < units.length; i++) { // BU LOOP
                    var project = units[i].CarveProjects;
                    for (var j = 0; j < project.length; j++) {
                        if(project[j].Flag != 'DL'){
                            project[j].Flag = "NU";    
                        }
                    }
                }
                return obj;
            },
            expandRowOnValidationFailure: function(elem){
                var parentId  = (elem.attr("id").split("-"))[1];
                $("#budget-body-"+parentId).show(); //expand the div
                // change the orientation of dropdown  
                $("#budget-body-"+parentId).parent().find(".expand-btn")
                    .removeClass('fa-chevron-down')
                        .addClass('fa-chevron-up')

            },
            expandOnSummaryValidationFailure: function(index) {
                $("#budget-body-"+index).show(); //expand the div
                // change the orientation of dropdown  
                $("#budget-body-"+index).parent().find(".expand-btn")
                    .removeClass('fa-chevron-down')
                        .addClass('fa-chevron-up')
            },
            disablePaste: function(){
                /*$('input').bind("cut copy paste",function(e) {
                      e.preventDefault();
                });*/
            },

            /*Depricated - need to remove the method*/
            update: function ($http, $scope, $rootScope, $localStorage) {
            
                $http({
                    method: 'POST',
                    url: $rootScope.api + 'LastUpdated/inseretLastUpdated?userid=' + $localStorage.user.UserID + '&partnerid=' + $localStorage.PartnerTypeID

                }).then(function (resp) {

                });
            },
            // select s the text
            selectText: function(){

                $(".allocation-layout").on("click","input[type='text']", function () {
                   $(this).select();
                });
                $(".allocation-layout").on("click", "input[type='number']", function () {
                    $(this).select();
                });
                $(".allocation-layout").on("focus", "input[type='text']", function () {
                   $(this).select();
                });
                $(".allocation-layout").on("focus", "input[type='number']", function () {
                    $(this).select();
                });

            }


          
       

        };

        return budgetFactory;
    })
})();