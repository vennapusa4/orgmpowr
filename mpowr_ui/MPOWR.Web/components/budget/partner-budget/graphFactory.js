
/*
Historical Factory
Created by: Aamin Khan
Created at: 06/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").factory("graphFactory", graphFactoryFn);

    function graphFactoryFn($rootScope, $http, HistoricalFactory, modelParameterFactory, $sessionStorage, $timeout, $localStorage) {
        return {
            formGraph: function ($scope, buId) {
                var curObj = this;
                var reseler = $sessionStorage.resellerPartner;
                var user = $localStorage.user;
                var versionId = $sessionStorage.currentVersion;
                $scope.title = reseler.obj.geo.DisplayName != 'North America' ? 'Carve outs' : 'MSA';
              //  $scope.graphData = {};
                $scope.salesData = [];
                $scope.mdfBySelloutData = [];
                $scope.investmentChangeData = [];
                $scope.baselineMDFbyMembershipData = [];
                $scope.data51 = [];
                $http({
                    method: 'POST',
                    url: $rootScope.api + "PartnerBudgetGraph",
                    data: {
                        "Versionid": versionId.VersionID,
                        /*"BusinessUnitID":isAll*/
                        BusinessUnitID: buId
                    }
                }).then(function (resp) {
                    var data = resp.data;
                    try {
                        var allocatedGraph = data.Graphs[0].AllocatedGraph;

                        var CarveOutGraph = data.Graphs[0].CarveOutGraph;
                        if (buId == 0) {
                            var msaGraph = data.Graphs[0].MSAGraph[0];
                            var AurubamsaGraph = data.Graphs[0].ARUBAMSAGraph[0];
                        }
                        var SalesGraph = data.Graphs[0].SalesGraph;
                        var MDF_SelloutGraph = data.Graphs[0].MDF_SelloutGraph[0];
                        var AlignedGraph = data.Graphs[0].AlignedGraph[0];
                        var MemberShipTier = data.Graphs[0].MemberShipTier[0];
                    } catch (e) {
                    }

                    $scope.graphData = data.Graphs[0];

                    /*Scater PLOT Graph*/
                    var scatterPlot = data["Graphs"][0]['PlottedGraph'];

                    $scope.MDF_Scatter_Array = [];
                    $scope.Without_MDF_Scatter_Array = [];
                    if (scatterPlot) {
                        angular.forEach(scatterPlot, function (obj) {
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

                        $scope.options51 = {
                            chart: {
                                type: 'scatterChart',
                                height: 120,
                                width: 800,
                                 margin: {
                                    top: 0,
                                    right: 280,
                                    bottom: 42,
                                    left: 50
                                },
                                color: function (d, i) {
                                    if (i % 2 !== 0) {
                                        return '#333'
                                    } else {
                                        return '#18b488'
                                    }
                                },
                                scatter: {
                                    onlyCircles: true
                                },
                                showDistX: true,
                                showDistY: true,
                                tooltip: {
                                    contentGenerator: function (d) {
                                        return '<h5>' + d['point'].partnerName + '<br/>' + "partner size: " + (d['point'].x / 1000000).toFixed(1) + 'M' + '<br/>' + "projected Sellout Growth: " + parseFloat(d['series'][0].value.toFixed(1).replace(/"/g, '')) + '<br/>' + d['series'][0].key + '</h5>';
                                    }
                                },
                                duration: 350,
                                 xAxis: {
                                    axisLabel: 'Partner Size',
                                     tickFormat: function (d) {
                                        var num = Math.round(d);
                                        return (d3.format('d')(num) / 1000000).toFixed(0) + 'M';
                                    }

                                 },
                                 yAxis: {
                                    axisLabel: 'Projected Sellout Growth',
                                    tickFormat: function (d) {
                                        return d3.format('d')(d);
                                    }
                                },
                                zoom: {
                                    //NOTE: All attributes below are optional
                                    enabled: false,
                                    scaleExtent: [0, 0],
                                    useFixedDomain: false,
                                    useNiceScale: false,
                                    horizontalOff: false,
                                    verticalOff: false,
                                    unzoomEventType: 'dblclick.zoom'
                                }
                            }
                        };

                        $scope.options53 = {
                            chart: {
                                type: 'scatterChart',
                                height: 120,
                                width: 800,
                                margin: {
                                    top: 0,
                                    right: 42,
                                    bottom: 42,
                                    left: 50
                                },
                                color: function (d, i) {
                                    if (i % 2 !== 0) {
                                        return '#333'
                                    } else {
                                        return '#18b488'
                                    }
                                },
                                scatter: {
                                    onlyCircles: true
                                },
                                showDistX: true,
                                showDistY: true,
                                tooltip: {
                                    contentGenerator: function (d) {
                                        return '<h5>' + d['point'].partnerName + '<br/>' + "partner size: " + (d['point'].x / 1000000).toFixed(1) + 'M' + '<br/>' + "projected Sellout Growth: " + parseFloat(d['series'][0].value.toFixed(1).replace(/"/g, '')) + '<br/>' + d['series'][0].key + '</h5>';
                                    }
                                },
                                duration: 350,
                                xAxis: {
                                    axisLabel: 'Partner Size',
                                    tickFormat: function (d) {
                                        var num = Math.round(d);
                                        return (d3.format('d')(num) / 1000000).toFixed(0) + 'M';
                                    }

                                },
                                yAxis: {
                                    axisLabel: 'Projected Sellout Growth',
                                    tickFormat: function (d) {
                                        return d3.format('d')(d);
                                    }
                                },
                                zoom: {
                                    //NOTE: All attributes below are optional
                                    enabled: false,
                                    scaleExtent: [0, 0],
                                    useFixedDomain: false,
                                    useNiceScale: false,
                                    horizontalOff: false,
                                    verticalOff: false,
                                    unzoomEventType: 'dblclick.zoom'
                                }
                            }
                        };

                        $scope.options52 = {
                            chart: {
                                type: 'scatterChart',
                                height: 220,
                                width: 700,
                                margin: {
                                    top: 40,
                                    right: 60,
                                    bottom: 30,
                                    left: 30
                                },
                                color: function (d, i) {
                                    if (i % 2 !== 0) {
                                        return '#333'
                                    } else {
                                        return '#18b488'
                                    }
                                },
                                scatter: {
                                    onlyCircles: true
                                },
                                showDistX: true,
                                showDistY: true,
                                tooltip: {
                                    contentGenerator: function (d) {
                                        return '<h5>' + d['point'].partnerName + '<br/>' + "partner size: " + (d['point'].x / 1000000).toFixed(1) + 'M' + '<br/>' + "projected Sellout Growth: " + parseFloat(d['series'][0].value.toFixed(1).replace(/"/g, '')) + '<br/>' + d['series'][0].key + '</h5>';
                                    }
                                },
                                duration: 350,
                                xAxis: {
                                    axisLabel: '',
                                    tickFormat: function (d) {
                                        var num = Math.round(d);
                                        return (d3.format('d')(num) / 1000000).toFixed(0) + 'M';
                                    }
                                },
                                yAxis: {
                                    axisLabel: '',
                                    tickFormat: function (d) {
                                        return d3.format('d')(d);
                                    }
                                },
                                zoom: {
                                    //NOTE: All attributes below are optional
                                    enabled: false,
                                    scaleExtent: [0, 0],
                                    useFixedDomain: false,
                                    useNiceScale: false,
                                    horizontalOff: false,
                                    verticalOff: false,
                                    unzoomEventType: 'dblclick.zoom'
                                }
                            }
                        };

                        $scope.data51 = [{
                            key: 'With MDF',
                            values: $scope.MDF_Scatter_Array
                        },
			            {
			                key: 'Without MDF',
			                values: $scope.Without_MDF_Scatter_Array
			            }];

                    }

                    /*** END OF Scater Plot ***/

                    //alocated
                    var allocated = [];

                    for (var i = 0; i < allocatedGraph.length; i++) {
                        allocated.push({
                            name: curObj.getBUName(allocatedGraph[i].Business_Unit),
                            allocated: allocatedGraph[i].Allocated,
                            remaining: allocatedGraph[i].Remaining,
                            total: allocatedGraph[i].TotalBaseline
                        });

                    }
                    
                    $scope.allocatedVsRemainingData = allocated;
                    curObj.initDonut($scope, allocated, "#chart2")
                 
                    //CarveOutAllocated
                    var Carveoutallocated = [];

                    if (reseler.obj.geo.DisplayName != 'North America') {
                        for (var i = 0; i < CarveOutGraph.length; i++) {
                            Carveoutallocated.push({
                                name: curObj.getBUName(CarveOutGraph[i].Business_Unit),
                                allocated: CarveOutGraph[i].Allocated,
                                remaining: CarveOutGraph[i].Remaining,
                                total: CarveOutGraph[i].Total_CarveOut
                            });

                        }
                    }
                    // if united states and then add the MSA Graph
                    if ((reseler.district.DistrictID > 0) && buId == 0) {
                        Carveoutallocated.push({
                            name: 'MSA',
                            allocated: msaGraph.Allocate,
                            remaining: msaGraph.Remains,
                            total: msaGraph.Total_MSA
                        });
                        Carveoutallocated.push({
                            name: 'ARUBA MSA',
                            allocated: AurubamsaGraph.Allocate,
                            remaining: AurubamsaGraph.Remains,
                            total: AurubamsaGraph.Total_ARUBAMSA
                        });
                    }

                    if (((((reseler.obj.countries == null || reseler.obj.countries.length == 0) ? reseler.obj.geo.DisplayName == 'North America' : reseler.obj.countries[0].DisplayName == 'CANADA') && reseler.partner.PartnerName == 'Reseller')) && buId == 0) {
                        Carveoutallocated.push({
                            name: 'ARUBA MSA',
                            allocated: AurubamsaGraph.Allocate,
                            remaining: AurubamsaGraph.Remains,
                            total: AurubamsaGraph.Total_ARUBAMSA
                        });
                    }

                    $scope.Carveout_allocatedVsRemainingData = Carveoutallocated;
                    if (Carveoutallocated.length >0) {
                        curObj.initDonut($scope, Carveoutallocated, "#chart3")
                    }
                    /*$scope.allocatedVsRemainingData = [
					  {
					      "key": "Allocated MDF($)",
					      "values": allocated
					  },
					  {
					      "key": "Remaining MDF($)",
					      "values": remaining
					  }
                    ];*/

                    $scope.allocatedVsRemainingOption = {
                        chart: {
                            type: 'multiBarChart',
                            height: 120,
                            width: 350,
                            margin: {
                                top: 10,
                                right: 50,
                                bottom: 50,
                                left: 45
                            },
                            clipEdge: true,
                            duration: 500,
                            stacked: true,
                            xAxis: {

                                showMaxMin: false,
                                tickFormat: function (d) {
                                    // get business unit name
                                    var o = _.filter($localStorage.businessUnits, function (o) { return o.Name == d; });
                                    var total = 0;
                                    if (d !== 'MSA') {

                                        if (typeof o[0] !== 'undefined') {
                                            var id = o[0].ID;
                                            var tx = _.filter(allocatedGraph, function (o) { return o.Business_Unit == id; });
                                            total = tx[0].TotalBaseline;
                                        }
                                        var prefix = d3.formatPrefix(total);
                                        total = prefix.scale(total) + prefix.symbol;

                                    } else {
                                        total = msaGraph.Total_MSA
                                        var prefix = d3.formatPrefix(total);
                                        total = prefix.scale(total) + prefix.symbol;
                                    }

                                    return d.slice(0, 3) + "-$" + total;//d3.format(',f')(d);
                                },
                                //ticks: 15,
                                rotateLabels: -45,
                            },
                            yAxis: {
                                ticks: 5,
                                opacity: 0.1,
                                tickFormat: function (d) {
                                    var prefix = d3.formatPrefix(d);
                                    return prefix.scale(d) + prefix.symbol;

                                    //return d3.format(',f')(d) ;//+ '%' 
                                }
                            },
                            color: function (d, i) {
                                if (i % 2 !== 0) {
                                    return '#333'
                                } else {
                                    return '#18b488'
                                }
                            },
                            showValues: true,
                            showXAxis: true,
                            showLegend: false
                        }
                    };

                    $scope.allocatedVsRemainingOptionExpanded = {
                        chart: {
                            type: 'multiBarChart',
                            height: 120,
                            width: 350,
                            margin: {
                                top: 10,
                                right: 50,
                                bottom: 70,
                                left: 45
                            },
                            clipEdge: true,
                            duration: 500,
                            stacked: true,
                            xAxis: {

                                showMaxMin: false,
                                tickFormat: function (d) {
                                    // get business unit name
                                    var o = _.filter($localStorage.businessUnits, function (o) { return o.Name == d; });
                                    var total = 0;
                                    if (d !== 'MSA') {

                                        if (typeof o[0] !== 'undefined') {
                                            var id = o[0].ID;
                                            var tx = _.filter(allocatedGraph, function (o) { return o.Business_Unit == id; });
                                            total = tx[0].TotalBaseline;
                                        }
                                        var prefix = d3.formatPrefix(total);
                                        total = prefix.scale(total) + prefix.symbol;

                                    } else {
                                        total = msaGraph.Total_MSA
                                        var prefix = d3.formatPrefix(total);
                                        total = prefix.scale(total) + prefix.symbol;
                                    }

                                    return d + "-$" + total;//d3.format(',f')(d);
                                },
                                //ticks: 15,
                                rotateLabels: -45,
                            },
                            yAxis: {
                                ticks: 5,
                                opacity: 0.1,
                                tickFormat: function (d) {
                                    var prefix = d3.formatPrefix(d);
                                    return prefix.scale(d) + prefix.symbol;

                                    //return d3.format(',f')(d) ;//+ '%' 
                                }
                            },
                            color: function (d, i) {
                                if (i % 2 !== 0) {
                                    return '#333'
                                } else {
                                    return '#18b488'
                                }
                            },
                            showValues: true,
                            showXAxis: true,
                            showLegend: false
                        }
                    };

                    //console.log(JSON.stringify($scope.allocatedVsRemainingData));

                    $scope.max_of_array = Math.max.apply(Math, [SalesGraph[0].default_Growth_With_MDF, SalesGraph[0].default__Growth_Without_MDF, SalesGraph[0].final_Growth_With_MDF, SalesGraph[0].final_Growth_Without_MDF]);
                    $scope.min_of_array = Math.min.apply(Math, [SalesGraph[0].default_Growth_With_MDF, SalesGraph[0].default__Growth_Without_MDF, SalesGraph[0].final_Growth_With_MDF, SalesGraph[0].final_Growth_Without_MDF]);
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


                    $scope.salesData = [
                    {
                        key: "Cumulative Return",
                        values: [
                              {
                                  "label": "Calculated with MDF",
                                  "value": SalesGraph[0].default_Growth_With_MDF
                              },
                              {
                                  "label": "Calculated without MDF",
                                  "value": SalesGraph[0].default__Growth_Without_MDF
                              },
                              {
                                  "label": "Allocated with MDF",
                                  "value": SalesGraph[0].final_Growth_With_MDF
                              },
                              {
                                  "label": "Allocated without MDF",
                                  "value": SalesGraph[0].final_Growth_Without_MDF
                              }
                        ]
                    }];

                    $scope.mdfBySelloutData = [
                    {
                        key: "Cumulative Return",
                        values: [
                          {
                              "label": "Calculated",
                              "value": MDF_SelloutGraph.default_Sellout_Last_Period
                          },
                          {
                              "label": "Allocated",
                              "value": MDF_SelloutGraph.Final_Sellout_Last_Period
                          }
                        ]
                    }
                    ];


                    //AlignedGraph
                    $scope.investmentChangeData = [
					  {
					      "key": "Alligned",
					      "values": [
                            {
                                "x": "Calculated",
                                "y": AlignedGraph.default_Aligned
                            },
                            {
                                "x": "Allocated",
                                "y": AlignedGraph.Final_Aligned
                            }
					      ]
					  },
					  {
					      "key": "Misaligned",
					      "values": [
                            {
                                "x": "Calculated",
                                "y": AlignedGraph.default_MisAligned
                            },
                            {
                                "x": "Allocated",
                                "y": AlignedGraph.Final_MisAligned
                            }
					      ]
					  }
                    ]

                    //MemberShipTier

                    $scope.baselineMDFbyMembershipData = [

					  {
					      "key": "Platinum",
					      "values": [
                            {
                                "x": " ",
                                "y": MemberShipTier.default_Plat
                            },
                            {
                                "x": "  ",
                                "y": MemberShipTier.Final_Plat
                            }
					      ]
					  },

					  {
					      "key": "Gold",
					      "values": [
                            {
                                "x": " ",
                                "y": MemberShipTier.default_Gold
                            },
                            {
                                "x": "  ",
                                "y": MemberShipTier.Final_Gold
                            }
					      ]
					  },
					  {
					      "key": "Silver & Bellow",
					      "values": [
                            {
                                "x": " ",
                                "y": MemberShipTier.Default_SB
                            },
                            {
                                "x": "  ",
                                "y": MemberShipTier.Final_SB
                            }
					      ]
					  }
                    ]
 
                })
                // ********* Allocated vs Remaining Graph **********
                /*$scope.allocatedVsRemainingOption = {
                    chart: {
                        type: 'multiBarChart',
                        height: 120,
                        width: 250,
                        margin: {
                            top: 10,
                            right: 50,
                            bottom: 20,
                            left: 45
                        },
                        clipEdge: true,
                        duration: 500,
                        stacked: true,
                        color: function (d, i) {
                            if (i % 2 !== 0) {
                                return '#333'
                            } else {
                                return '#18b488'
                            }
                        },
                        valueFormat: function (d) {
                            return d3.format(',.0f')(d) + '%';
                        },

                        
                        showValues: true,
                        showXAxis: false,
                        showLegend: false,

                        yAxis: {
                            ticks: 5,
                            opacity: 0.1,
                            tickFormat: function (d) { 
                                var prefix = d3.formatPrefix(d);
                                return prefix.scale(d) + prefix.symbol;

                                //return d3.format(',f')(d) ;//+ '%' 
                            }
                        },
                        xAxis: {
                            rotateLabels: -45,
                            ticks: 6
                        }
                    }
                };*/
 
                //********** Sales growth of partners with MDF vs Without

                $scope.salesOptions = {
                    chart: {
                        type: 'discreteBarChart',
                        height: 120,
                        width: 250,
                        margin: {
                            top: 10,
                            right: 50,
                            bottom: 20,
                            left: 60
                        },
                        x: function (d) { return d.label; },
                        y: function (d) { return d.value + (1e-10); },
                        showValues: true,
                        showXAxis: false,
                        valueFormat: function (d) {
                            return parseFloat(d).toFixed(1) + '%';
                        },

                        duration: 100,
                        color: function (d, i) {
                            if (i % 2 !== 0) {
                                return '#18b488'
                            } else {
                                return '#333'
                            }
                        },

                        yAxis: {

                            tickFormat: function (d) {
                                return parseFloat(d).toFixed(1) + '%';
                            },
                            opacity: 0.1
                        }
                    },
                    caption: {
                        enable: true,
                        html: " &nbsp; Calculated &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Allocated",
                        css: {
                            width: "100%"
                        }
                    }
                };
 
                // ********************MDF/Sellout*****************************

                $scope.mdfBySelloutOptions = {
                    chart: {
                        type: 'discreteBarChart',
                        height: 120,
                        width: 250,
                        margin: {
                            top: 0,
                            right: 50,
                            bottom: 20,
                            left: 60
                        },
                        x: function (d) { return d.label; },
                        y: function (d) { return d.value + (1e-10); },
                        showValues: true,

                        valueFormat: function (d) {
                            return parseFloat(d).toFixed(1) + '%';
                        },
                        duration: 100,
                        color: function (d, i) {
                            if (i % 2 !== 0) {
                                return '#2AD2C9'
                            } else {
                                return '#2AD2C9'
                            }
                        },
                        /*yDomain: [0, 100],*/
                        yAxis: {
                            ticks: 6,
                            tickFormat: function (d) { return d3.format(',.1f')(d) + '%' },
                            opacity: 0.1
                        }
                    }





                };
 

                // ****************** Allignement of investment change vs sales change

                $scope.investmentChangeOption = {
                    chart: {
                        type: 'multiBarChart',
                        height: 120,
                        width: 300,
                        margin: {
                            top: 20,
                            right: 50,
                            bottom: 20,
                            left: 45
                        },
                        clipEdge: true,
                        duration: 500,
                        stacked: true,
                        color: function (d, i) {
                            if (i % 2 !== 0) {
                                return '#111'
                            } else {
                                return '#5FDDD6'
                            }
                        },
                        valueFormat: function (d) {
                            return parseFloat(d).toFixed(1) + '%';
                        },
                        yDomain: [0, 100],
                        showValues: true,
                        showLegend: false,
                        yAxis: {
                            ticks: 5,
                            opacity: 0.1,
                            tickFormat: function (d) {
                                //return d3.format(',f')(d) + '%' ;
                                return parseFloat(d).toFixed(1) + '%';
                            }
                        },

                    }
                };
 
                /********************Baseline MDF by membership tire ****************/
                $scope.baselineMDFbyMembershipOption = {
                    chart: {
                        type: 'multiBarChart',
                        height: 150,
                        width: 270,
                        margin: {
                            top: 20,
                            right: 50,
                            bottom: 45,
                            left: 45
                        },
                        clipEdge: true,
                        duration: 500,
                        stacked: true,
                        color: function (d, i) {
                            if (i % 2 != 0) {
                                return '#FBB03B'
                            } else if (i % 3 == 0) {
                                return '#111'
                            } else {
                                return '#CCCCCC'
                            }
                        },
                        valueFormat: function (d) {
                            return parseFloat(d).toFixed(1) + '%';
                        },
                        yDomain: [0, 100],
                        showValues: true,
                        showLegend: false,
                        yAxis: {
                            ticks: 5,
                            opacity: 0.1,
                            tickFormat: function (d) {
                                return parseFloat(d).toFixed(1) + '%';
                            }
                        },

                    },
                    caption: {
                        enable: true,
                        html: "Calculated &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Allocated",
                        css: {
                            marginTop: "-35px"
                        }
                    }
                };
            },

            getBUName: function (id) {
                var o = _.filter($localStorage.businessUnits, function (o) { return o.ID == id; });
                return o[0].Name;
            },

            initDonutOnTimeout: function ($scope, data) {
                var curObj = this;
                $timeout(function () {
                    curObj.initDonut($scope, data,chartid)
                }, 1000)
            },

            initDonut: function ($scope, dx, chartid) {
                var reseler = $sessionStorage.resellerPartner;

                $scope.dx = [{
                    "label": "One",
                    "value": 50
                },
		        {
		            "label": "Two",
		            "value": 25
		        },
		        {
		            "label": "Three",
		            "value": 25
		        }
                ];

                var donutData = [
                  {
                      label: 'Allocated',
                      value: dx[0].allocated
                  },
                  {
                      label: 'Remaining',
                      value: dx[0].remaining
                  }
                ]


                nv.addGraph(function () {
                    var chart = nv.models.pieChart()
                        .x(function (d) { return d.label })
                        .y(function (d) { return d.value })
                        .showLabels(true)     //Display pie labels
                        .labelThreshold(.05)  //Configure the minimum slice size for labels to show up
                        .labelType("value") //Configure what type of data to show in the label. Can be "key", "value" or "percent"
                        .donut(true)          //Turn on Donut mode. Makes pie chart look tasty!
                        .donutRatio(0.35)     //Configure how big you want the donut hole size to be.
                        .width(140).height(140)
                        .color(['#18B488', '#666666', '#87E5E0'])
                        .margin({ top: -10, right: 0, bottom: 0, left: -20 })
                        .valueFormat(function (d) {
                            return parseFloat(d).toFixed(0);
                        })

                    ;
                  
                    // chart.pie.startAngle(function(d) { return d.startAngle/2 -Math.PI/2 })
                    //chart.pie.endAngle(function(d) { return d.endAngle/2 -Math.PI/2 });
                    chart.pie.donutLabelsOutside(false);
                    chart.legend.margin({ top: 50, right: 0, left: 190, bottom: 0 })

                    d3.select(chartid+" svg")
                        .datum(donutData)
                        .transition().duration(350)
                        .call(chart);


                    return chart;
                });

                var prefix = d3.formatPrefix(dx[0].total);
                var totalValue = prefix.scale(dx[0].total) + prefix.symbol;
                var width = 130;
                var height = 150;
             

                     var svg = d3.select(chartid +" svg").append("svg")
                      .attr("width", width)
                      .attr("height", height)
                      .append("g")
                      .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

                     $(chartid +" .total-text").remove();

                    svg.append("text")
                     .attr("text-anchor", "middle")
                     .attr("class", "total-text")
                     .text("$" + totalValue);
              }

        }
    }
})();


