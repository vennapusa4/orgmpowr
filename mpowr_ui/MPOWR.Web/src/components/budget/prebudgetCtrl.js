/*
Pre Budget controller: The user selects the country and partner
Created by: Aamin Khan
Created at: 19/01/2017
*/
(function () {
    'use strict';
	angular.module("hpe").controller("prebudgetCtrl", prebudgetCtrlFn);    

	function prebudgetCtrlFn($scope,$rootScope,countryParnerFactoy,$log,$timeout,$sessionStorage,$state,authService){
		//ngInject
		authService.checkUser();
		var usId = 138;
		countryParnerFactoy.getData().then(
			function onSuccess(response) {
		    // Handle success
		    var data = response.data;
		    var status = response.status;
		    var statusText = response.statusText;
		    var headers = response.headers;
		    var config = response.config;
		    
		    $scope.countries = data.Countries;
			$scope.partners = data.Partners;
			$scope.districts = data.Districts;

			$scope.selectedCountry = $scope.countries[0];
			$scope.selectedPartner = $scope.partners[0];
			$scope.selectedDistrict = $scope.districts[0];

		  }, 

		  function onError(response) {
		    
		  })

		

		$scope.showDistrict = false;

		$scope.changeCountryOrDistrict = function(){
			if($scope.selectedCountry.CountryID == usId){
				$scope.partners = [
					{PartnerTypeID: 1, PartnerName: "Distributor"}
				];
				for(var i=0;i<$scope.districts.length;i++){
					$scope.partners.push(
						{PartnerTypeID: i+2,PartnerName: "Reseler-"+$scope.districts[i].DistrictName}
					)
				}
			}else{
				$scope.partners = [
					{
				      "PartnerTypeID": 1,
				      "PartnerName": "Distributor"
				    },
				    {
				      "PartnerTypeID": 2,
				      "PartnerName": "Reseller"
				    }
				]
			}
		}

		$scope.procced = function () {
		    var obj = {};
		    var country = "Country";
		    if ($scope.showDistrict) {
		        obj.id = $scope.selectedDistrict.DistrictID;
		        obj.name = $scope.selectedDistrict.DistrictName;
		        country = "District";
		    } else {
		        obj.id = $scope.selectedCountry.CountryID;
                obj.name = $scope.selectedCountry.CountryName;
                country = "Country";
		    }

			$sessionStorage.resellerPartner = {
				obj: obj,
				partner: $scope.selectedPartner,
				countryOrDistrict: country
			}

			$state.go('budget.allocation')
		}


	}



})();

