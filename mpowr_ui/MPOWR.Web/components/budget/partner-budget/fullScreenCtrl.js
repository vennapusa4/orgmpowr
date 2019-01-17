/*
Partner Credit controller: Partner credit controller logics
Created by: Aamin Khan
Created at: 19/01/2017
*/
(function () {
    'use strict';
    angular.module("hpe").controller("fullScreenCtrl", fullScreenCtrlFn);

    function fullScreenCtrlFn(
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
        roleMappingService
        ) {
        //ngInject


        var colorFunction1 = function () {
            return function (d, i) {
                return 'rgb(146, 40, 141)'
            };
        }
        $scope.role = roleMappingService.apply();
        partnerCreditFactory.pageSize = 10;
        var CountryID = $sessionStorage.resellerPartner.obj.id;
        var PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
        var FinancialYear = $sessionStorage.resellerPartner.FinencialYear;
        var DistrictID = $sessionStorage.resellerPartner.district.DistrictID;

        $scope.PartnerTypeID = PartnerTypeID;
        $scope.reseller = $sessionStorage.resellerPartner
        $scope.Math = window.Math;
        $scope.noGraph = false;
        $scope.noTotalGraph = false;
        $scope.previousYear = $localStorage.previousYear.replace("_", "");

        partnerCreditFactory.getBu($scope); //$scope.bus
        $scope.currentBu = 0;

        $scope.showSpartlines = false;
        $scope.selectedPartner = [null, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true];


        $scope.$watch('showSpartlines', function (oldVal, newVal) {

            if (oldVal) {
                $timeout(function () {
                    $(".business-unit, .projected-sellout").css("height", "220px");
                }, 10)
            } else {
                $timeout(function () {
                    /*$(".business-unit,.projected-sellout").css("height", "46px")*/
                }, 10)

            }
        })

        $scope.expandedView = false;
        $scope.selectView = function (view) {
            if (view == "expanded")
                $scope.expandedView = true;
            else
                $scope.expandedView = false;
        }

        $scope.expand = false;
        $scope.hideShowGraph = function () {
            var elem = $(".partner-credit .data")
            $scope.expand = !$scope.expand;
            if ($scope.expand) {
                elem.css("margin-top", "250px")
            } else {
                elem.css("margin-top", "25px")
            }

        }


        $scope.gotoPage = function (pageNumber) {
            partnerCreditFactory.gotoPage(pageNumber, $scope)
        }

        $scope.next = function () {
            partnerCreditFactory.gotoPage($scope.currentPage + 1, $scope)
        }
        $scope.prev = function () {
            partnerCreditFactory.gotoPage($scope.currentPage - 1, $scope)
        }

        //export to excel
        $scope.exportSFDC = function () {
            partnerBudgetDataService.ExportSDFC({
                "CountryID": CountryID,
                "FinancialyearID": $localStorage.user.FinancialYearID,
                "DistrictID": DistrictID,
                "PartnerTypeID": PartnerTypeID
            })
        }


        $scope.export = function () {
            partnerBudgetDataService.Export({
                "CountryID": CountryID,
                "FinancialyearID": $localStorage.user.FinancialYearID,
                "DistrictID": DistrictID,
                "PartnerTypeID": PartnerTypeID
            })
        }





        $scope.checkWidth = function (sellout, mdf, mdfBySellout) {
            partnerCreditFactory.checkWidth($scope, sellout, mdf, mdfBySellout);
        }
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

        //$scope.data = [{"Id":1,"Partner_Name":"Computacter Name","Membership_Type":"Platinum","MembershipTypeId":2,"Status":"Up","BU":[{"Business_Unit":"DCN Compute","BusinessUnitID":21,"Sellout":{"Last_Period_Sellout":230568,"Projected_Sellout":14280151,"YoY_change_sellout":5},"MDF":{"Last_Period_MDF":5458145,"Recommended_MDF":125475,"YoY_change_MDF":12,"BaseLineMDF":74154,"ReasonForVariance":"Reason","Comment":"this is my comment"},"MDFOrSellout":{"LastYearMDFOrSellout":454145,"ProjectedMDFOrSellout":4215,"MedianAvdMDFOrSellout":541,"ProductivityImprovementPer":1},"Analysis":{"MDFAllignment":542,"AssesmentOfLastYearMDF":541,"PredictionAccuracy":54},"MSA":{"RecommendedMSA":454,"MSA":41},"PartnerID":21,"PBM":1255,"PMM":215,"RankingAmongPeers":125,"SellIn":124,"SellInGrowth":125,"DeltaBetweenSellInAndSellOut":12,"PreviousPeriodSellOutGrowth":54,"PlannedSaled":122,"targetAchievement":null,"SOW":null,"SOWGrowth":null,"FootprintGrowth":null,"No_of_end_customers":null,"Total_MDF":null,"Incremental_MDF":null,"Late_MDF":null,"W_MGO_Marketing_MDF":null,"New_Logos_MGO":null}]},{"Id":1,"Partner_Name":"Computacter Name","Membership_Type":"Platinum","MembershipTypeId":2,"Status":"Up","BU":[{"Business_Unit":"DCN Compute","BusinessUnitID":21,"Sellout":{"Last_Period_Sellout":230568,"Projected_Sellout":14280151,"YoY_change_sellout":5},"MDF":{"Last_Period_MDF":5458145,"Recommended_MDF":125475,"YoY_change_MDF":12,"BaseLineMDF":74154,"ReasonForVariance":"Reason","Comment":"this is my comment"},"MDFOrSellout":{"LastYearMDFOrSellout":454145,"ProjectedMDFOrSellout":4215,"MedianAvdMDFOrSellout":541,"ProductivityImprovementPer":1},"Analysis":{"MDFAllignment":542,"AssesmentOfLastYearMDF":541,"PredictionAccuracy":54},"MSA":{"RecommendedMSA":454,"MSA":41},"PartnerID":21,"PBM":1255,"PMM":215,"RankingAmongPeers":125,"SellIn":124,"SellInGrowth":125,"DeltaBetweenSellInAndSellOut":12,"PreviousPeriodSellOutGrowth":54,"PlannedSaled":122,"targetAchievement":null,"SOW":null,"SOWGrowth":null,"FootprintGrowth":null,"No_of_end_customers":null,"Total_MDF":null,"Incremental_MDF":null,"Late_MDF":null,"W_MGO_Marketing_MDF":null,"New_Logos_MGO":null}]}]
        //binds data to $scope.histories
        partnerCreditFactory.getReasons($scope);
        partnerCreditFactory.getData($scope, 0);

        $scope.partnerCols = [
            { id: 1, name: "Partner ID" },
            { id: 2, name: "PBM" },
            { id: 3, name: "PMM" },
            { id: 4, name: "Ranking among peers" },
            { id: 5, name: "delta between sell-in and sell-out" },
            { id: 6, name: "Previous Period Sell Out Growth" },
            { id: 9, name: "SOW" },
            { id: 10, name: "SOW growth" },
            { id: 11, name: "Footprint growth" },
            { id: 12, name: "Total MDF" },
            { id: 13, name: "Incremental MDF" },
            { id: 14, name: "Late MDF" },
            { id: 15, name: "(W)MGO/marketing MDF" },
            { id: 16, name: "New logos/MGO" },
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


        

        $scope.msExpnd = false; $scope.selloutColExpnd = false; $scope.mdfColExpnd = false;
        $scope.selectExtraCols = function () {
            partnerCreditFactory.checkWidth($scope, $scope.selloutColExpnd, $scope.mdfColExpnd, $scope.msExpnd);
        }


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

        $scope.toogle = function (index, partnerType, partner, isBu) {

            //if (isBu == 'true') { // if for business Unit
                $scope.currentPartner = $scope.histories[index];
                $(".graph-row").hide();
                var elem = $("#icon-" + index);
                var curObj = this;
                var div = $("#" + partnerType + "-graph-row-" + index);
                if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                    div.show();
                    elem.removeClass('fa-caret-down').addClass('fa-caret-up');

                } else {
                    elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                    div.hide();
                }
            /*} else {
                partnerCreditFactory.getPartnerBu(partner.MSA[0].PartnerBudgetID, $scope, index, partnerType);
            }*/
        }

        $scope.toogleInBU = function (index, partnerType, partner, isBu) {
            $scope.noGraph = false;
            $scope.bx = [];

            $scope.currentPartner = $scope.histories[index];
            $(".graph-row").hide();
            var elem = $("#icon-" + index);
            var curObj = this;
            var div = $("#" + partnerType + "-graph-row-" + index);
            if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                div.show();
                elem.removeClass('fa-caret-down').addClass('fa-caret-up');
                $http({
                    method: 'GET',
                    url: $rootScope.api + "PartnerBudget/GetPartnerBudgetBUDetails?PartnerBudgetID=" + partner.MSA[0].PartnerBudgetID,
                }).then(function (resp) {

                    if (resp.data.length == 0) {
                        return
                    }


                    var a = JSON.parse(resp.data);
                    a = _.filter(a, function (o) { return o.BusinessUnitID == $scope.buId; });
                    if (a.length == 0) {
                        $scope.noGraph = true;
                    } else {
                        $scope.noGraph = false;
                        $scope.bx = partnerCreditFactory.generateChartForBU(a); // data(HistoryMDF,HistorySellout) not coming from api
                        console.log($scope.bx)
                    }


                })
            } else {
                elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                div.hide();
            }

        }

        $scope.toogleTotal = function () {
            $(".graph-row").hide();
            var elem = $('.total-expand-btn');
            var curObj = this;
            var div = $('.summary-expand');


            var objToPass = {
                CountryID: CountryID,
                Financialyear: $sessionStorage.resellerPartner.FinencialYear,
                DistrictID: DistrictID,
                PartnerTypeID: PartnerTypeID,
                PageIndex: 0,
                PageSize: 0,
                businessUnitID: $scope.buId
            }

            if (elem.attr("class").indexOf('fa-caret-down') > -1) {
                partnerCreditFactory.generateTotalRow(objToPass, $scope, false);
                div.show();
                elem.removeClass('fa-caret-down').addClass('fa-caret-up');
            } else {
                elem.removeClass('fa-caret-up').addClass('fa-caret-down')
                div.hide();
            }
        }


        $scope.expand = function (cls, e) {
            if (e) {
                $("." + cls).css("width", "10%")
            } else {
                $("." + cls).css("width", "20%")
            }

        }

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
        $scope.buId = 0;

        $scope.selectBu = function (index, buId) {
            $scope.currentBu = index;
            $scope.buId = buId;
            partnerCreditFactory.getData($scope, $scope.buId);

            $scope.expand = true;

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

        $scope.saveFromBU = function (i, history) {

            if ($scope.calculateForBuSpecific(history, i)) {
                // no action
            } else {
                var partner = $scope.histories[i];
                var object = {
                    PartnerBUBudgetID: partner.MDF[0].PartnerBUBudgetID,
                    PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                    BusinessUnitID: $scope.buId,
                    Baseline_MDF: partner.MDF[0].BaseLineMDF,
                    Comments: partner.MDF[0].Comments,
                    MDFVarianceReasonID: typeof partner.MDF[0].MDFVarianceReasonID != 'undefined' ? partner.MDF[0].MDFVarianceReasonID.MDFVarianceReasonID : null
                }

                partnerCreditFactory.saveEditedData(object, false, $scope);
            }


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



        /*Save MSA*/
        $scope.saveMSA = function (index, trggierfrom) {

            var partner = $scope.histories[index];
            if (trggierfrom="MSA") {
                if ((partner.MSA[0].MSAValue - oldMSA) > $scope.graphData.MSAGraph[0].Remains) {

                    Notification.error({ message: "Sum of MSA allocated for partners should not exceed Total MSA allocated." + $rootScope.closeNotify, delay: null });
                    partner.MSA[0].MSAValue = oldMSA;
                    return
                }
                else {
                    var object = {
                        MSA: $scope.histories[index].MSA[0].MSAValue,
                        Aruba_MSA: $scope.histories[index].MSA[0].ArubaMsa,
                        PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                    }
                    partnerCreditFactory.saveMSA(object, $scope);
                }
            }
            if (trggierfrom = "ARUBA MSA") {
                if ((partner.MSA[0].ArubaMsa - oldARUBAMSA) > $scope.graphData.ARUBAMSAGraph[0].Remains) {
                    Notification.error({ message: "Sum of ARUBA MSA allocated for partners should not exceed Total ARUBA MSA allocated." + $rootScope.closeNotify, delay: null });
                    partner.MSA[0].ArubaMsa = oldARUBAMSA;
                    return
                }
                else {
                    var object = {
                        MSA: $scope.histories[index].MSA[0].MSAValue,
                        Aruba_MSA: $scope.histories[index].MSA[0].ArubaMsa,
                        PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                    }
                    partnerCreditFactory.saveMSA(object, $scope);
                }
            }
            
           
           

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
                MDFVarianceReasonID: typeof $scope.histories[pi].BU[i].MDF[0].MDFVarianceReasonID != 'undefined' ? $scope.histories[pi].BU[i].MDF[0].MDFVarianceReasonID.MDFVarianceReasonID : null
            }
            partnerCreditFactory.sumUpBaslineMDF($scope.histories[pi].BU, $scope)
            partnerCreditFactory.saveEditedData(object);
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
                    axisLabelDistance: -10,
                    ticks: 3,
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



        var prevValue = 0;

        var totalBaslineMDF = 0;
        var copiedBaseline;
        $scope.copyTotalVal = function (baslineVal) {
            totalBaslineMDF = $scope.total.BaseLineMDF;
            copiedBaseline = baslineVal
        }

        $scope.saveOnEnter = function (pi, i, BU, keyEvent) {
            if (keyEvent.which === 13)
                saveAllocatedMDF(pi, i, BU)
        }

        $scope.save = function (pi, i, BU) {
            saveAllocatedMDF(pi, i, BU)
        }
        // save from total
        function saveAllocatedMDF(pi, i, BU) {

            if ($scope.calculate(pi, i, BU)) {

            } else {
                //$("#baseline_"+i).parent().next().find("div").find("select").focus()
                var partner = $scope.histories[pi];
                var object = {
                    PartnerBUBudgetID: BU.MDF[0].PartnerBUBudgetID,
                    PartnerBudgetID: partner.MSA[0].PartnerBudgetID,
                    BusinessUnitID: BU.BusinessUnitID,
                    Baseline_MDF: BU.MDF[0].BaseLineMDF,
                    Comments: BU.MDF[0].Comment,
                    MDFVarianceReasonID: typeof BU.MDF[0].MDFVarianceReasonID != 'undefined' ? BU.MDF[0].MDFVarianceReasonID.MDFVarianceReasonID : null
                }
                partnerCreditFactory.saveEditedData(object, false, $scope);
            }
        }

        $scope.calculate = function (pi, i, bu) {
            var ret = false

            var theBU = _.find($scope.graphData.AllocatedGraph, function (o) { return o.Business_Unit == bu.BusinessUnitID; });

            if (bu.MDF[0].BaseLineMDF > theBU.TotalBaseline) {
                Notification.warning({
                    message: "Sum of allocated MDF for partners of the Business Unit is more than the total Baseline MDF allocated for the Business Unit." + $rootScope.closeNotify,
                    delay: null
                });
                /*bu.MDF[0].BaseLineMDF = copiedBaseline;
                return true*/
            }

            var recomenededMDF = bu.MDF[0].Recommended_MDF;
            var max = (recomenededMDF * 20) / 100 + recomenededMDF;
            var min = recomenededMDF - (recomenededMDF * 20) / 100;

            if ((bu.MDF[0].BaseLineMDF > max || bu.MDF[0].BaseLineMDF < min) && typeof bu.MDF[0].MDFVarianceReasonID == 'undefined') {

                Notification.warning({
                    message: 'Difference between Calculated and Allocated MDF is more than + or - 20%.' + $rootScope.closeNotify,
                    //title: 'Warning: Baseline mdf not saved!' + $rootScope.closeNotify,
                    delay: null
                });
                //$("#reason_"+pi+"_"+i).css("border","1px solid yellow");
                //return true;

            } else {
                $(".warning h3:contains('Warning')").parent().remove();
                $("#reason_" + pi + "_" + i).css("border", "1px solid #ccc");
                ret = false;
            }


            prevValue = bu.MDF[0].BaseLineMDF;

            var partner = $scope.histories[pi];

            //Sum up to partner
            var sum = 0;
            for (var i = 0; i < $scope.businessUnit.length; i++) {
                sum += $scope.businessUnit[i].MDF[0].BaseLineMDF
            }

            partner.MDF[0].BaseLineMDF = sum;

            // sum up to Sumary
            //$scope.total.BaseLineMDF = totalBaslineMDF + sum;
            //$scope.total.BaseLineMDF = totalBaslineMDF + bu.MDF[0].BaseLineMDF
            return ret;
        }

        var mdfBuScreen = 0;
        var x = 0;
        var tBux = 0;
        $scope.copyFromBu = function (val) {
            mdfBuScreen = val;
            tBux = $scope.total.BaseLineMDF;
            x = $scope.total.BaseLineMDF;
        }
        $scope.calculateForBuSpecific = function (partner, i) {
            var ret = false;
            var recomenededMDF = partner.MDF[0].Recommended_MDF;
            var max = (recomenededMDF * 20) / 100 + recomenededMDF;
            var min = recomenededMDF - (recomenededMDF * 20) / 100;

            //$scope.total.BaseLineMDF = tBux - mdfBuScreen + partner.MDF[0].BaseLineMDF;


            var theBU = _.find($scope.graphData.AllocatedGraph, function (o) { return o.Business_Unit == $scope.buId; })
            if (partner.MDF[0].BaseLineMDF > theBU.TotalBaseline) {
                Notification.warning({
                    message: "Sum of allocated MDF for partners of the Business Unit should not exceed the total Baseline MDF allocated for the Business Unit." + $rootScope.closeNotify,
                    delay: null
                });
                /*partner.MDF[0].BaseLineMDF = mdfBuScreen;
                $scope.total.BaseLineMDF = x;
                return true;*/
            } else {

            }

            if ((partner.MDF[0].BaseLineMDF > max || partner.MDF[0].BaseLineMDF < min) && typeof partner.MDF[0].MDFVarianceReasonID == 'undefined') {

                Notification.warning({
                    message: 'Difference between Calculated and Allocated MDF is more than + or - 20%.' + $rootScope.closeNotify,
                    //title: 'Warning: Baseline mdf not saved!' + $rootScope.closeNotify,
                    delay: null
                });
                //$("#reason_"+i).css("border","1px solid yellow");
                //return true;

            } else {
                $(".warning h3:contains('Warning')").parent().remove();
                $("#reason_" + i).css("border", "1px solid #ccc");
                ret = false;
            }



            return ret;

        }


        $scope.typeCheck = function (o) {

            return typeof o == 'undefined' || o.length == 0 ? true : false;
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

            $scope.data = UtilityService.sortBy($scope.data, superKey, key, toSort);
            partnerCreditFactory.applyFilter($scope);
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

            $scope.data = UtilityService.multiSort($scope.data, superKey, key1, key2, toSort);
            partnerCreditFactory.applyFilter($scope);
        }


        $scope.initAuto = function () {
            $('#data_auto_complete').bind("DOMSubtreeModified", function () {
                var data = $(this).html();
                if (data.length > 0) {
                    var x = JSON.parse(data);
                    $scope.histories = [x.originalObject]
                }
            });
        }

        $scope.initPartnerById = function () {
            $('#partner_by_id_autocomplete').bind("DOMSubtreeModified", function () {
                var data = $(this).html();
                if (data.length > 0) {
                    var x = JSON.parse(data);
                    $scope.histories = [x.originalObject]
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



        /*Lazy loading*/
        var lazyTo = 10;
        var lazySize = 10;
        var processing = false;
        $("table").scroll(function (event) {

            /*console.log($(window).scrollTop());
            console.log(($(document).height() - $(window).height())*0.7);
            console.log($(window).scrollTop() >= ($(document).height() - $(window).height())*0.7);*/
            if (processing) {
                return false;
            } else {
                if ($("table").scrollTop() >= $(document).height() - $("table").height()) {
                    processing = true;
                    console.log($("table").height());
                    var to = lazySize + lazyTo;
                    var data = $scope.data.slice(lazyTo, to);
                    $scope.histories = $scope.histories.concat(data);
                    $scope.$apply();
                    lazyTo = to;
                    processing = false;

                    $timeout(function () {
                        $(".graph-row").hide();
                    }, 300);

                }
            }

        });

        $scope.hideIfFullScreen = function () {
            if ($state.current.name == 'budget.partner-budget-expand') {
                partnerCreditFactory.pageSize = 50;
                $scope.expand = false;
            }
        }




    }

})();