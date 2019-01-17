/*
Historical Factory
Created by: Aamin Khan
Created at: 06/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").factory("partnerCreditFactory", partnerCreditFactoryFn);



    function partnerCreditFactoryFn($rootScope, 
        $http, 
        HistoricalFactory, 
        modelParameterFactory, 
        $sessionStorage, 
        $timeout, 
        $localStorage, 
        partnerCreditRound2Factory, 
        graphFactory, 
        roleMappingService,
        partnerBudgetDataService,
        $state,
        dataService) {
        //ngInject
        return {
            pageSize: 20,
            

            /*
                Fetch data
                Written to integrate lazyloading
            */

            fetchData: function(filters){
                if (filters.UserID == undefined) {
                    filters.UserID = $localStorage.user.UserID;
                }
                var $promise = $http({
                    method: 'POST',
                    url: $rootScope.api + "PartnerBudget/GetPartnerDetails",
                    data: filters
                }).then(
                    function(resp){
                        //return JSON.parse(resp.data);
                        return  resp.data;
                    }
                )

                return $promise
            },


            getData: function ($scope, businessUnitId) {
                var that = this;
                $('#commonLoader').css('display', 'block');

                var CountryID = $sessionStorage.resellerPartner.obj.id;
                var PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
                var FinancialYear = $sessionStorage.resellerPartner.FinencialYear
                var DistrictID = $sessionStorage.resellerPartner.district.DistrictID
                var versionId = $sessionStorage.currentVersion.VersionID;
                $scope.PartnerTypeID = PartnerTypeID;
                var curObj = this;

                $scope.pageLimit = 10;
                $scope.currentPage = 1;
                $scope.initial = 1, $scope.final = 10;
                //$scope.histories = curObj.generateChartData($scope.histories);
                curObj.createPagination($scope.initial, $scope.final, $scope);

                var objToPass = {
                    CountryID: CountryID,
                    FinancialYearID: $localStorage.user.FinancialYearID,
                    DistrictID: DistrictID,
                    PartnerTypeID: PartnerTypeID,
                    versionId:$sessionStorage.currentVersion.VersionID,
                    PageIndex: 1,
                    PageSize: 10,
                    SortColumn: "Partner_name",
                    SortOrder: "asc",
                    businessUnitID: businessUnitId,
                    UserID:$localStorage.user.UserID
                }

                
                var promise = $http({
                    method: 'POST',
                    url: $rootScope.api + "PartnerBudget/GetPartnerDetails",
                    data: objToPass
                }).then(function (resp) {
                    
                    //$scope.total = resp.data.TotalofPartners;
                    if (resp.data.length == 0) {
                        $('#commonLoader').css('display', 'none');
                        curObj.generateTotalRow(objToPass, $scope, true);

                        $timeout(function () {
                            $(".graph-row").hide();
                            $(".summary-expand").hide();
                        }, 300);

                    }else{
                       // resp.data = JSON.parse(resp.data);
                        $scope.data = angular.copy(resp.data);
                        $scope.showPagination = $scope.data.length / curObj.pageSize > 1 ? true : false;
                        $scope.pageLimit = 10;
                        $scope.currentPage = 1;
                        $scope.initial = 1,
                        $scope.final = 10;
                        $scope.histories = [];
                        try {
                            var arr = resp.data.slice(0, curObj.pageSize);
                            $scope.histories = arr;

                        } catch (e) {

                        }
                        curObj.generateTotalRow(objToPass, $scope, true);

                        $timeout(function () {
                            $(".graph-row").hide();
                            $(".summary-expand").hide();
                        }, 300);
                        curObj.hideShowGraph($scope);
                        $('#commonLoader').css('display', 'none');
                        
                        if($state.current.name == "budget.partner-budget-round2"){
                            partnerCreditRound2Factory.getFocusArea($scope, $scope.histories[0].FocusedAreaID);    
                        }else{
                            that.initGraph($scope,businessUnitId);
                        }
                        
                    }
                    
                });
                return promise

            },

            hideShowGraph: function($scope){
                $timeout(function(){
                    var elem = $(".partner-credit .data")
                    if($scope.expand){
                        elem.css("margin-top","250px")
                        if($state.current.name != 'budget.partner-budget-expand'){
                            //$(".fixed_headers tbody").css("height","140px")    
                             //$(".fixed_headers tbody").addClass("imp140")
                        }
                        
                    }else{
                        elem.css("margin-top","37px");
                        //$(".fixed_headers tbody").css("height","320px")
                         $(".fixed_headers tbody").addClass("imp320")
                    }
                },1000)
            },

            applyFilter: function ($scope) {

                var curObj = this;
                var howMany = $scope.data.length / curObj.pageSize
                $scope.initial = 1,
		        $scope.final = 10;
                $scope.currentPage = 1;
                try {
                    var arr = $scope.data.slice(0, curObj.pageSize);
                    $scope.histories = arr;
                    curObj.createPagination($scope.initial, $scope.final, $scope);
                    $timeout(function () {
                        $(".graph-row").hide();
                    }, 30);
                } catch (e) {

                }
            },

            generateTotalRow: function (obj, $scope, initial) {


                var curObj = this;
                var formatter = new Intl.NumberFormat('en-US', {
                    minimumFractionDigits: 0
                });
                $http({
                    method: 'POST',
                    url: $rootScope.api + "PartnerBudget/GetPartnerDetailsSummary",
                    data: obj
                }).then(function (resp) {
                    try{
                        
                        var data = JSON.parse(resp.data);
                        $scope.total = data.Total[0];
                        $scope.total.Total_ARUBAMSA = formatter.format(parseFloat($scope.total.Total_ARUBAMSA));
                        if ($scope.total.Total_ARUBAMSA=='NaN') {
                            $scope.total.Total_ARUBAMSA = 0;
                        }
                        $scope.total.Total_MSA = formatter.format(parseFloat($scope.total.Total_MSA));
                        if ($scope.total.Total_MSA == 'NaN') {
                            $scope.total.Total_MSA = 0;
                        }
                        $scope.noTotalGraph = true;
                        $scope.total.BusinnessUnitDetails = curObj.generateChartDataForTotal($scope.total.BusinnessUnitDetails,$scope);
                        $scope.tx = {};
                        $scope.tx.mdf = [
                            {
                                values: $scope.total.BusinnessUnitDetails[0].mdfHistory,
                                key: 'MDF'
                            }

                        ]

                        $scope.tx.sellout = [
                            {
                                values: $scope.total.BusinnessUnitDetails[0].selloutHistory,
                                key: 'Sellout'
                            }

                        ];
                        
                        curObj.graphOptionsForTotal($scope, $scope.tx)
                    }catch(e){
                        console.log(e);
                        $scope.noTotalGraph = false;
                    }finally{

                        $timeout(function () {
                                $(".summary-expand").hide();
                        }, 300);

                        if (initial) {
                            if($scope.showSpartlines) {
                                $timeout(function(){
                                    $(".bu-imp, .cont").css("height","220px");
                                },200)
                            } else{
                               
                                
                            }
                        }
                    }

                })

            },

            

            /*GET getPartnerBu*/
            getPartnerBu: function (partnerBugetId, $scope, index, partnerType) {

                //close whatever is open
                $(".graph-row").hide();
                var elem = $("#icon-" + index);
                var curObj = this;
                var div = $("#" + partnerType + "-graph-row-" + index);
                if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                    var vid = $localStorage.selectedversion != undefined ? $localStorage.selectedversion.VersionID : $sessionStorage.currentVersion.VersionID;

                    //hit the api
                    $http({
                        method: 'GET',
                        url: $rootScope.api + "PartnerBudget/GetPartnerBudgetBUDetails?PartnerBudgetID=" + partnerBugetId + "&VersionID=" + vid,
                    }).then(function (resp) {
                        
                       
                        if (resp.data.length == 0) {
                            $('#commonLoader').css('display', 'none');
                            return
                        }
                        div.show();
                        elem.removeClass('fa-caret-down').addClass('fa-caret-up');

                        $scope.businessUnit = JSON.parse(resp.data);

                        $scope.businessUnit = curObj.generateChartData($scope.businessUnit); // data(HistoryMDF,HistorySellout) not coming from api
                        //MDF Variance issue resulation
                        for (var i = 0; i < $scope.businessUnit.length; i++) {
                            $scope.businessUnit[i].MDF[0].MDFVarianceReasonID = curObj.buildModel($scope.businessUnit[i].MDF[0].MDFVarianceReasonID, $scope)
                        }

                        console.log($scope.businessUnit)

                        if($scope.showSpartlines) {
                            $timeout(function(){
                                $(".bu-imp, .cont").css("height","220px");
                            },800)
                        } else{
                            $timeout(function(){
                                /*$(".business-unit,.projected-sellout").css("height","46px")*/
                            },800)
                            
                        }
                        roleMappingService.apply();

                    })


                } else {
                    elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                    div.hide();
                }

            },
            buildModel: function (obj, $scope) {
                var o = _.filter($scope.reason, function (o) { return o.MDFVarianceReasonID == obj; });
                obj = o[0];
                return obj
            },

            createPagination: function (from, to, $scope) {
                $scope.pages = [];
                $scope.showPagination = true;

                for (var i = from; i <= to; i++) {
                    $scope.pages.push({ name: i, active: false });
                }

            },

            gotoPage: function (pageNumber, $scope) {

                /*if (pageNumber === 1)
                    pageNumber = 0;*/

                if (pageNumber * $scope.pageLimit > $scope.data.length) {
                    return
                }

                $scope.currentPage = pageNumber;
                
                pageNumber = pageNumber - 1;

                
                if (pageNumber == $scope.final) { // increment
                    $scope.initial = $scope.initial + $scope.pageLimit - 2;
                    $scope.final = $scope.final + $scope.pageLimit - 2;
                    this.createPagination($scope.initial, $scope.final, $scope);
                }

                if (pageNumber == $scope.initial && pageNumber > 1) {
                    $scope.initial = $scope.initial - $scope.pageLimit + 2;
                    $scope.final = $scope.final - $scope.pageLimit + 2;
                    this.createPagination($scope.initial, $scope.final, $scope);
                }
                var from = pageNumber * $scope.pageLimit
                var to = from + $scope.pageLimit;

                var arrX = $scope.data.slice(from, to);
                $scope.histories = arrX;
                $timeout(function () {
                    $(".graph-row").hide();
                }, 0);
                if (pageNumber === 0)
                    $scope.currentPage = 1;
            },


            getChartOption: function (color, height, mdf_or_sellout, $scope) {
                return {
                    chart: {
                        type: 'sparklinePlus',
                        height: height,
                        width: 200,
                        x: function (d, i) { return i; },
                        xTickFormat: function (d) {
                            return "";//d3.time.format('%x')(new Date(d))
                        },
                        duration: 250,
                        color: function (d, i) { return color; }
                    }
                }
            },
            generateChartData: function (arr) {



                try{
                    for (var j = 0; j < arr.length; j++) { // BU loop

                    var mdfX = [];
                    var selloutX = [];

                    for (var k = 0; k < arr[j].HistoryMDF.length; k++) { //MDF Loop
                        var mdfObj = arr[j].HistoryMDF[k];

                        var FY = mdfObj.FinancialYear.split("_")[0];
                        // push two records for MDF
                        var h1 = FY + "_H1";
                        mdfX.push({ x: HistoricalFactory.convertFYtoDate(h1), y: HistoricalFactory.checkNull(mdfObj.MDF_1H) }); //,'FY': h1
                        var h2 = FY + "_H2";
                        mdfX.push({ x: HistoricalFactory.convertFYtoDate(h2), y: HistoricalFactory.checkNull(mdfObj.MDF_2H) }); //,'FY': h2
                    }


                    arr[j].mdfHistory = mdfX;


                    for (var l = 0; l < arr[j].HistorySellout.length; l++) { //MDF Loop

                        var selloutObj = arr[j].HistorySellout[l];
                        var FY = selloutObj.FinancialYear.split("_")[0];

                        // push 4 record for sellout
                        var q1 = FY + "_Q1";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q1), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q1) }); //,'FY': q1
                        var q2 = FY + "_Q2";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q2), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q2) }); //,'FY': q2
                        var q3 = FY + "_Q3";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q3), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q3) }); //,'FY': q3
                        var q4 = FY + "_Q4";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q4), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q4) }); //,'FY': q4


                    }

                    arr[j].selloutHistory = selloutX;
                }
                }catch(e){
                    
                }
                
                return arr;
            },

            generateChartDataForTotal: function (arr) {
                var that = this;
                var half = $localStorage.user.FinancialYear.split(" ")[1];
                for (var j = 0; j < arr.length; j++) { // BU loop

                    var mdfX = [];
                    var selloutX = [];

                    var mdfBarObj = [];
                    var selloutBarObj = [];

                    for (var k = 0; k < arr[j].HistoryMDF.length; k++) { //MDF Loop
                        var mdfObj = arr[j].HistoryMDF[k];

                        var FY = mdfObj.FinancialYear.split("_")[0];
                        // push two records for MDF
                        var h1 = FY + "_H1";
                        mdfX.push({ x: HistoricalFactory.convertFYtoDate(h1), y: HistoricalFactory.checkNull(mdfObj.MDF_1H) }); //,'FY': h1
                        var h2 = FY + "_H2";
                        mdfX.push({ x: HistoricalFactory.convertFYtoDate(h2), y: HistoricalFactory.checkNull(mdfObj.MDF_2H) }); //,'FY': h2

                        //for bar chart
                        mdfBarObj.push({ x: h1, y: HistoricalFactory.checkNull(mdfObj.MDF_1H) })
                        mdfBarObj.push({ x: h2, y: HistoricalFactory.checkNull(mdfObj.MDF_2H) })
                    }


                    arr[j].mdfHistory = mdfX;
                    var temp = [
                        {
                            values: mdfBarObj,
                            key: 'MDF'
                        }
                    ]

                    temp= that.sortByDate(temp);
                    if (half == "1H") {
                        
                        temp[0].values.pop();
                        temp[0].values.shift();
                    }
                    arr[j].mdfBar = temp;

                    for (var l = 0; l < arr[j].HistorySellOut.length; l++) { //MDF Loop

                        var selloutObj = arr[j].HistorySellOut[l];
                        var FY = selloutObj.FinancialYear.split("_")[0];

                        // push 4 record for sellout
                        var q1 = FY + "_Q1";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q1), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q1) }); //,'FY': q1
                        var q2 = FY + "_Q2";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q2), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q2) }); //,'FY': q2
                        var q3 = FY + "_Q3";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q3), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q3) }); //,'FY': q3
                        var q4 = FY + "_Q4";
                        selloutX.push({ x: HistoricalFactory.convertQuaterFYtoDate(q4), y: HistoricalFactory.checkNull(selloutObj.SellOut_Q4) }); //,'FY': q4
                        var h1 = FY + "_H1";
                        var h2 = FY + "_H2";
                        selloutBarObj.push({ x: h1, y: HistoricalFactory.checkNull(selloutObj.SellOut_Q1 + selloutObj.SellOut_Q1) })
                        selloutBarObj.push({ x: h2, y: HistoricalFactory.checkNull(selloutObj.SellOut_Q4 + selloutObj.SellOut_Q4) })


                    }

                    arr[j].selloutHistory = selloutX;
                    
                    var temp = [
                        {
                            values: selloutBarObj,
                            key: 'Sellout'
                        }
                    ]
                    temp = that.sortByDate(temp);
                    if (half == "1H") {
                        temp[0].values.pop();
                        temp[0].values.shift();
                    }
                    arr[j].selloutBar = temp;

                }
                return arr;
            },
            getBu: function ($scope) {
                
                $scope.bus = $localStorage.businessUnits;
                //angular.forEach($scope.bus, function (value, key) {
                //    if ($localStorage.user.BUs.indexOf(value.ID) > -1) {
                //        $scope.bus.splice(key, 1);
                //    }

                //});
                //angular.forEach($scope.bus, function (value, key) {
                //    if ($localStorage.user.BUs.indexOf(value.ID) > -1) {
                //        $("input").prop('disabled', true);
                //    }
                //    else
                //        $("input").prop('disabled', false);

                //});
                $('#myModalcreate,#CopyModal').on('hidden.bs.modal', function () {

                    $scope.drpdpwnvalue1 = null;
                    $scope.DestPlanvalue = "";

                });
                /*return
                modelParameterFactory.getBusinessUnits().then(
					function success(resp) {
					    $scope.bus = resp.data;
					    $scope.bus.splice(0, 0, { ID: 0, Name: 'Total' })
					},
					function error() {

					}
				)*/
            },



            calculateTotalByBUID: function (key, subKey, buId, partners) {
                var sum = 0;
                for (var i = 0; i < partners.length; i++) {
                    var val = partners[i].BU[buId][key][0][subKey];
                    val = typeof val == 'undefined' || !val || val == 0 ? 0 : val;
                    sum += val;
                }
                return sum;
            },
            saveEditedData: function (data, IsFrom_Round2, busave, $scope, partnerBudgetId, patnerid, triggerfrom) {
                var curObj = this;
                var url = ""
                if (busave) {
                         url = $rootScope.api + 'PartnerBudget/UpdatePartnerBUBudget'
                 }
                else
                {
                    if (IsFrom_Round2) {
                        url = $rootScope.api + 'PartnerBudget/CreateUpdatePartnerBUBudget?IsFrom_Round2=true'
                    } else {
                        url = $rootScope.api + 'PartnerBudget/CreateUpdatePartnerBUBudget?IsFrom_Round2=false'
                    }
                }
                $http({
                    method: 'POST',
                    url: url,
                    data: data
                }).then(function (resp) {
                     curObj.initGraph($scope, $scope.buId);
                    if(IsFrom_Round2){
                        partnerCreditRound2Factory.getRemainingMDF($scope)
                    }

                    var objToPass = {
                        CountryID: $sessionStorage.resellerPartner.obj.id,
                        FinancialYearID: $localStorage.user.FinancialYearID,
                        DistrictID: $sessionStorage.resellerPartner.district.DistrictID,
                        PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
                        VersionID: $sessionStorage.currentVersion.VersionID,
                        PageIndex: 0,
                        PageSize: 0,
                        businessUnitID: $scope.buId
                    }
                    if (triggerfrom == "buSave") {
                        $http.get($rootScope.api + "PartnerBudget/GetPartnerBudgetSearch?VersionID=" + $sessionStorage.currentVersion.VersionID + "&businessUnitID=" + $scope.buId + "&SearchColumn=PartnerID&find=" + patnerid + "&WithoutHistory=" + $scope.chkhistory + "&FilterColumnName=" + $scope.filterColumn + "&FilterDelimeter=" + $scope.filterDelimeter + "&FilterValue=" + $scope.filterValue).success(function (response) {
                            $scope.PartnerIDs = response;
                            $scope.partnerData = $scope.PartnerIDs[0];
                            $scope.partnerData.MDF[0].MDFVarianceReasonID = curObj.buildModel($scope.partnerData.MDF[0].MDFVarianceReasonID, $scope)
                            var indexes = $.map($scope.histories, function (obj, index) {
                                if (obj.PartnerID == patnerid) {
                                    return index;
                                }
                            })
                            $scope.partnerData._parent = indexes[0];
                            $scope.histories[indexes[0]] = $scope.partnerData;
                        })
                .error(function () {
                });
                    }
                    else if (partnerBudgetId != undefined) {
                        var indexes = $.map($scope.histories, function (obj, index) {
                            if (obj.PartnerID == patnerid) {
                                return index;
                            }
                        })
                         partnerBudgetDataService.getPartnerBu(partnerBudgetId)
                                 .then(function (resp) {
                                   
                                     $scope.histories[indexes[0]].businessUnits = JSON.parse(resp.data);
                                     for (var i = 0; i < $scope.histories[indexes[0]].businessUnits.length; i++) {
                                         $scope.histories[indexes[0]].businessUnits[i].MDF[0].MDFVarianceReasonID = curObj.buildModel($scope.histories[indexes[0]].businessUnits[i].MDF[0].MDFVarianceReasonID, $scope)

                                     }
                                     $timeout(function () {
                                         var currentBus = JSON.parse(resp.data);
                                         for (var i = 0; i < currentBus.length; i++) {
                                             if ($scope.role.disabled || $scope.disablePrevious || $scope.NOACCESS) {
                                                 $(".businessUnit_" + currentBus[i].BusinessUnitID + " input").attr("disabled", "disabled");
                                                 $(".businessUnit_" + currentBus[i].BusinessUnitID + " select").attr("disabled", "disabled");
                                                 $(".businessUnit_" + currentBus[i].BusinessUnitID + " img").off('click');
                                             }
                                             if (($localStorage.user.BUs.indexOf(currentBus[i].BusinessUnitID) > -1)) {

                                             }
                                             else {
                                                 $(".businessUnit_" + currentBus[i].BusinessUnitID + " input").attr("disabled", "disabled");
                                                 $(".businessUnit_" + currentBus[i].BusinessUnitID + " select").attr("disabled", "disabled");
                                                 $(".businessUnit_" + currentBus[i].BusinessUnitID + " img").off('click');

                                             }
                                         }
                                         $(".specialText").priceFormat();
                                     }, 100)
                                 })
                      
                    }
                     curObj.generateTotalRow(objToPass, $scope, false);
                 });
            },
            saveMSA: function (data, $scope) {
                var curObj = this;
                var url = $rootScope.api + 'PartnerBudget/CreateUpdatePartnerBudget';

                var $promise = $http({
                    method: 'POST',
                    url: url,
                    data: data
                }).then(function (resp) {
                    console.log('MSA Saved!');
                    curObj.initGraph($scope, $scope.buId);
                });
                return $promise
            },
             getReasons: function ($scope) {
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'PartnerBudget/GetMDFVarianceReasonDetails'
                }).then(function (resp) {
                    $scope.reason = resp.data;
                });
            },
            sumUpBaslineMDF: function () {

            },
            salesGrowth: function ($scope, data) {

                $scope.salesGrowthData = [
	                {
	                    key: "Cumulative Return",
	                    values: [
	                        {
	                            "label": "A",
	                            "value": parseInt(data.Default_SalesGrowth_without_mdf)
	                        },
	                        {
	                            "label": "B",
	                            "value": parseInt(data.Default_SalesGrowth_with_mdf)
	                        },
	                        {
	                            "label": "C",
	                            "value": parseInt(data.SalesGrowth_without_mdf)
	                        },
	                        {
	                            "label": "D",
	                            "value": parseInt(data.SalesGrowth_with_mdf)
	                        }
	                    ]
	                }
                ];

                $scope.salesGrowthOption = {
                    chart: {
                        type: 'discreteBarChart',
                        height: 220,
                        width: 300,
                        margin: {
                            top: 20,
                            right: 50,
                            bottom: 50,
                            left: 60
                        },
                        x: function (d) { return d.label; },
                        y: function (d) { return d.value + (1e-10); },
                        showValues: true,
                        showXAxis: false,
                        valueFormat: function (d) {
                            return d3.format(',.0f')(d) + '%';
                        },
                        duration: 100,
                        color: this.colorFunction,
                        yDomain: [-2, 8],
                        yAxis: {
                            ticks: 6,
                            tickFormat: function (d) { return d3.format(',f')(d) + '%' },
                            opacity: 0.1
                        }
                    }
                };


            },
            colorFunction: function () {
                return function (d, i) {
                    if (i % 2 !== 0) {
                        return '#333'
                    } else {
                        return '#18b488'
                    }
                };
            },
            checkWidth: function ($scope, sellout, mdf, mdfBySellout) {
                var width = 1500;

                if (sellout || mdf || mdfBySellout) {

                    if (sellout) {
                        width = width + 100;
                    } else {
                        width = width - 100;
                    }

                    if (mdf) {
                        width = width + 100;
                    } else {
                        width = width - 100;
                    }

                    if (mdfBySellout) {
                        width = width + 100;
                    } else {
                        width = width - 100;
                    }

                    $(".hitorical-perfomance .data table").css("width", width + "px");
                    $(".hitorical-perfomance .data-container").css("overflow-x", "scroll")
                } else {
                    $(".hitorical-perfomance .data table").css("width", "auto");
                    $(".hitorical-perfomance .data-container").css("overflow-x", "hidden")
                }
                // selected 

                var tobe = angular.copy($scope.selectedPartner);
                tobe.shift();
                var evens = _.filter(tobe, function (isTrue) { return isTrue });
                if (evens.length > 0) {
                    var wd = evens.length * 100;
                    $(".hitorical-perfomance .data-container").css("overflow-x", "scroll")
                    $(".hitorical-perfomance .data table").css("width", width + wd + "px");
                }

            },
            getBUName: function (id) {
                var o = _.filter($localStorage.businessUnits, function (o) { return o.ID == id; });
                return o[0].Name;
            },
            initGraph: function ($scope, buId) {
                graphFactory.formGraph($scope,buId);

            },

            /*************** ENLARGE GRAPH **************/
            enlargeGraph: function ($scope, title, option, data) {
                var ox = angular.copy(option);
                ox.chart.width = 500;
                ox.chart.height = 350;
                $scope.graphTitle = title;
                $scope.graphOption = ox;
                $scope.graphDataForModal = data;
            },
            enlargeDonut: function ($scope, title, option, data) {
                
                $scope.graphTitle = title;
                graphFactory.initDonutOnTimeout($scope,$scope.allocatedVsRemainingData)
            },
            loadFilter : function () {
                $http({
                    method: 'GET',
                    url: $rootScope.api + "PartnerBudget/GetFilterColumns?VersionID=" + $sessionStorage.currentVersion.VersionID,
                }).then(function (resp) {

                    if (resp.data.length == 0) {
                        return
                    }
                    $scope.filterList = resp.data;
                    $scope.filterList.splice(0, 0, { FilterColumnName: 'Select', TableColumn: null})

                })
            },
            generateChartForBU: function (arr,$scope) {
                var curObj = this;
                var half = $localStorage.user.FinancialYear.split(" ")[1]

                for (var j = 0; j < arr.length; j++) { // BU loop

                    var mdfX = [];
                    var selloutX = [];
                    
                    for (var k = 0; k < arr[j].HistoryMDF.length; k++) { //MDF Loop
                        var mdfObj = arr[j].HistoryMDF[k];

                        var FY = mdfObj.FinancialYear.split("_")[0];
                        // push two records for MDF
                        var h1 = FY + "_H1";
                        mdfX.push({ x: h1, y: (HistoricalFactory.checkNull(mdfObj.MDF_1H) / 1000).toFixed(1) }); //,'FY': h1
                        var h2 = FY + "_H2";
                        mdfX.push({ x: h2, y: (HistoricalFactory.checkNull(mdfObj.MDF_2H) / 1000).toFixed(1) }); //,'FY': h2
                    }

                    var mdfmax_arr = [];
                    angular.forEach(mdfX, function (value) {
                        mdfmax_arr.push(parseInt(value.y));
                    })
                    var maxMDF_value = _.max(mdfmax_arr);

                    var temp = [{
                        values: mdfX,
                        key: 'MDF'
                    }];
                    
                    temp = curObj.sortByDate(temp);
                    if (half == "1H") {
                        temp[0].values.pop();
                        temp[0].values.shift();
                    }


                    arr[j]['mdfHistory'] = temp;


                    for (var l = 0; l < arr[j].HistorySellout.length; l++) { //MDF Loop

                        var selloutObj = arr[j].HistorySellout[l];
                        var FY = selloutObj.FinancialYear.split("_")[0];

                        // push 4 record for sellout
                        var q1 = FY + "_H1";
                        selloutX.push({ x: q1, y: (HistoricalFactory.checkNull(selloutObj.Sellout_1H) / 1000000).toFixed(1) });
                        var q2 = FY + "_H2";
                        selloutX.push({ x: q2, y: (HistoricalFactory.checkNull(selloutObj.Sellout_2H) / 1000000).toFixed(1) });

                    }


                    var temp1 = [{
                        values: selloutX,
                        key: 'Sellout',
                    }];
                    
                    temp1 = curObj.sortByDate(temp1);
                    if (half == "1H") {
                        temp1[0].values.pop();
                        temp1[0].values.shift();
                    }
                    arr[j]['selloutHistory'] = temp1;

                    

                }
                
                return arr;


            },

            graphOptionsForTotal: function ($scope,data) {
                

                var maxMDF = parseFloat((_.max(data.mdf[0].values, function (o) { return parseFloat(o.y); })).y);

                var that = this
                $scope.totalMDFOptions =  {
                    chart: {
                        type: 'multiBarChart',
                        height: 200,
                        width: 470,
                        margin: {
                            top: 20,
                            right: 0,
                            bottom: 40,
                            left: 130
                        },
                        clipEdge: true,
                        duration: 500,
                        color: that.colorFunction(),
                        stacked: false,
                        showValues: true,
                        valueFormat: function (d) {
                            return d3.format(',.1f')(d) + '%';
                        },
                        xAxis: {
                            axisLabel: '',
                            tickFormat: function (d) {
                                return d;
                            }
                        },
                        yAxis: {
                            axisLabel: '',
                            ticks: 5,
                            tickFormat: function (d) {

                                return '$' + d3.format(',.1f')(d) + 'K';
                            }
                        },
                        yDomain: [0, maxMDF]
                    }
                }

                var maxSellout = parseFloat((_.max(data.sellout[0].values, function (o) { return parseFloat(o.y); })).y);


                $scope.totalSelloutOption =  {
                    chart: {
                        type: 'multiBarChart',
                        width: 550,
                        height: 200,
                        margin: {
                            top: 20,
                            right: 0,
                            bottom: 40,
                            left: 130
                        },
                        clipEdge: true,
                        duration: 500,
                        color:  that.colorFunction(),
                        stacked: false,
                        showValues: true,
                        valueFormat: function (d) {
                            return d3.format(',.1f')(d) + '%';
                        },
                        xAxis: {
                            axisLabel: '',
                            tickFormat: function (d) {
                                return d;
                            }
                        },
                        yAxis: {
                            axisLabel: '',
                            axisLabelDistance: 10,
                            ticks: 2,
                            tickFormat: function (d) {
                                return '$' + d3.format(',.1f')(d) + 'M';
                            }
                        },
                        yDomain: [0, maxSellout]
                    }

                }


            },
            sortByDate: function (obj) {

                var arr = obj[0].values;
                var newArr = _.sortBy(arr, function (o) { return o.x; });
                obj[0].values = [];
                obj[0].values = newArr;
                return obj;
            }

        }



    }

})();