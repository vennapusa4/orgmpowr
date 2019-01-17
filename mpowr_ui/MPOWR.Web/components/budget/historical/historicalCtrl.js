/*
Historical Perfomance controller: Historicak Perfomacne
Created by: Aamin Khan
Created at: 06/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").controller("HistoricalCtrl", HistoricalFn);

    function HistoricalFn($scope, $rootScope, HistoricalFactory, Notification, $timeout, $sessionStorage, UtilityService, roleMappingService, $localStorage) {
        //ngInject
        $scope.role = roleMappingService.apply();
        $scope.$sessionstorage = $sessionStorage;
        var allBu = []
        $scope.reseller = $sessionStorage.resellerPartner
        $scope.all = true;
        var latest = [];
        $scope.mdfReceived = true; $scope.mdfNotReceived = true; $scope.allMDF = true;
        var maxMDF, maxSellout;
        console.log('fy ', $scope.reseller.FinencialYear, $localStorage.selectedversion);
        $scope.selectedFinancilaPeriod = $localStorage.selectedversion.FinancialPeriod.replace(' ', '_');

        if ($scope.selectedFinancilaPeriod.slice(5, 7) === '1H') {
            $scope.prevYear = 'FY' + (parseInt($scope.selectedFinancilaPeriod.slice(2, 4)) - 1).toString() + ' 1H';
            $scope.prevToPrevYear = 'FY' + (parseInt($scope.selectedFinancilaPeriod.slice(2, 4)) - 2).toString() + ' 1H';
        } else {
            $scope.prevYear = 'FY' + (parseInt($scope.selectedFinancilaPeriod.slice(2, 4)) - 1).toString() + ' 2H';
            $scope.prevToPrevYear = 'FY' + (parseInt($scope.selectedFinancilaPeriod.slice(2, 4)) - 2).toString() + ' 2H';
        }


        // Get the Bu List
        HistoricalFactory.getBuByVersion().then(
            function onSuccess(resp) {
                $scope.bu = resp.data;
                $scope.selectedBu = HistoricalFactory.generateBuModel($scope.bu);
                allBu = jQuery.extend(true, {}, $scope.selectedBu);
                //convert to comma seperate
                $scope.filter();
            },
            function onError(resp) {
                Notification.error({
                    message: resp.data,
                    delay: null
                });
            }
        );


        $scope.filter = function () {
            var arr = [];
            angular.forEach($scope.selectedBu, function (value, key) {
                if (value) {
                    this.push(key);
                }

            }, arr);
            arr = arr.join(",");
            var countries;
            angular.forEach($scope.reseller.obj.countries, function (rec) {
                if (countries == undefined)
                    countries = rec.CountryID;
                else
                    countries = ',' + rec.CountryID;
            });
            getData({
                CountryID: countries,
                PartnerTypeID: $scope.reseller.partner.PartnerTypeID,
                DistrictID: $scope.reseller.district.DistrictID,
                MDF: $scope.mdfReceived ? 1 : 0,
                NotMDF: $scope.mdfNotReceived ? 1 : 0,
                VersionID: $sessionStorage.currentVersion.VersionID,
                MembershipGroupID : $scope.reseller.membership.MembershipGroupID,
                BUs: arr,
                PlanningId: $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion.FinancialYearID : $localStorage.selectedversion.FinancialYearID
            });

        }

        // hit the api to get history Data
        // params: {countryid:57,partnertypeid:1,district:0,MDF:1,NotMDF:0,BUs:"1,2"}
        function getData(params) {
            HistoricalFactory.getData(params).then(
                function onSuccess(resp) {
                    $scope.histories = resp.data;
                    $scope.histories = HistoricalFactory.formatData($scope.histories);

                    $timeout(function () {
                        $(".graph-row").hide();
                        //$(".history-text-t").priceFormat();
                    }, 1000);
                },
                function onError(resp) {
                    //Notification.error({
                    //    message: response.data,
                    //    delay: null
                    //});
                }
            )
        }

        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                $scope.ctries = _.pluck(countryList, 'DisplayName').join(", ");
            }
        }

        $scope.toogle = function (index, partnerType) {
            $(".graph-row").hide();
            var elem = $("#icn-"+partnerType + "-" + index);
            var div = $("#" + partnerType + "-graph-row-" + index);

            

            if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                $(".expand-btn").removeClass('fa-caret-up').addClass('fa-caret-down');
                elem.removeClass('fa-caret-down').addClass('fa-caret-up');
                div.show();
            } else {
                $(".expand-btn").removeClass('fa-caret-up').addClass('fa-caret-down');
                elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                div.hide();
            }

        }




        $scope.buSelected = function () {
            if (!angular.equals($scope.selectedBu, allBu)) {
                latest = angular.copy($scope.selectedBu);
                $scope.all = false;
                $scope.selectedBu = latest
            } else {
                $scope.all = true;
                $scope.selectedBu = latest;
            }
        }

        $scope.checkedAll = function () {
            if ($scope.all) {
                var ox = {};
                angular.forEach(allBu, function (value, key) {
                    ox[key] = true
                });
                $scope.selectedBu = ox;
            } else {
                var tx = {};
                angular.forEach(allBu, function (value, key) {
                    tx[key] = false
                });
                $scope.selectedBu = tx;
            }
        }



        $scope.updateMDF = function () {
            if ($scope.mdfReceived && $scope.mdfNotReceived) {
                $scope.allMDF = true;
            } else {
                $scope.allMDF = false;
            }

        }


        $scope.updateAllMDF = function () {
            if ($scope.allMDF) {
                $scope.mdfReceived = true; $scope.mdfNotReceived = true;
            } else {
                $scope.mdfReceived = false; $scope.mdfNotReceived = false;
            }
        }


        $scope.initTicks = function (obj1, obj2) {
            $scope.mdfData = $scope.sortByDate(obj1);
            
            maxMDF = parseFloat((_.max($scope.mdfData[0].values, function(o){return parseFloat(o.y);})).y);
            $scope.selloutData = $scope.sortByDate(obj2);

            maxSellout = parseFloat((_.max($scope.selloutData[0].values, function(o){return parseFloat(o.y);})).y);

            if ($scope.mdfData[0] || $scope.selloutData[0]) {
                $scope.optionsMDF = HistoricalFactory.getOptionMDF(maxMDF);
                $scope.optionsMDF1 = HistoricalFactory.getOptionMDF1(maxMDF);
                $scope.optionsSellout = HistoricalFactory.getOptionSellout(maxSellout);
                $scope.optionsSellout1 = HistoricalFactory.getOptionSellout1(maxSellout);
            }
        }
        //hide the graph
        $scope.hideGraph = function () {
            $timeout(function () {
                $(".graph-row").hide();
                $(".nvd3-svg").css("width", "117%");
            }, 300);
        }


        $scope.getReseler = function (reseller) {
            return ((reseller.split("-")[0]).trim()).toLowerCase();
        }

        $scope.sortByDate = function (obj) {
            
            var arr = obj[0].values;
            var newArr = _.sortBy(arr, function (o) { return o.x; });
            obj[0].values = [];
            obj[0].values = newArr;
            return obj;
        }

        $scope.pipeMDF = function(obj){

            var arr = obj[0].values;
            var newArr = _.sortBy(arr, function (o) { return o.x; });
            var half = $localStorage.user.FinancialYear.split(" ")[1];
            if(angular.uppercase(half) == "1H"){
                newArr.shift();
                newArr.pop();
            }
            return [
                {
                    values: newArr,
                    key: 'MDF'
                }
            ]
            
        }

        $scope.pipeSellout = function(obj){

            var arr = obj[0].values;
            var newArr = _.sortBy(arr, function (o) { return o.x; });
            var half = $localStorage.user.FinancialYear.split(" ")[1]
            if(angular.uppercase(half) == "1H"){
                newArr.shift();
                newArr.pop();
            }

            return [
                {
                    values: newArr,
                    key: 'Sellout'
                }
            ]
            
        }



        $scope.openModal = function (name, option, data) {
            $scope.chartHead = name;
            if (name === 'MDF') {
                $scope.option = HistoricalFactory.getOptionMDF1(maxMDF);
                $scope.option.chart.margin.right = 20;
            } else {
                $scope.option1 = HistoricalFactory.getOptionSellout1(maxSellout);
                $scope.option1.chart.margin.right = 20;
            }
            $scope.data = data;
            
        }

        

        $scope.export = function (prevYear, prevToPrevYear, isReseller) {

            if (isReseller) {
                var arToExport = [];
                angular.forEach($scope.histories, function (object) {
                    var obj = {

                    }
                    obj['BG Membership'] = object.Name !== null ? object.Name : 0,
                    obj[prevYear + ' Sellout ($)'] = object.LastPeriodSellout,
                    obj['YoY Sellout Growth ($)'] = object.LastPeriodSelloutGrowth !== null ? Math.round(object.LastPeriodSelloutGrowth) : 0,
                    obj[' YoY Sellout Growth (%)'] = object.LastPeriodSelloutGrowthPer !== null ? Math.round(object.LastPeriodSelloutGrowthPer) : 0,
                    obj[prevYear + ' MDF ($)'] = object.LastPeriodMDF !== null ? Math.round(object.LastPeriodMDF) : 0,
                    obj[prevYear + ' MDF / ' + prevYear + ' Sellout (%)'] = object.OverallROI !== null ? object.OverallROI.toFixed(1) : 0
                    arToExport.push(obj);
                })
            }
            else {
                var arToExport = [];
                angular.forEach($scope.histories, function (object1) {
                    var obj = {

                    }
                    obj['Partner Name'] = object1.Name !== null ? object1.Name : 0,
                    obj[prevYear + ' Sellout (Silver and Below)'] = object1.SellOutForSilverAndBelow !== null ? Math.round(object1.SellOutForSilverAndBelow) : 0,
                    obj[prevYear + ' Sellout (Platinum and Gold)'] = object1.SellOutForPlatinumAndGold !== null ? Math.round(object1.SellOutForPlatinumAndGold) : 0,
                    obj[prevYear + ' Sellout ($)'] = object1.LastPeriodSellout !== null ? Math.round(object1.LastPeriodSellout) : 0,
                    obj['YoY Sellout Growth ($)'] = object1.LastPeriodSelloutGrowth !== null ? Math.round(object1.LastPeriodSelloutGrowth) : 0,
                    obj['YoY Sellout Growth (%)'] = object1.LastPeriodSelloutGrowthPer !== null ? Math.round(object1.LastPeriodSelloutGrowthPer) : 0,
                    obj[prevYear + ' MDF ($)'] = object1.LastPeriodMDF !== null ? Math.round(object1.LastPeriodMDF) : 0,
                    obj[prevYear + ' MDF / ' + prevYear + ' Sellout (%)'] = object1.OverallROI !== null ? object1.OverallROI.toFixed(1) : 0
                    arToExport.push(obj);
                })
            }


            UtilityService.JSONToCSVConvertor(arToExport, 'Historical', true);
        }

        $scope.setNext = function () {
            $rootScope.next = true;
        }
        $scope.setPrev = function () {
            $rootScope.prev = true;
        }
    }

})();