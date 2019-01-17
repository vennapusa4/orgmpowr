/*
Budget Controller
Created by: Aamin Khan
Created at: 05/02/2017
*/
(function () {

    angular.module("hpe").controller("budgetCtrl", bdg);

    function bdg($scope, $http, $rootScope, budgetFactory, modelParameterFactory, $timeout, Notification, $sessionStorage, authService, $localStorage, $state, roleMappingService, versioningservice, HistoricalFactory) {
        //ngInject
        var listener = $scope.$on('save', function (event, args) {
            $scope.save(args.menu);

        });
        var initialData = null;
        $scope.$sessionstorage = $sessionStorage;
        $rootScope.Saved = true;
        //HSM-413 - stop cursor reposition to end after price formatting.
        var inputControls = [];
        var startp = 0;
        var endp = 0;
        var Delay = 4000;
        var focusedId = "";
        HistoricalFactory.getBuByVersion();

        // apply the role management
        $scope.role = roleMappingService.apply();
        var financial = {
            FinancialyearID: $localStorage.user.FinancialYearID,

        }
        roleMappingService.getPlanningPeriods(financial).then(function onSuccess(response) {
            $scope.disablePrevious = $rootScope.disablePrevious;
            //if($rootScope.disablePrevious == false)
            //{
                roleMappingService.getCheckIsActiveFlag($localStorage.selectedversion.VersionID).then(function onSuccess(response) {
                    $scope.disablePrevious = $rootScope.disablePrevious;
                    if ($rootScope.disablePrevious == true && $rootScope.previousPlan == false) {
                        $localStorage.NOACCESS = true;
                        $scope.NOACCESS = $localStorage.NOACCESS;
                        Notification.error({ message: 'Plan is disabled due to Geo/Country/District/Business Unit has been made In-Active. Please Create a New Plan.', delay: Delay });
                    }
                    //else if ($rootScope.disablePrevious == true && $rootScope.previousPlan == true) {
                    //    $localStorage.NOACCESS = false;
                    //    $scope.NOACCESS = $localStorage.NOACCESS;
                    //}
                    else {
                        $localStorage.NOACCESS = false;
                        $scope.NOACCESS = $localStorage.NOACCESS;
                    }
                })
            //}
        })
        
        $scope.projectMDFTitle = function (e) {
            
            var color = e.currentTarget.style["border-color"];
            if (color == "red") {
                if (e.currentTarget.value=="0")
                    e.currentTarget.setAttribute("title", "Project MDF Should be greater than zero");
            }
            else e.currentTarget.removeAttribute("title");
        }
        $scope.projectNameTitle = function (e) {
            var color = e.currentTarget.style["border-color"];
            if (color == "red") {
                if (e.currentTarget.value.length > 0)
                    e.currentTarget.setAttribute("title", "Duplicate Project Name");
                else 
                    e.currentTarget.setAttribute("title", "Project name cannot be empty");
            }
            else e.currentTarget.removeAttribute("title");
        }
        $scope.baselineMDFTitle = function (index) {
            var index = 0;
            var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
            var elem = $("#txtClassName_" + index)[0];
            
            if (elem.style.borderColor == "red") {
                if (budgetObj.BaselineMDF < 0)
                    elem.setAttribute("title", "MDF should not be negative");
                else
                    elem.removeAttribute("title", "MDF should not be negative");
            }
            else elem.removeAttribute("title", "MDF should not be negative");
            //if (budgetObj.BaselineMDF < 0) {
                
            //}
            //budgetObj.baslinePer = budgetFactory.formatPercentage((budgetObj.BaselineMDF / budgetObj.TotalMDF) * 100);

            //var color = e.currentTarget.style["border-color"];
            //if (color == "red") {
            //    if (e.currentTarget.innerText.indexOf("-") == 0)
            //        e.currentTarget.setAttribute("title", "Baseline MDF should not be negative");
            //    else
            //        e.currentTarget.removeAttribute("title");
            //}
            //else
            //    e.currentTarget.removeAttribute("title");
        }
        $scope.NOACCESS = $localStorage.NOACCESS;
        //Declaring the function to load data from database
        if ($rootScope.versionData == undefined) {
            var financial = {
                FinancialyearID: $rootScope.verid

            }
        }
        else
            var financial = {
                FinancialyearID: $rootScope.versionData.VersionID

            }


        $scope.fillFinancialyearList = function (financial) {
            $scope.FinancialyearList = null;
            return $http({
                method: 'POST',
                url: $rootScope.api + 'BUBudgets/GetFinancialYear',
                data: $sessionStorage.fyParams
            }).success(function (result) {
                $scope.FinancialyearList = result;
                $scope.fillFinancialyearList = result;
                if ($sessionStorage.currentVersion != undefined) {

                    if ($localStorage.selectedversion != undefined) {
                        $scope.drpdpwnvalue = _.find($scope.fillFinancialyearList, function (o) {
                            return o.Financialyear == $localStorage.selectedversion.FinancialPeriod
                        })
                        $scope.versionList = $scope.drpdpwnvalue.Version;
                        $scope.versionSelected = _.find($scope.versionList, function (o) {
                            return o.VersionID == $localStorage.selectedversion.VersionID;
                        });
                    }
                    else {
                        $scope.drpdpwnvalue = _.find($scope.fillFinancialyearList, function (o) {
                            return o.Financialyear == $sessionStorage.currentVersion.FinancialPeriod
                        })
                        $scope.versionList = $scope.drpdpwnvalue.Version;
                        $scope.versionSelected = _.find($scope.versionList, function (o) {
                            return o.VersionID == $sessionStorage.currentVersion.VersionID
                        });
                    }

                    // console.log($scope.versionSelected);
                    budgetFactory.getBudgetOnSelect($scope, $scope.versionSelected.VersionID, $scope.drpdpwnvalue.FinancialyearID).then(function (response) {
                        var data = response.data;
                        $scope.BUObj = budgetFactory.generateJSON(data);
                        $scope.budgets = $scope.BUObj.BuUnits;
                        initialData = angular.copy($scope.budgets);

                        $scope.getTotal();
                        $timeout(function () {
                            // format the textbox into dolar comma seperated($111,222)
                            $(".specialText").priceFormat();
                            // on tab in select the text
                            budgetFactory.selectText();
                        }, 100)
                    })

                }
            });
        };

        $scope.showCountriesWithEllipses = function (countryList) {
            if (countryList.length > 0 && countryList[0].DisplayName !== '') {
                $scope.ctries = _.pluck(countryList, 'DisplayName').join(", ");
            }
        }

        $scope.fillFinancialyearListNew = function () {
            //console.log('parameters', $sessionStorage.fyParams);
            // $rootScope.versionData = version;
            $localStorage.selectedversion = $rootScope.versionData;
            $localStorage.selectedversion.FinancialPeriod = $rootScope.versionData.FinancialPeriod;
            $localStorage.selectedversion.VersionID = $rootScope.versionData.VersionID;

            $scope.FinancialyearList = null;
            return $http({
                method: 'POST',
                url: $rootScope.api + 'BUBudgets/GetFinancialYear',
                data: $sessionStorage.fyParams
            }).success(function (result) {
                $scope.fillFinancialyearList = result;
                $scope.FinancialyearList = result;
                $scope.drpdpwnvalue = _.find($scope.fillFinancialyearList, function (o) {
                    return o.Financialyear == $localStorage.selectedversion.FinancialPeriod
                })

                $scope.versionList = $scope.drpdpwnvalue.Version;
                $scope.versionSelected = _.find($scope.versionList, function (o) {
                    return o.VersionID == $localStorage.selectedversion.VersionID;
                });


                // console.log($scope.versionSelected);
                budgetFactory.getBudgetOnSelect($scope, $localStorage.selectedversion.VersionID, $rootScope.versionData.FinancialYearID).then(function (response) {
                    var data = response.data;
                    $scope.BUObj = budgetFactory.generateJSON(data);
                    $scope.budgets = $scope.BUObj.BuUnits;
                    initialData = angular.copy($scope.budgets);

                    $scope.getTotal();
                    $timeout(function () {
                        // format the textbox into dolar comma seperated($111,222)
                        $(".specialText").priceFormat();
                        // on tab in select the text
                        budgetFactory.selectText();
                    }, 100)
                })


            });
        };


        $scope.fillFinancialyearList(financial);

        $scope.gotoBudget = function (data) {
            $rootScope.versionData = data;
            $state.go('budget.allocation');
        }


        /*Get Data Budgets through Budget Factory
        budgetFactory.getBudget().then(
            function onSuccess(response) {
                // Handle success
                var data = response.data;
                $scope.BUObj = budgetFactory.generateJSON(data);
                $scope.budgets = $scope.BUObj.BuUnits;
                initialData = angular.copy($scope.budgets);

                $scope.getTotal();
                $timeout(function () {
                    // format the textbox into dolar comma seperated($111,222)
                    $(".specialText").priceFormat();
                    // on tab in select the text
                    budgetFactory.selectText();
                }, 100)
            },

              function onError(response) {
                  Notification.error({
                      message: response.data,
                      delay: null
                  });

              });
*/

        $scope.summaries = [];
        $scope.resellerPartner = $sessionStorage.resellerPartner;

        // format the textbox into dolar comma seperated($111,222)
        $timeout(function () {
            $(".specialText").priceFormat();
        }, 100)



        //remove a project

        $scope.remove = function (parentIndex, currentIndex) {
            // decorate the scope
            $scope.budgets[parentIndex].CarveProjects[currentIndex];
            $scope.budgets[parentIndex].CarveProjects[currentIndex].Flag = 'DL',
            $scope.budgets[parentIndex].CarveProjects[currentIndex].per = 0;

            //remove the dom
            $("#project-row-" + parentIndex + "-" + currentIndex).remove('*');
            $("#project-" + parentIndex + "-" + currentIndex).remove();
            $("#pmdf-" + parentIndex + "-" + currentIndex).remove();

            //if the record is newly added remove it
            if (typeof $scope.budgets[parentIndex].CarveProjects[currentIndex].ProgramMDFID == 'undefined') {
                $scope.budgets[parentIndex].CarveProjects.splice(currentIndex, 1)
            }
            $scope.calculateProjectPer(parentIndex, currentIndex, true);
            $scope.calculateBaselineMDF(currentIndex);
            $scope.getTotal();
        }

        // curve outs total callucation on change
        $scope.curveOutsCalc = function (index) {

            var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);

            if (budgetObj.ProgramMDF > (budgetObj.TotalMDF - (budgetObj.CountryReserveMDF + budgetObj.BaselineMDF))) {
                //if it exceds
                budgetObj.ProgramMDF = budgetObj.ProgramMDF.toString();
                budgetObj.ProgramMDF = parseInt(budgetObj.ProgramMDF.substring(0, budgetObj.ProgramMDF.length - 1));
            } else {
                //update the project budgets
                for (var i = 0; i < budgetObj.CarveProjects.length; i++) {
                    budgetObj.CarveProjects[i].per = budgetFactory.formatPercentage((budgetObj.CarveProjects[i].ProjectMDF / budgetObj.ProgramMDF) * 100);
                }
            }

            budgetObj.curvePer = budgetFactory.formatPercentage((budgetObj.ProgramMDF / budgetObj.TotalMDF) * 100);
            $scope.getTotal();
            $scope.getSummary();
        }

        // Country Reserve MDF changed 

        $scope.countryResCalc = function (index) {
            //HSM-413 start - stop cursor reposition to end after price formatting.
            inputControls = [];
            focusedId = $(document.activeElement)[0].id;
            if (focusedId != "" && focusedId != undefined) {
                var specialTextControls = $('#' + focusedId);
                angular.copy(specialTextControls, inputControls);
                angular.forEach(specialTextControls, function (item, key) {
                    startp = item.selectionStart;
                    endp = item.selectionEnd;
                })
            }
            //HSM-413 End
            var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
            if (isNaN(budgetObj.CountryReserveMDF)) {
                budgetObj.CountryReserveMDF = 0;
            }

            if (budgetObj.CountryReserveMDF > budgetObj.TotalMDF - budgetObj.ProgramMDF) {
                //Notification.warning({ message: 'Sum of Program MDF/Carve Outs, Country Reserve MDF and Baseline/Classic MDF should not exceed Total MDF allocated for the Business Unit' + $rootScope.closeNotify, delay: null });
                //budgetObj.CountryReserveMDF = budgetObj.CountryReserveMDF.toString();
                //var dummy = parseInt(budgetObj.CountryReserveMDF.substring(0, budgetObj.CountryReserveMDF.length - 1));
                //budgetObj.CountryReserveMDF = isNaN(dummy) ? 0 : dummy;
            }

            budgetObj.countryPer = budgetFactory.formatPercentage((budgetObj.CountryReserveMDF / budgetObj.TotalMDF) * 100);
            $scope.calculateBaselineMDF(index)
            $scope.getTotal();
        }

        // basline mdf calculate
        $scope.baselineMdfCalc = function (index) {
            var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
            budgetObj.baslinePer = budgetFactory.formatPercentage((budgetObj.BaselineMDF / budgetObj.TotalMDF) * 100);
        }

        $scope.setTotalBudget = function (index) {
            //HSM-413 start - stop cursor reposition to end after price formatting.
            inputControls = [];
            focusedId = $(document.activeElement)[0].id;
            if (focusedId != "" && focusedId != undefined) {
                var specialTextControls = $('#' + focusedId);
                angular.copy(specialTextControls, inputControls);
                angular.forEach(specialTextControls, function (item, key) {
                    startp = item.selectionStart;
                    endp = item.selectionEnd;
                })
            }
                //HSM-413 End
                $scope.calculateBaselineMDF(index);
                var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
                budgetObj.curvePer = budgetFactory.formatPercentage((budgetObj.ProgramMDF / budgetObj.TotalMDF) * 100);
                budgetObj.countryPer = budgetFactory.formatPercentage((budgetObj.CountryReserveMDF / budgetObj.TotalMDF) * 100);
                budgetObj.baslinePer = budgetFactory.formatPercentage((budgetObj.BaselineMDF / budgetObj.TotalMDF) * 100);

                $scope.getTotal();
            
            
        }

        $scope.calculateProjectPer = function (parentIndex, currentIndex, fromRemove) {
            //HSM-413 start - stop cursor reposition to end after price formatting.
            inputControls = [];
            focusedId = $(document.activeElement)[0].id;
            if (focusedId != "" && focusedId != undefined) {
                var specialTextControls = $('#' + focusedId);;
                angular.copy(specialTextControls, inputControls);
                angular.forEach(specialTextControls, function (item, key) {
                    startp = item.selectionStart;
                    endp = item.selectionEnd;
                })
            }
            //HSM-413 End
            var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[parentIndex]);
            var project = budgetObj.CarveProjects[currentIndex];

            if (typeof project != 'undefined') {
                if (parseInt(project.ProjectMDF) > 0) {
                    $("#pmdf-" + parentIndex + '-' + currentIndex).css("border-color", "#ddd");
                }
            }


            var total = 0;
            for (var i = 0; i < budgetObj.CarveProjects.length; i++) {

                if (budgetObj.CarveProjects[i].Flag != 'DL') {
                    var pmd = budgetFactory.toFormat(budgetObj.CarveProjects[i].ProjectMDF);
                    pmd = isNaN(pmd) ? 0 : pmd;
                    budgetObj.CarveProjects[i].ProjectMDF = typeof pmd == 'undefined' ? 0 : pmd;
                    total += budgetFactory.toFormat(budgetObj.CarveProjects[i].ProjectMDF);
                }
            }

            //calculate percentage
            budgetObj.ProgramMDF = total;


            if (budgetObj.ProgramMDF > budgetObj.TotalMDF - budgetObj.CountryReserveMDF) {

                //Notification.warning({ message: 'Program MDF should not be more than Total MDF allocated for the Business Unit' + $rootScope.closeNotify, delay: null });
                //project.ProjectMDF = budgetFactory.toFormat(project.ProjectMDF).toString();
                //project.ProjectMDF = parseInt(project.ProjectMDF.substring(0, project.ProjectMDF.length - 1));

                //var total = 0;
                //for (var i = 0; i < budgetObj.CarveProjects.length; i++) {
                //    if (budgetObj.CarveProjects[i].Flag != 'DL') {
                //        var pmd = budgetObj.CarveProjects[i].ProjectMDF;
                //        budgetObj.CarveProjects[i].ProjectMDF = typeof pmd == 'undefined' ? 0 : pmd;
                //        total += budgetFactory.toFormat(budgetObj.CarveProjects[i].ProjectMDF);
                //    }

                //}
                //budgetObj.ProgramMDF = total;
            }

            for (var i = 0; i < budgetObj.CarveProjects.length; i++) {
                if (budgetObj.CarveProjects[i].Flag != 'DL') {
                    budgetObj.CarveProjects[i].per = budgetFactory.formatPercentage((budgetFactory.toFormat(budgetObj.CarveProjects[i].ProjectMDF) / budgetObj.ProgramMDF) * 100);
                }
            }

            // sum up to program MDF

            budgetObj.curvePer = budgetFactory.formatPercentage((budgetObj.ProgramMDF / budgetObj.TotalMDF) * 100);

            if (!fromRemove) {
                if (typeof project.ProgramMDFID != 'undefined') {
                    project.Flag = 'UP';
                }
            }


            $scope.calculateBaselineMDF(parentIndex);
            $scope.getTotal();
        }

        $scope.getTotal = function () {
            if (typeof $scope.budgets != 'undefined') {
                $scope.MDFGrossTotal = 0;
                $scope.pmcGrossTotal = 0;
                $scope.crmGrossTotal = 0;
                $scope.bcmGrossTotal = 0;
                for (var i = 0; i < $scope.budgets.length; i++) {
                    $scope.MDFGrossTotal += budgetFactory.toFormat($scope.budgets[i].TotalMDF);
                    $scope.pmcGrossTotal += budgetFactory.toFormat($scope.budgets[i].ProgramMDF);
                    $scope.crmGrossTotal += budgetFactory.toFormat($scope.budgets[i].CountryReserveMDF);
                    $scope.bcmGrossTotal += budgetFactory.toFormat($scope.budgets[i].BaselineMDF);
                }
                $scope.MDFGrossTotal = isNaN($scope.MDFGrossTotal) ? 0 : $scope.MDFGrossTotal;
                $scope.pmcGrossTotal = isNaN($scope.pmcGrossTotal) ? 0 : $scope.pmcGrossTotal;
                $scope.crmGrossTotal = isNaN($scope.crmGrossTotal) ? 0 : $scope.crmGrossTotal;
                $scope.bcmGrossTotal = isNaN($scope.bcmGrossTotal) ? 0 : $scope.bcmGrossTotal;
                //pecentages
                $scope.pmcPer = budgetFactory.formatPercentage(($scope.pmcGrossTotal / $scope.MDFGrossTotal) * 100);
                $scope.crmPer = budgetFactory.formatPercentage(($scope.crmGrossTotal / $scope.MDFGrossTotal) * 100);
                $scope.bcmPer = budgetFactory.formatPercentage(($scope.bcmGrossTotal / $scope.MDFGrossTotal) * 100);

                $scope.mdfTotalPer = budgetFactory.formatPercentage((($scope.pmcGrossTotal + $scope.crmGrossTotal + $scope.bcmGrossTotal) / $scope.MDFGrossTotal) * 100)
                $scope.getSummary();
            }

        };


        $scope.getSummary = function () {
            var newArr = [];
            for (var i = 0; i < $scope.budgets.length; i++) {
                newArr = newArr.concat($scope.budgets[i].CarveProjects)
            }
            var groupedData = _.groupBy(newArr, function (d) { return d.ProjectName });

            //now sum it
            var sumary = [];
            $.each(groupedData, function (k, v) {
                var ox = {};
                var total = 0;
                var flag = 'DF';
                for (var i = 0; i < v.length; i++) {
                    total += budgetFactory.toFormat(v[i].ProjectMDF);
                    flag = v[i].Flag;
                }
                ox.ProjectName = k;
                ox.ProjectMDF = total;
                ox.per = budgetFactory.formatPercentage((total / $scope.pmcGrossTotal) * 100);
                if (flag != 'DL') {
                    sumary.push(ox);
                }
            });
            $scope.summaries = sumary;
            $timeout(function () {
                //HSM-413 start - stop cursor reposition to end after price formatting.
                //focusedId = $(document.activeElement)[0].id;
                focusedId = $(document.activeElement)[0].id;
                if (!focusedId.match(/project-.*/) && focusedId !="" && typeof focusedId != "undefined") {
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
                }

                //for (var i = 0; i < inputControls.length; i++) {
                //    if (inputControls[i].value != "0") {
                //        console.log();
                //    }
                //    if (inputControls[i].value.trim().length < formattedControls[i].value.trim().length && inputControls[i].selectionStart > 1) {
                //        formattedControls[i].setSelectionRange(inputControls[i].selectionStart + 1, inputControls[i].selectionEnd + 1);
                //    }
                //    else if (inputControls[i].value.trim().length > formattedControls[i].value.trim().length &&
                //         (inputControls[i].selectionStart > 1 || (inputControls[i].selectionStart === 1 && inputControls[i].value.trim()[0] === "0"))) {
                //        formattedControls[i].setSelectionRange(inputControls[i].selectionStart - 1, inputControls[i].selectionEnd - 1);
                //    }
                //    else
                //        formattedControls[i].setSelectionRange(inputControls[i].selectionStart, inputControls[i].selectionEnd);
                //};
                //HSM-413 End
                //HPEM-623 Start
                $rootScope.Saved = angular.equals(initialData, $scope.budgets);
                //HPEM-623 End
            }, 10)
        }

        // expand bu
        $scope.toggl = false;
        $scope.expand = function (index, id) {

            var elem = $("#" + id + index);
            if (id == 'total_') {
                if (elem.attr("class").indexOf('fa-chevron-down') > -1) {
                    elem.removeClass('fa-chevron-down').addClass('fa-chevron-up');
                    $("#totalData").show();
                } else {
                    elem.removeClass('fa-chevron-up').addClass('fa-chevron-down')
                    $("#totalData").hide();
                }
            } else {
                if (elem.attr("class").indexOf('fa-chevron-down') > -1) {
                    elem.removeClass('fa-chevron-down').addClass('fa-chevron-up');
                    $("#budget-body-" + index).show();
                } else {
                    elem.removeClass('fa-chevron-up').addClass('fa-chevron-down')
                    $("#budget-body-" + index).hide();
                }
            }

        }

        // add new project
        $scope.add = function (index) {

            //if ($scope.budgets[index].TotalMDF == 0) {
            //    Notification.error({ message: "Please enter Total MDF Budget to add Program MDF" + $rootScope.closeNotify, delay: null });
            //    return
            //}

            //for (var i = 0; i < $scope.budgets[index].CarveProjects.length; i++) {
            //    if ($scope.budgets[index].CarveProjects[i].ProjectName.length == 0) {
            //        Notification.error({ message: "Please fill the project name" + $rootScope.closeNotify, delay: null });
            //        return
            //    }
            //}

            //new code - added on top
            $scope.budgets[index].CarveProjects.splice(0, 0, {
                ProjectName: '',
                ProjectMDF: '0',
                per: '0',
                Flag: 'IN'
            });

            var elem = $("#" + 'exp_' + index);
            elem.removeClass('fa-chevron-down').addClass('fa-chevron-up');
            $("#budget-body-" + index).show();
            $timeout(function () {
                $(".specialText").priceFormat();
                budgetFactory.disablePaste();
            }, 100);

            $scope.calculateBaselineMDF();
        };


        // Through the data to the api layer
        $scope.save = function (action) {
            console.log(action);
            if (!validate()) { // first validate
                // handle if  any event is required
            } else {
                //save it
                delete $scope.BUObj.BuUnits;
                $scope.BUObj.BuUnits = $scope.budgets;

                angular.forEach($scope.BUObj.BuUnits, function (value, key) {
                    if (($localStorage.user.BUs.indexOf($scope.BUObj.BuUnits[key].BusinessUnitID) > -1)) {
                        $scope.BUObj.BuUnits[key].isvalid = true;
                    }

                });
                $scope.BUObj.VersionID = $sessionStorage.currentVersion.VersionID;
                $scope.BUObj.UserName = $localStorage.user.UserName;
                $scope.BUObj.Status = "Draft";
                $scope.BUObj.UserID = $localStorage.user.UserID;
                $scope.BUObj.CountryID = $sessionStorage.resellerPartner.obj.id;
                $scope.BUObj.PartnerTypeID = $sessionStorage.resellerPartner.partner.PartnerTypeID;
                $scope.BUObj.DistrictID = $sessionStorage.resellerPartner.district.DistrictID;
                $scope.BUObj.MembershipGroupId = $sessionStorage.resellerPartner.membership.MembershipGroupID;
                $('#mdf-loader').css('display', 'block');


                budgetFactory.persist($scope.BUObj).then(
                        function onSuccess(response) {
                            if (action === undefined) {
                                budgetFactory.getBudget().then(success, error)
                                function success(resp) {
                                    $scope.BUObj = budgetFactory.generateJSON(resp.data);
                                    $scope.budgets = $scope.BUObj.BuUnits;
                                    initialData = angular.copy($scope.budgets)
                                    $rootScope.Saved = true;
                                    Notification.success({
                                        message: "Budget Saved successfully!" + $rootScope.closeNotify,
                                        delay: Delay
                                    });
                                    $('#mdf-loader').css('display', 'none');
                                }
                                function error() {

                                }
                            }
                            else {//action == next
                                $scope.$on('$destroy', function () {
                                    //console.log("Unregistering listener");
                                    listener();
                                });
                                if (action == 'next') {
                                    var bool = true;
                                    $('#mdf-loader').css('display', 'block');
                                    //budgetFactory.getModelParam();
                                    $http({
                                        method: 'GET',
                                        url: $rootScope.api + 'ModelParameter/GetModelParameterDefault?VersionID=' + $localStorage.selectedversion.VersionID + '&UserID=' + $localStorage.user.UserID + '&frmStep1=' + bool
                                    }).then(function (resp) {
                                        $('#mdf-loader').css('display', 'none');
                                        $state.go("budget.partner-budget");
                                    });
                                }
                                else {
                                    if (action == 'budget.partner-budget') {
                                        var bool = true;
                                        $('#mdf-loader').css('display', 'block');
                                        //budgetFactory.getModelParam();
                                        $http({
                                            method: 'GET',
                                            url: $rootScope.api + 'ModelParameter/GetModelParameterDefault?VersionID=' + $localStorage.selectedversion.VersionID + '&UserID=' + $localStorage.user.UserID + '&frmStep1=' + bool
                                        }).then(function (resp) {
                                            $('#mdf-loader').css('display', 'none');
                                            $state.go("budget.partner-budget");
                                        });
                                    }
                                    else {
                                        //Notification.success({
                                        //    message: "Budget Saved successfully!" + $rootScope.closeNotify,
                                        //    delay: Delay
                                        //});
                                        $('#mdf-loader').css('display', 'none');
                                        $state.go(action);
                                    }
                                    
                                }
                                

                                //$state.go("budget.partner-budget");
                            }

                        },

                      function onError(response) {
                          Notification.error({
                              message: "Could not save the data!",
                              delay: Delay
                          });

                      }
                )
            }
        }


        // pre save validation

        function validate() {
            var ret = false;

            var flag = false;
            var projectFlag = false;
            var mdfFlag = false;
            var carveoutsProjectnameFlag = [];
            var carveoutsProjectMDFFlag = [];
            $(".tmdf-per").each(function (key, val) { // Total MDF percentage
                if ((parseInt($(this).val()) != 100 && parseInt($(this).val()) != 0) || parseInt($(this).val()) > 100) {
                    $(this).css("border-color", "red");
                    $(this)[0].setAttribute('title', 'Total MDF Percentage should be 100%');
                    $(this).next().css("border-color", "red");
                    flag = true;
                } else {
                    $(this).css("border-color", "#ddd");
                    $(this).next().css("border-color", "#ddd");
                }

            })

            $(".project-inp").each(function (key, val) {// Project Name
                var elem = $(this);
                if (elem[0].parentElement != undefined && elem[0].parentElement.parentElement != undefined && elem[0].parentElement.parentElement.className != undefined && elem[0].parentElement.parentElement.className.indexOf("ng-hide") < 0) {
                    if (elem.val().length == 0) {
                        elem.css("border-color", "red");
                        elem.next().css("border-color", "red");
                        budgetFactory.expandRowOnValidationFailure(elem);
                        projectFlag = true;
                        carveoutsProjectnameFlag.push(true);
                    } else {
                        elem.css("border-color", "#ddd");
                        elem.next().css("border-color", "#ddd");
                        carveoutsProjectnameFlag.push(false);
                    }
                }
                

            })

            $(".project-mdf").each(function (key, val) { // project amount
                var elem = $(this);
                if (elem[0].parentElement != undefined && elem[0].parentElement.parentElement != undefined && elem[0].parentElement.parentElement.parentElement != undefined && elem[0].parentElement.parentElement.parentElement.className != undefined
                    && elem[0].parentElement.parentElement.parentElement.className.indexOf("ng-hide") < 0) {
                    if (elem.val() == 0) {
                        elem.css("border-color", "red");
                        elem.next().css("border-color", "red");
                        budgetFactory.expandRowOnValidationFailure(elem);
                        mdfFlag = true;
                        carveoutsProjectMDFFlag.push(true);
                    } else {
                        elem.css("border-color", "#ddd");
                        elem.next().css("border-color", "#ddd");
                        carveoutsProjectMDFFlag.push(false);
                    }
                }         
            })



            if (flag || projectFlag || mdfFlag) {

                if (mdfFlag && projectFlag) {
                    var breakflag = 0;
                    //$scope.budgets[i].CarveProjects.length


                    for (var i = 0; i <= 4; i++) {

                        if ($scope.budgets[i].CarveProjects.length != 0) {
                            if ((($scope.budgets[i].CarveProjects[0].ProjectName.length == 0) &&
                                ($scope.budgets[i].CarveProjects[0].ProjectMDF != 0)) ||
                                (($scope.budgets[i].CarveProjects[0].ProjectName.length != 0) &&
                                ($scope.budgets[i].CarveProjects[0].ProjectMDF == 0)) 
                                || (($scope.budgets[i].CarveProjects[0].ProjectName.length == 0) && 
                                ($scope.budgets[i].CarveProjects[0].ProjectMDF == 0))
                                ) {
                                breakflag = 1;
                                break;
                            }

                            //if ($scope.budgets[i].CarveProjects[0].ProjectName.length == 0 && $scope.budgets[i].CarveProjects[0].ProjectMDF == 0) {



                            //    $scope.budgets[i].CarveProjects.splice(0, 1);


                            //}
                        }

                    }
                    for (var s = 0; s < $scope.summaries.length; s++) {
                        if ($scope.summaries[s].ProjectMDF == 0 && $scope.summaries[s].ProjectName == "") {
                            $scope.summaries.splice(s, 1);
                        }
                    }
                    //if (breakflag == 1) {
                    //    if (projectFlag) {
                    //        Notification.error({ message: 'Project name cannot be empty' + $rootScope.closeNotify, delay: null });
                    //    } if (flag) {
                    //    }
                    //    if (mdfFlag) {
                    //        Notification.error({ message: 'Project MDF Should be greater than zero' + $rootScope.closeNotify, delay: null });
                    //    }
                    //    ret = false;
                    //}
                    //else
                    //    ret = true;
                }
                //else {
                //    if (projectFlag) {
                //        Notification.error({ message: 'Project name cannot be empty' + $rootScope.closeNotify, delay: null });
                //    } if (flag) {
                //    }
                //    if (mdfFlag) {
                //        Notification.error({ message: 'Project MDF Should be greater than zero' + $rootScope.closeNotify, delay: null });
                //    }

                //}
            }
            else {
                ret = true;
            }
            if (!checkBudget()) {
                ret = false;
            }


            return ret;
        }
        function checkCarveouts(Errormessage) {
            var flag = false;
            for (var i = 0; i < $scope.budgets.length; i++) {

                if ($scope.budgets[i].CarveProjects.length != 0) {
                    for (var j = 0; j < $scope.budgets[i].CarveProjects.length; j++) {
                        var current = $scope.budgets[i].CarveProjects[j].ProjectName;
                        //if (current.length > 0) {
                        //    $("#project-" + i + '-' + j).css("border-color", "#ddd");
                        //}
                        
                            
                        
                            
                        if (j != 0 && $scope.budgets[i].CarveProjects[j].Flag != 'DL') {

                            for (var k = j-1 ; k >= 0; k--) {
                                    if($scope.budgets[i].CarveProjects[j].ProjectName == $scope.budgets[i].CarveProjects[k].ProjectName)
                                    {
                                        $("#project-" + i + '-' + j).css("border-color", "red");
                                        budgetFactory.expandOnSummaryValidationFailure(i);
                                        if (Errormessage != undefined)
                                            Errormessage = Errormessage + '<br/>-Duplicate Project name in business unit ' + $scope.budgets[i].BusinessUnitName;
                                        else
                                            Errormessage = '-Duplicate Project name in business unit ' + $scope.budgets[i].BusinessUnitName;
                                    }
                                }
                            }
                            
                        
                        if ($scope.budgets[i].CarveProjects[j].Flag != 'DL') {
                            if ($scope.budgets[i].CarveProjects[j].ProjectName.length == 0 && $scope.budgets[i].CarveProjects[j].ProjectMDF != 0) {
                                if (Errormessage != undefined)
                                    Errormessage = Errormessage + '<br/>-Project name cannot be empty in business unit ' + $scope.budgets[i].BusinessUnitName;
                                else
                                    Errormessage = '-Project name cannot be empty in business unit ' + $scope.budgets[i].BusinessUnitName;
                            }

                            else if ($scope.budgets[i].CarveProjects[j].ProjectMDF == 0 && $scope.budgets[i].CarveProjects[j].ProjectName.length != 0) {
                                if (Errormessage != undefined)
                                    Errormessage = Errormessage + '<br/>-Project MDF Should be greater than zero in business unit ' + $scope.budgets[i].BusinessUnitName + ' for Project Name ' + $scope.budgets[i].CarveProjects[j].ProjectName;
                                else
                                    Errormessage = '-Project MDF Should be greater than zero in business unit ' + $scope.budgets[i].BusinessUnitName + ' for Project Name ' + $scope.budgets[i].CarveProjects[j].ProjectName;
                            }

                            else if ($scope.budgets[i].CarveProjects[j].ProjectMDF == 0 && $scope.budgets[i].CarveProjects[j].ProjectName.length == 0) {
                                if (Errormessage != undefined)
                                    Errormessage = Errormessage + '<br/>-Project name cannot be empty and Project MDF Should be greater than zero in business unit ' + $scope.budgets[i].BusinessUnitName;
                                else
                                    Errormessage = '-Project name cannot be empty and Project MDF Should be greater than zero in business unit ' + $scope.budgets[i].BusinessUnitName;
                            }
                        }
                        
                    }
                    
                    //if ((($scope.budgets[i].CarveProjects[0].ProjectName.length == 0) &&
                    //    ($scope.budgets[i].CarveProjects[0].ProjectMDF != 0)) ||
                    //    (($scope.budgets[i].CarveProjects[0].ProjectName.length != 0) &&
                    //    ($scope.budgets[i].CarveProjects[0].ProjectMDF == 0)) ||
                    //    (($scope.budgets[i].CarveProjects[0].ProjectName.length == 0) &&
                    //    ($scope.budgets[i].CarveProjects[0].ProjectMDF == 0))
                    //    ) {
                    //    breakflag = 1;
                        
                    //}
                    
                }
            }
            return Errormessage;
        }

        function checkBudget() {
            var flag = false;
            var Errormessage;
            for (var i = 0; i < $scope.budgets.length; i++) {
                var total = budgetFactory.toFormat($scope.budgets[i].BaselineMDF) + budgetFactory.toFormat($scope.budgets[i].CountryReserveMDF) + budgetFactory.toFormat($scope.budgets[i].ProgramMDF);
                var value = $scope.budgets[i].TotalMDF;

                if (value !== null && typeof value === 'string') {
                    value = value.replace(/\,/g, ''); // 1125, but a string, so convert it to number
                    value = parseInt(value, 10);
                }
                $scope.budgets[i].TotalMDF = value;
                if (total != $scope.budgets[i].TotalMDF) {
                    $("#budget-head-" + i + ' .pmcTotal').css("border-color", "red");
                    $("#budget-head-" + i + ' .mdfTotal').css("border-color", "red");
                    $("#budget-head-" + i + ' .crmTotal').css("border-color", "red");
                    $("#budget-head-" + i + ' .bcmTotal').css("border-color", "red");
                    if (Errormessage === undefined) {
                        Errormessage = '-Sum of Program MDF/Carve Outs, Country Reserve MDF and Baseline/Classic MDF should be equal to  Total MDF allocated for the Business Unit.';
                    }
                    else {
                        Errormessage = Errormessage + '<br/>-Sum of Program MDF/Carve Outs, Country Reserve MDF and Baseline/Classic MDF should be equal to  Total MDF allocated for the Business Unit.';
                    }
                    
                    //Notification.error({ message: 'Sum of Program MDF/Carve Outs, Country Reserve MDF and Baseline/Classic MDF should be equal to  Total MDF allocated for the Business Unit.', delay: null });
                    //return false;
                } else if (budgetFactory.toFormat($scope.budgets[i].BaselineMDF) < 0) {
                    $("#budget-head-" + i + ' .bcmTotal').css("border-color", "red");
                    if (Errormessage === undefined) {
                        Errormessage = '-Business Unit "' + $scope.budgets[i].BusinessUnitName + '" MDF should not be negative';
                    }
                    else {
                        Errormessage = Errormessage + '<br/>-Business Unit "' + $scope.budgets[i].BusinessUnitName + '" MDF should not be negative';
                    }
                    //Notification.error({ message: 'Business Unit "' + $scope.budgets[i].BusinessUnitName + '" Baseline MDF should not be negative' + $rootScope.closeNotify, delay: null });
                    //return false;
                    //Errormessage = checkCarveouts(Errormessage);
                }
                else {
                    $("#budget-head-" + i + ' .pmcTotal').css("border-color", "#ddd");
                    $("#budget-head-" + i + ' .mdfTotal').css("border-color", "#ddd");
                    $("#budget-head-" + i + ' .crmTotal').css("border-color", "#ddd");
                    $("#budget-head-" + i + ' .bcmTotal').css("border-color", "#ddd");
                    //Errormessage = checkCarveouts(Errormessage);
                    flag = true;
                }
                
            }
            Errormessage = checkCarveouts(Errormessage);
            if (Errormessage === undefined) {
                return flag;
            }
            else {
                //Notification.error({ message: Errormessage + $rootScope.closeNotify, delay: null });
                Notification.error({ message: 'Please correct the highlighted fields' + $rootScope.closeNotify, delay: Delay });
                return false;
            }
        }

        // check duplicate for MDF Project Name
        $scope.checkDuplicate = function (parent, index) {
            var current = $scope.budgets[parent].CarveProjects[index].ProjectName;
            if (current.length > 0) {
                $("#project-" + parent + '-' + index).css("border-color", "#ddd");
            }
            //var flag = false;
            //for (var i = 0; i < $scope.budgets[parent].CarveProjects.length; i++) {

            //    if (i == index) {
            //        continue
            //    }
            //    if ($scope.budgets[parent].CarveProjects[i].ProjectName == current && $scope.budgets[parent].CarveProjects[i].Flag != 'DL') {
            //        flag = true;
            //        //$scope.budgets[parent].CarveProjects[index].ProjectName =
            //        //    $scope.budgets[parent].CarveProjects[index].ProjectName
            //        //        .substring(0, $scope.budgets[parent].CarveProjects[index].ProjectName.length - 1)
            //        break;
            //    }
            //}
            //if (flag == true) {
            //    Notification.error({
            //        message: "Duplicate Project name." + $rootScope.closeNotify,
            //        delay: null
            //    });
            //}

            var project = $scope.budgets[parent].CarveProjects[index];

            if (typeof project.ProgramMDFID != 'undefined') {
                project.Flag = 'UP';
            }
            $scope.calculateBaselineMDF(index);
            $scope.getTotal()

        }

        // this function allows to format percentage from view
        $scope.formatPercentage = function (data) {

            return budgetFactory.formatPercentage(data);
        }

        $scope.isN = function (data) {
            data = data.toString();
            data = data.replace(/,/g, '')
            return parseInt(data);
        }


        $scope.toFormat = function (val) {
            return budgetFactory.toFormat(val)
        }

        $scope.calculateBaselineMDF = function (index) {
            var obj = $scope.budgets[index];
            if (typeof obj != 'undefined') {
                var base = budgetFactory.toFormat(obj.TotalMDF) - (budgetFactory.toFormat(obj.ProgramMDF) + budgetFactory.toFormat(obj.CountryReserveMDF));
                obj.BaselineMDF = isNaN(base) ? 0 : base;
                var bPer = budgetFactory.formatPercentage((obj.BaselineMDF / obj.TotalMDF) * 100);
                obj.baslinePer = isNaN(bPer) ? 0 : bPer;
            }

        }

        $scope.checkProgramMDF = function () {
            if (typeof $scope.budgets != 'undefined') {
                for (var i = 0; i < $scope.budgets.length; i++) {
                    if ($scope.budgets[i].curvePer > 10) {
                        return true;
                    }
                }
            }

        }

        // check pmc per and add a popup message if it increase 10%
        var currentWarningFlag = false;
        $scope.$watch('pmcPer', function (newVal, oldVal) {
            if (newVal > 10 && !currentWarningFlag) {
                Notification.warning({ message: 'WW Channel guidance states that total carve-out should not exceed 10%.' + $rootScope.closeNotify, delay: Delay })
                $('#pmcPer').addClass('input-group-addon-border');
                var elem = $("#totalpmcperc")[0];
                elem.setAttribute('title', 'Total carve-out should not exceed 10%');
                $('#pmcPerSymbol').addClass('input-group-addon-symbol');
                currentWarningFlag = true
            }
            else if (newVal < 10 && currentWarningFlag) {
                $(".warning .message:contains('WW Channel')").parent().remove();
                currentWarningFlag = false;
                $('#pmcPer').removeClass('input-group-addon-border');
                var elem = $("#totalpmcperc")[0];
                elem.removeAttribute('title');
                $('#pmcPerSymbol').removeClass('input-group-addon-symbol');

            }
        })

        $scope.gotoNext = function () {

               $('#btnnext').attr('disabled', true);

            var equals = angular.equals(initialData, $scope.budgets);
           
            if (!equals) {
                bootbox.dialog({
                    title: "  ",
                    message: "<p style='margin-top: -30px !important;'>There are unsaved changes. Do you want to save the changes and navigate?</p>",
                    buttons: {
                        cancel: {
                            label: 'NO - Discard the changes and Navigate',
                            className: 'btn-default btn-notexttransform',
                            callback: function () {
                                $('#btnnext').attr('disabled', false);
                                $state.go("budget.partner-budget");
                            }

                        },
                        confirm: {
                            label: 'YES - Save and Navigate',
                            className: 'btn-success btn-notexttransform',
                            callback: function () {
                                $('#btnnext').attr('disabled', false);
                                $rootScope.Saved = true;
                                if ($scope.disablePrevious == false && $localStorage.NOACCESS == false) {
                                    $scope.save('next');
                                    $('#btnnext').attr('disabled', false);
                                 }
                                else {
                                    $state.go("budget.partner-budget")
                                }
                            }
                        }

                    }
                });
                $('#btnnext').attr('disabled', false);
            } else {
                $rootScope.Saved = true;
                if ($scope.disablePrevious == false && $localStorage.NOACCESS == false) {
                    modelParameterFactory.getWarningdata($localStorage.selectedversion.VersionID)
                                               .then(
                                                   function success(resp) {
                                                       if (resp.data == true) {
                                                           //Notification.warning("")
                                                           $state.go("budget.partner-budget")
                                                       }
                                                       else {

                                                           var bool = true;
                                                           $('#mdf-loader').css('display', 'block');
                                                           //budgetFactory.getModelParam();
                                                           $http({
                                                               method: 'GET',
                                                               url: $rootScope.api + 'ModelParameter/GetModelParameterDefault?VersionID=' + $localStorage.selectedversion.VersionID + '&UserID=' + $localStorage.user.UserID + '&frmStep1=' + bool
                                                           }).then(function (resp) {
                                                               $('#mdf-loader').css('display', 'none');
                                                               $state.go("budget.partner-budget")
                                                           });
                                                       }
                                                   },
                                                   function err(resp) {

                                                   }
                                             )

                }
                else {
                    $state.go("budget.partner-budget");
                }




                //$state.go("budget.historical-perfomance")
            }


        }

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
        //$scope.fillFinancialyearList(financial);
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
        //$scope.fillFinancialyearList(financial);
        $scope.fillFinancialyearss(financial);


        $scope.updateoncopy = function (version) {
            $rootScope.versionData = version;
            $localStorage.selectedversion = version;
            $localStorage.user.FinancialYearID = version.FinancialYearID;
            //get financial year for dropdown
            var financial = {
                FinancialyearID: $localStorage.user.FinancialYearID,
            }
            roleMappingService.getPlanningPeriods(financial).then(function onSuccess(response) {
                $scope.disablePrevious = $rootScope.disablePrevious;
            })
            $sessionStorage.fyParams.FinancialYearID = version.FinancialYearID;

            $scope.fillFinancialyearListNew();

            //     $scope.FinancialyearList = null;

            //   $http({
            //         method: 'POST',
            //         url: $rootScope.api + 'BUBudgets/GetFinancialYear',
            //         data: $sessionStorage.fyParams
            //     }).success(function (result) {
            //         $scope.FinancialyearList = result;
            //         $rootScope.FinancialyearList = result;
            //         $scope.versionSelected = version.VersionNo;
            //         debugger;
            //         $scope.drpdpwnvalue =version.FinancialPeriod;
            //     });



            //       // console.log($scope.versionSelected);
            //       budgetFactory.getBudgetOnSelect($scope, version.VersionID, version.FinancialYearID).then(function (response) {
            //         var data = response.data;
            //         $scope.BUObj = budgetFactory.generateJSON(data);
            //         $scope.budgets = $scope.BUObj.BuUnits;
            //         initialData = angular.copy($scope.budgets);

            //         $scope.getTotal();
            //         $timeout(function () {
            //             // format the textbox into dolar comma seperated($111,222)
            //             $(".specialText").priceFormat();
            //             // on tab in select the text
            //             budgetFactory.selectText();
            //         }, 100)
            //     })




            //     $localStorage.user.FinancialYearID = version.FinancialYearID;
            //    // $state.go('budget.allocation');
            //     $scope.drpdpwnvalue = version.FinancialPeriod;
            //     $scope.versionSelected = version.VersionNo;
            //     $sessionStorage.currentVersion.VersionID = version.VersionID;
            //     $scope.drpdpwnvalue.FinancialyearID = version.FinancialYearID;
            //     $scope.tableload();
        }


        //on table load values will change
        $scope.tableload = function () {
            var financial = {
                FinancialyearID: $scope.drpdpwnvalue.FinancialyearID,

            }
            $localStorage.previousYear = $scope.drpdpwnvalue.Financialyear.replace($scope.drpdpwnvalue.Financialyear.slice(2, 4), $scope.drpdpwnvalue.Financialyear.slice(2, 4) - 1);
            roleMappingService.getPlanningPeriods(financial).then(function onSuccess(response) {
                $scope.disablePrevious = $rootScope.disablePrevious;
                $scope.versionSelected = _.find($scope.drpdpwnvalue.Version, function (o) {
                    return o.VersionID == $scope.versionSelected
                });
                if ($scope.versionSelected) {
                    $localStorage.selectedversion = $scope.versionSelected;
                    $localStorage.selectedversion.FinancialPeriod = $scope.drpdpwnvalue.Financialyear;
                    $localStorage.user.FinancialYearID = $scope.drpdpwnvalue.FinancialyearID;
                    $sessionStorage.currentVersion.VersionID = $scope.versionSelected.VersionID;
                }

                budgetFactory.getBudgetOnSelect($scope, $scope.versionSelected.VersionID, $scope.drpdpwnvalue.FinancialyearID).then(function (response) {
                    var data = response.data;
                    $scope.BUObj = budgetFactory.generateJSON(data);
                    $scope.budgets = $scope.BUObj.BuUnits;
                    initialData = angular.copy($scope.budgets);

                    $scope.getTotal();
                    $timeout(function () {
                        // format the textbox into dolar comma seperated($111,222)
                        $(".specialText").priceFormat();
                        // on tab in select the text
                        budgetFactory.selectText();
                    }, 100)
                })
            })


        }

        $scope.loadVersion = function (fy) {
            $scope.versionSelected = null;

            if (fy == undefined) {
                $scope.versionList = [];
            } else {
                //var currentYear = _.find($rootScope.FinancialyearList, function (o) {
                //    return o.FinancialyearID == fy.FinancialyearID
                //});
                $scope.versionList = fy.Version;
                $localStorage.selectedversion = fy;
                $localStorage.selectedversion.FinancialPeriod = $localStorage.selectedversion.Financialyear;
                $scope.selectedyear = fy.FinancialyearID;
            }

        }

        //$scope.copy = function () {
        //    console.log($scope.drpdpwnvalue, $scope.versionSelected);

        //    //var copyobj = {
        //    //    OldFinancialyearID: $scope.drpdpwnvalue.Financialyear,
        //    //    OldVersionNo: $scope.versionSelected.VersionNo,
        //    //    NewFinancialyearID: $scope.drpdpwnvalue.FinancialyearID, // dest fy id
        //    //    RegionID: $localStorage.user.RegionID,
        //    //    PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
        //    //    DistrictId: $sessionStorage.resellerPartner.district.DistrictID,
        //    //    CountryID: $sessionStorage.resellerPartner.obj.id,
        //    //    UserID: $localStorage.user.UserID,
        //    //    VersionName: $scope.DestPlanvalue, // dest plan name
        //    //    VersionID: $scope.versionSelected.VersionID // version id

        //    //}
        //    //versioningservice.copydata(copyobj).success(function () {

        //    //    Notification.success({
        //    //        message: "Version Cloned Successfully!" + $rootScope.closeNotify,
        //    //        delay: null
        //    //    });
        //    //})
        //}


        $scope.copydata = function () {
            var copyobj = {
                OldFinancialyearID: $scope.drpdpwnvalue.Financialyear,
                OldVersionNo: $scope.versionSelected.VersionNo,
                NewFinancialyearID: $scope.drpdpwnvalue1, // dest fy id
                PartnerTypeID: $sessionStorage.resellerPartner.partner.PartnerTypeID,
                GeoID: $sessionStorage.resellerPartner.obj.geo.GeoID,
                DistrictId: $sessionStorage.resellerPartner.district.DistrictID,
                CountryID: $sessionStorage.resellerPartner.obj.countryID,
                UserID: $localStorage.user.UserID,
                VersionName: $scope.DestPlanvalue, // dest plan name
                VersionID: $scope.versionSelected.VersionID, // version id
                CountryOrGeoOrDistrict: $sessionStorage.resellerPartner.CountryOrGeoOrDistrict,
                AllocationLevel: $sessionStorage.resellerPartner.obj.AllocationLevel.charAt(0),
                MembershipGroupID: $sessionStorage.resellerPartner.membership.MembershipGroupID

            }

            versioningservice.copydata(copyobj).success(function (resp) {
                $sessionStorage.currentVersion = resp;
                $scope.updateoncopy(resp);
                Notification.success({
                    message: "Version Copied Successfully!" + $rootScope.closeNotify,
                    delay: Delay
                });
            })
        }
        //to clear the values after closing modal popup
        $('#myModalcreate,#CopyModal').on('hidden.bs.modal', function () {

            $scope.drpdpwnvalue1 = null;
            $scope.DestPlanvalue = "";

        });

    }
})();

