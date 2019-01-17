/*
Budget Factory: All the budget related functionality are written here
Created by: Aamin Khan
Created at: 25/01/2017
*/
(function () {
    'use strict';
    angular.module('hpe').factory('budgetFactory', function ($http,$rootScope,$localStorage,$sessionStorage) {
        var budgetFactory = {

            BuObj: {"FinancialYearID":0,"FinancialYear":"FY17 2H","PartnerTypeID":1,"PartnerName":"Distributor","CountryID":3,"CountryName":"ANTIGUA AND BARBUDA","BuUnits":[{"BUBudgetID":2,"BusinessUnitID":1,"TotalMDF":2000,"CountryReserveMDF":1000,"BaselineMDF":500,"BusinessUnitName":"DC Networking","ProgramMDF":500,"CarveProjects":[{"ProgramMDFID":1,"ProjectName":" Cloud","ProjectMDF":300,"Flag":"NU"},{"ProgramMDFID":6,"ProjectName":"GPC","ProjectMDF":100,"Flag":"NU"},{"ProgramMDFID":9,"ProjectName":"Discover","ProjectMDF":100,"Flag":"NU"}]}]},
            budgets: {},
            setBudget: function (budget) {
                this.budget = budget;
            },
            getBudget: function () {
                var curObj = this;
                var promise = 
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'BUBudgets/GetBUBudgets?PartnerTypeID='+$sessionStorage.resellerPartner.partner.PartnerTypeID+"&CountryId="+$sessionStorage.resellerPartner.obj.id
                }).then(function(resp){
                    return resp;
                });
                return promise;
            },

            generateJSON: function(obj){
                var newObj = {
                    FinancialYearID: obj[0].FinancialYearID,
                    FinancialYear: obj[0].FinancialYear,
                    PartnerTypeID: obj[0].PartnerTypeID,
                    PartnerName: obj[0].PartnerName,
                    CountryID: obj[0].CountryID,
                    CountryName: obj[0].CountryName,
                    BuUnits:[]
                }
                for(var i=0;i<obj.length;i++){
                    newObj.BuUnits.push(obj[i].BuUnits);
                }
                var afterCalculationObj = this.performInitCalculation(newObj);
                return afterCalculationObj;
            },
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
                return obj;
            },
            toFormat: function (val) {
                if (val == "") {
                    return 0;
                }
                if (typeof val == 'string') {
                    val = val.replace(/,/g, '');
                }
                return parseInt(val);
            },
            toBudgetFormat: function (budgetObj) {
                budgetObj.ProgramMDF = budgetFactory.toFormat(budgetObj.ProgramMDF);
                budgetObj.TotalMDF = budgetFactory.toFormat(budgetObj.TotalMDF);
                budgetObj.CountryReserveMDF = budgetFactory.toFormat(budgetObj.CountryReserveMDF);
                budgetObj.BaselineMDF = budgetFactory.toFormat(budgetObj.BaselineMDF);
                return budgetObj;
            },
            formatPercentage: function (data) {
                var val = parseFloat(data.toFixed(1)) || 0;
                val = isFinite(val) ? val : 0;
                return val;
            },

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
            }
        };

        return budgetFactory;
    })
})();