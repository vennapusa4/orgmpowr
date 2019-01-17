(function(){
	'use strict';
	angular.module('hpe').service('UtilityService', utilityServiceFn);

	function utilityServiceFn($localStorage, $state, $rootScope, $http, $sessionStorage) {
			//ngInject
			
		this.JSONToCSVConvertor =  function(JSONData, ReportTitle, ShowLabel){
			 //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
		    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
		    
		    var CSV = '';    
		    //Set Report title in first row or line
		    
		    CSV += ReportTitle + '\r\n\n';

		    //This condition will generate the Label/Header
		    if (ShowLabel) {
		        var row = "";
		        
		        //This loop will extract the label from 1st index of on array
		        for (var index in arrData[0]) {
		            
		            //Now convert each value to string and comma-seprated
		            row += index + ',';
		        }

		        row = row.slice(0, -1);
		        
		        //append Label row with line break
		        CSV += row + '\r\n';
		    }
		    
		    //1st loop is to extract each row
		    for (var i = 0; i < arrData.length; i++) {
		        var row = "";
		        
		        //2nd loop will extract each column and convert it in string comma-seprated
		        for (var index in arrData[i]) {
		            row += '"' + arrData[i][index] + '",';
		        }

		        row.slice(0, row.length - 1);
		        
		        //add a line break after each row
		        CSV += row + '\r\n';
		    }

		    if (CSV == '') {        
		        alert("Invalid data");
		        return;
		    }   
		    
		    //Generate a file name
		    var fileName = "MyReport_";
		    //this will remove the blank-spaces from the title and replace it with an underscore
		    fileName += ReportTitle.replace(/ /g,"_");   
		    
		    //Initialize file format you want csv or xls
		    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);
		    
		    // Now the little tricky part.
		    // you can use either>> window.open(uri);
		    // but this will not work in some browsers
		    // or you will not get the correct file extension    
		    
		    //this trick will generate a temp <a /> tag
		    var link = document.createElement("a");    
		    link.href = uri;
		    
		    //set the visibility hidden so it will not effect on your web-layout
		    link.style = "visibility:hidden";
		    link.download = fileName + ".csv";
		    
		    //this part will append the anchor tag and remove it after automatic click
		    document.body.appendChild(link);
		    link.click();
		    document.body.removeChild(link);
		},
		this.sortBy = function(arr,superKey,key,sortBy){ //[] name asc
			if(sortBy == 'asc' || typeof sortBy == 'undefined'){

				if(superKey.length == 0)
					return _.sortBy(arr, key);
				else
					return _.sortBy(arr, function(obj) { return obj[superKey][0][key]; });
					
			}else{
				if(superKey.length == 0)
					return _.sortBy(arr, key).reverse();
				else
					return _.sortBy(arr, function(obj) { return obj[superKey][0][key]; }).reverse();;
			}
			
		},
        this.sortForUserManagement = function (arr, key, sortBy) {
            var arrData = [];
            var arrData1 = [];
            var umData = {};
            _.each(arr, function (obj) { arrData.push(obj.user) })
            if (sortBy == 'asc') {
                    _.each(_.sortBy(arrData, key).reverse(), function (obj) { var umData = {}; umData.user = obj; arrData1.push(umData) });
                    return arrData1;

            } else {
                    _.each(_.sortBy(arrData, key).reverse(), function (obj) { var umData = {}; umData.user = obj; arrData1.push(umData) });
                    return arrData1.reverse();
            }

        },
        this.sortForVersionManagement = function (arr, key, sortBy) {
            var arrData = [];
            var arrData1 = [];
            var umData = {};
            _.each(arr, function (obj) { arrData.push(obj) })
            if (sortBy == 'asc') {
                _.each(_.sortBy(arrData, key).reverse(), function (obj) { var umData = {}; umData = obj; arrData1.push(umData) });
                return arrData1;

            } else {
                _.each(_.sortBy(arrData, key).reverse(), function (obj) { var umData = {}; umData = obj; arrData1.push(umData) });
                return arrData1.reverse();
            }

        },
        this.sortForUserManual = function (arr, key, sortBy) {
            var arrData = [];
            var arrData1 = [];
            var umData = {};
            _.each(arr, function (obj) { arrData.push(obj) })
            if (sortBy == 'asc') {
                    _.each(_.sortBy(arrData, key).reverse(), function (obj) { var umData = {}; umData = obj; arrData1.push(umData) });
                    return arrData1;

            } else {
                    _.each(_.sortBy(arrData, key).reverse(), function (obj) { var umData = {}; umData = obj; arrData1.push(umData) });
                    return arrData1.reverse();
            }

        },
		this.multiSort = function(arr,superKey,key1,key2,sortBy){ //[] name asc
			if(sortBy == 'asc' || typeof sortBy == 'undefined'){

				var sortedArray = _(arr).chain().sortBy(function(obj) {
				    return obj[superKey][0][key1];
				}).sortBy(function(obj) {
				    return obj[superKey][0][key2];
				}).value();
				return sortedArray
			}else{
				var revSortedArray = _(arr).chain().sortBy(function(obj) {
				    return obj[superKey][0][key1];
				}).sortBy(function(obj) {
				    return obj[superKey][0][key2];
				}).value();
				return revSortedArray.reverse();
			}
			
		}

		this.assesmentSort = function(arr,toSort){
			
			// red sort
			var unsort = [];
			var sort = [];
			

			for(var i=0;i<arr.length;i++){
				var val = arr[i];

				if(val.MDF[0].MDF_Alignment == "Misaligned" && val.MDF[0].PREV_MDF_Assessment == "Overfunded"){
					sort.push(val);
				}else{
					unsort.push(val);
				}
			}

			var unsort2 = [];
			var sort2 = [];

			for(var i=0;i<unsort.length;i++){
				var val = unsort[i];
				if(val.MDF[0].MDF_Alignment == "Misaligned" && val.MDF[0].PREV_MDF_Assessment == "Underfunded"){
					sort2.push(val);
				}else{
					unsort2.push(val);
				}
			}
			
			var unsort3 = [];
			var sort3 = [];

			for(var i=0;i<unsort2.length;i++){
				var val = unsort2[i];
				if((val.MDF[0].MDF_Alignment == "Aligned" && val.MDF[0].PREV_MDF_Assessment == "Overfunded") || (val.MDF[0].MDF_Alignment == "Aligned" && val.MDF[0].PREV_MDF_Assessment == "Underfunded")){
					sort3.push(val);
				}else{
					unsort3.push(val);
				}
			}
			

			var sorted = ((sort.concat(sort2)).concat(sort3)).concat(unsort3)
			
			if(toSort == 'asc'){
				sorted = sorted.reverse();
			}

			return sorted;

			
		}

		this.getLastUpdatedTime = function (countryId) {
		    var VersionID;
		    if ($sessionStorage.currentVersion == undefined || $sessionStorage.currentVersion == null || $sessionStorage.currentVersion.Version == undefined || $sessionStorage.currentVersion.Version == null || $sessionStorage.currentVersion.Version.length === 0) {

		        VersionID = 0;
		    }
		    else {
		        VersionID = $sessionStorage.currentVersion.Version[$sessionStorage.currentVersion.Version.length - 1].VersionID
		    }
		    $http({
		        method: 'GET',
		        url: $rootScope.api + 'Users/GetLastUpdated?VersionId=' + VersionID

		    }).then(function (resp) {
		        $sessionStorage.lastUpdatedDate = resp.data;
		        $rootScope.$broadcast("lastUpdatedTimeUpdated", resp.data)
		    },
		    function onError(response) {

		    });
		}




	}
})();