/*
Model Parameter controller
Created by: Rohit Kumar
Created at: 08/02/2017
*/
(function () {
    'use strict';
    angular.module("hpe").controller("ModelParameterCtrl", modelParameterCtrlFn);

    function modelParameterCtrlFn($scope, $http, $rootScope, modelParameterFactory, $window, Notification, $sessionStorage, $timeout, $localStorage, $state, roleMappingService, budgetFactory, versioningservice, $filter) {
        //ngInject
        $scope.role = roleMappingService.apply();
        $scope.sessionStorage = $window.sessionStorage;
        $scope.countryName = $sessionStorage.resellerPartner.obj.name;// JSON.parse(sessionStorage["ngStorage-resellerPartner"])["obj"].name;
        $scope.versionId = $sessionStorage.currentVersion.VersionID;
        var inputControls = [];
        var countries;
        angular.forEach($sessionStorage.resellerPartner.obj.countries, function (rec) {
            if (countries == undefined)
                countries = rec.CountryID;
            else
                countries = ',' + rec.CountryID;
        });
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
                    //else if ($rootScope.disablePrevious == true && $rootScope.previousPlan == true) {
                    //    $localStorage.NOACCESS = false;
                    //    $scope.NOACCESS = $localStorage.NOACCESS;
                    //}
                    else {
                        $localStorage.NOACCESS = false;
                        $scope.NOACCESS = $localStorage.NOACCESS;
                    }
                    //    Notification.error({ message: 'The Plan is disabled due to any of the Geo/Country/District/BusinessUnit is made InActive', delay: null });
                })
            //}
        });
        $scope.NOACCESS = $localStorage.NOACCESS;
        $scope.countryId = countries;// JSON.parse(sessionStorage["ngStorage-resellerPartner"])["obj"].id;
        $scope.PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;// JSON.parse(sessionStorage["ngStorage-resellerPartner"])["partner"].PartnerTypeID;
        //$scope.DistrictID = parseInt((JSON.parse(sessionStorage["ngStorage-resellerPartner"])["district"].DistrictID === undefined || JSON.parse(sessionStorage["ngStorage-resellerPartner"])["district"].DistrictID.toString() === '0') ? '0' : JSON.parse(sessionStorage["ngStorage-resellerPartner"])["district"].DistrictID);
        $scope.DistrictID = parseInt($sessionStorage.resellerPartner.district.DistrictID === undefined || $sessionStorage.resellerPartner.district.DistrictID === '0' ? '0' : $sessionStorage.resellerPartner.district.DistrictID);
        $scope.FinancialYearID = $localStorage.user.FinancialYearID;
        $scope.total = true;

        // $scope.versiondata = $sessionStorage.currentVersion;
        $scope.versiondata = $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion : $localStorage.selectedversion;
        if ($localStorage.selectedversion != undefined) {
            $scope.versiondata.Financialyear = $localStorage.selectedversion.FinancialPeriod;
        }


        var reseller = $sessionStorage.resellerPartner;
        $scope.bucket = reseller.partner.PartnerName + ($scope.DistrictID == 0 ? '' :' - ' + reseller.district.DistrictName);
        $scope.membership = reseller.membership.MembershipName;
        $scope.BU = $localStorage.businessUnits;
        
        $scope.currentBu = $scope.BU[0];
        $scope.currentBuTabIndex = 0;
        $scope.disableExpression = true;
        budgetFactory.selectText();

        decorate()
        /*Decorate the number textbox*/
        function decorate() {
            $(".specialText").priceFormat();
            $timeout(function () {
                $(".specialText1").priceFormat();
            }, 200)
        }
        $scope.$sessionstorage = $sessionStorage;
        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                $scope.ctries = _.pluck(countryList, 'DisplayName').join(", ");
            }
        }

        $scope.updatePerfomance = function (highPerf, buId, index) {
            highPerf = parseInt(highPerf);
            $scope.data[1][index].High_Performance = highPerf * 1;
            $scope.data[1][index].Medium_Performance = highPerf * 1.2;
            $scope.data[1][index].Low_Performance = highPerf * 1.5;
            
        }



        $scope.validateRangedInput = function (input, min, max, id) {
            if (parseInt(input) < min) {
                $('#' + id).val(min);
                return min;
            } else if (parseInt(input) > max) {
                $('#' + id).val(max);
                return max;
            } else {
                return input;
            }
        }

        //dynamically setting the tabs for model parameter enabled for the user to navigate based on business units
        modelParameterFactory.getBusinessUnits().then(
            function onSuccess(response) {
                $scope.businessUnits = response.data;
                angular.forEach($scope.businessUnits, function (obj) {
                    if (obj.Name === 'DCN') {
                        $scope.enableDcn = true;
                    }
                    if (obj.Name === 'Compute') {
                        $scope.enableCompute = true;
                    }
                    if (obj.Name === 'Storage') {
                        $scope.enableStorage = true;
                    }
                    if (obj.Name === 'Pointnext') {
                        $scope.enablePointnext = true;
                    }
                    if (obj.Name === 'Aruba Products') {
                        $scope.enableArubaProducts = true;
                    }
                    if (obj.Name === 'Aruba Services') {
                        $scope.enableArubaServices = true;
                    }
                    if (obj.Name === 'Compute Volume') {
                        $scope.enableArubaServices = true;
                    }
                    if (obj.Name === 'Compute Value') {
                        $scope.enableComputeVolume = true;
                    }
                });
            },
            function onError(response) {
                Notification.error({
                    message: response.data,
                    delay: null
                });
            }
        )

        modelParameterFactory.getBUAccess().then(
            function onSuccess(response) {
                $scope.getBUAccess = !response.data;

            },
            function onError(response) {
                Notification.error({
                    message: response.data,
                    delay: null
                });
            }
        )

        var updateTheReceivedData = function (data) {
            data[0][0].FinancialYearID = $scope.FinancialYearID;
            //data[0][0].CountryID = $scope.countryId;
            //data[0][0].DistrictID = $scope.DistrictID;
            data[0][0].PartnerTypeID = $scope.PartnerTypeID;
            data[0][0].VersionID = $sessionStorage.currentVersion.VersionID
        }

        //method created to load and reset the model parameter values  $scope.countryId, $scope.PartnerTypeID, $scope.DistrictID, $scope.FinancialYearID
        $scope.backUpModelParamData = [];
        modelParameterFactory.getData($scope.countryId, $scope.PartnerTypeID, $scope.DistrictID, $scope.FinancialYearID, $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion.VersionID : $localStorage.selectedversion.VersionID).then(
            function onSuccess(response) {
                $scope.data = response.data;

                $scope.data[1] = $filter("orderBy")($scope.data[1], 'BusinessUnitID');
                
                updateTheReceivedData($scope.data);
                angular.copy($scope.data, $scope.backUpModelParamData);
                $timeout(function () {
                    $(".specialText").priceFormat();
                }, 100)
                setupSlider();
            },
            function onError(response) {
                //Notification.error({
                //    message: response.data,
                //    delay: null
                //});
            }
        )

        var loadYOYGrapghData = function (yoyData) {
            $scope.MDF_Scatter_Array = [];
            $scope.Without_MDF_Scatter_Array = [];
            if (yoyData) {
                angular.forEach(yoyData, function (obj) {
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
            loadingScatterGraph();
        }

        var loadGrapghData = function (gdata) {
            //baseline MDF graph data
            $scope.DSustain_MDF_Percentage = parseFloat((gdata.Default_Sustain_MDF_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.DGrowth_MDF_Percentage = parseFloat((gdata.Default_Growth_MDF_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.FSustain_MDF_Percentage = parseFloat((gdata.Final_Sustain_MDF_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.FGrowth_MDF_Percentage = parseFloat((gdata.Final_Growth_MDF_Percentage * 100).toFixed(1).replace(/"/g, ''));

            //Sales growth graph data
            $scope.Default_SalesGrowth_with_mdf = parseFloat((gdata.Default_SalesGrowth_with_mdf * 100).toFixed(1).replace(/"/g, ''));
            $scope.Default_SalesGrowth_without_mdf = parseFloat((gdata.Default_SalesGrowth_without_mdf * 100).toFixed(1).replace(/"/g, ''));
            $scope.FSalesGrowth_with_mdf = parseFloat((gdata.SalesGrowth_with_mdf * 100).toFixed(1).replace(/"/g, ''));
            $scope.FSalesGrowth_without_mdf = parseFloat((gdata.SalesGrowth_without_mdf * 100).toFixed(1).replace(/"/g, ''));

            $scope.max_of_array = Math.max.apply(Math, [$scope.Default_SalesGrowth_with_mdf, $scope.Default_SalesGrowth_without_mdf, $scope.FSalesGrowth_with_mdf, $scope.FSalesGrowth_without_mdf]);
            $scope.min_of_array = Math.min.apply(Math, [$scope.Default_SalesGrowth_with_mdf, $scope.Default_SalesGrowth_without_mdf, $scope.FSalesGrowth_with_mdf, $scope.FSalesGrowth_without_mdf]);




            if ($scope.max_of_array > 0) {
                $scope.max_of_array = $scope.max_of_array + (10 - ($scope.max_of_array % 10))
            } else {
                if ($scope.max_of_array < 0 && $scope.min_of_array < 0) {
                    $scope.max_of_array = 10;
                }
                else {
                    $scope.max_of_array = $scope.max_of_array - (10 + ($scope.max_of_array % 10))
                }
            }

            if ($scope.min_of_array > 0) {

                if ($scope.max_of_array > 0 && $scope.min_of_array > 0) {
                    $scope.min_of_array = -10;
                }
                else {
                    $scope.min_of_array = $scope.min_of_array + (10 - ($scope.min_of_array % 10))
                }
            } else {
                $scope.min_of_array = $scope.min_of_array - (10 + ($scope.min_of_array % 10))

            }


            //MDF/Sellout graph Data
            $scope.Default_MDFBYSellout_Percentage = parseFloat((gdata.Default_MDFBYSellout_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.MDFBYSellout_Percentage = parseFloat((gdata.MDFBYSellout_Percentage * 100).toFixed(1).replace(/"/g, ''));

            //Alignment of investment graph Data
            $scope.DAligned_Percentage = parseFloat((gdata.Default_Aligned_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.DMisAligned_Percentage = parseFloat((gdata.Default_Misaligned_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.FAligned_Percentage = parseFloat((gdata.Aligned_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.FMisAligned_Percentage = parseFloat((gdata.MisAligned_Percentage * 100).toFixed(1).replace(/"/g, ''));

            //Baseline MDF by membership tier
            $scope.Default_MDF_Platinum_Percentage = parseFloat((gdata.Default_MDF_Platinum_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.Default_MDF_Gold_Percentage = parseFloat((gdata.Default_MDF_Gold_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.Default_MDF_SB_Percentage = parseFloat((gdata.Default_MDF_SB_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.Final_MDF_Platinum_Percentage = parseFloat((gdata.MDF_Platinum_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.Final_MDF_Gold_Percentage = parseFloat((gdata.MDF_Gold_Percentage * 100).toFixed(1).replace(/"/g, ''));
            $scope.Final_MDF_SB_Percentage = parseFloat((gdata.MDF_SB_Percentage * 100).toFixed(1).replace(/"/g, ''));

            loadingGrapgh();

        }


        // load The Model Parameter Graph Data
        var loadTheMPGraph = function (buId, versionId) {
            //YOY graph Data
            var param = {
                BusinessUnitID: buId,
                versionId: versionId
            }


            modelParameterFactory.getYOYGraphData(param).then(
                function onSuccess(response) {
                    var graphYOYData = response.data["Graphs"][0]["YOYGraph"];
                    if (!angular.isUndefined(response.data["Graphs"][0]["PartnerChangedGraph"])) {
                        $scope.MDFForYOYmin = parseFloat((response.data["Graphs"][0]["PartnerChangedGraph"][0].default_perc.toFixed(1).replace(/"/g, '')));
                        $scope.MDFForYOYmax = parseFloat((response.data["Graphs"][0]["PartnerChangedGraph"][0].final_input_perc.toFixed(1).replace(/"/g, '')));
                    }
                    if (!angular.isUndefined(response.data["Graphs"][0]["Partner_BU_ChangedGraph"])) {
                        $scope.MDFForBUYOYmin = parseFloat((response.data["Graphs"][0]["Partner_BU_ChangedGraph"][0].default_perc.toFixed(1).replace(/"/g, '')));
                        $scope.MDFForBUYOYmax = parseFloat((response.data["Graphs"][0]["Partner_BU_ChangedGraph"][0].final_input_perc.toFixed(1).replace(/"/g, '')));
                    }
                    // calling to load Graph
                    loadYOYGrapghData(graphYOYData);

                },
                function onError(response) {
                    //Notification.error({
                    //    message: response.data,
                    //    delay: null
                    //});

                }
            )


            modelParameterFactory.getGraphData(buId, $sessionStorage.currentVersion.VersionID).then(
                function onSuccess(response) {
                    var graphData = response.data;
                    // calling to load Graph
                    loadGrapghData(graphData);
                },
                function onError(response) {
                    //Notification.error({
                    //    message: response.data,
                    //    delay: null
                    //});
                }
            )

        }

        //default Graph Total Tab data on refresh
        loadTheMPGraph(0, $scope.versionId);

        var updateTheOutPutJson = function (jsonData) {

            angular.forEach(jsonData[0][0], function (value, key) {
                if (value !== null && typeof value === 'string') {
                    if (value.indexOf(',') > -1) {
                        jsonData[0][0][key] = parseFloat(value.replace(',', '').replace(/"/g, ''));
                    } else {
                        jsonData[0][0][key] = parseFloat(value.replace(/"/g, ''));
                    }
                }
            })

            var index = 0;
            angular.forEach(jsonData[1], function (row) {
                angular.forEach(row, function (value, key) {
                    if (value !== null && typeof value === 'string') {
                        if (value.indexOf(',') > -1) {
                            jsonData[1][index][key] = parseFloat(value.replace(',', '').replace(/"/g, ''));
                        } else {
                            jsonData[1][index][key] = parseFloat(value.replace(/"/g, ''));
                        }
                    }
                })
                index = index + 1;
            })
        }


        var ValidateTheOutPutJsonForNullValues = function (jsonData) {
            var nullIs = true;
            $scope.nullParamValues = [];
            
            angular.forEach(jsonData[0][0], function (value, key) {
                if (key != "UserID") {
                    if (value === null || value === '' || value.toString() === '-' || isNaN(budgetFactory.toFormat(value))) {
                        nullIs = false;
                        $scope.nullParamValues.push(key + '<br/>');
                    }
                }
            });

            var index = 0;
            angular.forEach(jsonData[1], function (row) {
                angular.forEach(row, function (value, key) {
                    if (value === null || value === '' || value.toString() === '-' || isNaN(budgetFactory.toFormat(value))) {
                        nullIs = false;
                        $scope.nullParamValues.push(key + '<br/>');
                    }
                });
                index = index + 1;
            });
            return nullIs;
        }

        var validateDCMSumForBus = function (jsonData) {
            $scope.isNotCorrectSB = true;
            $scope.isNotCorrectPG = true;
            $scope.isNotCorrectGrowth = true;
            angular.forEach(jsonData[1], function (row) {
                if (row.Dist_cust_membership_weight_Silver_and_Below > 100) {
                    $scope.isNotCorrectSB = false;
                }
                if (row.Dist_cust_membership_weight_Platinum_and_Gold > 100) {
                    $scope.isNotCorrectPG = false;
                }
                if (row.Growth_Revenue > 100) {
                    $scope.isNotCorrectGrowth = false;
                }
            });
            return $scope.isNotCorrectSB && $scope.isNotCorrectPG && $scope.isNotCorrectGrowth;
        }


        ////method to be called on click of apply button

        $scope.name = [];
        $scope.modelParamOnApply = function (action) {

            if (ValidateTheOutPutJsonForNullValues($scope.data) === false) {
                Notification.error('Please provide the data for missing field!\n' + $scope.nullParamValues + $rootScope.closeNotify);

            }
            else if (!validateDCMSumForBus($scope.data)) {
                if (!$scope.isNotCorrectSB) {
                    Notification.error({
                        message: ' Distributor customer membership weighting should not be more than 100% (Silver and Below) \n' + $scope.nullParamValues + $rootScope.closeNotify,
                        delay: null
                    });
                }
                if (!$scope.isNotCorrectPG) {
                    Notification.error({
                        message: ' Distributor customer membership weighting should not be more than 100% (Platinum and Gold) \n' + $scope.nullParamValues + $rootScope.closeNotify,
                        delay: null
                    });
                }
                if (!$scope.isNotCorrectGrowth) {
                    Notification.error({
                        message: ' Growth Weighting Factor should not be more than 100% \n' + $scope.nullParamValues + $rootScope.closeNotify,
                        delay: null
                    });
                }
            }
            else {
                // hit the api to check for warning
                modelParameterFactory.getWarningdata($scope.versionId)
            .then(
                 function success(resp) {
                     if (resp.data == true) {
                         bootbox.dialog({
                             title: "",
                             message: "Since parameters are modified, MDF allocations (Round 1 and Round 2) will be overwritten.  Do you want to continue?",
                             buttons: {
                                 cancel: {
                                     label: '<i class="fa fa-times"></i> Cancel'
                                 },
                                 confirm: {
                                     label: '<i class="fa fa-check"></i> Confirm',
                                     callback: function (result) {
                                         save(action);
                                     }
                                 }
                             }

                         })
                     } else {
                         // if no warning save directly
                         save(action);
                       
                     }
                 },
                function err(resp) {

                }
            )


                /*PERSIST THE DATA TO THE API*/

                function save(action) {
                    $scope.show = true;
                    jQuery('#mdf-loader').css('display', 'block');
                    //$scope.data[0][0].CountryID = $scope.countryId;
                    //$scope.data[0][0].DistrictID = parseInt($scope.DistrictID);
                    $scope.data[0][0].FinancialYearID = parseInt($scope.FinancialYearID);
                    $scope.data[0][0].PartnerTypeID = parseInt($scope.PartnerTypeID);
                    $scope.data[0][0].VersionID = parseInt($scope.versionId);
                    angular.copy($scope.data, $scope.name);
                    updateTheOutPutJson($scope.name);


                    angular.forEach($scope.name[1], function (value, key) {
                        if (($localStorage.user.BUs.indexOf($scope.name[1][key].BusinessUnitID) > -1)) {
                            $scope.name[1][key].isvalid = true;
                        }

                    });

                    angular.forEach($scope.name[1], function (value, key) {
                        $scope.name[1][key].ModifiedBy = null;
                        $scope.name[1][key].ModifiedDate = null;
                    })
                    $scope.name[0][0].ModifiedBy = null;
                    $scope.name[0][0].ModifiedDate = null;
                    var temp = {};
                    temp["Modelparameters"] = $scope.name[0][0];
                    temp["ModelBUparameters"] = $scope.name[1];
                    modelParameterFactory.updateUserModelValues(temp).then(
                    function onSuccess(response) {
                                            

                        
                        if (action != undefined) {
                            jQuery('#mdf-loader').css('display', 'none');
                            actionForRoute(action);
                        }
                        else {
                            $scope.show = false;

                            //updating the modal values   
                            modelParameterFactory.getData($scope.countryId, $scope.PartnerTypeID, $scope.DistrictID, $scope.FinancialYearID, $scope.versionId).then(
                                function onSuccess(response) {
                                    $scope.backUpModelParamData = [];
                                    $scope.data = response.data;
                                    updateTheReceivedData($scope.data);
                                    angular.copy($scope.data, $scope.backUpModelParamData);
                                    $timeout(function () {
                                        $(".specialText").priceFormat();
                                    }, 100)
                                    setupSlider();
                                },
                                function onError(response) {
                                    //Notification.error({
                                    //    message: response.data,
                                    //    delay: null
                                    //});
                                }
                            )
                            //load the graph values
                            //to be done
                            loadTheMPGraph($scope.currentBu.ID, $scope.versionId);

                            modelParameterFactory.getWarningdata($scope.versionId)
                                .then(
                                    function success(resp) {
                                        if (resp.data == true) {
                                            //Notification.warning("")
                                        }
                                    },
                                    function err(resp) {

                                    }
                              )
                            if (response.data !== 'Save Successfully') {
                                Notification.warning({
                                    message: "Sum of allocated MDF for partners is exceeded by the MDF allocated for the Business Unit(s) - " + response.data + '.' + $rootScope.closeNotify,
                                    delay: null
                                });
                            }
                            Notification.success({
                                message: "Model Parameter Saved Successfully!" + $rootScope.closeNotify,
                                delay: null
                            });
                            jQuery('#mdf-loader').css('display', 'none');
                        }
                        
                        //if (action != undefined)
                           
                        
                    },
                    function onError(response) {
                        jQuery('#mdf-loader').css('display', 'none');
                        //updating the modal values again from saved values in db  
                        modelParameterFactory.getData($scope.countryId, $scope.PartnerTypeID, $scope.DistrictID, $scope.FinancialYearID, $scope.versionId).then(
                            function onSuccess(response) {
                                $scope.backUpModelParamData = [];
                                $scope.data = response.data;
                                updateTheReceivedData($scope.data);
                                angular.copy($scope.data, $scope.backUpModelParamData);
                                $timeout(function () {
                                    $(".specialText").priceFormat();
                                }, 100)
                                setupSlider();
                            },
                            function onError(response) {
                                Notification.error({
                                    message: "Model Parameter loading values failed!" + $rootScope.closeNotify,
                                    delay: null
                                });
                            }
                        )
                        Notification.error({
                            message: "Model Parameter Could not be saved!" + $rootScope.closeNotify,
                            delay: null
                        });
                    })
                }
            }
        }

        //method to be called on click of Reset button
        $scope.resetModelParam = function () {
            bootbox.confirm({
                title: "",
                message: "Default values will be set for all parameters. Do you want to Continue?",
                buttons: {
                    cancel: {
                        label: '<i class="fa fa-times"></i> Cancel'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> Confirm'
                    }
                },
                callback: function (result) {
                    if (result) {

                        modelParameterFactory.getResetData($scope.versionId).then(
                            function onSuccess(response) {


                                var firstArr = response.data.Modelparameters;
                                var secondArr = response.data.ModelBUparameters;
                                var finalArr = [[firstArr], secondArr]
                                $scope.data = finalArr;
                                updateTheReceivedData(finalArr);
                                $timeout(function () {
                                    $(".specialText").priceFormat();
                                }, 100)

                                loadTheMPGraph($scope.currentBu.ID, $scope.versionId);

                                $scope.modelParamOnApply();
                                //modelParameterFactory.setDefaultModalAttributeValues($scope);
                                setupSlider();
                            },
                            function onError(response) {
                                //Notification.error({
                                //    message: response.data,
                                //    delay: null
                                //});
                            })

                    }
                }
            });

        }

        //enable the tab to be as per bu units
        $scope.enableTab = function (bu, index) {
            $scope.currentBuTabIndex = index;
            $scope.currentBu = bu;
            loadTheMPGraph(bu.ID, $scope.versionId);
        }

        //validating intergral input along with -ve sign
        $scope.validateInput = function (event) {

            if (event.which != 8 && event.which != 45 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault(); //stop character from entering input
            }
        }

        $scope.validateInput1 = function (event, id) {

            if (String.fromCharCode(event.which) === '-') {
                var elem = angular.element('#' + id);
                if (elem.val().slice(0, 1) !== '-') {
                    if (parseInt(elem.val().indexOf('-')) > -1) {
                        event.preventDefault();
                    }
                } else {
                    event.preventDefault();
                }
            }

            if (event.which != 8 && event.which != 45 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault(); //stop character from entering input
            }
        }
        var colorFunctionBaseLine = function () {
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

        var colorFunction = function () {
            return function (d, i) {
                if (i % 2 !== 0) {
                    return '#18b488'
                } else {
                    return '#333'
                }
            };
        }

        var colorFunctionYOY = function () {
            return function (d, i) {
                if (i % 2 !== 0) {
                    return '#333'
                } else {
                    return '#18b488'
                }
            };
        }

        var colorFunctionForMDFSellout = function () {
            return function (d, i) {
                return '#18b488'
            }
        }

        var loadingScatterGraph = function () {
            //graph 6 data

            $scope.sactteroptions = {
                chart: {
                    type: 'scatterChart',
                    height: 213,
                    margin: {
                        top: 40,
                        right: 30,
                        bottom: 40,
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
                }, title: {
                    enable: true
                        , text: 'YOY Growth Portfolio (Calculated)'
                        , css: {
                            fontSize: "14px",
                            fontWeight: "bold",
                            textAlign: "left",
                            width: "200px",
                            marginTop: "-16%",
                            paddingLeft: "6%"
                        }
                }
            };

            $scope.sactteroptions1 = {
                chart: {
                    type: 'scatterChart',
                    height: 400,
                    width: 600,
                    margin: {
                        top: 40,
                        right: 40,
                        bottom: 40,
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

            $scope.sactterdata = [{
                key: 'With MDF',
                values: $scope.MDF_Scatter_Array
            },
            {
                key: 'Without MDF',
                values: $scope.Without_MDF_Scatter_Array
            }];
        }


        var loadingGrapgh = function () {

            //graph 1 input

            $scope.options1 = {
                chart: {
                    type: 'multiBarChart',
                    height: 200,
                    width: 300,
                    margin: {
                        top: 30,
                        right: 60,
                        bottom: 60,
                        left: 40
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunction(),
                    valueFormat: function (d) {
                        return d3.format(',.2f')(d) + '%';
                    },
                    yDomain: [0, 100],
                    showValues: true,
                    showLegend: false,
                    yAxis: {
                        ticks: 5,
                        opacity: 0.1,
                        tickFormat: function (d) { return d3.format(',.1f')(d) + '%' }
                    }
                }
                , title: {
                    enable: true
                    , text: '% of MDF focused on growth'
                    , css: {
                        fontSize: "14px",
                        fontWeight: "bold",
                        textAlign: "left",
                        width: "200px",
                        marginTop: "-18%",
                        paddingLeft: "6%"
                    }
                }
            };

            $scope.data1 = [{
                "key": "Growth MDF",
                "values": [{
                    "x": "Default",
                    "y": $scope.DGrowth_MDF_Percentage
                }, {
                    "x": "Calculated",
                    "y": $scope.FGrowth_MDF_Percentage
                }]
            }, {
                "key": "Sustain MDF",
                "values": [{
                    "x": "Default",
                    "y": $scope.DSustain_MDF_Percentage
                }, {
                    "x": "Calculated",
                    "y": $scope.FSustain_MDF_Percentage
                }]
            }]


            $scope.options5 = {
                chart: {
                    type: 'multiBarChart',
                    height: 400,
                    width: 600,
                    margin: {
                        top: 50,
                        right: 60,
                        bottom: 45,
                        left: 45
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunction(),
                    valueFormat: function (d) {
                        return d3.format(',.2f')(d) + '%';
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

            //graph 2 input

            $scope.options2 = {
                chart: {
                    type: 'discreteBarChart',
                    height: 200,
                    width: 300,
                    margin: {
                        top: 20,
                        right: 60,
                        bottom: 50,
                        left: 40
                    },
                    x: function (d) { return d.label; },
                    y: function (d) { return d.value + (1e-10); },
                    showValues: true,
                    showXAxis: false,
                    valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    },
                    duration: 100,
                    color: colorFunction(),
                    yDomain: [$scope.min_of_array, $scope.max_of_array],
                    yAxis: {
                        ticks: 6,
                        tickFormat: function (d) { return d3.format(',.1f')(d) + '%' },
                        opacity: 0.1
                    }
                }
                , title: {
                    enable: true
                    , text: 'Sales growth of partner with MDF vs without MDF'
                    , css: {
                        fontSize: "14px",
                        fontWeight: "bold",
                        textAlign: "left",
                        width: "200px",
                        marginTop: "-18%",
                        paddingLeft: "6%"
                    }
                },
                caption: {
                    enable: true,
                    html: "Default &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Calculated",
                    css: {
                        position: "absolute",
                        top: "220px",
                        left: "111px",
                        width: "auto",
                        textAlign: "center"
                        // ,backgroundColor: "transparent",
                        //marginTop:"10px"
                    }
                }
            };

            $scope.data2 = [
                {
                    key: "Cumulative Return",
                    values: [
                            {
                                "label": "Default SalesGrowth With MDF",
                                "value": $scope.Default_SalesGrowth_with_mdf
                            },
                            {
                                "label": "Default SalesGrowth Without MDF",
                                "value": $scope.Default_SalesGrowth_without_mdf
                            },
                            {
                                "label": "Calculated SalesGrowth With MDF",
                                "value": $scope.FSalesGrowth_with_mdf
                            },
                            {
                                "label": "Calculated SalesGrowth Without MDF",
                                "value": $scope.FSalesGrowth_without_mdf
                            }
                    ]
                }
            ]


            $scope.options7 = {
                chart: {
                    type: 'discreteBarChart',
                    height: 400,
                    width: 600,
                    margin: {
                        top: 20,
                        right: 70,
                        bottom: 50,
                        left: 40
                    },
                    x: function (d) { return d.label; },
                    y: function (d) { return d.value + (1e-10); },
                    showValues: true,
                    showXAxis: false,
                    valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    },
                    duration: 100,
                    color: colorFunction(),
                    yDomain: [$scope.min_of_array, $scope.max_of_array],
                    yAxis: {
                        ticks: 6,
                        tickFormat: function (d) { return d3.format(',f')(d) + '%' },
                        opacity: 0.1
                    }
                },
                caption: {
                    enable: true,
                    html: "Default &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  Calculated",
                    css: {
                        marginTop: "-8%",
                    }
                }
            };



            //graph 3 input

            $scope.options3 = {
                chart: {
                    type: 'discreteBarChart',
                    height: 200,
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
                    valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    },
                    duration: 100,
                    color: colorFunctionForMDFSellout(),
                    //yDomain: [0, 20],
                    yAxis: {
                        ticks: 6,
                        tickFormat: function (d) { return d3.format(',.1f')(d) + '%' },
                        opacity: 0.1
                    }
                }
                , title: {
                    enable: true
                        , text: 'MDF / Sellout'
                        , css: {
                            fontSize: "14px",
                            fontWeight: "bold",
                            textAlign: "left",
                            width: "200px",
                            marginTop: "-12%",
                            paddingLeft: "6%"
                        }
                }
            };

            $scope.data3 = [
                {
                    key: "Cumulative Return",
                    values: [
                        {
                            "label": "Default",
                            "value": $scope.Default_MDFBYSellout_Percentage
                        },
                        {
                            "label": "Calculated",
                            "value": $scope.MDFBYSellout_Percentage
                        }
                    ]
                }
            ]

            $scope.options8 = {
                chart: {
                    type: 'discreteBarChart',
                    height: 400,
                    width: 600,
                    margin: {
                        top: 20,
                        right: 50,
                        bottom: 50,
                        left: 60
                    },
                    x: function (d) { return d.label; },
                    y: function (d) { return d.value + (1e-10); },
                    showValues: true,
                    valueFormat: function (d) {
                        return d3.format(',.1f')(d) + '%';
                    },
                    duration: 100,
                    color: colorFunctionForMDFSellout(),
                    //yDomain: [0, 20],
                    yAxis: {
                        ticks: 6,
                        tickFormat: function (d) { return d3.format(',f')(d) + '%' },
                        opacity: 0.1
                    }
                }
            };

            //graph 4 input

            $scope.options6 = {
                chart: {
                    type: 'multiBarChart',
                    height: 200,
                    width: 300,
                    margin: {
                        top: 50,
                        right: 50,
                        bottom: 45,
                        left: 45
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunction(),
                    valueFormat: function (d) {
                        return parseFloat(d).toFixed(1) + '%';
                    },
                    yDomain: [0, 100],
                    showValues: true,
                    showLegend: false,
                    yAxis: {
                        ticks: 5,
                        opacity: 0.1,
                        tickFormat: function (d) { return parseFloat(d).toFixed(1) + '%' }
                    }
                }, title: {
                    enable: true
                        , text: 'Alignment of Investment Change vs Sales Change'
                        , css: {
                            fontSize: "14px",
                            fontWeight: "bold",
                            textAlign: "left",
                            width: "200px",
                            marginTop: "-16%",
                            paddingLeft: "6%"
                        }
                }
            };

            $scope.data6 = [{
                "key": "Aligned",
                "values": [{
                    "x": "Default",
                    "y": $scope.DAligned_Percentage
                }, {
                    "x": "Calculated",
                    "y": $scope.FAligned_Percentage
                }]
            }, {
                "key": "Misaligned",
                "values": [{
                    "x": "Default",
                    "y": $scope.DMisAligned_Percentage
                }, {
                    "x": "Calculated",
                    "y": $scope.FMisAligned_Percentage
                }]
            }]

            $scope.options9 = {
                chart: {
                    type: 'multiBarChart',
                    height: 400,
                    width: 600,
                    margin: {
                        top: 50,
                        right: 50,
                        bottom: 45,
                        left: 45
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunction(),
                    valueFormat: function (d) {
                        return parseFloat(d).toFixed(1) + '%';
                    },
                    yDomain: [0, 100],
                    showValues: true,
                    showLegend: false,
                    yAxis: {
                        ticks: 5,
                        opacity: 0.1,
                        tickFormat: function (d) { return parseFloat(d).toFixed(1) + '%' }
                    }
                }
            };

            //graph 5 data

            $scope.options11 = {
                chart: {
                    type: 'multiBarChart',
                    height: 200,
                    width: 300,
                    margin: {
                        top: 50,
                        right: 50,
                        bottom: 45,
                        left: 45
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunctionBaseLine(),
                    valueFormat: function (d) {
                        return d3.format(',.2f')(d) + '%';
                    },
                    yDomain: [0, 100],
                    showValues: true,
                    showLegend: false,
                    yAxis: {
                        ticks: 5,
                        opacity: 0.1,
                        tickFormat: function (d) { return d3.format(',.1f')(d) + '%' }
                    }
                }, title: {
                    enable: true
                        , text: 'MDF by membership tier'
                        , css: {
                            fontSize: "14px",
                            fontWeight: "bold",
                            textAlign: "left",
                            width: "200px",
                            marginTop: "-16%",
                            paddingLeft: "6%"
                        }
                }
            };

            $scope.data11 = [{
                "key": "Platinum",
                "values": [{
                    "x": "Default",
                    "y": $scope.Default_MDF_Platinum_Percentage
                }, {
                    "x": "Calculated",
                    "y": $scope.Final_MDF_Platinum_Percentage
                }]
            }, {
                "key": "Gold",
                "values": [{
                    "x": "Default",
                    "y": $scope.Default_MDF_Gold_Percentage
                }, {
                    "x": "Calculated",
                    "y": $scope.Final_MDF_Gold_Percentage
                }]
            }, {
                "key": "Silver & Below",
                "values": [{
                    "x": "Default",
                    "y": $scope.Default_MDF_SB_Percentage
                }, {
                    "x": "Calculated",
                    "y": $scope.Final_MDF_SB_Percentage
                }]
            }]

            $scope.options10 = {
                chart: {
                    type: 'multiBarChart',
                    height: 400,
                    width: 600,
                    margin: {
                        top: 30,
                        right: 50,
                        bottom: 60,
                        left: 45
                    },
                    clipEdge: true,
                    duration: 500,
                    stacked: true,
                    color: colorFunctionBaseLine(),
                    valueFormat: function (d) {
                        return d3.format(',.2f')(d) + '%';
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
        }

        var actionForRoute = function (action) {
            if (action === 'next') {
                $rootScope.next = true;
                $state.go("budget.partner-budget")
            }
            else {
                //$rootScope.next = true;
                $rootScope.prev = true;
                $state.go("budget.historical-perfomance")
            }
        }

        $scope.validateJson = function (action) {
            if (JSON.stringify($scope.data) === JSON.stringify($scope.backUpModelParamData)) {
                actionForRoute(action);
            }
            else {
                bootbox.dialog({
                    title: "  ",
                    message: "<p style='margin-top: -30px !important;'>There are unsaved changes. Do you want to save the changes and navigate?</p>",
                    buttons: {
                        cancel: {
                            label: 'NO - Discard the changes and Navigate',
                            className: 'btn-default btn-notexttransform',
                            callback: function () {
                                actionForRoute(action);
                            }

                        },
                        confirm: {
                            label: 'YES - Save and Navigate',
                            className: 'btn-success btn-notexttransform',
                            callback: function () {
                                $scope.modelParamOnApply(action);
                            }
                        }

                    }
                });

                //bootbox.confirm({
                //    message: "There are unsaved changes. Do you want to save the changes and navigate?",
                //    buttons: {
                //        cancel: {
                //            label: 'NO - Discard the changes and Navigate',
                //            className: 'btn-default btn-notexttransform'
                //        },
                //        confirm: {
                //            label: 'YES - Save and Navigate',
                //            className: 'btn-success btn-notexttransform'
                //        }

                //    },
                //    callback: function (result) {
                //        if (result) {
                //            $scope.modelParamOnApply(action);
                //            //actionForRoute(action);
                //        } else {
                //            actionForRoute(action);
                //            //console.log("User clicked cancel")
                //        }

                //    }
                //});
            }
        }

        function setupSlider() {
            $("#storlekslider").slider({
                range: "max",
                min: 0,
                max: 100,
                value: $scope.data[0][0].Weights_applied_t_1H,
                slide: function (event, ui) {
                    $("#storlek_testet").val(ui.value);
                    $(ui.value).val($('#storlek_testet').val());
                    $scope.data[0][0].Weights_applied_t_1H = $('#storlek_testet').val();
                }
            });
            $("#storlek_testet").keyup(function () {
                $("#storlekslider").slider("value", $(this).val());
            });
            $("#storlekslider1").slider({
                range: "max",
                min: 0,
                max: 100,
                value: $scope.data[0][0].Weights_applied_t_2H,
                slide: function (event, ui) {
                    $("#storlek_testet1").val(ui.value);
                    $(ui.value).val($('#storlek_testet1').val());
                    $scope.data[0][0].Weights_applied_t_2H = $('#storlek_testet1').val();
                }
            });
            $("#storlek_testet1").keyup(function () {
                $("#storlekslider1").slider("value", $(this).val())
            });
            $("#storlekslider2").slider({
                range: "max",
                min: 0,
                max: 100,
                value: $scope.data[0][0].Weights_applied_t_3H,
                slide: function (event, ui) {
                    $("#storlek_testet2").val(ui.value);
                    $(ui.value).val($('#storlek_testet2').val());
                    $scope.data[0][0].Weights_applied_t_3H = $('#storlek_testet2').val();
                }
            });
            $("#storlek_testet2").keyup(function () {
                $("#storlekslider2").slider("value", $(this).val())
            });

            $("#storlekslider4").slider({
                range: "max",
                min: 80,
                max: 100,
                value: $scope.data[0][0].Target_accomplish_HighPrecision_Score,
                slide: function (event, ui) {
                    $("#storlek_testet4").val(ui.value);
                    $(ui.value).val($('#storlek_testet4').val());
                    $scope.data[0][0].Target_accomplish_HighPrecision_Score = $('#storlek_testet4').val();
                }
            });
            $("#storlek_testet4").keyup(function () {
                $("#storlekslider4").slider("value", $(this).val())
            });
            $("#storlekslider5").slider({
                range: "max",
                min: 50,
                max: 80,
                value: $scope.data[0][0].Target_accomplish_MediumPrecision_Score,
                slide: function (event, ui) {
                    $("#storlek_testet5").val(ui.value);
                    $(ui.value).val($('#storlek_testet5').val());
                    $scope.data[0][0].Target_accomplish_MediumPrecision_Score = $('#storlek_testet5').val();
                }
            });
            $("#storlek_testet5").keyup(function () {
                $("#storlekslider5").slider("value", $(this).val())
            });
            $("#storlekslider6").slider({
                range: "max",
                min: 80,
                max: 100,
                value: $scope.data[0][0].Max_Target_Accomplish_percentage,
                slide: function (event, ui) {
                    $("#storlek_testet6").val(ui.value);
                    $(ui.value).val($('#storlek_testet6').val());
                    $scope.data[0][0].Max_Target_Accomplish_percentage = $('#storlek_testet6').val();
                }
            });
            $("#storlek_testet6").keyup(function () {
                $("#storlekslider6").slider("value", $(this).val())
            });
            $("#storlekslider7").slider({
                range: "max",
                min: 50,
                max: 100,
                value: (($scope.data[0][0].Min_Target_Accomplish_percentage === null) ? ($scope.data[0][0].Min_Target_Accomplish_percentage = 60) : $scope.data[0][0].Min_Target_Accomplish_percentage),
                slide: function (event, ui) {
                    $("#storlek_testet7").val(ui.value);
                    $(ui.value).val($('#storlek_testet7').val());
                    $scope.data[0][0].Min_Target_Accomplish_percentage = $('#storlek_testet7').val();
                }
            });
            $("#storlek_testet7").keyup(function () {
                $("#storlekslider7").slider("value", $(this).val())
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
                    GeoID: $sessionStorage.resellerPartner.obj.geo.GeoID,
                    DistrictId: $sessionStorage.resellerPartner.district.DistrictID,
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

            $scope.invperformanceacc = function (vhighval) {
                if (vhighval <= 0)
                { $scope.data[0][0].WMGO_Perf_Acc_VeryLow = 0; }
                else {
                    $scope.data[0][0].WMGO_Perf_Acc_VeryLow = -vhighval;
                }
            }

            $scope.AssignPosition = function () {
                //HSM-413 start - stop cursor reposition to end after price formatting.
                var focusedId = "";
                focusedId = $(document.activeElement)[0].id;
                inputControls = [];
                var specialTextControls = $('#'+focusedId);
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
                //HSM-413 End
            }

            //$scope.AssignPosition = function (a) {
            //    //HSM-413 start - stop cursor reposition to end after price formatting.
            //    inputControls = [];
            //    var specialTextControls = $(".specialText");
            //    angular.copy(specialTextControls, inputControls);
            //    angular.forEach(specialTextControls, function (item, key) {
            //        inputControls[key].selectionStart = item.selectionStart;
            //        inputControls[key].selectionEnd = item.selectionEnd;
            //    })
            //    $(".specialText").priceFormat();
            //    var formattedControls = $(".specialText");
            //    for (var i = 0; i < inputControls.length; i++) {
            //        if (inputControls[i].value != "0") {
            //            console.log();
            //        }
            //        if (inputControls[i].value.trim().length < formattedControls[i].value.trim().length && inputControls[i].selectionStart > 1) {
            //            formattedControls[i].setSelectionRange(inputControls[i].selectionStart + 1, inputControls[i].selectionEnd + 1);
            //        }
            //        else if (inputControls[i].value.trim().length > formattedControls[i].value.trim().length &&
            //             (inputControls[i].selectionStart > 1 || (inputControls[i].selectionStart === 1 && inputControls[i].value.trim()[0] === "0"))) {
            //            formattedControls[i].setSelectionRange(inputControls[i].selectionStart - 1, inputControls[i].selectionEnd - 1);
            //        }
            //        else
            //            formattedControls[i].setSelectionRange(inputControls[i].selectionStart, inputControls[i].selectionEnd);
            //    };
            //    $(document.activeElement)[0].focus();
            //    //HSM-413 End
            //}

            $scope.invperformanceacchigh = function (highval) {
                if (highval <= 0)
                { $scope.data[0][0].WMGO_Perf_Acc_Low = 0; }
                else {
                    $scope.data[0][0].WMGO_Perf_Acc_Low = -highval;
                }
            }
        }

        $('#myModalcreate,#CopyModal').on('hidden.bs.modal', function () {

            $scope.drpdpwnvalue1 = null;
            $scope.DestPlanvalue = "";

        });

        $scope.updateGrowth = function (input,buId) {
            //var buData = _.filter(input, function (i) { return i.BusinessUnitID == buId });
            if (input.Sustain_Revenue > 100) {
                input.Growth_Revenue = 0;
            }
            else {
                input.Growth_Revenue = 100 - input.Sustain_Revenue;
            }

        }
    }

})();
