/*
Dashboard Controller: This is the controller for default landing page
Created by: Aamin Khan
Created at: 19/01/2017
*/
angular.module("hpe").controller("tsCtrl", tsFn);

function tsFn($scope,$rootScope) {
    /*//ngInject
    $scope.activeMenu = 'MDF Strategy';
    $scope.txtCloud = "0";
    $scope.txtGPC = "0";
    $scope.txtDiscover = "0";
    $scope.txtOwn = "0";
    $scope.txtBudgetPer="100";
    $scope.txtCarve="0";
    $scope.txtCountry="0";
    $scope.txtClassic="0";


    var divName = "#hpsControl ";
  
        
    $scope.totalPer = function(){
        var k = ($scope.txtCarve.replace(",","") * 100) /  $scope.txtBudget.replace(",","");
        return Math.round(k);
        //return k.toFixed(2)
    };
    $scope.totalCon = function(){
        var k = ($scope.txtCountry.replace(",","") * 100) /  $scope.txtBudget.replace(",","");
        return Math.round(k);
        //return k.toFixed(2)
    };
    $scope.totalClassic = function(){
        var k = ($scope.txtClassic.replace(",","") * 100) /  $scope.txtBudget.replace(",","");
        return Math.round(k);
        //return k.toFixed(2);
    };
     $scope.totalBase1 = function(){
       
        var k = ($scope.txtBase1.replace(",","") * 100) /  $scope.txtClassic.replace(",","");
        return Math.round(k);
        //return k.toFixed(2)
    };
     
      $scope.totalCloud = function(){
       
        var k = ($scope.txtCloud.replace(",","") * 100) /  $scope.txtCarve.replace(",","");
        return Math.round(k);
        //return k.toFixed(2)
    };
     $scope.totalGPC = function(){
       
        var k = ($scope.txtGPC.replace(",","") * 100) /  $scope.txtCarve.replace(",","");
        return Math.round(k);
        //return k.toFixed(2)
    };
    $scope.totalDiscover = function(){
       
        var k = ($scope.txtDiscover.replace(",","") * 100) /  $scope.txtCarve.replace(",","");
        return Math.round(k);
        //return k.toFixed(2)
    };
     $scope.totalOwn = function(){
       
        var k = ($scope.txtOwn.replace(",","") * 100) /  $scope.txtCarve.replace(",","")
        return Math.round(k);
        //return k.toFixed(2)
    };
    $scope.totalOwnCarve = function(){
       
        var k = ($scope.txtCloud.replace(",","") - 0) + ($scope.txtGPC.replace(",","") - 0) + ($scope.txtDiscover.replace(",","") - 0) + ($scope.txtOwn.replace(",","")  - 0)
        return Math.round(k);
        //return k;
    };
    $scope.totalbased = function(){
       
       var k = ($scope.txtValue1.replace(",","") * $scope.txtClassic.replace(",","")) /  100
       return Math.round(k);
       //return k
       
    };
   $scope.totalbased5 = function(){
       
       var j = $(divName+"#txtMDF").val()
       var k = $scope.txtClassic.replace(",","")- j;
       return Math.round(k);
        //return k
       
    };
    $scope.totalbasedCompare = function(){
      
       var k = ($scope.txtCarve.replace(",","") -0) + ($scope.txtCountry.replace(",","") -0) + ($scope.txtClassic.replace(",","") -0);
        return Math.round(k);
        //return k
       
    };
    $scope.totalBudgetCompare = function(){
      
       var k = $scope.txtBudget.replace(",","")
        return Math.round(k);
        //return k
       
    };
    $scope.totalMDFCalculation = function(){
      var totalMDF = $(divName+"#txtMDF_DES").text().replace(",","").replace("$","");
      var totalMDF1 = $(divName+"#txtMDF_DES1").text().replace(",","").replace("$","");
       var k =totalMDF/totalMDF1
       return Math.round(k);
        //return k.toFixed(2);
       
    };
    $scope.totalRESCalculation = function(){
      var totalRES = $(divName+"#txtMDF_RES").text().replace(",","").replace("$","");
      var totalRES1 = $(divName+"#txtMDF_RES1").text().replace(",","").replace("$","");
       var k =totalRES/totalRES1
       return Math.round(k);
        //return k.toFixed(2);
       
    };

    $(".specialText").priceFormat();
   
    $rootScope.ts = [
      {name: 'Cloud',budget:'0',per:'0'},
      {name: 'GPC',budget:'0',per:'0'},
      {name: 'Discover',budget:'0',per:'0'}
    ];
    // $scope.txtCarve = 
    $scope.getVal = function(index){
      $rootScope.ts[index].per = (100   * $rootScope.ts[index].budget)/$scope.txtCarve;
      return Math.round($rootScope.ts[index].per);
    }

    $scope.remove = function(index){
      $rootScope.ts.splice(index,1);
    }

    $scope.add = function(){
      $rootScope.ts.push(
        {name: '',budget:'0',per:'0'}
      )
    }*/

}


