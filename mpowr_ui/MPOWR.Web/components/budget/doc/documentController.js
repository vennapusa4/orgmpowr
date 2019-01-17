
(function () {
    'use strict';
    angular.module("hpe")
        .directive('ngFiles', ['$parse', function ($parse) {

            function fn_link(scope, element, attrs) {
                
                var onChange = $parse(attrs.ngFiles);
                element.on('change', function (event) {
                    onChange(scope, { $files: event.target.files });
                });
            };

            return {
                link: fn_link
            }
           
        }])

        .controller("documentCtrl", docFunction)

    function docFunction($scope, $http, $rootScope,  $localStorage, UtilityService, Notification) {
        
        getdata();
        function getdata() {
            $http.get($rootScope.api + 'UserManualController/GetDetails').success(function (response) {
                $scope.userDetails = response;
            
            });
        }

        $scope.deleteFile = function (id) {
           
            $http.get($rootScope.api + 'UserManualController/DeleteFile?fileID=' + id).success(function (response) {
                getdata();
               
            });
        }

        $scope.clickUpload = function () {
            angular.element('#file1').trigger('click');
          
        };

        var formdata = new FormData();
        $scope.getTheFiles = function ($files) {
           // formdata = null;
            angular.forEach($files, function (value, key) {
                formdata.append(key, value);
                
            });
            console.log($files);
        };
        $scope.onFileSelect = function () {
            
           
         
            var request = {
                method: 'POST',
                url: $rootScope.api + 'UserManualController/fileupload',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };

            // SEND THE FILES.
            $http(request)
                .success(function (d) {
                   
                    getdata();
                  
                    if (d == "Success") {
                        Notification.success({
                            message: "File Uploaded Successfully",
                            delay: null
                        });
                    }
                    if (d == "fileAlreadyExited") {
                        Notification.warning({
                        message: "File Already Exist",
                        delay: null
                    });
                    }
                    if (d == "maxSizeExceeded") {
                        Notification.error({
                            message: "Maximum File Size Exceeded",
                            delay: null
                        });
                    }
                    else {
                        Notification.warning({
                            message: d,
                            delay: null
                        });
                    }
                    formdata = new FormData();
                })
                .error(function () {
                    formdata = new FormData();
                    Notification.error({
                        message: "Maximum File Size Exceeded",
                        delay: null
                    });
                });
           
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


    };
   
})();
