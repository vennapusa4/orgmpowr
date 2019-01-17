

(function () {
    'use strict';
    angular.module('hpe').directive('document', function () {
        // Runs during compile
        return {
            // name: '',
            // priority: 1,
            // terminal: true,
            // scope: {}, // {} = isolate, true = child, false/undefined = no change
            controller: function ($scope, $element, $window, $attrs, $transclude, $state, $localStorage, $http, $rootScope, UtilityService, Notification, $timeout) {

                $scope.$on('UserManual', function () {
                    $scope.isAdmin = $localStorage.Admin;
                    $scope.orderByField = 'id';
                    $scope.reverseSort = null;
                    document.getElementById("uploadFile").value = null;
                    document.getElementById('uploadBtn').value = null;
                    getdata();
                });


                //$rootScope.$on('LoginTrigger', function (event) {
                //    getdata();
                //});
                $scope.alerts = function () {
                    alert("Clicked");
                };
                function getdata() {

                    $http.get($rootScope.api + 'UserManualController/GetDetails').success(function (response) {
                        $scope.userDetails = response;
                        $scope.count = $scope.userDetails
                    });
                }


                $scope.deleteFile = function (id) {
                    if ($window.confirm("Do You want to delete that Document ?")) {
                        $http.get($rootScope.api + 'UserManualController/DeleteFile?fileID=' + id).success(function (response) {
                            getdata();
                            Notification.success({
                                message: "File Deleted Successfully" + $rootScope.closeNotify,
                                delay: null
                            });
                        });
                    }
                }

                var formdata = new FormData();
                $scope.setFiles = function (element) {
                    $scope.orderByField = 'id';
                    $scope.reverseSort = null;
                    for (var i in element.files) {
                        formdata.append(element.files[i].name, element.files[i])
                    }
                    //angular.forEach(element.files, function (value, key) {
                    //    formdata.append(key, value);

                    //});
                    if (element.files.length > 1) {
                        document.getElementById("uploadFile").value = element.files.length + " Files attached.";
                    }
                    else {
                        document.getElementById("uploadFile").value = element.files[0].name;
                    }


                }

                $scope.onFileSelect = function () {
                    var fi = document.getElementById('uploadBtn');
                    if (fi.files.length <= 0) {
                        Notification.error({
                            message: "Please select File" + $rootScope.closeNotify,
                            delay: null
                        });
                    }
                    else {
                        $('#mdf_loader').css('display', 'block');
                        var request = {
                            method: 'POST',
                            url: $rootScope.api + 'UserManualController/fileupload',
                            data: formdata,
                            headers: {
                                'Content-Type': undefined
                            }
                        }

                        // SEND THE FILES.
                        $http(request)
                            .success(function (d) {
                                document.getElementById("uploadFile").value = null;
                                document.getElementById('uploadBtn').value = null;
                                getdata();
                                $('#mdf_loader').css('display', 'none');
                                if (d == "Success") {
                                    Notification.success({
                                        message: "File uploaded successfully" + $rootScope.closeNotify,
                                        delay: null
                                    });
                                }
                                if (d == "File Already Existed") {
                                    Notification.warning({
                                        message: "File already exist" + $rootScope.closeNotify,
                                        delay: null
                                    });
                                }
                                if (d == "maxSizeExceeded") {
                                    Notification.error({
                                        message: "Maximum file size exceeded" + $rootScope.closeNotify,
                                        delay: null
                                    });
                                }

                                formdata = new FormData();
                            })
                            .error(function (d) {
                                document.getElementById("uploadFile").value = null;
                                document.getElementById('uploadBtn').value = null;
                                formdata = new FormData();
                                $('#mdf_loader').css('display', 'none');
                                Notification.error({
                                    message: d + "Error" + $rootScope.closeNotify,
                                    delay: null
                                });
                            });

                    }
                }

                // Sorting Users
                $scope.sort = function (key, elemId) { // key = PartnerName
                    var elem = $("#" + elemId);
                    if (elemId !== 'fileList')
                        angular.element('#fileList').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
                    if (elemId !== 'filetype')
                        angular.element('#filetype').find("i").removeClass('fa-caret-up').addClass('fa-caret-down');

                    var icon = elem.find("i").attr("class").split(" ")[2];
                    var toSort = 'asc';
                    if (icon === 'fa-caret-down') { // do descending sort
                        var toSort = 'asc';
                        elem.find("i").removeClass('fa-caret-down').addClass('fa-caret-up');
                        console.log("a");
                    } else { // do ascending sort
                        var toSort = 'desc';
                        elem.find("i").removeClass('fa-caret-up').addClass('fa-caret-down');
                        console.log("d");
                    }
                    console.log(toSort);
                    $scope.userDetails = UtilityService.sortForUserManual($scope.userDetails, key, toSort);
                    console.log($scope.userDetails);
                }
            },
            // require: 'ngModel', // Array = multiple requires, ? = optional, ^ = check parent elements
            restrict: 'AE', // E = Element, A = Attribute, C = Class, M = Comment
            // template: '',
            templateUrl: 'directives/docs/document.html',
            // replace: true,
            // transclude: true,
            // compile: function(tElement, tAttrs, function transclude(function(scope, cloneLinkingFn){ return function linking(scope, elm, attrs){}})),
            link: function ($scope, iElm, iAttrs, controller) {

            }

        };
    });
})();
