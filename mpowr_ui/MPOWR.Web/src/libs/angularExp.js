var app = angular.module('app', []);
  app.controller('MainCtrl', function($scope, $http) {
    $scope.activeMenu = 'MDF Strategy';
    $scope.txtCloud = "0";
    $scope.txtGPC = "0";
    $scope.txtDiscover = "0";
    $scope.txtOwn = "0";
    $scope.txtBudgetPer="100";
    $scope.txtCarve="0";
    $scope.txtCountry="0";
    $scope.txtClassic="0";



  
        
     $scope.totalPer = function(){
        var k = ($scope.txtCarve.replace(",","") * 100) /  $scope.txtBudget.replace(",","");
        return k.toFixed(2)
    };
    $scope.totalCon = function(){
        var k = ($scope.txtCountry.replace(",","") * 100) /  $scope.txtBudget.replace(",","");
        return k.toFixed(2)
    };
    $scope.totalClassic = function(){
        var k = ($scope.txtClassic.replace(",","") * 100) /  $scope.txtBudget.replace(",","");
        return k.toFixed(2)
    };
     $scope.totalBase1 = function(){
       
        var k = ($scope.txtBase1.replace(",","") * 100) /  $scope.txtClassic.replace(",","")
        return k.toFixed(2)
    };
     
      $scope.totalCloud = function(){
       
        var k = ($scope.txtCloud.replace(",","") * 100) /  $scope.txtCarve.replace(",","")
        return k.toFixed(2)
    };
     $scope.totalGPC = function(){
       
        var k = ($scope.txtGPC.replace(",","") * 100) /  $scope.txtCarve.replace(",","")
        return k.toFixed(2)
    };
    $scope.totalDiscover = function(){
       
        var k = ($scope.txtDiscover.replace(",","") * 100) /  $scope.txtCarve.replace(",","")
        return k.toFixed(2)
    };
     $scope.totalOwn = function(){
       
        var k = ($scope.txtOwn.replace(",","") * 100) /  $scope.txtCarve.replace(",","")
        return k.toFixed(2)
    };
    $scope.totalOwnCarve = function(){
       
        var k = ($scope.txtCloud.replace(",","") - 0) + ($scope.txtGPC.replace(",","") - 0) + ($scope.txtDiscover.replace(",","") - 0) + ($scope.txtOwn.replace(",","")  - 0)
        return k;
    };
    $scope.totalbased = function(){
       
       var k = ($scope.txtValue1.replace(",","") * $scope.txtClassic.replace(",","")) /  100
        return k
       
    };
   $scope.totalbased5 = function(){
       var j=document.getElementById("txtMDF").value
       var k = $scope.txtClassic.replace(",","")- j
        return k
       
    };
    $scope.totalbasedCompare = function(){
      
       var k = ($scope.txtCarve.replace(",","") -0) + ($scope.txtCountry.replace(",","") -0) + ($scope.txtClassic.replace(",","") -0);
        return k
       
    };
    $scope.totalBudgetCompare = function(){
      
       var k = $scope.txtBudget.replace(",","")
        return k
       
    };
    $scope.totalMDFCalculation = function(){
      var totalMDF = $("#txtMDF_DES").text().replace(",","").replace("$","");
      var totalMDF1 = $("#txtMDF_DES1").text().replace(",","").replace("$","");
       var k =totalMDF/totalMDF1
        return k.toFixed(2);
       
    };
    $scope.totalRESCalculation = function(){
      var totalRES = $("#txtMDF_RES").text().replace(",","").replace("$","");
      var totalRES1 = $("#txtMDF_RES1").text().replace(",","").replace("$","");
       var k =totalRES/totalRES1
        return k.toFixed(2);
       
    };
   
     
    
});
 
 