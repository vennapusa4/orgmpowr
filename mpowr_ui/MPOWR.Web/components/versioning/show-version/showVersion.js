(function (app) {
    "use strict";
    app.component('showVersion', {
        bindings: {

        },
        templateUrl: 'components/versioning/show-version/show-version.html',
        controller: ['$scope', '$rootScope', '$sessionStorage', '$localStorage' ,linkFn]
    });

    function linkFn($scope,$rootScope,$sessionStorage,$localStorage) {
        
        var vm = this;
        vm.versionNo = $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion.VersionNo : $localStorage.selectedversion.VersionNo;
        vm.VersionName = $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion.VersionName : $localStorage.selectedversion.VersionName;
        vm.year = $localStorage.selectedversion == undefined ? $sessionStorage.currentVersion.FinancialPeriod : $localStorage.selectedversion.FinancialPeriod;
       
    }
})(angular.module("hpe"));
