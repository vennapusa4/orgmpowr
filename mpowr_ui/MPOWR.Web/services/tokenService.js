'use strict';
angular.module("hpe").factory('httpRequestInterceptor', function ($localStorage, $injector, $sessionStorage) {
    return {
        request: function (config) {
            if ($localStorage.user !== undefined)
            //    config.headers['authenticationToken'] = $localStorage.user.TokenID;
                config.headers['authenticationToken'] = $localStorage.user.TokenID + '/' + $localStorage.user.RoleID ;
            return config;
        },
        responseError: function (res) {

            if (res.data == 'Sorry, You do not have the required permission to perform this action. ' && $injector.get('$state').current.name != 'login') {
                delete $sessionStorage.configdays;
                delete $sessionStorage.searchResultss;
                delete $sessionStorage.fyParams;
                delete $sessionStorage.resellerPartner;
                delete $sessionStorage.lastUpdatedDate;
                delete $localStorage.selectedversion;
                delete $sessionStorage.currentVersion;
                // Delete localStorage
                delete $localStorage.showSpartlines;
                delete $localStorage.user;
                delete $localStorage.preYear;
                delete $localStorage.prePeriod;
                delete $localStorage.previousYear;
                delete $localStorage.previoustoPreviousYear;
                delete $localStorage.businessUnits;
                delete $localStorage.Admin;
                delete $localStorage.PartnerTypeID;
                $injector.get('$state').go('login');
            }

            return res;
        }

    };
});
 