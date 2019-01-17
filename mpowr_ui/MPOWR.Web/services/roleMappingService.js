/*
Name: 		Role Mapping Service
Developer: 	Aamin Khan
Created at: 07 Apr 2017
*/

(function () {
    "use strict";
    angular.module("hpe").service("roleMappingService", roleMappingServiceFn);

    function roleMappingServiceFn($state,$http, $rootScope, $localStorage, $sessionStorage, $timeout, Notification) {
        //ngInject
        this.Feature = [
         {
             Route: 'budget.versioning',
             FeatureID: 8,
             FeatureName: 'MDF Plans Overview  ',
             Name: 'MDF Plans Overview',
             enabled: true
         },
           {
               Route: 'budget.allocation',
               FeatureID: 2,
               FeatureName: 'SetBuBudget',
               Name: 'Budget Settings'
           },
           {
               Route: 'budget.historical-perfomance',
               FeatureID: 3,
               FeatureName: 'Historical Performance',
               Name: 'Historical Performance'
           },
           {
               Route: 'budget.model-parameter',
               FeatureID: 4,
               FeatureName: 'ModelParameter',
               Name: 'Model Parameters'
           },
           {
               Route: 'budget.partner-budget',
               FeatureID: 5,
               FeatureName: 'PartnerBudget',
               Name: 'Partner Budget'
           },
           /*Duplicate route for full screen*/
           {
               Route: 'budget.partner-budget-expand',
               FeatureID: 5,
               FeatureName: 'PartnerBudget',
               Name: 'Partner Budget'
           },
           {
               Route: 'budget.partner-budget-round2',
               FeatureID: 5,
               FeatureName: 'PartnerBudget',
               Name: 'Partner Budget'
           },
           {
               Route: 'budget.final-summary',
               FeatureID: 6,
               FeatureName: 'PortfolioSummary',
               Name: 'Final Summary'
           }
        ];
        this.roles = {};
        this.Action = {
            1: "View",
            2: "Edit",
            3: "Export",
            4: "ExportExceptions",
            5: "ExportToSFDC",
            6: "ReviewComplete",
            7: "Submit",
            8: "Import",
            11: "Copy"
        }

        this.getView = function () {
            var whatIsView;
            _.each(this.Action, function (val, key) {
                if (val == 'View') {
                    whatIsView = key
                }
            });
            return parseInt(whatIsView)
        }
        this.getPlanningPeriods = function (financial) {
            var promise =
            $http({
                method: 'POST',
                url: $rootScope.api + 'Version/GetFinancialYear',
                data: financial
            }).then(function (result) {
                var FinancialyearList = result;
                if (FinancialyearList.data.filter(function (e) { return e.FinancialyearID == financial.FinancialyearID }).length == 0) {
                    $rootScope.disablePrevious = true;

                }
                else
                    $rootScope.disablePrevious = false;

            });
            return promise;

        }
        this.getCheckIsActiveFlag = function (versionId) {
            var promise =
            $http({
                method: 'GET',
                url: $rootScope.api + 'Version/CheckIsActiveFlag?VersionID=' + versionId,
            }).then(function (result) {
                var isActive = result.data;
                if (isActive == 1) {
                    $rootScope.previousPlan = false;
                    $rootScope.disablePrevious = false;
                }
                else if (isActive == -1) {
                    $rootScope.previousPlan = true;
                    $rootScope.disablePrevious = true;
                }
                else {
                    $rootScope.previousPlan = false;
                    $rootScope.disablePrevious = true;
                }
            });
            return promise;
        }
        this.getCheckIsActiveFlagByVersion = function (version) {
            var promise =
            $http({
                method: 'POST',
                url: $rootScope.api + 'Version/CheckIsActiveFlagByVersion',
                data: version
            }).then(function (result) {
                var isActive = result.data;
                if (isActive == true) {
                    $localStorage.NOACCESS = false;

                }
                else
                    $localStorage.NOACCESS = true;

            });
            return promise;
        }
        this.apply = function () {
            var curObj = this;
            /*$timeout(function () {
	            curObj.transpose();
	        },1000)*/
            var userContext = this.getFeatures();
            // userContext = _.filter(userContext, function (value) { return value.Action[0].IsFeatureActionChecked == true });
           
            var Allfeatures = this.Feature;
            var currentFeature = _.find(this.Feature, function (o) {
                return o.Route == $state.current.name
            })
            
            var currentContext = _.find(userContext, function (o) {
                return o.FeatureID == currentFeature.FeatureID;
            })

            //apply roles
            var actionList = currentContext.Action;
            
            var returnObj = {
                disabled: false,
                exportButton: false,
                exportSFDC: false,
                exporException: false,
                submit: false,
                importButton: false,
                copy:false
            };
            var index;
            
            for (var i = 0; i < actionList.length; i++) {
                var action = actionList[i];
                if ($state.current.name === 'budget.versioning') {
                    
                    this.val = action.FeatureActionType;
                    
                    this.roles[this.val.replace(/ /g, "")] = action.IsFeatureActionChecked;
                }else{
       
                if (action.IsFeatureActionChecked == false) {
                    // hide the element
                    var elementName = this.Action[action.FeatureActionTypeID];
                    //if ($state.current.name == 'budget.allocation' && elementName == "View") {

                    //}
                    if ($rootScope.next == true && elementName == "View") {
                        $rootScope.next = false;
                        

                        userContext = _.filter(userContext, function (value) { return value.FeatureID >= currentContext.FeatureID });

                        var keepGoing = true;
                        angular.forEach(userContext, function (value, key) {
                            angular.forEach(value.Action, function (action, key) {
                                if (keepGoing) {
                                    if (action.IsFeatureActionChecked == true) {
                                        currentContext = _.find(userContext, function (o) {
                                            return o.FeatureID == value.FeatureID;
                                        })
                                        var next = _.find(Allfeatures, function (o) {
                                            return o.FeatureID == value.FeatureID;
                                        })
                                        $state.go(next.Route)
                                        keepGoing = false;
                                    }
                                }

                            });
                        });

                    }

                    else if ($rootScope.prev == true && elementName == "View") {

                        $rootScope.prev = false;
                        //Notification.error("You don't have permission to access the page")
                        //$state.go("dashboard")
                        //var kopy = userContext;
                        //angular.forEach(kopy, function (value, key) {
                        //    if (value.FeatureID > currentContext.FeatureID) {
                        //        index = _.findIndex(userContext, function (val) { return val.FeatureID == value.FeatureID })
                        //        userContext.splice(index, 1);
                        //       // userContext.splice(key, 1);
                        //    }
                        //})

                        userContext = _.filter(userContext, function (value) { return value.FeatureID <= currentContext.FeatureID });
                        userContext.reverse();
                        var keepGoing = true;
                        var count = _.filter(userContext, function (value) { return value.Action[0].IsFeatureActionChecked == true }).length;
                        if (count == 0) {
                            keepGoing = false;
                            $state.go($rootScope.previousState.name);
                            return;
                        }


                        angular.forEach(userContext, function (value, key) {
                            angular.forEach(value.Action, function (action, key) {
                                if (keepGoing) {
                                    if (action.IsFeatureActionChecked == true) {
                                        currentContext = _.find(userContext, function (o) {
                                            return o.FeatureID == value.FeatureID;
                                        })
                                        var next = _.find(Allfeatures, function (o) {
                                            return o.FeatureID == value.FeatureID;
                                        })
                                        $state.go(next.Route)
                                        keepGoing = false;
                                    }
                                }

                            });
                        });


                    }

                    else if (elementName == "View") {
                        //history back and make it disbled in breadcrumb
                        var keepGoing = true;
                        angular.forEach(userContext, function (value, key) {
                            angular.forEach(value.Action, function (action, key) {
                                if (keepGoing) {
                                    if (action.IsFeatureActionChecked == true) {
                                        currentContext = _.find(userContext, function (o) {
                                            return o.FeatureID == value.FeatureID;
                                        })
                                        var next = _.find(Allfeatures, function (o) {
                                            return o.FeatureID == value.FeatureID;
                                        })
                                        $state.go(next.Route)
                                        keepGoing = false;
                                    }
                                }

                            });
                        });

                        //Notification.error("You don't have permission to access the page")
                        //$state.go("dashboard")
                    } else if (elementName == 'Edit') {
                        //disable all the inputs
                        /*$("input[type=text]").not($(".angucomplete-holder input")).prop("disabled", "disabled")
                        $("select").prop("disabled", "disabled");
                        $("textarea").prop("disabled", "disabled");
                        $("input[type=number]").prop("disabled", "disabled");
                        $("input[type=date]").prop("disabled", "disabled");
                        $("input[type=email]").prop("disabled", "disabled");
                        $("input[type=button]").prop("disabled", "disabled");
                        $(".edit-button").prop("disabled", "disabled");
                        $(".close-button").prop("disabled", "disabled");
                        $("#Submit").prop("disabled", "disabled");*/

                        returnObj.disabled = true;

                    } else if (elementName == "Export") {
                        //$(".export-button").prop("disabled", "disabled");
                        returnObj.exportButton = true;
                    } else if (elementName == "ExportToSFDC") {
                        //$(".export-sfdc").prop("disabled", "disabled");
                        returnObj.exportSFDC = true;
                    } else if (elementName == "ExportExceptions") {
                        //$(".export-exception").prop("disabled", "disabled");
                        returnObj.exporException = true;
                    } else if (elementName == "Submit") {
                        //$(".submit-button").prop("disabled", "disabled");
                        returnObj.submit = true;
                    } else if (elementName == "Import") {
                        //$(".submit-button").prop("disabled", "disabled");
                        returnObj.importButton = true;
                    }
                    else if (elementName == "Copy") {
                        //$(".submit-button").prop("disabled", "disabled");
                        returnObj.copy = true;
                    }
                }
                }
            }
            if ($state.current.name === 'budget.versioning') {
                returnObj = this.roles;
            }
            console.log("role ",returnObj);
            return returnObj;
        }

        this.getFeatures = function () {

            var arr = _.groupBy($localStorage.user.Features, "FeatureID")
            var features = [];
            _.each(arr, function (val, key) {
                var obj = {
                    FeatureID: parseInt(key) + 1,
                    Action: val
                }
                features.push(obj);
            })
            return features

        }
        this.transpose = function () {
            var userContext = this.getFeatures();


            var currentFeature = _.find(this.Feature, function (o) {
                return o.Route == $state.current.name
            })

            var currentContext = _.find(userContext, function (o) {
                return o.FeatureID == currentFeature.FeatureID;
            })
            //apply roles
            this.applyRoleOnElements(currentContext);

        },
        this.applyRoleOnElements = function (currentContext) {
            //loop thorugh and hide the feaure which is unchecked;
            var actionList = currentContext.Action;
            for (var i = 0; i < actionList.length; i++) {
                var action = actionList[i];
                if (action.IsFeatureActionChecked == false) {
                    // hide the element
                    var elementName = this.Action[action.FeatureActionTypeID];
                    if (elementName == "View") {
                        //history back and make it disbled in breadcrumb
                        Notification.error("You don't have permission to access the page")
                        $state.go("dashboard")
                    } else if (elementName == 'Edit') {
                        //disable all the inputs
                        $("input[type=text]").not($(".angucomplete-holder input")).prop("disabled", "disabled")
                        $("select").prop("disabled", "disabled");
                        $("textarea").prop("disabled", "disabled");
                        $("input[type=number]").prop("disabled", "disabled");
                        $("input[type=date]").prop("disabled", "disabled");
                        $("input[type=email]").prop("disabled", "disabled");
                        $("input[type=button]").prop("disabled", "disabled");
                        $(".edit-button").prop("disabled", "disabled");
                        $(".close-button").prop("disabled", "disabled");
                        $("#Submit").prop("disabled", "disabled");
                    } else if (elementName == "Export") {
                        $(".export-button").prop("disabled", "disabled");
                    } else if (elementName == "ExportToSFDC") {
                        $(".export-sfdc").prop("disabled", "disabled");
                    } else if (elementName == "ExportExceptions") {
                        $(".export-exception").prop("disabled", "disabled");
                    } else if (elementName == "Submit") {
                        $(".submit-button").prop("disabled", "disabled");
                    }

                }
            }
        },

        this.getMenu = function () {
            var curObj = this;
            var data = this.getFeatures();
            var menu = [];
            menu.push({
                url: 'budget.versioning',
                FeatureID: 1,
                name: 'MDF Plans Overview  ',
                active: false,
                enabled: true
            });
            for (var i = 0; i < data.length; i++) {

                var Action = data[i].Action;
                var viewPage = _.find(Action, function (x) {
                    return x.FeatureActionTypeID == curObj.getView();
                })



                var route = _.find(this.Feature, function (o) {
                    return o.FeatureID == data[i].FeatureID
                })

                try {
                    menu.push({
                        name: route.Name,
                        url: route.Route,
                        active: false,
                        enabled: viewPage.IsFeatureActionChecked
                    })
                } catch (e) {

                }


            }
            return menu;
        }

        this.nextMenu = function () {
            var curObj = this;
            var features = this.getFeatures();
            var index;
            for (var i = 0; i < features.length; i++) {
                var enabled = _.find(features[i].Action, function (param) {
                    return param.FeatureActionTypeID == curObj.getView() && param.IsFeatureActionChecked
                })
                if (typeof enabled != 'undefined') {
                    index = enabled
                    break
                }
            }

            var curMenu = _.find(this.Feature, function (o) {
                return o.FeatureID == index.FeatureID
            })

            return curMenu.Route;
        }



    }

})();