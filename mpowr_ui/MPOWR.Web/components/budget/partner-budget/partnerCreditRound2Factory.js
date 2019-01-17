/*
Historical Factory
Created by: Aamin Khan
Created at: 06/02/2017
*/
(function () {
    'use strict';
	angular.module("hpe").factory("partnerCreditRound2Factory", partnerCreditRound2FactoryFn);  

	function partnerCreditRound2FactoryFn($rootScope,$http,HistoricalFactory,modelParameterFactory,$sessionStorage,$timeout,$localStorage){
		//ngInject
		return{
			getData: function($scope){
				//$scope.partners = [{"PartnerID":12,"ParnterName":"COMPUTACELL","BusinessUnits":[{"Name":"DCN Compute","MDFBySellout":85,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"","Sellout":361254},{"Name":"Aruba","MDFBySellout":56,"MDF":120000,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Aruba is onf fire","Sellout":100000},{"Name":"Storage","MDFBySellout":65,"MDF":542125,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Nothing","Sellout":500000},{"Name":"DCN Compute","MDFBySellout":0.2,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"Nothing","Sellout":12542}]},{"PartnerID":12,"ParnterName":"COMPUECENTER RESELLER","BusinessUnits":[{"Name":"DCN Compute","MDFBySellout":85,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"","Sellout":361254},{"Name":"Aruba","MDFBySellout":56,"MDF":120000,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Aruba is onf fire","Sellout":100000},{"Name":"Storage","MDFBySellout":65,"MDF":542125,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Nothing","Sellout":500000},{"Name":"DCN Compute","MDFBySellout":0.2,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"Nothing","Sellout":12542}]},{"PartnerID":12,"ParnterName":"COW LIMITTED","BusinessUnits":[{"Name":"DCN Compute","MDFBySellout":85,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"","Sellout":361254},{"Name":"Aruba","MDFBySellout":56,"MDF":120000,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Aruba is onf fire","Sellout":100000},{"Name":"Storage","MDFBySellout":65,"MDF":542125,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Nothing","Sellout":500000},{"Name":"DCN Compute","MDFBySellout":0.2,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"Nothing","Sellout":12542}]},{"PartnerID":12,"ParnterName":"SPECIALIST COMPUTER CENTER","BusinessUnits":[{"Name":"DCN Compute","MDFBySellout":85,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"","Sellout":361254},{"Name":"Aruba","MDFBySellout":56,"MDF":120000,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Aruba is onf fire","Sellout":100000},{"Name":"Storage","MDFBySellout":65,"MDF":542125,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Nothing","Sellout":500000},{"Name":"DCN Compute","MDFBySellout":0.2,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"Nothing","Sellout":12542}]},{"PartnerID":12,"ParnterName":"SOFTCAT LTD","BusinessUnits":[{"Name":"DCN Compute","MDFBySellout":85,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"","Sellout":361254},{"Name":"Aruba","MDFBySellout":56,"MDF":120000,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Aruba is onf fire","Sellout":100000},{"Name":"Storage","MDFBySellout":65,"MDF":542125,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Nothing","Sellout":500000},{"Name":"DCN Compute","MDFBySellout":0.2,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"Nothing","Sellout":12542}]},{"PartnerID":12,"ParnterName":"INSIGHT DIRECOR UK LTD","BusinessUnits":[{"Name":"DCN Compute","MDFBySellout":85,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"","Sellout":361254},{"Name":"Aruba","MDFBySellout":56,"MDF":120000,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Aruba is onf fire","Sellout":100000},{"Name":"Storage","MDFBySellout":65,"MDF":542125,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Nothing","Sellout":500000},{"Name":"DCN Compute","MDFBySellout":0.2,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"Nothing","Sellout":12542}]},{"PartnerID":12,"ParnterName":"OCSL HEADQUATERS","BusinessUnits":[{"Name":"DCN Compute","MDFBySellout":85,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"","Sellout":361254},{"Name":"Aruba","MDFBySellout":56,"MDF":120000,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Aruba is onf fire","Sellout":100000},{"Name":"Storage","MDFBySellout":65,"MDF":542125,"AdditionalCalculatedMDF":4568,"AdditionalMDF":0,"Reason":"Nothing","Sellout":500000},{"Name":"DCN Compute","MDFBySellout":0.2,"MDF":403564,"AdditionalCalculatedMDF":4568,"AdditionalMDF":1000,"Reason":"Nothing","Sellout":12542}]}];
			    var versionId = $sessionStorage.currentVersion.VersionID;
				var CountryID = $sessionStorage.resellerPartner.obj.id;
				var PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
				$http({
                    method: 'GET',
                    url: $rootScope.api + 'PartnerBudget/GetRound2Details?CountryID=' + CountryID + '&PartnerTypeID=' + PartnerTypeID + "&VersionID=" + versionId + '&FinancialYear=FY17_2H&&DistrictID=18&FocusAreaID=1'
                }).then(function(resp){
                	var group = _.groupBy(resp.data,'Partner_Name')
                	
                	var arr = [];
                	angular.forEach(group,function(businessUnis,parner){
                		var bu = [];

                		for(var i=0;i<businessUnis.length;i++){
                			var buObj = businessUnis[i].BU[0];
                			bu.push(buObj);
                		}
                		var obj = angular.copy(businessUnis[0]);
                		delete obj.BU;
                		obj.BU = bu;
                		arr.push(obj)
                	})

                	$scope.partners= arr;
                    $timeout(function(){
                        $(".graph-row").hide()
                    },500)
                });
			},
            getFocusArea: function($scope,current){
                
                //http://localhost:54961/api/PartnerBudget/GetFocusAreaDetails
                var CountryID = $sessionStorage.resellerPartner.obj.id;
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'PartnerBudget/GetFocusAreaDetails?CountryID='+CountryID,
                }).then(function(resp){
                    $scope.options = resp.data;
                    $scope.selected = $scope.options[typeof current == 'undefined' ? 0 : current-1];


                   /* $scope.selected = {c:'red'};
                    $scope.options = [{c:'red'}, {c:'blue'}, {c:'yellow'}, {c:'green'}];*/
                })
            },

            updateDataByRModel: function($scope){
                $('#mdf-loader').css('display', 'block');
                             
                var CountryID = $sessionStorage.resellerPartner.obj.id;
                var PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
                var FinancialYear = $sessionStorage.resellerPartner.FinencialYear
                var DistrictID = $sessionStorage.resellerPartner.district.DistrictID
                var versionId = $sessionStorage.currentVersion.VersionID;
                var curObj = this;
                var dataObj = {
                    //"CountryID":$sessionStorage.resellerPartner.obj.id,
                    //"PartnerTypeID": $sessionStorage.resellerPartner.partner.PartnerTypeID,
                    //"District_ID": $sessionStorage.resellerPartner.district.DistrictID,
                    //"FinancialYearID": $localStorage.user.FinancialYearID,
                    "FocusedAreaID": $scope.selected.FocusedAreaID,
                    "Versionid": $sessionStorage.currentVersion.VersionID,
                    "UserID":$localStorage.user.UserID
                }
                $http({
                    method: 'POST',
                    url: $rootScope.api+ "PBRoundtwo/ApplyFilter",
                    data: dataObj
                }).then(function(resp){
                    $('#mdf-loader').css('display', 'none');
                    
                })
            },

            getRemainingMDF: function($scope){
                var CountryID = $sessionStorage.resellerPartner.obj.id;
                var PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
                var FinancialYear = $sessionStorage.resellerPartner.FinencialYear
                var DistrictID = $sessionStorage.resellerPartner.district.DistrictID
                var versionId = $sessionStorage.currentVersion.VersionID;
                var curObj = this;
                var dataObj = {
                    "CountryID":$sessionStorage.resellerPartner.obj.id,
                    "PartnerTypeID": $sessionStorage.resellerPartner.partner.PartnerTypeID,
                    "DistrictID": $sessionStorage.resellerPartner.district.DistrictID,
                    "Financialyear": $localStorage.user.FinancialYearID,
                    "Versionid":$sessionStorage.currentVersion.VersionID,
                    "FocusedAreaID": 1
                }

                $http({
                    method: 'POST',
                    url: $rootScope.api+ "PartnerBudget/GetRemainingBudget",
                    data: dataObj
                }).then(function(resp){
                    $scope.remaining = resp.data;
                    
                    /*var withID = [];
                    for(var i=0;i<$scope.remaining.length;i++){

                        var data = _.find($localStorage.businessUnits,function(o){
                            return o.Name == $scope.remaining[i].BusinessUnit;
                        });
                        try{
                            var nx = angular.copy($scope.remaining[i]);
                            nx.ID = data.ID;
                            withID.push(nx);
                        }catch(e){

                        }
                    }
                    $scope.withID = withID;*/
                })
            }
			
			
		}
	}

})();