(function () {
    "use strict"
    angular.module("hpe").factory("partnerBudgetDataService", partnerBudgetDataService);

    function partnerBudgetDataService($http, $rootScope, $log, $q, $localStorage, $sessionStorage) {
        //ngInject
        var service = {
            ExportSDFC: ExportSDFC,
            Export: Export,
            getPartnerBu: getPartnerBu
         }
        return service;

        function ExportSDFC(params) {
            var vid = $localStorage.selectedversion != undefined ? $localStorage.selectedversion.VersionID : $sessionStorage.currentVersion.VersionID;
            //var url = $rootScope.api + "Export/ExporttoExcelSDFC?CountryID=" + params.CountryID + "&FinancialyearID=" + params.FinancialyearID + "&DistrictID=" + params.DistrictID + "&PartnerTypeID=" + params.PartnerTypeID+
            //"&VersionID=" + vid;
            var url = $rootScope.api + "Export/ExporttoExcelSDFC?VersionID=" + vid;
            window.open(url, '_blank', '');
        }
        function Export(params) {
            var vid = $localStorage.selectedversion != undefined ? $localStorage.selectedversion.VersionID : $sessionStorage.currentVersion.VersionID;
          //  var url = $rootScope.api + "Export/ExporttoExcelPartnerBudget?CountryID="+params.CountryID+"&FinancialyearID="+params.FinancialyearID+"&DistrictID="+params.DistrictID+"&PartnerTypeID="+params.PartnerTypeID  +"&VersionID=" + vid;
          var url = $rootScope.api + "Export/ExporttoExcelPartnerBudget?VersionID=" + vid + '&bulist=' + $localStorage.user.BUs;

            window.open(url, '_blank', '');
        }
        
        function getPartnerBu(partnerBugetId) {
            var vid = $localStorage.selectedversion != undefined ? $localStorage.selectedversion.VersionID : $sessionStorage.currentVersion.VersionID;
                 var $promise =$http({
                    method: 'GET',
                    url: $rootScope.api + "PartnerBudget/GetPartnerBudgetBUDetails?PartnerBudgetID=" + partnerBugetId + "&VersionID=" + vid + "&WithoutHistory=" + $localStorage.chkhistory, 
                }).then(function(resp){
                    return resp;
                });
            
            return $promise;
        }
    }
})();


(function () {
    "use strict"
    angular.module("hpe").service("dataService", dataService);

    function dataService() {
        //ngInject
        this.partners = [];
        this.setPartners = function(arr){
            this.partners = arr;
        }
        this.getPartners = function(){
            return this.partners
        }
    }
})();