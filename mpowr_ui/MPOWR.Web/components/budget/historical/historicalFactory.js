/*
Historical Factory
Created by: Aamin Khan
Created at: 06/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").factory("HistoricalFactory", HistoricalFactoryFn);

    function HistoricalFactoryFn($rootScope, $http, $localStorage) {
        //ngInject

        var colorFunction = function () {
            return function (d, i) {
                return 'rgb(0, 177, 137)'
            };
        }

        var colorFunction1 = function () {
            return function (d, i) {
                return 'rgb(146, 40, 141)'
            };
        }




        return {
            getBu: function () {

                var promise =
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'HistoricPerformance/GetBusinessUnits'
                }).then(function (resp) {
                    delete $localStorage.businessUnits;
                    var data = angular.copy(resp.data);
                    data.splice(0,0,{ ID: 0, Name: 'Total' });
                    $localStorage.businessUnits = data;
                    return resp;
                });
                return promise;

            },
            getBuByVersion: function () {
                var versionId;
                if ($localStorage.selectedversion == undefined || $localStorage.selectedversion.VersionID == undefined)
                    versionId = 0;
                else
                    versionId = $localStorage.selectedversion.VersionID;
                var promise =
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'HistoricPerformance/GetBusinessUnitByVersion?VersionID=' + versionId
                }).then(function (resp) {
                    delete $localStorage.businessUnits;
                    var data = angular.copy(resp.data);
                    data.splice(0, 0, { ID: 0, Name: 'Total' });
                    $localStorage.businessUnits = data;
                    return resp;
                });
                return promise;

            },

            // GET MDF Chart Option
            getOptionMDF: function (maxMDF) {
                
                if(maxMDF == 0)
                    maxMDF = 10

                return {
                    chart: {
                        type: 'multiBarChart',
                        height: 250,
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
                            console.log(d);
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
                            axisLabelDistance: -20,
                            ticks: 3,
                            tickFormat: function (d) {
                                return '$' + d3.format(',.1f')(d) + 'K';
                            }
                        },
                        yDomain: [0, maxMDF]
                    }
                }
            },
            
            getOptionMDF1: function (maxMDF) {
                if(maxMDF == 0)
                    maxMDF = 10
                return {
                    chart: {
                        type: 'multiBarChart',
                        width: 1000,
                        height: 250,
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
                            console.log(d);
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
                            axisLabelDistance: -20,
                            ticks: 3,
                            tickFormat: function (d) {
                                return '$' + d3.format(',.1f')(d) + 'K';
                            }
                        },
                        yDomain: [0, maxMDF]
                    }
                }
            },
            // For Dialogue charts;
            // GET Chart Option
            
            getOptionSellout: function (maxSellout) {
                if(maxSellout == 0)
                    maxSellout = 10
                return {
                    chart: {
                        type: 'multiBarChart',
                        height: 250,
                        width: 470,
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
                            axisLabelDistance: -20,
                            ticks: 3,
                            tickFormat: function (d, i) {
                                    return '$' + d3.format(',.1f')(d) + 'M';
                                
                            }
                        },
                        yDomain: [0, maxSellout]
                    }
                }
            },

            getOptionSellout1: function (maxSellout) {
                if(maxSellout == 0)
                    maxSellout = 10
                return {
                    chart: {
                        type: 'multiBarChart',
                        width: 1000,
                        height: 250,
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
                            axisLabelDistance: -20,
                            ticks: 3,
                            tickFormat: function (d) {
                                return '$' + d3.format(',.1f')(d) + 'M';
                            }
                        },
                        yDomain: [0, maxSellout]
                    }
                }
            },
            

            // change the model to checkbox model
            generateBuModel: function (arr) {
                var nArr = {};
                for (var i = 0; i < arr.length; i++) {
                    var ob = {};
                    nArr[arr[i].ID] = true
                }
                return nArr;
            },

            // depricated, can be removed
            reverseBu: function (arr) {
                var log = {};
                angular.forEach(arr, function (value, key) {
                    log[key] = false;
                })
                return log;
            },

            getData: function (params) {

                var curObj = this;
                var promise =
                $http({
                    method: 'POST',
                    url: $rootScope.api + 'HistoricPerformance/HistoricPerformanceData',
                    data: params
                }).then(function (resp) {
                    resp.data = curObj.getChartData(resp.data);
                    return resp;
                });
                return promise;
            },

            summariseData: function (arr) {

                var parts = [];
                var total = {};
                var LastPeriodSellout_SB = 0;
                var LastPeriodSellout_PG = 0;
                var LastPeriodSellout = 0;
                var LastPeriodSelloutGrowth = 0;
                var LastPeriodSelloutGrowthPer = 0;
                var LastPeriodMDF = 0;
                var OverallROI = 0;
                var ShareOfWalletGrowth = 0;
                var FootprintGrowth = 0;

                for (var i = 0; i < arr.length; i++) {
                    for (var j = 0; j < arr[i].rows.length; j++) {
                            arr[i].rows[j]['Group'] = arr[i].Name;
                            parts.push(arr[i].rows[j])
                    }

                    LastPeriodSellout_SB = LastPeriodSellout_SB + this.checkNull(arr[i].LastPeriodSellout_SB);
                    LastPeriodSellout_PG = LastPeriodSellout_PG + this.checkNull(arr[i].LastPeriodSellout_PG);
                    LastPeriodSellout = LastPeriodSellout + this.checkNull(arr[i].LastPeriodSellout);
                    LastPeriodSelloutGrowth = LastPeriodSelloutGrowth + this.checkNull(arr[i].LastPeriodSelloutGrowth);
                    LastPeriodSelloutGrowthPer = LastPeriodSelloutGrowthPer + this.checkNull(arr[i].LastPeriodSelloutGrowthPer);
                    LastPeriodMDF = LastPeriodMDF + this.checkNull(arr[i].LastPeriodMDF);
                    OverallROI = OverallROI + this.checkNull(arr[i].OverallROI);
                    ShareOfWalletGrowth = ShareOfWalletGrowth + this.checkNull(arr[i].ShareOfWalletGrowth);
                    FootprintGrowth = FootprintGrowth + this.checkNull(arr[i].FootprintGrowth);
                }

                total['Name'] = "Total";
                total['LastPeriodSellout_SB'] = Math.round(LastPeriodSellout_SB);
                total['LastPeriodSellout_PG'] = Math.round(LastPeriodSellout_PG);
                total['LastPeriodSellout'] = Math.round(LastPeriodSellout);
                total['LastPeriodSelloutGrowth'] = Math.round(LastPeriodSelloutGrowth);
                total['LastPeriodSelloutGrowthPer'] = Math.round(LastPeriodSelloutGrowthPer);
                total['LastPeriodMDF'] = Math.round(LastPeriodMDF);
                total['OverallROI'] = Math.round(OverallROI);
                total['ShareOfWalletGrowth'] = Math.round(ShareOfWalletGrowth);
                total['FootprintGrowth'] = Math.round(FootprintGrowth);

                var groupedData = _.groupBy(parts, function (d) { return d.FinancialYear })

                var arx = [];
                angular.forEach(groupedData, function (val, key) {
                    var tempObj = { SellOut: 0, MDF: 0 }
                    tempObj['FinancialYear'] = key;
                    var sumSellout = 0;
                    var sumMDF = 0;
                    for (var i = 0; i < val.length; i++) {
                        sumSellout += val[i].SellOut;
                        sumMDF += val[i].MDF;
                    }
                    tempObj.SellOut = parseFloat(sumSellout);
                    tempObj.MDF = parseFloat(sumMDF);
                    arx.push(tempObj)
                })


                total['rows'] = arx;
                var ax = [];
                ax.push(total)
                var tx = this.getChartData(ax);
                return tx[0];
            },

            //generate chart data
            getChartData: function (obj) {
                var curObj = this;

                //get summarised

                for (var i = 0 ; i < obj.length; i++) { // Unit loop
                    var selloutX = [];
                    var mdfX = [];

                    for (var j = 0; j < obj[i].rows.length; j++) { //rows loop
                        var tran = obj[i].rows[j]
                        var FY = tran.FinancialYear.split("_")[0];

                        var q1 = FY + "_H1";
                        selloutX.push({ x: q1, y: (curObj.checkNull(tran.SellOut_1H) / 1000000).toFixed(1) });
                        var q2 = FY + "_H2";
                        selloutX.push({ x: q2, y: (curObj.checkNull(tran.SellOut_2H) / 1000000).toFixed(1) });

                        // push two records for MDF
                        var h1 = FY + "_H1";
                        mdfX.push({ x: h1, y: (curObj.checkNull(tran.MDF_1H) / 1000).toFixed(1) });
                        var h2 = FY + "_H2";
                        mdfX.push({ x: h2, y: (curObj.checkNull(tran.MDF_2H) / 1000).toFixed(1) });
                    }

                    obj[i]['sellout'] = [{
                        values: selloutX,
                        key: 'SellOut'
                    }];

                    obj[i]['mdf'] = [{
                        values: mdfX,
                        key: 'MDF'
                    }]

                }


                return obj;
            },
            // Financial year to miliseconds
            convertFYtoDate: function (fy) {
                var year = "20" + fy.substring(2, 4);
                var date = fy.substring(5, 7);
                var month, date;
                if (date == 'H1' || date == 'h1') {
                    month = "01";
                    date = "01";
                } else {
                    month = "07";
                    date = "01";
                }

                date = year + "-" + month + "-" + date;
                return new Date(date).getTime();
            },

            // Financial year to miliseconds
            convertQuaterFYtoDate: function (fy) {

                var year = "20" + fy.substring(2, 4);
                var date = fy.substring(5, 7);
                var month, date;
                if (date == 'Q1' || date == 'q1') {
                    month = "01";
                    date = "01";
                }
                else if (date == 'Q2' || date == 'q2') {
                    month = "03";
                    date = "11";
                }
                else if (date == 'Q3' || date == 'q3') {
                    month = "07";
                    date = "15";
                }
                else {
                    month = "10";
                    date = "15";
                }

                date = year + "-" + month + "-" + date;
                return new Date(date).getTime();
            },
            checkNull: function (num) {
                if (!num || typeof num == 'undefined')
                    return 0;
                else
                    return num;
            },
            checkFY: function(chartData){
                
                var half = $localStorage.user.FinancialYear.split(" ")[1];
                if(angular.uppercase(half) == "1H"){
                    chartData.splice(chartData.length,1)
                    chartData.splice(0,1) // remove first elem
                }
                return chartData;
            },

            formatData: function(histories){
                var half = $localStorage.user.FinancialYear.split(" ")[1];
                for(var i=0;i<histories.length;i++){

                    var rows = histories[i].rows;
                    var mdfX = [];
                    var selloutX = [];
                    for(var j = 0; j < rows.length; j++){
                        var year = rows[j].FinancialYear;
                        var hy = year.split("_")[1];
                        var MDF_KEY = "MDF_"+hy;
                        var SELLOUT_KEY = "SellOut_"+hy;

                        mdfX.push({
                            x: year.replace("_"," "),
                            y: (rows[j][MDF_KEY] / 1000).toFixed(1)
                        })

                        selloutX.push({
                            x: year.replace("_"," "),
                            y: (rows[j][SELLOUT_KEY] / 1000000).toFixed(1)
                        })
                    }
                    histories[i].MDFX = [
                        {
                            values: mdfX,
                            key: 'MDF'
                        }
                    ];

                    histories[i].SELLOUTX = [
                        {
                            values: selloutX,
                            key: 'MDF'
                        }
                    ];
                    
                }
               // console.log(histories);
                return histories;
            }
        }
    }

})();