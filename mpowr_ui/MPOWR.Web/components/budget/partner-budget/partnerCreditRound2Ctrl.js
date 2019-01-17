/*
Partner Credit controller: Partner credit Round 2 controller
Created by: Aamin Khan
Created at: 19/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").controller("partnerCreditRound2Ctrl", partnerCreditRound2CtrlFn);

    function partnerCreditRound2CtrlFn($scope, $rootScope, $timeout, $state, partnerCreditRound2Factory,budgetFactory, $sessionStorage, partnerCreditFactory, $http, Notification, UtilityService, $localStorage, roleMappingService, versioningservice) {
        
        var Msg = "";
        var Alloc_Msg = "Additional MDF should not exceed the Remaining MDF for the current Business Unit.";
        
        $scope.filterText = 0;
        $scope.hideOperator = true;
        $scope.hidePartnerName = true;
        $scope.hidePartnerId = true;
        $scope.hideFilterValue2 = true;

        $scope.chkhistory = $localStorage.chkhistory;
        $scope.operand = [];
        $scope.operand.push({ key: "Select Operator", value: "" });
        $scope.operand.push({ key: "Equals", value: "=" });
        $scope.operand.push({ key: "Not Equals", value: "<>" });
        $scope.operand.push({ key: "Greater Than", value: ">" });
        $scope.operand.push({ key: "Greater Than or Equal", value: ">=" });
        $scope.operand.push({ key: "Less Than", value: "<" });
        $scope.operand.push({ key: "Less Than or Equal", value: "<=" });
        $scope.operand.push({ key: "Between", value: "between" });
        $scope.filterDelimeter = $scope.operand[0];
        $scope.Popover = function (e) {
            var yellow = e.currentTarget.classList.value.indexOf('yelow');
          //  var red = e.currentTarget.classList.value.indexOf('rd');
            if (yellow > 0) {
                e.currentTarget.setAttribute("title", Msg);
            }
                //else if (red > 0) {
                //    e.currentTarget.setAttribute("title", Aruba_Msg);
                //}
            else e.currentTarget.removeAttribute("title");
        }
        
    	//ngInject
        $scope.role = roleMappingService.apply();
        loadFilter(false, true);
        $scope.GetFilter = function () {

            var filterColumn = $scope.filterColumn;
            var filterDelimeter = $scope.filterDelimeter;
            var filterValue2;
            if (angular.element('#filterValue2') != undefined && angular.element('#filterValue2')[0] != undefined)
                filterValue2 = angular.element('#filterValue2')[0].value
            //var filterValue = $scope.filterText;
            filterValue = angular.element('#filterValue')[0].value;

            if (filterDelimeter.value == 'between') {
                filterValue = filterValue + ' and ' + filterValue2
            }


            currentPage = 0;
            fetch(filter, sortColumn, sortOrder, false, filterColumn.TableColumn, filterDelimeter.value, filterValue);
        }
        $scope.OnFilterColumnChange = function (filterColumn) {
            if (filterColumn.FilterColumnNames == 'Partner Name' || filterColumn.FilterColumnNames == 'Partner ID') {
                $scope.hideOperator = true;
                $scope.hideFilterValue2 = true;
                if (filterColumn.FilterColumnNames == 'Partner Name') {
                    $scope.hidePartnerName = false;
                    $scope.hidePartnerId = true;
                }

                else {
                    $scope.hidePartnerId = false;
                    $scope.hidePartnerName = true;
                }
            }
            else {
                $scope.hideOperator = false;
                $scope.hidePartnerName = true;
                $scope.hidePartnerId = true;
                $scope.hideFilterValue2 = true;
             }
        }

        $scope.OnFilterDelimeterChange = function (filterDelimeter) {
            if (filterDelimeter.value != 'between') {
                $scope.hideFilterValue2 = true;
            }
            else {
                $scope.hideFilterValue2 = false;
            }
            $scope.filterDelimeter = filterDelimeter;
        }
        
        function loadFilter(expanded, Round2) {
            //if ($scope.filterList == undefined || expanded == true)
            $http({
                method: 'GET',
                url: $rootScope.api + "PartnerBudget/GetFilterColumns?VersionID=" + $sessionStorage.currentVersion.VersionID + "&ExtendedView=" + expanded + "&Round2=" + Round2,
            }).then(function (resp) {

                if (resp.data.length == 0) {
                    return
                }

                $scope.filterList = resp.data;
                if ($scope.filterColumn == undefined) {
                    $scope.filterColumn = $scope.filterList[0];

                    $scope.hideOperator = true;
                    $scope.hidePartnerName = false;
                    $scope.hidePartnerId = true;
                    $scope.hideFilterValue2 = true;
                }
                else {
                    if ($scope.filterColumn != undefined && $scope.filterColumn.FilterColumnNames != "Partner Name" && $scope.filterColumn.FilterColumnNames != "Partner ID") {
                        $scope.filterColumn = $scope.filterList[0];

                        $scope.hideOperator = true;
                        $scope.hidePartnerName = false;
                        $scope.hidePartnerId = true;
                        $scope.hideFilterValue2 = true;
                    }
                    else {
                        $scope.hideOperator = true;
                        $scope.hideFilterValue2 = true;
                        if ($scope.filterColumn.FilterColumnNames == 'Partner Name') {
                            $scope.filterColumn = $scope.filterList[0];
                            $scope.hidePartnerName = false;
                            $scope.hidePartnerId = true;
                        }

                        else {
                            $scope.filterColumn = _.filter($scope.filterList, function (o) { return o.FilterColumnNames == 'Partner ID'; })[0];
                            $scope.hidePartnerId = false;
                            $scope.hidePartnerName = true;
                        }
                    }
                }
            })
        }

        var goNext = function () {

            var f = null;
            angular.forEach($localStorage.user.Features, function (feature) {
                if (feature.FeatureID == 5 && feature.FeatureActionType == 'View') {
                    f = feature;
                }
            });
            if (f.IsFeatureActionChecked) {
                $state.go('budget.final-summary');
            }
            else
            {
                $state.go('budget.partner-budget');
            }
        }

        if (($sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0) == "C" &&
         $sessionStorage.resellerPartner.CountryOrGeoOrDistrict.indexOf("26") != -1) || ($sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0) == "C" &&
         $sessionStorage.resellerPartner.CountryOrGeoOrDistrict.indexOf("138") != -1) ||
       $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0) == "D") {
            goNext();
        }
        else {
        
        }
        $scope.reseller = $sessionStorage.resellerPartner;
        $scope.CurrrentFyPeriod = $localStorage.selectedversion.Financialyear.slice(5, 7) + $localStorage.selectedversion.Financialyear.slice(2, 4);
        var versionId = $sessionStorage.currentVersion.VersionID;
    	//partnerCreditFactory.getData($scope); // set to $scope.partners
        partnerCreditRound2Factory.getFocusArea($scope); //$scope.focusArea
        partnerCreditRound2Factory.getRemainingMDF($scope);
        partnerCreditFactory.initGraph($scope);
        budgetFactory.selectText();
        //$scope.businessUnit = [1,2,3,4]
        // $scope.versiondata = $sessionStorage.currentVersion;
        $scope.$sessionstorage = $sessionStorage;
        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                $scope.ctries = _.pluck(countryList, 'DisplayName').join(", ");
            }
        }
        var financial = {
            FinancialyearID: $localStorage.user.FinancialYearID,

        }
        //$scope.NOACCESS = $localStorage.NOACCESS;
        roleMappingService.getPlanningPeriods(financial).then(function onSuccess(response) {
            $scope.disablePrevious = $rootScope.disablePrevious;
            //if ($rootScope.disablePrevious == false) {
                roleMappingService.getCheckIsActiveFlag($localStorage.selectedversion.VersionID).then(function onSuccess(response) {
                    $scope.disablePrevious = $rootScope.disablePrevious;
                    if ($rootScope.disablePrevious == true && $rootScope.previousPlan == false) {
                        $localStorage.NOACCESS = true;
                    }
                    //else {
                    //    $localStorage.NOACCESS = true;
                    //    $scope.NOACCESS = $localStorage.NOACCESS;
                    //}
                    //if ($rootScope.disablePrevious == true)
                    //    Notification.error({ message: 'The Plan is disabled due to any of the Geo/Country/District/BusinessUnit is made InActive', delay: null });
                })
            //}
        })

        $scope.NOACCESS = $localStorage.NOACCESS;
         $scope.versiondata = $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion : $localStorage.selectedversion;
        if ($localStorage.selectedversion != undefined) {
            $scope.versiondata.Financialyear = $localStorage.selectedversion.FinancialPeriod;
        }

        $scope.Math = window.Math;

    	$scope.radio = {name:false};
    	$scope.show = false;

    	$timeout(function () {
    	    $(".specialText").priceFormat();
            $(".graph-row").hide();
        },300);

       

    	$scope.getTotal =  function(arr,key){
    		
			return _.reduce(arr, function(memo, num){ 
				return memo + num[key] ; 
			}, 0);
		}

        $scope.getTotal_PB2 = function(BU,skey,key){
            
                var sum = 0;
                if(typeof BU !== 'undefined'){
                    for(var i=0;i<BU.length;i++){
                        try{
                            sum += BU[i][skey][0][key];
                        }catch(e){
                            
                            sum = sum + 0
                        }
                    }
                    return sum; 
                }
               return sum;
        }

        




        $scope.toogle = function (partner, index) {
            $scope.Varid = 0;
                $(".graph-row").hide();
                 var elem = $("#icon-" + partner.PartnerID);
                 var curObj = this;
                 var div = $(".reseler-graph-row-" + partner.PartnerID);

                 if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                     var vid = $localStorage.selectedversion != undefined ? $localStorage.selectedversion.VersionID : $sessionStorage.currentVersion.VersionID;
                         //hit the api
                        $http({
                            method:'GET',   
                            url: $rootScope.api + "PartnerBudget/GetPartnerBudgetBUDetails?PartnerBudgetID=" + partner.MSA[0].PartnerBudgetID + "&VersionID=" + vid + "&WithoutHistory=" + $localStorage.chkhistory,
                        }).then(function (resp) {

                            $timeout(function () {
                                 $(".specialText").priceFormat();
                                var currentBus = JSON.parse(resp.data);
                                for (var i = 0; i < currentBus.length; i++) {
                                    if (($localStorage.user.BUs.indexOf(currentBus[i].BusinessUnitID) > -1)) {
                                    }
                                    else {
                                        $("#businessUnit_" + currentBus[i].BusinessUnitID + " input").attr("disabled", "disabled")
                                    }
                                }
                            }, 500)
                                
                            div.show();
                            elem.removeClass('fa-caret-down').addClass('fa-caret-up');
                            $scope.histories[index]["businessUnit"] = JSON.parse(resp.data);
                        })
                } else {
                    elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                    div.hide();
                }
        }
      
        $scope.setNext = function () {

            var f = null;
            angular.forEach($localStorage.user.Features, function (feature) {
                if (feature.FeatureID == 5 && feature.FeatureActionType == 'View') {
                    f = feature;
                }
            });
            if (f.IsFeatureActionChecked) {
                $state.go('budget.final-summary');
            }
        }
        $scope.setPrev = function () {
            $rootScope.prev = true;
        }

        $scope.copy = function(){
            
            for(var i=0;i<$scope.partners.length;i++){
                var BU = $scope.partners[i].BusinessUnits;
                for(var j=0;j<BU.length;j++){

                    var ox = BU[j];
                    if(ox.AdditionalMDF == 0){
                        ox.AdditionalMDF = ox.AdditionalCalculatedMDF 
                    }                    
                }
            }
        }

        $scope.gotoPage = function(pageNumber){
            partnerCreditFactory.gotoPage(pageNumber,$scope)
        }
        
        $scope.next = function(){
            partnerCreditFactory.gotoPage($scope.currentPage+1,$scope)
        }
        $scope.prev = function(){
            partnerCreditFactory.gotoPage($scope.currentPage-1,$scope)
        }

        $scope.trimm = function (value) {
            if (typeof (value) == "number") {
                return value;
            }
            else {
                value = parseInt(value.replace(/,/g, ""));
                return value;
            }
        }

        $scope.saveData = function (BU, partner, fromReason, e) {

            var chk = false;

            var theBU = _.find($scope.graphData.AllocatedGraph, function (o) { return o.Business_Unit == BU.BusinessUnitID; });
          

             if(!fromReason){
         
                 if (($scope.trimm(BU.MDF[0].AdditionalMDF) - prevData) > theBU.Remaining) {
                     if ($localStorage.user.IsOverAllocation == false) {
                         $timeout(function () {
                             $(".specialText").priceFormat();
                         }, 1000)
                         $scope.Varid = 0;
                         BU.MDF[0].AdditionalMDF = prevData;
                         Notification.error(Alloc_Msg);
                         return false
                     }
                     else {
                         $timeout(function () {
                             $(".specialText").priceFormat();
                         }, 1000)
                         $scope.Varid = e.currentTarget.id;
                         chk = true;
                     }

                     //prevTotal
                    // return
                }
             }

             var sum = 0;
             for (var i = 0; i < partner.businessUnit.length; i++) {
                 var cellvalue = partner.businessUnit[i].MDF[0].AdditionalMDF.toString();
                 cellvalue = cellvalue.replace(/,/g, "");
                 sum += parseInt(cellvalue);
             }
             partner.MDF[0].Additional_MDF = sum;

             var IsFrom_Round2 = true;
            
            if (!chk) {
                $scope.Varid = 0;
            }
            

            var object = {
                PartnerBUBudgetID:  BU.MDF[0].PartnerBUBudgetID,
                PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                BusinessUnitID: BU.BusinessUnitID,
                Additional_MDF: BU.MDF[0].AdditionalMDF,
                Additional_MDF_Reason: BU.MDF[0].AdditionalMDFReason,
                UserID:$localStorage.user.UserID
            }
           
            partnerCreditFactory.saveEditedData(object, IsFrom_Round2,false, $scope);
            $timeout(function () {
                $(".specialText").priceFormat();
            }, 1000)
        }
   

        $scope.filter = function(){
            //partnerCreditRound2Factory.updateDataByRModel($scope);
             var objToPass = {
                //CountryID: $sessionStorage.resellerPartner.obj.id,
                //Financialyear: $sessionStorage.resellerPartner.FinencialYear,
                //DistrictID: $sessionStorage.resellerPartner.district.DistrictID,
                //PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
                versionId:$sessionStorage.currentVersion.VersionID,
                PageIndex: 0,
                PageSize: 0,
                businessUnitID: 0
                //,UserID: $localStorage.user.UserID
            }

            $http({
                method: 'POST',
                url: $rootScope.api + "PartnerBudget/GetPartnerDetailsSummary",
                data: objToPass
            }).then(function (resp) {
                
                var data = JSON.parse(resp.data);
                var tx = data.Total[0].BusinnessUnitDetails
                var sum = 0;
                _.each(tx, function(o){ 
                    sum += o.Additional_MDF;
                });
                if(sum > 0){
                    // open bootbox
                    bootbox.confirm({
                        message: "Additional MDF will be overwritten with Recommendation. Do you want to continue?",
                        buttons: {
                            cancel: {
                                label: 'No',
                                className: 'btn-default'
                            },
                            confirm: {
                                label: 'Yes',
                                className: 'btn-success'
                            }
                            
                        },
                        callback: function (result) {
                            if(result){
                                saveit();
                            }else{
                                console.log("User clicked cancel")
                            }   
                            
                        }
                    });

                }else{
                    saveit();
                }
            })

            function saveit(){
                var curObj = this;
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
                    "Versionid": $sessionStorage.currentVersion.VersionID,
                    "FocusedAreaID": $scope.selected.FocusedAreaID,
                    "UserID": $localStorage.user.UserID
                }
                $http({
                    method: 'POST',
                    url: $rootScope.api+ "PBRoundtwo/ApplyFilter",
                    data: dataObj
                }).then(function(resp){
                    partnerCreditFactory.getData($scope,0);
                    partnerCreditRound2Factory.getRemainingMDF($scope); 
                    $('#mdf-loader').css('display', 'none');
                    $('#commonLoader').css('display', 'none');
                })
            }

            
        }

        var prevTotal = 0
        var prevData = 0;
        $scope.copy = function(bu,partner){
            prevTotal = partner.MDF[0].Additional_MDF
            prevData = $scope.trimm(bu.MDF[0].AdditionalMDF)
        }

        $scope.calculate = function(bu,partner,e){
            var chk = false;
            
            var theBU = _.find($scope.graphData.AllocatedGraph, function(o){ return o.Business_Unit == bu.BusinessUnitID; });

            if (bu.MDF[0].AdditionalMDF > theBU.TotalBaseline) {
                $scope.Varid = e.currentTarget.id;
                Msg = Alloc_Msg;
                chk = true;
                /*Notification.error({
                    message: "Sum of allocated MDF for partners of the Business Unit should not exceed the total Baseline MDF allocated for the Business Unit." + $rootScope.closeNotify,
                    delay: null
                });*/
                /*partner.MDF[0].Additional_MDF = prevTotal;
                bu.MDF[0].AdditionalMDF = prevData;*/
                //return
            }else{
                var sum = 0;
                for (var i = 0; i < partner.businessUnit.length; i++) {
                    var cellvalue = partner.businessUnit[i].MDF[0].AdditionalMDF.toString();
                    cellvalue = cellvalue.replace(/,/g, "");
                    sum += parseInt(cellvalue);
                }
                partner.MDF[0].Additional_MDF = sum
            }
            
            if (!chk) {
                $scope.Varid = 0;
            }
            
        }

        //$scope.resetAutoComplete = function(){
        //    partnerCreditFactory.applyFilter($scope);
        //    $(".angucomplete-holder .form-control").val("");
        //}

        $scope.clearSearch = function () {
            commonReset();
            //$("#PartnerId").val("");
            $("#PartnerName").val("");
            $("#filterValue").val("");
            $scope.Partner_Name = "";
            angular.element('#partnerInput').removeClass('open');

           $scope.filterColumn = $scope.filterList[0];

            $scope.hideOperator = true;
            $scope.hidePartnerName = false;
            $scope.hidePartnerId = true;
            $scope.hideFilterValue2 = true;
            $scope.filterDelimeter = $scope.operand[0];
        }


        function commonReset() {
            sortColumn = "Partner_name";
            sortOrder = "asc";
            currentPage = 0
            fetch(filter, sortColumn, sortOrder, true);
        }

        // Search by partner name
        $scope.getPartnerName = function (partnerName) {
            // console.log(partnerName);
            $scope.Partner_Name = partnerName;
            if ($scope.Partner_Name.length > 2) {
                var searchkey = encodeURIComponent(partnerName);
                //$http.get($rootScope.api + "PartnerBudget/GetPartnerBudgetSearch?PartnerTypeID=" + PartnerTypeID + "&DistrictID=" + DistrictID + "&FinancialyearID=" + $localStorage.user.FinancialYearID + "&businessUnitID=0&SearchColumn=Partner_Name&find=" + $scope.Partner_Name + "&VersionID=" + versionId).success(function (response) {
                $http.get($rootScope.api + "PartnerBudget/GetPartnerBudgetSearch?VersionID=" + versionId + "&businessUnitID=0&SearchColumn=Partner_Name&find=" + searchkey +"&WithoutHistory=" + $scope.chkhistory + "&FilterColumnName=" + $scope.filterColumn + "&FilterDelimeter=" + $scope.filterDelimeter + "&FilterValue=" + $scope.filterValue).success(function (response) {
                    $scope.partnersDetails = response;
                    angular.element('#partnerInput').addClass('open');
                })
                    .error(function () {
                        angular.element('#partnerInput').removeClass('open');


                    });
            }

            if ($scope.Partner_Name.length == 0) {
                commonReset();
            }
        }

        $scope.selectedName = function (name, index) {
            angular.element('#partnerInput').removeClass('open');

            $scope.partnersDetails = $scope.partnersDetails[index];
            //$scope.Partner_Name =  $scope.partnersDetails.Partner_Name;
            $scope.histories = [$scope.partnersDetails];
            angular.element("#PartnerName").val(name);
            //$scope.$apply();

        }


        $scope.initAuto = function(){
            $('#data_auto_complete').bind("DOMSubtreeModified",function(){
              var data = $(this).html();
              if(data.length>0){
                var x = JSON.parse(data);
                $scope.histories = [x.originalObject]
              }
            });
        }

        $scope.sort = function (superKey, key, elemId) { // key = PartnerName

            var elem = $("#" + elemId);
            var icon = elem.find("i").attr("class").split(" ")[2];
            var toSort = 'asc';
            if (icon === 'fa-caret-down') { // do descending sort    
                var toSort = 'asc';
                elem.find("i").removeClass('fa-caret-down').addClass('fa-caret-up');
            } else { // do ascending sort
                var toSort = 'desc';
                elem.find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            }

            // reset the current page to 0
            currentPage = 0;
            sortColumn = elemId;
            sortOrder = toSort;
            filterValue = 0;
            if ($scope.filterColumn != undefined)
                filterColumn = $scope.filterColumn.TableColumn;
            if ($scope.filterDelimeter != undefined)
                filterDelimeter = $scope.filterDelimeter.value;
            if ($scope.filterText != undefined && angular.element('#filterValue') != undefined && angular.element('#filterValue')[0] != undefined) {
                filterValue = angular.element('#filterValue')[0].value;
                //filterValue = $scope.filterText;
            }
            fetch(filter, sortColumn, toSort, true, filterColumn, filterDelimeter, filterValue)

            
        }
        
        

        /*Lazy loading*/
        var lazyTo = 10;
        var lazySize=10;
        var processing = false;
        
        
        
        var CountryID = $sessionStorage.resellerPartner.obj.id;
        var PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
        var FinancialYear = $sessionStorage.resellerPartner.FinencialYear;
        var DistrictID = $sessionStorage.resellerPartner.district.DistrictID;
        
        
        var filter = {
            CountryID: CountryID,
            FinancialYearID: $localStorage.user.FinancialYearID,
            DistrictID: DistrictID,
            PartnerTypeID: PartnerTypeID,
            businessUnitID: 0
        }
        
        
        
        var sortColumn = "Partner_name";
        var sortOrder = "asc";
        var pageSize = 30;
        var currentPage = 0;
        var filterColumn = null;
        var filterDelimeter = null;
        var filterValue = null;
        
        
        fetch(filter, sortColumn, sortOrder, false, null, null, null, 'ok'); //first fetch
        
        
        function fetch(filter,SortColumn,SortOrder,fromSort, filterColumn, filterDelimeter, filterValue, fetchFirst){
            
            currentPage = currentPage + 1
            var processing = true;
            filter.PageIndex = currentPage;
            filter.PageSize = pageSize;
            filter.SortColumn = SortColumn;
            filter.sortOrder = SortOrder;
            filter.VersionID = $sessionStorage.currentVersion.VersionID;
             filter.FilterColumn = filterColumn;
            filter.FilterDelimeter = filterDelimeter;
            filter.FilterValue = filterValue;
            partnerCreditFactory.fetchData(filter)
                .then(function(data){
                    if (fromSort == undefined) {
                        if (data != undefined && data != "") {
                            $scope.histories = $scope.histories.concat(data);
                          
                        }  
                    }else{
                        $scope.histories = data;
                    }
                        
                    var processing = true;
                    
                    if(fetchFirst != undefined){
                         //partnerCreditFactory.initGraph($scope,$scope.buId); // generate graph
                         //partnerCreditFactory.generateTotalRow(filter,$scope, true); //generate total row 
                    }
                    $timeout(function () {
                        $(".specialText").priceFormat();
                        $(".summary-expand").hide();
                        $(".graph-row").hide();
                    }, 1000);
                
                
                })
        }
        
        
        $(window).scroll(function (event) {
            if (window.location.hash == "#/budget/partner-budget-round2" || window.location.hash == "#/budget/partner-budget") {
                if (processing) {
                    return false;
                } else {
                    if ($(window).scrollTop() >= $(document).height() - $(window).height()) {
                        fetch(filter, sortColumn, sortOrder);
                    }
                }
            }
            

        });
        
        //get financial year for dropdown
        var financial = {
            FinancialyearID: $localStorage.user.FinancialYearID,

        }

        $scope.fillFinancialyearList = function (financial) {

            $scope.FinancialyearList = null;

            return $http({
                method: 'POST',
                url: $rootScope.api + 'BUBudgets/GetFinancialYear',
                data: $sessionStorage.fyParams
            }).success(function (result) {
                $scope.FinancialyearList = result;
                $rootScope.FinancialyearList = result;
                //console.log('ver list', $rootScope.FinancialyearList);
            });
        };
        $scope.fillFinancialyearList(financial);
        $scope.fillFinancialyearss = function (financial) {

            $scope.Financialyearend = null;

            return $http({
                method: 'POST',
                url: $rootScope.api + 'Version/GetFinancialYear',
                data: financial
            }).success(function (result) {
                $scope.Financialyearend = result;
                console.log($scope.FinancialyearList);
            });
        };
        $scope.fillFinancialyearss(financial);

        //for clone the data
        $scope.copydata = function () {
            var copyobj = {
                OldFinancialyearID: $scope.versiondata.FinancialPeriod,
                OldVersionNo: $scope.versiondata.VersionNo,
                NewFinancialyearID: $scope.drpdpwnvalue1, // dest fy id
                PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
                DistrictId: $sessionStorage.resellerPartner.district.DistrictID,
                GeoID: $sessionStorage.resellerPartner.obj.geo.GeoID,
                CountryID: $sessionStorage.resellerPartner.obj.countryID,
                UserID: $localStorage.user.UserID,
                VersionName: $scope.DestPlanvalue, // dest plan name
                VersionID: $scope.versiondata.VersionID, // version id
                CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
                AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
                MembershipGroupID: $sessionStorage.resellerPartner.membership.MembershipGroupID
            }

            versioningservice.copydata(copyobj).success(function (resp) {
                $localStorage.user.FinancialYearID = resp.FinancialYearID;
                $localStorage.user.FinancialYear = resp.FinancialPeriod;
                $sessionStorage.currentVersion.VersionID = resp.VersionID;
                $sessionStorage.currentVersion.VersionNo = resp.VersionNo
                $sessionStorage.currentVersion.FinancialPeriod = resp.FinancialPeriod;
                $sessionStorage.currentVersion.Financialyear = resp.FinancialPeriod;
                $localStorage.selectedversion = resp;
                $state.reload();
                Notification.success({
                    message: "Version Copied Successfully!" + $rootScope.closeNotify,
                    delay: null
                });
            })
        }
        $('#myModalcreate,#CopyModal').on('hidden.bs.modal', function () {

            $scope.drpdpwnvalue1 = null;
            $scope.DestPlanvalue = "";

        });
        var inputControls = [];
        $scope.AssignPosition = function () {
            //HSM-413 start - stop cursor reposition to end after price formatting.

            var focusedId = $(document.activeElement)[0].id;
            inputControls = [];
            var specialTextControls = $('#' + focusedId);
            angular.copy(specialTextControls, inputControls);
            var startp = 0;
            var endp = 0;
            angular.forEach(specialTextControls, function (item, key) {
                startp = item.selectionStart;
                endp = item.selectionEnd;
                // inputControls[key].selectionStart = item.selectionStart;
            })
            $(".specialText").priceFormat();
            var formattedControls = $('#' + focusedId);
            for (var i = 0; i < inputControls.length; i++) {
                if (inputControls[i].value != "0") {
                    console.log();
                }
                if (inputControls[i].value.trim().length < formattedControls[i].value.trim().length && startp > 1) {
                    formattedControls[i].setSelectionRange(startp + 1, endp + 1);
                }
                else if (inputControls[i].value.trim().length > formattedControls[i].value.trim().length &&
                     (startp > 1 || (startp === 1 && inputControls[i].value.trim()[0] === "0"))) {
                    formattedControls[i].setSelectionRange(startp - 1, endp - 1);
                }
                else
                    formattedControls[i].setSelectionRange(startp, endp);
            };
            $(focusedId).focus();
            //  HSM-413 End
        }

    }
})();