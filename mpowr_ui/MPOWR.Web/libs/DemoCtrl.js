angular.module('hpe')
    .controller('DemoCtrl', function($scope, Idle, Keepalive, $uibModal,authService,$window,$state){
      $scope.started = false;

      var url = $window.location.hash.split('/')[1]
      
      
      function closeModals() {
        if ($scope.warning) {
            $scope.warning.close();
            $('#UserManualModel').modal('hide');
          $scope.warning = null;
        }

        if ($scope.timedout) {
          $scope.timedout.close();
          $scope.timedout = null;
        }
      }

      $scope.$on('IdleStart', function() {
        if($window.location.hash.split('/')[1] != 'login'){
          closeModals();
          $scope.warning = $uibModal.open({
            templateUrl: 'warning-dialog.html',
            windowClass: 'modal-danger',
          });  
        }
        
      });

      $scope.$on('IdleEnd', function() {
        closeModals();

      });

      $scope.$on('IdleTimeout', function () {
          //
        if($window.location.hash.split('/')[1] != 'login'){
            closeModals();
            bootbox.hideAll();
          authService.logout();
          $scope.timedout = $uibModal.open({
            templateUrl: 'timedout-dialog.html',
            windowClass: 'modal-danger'
          });
        }
      });

      $scope.start = function() {
        closeModals();
        Idle.watch();
        $scope.started = true;
      };

      $scope.stop = function() {
        closeModals();
        Idle.unwatch();
        $scope.started = false;

      };

      $scope.closeModal = function(){
        closeModals();
      }

    })
    