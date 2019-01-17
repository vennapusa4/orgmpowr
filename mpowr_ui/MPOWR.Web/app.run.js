(function () {
    'use strict';

    angular.module("hpe").run(rn);



    function rn($rootScope,Notification, $timeout, $localStorage, authService,Idle) {

        Idle.watch();

        var api = {
            dev: "http://localhost:54961/api/",
            sit: "http://172.16.69.36:85/api/",
            cloud: "http://mpowr.cloudapp.net:7002/api/",   
            cloud1: "http://mpowr.cloudapp.net:8002/api/",
            azure: "https://mpowrapi.azurewebsites.net/api/",
            azuredev: "https://mpowrapi-apidev.azurewebsites.net/api/",
            azureuat: "https://mpowrapi-apiuat.azurewebsites.net/api/",
            azuresit: "https://mpowrapi-apisit.azurewebsites.net/api/",
            azurestag:"https://mpowrapi-apistag.azurewebsites.net/api/"
        }
        $rootScope.api = api.dev;

        var DataUploadUri = {
            dev: "http://localhost:4200",
            SIT: "https://etluploaddata-mpowrupload-sit.azurewebsites.net",
        };

        $rootScope.DataUpload = DataUploadUri.SIT;

        $rootScope.$on('$stateChangeStart', function (event, toState, toStateParams) {

            if (typeof $localStorage.user === 'undefined') {
                // Here you can take the control and call your own functions:
                window.location.href = '/#/login';
            }
            Notification.clearAll();

        });
        $rootScope.$on('$stateChangeSuccess', function (event, to, toParams, from, fromParams) {
            //save the previous state in a rootScope variable so that it's accessible from everywhere
            $rootScope.previousState = from;
        });
        $rootScope.closeNotify = "<i class='fa fa-times-circle-o close-notify'></i>";
        $rootScope.version = version;
        $rootScope.labels = {
            Login: {
                username: 'Username',
                password: 'Password',
                ErrMsg: {
                    auth: 'Please enter a valid username / password',
                    invalidUsername: 'Invalid username.',
                    requireUsername: 'Please enter username.',
                    requirePassword: 'Please enter password.'
                },
                button: {
                    Login: 'Login'
                }
            }
        }
    }

})();

