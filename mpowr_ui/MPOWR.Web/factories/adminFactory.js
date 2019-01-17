/*
Budget Factory: All the budget related functionality are written here
Created by: Aamin Khan
Created at: 25/01/2017
*/
(function () {
    'use strict';
    angular.module('hpe').factory('adminFactory', adminFactoryFn);
    function adminFactoryFn($localStorage) {
        //var admin = false;
        //ngInject
        return {
            IsAdmin: function () {
                return $localStorage.Admin;
            },
            setAdmin: function (isAdmin) {
                //admin = isAdmin;
                //$localstorage.setItem('isAdmin', isAdmin); 
                $localStorage.Admin = isAdmin;
            }
        }
    }

})();