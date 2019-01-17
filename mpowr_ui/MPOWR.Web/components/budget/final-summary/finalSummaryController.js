/*
Final Summary controller logics
Created by: Avinash Kumar
Created at: 20/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").controller("finalSummaryCtrl", controllerFunction);

    function controllerFunction($scope, $rootScope, $sessionStorage, Notification, summaryFactory, modelParameterFactory, $timeout, $localStorage, UtilityService, budgetFactory, roleMappingService, fsGraphFactory) {
        // ngInject
        $scope.role = roleMappingService.apply();
        $scope.toggleFlag = true;
        $scope.toggleFlagForMDF = true;
        $scope.toggleFlagForSelloutMDF = true;
        $scope.periodsApplied = false;
        $scope.preYear = $localStorage.previousYear.replace("_", "");//"Previous Year"

        // hide/show rows
        $scope.toogleCaretIcon = function (id, flag) {
            var element = angular.element('#' + id);
            if (flag) {
                element.addClass('fa-caret-down');
                element.removeClass('fa-caret-up');
            } else {
                element.removeClass('fa-caret-down');
                element.addClass('fa-caret-up');
            }
        }
        $scope.setNext = function () {
            $rootScope.next = true;
        }
        $scope.setPrev = function () {
            $rootScope.prev = true;
        }

        $scope.$sessionstorage = $sessionStorage;
        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                $scope.ctries = _.pluck(countryList, 'DisplayName').join(", ");
            }
        }
        $scope.DistrictID = parseInt((JSON.parse(sessionStorage["ngStorage-resellerPartner"])["district"].DistrictID === undefined || JSON.parse(sessionStorage["ngStorage-resellerPartner"])["district"].DistrictID.toString() === '0') ? '0' : JSON.parse(sessionStorage["ngStorage-resellerPartner"])["district"].DistrictID);
        /*Can be changes to*/
        //$scope.DistrictID = parseInt( $sessionStorage.resellerPartner.district.DistrictID === undefined || $sessionStorage.resellerPartner.district.DistrictID === '0' ? '0' : $sessionStorage.resellerPartner.district.DistrictID);
        var sessionData = $sessionStorage.resellerPartner;
        $scope.country = sessionData.obj.name;
        var reserveKey = sessionData.district.DistrictName ? ' - ' + sessionData.district.DistrictName : '';
        $scope.budget = sessionData.partner.PartnerName + reserveKey;
        $scope.membership = sessionData.membership.MembershipName;
        $scope.periods = 'yes';
        $scope.graphOne = true;
        var selectedType = 1;
        $scope.condition = true;
        $scope.FinancialYearID = $localStorage.user.FinancialYearID;
        $scope.versionId = $sessionStorage.currentVersion.VersionID;
        var countryId = sessionData.obj.id
            , partnerTypeId = sessionData.partner.PartnerTypeID;



        // load the chart
        var loadChartDataValues = function () {
            //Sales growth graph data
            $scope.Default_SalesGrowth_with_mdf = parseFloat($scope.gdata["Graphs"][0]["SalesGraph"][0].default_Growth_With_MDF.toFixed(1).replace(/"/g, ''));
            $scope.Default_SalesGrowth_without_mdf = parseFloat($scope.gdata["Graphs"][0]["SalesGraph"][0].default__Growth_Without_MDF.toFixed(1).replace(/"/g, ''));
            $scope.FSalesGrowth_with_mdf = parseFloat($scope.gdata["Graphs"][0]["SalesGraph"][0].final_Growth_With_MDF.toFixed(1).replace(/"/g, ''));
            $scope.FSalesGrowth_without_mdf = parseFloat($scope.gdata["Graphs"][0]["SalesGraph"][0].final_Growth_Without_MDF.toFixed(1).replace(/"/g, ''));

            $scope.max_of_array = Math.max.apply(Math, [$scope.Default_SalesGrowth_with_mdf, $scope.Default_SalesGrowth_without_mdf, $scope.FSalesGrowth_with_mdf, $scope.FSalesGrowth_without_mdf]);
            $scope.min_of_array = Math.min.apply(Math, [$scope.Default_SalesGrowth_with_mdf, $scope.Default_SalesGrowth_without_mdf, $scope.FSalesGrowth_with_mdf, $scope.FSalesGrowth_without_mdf]);
            if ($scope.max_of_array > 0) {
                $scope.max_of_array = $scope.max_of_array + (10 - ($scope.max_of_array % 10))
            } else {
                $scope.max_of_array = $scope.max_of_array - (10 + ($scope.max_of_array % 10))
            }

            if ($scope.min_of_array > 0) {
                $scope.min_of_array = $scope.min_of_array + (10 - ($scope.min_of_array % 10))
            } else {
                $scope.min_of_array = $scope.min_of_array - (10 + ($scope.min_of_array % 10))
            }

            //MDF/Sellout graph Data
            $scope.Default_MDFBYSellout_Percentage = parseFloat($scope.gdata["Graphs"][0]["MDF_SelloutGraph"][0].default_Sellout_Last_Period.toFixed(1).replace(/"/g, ''));
            $scope.MDFBYSellout_Percentage = parseFloat($scope.gdata["Graphs"][0]["MDF_SelloutGraph"][0].Final_Sellout_Last_Period.toFixed(1).replace(/"/g, ''));
            $scope.FinancialYear = $scope.gdata["Graphs"][0]["MDF_SelloutGraph"][0].financialyear.replace(/"/g, '');

            //Alignment of investment graph Data
            $scope.DAligned_Percentage = parseFloat($scope.gdata["Graphs"][0]["AlignedGraph"][0].default_Aligned.toFixed(1).replace(/"/g, ''));
            $scope.DMisAligned_Percentage = parseFloat($scope.gdata["Graphs"][0]["AlignedGraph"][0].default_MisAligned.toFixed(1).replace(/"/g, ''));
            $scope.FAligned_Percentage = parseFloat($scope.gdata["Graphs"][0]["AlignedGraph"][0].Final_Aligned.toFixed(1).replace(/"/g, ''));
            $scope.FMisAligned_Percentage = parseFloat($scope.gdata["Graphs"][0]["AlignedGraph"][0].Final_MisAligned.toFixed(1).replace(/"/g, ''));

            //Baseline MDF by membership tier
            $scope.Default_MDF_Platinum_Percentage = parseFloat($scope.gdata["Graphs"][0]["MemberShipTier"][0].default_Plat.toFixed(1).replace(/"/g, ''));
            $scope.Default_MDF_Gold_Percentage = parseFloat($scope.gdata["Graphs"][0]["MemberShipTier"][0].default_Gold.toFixed(1).replace(/"/g, ''));
            $scope.Default_MDF_SB_Percentage = parseFloat($scope.gdata["Graphs"][0]["MemberShipTier"][0].Default_SB.toFixed(1).replace(/"/g, ''));
            $scope.Final_MDF_Platinum_Percentage = parseFloat($scope.gdata["Graphs"][0]["MemberShipTier"][0].Final_Plat.toFixed(1).replace(/"/g, ''));
            $scope.Final_MDF_Gold_Percentage = parseFloat($scope.gdata["Graphs"][0]["MemberShipTier"][0].Final_Gold.toFixed(1).replace(/"/g, ''));
            $scope.Final_MDF_SB_Percentage = parseFloat($scope.gdata["Graphs"][0]["MemberShipTier"][0].Final_SB.toFixed(1).replace(/"/g, ''));





            $scope.MDF_Scatter_Array = [];
            $scope.Without_MDF_Scatter_Array = [];
           if ($scope.gdata["Graphs"][0]["PlottedGraph"]) {
                angular.forEach($scope.gdata["Graphs"][0]["PlottedGraph"], function (obj) {
                    if (obj.mdf === 0) {
                        var scObj = {
                            shape: 'circle', size: 2
                        }
                        scObj['x'] = obj.partnersize;
                        scObj['y'] = obj.projected_sellout_growth;
                        scObj['partnerName'] = obj.Partner_Name;
                        $scope.Without_MDF_Scatter_Array.push(scObj);
                    }
                    else {
                        var scObj = {
                            shape: 'circle', size: 2
                        }
                        scObj['x'] = obj.partnersize;
                        scObj['y'] = obj.projected_sellout_growth;
                        scObj['partnerName'] = obj.Partner_Name;
                        $scope.MDF_Scatter_Array.push(scObj);
                    }
                })

            }

            fsGraphFactory.loadSummaryCharts($scope);
        }

        //calls graph api to get the data
        var loadGraph = function () {
            var graphData = {
                Previous_Period_Year: $scope.periods == 'yes' ? 0 : 1,
                CountryID: sessionData.obj.id,
                District_ID: $scope.DistrictID,
                PartnerTypeID: sessionData.partner.PartnerTypeID,
                FinancialYearID: $scope.FinancialYearID,
                VersionID: $sessionStorage.currentVersion.VersionID,
                BusinessUnitID: 0
            }

            summaryFactory.getGraphData(graphData).then(
                    function onSuccess(response) {
                        $scope.gdata = response.data;
                        loadChartDataValues();
                    },
                    function onError(response) {
                        Notification.error({
                            message: response.data,
                            delay: null
                        });
                    }
            )
        }


        loadGraph();

        // color function for graphs



        // First method to execute
        $scope.onLoad = function (data) {
            var params = {
                CountryID: countryId
                , Previous_Period_Year: data
                , PartnerTypeID: partnerTypeId
                , DistrictID: $scope.DistrictID
                , Current_Financial_Period_Year_Id: $scope.FinancialYearID
                , VersionID: $sessionStorage.currentVersion.VersionID
            }
            summaryFactory.getData(params).then(function (result) {
                angular.forEach(result, function (value, key) {
                    if (value['Description'] === "# of Partner" || value['Description'] === "Sellout ($K)" || value['Description'] === "MDF ($K)" || value['Description'] === "MDF / Sellout (%)") {
                        this[key].flag = true
                    }
                    else {
                        this[key].flag = false
                    }
                }, result);
                $scope.gridArray = result;
            });
        }


        $scope.apply = function () {
            $scope.periodsApplied = !$scope.periodsApplied;
            selectedType = this.periods == 'yes' ? 0 : 1;
            $scope.onLoad(selectedType);
            loadGraph();
        }


        $scope.changeFunction = function () {
            $scope.graphOne = !$scope.graphOne;
            $scope.condition = !$scope.condition;
        }




        $scope.export = function () {

            if ($scope.periodsApplied) {
                var arToExport = [];
                angular.forEach($scope.gridArray, function (object) {
                    var obj = {

                    }
                    obj[' '] = object.Description,
                    obj['Growing Partner Previous Period'] = object.Grew_Previous_Period,
                    obj['Growing Partner Current Period'] = object.Grew_Current_Period,
                    obj['Growing Partner PoP % (Period over period)'] = object.Grew_PoP,
                    obj['Declining Partner Previous Period'] = object.Decl_Current_Period,
                    obj['Declining Partner Current Period'] = object.Decl_Previous_Period,
                    obj['Declining Partner PoP % (Period over period)'] = object.Decl_PoP
                    arToExport.push(obj);
                })
            }
            else {
                var arToExport = [];
                angular.forEach($scope.gridArray, function (object) {
                    var obj = {

                    }
                    obj[' '] = object.Description,
                    obj['Growing Partner Previous Year'] = object.Grew_Previous_Period,
                    obj['Growing Partner Current Period'] = object.Grew_Current_Period,
                    obj['Growing Partner YoY % (Year over year)'] = object.Grew_PoP,
                    obj['Declining Partner Previous Year'] = object.Decl_Current_Period,
                    obj['Declining Partner Current Period'] = object.Decl_Previous_Period,
                    obj['Declining Partner YoY % (Year over year)'] = object.Decl_PoP
                    arToExport.push(obj);
                })
            }
            UtilityService.JSONToCSVConvertor(arToExport, 'Final Summary', true);
        }

        $scope.onLoad(selectedType);

        $scope.format = function (o) {
            return budgetFactory.toFormat(o);
        }
    };
})();


 
 

