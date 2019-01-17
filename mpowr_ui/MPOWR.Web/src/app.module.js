/*
App module: This is the main module of hpe ui App
Created by: Aamin Khan
Created at: 19/01/2017
*/
(function () {
    'use strict';

    angular.module('hpe', 
    	[
    		'ui.router',
    		'navBar.module',
    		'leftbar.module',
    		'ui-notification',
            'ngStorage',
            'nvd3'
    	])
    /*
	* navBar.module is the module which defines the top navidation bar
	* leftbar.module is the module that defines the left bar
	* ui-notification is for showing notifications
    */
})();