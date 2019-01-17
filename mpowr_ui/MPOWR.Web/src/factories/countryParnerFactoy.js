/*
Budget Factory: All the budget related functionality are written here
Created by: Aamin Khan
Created at: 25/01/2017
*/
(function () {
    'use strict';
    angular.module('hpe').factory('countryParnerFactoy', countryParnerFactoyFn);
    function countryParnerFactoyFn($http,$rootScope,$localStorage) {
        //ngInject
        return {
            row : {"Countries":[{"CountryID":1,"CountryName":"CANADA"},{"CountryID":2,"CountryName":"UNITED STATES"},{"CountryID":3,"CountryName":"ANTIGUA AND BARBUDA"},{"CountryID":4,"CountryName":"ARGENTINA"},{"CountryID":5,"CountryName":"BRAZIL"},{"CountryID":6,"CountryName":"CENTRAL AMERICA"},{"CountryID":7,"CountryName":"CENTRAL AMERICA AND CARIBBEAN"},{"CountryID":8,"CountryName":"CHILE"},{"CountryID":9,"CountryName":"COLOMBIA"},{"CountryID":10,"CountryName":"ECUADOR"},{"CountryID":11,"CountryName":"MCA MEXICO AND BRAZIL"},{"CountryID":12,"CountryName":"MEXICO"},{"CountryID":13,"CountryName":"NETHERLANDS ANTILLES"},{"CountryID":14,"CountryName":"PERU"},{"CountryID":15,"CountryName":"PUERTO RICO"},{"CountryID":16,"CountryName":"TRINIDAD AND TOBAGO"},{"CountryID":17,"CountryName":"VENEZUELA"},{"CountryID":18,"CountryName":"AEC"},{"CountryID":19,"CountryName":"INDONESIA"},{"CountryID":20,"CountryName":"MALAYSIA"},{"CountryID":21,"CountryName":"PHILIPPINES"},{"CountryID":22,"CountryName":"SINGAPORE"},{"CountryID":23,"CountryName":"THAILAND"},{"CountryID":24,"CountryName":"VIETNAM"},{"CountryID":25,"CountryName":"AUSTRALIA"},{"CountryID":26,"CountryName":"NEW ZEALAND"},{"CountryID":27,"CountryName":"CHINA"},{"CountryID":28,"CountryName":"HONG KONG"},{"CountryID":29,"CountryName":"INDIA"},{"CountryID":30,"CountryName":"JAPAN"},{"CountryID":31,"CountryName":"KOREA (REP.)"},{"CountryID":32,"CountryName":"TAIWAN"},{"CountryID":33,"CountryName":"ALBANIA"},{"CountryID":34,"CountryName":"ARMENIA"},{"CountryID":35,"CountryName":"AZERBAIJAN"},{"CountryID":36,"CountryName":"BELARUS"},{"CountryID":37,"CountryName":"BOSNIA AND HERZEGOVINA"},{"CountryID":38,"CountryName":"BULGARIA"},{"CountryID":39,"CountryName":"CIS"},{"CountryID":40,"CountryName":"CROATIA"},{"CountryID":41,"CountryName":"GEORGIA"},{"CountryID":42,"CountryName":"HUNGARY"},{"CountryID":43,"CountryName":"ISRAEL"},{"CountryID":44,"CountryName":"KAZAKHSTAN"},{"CountryID":45,"CountryName":"KOSOVO"},{"CountryID":46,"CountryName":"KYRGYZSTAN"},{"CountryID":47,"CountryName":"MACEDONIA"},{"CountryID":48,"CountryName":"MALTA"},{"CountryID":49,"CountryName":"MOLDOVA"},{"CountryID":50,"CountryName":"MONTENEGRO"},{"CountryID":51,"CountryName":"POLAND"},{"CountryID":52,"CountryName":"ROMANIA"},{"CountryID":53,"CountryName":"SEE"},{"CountryID":54,"CountryName":"SERBIA"},{"CountryID":55,"CountryName":"SLOVENIA"},{"CountryID":56,"CountryName":"TURKMENISTAN"},{"CountryID":57,"CountryName":"UKRAINE"},{"CountryID":58,"CountryName":"UZBEKISTAN"},{"CountryID":59,"CountryName":"AUSTRIA"},{"CountryID":60,"CountryName":"BELGIUM"},{"CountryID":61,"CountryName":"DENMARK"},{"CountryID":62,"CountryName":"ESTONIA"},{"CountryID":63,"CountryName":"FINLAND"},{"CountryID":64,"CountryName":"ICELAND"},{"CountryID":65,"CountryName":"LATVIA"},{"CountryID":66,"CountryName":"LIECHTENSTEIN"},{"CountryID":67,"CountryName":"LITHUANIA"},{"CountryID":68,"CountryName":"LUXEMBOURG"},{"CountryID":69,"CountryName":"NETHERLANDS"},{"CountryID":70,"CountryName":"NORWAY"},{"CountryID":71,"CountryName":"SWEDEN"},{"CountryID":72,"CountryName":"SWITZERLAND"},{"CountryID":73,"CountryName":"FRANCE"},{"CountryID":74,"CountryName":"GERMANY"},{"CountryID":75,"CountryName":"IRELAND"},{"CountryID":76,"CountryName":"UNITED KINGDOM"},{"CountryID":77,"CountryName":"ITALY"},{"CountryID":78,"CountryName":"PAN EUROPEAN"},{"CountryID":79,"CountryName":"PORTUGAL"},{"CountryID":80,"CountryName":"SPAIN"},{"CountryID":81,"CountryName":"RUSSIAN FEDERATION"}],"Partners":[{"PartnerTypeID":1,"PartnerName":"Distributor"},{"PartnerTypeID":2,"PartnerName":"Reseller"}],"Districts":[{"DistrictID":1,"DistrictName":"Las Vegas"}]},
            
            getCountry: function(){
                return this.row.Countries;
            },
            getPartners: function(){
                return this.row.Partners;
            },
            getDistricts: function(){
                return this.row.Districts;
            },
            getData: function(){
                console.log($localStorage.user)
                var promise = 
                $http({
                    method: 'GET',
                    url: $rootScope.api + 'countries/GetCountries?userid='+$localStorage.user.UserID
                }).then(function(resp){
                    return resp;
                });
                return promise;
            }
        }
    }

})();