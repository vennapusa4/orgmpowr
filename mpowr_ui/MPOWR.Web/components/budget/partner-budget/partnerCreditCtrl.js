/*
Partner Credit controller: Partner credit controller logics
Created by: Aamin Khan
Created at: 19/01/2017
*/
(function () {
    'use strict';
    angular.module("hpe").controller("partnerCreditCtrl", partnerCreditCtrlFn);

    function partnerCreditCtrlFn(
        $scope,
        $rootScope,
        $timeout,
        partnerCreditFactory,
        $sessionStorage,
        modelParameterFactory,
        Notification,
        $http,
        HistoricalFactory,
        UtilityService,
        $localStorage,
        $state,
        partnerBudgetDataService,
        roleMappingService,
        versioningservice,
        budgetFactory,
        $window,
        $uibModal
        ) {
        //ngInject
        var vm = this;
        var Delay = 4000;
        $scope.buId = 0;
        $scope.chkhistory = false;
      
        var Msg = "";
        var Alloc_Msg = "Allocated MDF for the Business Unit is more than the total Baseline MDF allocated for the Business Unit.";
        var Carve_Msg = "Allocated MDF for the Business Unit is more than the total CarveOut MDF allocated for the Business Unit.";
        var Diff_Msg = "Difference between Calculated and Allocated MDF is more than + or - 20%.";
  
        var buParam = parseInt($state.params.currentBu);
        $scope.buParam = isNaN(buParam) ? 0 : buParam;
        $scope.filterText = 0;
        $scope.hideOperator = true;
        $scope.hidePartnerName = true;
        $scope.hidePartnerId = true;
        $scope.hideFilterValue2 = true;
        //$scope.Round2 = false;
        $scope.buId = $scope.buParam;
        budgetFactory.selectText();
        //$scope.versiondata = $sessionStorage.currentVersion;
        $scope.versiondata = $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion : $localStorage.selectedversion;
        if ($localStorage.selectedversion != undefined) {
            $scope.versiondata.Financialyear = $localStorage.selectedversion.FinancialPeriod;
        }
        var financial = {
            FinancialyearID: $localStorage.user.FinancialYearID,

        }
        roleMappingService.getPlanningPeriods(financial).then(function onSuccess(response) {
            $scope.disablePrevious = $rootScope.disablePrevious;
            //if ($rootScope.disablePrevious == false) {
            roleMappingService.getCheckIsActiveFlag($localStorage.selectedversion.VersionID).then(function onSuccess(response) {
                $scope.disablePrevious = $rootScope.disablePrevious;
                if ($rootScope.disablePrevious == true && $rootScope.previousPlan == false) {
                    $localStorage.NOACCESS = true;
                    $scope.NOACCESS = $localStorage.NOACCESS;
                }
                else {
                    $localStorage.NOACCESS = false;
                    $scope.NOACCESS = $localStorage.NOACCESS;
                }
                //    Notification.error({ message: 'The Plan is disabled due to any of the Geo/Country/District/BusinessUnit is made InActive', delay: null });
            })
            //}
        });
        $scope.NOACCESS = $localStorage.NOACCESS;
        //$scope.filterValue = "";
        //$scope.filterDelimeter = "";
        //$scope.filterColumn = "";
        $scope.view = "main";
        if (!isNaN($state.params.currentBu)) {
            $scope.view = "full";
        }
        if ($localStorage.user.BUs.indexOf(5) == -1) {
            $scope.isARUMSA = true;
        }
        else
            $scope.isARUMSA = false;


        var colorFunction1 = function () {
            return function (d, i) {
                return 'rgb(146, 40, 141)'
            };
        }
        $scope.role = roleMappingService.apply();
        $window.scrollTo(0, 0);
        $scope.prePeriod = $localStorage.prePeriod;
        partnerCreditFactory.pageSize = 20;
        var CountryID = $sessionStorage.resellerPartner.obj.id;
        var PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
        var FinancialYear = $sessionStorage.resellerPartner.FinencialYear;
        var DistrictID = $sessionStorage.resellerPartner.district.DistrictID;
        var versionId = $sessionStorage.currentVersion.VersionID;

        $scope.ShowRound2Tab = false;
        $scope.validData = [];
        $scope.PartnerTypeID = PartnerTypeID;
        $scope.reseller = $sessionStorage.resellerPartner
        $scope.Math = window.Math;
        $scope.noGraph = false;
        $scope.noTotalGraph = false;
        $scope.previousYear = $localStorage.previousYear.replace("_", "");

        $scope.previousPeriod = $localStorage.prePeriod;

        partnerCreditFactory.getBu($scope); //$scope.bus
        $scope.currentBu = 0;
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
        // $scope.showSpartlines = false;

        if ($localStorage.showSpartlines) {
            $scope.showSpartlines = true;
        }
        if (!$localStorage.showSpartlines) {
            $scope.showSpartlines = false;
        }

        $scope.$sessionstorage = $sessionStorage;
        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                $scope.ctries = _.pluck(countryList, 'DisplayName').join(", ");
            }
        }


        $sessionStorage.resellerPartner.CountryOrGeoOrDistrict;
        $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0);

        if (($sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0) == "C" &&
          $sessionStorage.resellerPartner.CountryOrGeoOrDistrict.indexOf("26") != -1) || ($sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0) == "C" &&
          $sessionStorage.resellerPartner.CountryOrGeoOrDistrict.indexOf("138") != -1) ||
        $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0) == "D") {
            $scope.ShowRound2Tab = true;
        }

        var processing = false;
        $scope.histories = [];
        // pagination setting

        $scope.search_url_pname = $rootScope.api + "PartnerBudget/GetPartnerBudgetSearch?VersionID=" + versionId + "&businessUnitID=0&SearchColumn=Partner_Name&find=";
        $scope.search_url_pid = $rootScope.api + "PartnerBudget/GetPartnerBudgetSearch?VersionID=" + versionId + "&businessUnitID=0&SearchColumn=PartnerID&find="

        var filter = {
            CountryID: CountryID,
            FinancialYearID: $localStorage.user.FinancialYearID,
            DistrictID: DistrictID,
            PartnerTypeID: PartnerTypeID,
            businessUnitID: $scope.buId,
            VersionID: $sessionStorage.currentVersion.VersionID,
            WithoutHistory: $scope.chkhistory
        }

        var sortColumn = "Projected_Sellout";//"Partner_name";
        var sortOrder = "desc";

        var pageSize = 30;
        var currentPage = 0;
        var filterColumn = null;
        var filterDelimeter = null;
        var filterValue = null;

        $scope.selecteFyPeriod = $localStorage.selectedversion.Financialyear;
        $scope.CurrrentFyPeriod = $localStorage.selectedversion.Financialyear.slice(5, 7) + $localStorage.selectedversion.Financialyear.slice(2, 4);

        if ($localStorage.selectedversion.Financialyear.slice(5, 7) === '1H') {
            $scope.PreviousFYear = '1H' + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString();
            $scope.PreviousToPreviousFyPeriod = '1H' + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 2).toString();
            // $scope.SelectedPreviousFyYear = "FY" + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString() + ' 1H';
            $scope.SelectedPreviousFyYear = "1H" + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString();
            $scope.PreviousFyPeriod = ' 2H' + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString();
        } else {
            $scope.PreviousFYear = '2H' + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString();
            $scope.PreviousToPreviousFyPeriod = '2H' + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 2).toString();
            //$scope.SelectedPreviousFyYear = "FY" + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString() + ' 2H';
            $scope.SelectedPreviousFyYear = "2H" + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString();
            $scope.PreviousFyPeriod = "1H" + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4))).toString();
        }

        console.log('pfyp', $scope.PreviousFyPeriod);

        //var sellout = $scope.CurrrentFyPeriod + " M-POWR Projected";
        //if($scope.PartnerTypeID == 1)
        //    sellout = sellout + " Weighted Sellout ($)"
        //else 
        //    sellout = sellout + " Sellout ($)"

        fetch(filter, sortColumn, sortOrder, false, null,null,null,'ok'); //first fetch
        loadFilter(false, false);
        //$scope.filterOptions = [
        //    { id: 1, name: "Partner ID" },
        //    { id: 2, name: "Partner Name" },
        //    { id: 3, name: "Membership Type" },
        //    { id: 4, name: sellout },
        //    { id: 5, name: "M - POWR " + $scope.CurrrentFyPeriod + " Calculated MDF" },
        //    { id: 6, name: $scope.PreviousFYear + " WMGO Ratio" },
        //    { id: 7, name: $scope.CurrrentFyPeriod + " allocated MSA" },
        //    { id: 8, name: "Allocated " + $scope.CurrrentFyPeriod + " Proposed Budget" },
        //    { id: 9, name: "Total " + $scope.CurrrentFyPeriod + " MDF" },
        //    { id: 10, name: "Carve Out" },
        //    { id: 11, name:  $scope.CurrrentFyPeriod +" MDF/" + $scope.CurrrentFyPeriod + " Projected Sellout" }

        
        //];
        //$scope.filterOptions.splice(0, 0, { id: -1, name: 'Select' });
        //$scope.selectedOption = $scope.filterOptions[0];


        $scope.clearSearch = function () {
            commonReset();
            $("#PartnerId").val("");
            $("#PartnerName").val("");
            $("#filterValue").val("");
            $scope.Partner_Name = "";
            $scope.PartnerID = "";
            $scope.filterColumn = $scope.filterList[0];
           
            

            $scope.hideOperator = true;
            $scope.hidePartnerName = false;
            $scope.hidePartnerId = true;
            $scope.hideFilterValue2 = true;

            $scope.filterDelimeter = $scope.operand[0];
            angular.element('#partnerInput').removeClass('open');
            var options = angular.element("#filterColumnSelect")[0].options;
            angular.forEach(options, function (item, key) {
                if (key == 0)
                    angular.element("#filterColumnSelect")[0].options[key].selected = true;
                else
                    angular.element("#filterColumnSelect")[0].options[key].selected = false;
                // inputControls[key].selectionStart = item.selectionStart;
            })
            //angular.element("#filterColumnSelect")[0].options[0].selected = true
        }

        $scope.WithoutHistory = function () {
            filter.WithoutHistory = $scope.chkhistory;
            $localStorage.chkhistory = $scope.chkhistory;
            sortColumn = "Projected_Sellout";
            sortOrder = "desc";
            currentPage = 0;
            $(".fa-caret-up").removeClass("fa-caret-up").addClass("fa-caret-down");
            fetch(filter, sortColumn, sortOrder, true);

        }


        function commonReset() {
            sortColumn = "Projected_Sellout";
            sortOrder = "desc";
            currentPage = 0;
            $(".fa-caret-up").removeClass("fa-caret-up").addClass("fa-caret-down")
            fetch(filter, sortColumn, sortOrder, true, null, null, null);
        }

        var processing = false;

        function fetch(filter, SortColumn, SortOrder, fromSort, filterColumn, filterDelimeter, filterValue, fetchFirst) {
            //if ($("#PartnerName").val() != undefined) {
            //    if ($("#PartnerName").val() != "")
            //        return false;
            //}
            //if ($("#PartnerId").val() != undefined) {
            //    if ($("#PartnerId").val() != "")
            //        return false;
            //}
            currentPage = currentPage + 1;
            $localStorage.chkhistory = $scope.chkhistory;
            filter.PageIndex = currentPage;
            filter.PageSize = pageSize;
            filter.SortColumn = SortColumn;
            filter.sortOrder = SortOrder;
            filter.UserID = $localStorage.user.UserID;
            filter.FilterColumn = filterColumn;
            filter.FilterDelimeter = filterDelimeter;
            filter.FilterValue = filterValue;
            processing = true;
            partnerCreditFactory.fetchData(filter)
                .then(function (data) {
                    if ($scope.buId > 0) { // iterate and build the MDF Variance id model
                        for (var i = 0; i < data.length; i++) {
                            data[i].MDF[0].MDFVarianceReasonID = $scope.buildModel(data[i].MDF[0].MDFVarianceReasonID)
                        }
                    }

                    if (fromSort == undefined) {
                        if (data != undefined && data != "") {
                            $scope.histories = $scope.histories.concat(data);
                        }

                        if ($scope.buId > 0) {
                            $timeout(function () {
                                if ($scope.role.disabled || $scope.disablePrevious || $scope.NOACCESS) {
                                    $(".businessUnit_" + $scope.buId + " input").attr("disabled", "disabled");
                                    $(".businessUnit_" + $scope.buId + " select").attr("disabled", "disabled");
                                    $(".businessUnit_" + $scope.buId + " img").off('click');
                                }
                            }, 500);

                        }
                    } else {
                        $scope.histories = data;

                        if ($scope.buId > 0) {
                            $timeout(function () {
                                if ($scope.role.disabled || $scope.disablePrevious || $scope.NOACCESS) {
                                    $(".businessUnit_" + $scope.buId + " input").attr("disabled", "disabled");
                                    $(".businessUnit_" + $scope.buId + " select").attr("disabled", "disabled");
                                    $(".businessUnit_" + $scope.buId + " img").off('click');
                                }
                            }, 500);

                        }
                    }


                    processing = false;

                    if (fetchFirst != undefined) {
                        partnerCreditFactory.initGraph($scope, $scope.buId); // generate graph
                        partnerCreditFactory.generateTotalRow(filter, $scope, true); //generate total row 
                    }
                    $timeout(function () {
                        $(".specialText").priceFormat();
                        var nc = "";
                        for (var i = 0; i < notToCollaspe.length; i++) {
                            nc = nc + " , " + notToCollaspe[i];
                        }

                        $(".summary-expand").hide();
                        try {
                            if ($scope.buId == 0) {
                                notToCollaspe = $(nc);
                                $(".graph-row").not(notToCollaspe).hide();
                            } else {
                                $(".graph-row").hide();
                                $('.fa-caret-up').removeClass("fa-caret-up").addClass('fa-caret-down');
                                $(".expanded-row-bu").hide();

                            }
                        } catch (e) {

                        }
                    }, 1000);


                })
        }

        // Search by partner name
        $scope.getPartnerName = function (partnerName) {
            // console.log(partnerName);
            $scope.Partner_Name = partnerName;
            $localStorage.chkhistory = $scope.chkhistory;
            if ($scope.Partner_Name.length > 2) {
                var searchkey = encodeURIComponent(partnerName);
                $http.get($rootScope.api + "PartnerBudget/GetPartnerBudgetSearch?VersionID=" + versionId + "&businessUnitID=" + $scope.buId + "&SearchColumn=Partner_Name&find=" + searchkey + "&WithoutHistory=" + $scope.chkhistory + "&FilterColumnName=null&FilterDelimeter=null&FilterValue=null").success(function (response) {
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
        $scope.filter = function () {
            var filterValue2;
            var filterColumn = $scope.filterColumn;
            var filterDelimeter = $scope.filterDelimeter;
            if (angular.element('#filterValue2') != undefined && angular.element('#filterValue2')[0] != undefined)
                filterValue2 = angular.element('#filterValue2')[0].value
            //var filterValue = $scope.filterText;
            filterValue = angular.element('#filterValue')[0].value;

            if (filterDelimeter.value == 'between') {
                filterValue = filterValue + ' and ' + filterValue2
            }
            $scope.filterValue = filterValue;

            currentPage = 0;
            fetch(filter, sortColumn, sortOrder, false, filterColumn.TableColumn, filterDelimeter.value, filterValue);
        }
        $scope.OnFilterColumnChange = function (filterColumn) {
            $scope.filterColumn = filterColumn;
            if (filterColumn.FilterColumnNames == 'Partner Name' || filterColumn.FilterColumnNames == 'Partner ID') {
                $scope.hideOperator = true;
                $scope.hideFilterValue2 = true;
                if (filterColumn.FilterColumnNames == 'Partner Name'){
                    $scope.hidePartnerName = false;
                    $scope.hidePartnerId = true;
                }
                    
                else {
                    $scope.hidePartnerId = false;
                    $scope.hidePartnerName = true;
                }
            }
            else {
                if ($scope.filterDelimeter != undefined && $scope.filterDelimeter.value == 'between'){
                    $scope.hideOperator = false;
                    $scope.hidePartnerName = true;
                    $scope.hidePartnerId = true;
                    $scope.hideFilterValue2 = false;
                }
                else {
                    $scope.hideOperator = false;
                    $scope.hidePartnerName = true;
                    $scope.hidePartnerId = true;
                    $scope.hideFilterValue2 = true;
                }
            }

        }
        $scope.OnFilterDelimeterChange = function (filterDelimeter) {
            if (filterDelimeter.value != 'between')
            {
                $scope.hideFilterValue2 = true;
            }
            else {
                $scope.hideFilterValue2 = false;
            }
            $scope.filterDelimeter = filterDelimeter;

        
        }
        $scope.selectedName = function (name, index) {
            angular.element('#partnerInput').removeClass('open');

            $scope.partnersDetails = $scope.partnersDetails[index];
            $scope.partnersDetails._parent = index;
            $scope.partnersDetails.MDF[0].MDFVarianceReasonID = partnerCreditFactory.buildModel($scope.partnersDetails.MDF[0].MDFVarianceReasonID, $scope)

            $scope.histories = [$scope.partnersDetails];
            console.log($scope.partnersDetails);

            angular.element("#PartnerName").val(name);
            $("#PartnerId").val("");
            $timeout(function () {
                $(".graph-row").hide();
            }, 300)


        }

        $scope.setNext = function () {
            $rootScope.next = true;
        }
        $scope.setPrev = function () {
            $rootScope.prev = true;
        }



        // Search by partner id
        $scope.getPartnerId = function (PartnerID) {

            $scope.PartnerID = PartnerID;
            $localStorage.chkhistory = $scope.chkhistory;
            if ($scope.PartnerID.length > 1) {
                $http.get($rootScope.api + "PartnerBudget/GetPartnerBudgetSearch?VersionID=" + versionId + "&businessUnitID=" + $scope.buId + "&SearchColumn=PartnerID&find=" + $scope.PartnerID + "&WithoutHistory=" + $scope.chkhistory + "&FilterColumnName=" + $scope.filterColumn + "&FilterDelimeter=" + $scope.filterDelimeter + "&FilterValue=" + $scope.filterValue).success(function (response) {
                    $scope.PartnerIDs = response;
                })
                 .error(function () {


                 });
            }
            if ($scope.PartnerID.length == 0) {
                if ($scope.Partner_Name.length == 0) {
                    commonReset();
                }
            }
        }

        $scope.selectedPartnerID = function (id, index) {
            angular.element('#partnerInput').removeClass('open');
            angular.element("#PartnerId").val(id);
            $scope.PartnerID = id;
            $scope.partnerData = $scope.PartnerIDs[index];
            $scope.partnerData._parent = index;
            $scope.partnerData.MDF[0].MDFVarianceReasonID = partnerCreditFactory.buildModel($scope.partnerData.MDF[0].MDFVarianceReasonID, $scope)

            $scope.histories = [$scope.partnerData];
            $("#PartnerName").val("");
            $timeout(function () {
                $(".graph-row").hide();
            }, 300)

        }


        partnerCreditFactory.getReasons($scope)

        /*;
        partnerCreditFactory.getData($scope, $scope.buId).then(function(){
            ifFullScreen();
        });*/



        var lazyTo = partnerCreditFactory.pageSize;
        var lazySize = partnerCreditFactory.pageSize;

        /*Invoke lazy loading with scope*/
        $scope.lazyLoading = function () {
            lazyLoading();
        }

        /*Lazy loading*/

        function lazyLoading() {

            $('.lazy-applicable').on('scroll', function () {

                var currentPos = $(this).scrollTop()
                $(".sticky-table table").scrollTop(currentPos)

                if (($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) && !processing) {
                    if ($scope.filterColumn != undefined)
                        filterColumn = $scope.filterColumn.TableColumn;
                    if ($scope.filterDelimeter != undefined)
                        filterDelimeter = $scope.filterDelimeter.value;
                    if ($scope.filterText != undefined && angular.element('#filterValue') != undefined && angular.element('#filterValue')[0] != undefined) {
                        filterValue = angular.element('#filterValue')[0].value;
                    if (angular.element('#filterValue2') != undefined && angular.element('#filterValue2')[0] != undefined)
                        filterValue2 = angular.element('#filterValue2')[0].value
                    //var filterValue = $scope.filterText;
                    //filterValue = angular.element('#filterValue')[0].value;

                    if (filterDelimeter == 'between') {
                        filterValue = filterValue + ' and ' + filterValue2
                    }
                    $scope.filterValue = filterValue;
                        //filterValue = $scope.filterText;
                    }
                    fetch(filter, sortColumn, sortOrder, undefined, filterColumn,filterDelimeter, filterValue);
                    $scope.$apply();
                }
            })



        }
        $(".sticky-table table").on("scroll", function () {
            alert();
        })

       
        /*Spark line checkbox functionality*/
        $scope.$watch('showSpartlines', function (oldVal, newVal) {
            if ($scope.showSpartlines) {
                $localStorage.showSpartlines = true;
            }
            if (!$scope.showSpartlines) {
                $localStorage.showSpartlines = false;
            }


            if (oldVal) {
                $timeout(function () {
                    $(".graph-row").css("height", "224px");
                }, 10)
            } else {
                $timeout(function () {
                    $(".graph-row").css("height", "41px")
                }, 10)

            }
        })


        function spark() {
            if ($localStorage.showSpartlines) {
                $timeout(function () {
                    $(".graph-row").css("height", "224px");
                }, 10)
            } else {
                $timeout(function () {
                    $(".graph-row").css("height", "41px")
                }, 10)

            }
        }


        $scope.expand = true;
        /* Toogle graphs*/
        $scope.hideShowGraph = function () {
            var elem = $(".partner-credit .data")
            $scope.expand = !$scope.expand;
            if ($scope.expand) {
                //elem.css("margin-top","250px")


                $(".lazy-applicable").removeClass("hiden-graph")

            } else {
                //elem.css("margin-top","37px");

                $(".lazy-applicable").addClass("hiden-graph")
            }

        }


        /*Pagination - go to page by number - DEPRICATED */
        $scope.gotoPage = function (pageNumber) {
            partnerCreditFactory.gotoPage(pageNumber, $scope)
        }

        /* Pagination - NEXT Page*/
        $scope.next = function () {
            partnerCreditFactory.gotoPage($scope.currentPage + 1, $scope)
        }
        /* Pagination - Prev Page*/
        $scope.prev = function () {
            partnerCreditFactory.gotoPage($scope.currentPage - 1, $scope)
        }

        //export to excel SFDC
        $scope.exportSFDC = function () {
            partnerBudgetDataService.ExportSDFC({
                "CountryID": CountryID,
                "FinancialyearID": $localStorage.user.FinancialYearID,
                "DistrictID": DistrictID,
                "PartnerTypeID": PartnerTypeID
            })
        }

        //export to excel
        $scope.export = function () {
            partnerBudgetDataService.Export({
                "CountryID": CountryID,
                "FinancialyearID": $localStorage.user.FinancialYearID,
                "DistrictID": DistrictID,
                "PartnerTypeID": PartnerTypeID
            })
        }

        /*CHECK WIDTH - DEPRICATED*/
        $scope.checkWidth = function (sellout, mdf, mdfBySellout) {
            partnerCreditFactory.checkWidth($scope, sellout, mdf, mdfBySellout);
        }

        /*NEXT / PREV Graph */
        $scope.showGraph = function (toShow) {
            if (toShow == 'right') {

                $(".graph-set-2").hide();
                $(".graph-set-1").show();

                $(".left-button").attr("src", "assets/resources/previous@1x.png");
                $(".right-button").attr("src", "assets/resources/next_on_hover@1x.png");
            } else {
                $(".right-button").attr("src", "assets/resources/next@1x.png");
                $(".left-button").attr("src", "assets/resources/previous_on_hover@1x.png");
                $(".graph-set-2").show();
                $(".graph-set-1").hide();
            }
        }








        /* Dynamic Cols - Depricated*/
        $scope.partnerCols = [
            { id: 1, name: "Partner ID" },
            { id: 2, name: "PBM" },
            { id: 3, name: "PMM" },
            { id: 4, name: "Ranking among peers" },
            { id: 5, name: "delta between sell-in and sell-out" },
            { id: 6, name: "Previous Period Sell Out Growth" },
            { id: 7, name: "Last Quarter SOW" },
            { id: 8, name: "SOW growth" },
            { id: 9, name: "Footprint growth" },
            { id: 10, name: "Total MDF" },
            { id: 11, name: "Incremental MDF" },
            { id: 12, name: "Late MDF" },
            { id: 13, name: "(W)MGO/marketing MDF" },
            { id: 14, name: "New logos/MGO" },
            { id: 15, name: $scope.SelectedPreviousFyYear + " MGO" },
            { id: 16, name: $scope.SelectedPreviousFyYear + " MGO ROI" },
            { id: 17, name: $scope.SelectedPreviousFyYear + " (W) MGO" },
            { id: 18, name: $scope.SelectedPreviousFyYear + " (W) MGO ROI" },
            { id: 19, name: $scope.SelectedPreviousFyYear }
        ];



        $scope.historyCols = [
            { id: 1, name: 'Sell-in' },
            { id: 2, name: 'Sell-in growth' }
        ];


        $scope.selectCols = function (showDropDown) {
            $timeout(tx, 100);
            function tx() {
                $(".extra-cols .dropdown-menu").hide();
            }
        }

        /* Dynamic cols -  depricated*/
        $scope.selectedPartner = [null, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false];
        $scope.msExpnd = false; $scope.selloutColExpnd = false; $scope.mdfColExpnd = false;

        $scope.selectExtraCols = function () {
            partnerCreditFactory.checkWidth($scope, $scope.selloutColExpnd, $scope.mdfColExpnd, $scope.msExpnd);
        }


        /*Extra cols - DEPRICATED*/
        $scope.show = function () {
            $(".extra-cols .dropdown-menu").show();
        }
        $scope.tempMDF = null
        $scope.mdfChartOption = partnerCreditFactory.getChartOption("#00B188", 80, 'mdf', $scope);
        $scope.selloutChartOption = partnerCreditFactory.getChartOption("#A0419D", 80, 'sellout', $scope);

        $timeout(function () {
            $(".graph-row").hide();
            $(".formated-price").priceFormat();
        }, 300);

        var notToCollaspe = [];

        /* TOOGLE rows in total tabs*/
        $scope.toogle = function (index, partnerType, partner, isBu) {
            var d = "#" + partnerType + "-graph-row-" + index;
            var div = $(d);
            notToCollaspe = d;

            if (isBu == 'true') { // if for business Unit
                $scope.currentPartner = $scope.histories[index];
                $(".graph-row").hide();
                var elem = $("#icon-" + index);
                var curObj = this;
                if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                    div.show();
                    elem.removeClass('fa-caret-down').addClass('fa-caret-up');

                } else {
                    elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                    div.hide();
                }
            } else {
                partnerCreditFactory.getPartnerBu(partner.MSA[0].PartnerBudgetID, $scope, index, partnerType);
            }
        }


        $scope.toogleCurrent = function (index, partnerType, partner, isBu) {
           
            $rootScope.isbuu = false;
            $scope.Varid = 0;
            var d = "#" + partnerType + "-graph-row-" + index;
            var div = $(d);
            $scope.histories[index]._parent = index;
            var elem = $("#icon-" + index);
            notToCollaspe.push("." + partnerType + "-graph-row-" + partner.PartnerID);

            if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                var index = index;
                partnerBudgetDataService
                .getPartnerBu(partner.MSA[0].PartnerBudgetID)
                    .then(function (resp) {

                        $timeout(function () {

                            var currentBus = JSON.parse(resp.data);
                            for (var i = 0; i < currentBus.length; i++) {
                                if (currentBus[i].MDF[0].MDFVarianceReasonID != 0) {
                                    var Result = $scope.reason.filter(function (e) { return currentBus[i].MDF[0].MDFVarianceReasonID == e.MDFVarianceReasonID }).length == 0
                                    //_.filter($scope.reason, function (x) { return x.MDFVarianceReasonID == currentBus[i].MDF[0].MDFVarianceReasonID; });// 
                                    if (Result == true) {
                                        var object = {
                                            MDFVarianceReasonID: currentBus[i].MDF[0].MDFVarianceReasonID,
                                            ShortName: currentBus[i].MDF[0].ReasonForVariance,
                                            Reason: currentBus[i].MDF[0].ReasonForVariance

                                        }
                                        $scope.histories[index].businessUnits[i].MDF[0].MDFVarianceReasonID = object;
                                        $scope.reason.push(object);
                                        // $scope.reason = _.indexBy($scope.reason, MDFVarianceReasonID);
                                        $scope.reason.sort(function (a, b) {
                                            var nameA = a.MDFVarianceReasonID, nameB = b.MDFVarianceReasonID
                                            if (nameA < nameB) //sort string ascending
                                                return -1
                                            if (nameA > nameB)
                                                return 1
                                            return 0 //default return value (no sorting)
                                        });

                                    }

                                    $scope.$apply();
                                }
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
                        }, 500)
                        $scope.histories[index].businessUnits = JSON.parse(resp.data);
                        elem.removeClass('fa-caret-down').addClass('fa-caret-up');

                        for (var i = 0; i < $scope.histories[index].businessUnits.length; i++) {
                            $scope.histories[index].businessUnits[i].MDF[0].MDFVarianceReasonID = partnerCreditFactory.buildModel($scope.histories[index].businessUnits[i].MDF[0].MDFVarianceReasonID, $scope)

                        }
                        partnerCreditFactory.generateChartData($scope.histories[index].businessUnits); // data(HistoryMDF,HistorySellout) not coming from api    
                        if ($scope.showSpartlines) {
                            $timeout(function () {
                                $(".graph-row").css("height", "224px");
                                $(".specialText").priceFormat();
                            }, 100)
                        } else {
                            $timeout(function () {
                                $(".graph-row").css("height", "41px");
                                $(".specialText").priceFormat();
                            }, 100)

                        }
                    })

            } else {

                elem.removeClass('fa-caret-up').addClass('fa-caret-down');
                $scope.histories[index].businessUnits = null;

            }




        }


        /*Toogle tabs (graphs) in BU tabs*/
        $scope.toogleInBU = function (index, partnerType, partner, isBu) {
            $scope.noGraph = false;
            $scope.bx = [];

            $scope.currentPartner = $scope.histories[index];
            $(".graph-row").hide();
            var elem = $("#icon-" + index);
            var curObj = this;

            var d = "#" + partnerType + "-graph-row-" + index;
            var div = $(d);
            notToCollaspe.push("." + partnerType + "-graph-row-" + partner.PartnerID);
            $(".fa-caret-up").not(elem).removeClass("fa-caret-up").addClass("fa-caret-down")

            if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                div.show();
                elem.removeClass('fa-caret-down').addClass('fa-caret-up');
                $http({
                    method: 'GET',
                    url: $rootScope.api + "PartnerBudget/GetPartnerBudgetBUDetails?PartnerBudgetID=" + partner.MSA[0].PartnerBudgetID + "&VersionID=" + $sessionStorage.currentVersion.VersionID + "&WithoutHistory=" + $scope.chkhistory,
                }).then(function (resp) {

                    if (resp.data.length == 0) {
                        return
                    }
                    var ref = JSON.parse(resp.data);

                    if (ref[0].HistoryMDF == undefined || ref[0].HistorySellout == undefined) {
                        div.hide();
                        Notification.error("MDF/Sellout data is not available.")
                        return
                    }


                    var a = JSON.parse(resp.data);
                    a = _.filter(a, function (o) { return o.BusinessUnitID == $scope.buId; });
                    if (a.length == 0) {
                        $scope.noGraph = true;
                    } else {
                        $scope.noGraph = false;
                        $scope.bx = partnerCreditFactory.generateChartForBU(a); // data(HistoryMDF,HistorySellout) not coming from api 
                        $scope.optionsMDF = generateMDFOption($scope.bx);
                        $scope.optionSellout = generateSelloutOption($scope.bx);
                    }
                    var graph = "." + partnerType + "-graph-row-" + partner.PartnerID;
                    $timeout(function () {
                        $(".specialText").priceFormat();
                        $(graph).show();
                    }, 1000)

                })
            } else {
                elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                div.hide();
            }

        }

        /*Show total rows*/

        $scope.toogleTotal = function () {
            $(".graph-row").hide();
            var elem = $('.total-expand-btn');
            var curObj = this;
            var div = $('.summary-expand');


            var objToPass = {
                CountryID: CountryID,
                FinancialYearID: $localStorage.user.FinancialYearID,
                DistrictID: DistrictID,
                PartnerTypeID: PartnerTypeID,
                PageIndex: 0,
                PageSize: 0,
                businessUnitID: $scope.buId
            }

            if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                //partnerCreditFactory.generateTotalRow(objToPass, $scope, false);
                div.show();
                elem.removeClass('fa-caret-down').addClass('fa-caret-up');
                spark();
            } else {
                elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                div.hide();
            }
        }






        /* Expand cols */
        /*$scope.expand = function (cls, e) {
            if (e) {
                $("." + cls).css("width", "10%")
            } else {
                $("." + cls).css("width", "20%")
            }
        }*/

        $scope.expandCols = function (cls, e) {
            if (e) {
                $("." + cls).css("width", "10%")
            } else {
                $("." + cls).css("width", "20%")
            }
        }


        $scope.getTotal = function (BU, skey, key) {
            var sum = 0;
            if (typeof BU !== 'undefined') {
                for (var i = 0; i < BU.length; i++) {
                    try {
                        sum += BU[i][skey][0][key];
                    } catch (e) {

                        sum = sum + 0
                    }
                }
                return sum;
            }
            return sum;


        }


        $scope.getTotalHead = function (BU, Key) {
            var sum = 0;
            try {
                for (var i = 0; i < BU.length; i++) {
                    val = !BU[i][key] ? 0 : BU[i][key];
                    sum += val;
                }
                return sum;
            } catch (e) {
                return 0;
            }
        }



        $scope.calculateForBU = function (key, subKey, buId) {
            return partnerCreditFactory.calculateTotalByBUID(key, subKey, buId, $scope.histories);
        }
        $scope.hideGraph = function () {
            $timeout(function () {
                $(".graph-row").hide();
            }, 300);
        }


        $scope.selectBu = function (index, buId) {
            $scope.currentBu = index;
            $scope.buParam = index;
            currentPage = 0;
            $scope.buId = buId;
            filter.businessUnitID = buId;
            filter.WithoutHistory = $scope.chkhistory;
            $localStorage.chkhistory = $scope.chkhistory;
            $scope.histories = [];
            $scope.total = {};
            $("#dx").removeClass("fa-caret-up").addClass("fa-caret-down");
            partnerCreditFactory.initGraph($scope, buId); // generate graph
            partnerCreditFactory.generateTotalRow(filter, $scope, true);
            fetch(filter, sortColumn, sortOrder, false)
            if ($localStorage.user.BUs.indexOf(buId) > -1) {
                $rootScope.isbuu = false;

            } else {
                $rootScope.isbuu = true;

            }


            $timeout(function () {
                $("#PartnerId").val("");
                $("#PartnerName").val("");
                $(".fa-caret-up").removeClass("fa-caret-up").addClass("fa-caret-down")
                $scope.Partner_Name = "";
                $scope.PartnerId = "";
            }, 10)

        }

        $scope.getMyBu = function (BUs, key, subKey) {

            var unit = _.find(BUs, function (o) { return o.BusinessUnitID == $scope.buId; })
            if (typeof unit != 'undefined') {
                return unit[key][0][subKey];
            } else {
                return 0;
            }

        }





        // save from Business Unit

        $scope.saveFromBU = function (i, e, history, fromSelect, busave) {

            if ($scope.calculateForBuSpecific(history, i, e, fromSelect)) {
                // no action

            } else {
                var partner = $scope.histories[i];
                var object = {
                    PartnerBUBudgetID: partner.MDF[0].PartnerBUBudgetID,
                    PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                    BusinessUnitID: $scope.buId,
                    Baseline_MDF: partner.MDF[0].BaseLineMDF,
                    CarveOut: partner.CarveOut,
                    BU_MSA: partner.BU_MSA,
                    TotalMDF: partner.TotalMDF,
                    Comments: partner.MDF[0].Comments,
                    MDFVarianceReasonID: typeof partner.MDF[0].MDFVarianceReasonID != 'undefined' ? partner.MDF[0].MDFVarianceReasonID.MDFVarianceReasonID : null,
                    UserID: $localStorage.user.UserID,
                    VersionID: $sessionStorage.currentVersion.VersionID
                }

                partnerCreditFactory.saveEditedData(object, false, busave, $scope, partner.MSA[0].PartnerBudgetID, partner.PartnerID, "buSave");
            }

            $timeout(function () {
                $(".specialText").priceFormat();
            }, 1000)

        }

        $scope.buildModel = function (obj) {
            var o = _.filter($scope.reason, function (o) { return o.MDFVarianceReasonID == obj; });
            obj = o[0];
            return obj
        }



        $scope.buildModel2 = function (obj) {
            if (typeof obj != 'undefined') {
                var o = _.filter($scope.reason, function (o) { return o.MDFVarianceReasonID == obj.MDFVarianceReasonID; });
                obj = o;
                return obj
            }
        }

        var colorFunction = function () {
            return function (d, i) {
                if (i % 2 !== 0) {
                    return '#333'
                } else {
                    return '#18b488'
                }
            };
        };





        $scope.copy = function () {
            for (var i = 0; i < $scope.histories.length; i++) {
                var BU = $scope.histories[i].BU;
                for (var j = 0; j < BU.length; j++) {
                    var ox = BU[j].MDF[0];
                    console.log(ox)
                    if (ox.BaseLineMDF == 0) {
                        ox.BaseLineMDF = ox.Recommended_MDF
                    }
                }
            }
        }


        /*ENLARGE GRAPH*/
        $scope.enlargeGraph = function (title, option, data) {
            partnerCreditFactory.enlargeGraph($scope, title, option, data)
        }

        $scope.enlargeDonut = function (title, option, data) {
            partnerCreditFactory.enlargeDonut($scope, title)
        }
        var oldMSA = 0;
        $scope.copyValue = function (o) {
            oldMSA = o;
        }
        var oldARUBAMSA = 0;
        $scope.copyARUBAMSAValue = function (o) {
            oldARUBAMSA = o;
        }



        /*Save MSA*/
        $scope.saveMSA = function (index, trggierfrom) {

            var partner = $scope.histories[index];
            if (trggierfrom == "MSA") {

                if (typeof (partner.MSA[0].MSAValue) == "number") {

                }
                else {
                    partner.MSA[0].MSAValue = parseInt(partner.MSA[0].MSAValue.replace(/,/g, ""));
                }

                if (typeof (oldMSA) == "number") {

                }
                else {
                    oldMSA = parseInt(oldMSA.replace(/,/g, ""));
                }

                if ($scope.graphData.ARUBAMSAGraph[0] == 'undefined') {
                    console.log('aa');
                }
                if ((partner.MSA[0].MSAValue - oldMSA) > $scope.graphData.MSAGraph[0].Remains) {

                    Notification.error({ message: "Sum of MSA allocated for partners should not exceed Total MSA allocated." + $rootScope.closeNotify, delay: Delay });
                    $scope.Varid = e.currentTarget.id;
                    partner.MSA[0].MSAValue = oldMSA;
                    $timeout(function () {
                        $(".specialText").priceFormat();
                    }, 1000)

                    return
                }
                else {
                    var object = {
                        MSA: $scope.histories[index].MSA[0].MSAValue,
                        Aruba_MSA: $scope.histories[index].MSA[0].ArubaMsa,
                        PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                        UserID: $localStorage.user.UserID,
                        ModifiedBy: $localStorage.user.UserID,
                        VersionID: $sessionStorage.currentVersion.VersionID
                    }
                    partnerCreditFactory.saveMSA(object, $scope).then(function (resp) {
                        partnerCreditFactory.generateTotalRow(filter, $scope, true);
                    })
                }
            }
            if (trggierfrom == "ARUBA MSA") {

                if (typeof (partner.MSA[0].ArubaMsa) == "number") {

                }
                else {
                    partner.MSA[0].ArubaMsa = parseInt(partner.MSA[0].ArubaMsa.replace(/,/g, ""));
                }

                if (typeof (oldARUBAMSA) == "number") {

                }
                else {
                    oldARUBAMSA = parseInt(oldARUBAMSA.replace(/,/g, ""));
                }

                if ((partner.MSA[0].ArubaMsa - oldARUBAMSA) > $scope.graphData.ARUBAMSAGraph[0].Remains) {
                    Notification.error({ message: "Sum of ARUBA MSA allocated for partners should not exceed Total ARUBA MSA allocated." + $rootScope.closeNotify, delay: Delay });
                     partner.MSA[0].ArubaMsa = oldARUBAMSA;
                    $timeout(function () {
                        $(".specialText").priceFormat();
                    }, 1000)

                    return
                }
                else {
                    var object = {
                        MSA: $scope.histories[index].MSA[0].MSAValue,
                        Aruba_MSA: $scope.histories[index].MSA[0].ArubaMsa,
                        PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                        ModifiedBy: $localStorage.user.UserID,
                        VersionID: $sessionStorage.currentVersion.VersionID

                    }
                     partnerCreditFactory.saveMSA(object, $scope).then(function (resp) {
                        partnerCreditFactory.generateTotalRow(filter, $scope, true);
                    })
                }
            }
            $timeout(function () {
                $(".specialText").priceFormat();
            }, 1000)


        }

        /*get index by BU ID*/

        $scope.getIndexByBUID = function (BUId, bus) {
            for (var i = 0; i < bus.length; i++) {
                if (bus[i].BusinessUnitID == BUId) {
                    return i;
                }
            }
            return -1

        }

        $scope.PreSave = function (pi) {


            var i = $scope.getIndexByBUID($scope.buId, $scope.histories[pi].BU);

            var object = {
                PartnerBUBudgetID: $scope.histories[pi].BU[i].MDF[0].PartnerBUBudgetID,
                PartnerBudgetID: $scope.histories[pi].BU[i].PartnerBudgetID,
                BusinessUnitID: $scope.histories[pi].BU[i].Sellout[0].BusinessUnitID,
                Baseline_MDF: $scope.histories[pi].BU[i].MDF[0].BaseLineMDF,
                Comments: $scope.histories[pi].BU[i].MDF[0].Comment,
                MDFVarianceReasonID: typeof $scope.histories[pi].BU[i].MDF[0].MDFVarianceReasonID != 'undefined' ? $scope.histories[pi].BU[i].MDF[0].MDFVarianceReasonID.MDFVarianceReasonID : null,
                UserID: $localStorage.user.UserID,
                VersionID: $sessionStorage.currentVersion.VersionID
            }
            partnerCreditFactory.sumUpBaslineMDF($scope.histories[pi].BU, $scope)
            partnerCreditFactory.saveEditedData(object);
        }




        function generateMDFOption(data) {



            var maxMDF = parseFloat((_.max(data[0].mdfHistory[0].values, function (o) { return parseFloat(o.y); })).y);


            return {
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
                    color: colorFunction(),
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
        }


        function generateSelloutOption(data) {

            var maxSellout = parseFloat((_.max(data[0].selloutHistory[0].values, function (o) { return parseFloat(o.y); })).y);

            return {
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
                    color: colorFunction1(),
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
        }


        $scope.optionsMDF = {
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
                color: colorFunction(),
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
                }
            }
        }

        $scope.TotalOptionsMDF = {
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
                color: colorFunction(),
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
                }
            }
        }



        $scope.optionSellout = {
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
                color: colorFunction1(),
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
                }
            }

        }

        $scope.TotalOptionSellout = {
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
                color: colorFunction1(),
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
                }
            }

        }



         var copiedBaseline = 0;
        $scope.copyTotalVal = function (baslineVal) {
        //    totalBaslineMDF = $scope.total.BaseLineMDF;
            copiedBaseline = baslineVal
        }

     
       
        $scope.Popover = function (e) {
            var yellow = e.currentTarget.classList.value.indexOf('yelow');
            var red = e.currentTarget.classList.value.indexOf('rd');
            if (yellow > 0) {
                e.currentTarget.setAttribute("title", Msg);
            }
            //else if (red > 0) {
            //    e.currentTarget.setAttribute("title", Aruba_Msg);
            //}
            else e.currentTarget.removeAttribute("title");
        }

        $scope.saveOnEnter = function (pi, i, BU, fromSelect, busave, keyEvent) {
            if (keyEvent.which === 13) {
              //  $scope.Varid = keyEvent.currentTarget.id;
                saveAllocatedMDF(pi, i, keyEvent, BU, fromSelect, busave)
                
            }

        }

        $scope.save = function (pi, i, e, BU, fromSelect, busave) {
            saveAllocatedMDF(pi, i, e, BU, fromSelect, busave)
        }
        // save from total
        function saveAllocatedMDF(pi, i, e, BU, fromSelect, busave) {

            if ($scope.calculate(pi, i, e, BU, fromSelect)) {

            } else {
                //$("#baseline_"+i).parent().next().find("div").find("select").focus()
                var partner = $scope.histories[pi];
                var object = {
                    PartnerBUBudgetID: BU.MDF[0].PartnerBUBudgetID,
                    PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                    BusinessUnitID: BU.BusinessUnitID,
                    Baseline_MDF: BU.MDF[0].BaseLineMDF,
                    CarveOut: BU.CarveOut,
                    TotalMDF: BU.TotalMDF,
                    BU_MSA: BU.BU_MSA,
                    Comments: BU.MDF[0].Comment,
                    MDFVarianceReasonID: typeof BU.MDF[0].MDFVarianceReasonID != 'undefined' ? BU.MDF[0].MDFVarianceReasonID.MDFVarianceReasonID : null,
                    UserID: $localStorage.user.UserID,
                    VersionID: $sessionStorage.currentVersion.VersionID
                }
                partnerCreditFactory.saveEditedData(object, false, busave, $scope, partner.MSA[0].PartnerBudgetID, partner.PartnerID);

            }

        }

        $scope.trimm = function(value)
        {
            if (typeof (value) == "number") {
                return value;
            }
            else
            {
                value = parseInt(value.replace(/,/g, ""));
                return value;
            }
        }
      
        $scope.calculate = function (pi, i, e, bu, fromSelect) {
            var ret = false;
            var chk = false;
            var theBU = _.find($scope.graphData.AllocatedGraph, function (o) { return o.Business_Unit == bu.BusinessUnitID; });

            var carveBU = _.find($scope.graphData.CarveOutGraph, function (o) {return o.Business_Unit == bu.BusinessUnitID;});

            var MDFValue = 0;
            var Carve = false;
 
            var msa =  bu.BU_MSA ;
            var base =  bu.TotalMDF ;

            if (e.currentTarget.id.indexOf("CarveOut_") != -1) {
                MDFValue = $scope.trimm(bu.CarveOut);
                Carve = true;
            }
            else {
                     MDFValue = $scope.trimm(msa) + $scope.trimm(base);
              }
            
            if ( MDFValue - $scope.trimm( bu.MDF[0].BaseLineMDF )  > theBU.Remaining && fromSelect && Carve == false) {
                 Msg = Alloc_Msg;
                 if ($localStorage.user.IsOverAllocation == false)
                {
                    $timeout(function () {
                        $(".specialText").priceFormat();
                    }, 1000)
                    if (e.currentTarget.id.indexOf("BU_MSA_") != -1) {
                        bu.BU_MSA = copiedBaseline;
                    }
                    else {
                        bu.TotalMDF = copiedBaseline;
                    }
                    $scope.Varid = 0;
                    Notification.error(Alloc_Msg);
                    return true
                }
                else
                {
                    $scope.Varid = e.currentTarget.id;
                    chk = true;
                }
             }


            var recomenededMDF = bu.MDF[0].Recommended_MDF;
            var max = (recomenededMDF * 20) / 100 + recomenededMDF;
            var min = recomenededMDF - (recomenededMDF * 20) / 100;

            if ((MDFValue > max || MDFValue < min) && typeof bu.MDF[0].MDFVarianceReasonID == 'undefined' && Carve == false) {
                chk = true;
                $scope.Varid = e.currentTarget.id;
                Msg = Diff_Msg;
            }

            if (carveBU.Total_CarveOut - MDFValue < 0 && fromSelect && Carve) {
                $scope.Varid = e.currentTarget.id;
                Msg = Carve_Msg;
                chk = true;
            }

            if (!chk) {
                $scope.Varid = 0;
            }

              var partner = $scope.histories[pi];

            //Sum up to partner
            var sum = 0;
            var sum1 = 0;
            var sum2 = 0;
            var buX = $scope.histories[pi].businessUnits;

            for (var i = 0; i < buX.length; i++) {
                var cellvalue = buX[i].TotalMDF.toString();
                cellvalue = cellvalue.replace(/,/g, "");
                sum += parseInt(cellvalue);
            }
            partner.TotalMDF = sum;

            for (var i = 0; i < buX.length; i++) {
                var cellvalue = buX[i].BU_MSA.toString();
                cellvalue = cellvalue.replace(/,/g, "");
                sum2 += parseInt(cellvalue);
            }

            partner.BU_MSA = sum2;

            partner.MDF[0].BaseLineMDF = partner.TotalMDF + partner.BU_MSA;

             for (var i = 0; i < buX.length; i++) {
                var cellvalue = buX[i].CarveOut.toString();
                cellvalue = cellvalue.replace(/,/g, "");
                sum1 += parseInt(cellvalue);
            }

            partner.CarveOut = sum1;


         

            $timeout(function () {
                $(".specialText").priceFormat();
            }, 1000)
            return ret;
        }

        var mdfBuScreen = 0;
        //var x = 0;
        //var tBux = 0;
        $scope.copyFromBu = function (val) {
            mdfBuScreen = val;
            //tBux = $scope.total.BaseLineMDF;
            //x = $scope.total.BaseLineMDF;
        }
        $scope.calculateForBuSpecific = function (partner, i, e, fromSelect) {
            var ret = false;
            var chk = false;
            var recomenededMDF = partner.MDF[0].Recommended_MDF;
            var max = (recomenededMDF * 20) / 100 + recomenededMDF;
            var min = recomenededMDF - (recomenededMDF * 20) / 100;

            //$scope.total.BaseLineMDF = tBux - mdfBuScreen + partner.MDF[0].BaseLineMDF;


            var theBU = _.find($scope.graphData.AllocatedGraph, function (o) { return o.Business_Unit == $scope.buId; });
            var carveBU = _.find($scope.graphData.CarveOutGraph, function (o) { return o.Business_Unit == $scope.buId;});


            var MDFValue = 0;
            var Carve = false;

            var msa = partner.BU_MSA == undefined ? 0 : partner.BU_MSA;
            var base = partner.TotalMDF == undefined ? 0 : partner.TotalMDF;

            if (e.currentTarget.id.indexOf("CarveOut_") != -1) {
                MDFValue = $scope.trimm(partner.CarveOut);  
                Carve = true;
            }
            else {
                MDFValue = $scope.trimm(msa) + $scope.trimm(base); 
            }


            if (MDFValue - $scope.trimm(partner.MDF[0].BaseLineMDF) > theBU.Remaining && fromSelect && Carve == false) {
                  Msg = Alloc_Msg;

                  if ($localStorage.user.IsOverAllocation == false) {
                     $timeout(function () {
                         $(".specialText").priceFormat();
                     }, 1000)
                     if (e.currentTarget.id.indexOf("BU_MSA_") != -1) {
                         partner.BU_MSA = mdfBuScreen;
                     }
                     else {
                         partner.TotalMDF = mdfBuScreen;
                     }
                     $scope.Varid = 0;
                     Notification.error(Alloc_Msg);
                     return true
                 }
                 else {
                     $scope.Varid = e.currentTarget.id;
                     chk = true;
                 }

            } else {

            }

            if ((MDFValue > max || MDFValue < min) && typeof partner.MDF[0].MDFVarianceReasonID == 'undefined'  && Carve == false) {

                chk = true;
                $scope.Varid = e.currentTarget.id;
                Msg = Diff_Msg;
            }
 
              if (carveBU.Total_CarveOut -MDFValue < 0 && fromSelect && Carve) {
                chk = true;
                $scope.Varid = e.currentTarget.id;
                Msg = Carve_Msg;
               
             }

              if (!chk) {
                  $scope.Varid = 0;
              }

            $timeout(function () {
                $(".specialText").priceFormat();
            }, 1000)
            return ret;

        }

        $scope.typeCheck = function (o) {

            return typeof o == 'undefined' || o.length == 0 ? true : false;
        }

        $scope.sort = function (superKey, key, elemId) { // key = PartnerName

            var elem = $("#" + elemId);
            var icon = elem.find("i").attr("class").split(" ")[2];
            if (icon == "ng-scope") {
                icon = elem.find("i").attr("class").split(" ")[3];
            }
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
            if (angular.element('#filterValue2') != undefined && angular.element('#filterValue2')[0] != undefined)
                filterValue2 = angular.element('#filterValue2')[0].value

            if (filterDelimeter == 'between') {
                filterValue = filterValue + ' and ' + filterValue2
            }
            $scope.filterValue = filterValue;
                

            fetch(filter, sortColumn, toSort, true, filterColumn, filterDelimeter, filterValue);

            /*$scope.data = UtilityService.sortBy($scope.data, superKey, key, toSort);
            partnerCreditFactory.applyFilter($scope);*/
        }

        $scope.multiSort = function (elemId, superKey, key1, key2) { //
            //console.log(elemId,superKey,key1,key2);

            var elem = $("#" + elemId);
            var icon = elem.find("i").attr("class").split(" ")[2];
            var toSort = 'asc';
            if (icon === 'fa-caret-down') { // do descending sort

                var toSort = 'desc';
                elem.find("i").removeClass('fa-caret-down').addClass('fa-caret-up');
            } else { // do ascending sort
                var toSort = 'asc';
                elem.find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
            }

            //$scope.data = UtilityService.multiSort($scope.data, superKey, key1, key2, toSort);
            $scope.data = UtilityService.assesmentSort($scope.data, toSort);
            partnerCreditFactory.applyFilter($scope);
        }

        $scope.initAuto = function () {
            $('#data_auto_complete').bind("DOMSubtreeModified", function () {
                var data = $(this).html();
                if (data.length > 0) {
                    var x = JSON.parse(data);
                    $scope.histories = [x.originalObject];
                    $timeout(function () {
                        $(".graph-row").hide();
                    }, 1000)
                }
            });
        }

        $scope.initPartnerById = function () {
            $('#partner_by_id_autocomplete').bind("DOMSubtreeModified", function () {
                var data = $(this).html();
                if (data.length > 0) {
                    var x = JSON.parse(data);
                    $scope.histories = [x.originalObject];
                    $timeout(function () {
                        $(".graph-row").hide();
                    }, 1000)
                }
            });
        }

        $scope.resetAutoComplete = function () {
            partnerCreditFactory.applyFilter($scope);
            $(".angucomplete-holder .form-control").val("");
        }

        $scope.getNames = function (id) {
            var data = $localStorage.businessUnits;
            //data.insert(0, 0, { ID: 0, Name: 'Total' });
            var dt = _.filter(data, function (o) {
                return o.ID == id
            });

            return dt[0].Name;
        }
        Array.prototype.insert = function (index, item) {
            this.splice(index, 0, item);
        };

        $scope.getTypeof = function (o) {
            if (typeof o == 'undefined') {
                return true;
            } else {
                return o.length == 0 ? true : false;
            }
        }

        $scope.convertToChartData = function (data, what) {
            if (what == 'MDF') {
                return [
                  {
                      "values": data,
                      "key": "MDF",
                      "color": "#92288D",
                      "strokeWidth": 2,
                      "classed": "dashed"
                  }
                ]
            } else {

            }
        };

        var ANIMATION_TIME = 1500,
    countSeriesDisplayed = 2,
    promise,
    labels = ["label1", "label2", "label3", "label4", "label5"];

        $scope.isStacked = false;

        // Example data
        $scope.chartData = [{
            "key": "Series 1",
            "values": [
              [0, 10],
              [1, 20],
              [2, 30],
              [3, 40],
              [4, 50]
            ]
        }, {
            "key": "Series 2",
            "values": [
              [0, 10],
              [1, 40],
              [2, 60],
              [3, 20],
              [4, 40]
            ]
        }];

        /* To add labels, images, or other nodes on the created SVG, we need to wait
         *  for the chart to be rendered with a callback.
         *  Once the chart is rendered, a timeout is set to wait for the animation to
         *  finish.
         *
         *  Then, we need to find the position of the labels and set it with the
         *  transform attribute in SVG.
         *  To do so, we have to get the width and height of each bar/group of bar 
         *  which changes if stacked or not
         *
         */

        // Callback called when the chartData is assigned
        $scope.initLabels = function () {
            return function (graph) {
                promise = $timeout(function () {
                    var svg = d3.select("svg"),
                      lastRects, rectWidth,
                      heightForXvalue = []; // Used for grouped mode

                    // We get one positive rect of each serie from the svg (here the last serie)
                    lastRects = svg.selectAll("g.nv-group").filter(
                      function (d, i) {
                          return i == countSeriesDisplayed - 1;
                      }).selectAll("rect.positive");

                    if ($scope.isStacked) {
                        // If stacked, we get the width of one rect
                        rectWidth = lastRects.filter(
                          function (d, i) {
                              return i == countSeriesDisplayed - 1;
                          }).attr("width");
                    } else {
                        // If grouped, we need to get the greatest height of each bar
                        var nvGroups = svg.selectAll("g.nv-group").selectAll("rect.positive");
                        nvGroups.each(
                          function (d, i) {
                              // Get the Min height space for each group (Max height for each group)
                              var rectHeight = parseFloat(d3.select(this).attr("y"));
                              if (angular.isUndefined(heightForXvalue[i])) {
                                  heightForXvalue[i] = rectHeight;
                              } else {
                                  if (rectHeight < heightForXvalue[i]) {
                                      heightForXvalue[i] = rectHeight;
                                  }
                              }
                          }
                        );

                        // We get the width of one rect multiplied by the number of series displayed
                        rectWidth = lastRects.filter(
                          function (d, i) {
                              return i == countSeriesDisplayed - 1;
                          }).attr("width") * countSeriesDisplayed;
                    }

                    // We choose a width equals to 70% of the group width
                    var labelWidth = rectWidth * 70 / 100;

                    var groupLabels = svg.select("g.nv-barsWrap").append("g");

                    lastRects.each(
                      function (d, index) {
                          var transformAttr = d3.select(this).attr("transform");
                          var yPos = parseFloat(d3.select(this).attr("y"));
                          groupLabels.append("text")
                            .attr("x", (rectWidth / 2) - (labelWidth / 2)) // We center the label
                            // We add a padding of 5 above the highest rect
                            .attr("y", (angular.isUndefined(heightForXvalue[index]) ? yPos : heightForXvalue[index]) - 5)
                            // We set the text
                            .text(labels[index])
                            .attr("transform", transformAttr)
                            .attr("class", "bar-chart-label");
                      });

                }, ANIMATION_TIME);
            }
        };

        // Tooltips
        $scope.toolTipContentFunction = function () {
            return function (key, x, y, e, graph) {
                return labels[x];
            }
        };








        $scope.hideIfFullScreen = function () {
            if ($state.current.name == 'budget.partner-budget-expand') {
                partnerCreditFactory.pageSize = 50;
                $scope.expand = false;
            }
        }

        $scope.expanded = false;
        $scope.switchView = function () {
            ifFullScreen();
            $scope.expanded = !$scope.expanded;
            loadFilter($scope.expanded, false);
            $timeout(function () {
                $(".summary-expand").hide();
                $(".graph-row").hide();
                if ($scope.buId > 0) {
                    $timeout(function () {
                        if ($scope.role.disabled || $scope.disablePrevious || $scope.NOACCESS) {
                            $(".businessUnit_" + $scope.buId + " input").attr("disabled", "disabled");
                            $(".businessUnit_" + $scope.buId + " select").attr("disabled", "disabled");
                            $(".businessUnit_" + $scope.buId + " img").off('click');
                        }
                    }, 500);

                }

            }, 2000)

        }



        function ifFullScreen() {
            // first check which view
            var fullview = !isNaN(buParam);

        }


        function graphProp() {



        }

        $scope.openCommentBox = function (msg, $event) {


            var el = event.target;
            //$(el).parent().find(".dropdown").addClass("open");
            //console.log(el);
        }

        $scope.clickDD = function (bu, $index, e, busave) {

            bootbox.prompt({
                title: "Add Comment",
                inputType: 'textarea',
                value: bu.MDF[0].Comments,
                callback: function (result) {
                    if (result != null) {
                        bu.MDF[0].Comments = result;
                        $scope.saveFromBU($index, e, bu, false, busave)
                    }

                }
            });

            /*var el = event.target;
            $timeout(function(){
                $(el).parent().next().find("span").trigger("click")
            },10) */
        }

        $scope.clickBU = function (parent, index, e, bu, t, busave) {

            bootbox.prompt({
                title: "Add Comment",
                inputType: 'textarea',
                value: bu.MDF[0].Comment,
                callback: function (result) {
                    if (result != null) {
                        bu.MDF[0].Comment = result;
                        $scope.save(parent, index, e, bu, false, busave)
                    }

                }
            });
        }
        //function clickBU(parent, index, bu, t) {

        //    bootbox.prompt({
        //        title: "Add Comment",
        //        inputType: 'textarea',
        //        value: bu.MDF[0].Comment,
        //        callback: function (result) {
        //            if (result != null) {
        //                bu.MDF[0].Comment = result;
        //                $scope.save(parent, index, bu, false)
        //            }

        //        }
        //    });
        //}

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
                VersionID: $scope.versiondata.VersionID,// version id
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
                // partnerCreditFactory.generateTotalRow(filter,$scope, true);
                //console.log(resp);
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

        /*$scope.getLastQuater = function () {
            var date = new Date();
            //var year = moment(date).format("YY");
            var year = parseInt($localStorage.selectedversion.FinancialPeriod.substr(2, 2));
            var month = moment(date).format("MMM");
            var lastQuater = "";
            var Q1 = ["Nov", "Dec", "Jan"];
            var Q2 = ["Feb", "Mar", "Apr"];
            var Q3 = ["May", "Jun", "Jul"];
            var Q4 = ["Aug", "Sep", "Oct"];
            if (Q1.indexOf(month) > -1) {
                lastQuater = "FY" + (year-1) + " Q4";
            } else if (Q2.indexOf(month) > -1) {
                lastQuater = "FY" + year + " Q1";
            } else if (Q3.indexOf(month) > -1) {
                lastQuater = "FY" + year + " Q2";
            } else if (Q4.indexOf(month) > -1) {
                lastQuater = "FY" + year + " Q3";
            }
            return lastQuater;

        }*/

        $scope.getLastQuater = function () {

            if ($localStorage.selectedversion.Financialyear.slice(5, 7) === '1H') {
                return "4Q" + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4)) - 1).toString();
            } else {
                return "2Q" + (parseInt($localStorage.selectedversion.Financialyear.slice(2, 4))).toString();
            }
            //var year = parseInt($localStorage.selectedversion.FinancialPeriod.substr(2, 2))-1;
            //var half = $localStorage.selectedversion.FinancialPeriod.split(" ")[1];
            //if (half == "1H" || half == '1h' || half == 'h1' || half == 'H1') {
            //    return "FY" + (year - 1) + " Q4";
            //} else if (half == "2H" || half == '2h' || half == 'h2' || half == 'H2') {
            //    return "FY" + year  + " Q2";
            //}
        }

        var formdata = new FormData();
        $scope.setFile = function (element) {
            $scope.orderByField = 'id';
            $scope.reverseSort = null;
            for (var i in element.files) {
                formdata.append(element.files[i].name, element.files[i])
            }
            //angular.forEach(element.files, function (value, key) {
            //    formdata.append(key, value);

            //});
            if (element.files.length > 1) {
                document.getElementById("uploadFilep").value = element.files.length + " Files attached.";
            }
            else {
                document.getElementById("uploadFilep").value = element.files[0].name;
            }


        }
        $scope.onUpload = function myfunction() {
            $scope.invalidRows = [];
            $scope.validData = [];
            $scope.importTotal = 0;
            $scope.importSuccess = 0;
            var fi = document.getElementById('uploadBtnp');
            if (fi.files.length <= 0) {
                Notification.error({
                    message: "Please select File" + $rootScope.closeNotify,
                    delay: null
                });
            }
            else {
                $('#mdf_loader').css('display', 'block');
                $scope.invalidRows = [];
                $scope.validData = [];
                var request = {
                    method: 'POST',
                    url: $rootScope.api + 'Export/importMDF/?versionId=' + $sessionStorage.currentVersion.VersionID + '&bulist=' + $localStorage.user.BUs,
                    headers: {
                        'Content-Type': undefined
                    },
                    data: formdata
                }
                // SEND THE FILES.
                $http(request)
                    .success(function (d) {

                        document.getElementById("uploadFilep").value = null;
                        document.getElementById('uploadBtnp').value = null;
                        // getdata();
                        $('#mdf_loader').css('display', 'none');
                        $scope.importTotal = d["total"];
                        $scope.importSuccess = d["success"];
                        if (d["invalidExcel"] != undefined) {
                            Notification.error({
                                message: d["invalidExcel"] + $rootScope.closeNotify,
                                delay: null
                            });
                        }
                        else {
                            if (d["invalid"] != undefined) {
                                $scope.invalidRows = d["invalid"];
                            }
                            if (d["valid"] != undefined) {
                                $scope.validData = d["valid"]
                            }
                            Notification.success({
                                message: "File uploaded successfully" + $rootScope.closeNotify,
                                delay: null
                            });
                        }
                        formdata = new FormData();
                    })
                    .error(function (d) {
                        document.getElementById("uploadFilep").value = null;
                        document.getElementById('uploadBtnp').value = null;
                        formdata = new FormData();
                        $('#mdf_loader').css('display', 'none');
                        Notification.error({
                            message: d + "Error" + $rootScope.closeNotify,
                            delay: null
                        });
                    });

            }
        }
        $scope.import = function () {
            $scope.invalidRows = [];
            $scope.validData = [];
            $scope.importTotal = 0;
            $scope.importSuccess = 0;
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
        $scope.SaveImportData = function () {
            var request = {
                method: 'POST',
                url: $rootScope.api + 'Export/SaveImportData?versionId=' + $sessionStorage.currentVersion.VersionID + '&UserID=' + $localStorage.user.UserID,
                data: $scope.validData

            }
            $http(request)
                 .success(function (d) {
                     if (d.count == 0) {
                         Notification.error({
                             message: d.message + $rootScope.closeNotify,
                             delay: null
                         });
                     }
                     else {
                         Notification.success({
                             message: "Data updated successfully for " + d.count + " Partners" + $rootScope.closeNotify,
                             delay: null
                         });
                         setTimeout(function () { window.location.reload(); }, 2000);

                     }

                 });


        }
        var inputControls = [];
        $(".specialText").priceFormat();
        $scope.AssignPosition = function () {
            //HSM-413 start - stop cursor reposition to end after price formatting.
            var focusedId = "";
            focusedId = $(document.activeElement)[0].id;
            if (focusedId == undefined) {
                focusedId = "";
            }
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


        var productivityImprovement = 0;
        $scope.ProductivityImprovement = function (PartnerID) {
            var indexes = $.map($scope.histories, function (obj, index) {
                if (obj.PartnerID == PartnerID) {
                    return index;
                }
            })
            if ($scope.histories[indexes[0]].Sellout[0].Last_Period_Sellout === 0 || $scope.histories[indexes[0]].Sellout[0].Projected_Sellout === 0)
                productivityImprovement = 0;
            else if (($scope.histories[indexes[0]].MDF[0].Last_year_mdf / $scope.histories[indexes[0]].Sellout[0].Last_Period_Sellout) === 0 || ($scope.histories[indexes[0]].MDF[0].BaseLineMDF / $scope.histories[indexes[0]].Sellout[0].Projected_Sellout) === 0)
                productivityImprovement = 0;
            else if (($scope.histories[indexes[0]].MDF[0].Last_year_mdf / $scope.histories[indexes[0]].Sellout[0].Last_Period_Sellout) === 0 && ($scope.histories[indexes[0]].MDF[0].BaseLineMDF / $scope.histories[indexes[0]].Sellout[0].Projected_Sellout) > 0)
                productivityImprovement = 100;
            else
                productivityImprovement = (1 - ($scope.histories[indexes[0]].MDF[0].BaseLineMDF / $scope.histories[indexes[0]].Sellout[0].Projected_Sellout) / ($scope.histories[indexes[0]].MDF[0].Last_year_mdf / $scope.histories[indexes[0]].Sellout[0].Last_Period_Sellout)) * 100;

            return productivityImprovement.toFixed(2);
        }
    }

})();

