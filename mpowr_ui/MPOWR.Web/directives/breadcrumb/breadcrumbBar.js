(function () {
    'use strict';
    angular.module('hpe').directive('breadcrumbBar', function () {

        // Runs during compile
        return {
            // name: '',
            // priority: 1,
            // terminal: true,
            scope: {
                module: "@"
            }, // {} = isolate, true = child, false/undefined = no change
            controller: function ($scope, $element, $attrs, $transclude, $state, $rootScope, $http, $localStorage, $sessionStorage, modelParameterFactory, roleMappingService, breadCrumbFactory) {
                
                $scope.adminMenus = ['User Management', 'Role Management', 'Glossary Management', 'Geo Configuration','BU Configuration'];
                $scope.sessionStorage = $sessionStorage;
                $rootScope.Saved = true;
                // handle for portal
                if ($scope.module == 'portal') {
                    breadCrumbFactory.portal($state, $scope, $localStorage)
                }
                else if ($scope.module == 'admin') {
                    breadCrumbFactory.admin($state, $scope, $localStorage)
                }
                var financial = {
                    FinancialyearID: $localStorage.user.FinancialYearID,

                }
                roleMappingService.getPlanningPeriods(financial).then(function onSuccess(response) {
                    $scope.disablePrevious = $rootScope.disablePrevious;
                    //if ($rootScope.disablePrevious == false) {
                        if ($localStorage.selectedversion != undefined && $localStorage.selectedversion.VersionID != undefined) {
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
                            })
                        }
                    //}
                })
                $scope.NOACCESS = $localStorage.NOACCESS;
                $scope.GlossaryApprover = $localStorage.user.GlossaryApprover;
                //breadCrumbFactory.update($http, $scope, $rootScope, $localStorage)
                $scope.goto = function (menu) {
                    if ($state.current.name == 'budget.allocation') {
                        if (!$rootScope.Saved) {

                            bootbox.dialog({
                                title: "  ",
                                message: "<p style='margin-top: -30px !important;'>There are unsaved changes. Do you want to save the changes and navigate?</p>",
                                buttons: {
                                    cancel: {
                                        label: 'NO - Discard the changes and Navigate',
                                        className: 'btn-default btn-notexttransform',
                                        callback: function () {
                                            $('#btnnext').attr('disabled', false);
                                            $rootScope.Saved = true;
                                            if (menu.enabled) {
                                                if (menu.url == 'budget.partner-budget') {
                                                    if ($scope.disablePrevious == false && $scope.NOACCESS == false) {
                                                        modelParameterFactory.getWarningdata($localStorage.selectedversion.VersionID)
                                                  .then(
                                                      function success(resp) {
                                                          if (resp.data == true) {
                                                              //Notification.warning("")
                                                              $state.go(menu.url)
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
                                                        $state.go(menu.url);
                                                    }
                                                }
                                                else {
                                                    $state.go(menu.url);
                                                }
                                            }

                                        }

                                    },
                                    confirm: {
                                        label: 'YES - Save and Navigate',
                                        className: 'btn-success btn-notexttransform',
                                        callback: function () {
                                            $('#btnnext').attr('disabled', false);
                                            $rootScope.Saved = true;
                                            if ($scope.disablePrevious == false && $localStorage.NOACCESS == false) {

                                                $rootScope.$broadcast('save', { menu: menu.url });

                                                $('#btnnext').attr('disabled', false);
                                            }
                                            else {
                                                $state.go(menu.url);
                                            }
                                        }
                                    }

                                }
                            });

                        }
                        else {
                            if (menu.enabled) {
                                if (menu.url == 'budget.partner-budget') {
                                    if ($scope.disablePrevious == false && $scope.NOACCESS == false) {
                                        modelParameterFactory.getWarningdata($localStorage.selectedversion.VersionID)
                                  .then(
                                      function success(resp) {
                                          if (resp.data == true) {
                                              //Notification.warning("")
                                              $state.go(menu.url)
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
                                        $state.go(menu.url);
                                    }

                                }
                                else {
                                    $state.go(menu.url);
                                }
                            }
                        }
                    }
                    else {
                        if (!$rootScope.Saved) {
                            bootbox.confirm({
                                message: "There are unsaved changes. Do you want to continue without saving?",
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
                                    if (result) {
                                        $rootScope.Saved = true;
                                        if (menu.enabled) {
                                            if (menu.url == 'budget.partner-budget') {
                                                if ($scope.disablePrevious == false && $scope.NOACCESS == false) {
                                                    modelParameterFactory.getWarningdata($localStorage.selectedversion.VersionID)
                                              .then(
                                                  function success(resp) {
                                                      if (resp.data == true) {
                                                          //Notification.warning("")
                                                          $state.go(menu.url)
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
                                                    $state.go(menu.url);
                                                }
                                            }
                                            else {
                                                $state.go(menu.url);
                                            }
                                        }
                                        //$state.go("budget.partner-budget")
                                        //$state.go("budget.historical-perfomance")
                                    } else {
                                        console.log("User clicked cancel")
                                    }

                                }
                            });
                        }
                        else {
                            if (menu.enabled) {
                                if (menu.url == 'budget.partner-budget') {
                                    if ($scope.disablePrevious == false && $scope.NOACCESS == false) {
                                        modelParameterFactory.getWarningdata($localStorage.selectedversion.VersionID)
                                  .then(
                                      function success(resp) {
                                          if (resp.data == true) {
                                              //Notification.warning("")
                                              $state.go(menu.url)
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
                                        $state.go(menu.url);
                                    }

                                }
                                else {
                                    $state.go(menu.url);
                                }
                            }
                        }
                    }
                    

                },
                  
                   $scope.validate = function (menu) {
                       if ($scope.adminMenus.indexOf(menu.name)!=-1) {
                           return false;
                       }
                       else {
                           if ($sessionStorage.currentVersion != undefined) {
                               if ($sessionStorage.currentVersion.Version != undefined)
                                   return $sessionStorage.currentVersion.Version.length === 0;
                               else if ($sessionStorage.currentVersion.VersionID != undefined)
                                   return false;
                               else return true;
                           }
                           else return true;
                       }
                       //return $sessionStorage.currentVersion.Version.length === 0;
                   }
            },
            // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
            restrict: 'AE', // E = Element, A = Attribute, C = Class, M = Comment
            // template: '',
            templateUrl: 'directives/breadcrumb/breadcrumb.html',
            // replace: true,
            // transclude: true,
            // compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
            link: function ($scope, iElm, iAttrs, controller) {

            }

        };
    });
})();
