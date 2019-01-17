/*Final Summary Graph Factory*/
(function () {
    'use strict';
    angular.module("hpe").factory('fsGraphFactory',fsGraphFactoryFn)
    function fsGraphFactoryFn(){
    	return {
    		loadSummaryCharts: loadSummaryCharts
    	};

    	function loadSummaryCharts($scope) {
            $scope.chartOneOption = {
                chart: {
                    type: 'discreteBarChart',
                    height: 100,
                    width: 250,
                    margin: {
                        top: 15,
                        right: 0,
                        bottom: 15,
                        left: 40
                    }
                    , x: function (d) {
                        return d.label;
                    }
                    , y: function (d) {
                        return d.value + (1e-10);
                    }
                    , showValues: true
                    , showXAxis: false
                    , valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    }
                    , duration: 100
                    , color: colorFunction()
                    , yDomain: [$scope.min_of_array, $scope.max_of_array]
                    , yAxis: {
                        ticks: 5
                        , tickFormat: function (d) {
                            return d3.format(',.1f')(d) + '%'
                        }
                        , opacity: 0.1
                    }
                }
                , title: {
                    enable: true
                    , text: 'Sales growth of Partner With MDF vs without MDF'
                    , css: {
                        fontSize: "11.5px",
                        fontWeight: "bold",
                        textAlign: "left",
                        width: "200px",
                        lineHeight: "16px",
                        color: "#4D4D4D",
                        margin: "12px 0px"
                    }
                },
                caption: {
                    enable: true,
                    html: $scope.preYear + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Allocated",
                    css: {
                        marginTop: "-6%",
                    }
                }
            };

            $scope.chartOneZoomOption = {
                chart: {
                    type: 'discreteBarChart',
                    height: 400,
                    width: 600
                    , x: function (d) {
                        return d.label;
                    }
                    , y: function (d) {
                        return d.value + (1e-10);
                    }
                    , showValues: true
                    , showXAxis: false
                    , valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    }
                    , duration: 100
                    , color: colorFunction()
                    , yDomain: [$scope.min_of_array, $scope.max_of_array]
                    , yAxis: {
                        ticks: 6
                        , tickFormat: function (d) {
                            return d3.format(',.1f')(d) + '%'
                        }
                        , opacity: 0.1
                    }
                },
                caption: {
                    enable: true,
                    html: $scope.preYear + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Allocated",
                    css: {
                        marginTop: "-6%",
                        marginLeft: "20%"
                    }
                }
            };

            $scope.chartOneData = [
                {
                    key: "Cumulative Return"
                    , values: [
                        {
                            "label": $scope.preYear + " With MDF"
                            , "value": $scope.Default_SalesGrowth_with_mdf
                        }
                        , {
                            "label": $scope.preYear + " Without MDF"
                            , "value": $scope.Default_SalesGrowth_without_mdf
                        }
                                , {
                                    "label": "Allocated With MDF"
                            , "value": $scope.FSalesGrowth_with_mdf
                                }
                                , {
                                    "label": "Allocated Without MDF"
                            , "value": $scope.FSalesGrowth_without_mdf
                                }]
                }]

            //code for MDF/Sellout graph
            $scope.mdfOptions = {
                chart: {
                    type: 'discreteBarChart'
                    , height: 100,
                    width: 250,
                    margin: {
                        top: 20,
                        right: 0,
                        bottom: 18,
                        left: 40
                    }
                    , x: function (d) {
                        return d.label;
                    }
                    , y: function (d) {
                        return d.value + (1e-10);
                    },
                    color: function (d, i) {
                        return '#18b488'
                    }
                    , showValues: true
                    , valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    }
                    , duration: 500
                    , yDomain: [0, 5]
                    , yAxis: {
                        ticks: 3
                            , tickFormat: function (d) {
                                return d3.format(',.1f')(d) + '%'
                            }
                            , opacity: 0.1
                    }
                }
                , title: {
                    enable: true
                    , text: 'MDF/Sellout'
                    , css: {
                        fontSize: "11.5px",
                        fontWeight: "bold",
                        textAlign: "left",
                        width: "200px",
                        lineHeight: "16px",
                        color: "#4D4D4D",
                        margin: "12px 0px"
                    }
                }
            };

            $scope.mdfOptions1 = {
                chart: {
                    type: 'discreteBarChart'
                    , height: 400,
                    width: 600
                    , x: function (d) {
                        return d.label;
                    }
                    , y: function (d) {
                        return d.value + (1e-10);
                    },
                    color: function (d, i) {
                        return '#18b488'
                    }
                    , showValues: true
                    , valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    }
                    , duration: 500
                    , yDomain: [0, 5]
                    , yAxis: {
                        ticks: 3
                            , tickFormat: function (d) {
                                return d3.format(',f')(d) + '%'
                            }
                            , opacity: 0.1
                    }
                }
            };

            $scope.MdfData = [
                {
                    key: "Cumulative Return"
                    , values: [
                        {
                            "label": $scope.preYear
                            , "value": $scope.Default_MDFBYSellout_Percentage
                        }
                        , {
                            "label": "Allocated"
                            , "value": $scope.MDFBYSellout_Percentage
                        }
                    ]
                }
            ];

            //graph 3 loading

            $scope.chartThreeOption = {
                chart: {
                    type: 'multiBarChart',
                    height: 100,
                    width: 250,
                    margin: {
                        top: 10,
                        right: 0,
                        bottom: 35,
                        left: 45
                    },
                    clipEdge: true,
                    stacked: true,
                    color: colorFunction(),
                    valueFormat: function (d) {
                        return parseFloat(d).toFixed(1) + '%';
                    },
                    duration: 100,
                    yDomain: [0, 100],
                    showValues: true,
                    showLegend: false,
                    yAxis: {
                        ticks: 6,
                        opacity: 0.1,
                        tickFormat: function (d) { return parseFloat(d).toFixed(1) + '%' }
                    }
                },
                title: {
                    enable: true
                    , text: 'Alignment of Investment Change vs Sales Change'
                    , css: {
                        fontSize: "11.5px",
                        fontWeight: "bold",
                        textAlign: "left",
                        width: "200px",
                        lineHeight: "16px",
                        color: "#4D4D4D",
                        margin: "12px 0px"

                    }
                }
            };

            $scope.chartThreeOption1 = {
                chart: {
                    type: 'multiBarChart',
                    height: 400,
                    width: 600,
                    clipEdge: true,
                    stacked: true,
                    color: colorFunction(),
                    showValues: true,
                    valueFormat: function (d) {
                        return parseFloat(d).toFixed(1) + '%';
                    },
                    duration: 100,
                    yDomain: [0, 100],
                    showLegend: false,
                    yAxis: {
                        ticks: 6,
                        opacity: 0.1,
                        tickFormat: function (d) { return parseFloat(d).toFixed(1) + '%' }
                    }
                }
            };

            $scope.chartThreeData = [{
                "key": "Aligned",
                "values": [{
                    "x": $scope.preYear,
                    "y": $scope.DAligned_Percentage
                }, {
                    "x": "Allocated",
                    "y": $scope.FAligned_Percentage
                }]
            }, {
                "key": "Misaligned",
                "values": [{
                    "x": $scope.preYear,
                    "y": $scope.DMisAligned_Percentage
                }, {
                    "x": "Allocated",
                    "y": $scope.FMisAligned_Percentage
                }]
            }]

            //graph 4 data

            $scope.chartFourOption = {
                chart: {
                    type: 'multiBarChart',
                    height: 100,
                    width: 250,
                    margin: {
                        top: 20,
                        right: 0,
                        bottom: 20,
                        left: 40
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunctionBaseLine(),
                    valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    },
                    yDomain: [0, 100],
                    showValues: true,
                    showLegend: false,
                    yAxis: {
                        ticks: 5,
                        opacity: 0.1,
                        tickFormat: function (d) { return d3.format(',.1f')(d) + '%' }
                    }
                },
                title: {
                    enable: true
                    , text: 'Baseline MDF by membership tier'
                    , css: {
                        fontSize: "11.5px",
                        fontWeight: "bold",
                        textAlign: "left",
                        width: "200px",
                        lineHeight: "16px",
                        color: "#4D4D4D",
                        margin: "12px 0px"

                    }
                }
            };
            $scope.chartFourOption1 = {
                chart: {
                    type: 'multiBarChart',
                    height: 400,
                    width: 600,
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunctionBaseLine(),
                    valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    },
                    yDomain: [0, 100],
                    showValues: true,
                    showLegend: false,
                    yAxis: {
                        ticks: 5,
                        opacity: 0.1,
                        tickFormat: function (d) { return d3.format(',f')(d) + '%' }
                    }
                }

            };


            $scope.chartFourData = [{
                "key": "Platinum",
                "values": [{
                    "x": $scope.preYear,
                    "y": $scope.Default_MDF_Platinum_Percentage
                }, {
                    "x": "Allocated",
                    "y": $scope.Final_MDF_Platinum_Percentage
                }]
            }, {
                "key": "Gold",
                "values": [{
                    "x": $scope.preYear,
                    "y": $scope.Default_MDF_Gold_Percentage
                }, {
                    "x": "Allocated",
                    "y": $scope.Final_MDF_Gold_Percentage
                }]
            }, {
                "key": "Silver & Below",
                "values": [{
                    "x": $scope.preYear,
                    "y": $scope.Default_MDF_SB_Percentage
                }, {
                    "x": "Allocated",
                    "y": $scope.Final_MDF_SB_Percentage
                }]
            }]

            // Code for Projected Sellout Growth Scattered graph



            $scope.psgOptions = {
                chart: {
                    type: 'scatterChart',
                    height: 131,
                    width: 1100,
                    margin: {
                        top: 0,
                        right: 20,
                        bottom: 42,
                        left: 60
                    },
                    color: colorFunctionYOY(),
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
            $scope.psgOptions1 = {
                chart: {
                    type: 'scatterChart',
                    height: 400,
                    width: 600,
                    margin: {
                        top: 0,
                        right: 20,
                        bottom: 70,
                        left: 90
                    },
                    color: colorFunctionYOY(),
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
                        verticalOff: true,
                        unzoomEventType: 'dblclick.zoom'
                    }
                }
            };

            $scope.psgData = [{
                key: 'With MDF',
                values: $scope.MDF_Scatter_Array
            },
            {
                key: 'Without MDF',
                values: $scope.Without_MDF_Scatter_Array
            }];

        }

        function colorFunctionBaseLine() {
            return function (d, i) {
                if (i === 0 || i === 3) {
                    return '#425563';
                } else if (i === 1 || i === 4) {
                    return '#FFD700';
                } else if (i === 2 || i === 5) {
                    return '#e5e4e2';
                }

            };
        }

        function colorFunction() {
            return function (d, i) {
                if (i % 2 !== 0) {
                    return '#18b488'
                }
                else {
                    return '#333'
                }
            };
        }

        function colorFunctionYOY() {
            return function (d, i) {
                if (i % 2 !== 0) {
                    return '#333'
                } else {
                    return '#18b488'
                }
            };
        }
    }
    	
})();
