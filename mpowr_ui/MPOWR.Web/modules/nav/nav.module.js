/*
App module: Top panel module
Created by: Aamin Khan
Created at: 23/01/2017
Dependencies: bootstrap css, font-awesome
*/
(function () {
    'use strict';

    angular.module('navBar.module', ['ngStorage']);

    angular.module('navBar.module').directive('navBar', function () {
        // Runs during compile
        return {
            // name: '',
            // priority: 1,
            // terminal: true,
            // scope: {}, // {} = isolate, true = child, false/undefined = no change
            controller: function ($scope, $filter, $element, $attrs, ExitService, $transclude, $timeout, $localStorage, $state, $rootScope, $http, $sessionStorage, authService) {
                $scope.testt = $localStorage;
                if ($sessionStorage.configdays === undefined) {
                    $http({
                        method: 'GET',
                        url: $rootScope.api + "Search/GetPopup"
                    }).then(function (res) {
                        console.log(res);
                        $sessionStorage.searchResultss = res.data;

                        var currentdate = new Date();
                        var sdate = new Date($filter('date')(res.data[0].LatestDataRefresh, "yyyy-MM-dd"));
                        sdate.setDate(sdate.getDate() + res.data[0].PopupDays);

                        // $scope.IsPopup= res.data[0].IsPopup,
                        $scope.configdays = currentdate <= sdate ? 'true' : 'false';
                        $sessionStorage.configdays = $scope.configdays;
                    });


                }

                $scope.switchToDataUpload = function () {
                    var SwitchToDataUploadUrl = $rootScope.DataUpload + '/#/authenticate/' + (btoa($localStorage.user.ApplicationID)).replace(/\=/g, "") + '/'
                   + (btoa($localStorage.user.AppName)).replace(/\=/g, "") + '/' + (btoa($localStorage.user.UserID)).replace(/\=/g, "") + '/'
                   + (btoa($localStorage.user.TokenID)).replace(/\=/g, "")
                   + '/' + (btoa("SIT")).replace(/\=/g, "");
                    window.open(SwitchToDataUploadUrl, "_blank");
                }

                $scope.configdays = $sessionStorage.configdays;
                $scope.showUpdateTime = false;
                $scope.UpdatedDate = $sessionStorage.lastUpdatedDate;

                var cleanUpFunc = $rootScope.$on("lastUpdatedTimeUpdated", function (event, data) {
                    $scope.showUpdateTime = true;
                    $scope.UpdatedDate = data;
                })

                $scope.$on('$destroy', function () {
                    cleanUpFunc();
                });



                $timeout(function () {
                    $(".top-navbar").click(function (e) {
                        if ($(e.target).closest(".icon .fa-bars").length > 0) {
                            return false;
                        }
                        $(".leftbar").css("width", '81px');
                        $(".menu-items > ul > li > a > h5").hide();
                        $(".no-exp").show();
                        $(".exp").hide();
                    })
                }, 100);

                //logout
                $scope.logout = function () {
                    delete $sessionStorage.configdays;
                    delete $sessionStorage.searchResultss;
                    authService.logout();
                }



                $scope.expandBut = true;
                checkURL();

                $rootScope.$on('$stateChangeStart', function (event, toState, toStateParams) {
                    console
                    checkURL();

                });
                function checkURL() {
                    $timeout(fn, 10);
                    function fn() {
                        var url = window.location.href.split("#")[1];
                        if (url == '/dashboard' || url == '/prebudget') {
                            $scope.expandBut = false;
                        } else {
                            $scope.expandBut = true;
                        }
                    }
                }
                $scope.userName = $localStorage.user.FirstName;
                $scope.isAdmin = $localStorage.Admin;
                $scope.refreshDirective = function () {
                    var emptyData = [];
                    $rootScope.$broadcast('refreshGlossary', emptyData);
                }
                $scope.UserManual = function () {
                    $rootScope.$broadcast('UserManual');
                }
            },
            // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
            // restrict: 'A', // E = Element, A = Attribute, C = Class, M = Comment
            // template: '',
            templateUrl: 'modules/nav/nav.html',
            // replace: true,
            transclude: true,
            // compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
            link: function ($scope, iElm, iAttrs, controller) {
                $scope.toggle = function () {
                    $(".leftbar").css("width", '245px');
                    $(".leftbar").css("z-index", '11');
                    $(".menu-items > ul > li > a > h5").show();
                    $(".logo > img").toggleClass(".show");
                    $(".no-exp").hide();
                    $(".exp").show();
                }
            }
        };
    });


})();

//to create exit log

(function () {
    'use strict';
    angular.module('navBar.module').service('ExitService', ExitServiceFn);

    function ExitServiceFn($rootScope, $http, $localStorage) {
        //ngInject
        this.Exit = function (user) {
            var promise =
			$http({
			    method: 'POST',
			    url: $rootScope.api + 'LoginService/UpdateLogOffTime',
			    headers: {
			        "authenticationToken": $localStorage.user.TokenID,
			    },
			    data: user
			}).then(function (resp) {
			    return resp;
			});
            return promise;
        }
    }

})();
