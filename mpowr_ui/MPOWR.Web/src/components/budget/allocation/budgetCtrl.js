angular.module("hpe").controller("budgetCtrl", bdg);

function bdg($scope,$rootScope,budgetFactory,$timeout,Notification,$sessionStorage,authService,$localStorage) {

    authService.checkUser();


	budgetFactory.getBudget().then(
		function onSuccess(response) {
		    // Handle success
		    var data = response.data;
		    var status = response.status;
		    var statusText = response.statusText;
		    var headers = response.headers;
		    var config = response.config;

		    $scope.BUObj = budgetFactory.generateJSON(data);
		    $scope.budgets = $scope.BUObj.BuUnits;
		    $scope.getTotal();
		  }, 

		  function onError(response) {
		    // Handle error
		    var data = response.data;
		    var status = response.status;
		    var statusText = response.statusText;
		    var headers = response.headers;
		    var config = response.config;
		    
		  })
	$scope.summaries = [];

	$scope.resellerPartner = $sessionStorage.resellerPartner;
	


	//for comma seperated currency format
	$timeout(function(){
		$(".specialText").priceFormat();
	},0)

	

	//remove a project

	$scope.remove = function(parentIndex,currentIndex){
		var project = $scope.budgets[parentIndex].CarveProjects[currentIndex];
		project.Flag = 'DL',
		project.per = 0;
		project.ProjectMDF = 0;
		//$scope.budgets[parentIndex].CarveProjects.splice(currentIndex,1);
		$scope.getTotal();
	}

	// curve outs total callucation on change
	$scope.curveOutsCalc = function(index){

		var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
	
		if(budgetObj.ProgramMDF > (budgetObj.TotalMDF - (budgetObj.CountryReserveMDF + budgetObj.BaselineMDF) ) ){
			//if it exceds
			Notification.warning('Please enter a lesser value!');
			budgetObj.ProgramMDF = budgetObj.ProgramMDF.toString();
			budgetObj.ProgramMDF = parseInt(budgetObj.ProgramMDF.substring(0, budgetObj.ProgramMDF.length-1));
		}else{
			//update the project budgets
			for(var i=0;i<budgetObj.CarveProjects.length;i++){
				
				budgetObj.CarveProjects[i].per = budgetFactory.formatPercentage(( budgetObj.CarveProjects[i].ProjectMDF / budgetObj.ProgramMDF ) * 100);

			}
		}

		budgetObj.curvePer = budgetFactory.formatPercentage((budgetObj.ProgramMDF/budgetObj.TotalMDF) * 100);
		$scope.getTotal();
		$scope.getSummary();
	}
	
	// Country Reserve MDF changed 

	$scope.countryResCalc = function(index){

		var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
	
		if(budgetObj.CountryReserveMDF > (budgetObj.TotalMDF - (budgetObj.ProgramMDF + budgetObj.BaselineMDF) ) ){
			Notification.warning('Please enter a lesser value!');
			budgetObj.CountryReserveMDF = budgetObj.CountryReserveMDF.toString();
			budgetObj.CountryReserveMDF = parseInt(budgetObj.CountryReserveMDF.substring(0, budgetObj.CountryReserveMDF.length-1));
		}
		
		budgetObj.countryPer = budgetFactory.formatPercentage((budgetObj.CountryReserveMDF/budgetObj.TotalMDF) * 100);
		$scope.getTotal();
	}

	// basline mdf

	$scope.baselineMdfCalc = function(index){
		var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
	
		if(budgetObj.BaselineMDF > (budgetObj.TotalMDF - (budgetObj.ProgramMDF + budgetObj.CountryReserveMDF) ) ){
			Notification.warning('Please enter a lesser value!');
			budgetObj.BaselineMDF = budgetObj.BaselineMDF.toString();
			budgetObj.BaselineMDF = parseInt(budgetObj.BaselineMDF.substring(0, budgetObj.BaselineMDF.length-1));
		}
		
		budgetObj.baslinePer = budgetFactory.formatPercentage((budgetObj.BaselineMDF/budgetObj.TotalMDF) * 100);
		$scope.getTotal();
	}

	$scope.setTotalBudget = function(index){
		var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[index]);
		budgetObj.curvePer = budgetFactory.formatPercentage((budgetObj.ProgramMDF/budgetObj.TotalMDF) * 100);
		budgetObj.countryPer = budgetFactory.formatPercentage((budgetObj.CountryReserveMDF/budgetObj.TotalMDF) * 100);
		budgetObj.baslinePer = budgetFactory.formatPercentage((budgetObj.BaselineMDF/budgetObj.TotalMDF) * 100);
		$scope.getTotal();
	}

	$scope.calculateProjectPer = function(parentIndex,currentIndex){
		var budgetObj = budgetFactory.toBudgetFormat($scope.budgets[parentIndex]);
		var project = budgetObj.CarveProjects[currentIndex];
		var total = 0;
		for(var i=0;i<budgetObj.CarveProjects.length;i++){
			total += budgetFactory.toFormat(budgetObj.CarveProjects[i].ProjectMDF);
		}
		if(total > budgetObj.ProgramMDF){
			Notification.warning('You cannot add a value which exits the total cost');

			project.ProjectMDF = budgetFactory.toFormat(project.ProjectMDF).toString();

			project.ProjectMDF = parseInt(project.ProjectMDF.substring(0, project.ProjectMDF.length-1));
		}
		project.per = budgetFactory.formatPercentage((budgetFactory.toFormat(project.ProjectMDF) / budgetObj.ProgramMDF) * 100);
		project.Flag = 'UP';
		$scope.getTotal();
	}

	$scope.getTotal = function(){
		if(typeof $scope.budgets != 'undefined'){
			$scope.MDFGrossTotal = 0;
			$scope.pmcGrossTotal = 0;
			$scope.crmGrossTotal = 0;
			$scope.bcmGrossTotal = 0;
			for(var i=0;i< $scope.budgets.length;i++){
				$scope.MDFGrossTotal += budgetFactory.toFormat($scope.budgets[i].TotalMDF);
				$scope.pmcGrossTotal += budgetFactory.toFormat($scope.budgets[i].ProgramMDF);
				$scope.crmGrossTotal += budgetFactory.toFormat($scope.budgets[i].CountryReserveMDF);
				$scope.bcmGrossTotal += budgetFactory.toFormat($scope.budgets[i].BaselineMDF);
			}

			//pecentages
			$scope.pmcPer = budgetFactory.formatPercentage(($scope.pmcGrossTotal/$scope.MDFGrossTotal)*100);
			$scope.crmPer = budgetFactory.formatPercentage(($scope.crmGrossTotal/$scope.MDFGrossTotal)*100);
			$scope.bcmPer = budgetFactory.formatPercentage(($scope.bcmGrossTotal/$scope.MDFGrossTotal)*100);
			$scope.mdfTotalPer = budgetFactory.formatPercentage($scope.pmcPer + $scope.crmPer + $scope.bcmPer);
			$scope.getSummary();
		}
		
	};


	$scope.getSummary = function(){
		var newArr = [];
		for(var i = 0;i<$scope.budgets.length; i++){
			newArr = newArr.concat($scope.budgets[i].CarveProjects)
		}
		var groupedData = _.groupBy(newArr, function(d){return d.ProjectName});
		
		//now sum it
		var sumary = [];
		$.each(groupedData, function(k, v) {
			var ox = {};
			var total =0;
			var flag = 'DF';
		    for(var i=0;i<v.length;i++){
		    	total += budgetFactory.toFormat(v[i].ProjectMDF);
		    	flag = v[i].Flag;
		    }
		    ox.ProjectName=k;
		    ox.ProjectMDF = total;
		    ox.per = budgetFactory.formatPercentage((total/$scope.pmcGrossTotal) * 100);
		    if(flag != 'DL'){
		    	sumary.push(ox);
		    }
		});
		$scope.summaries = sumary;
		
	}

	$scope.expand = function(index,id){
		var elem = $("#"+id+index);
		if(id == 'total_'){
			if(elem.attr("class").indexOf('fa-chevron-down') > -1){
				elem.removeClass('fa-chevron-down').addClass('fa-chevron-up');
				//$("#add_"+index).show();
				$("#totalData").show();
			}else{
				elem.removeClass('fa-chevron-up').addClass('fa-chevron-down')
				//$("#add_"+index).hide();
				$("#totalData").hide();
			}
		}else{
			if(elem.attr("class").indexOf('fa-chevron-down') > -1){
				elem.removeClass('fa-chevron-down').addClass('fa-chevron-up');
				//$("#add_"+index).show();
				$("#budget-body-"+index).show();
			}else{
				elem.removeClass('fa-chevron-up').addClass('fa-chevron-down')
				//$("#add_"+index).hide();
				$("#budget-body-"+index).hide();
			}
		}
		
	}

	// add new project
	$scope.add = function(index){
		$scope.budgets[index].CarveProjects.push({
			ProjectName: '',
			ProjectMDF:'0',
			per:'0',
			Flag: 'IN'
		});
		var elem = $("#"+'exp_'+index);
		elem.removeClass('fa-chevron-down').addClass('fa-chevron-up');
		$("#budget-body-"+index).show();
		$timeout(function(){
			$(".specialText").priceFormat();
		},0)
		
	};

	//save

	$scope.save = function(){
		var flag = false;
		$(".specialText, .project-inp").each(function(key,val){
			// if the element having value and not hidden (hidden means deleted from json)
			if( ($(this).val() == "" || $(this).val() == '0' || $(this).val() == 0) &&  $(this).is(":visible")) {
				$(this).css('border-color','red');
				flag = true;
			}else{
				$(this).css('border-color','#ddd');
			}
		})

		if(flag){
			Notification.error({message: 'Please complete the form', delay: null});
		}else{
			//save it
			$scope.BUObj.UserName = $localStorage.user.UserName;
			budgetFactory.persist($scope.BUObj).then(
					function onSuccess(response) {
						Notification.success("Budget Saved successfully!");				    
			  		}, 

				  function onError(response) {
				   	 Notification.error("Could not save the data!");
				    
				  }
			)
		}
	}


	/*$timeout(function(){
		$(".project-inp").on('keyup', function(){
			if($(this).val() != "" || parseInt($(this).val()) > 0){
					$(this).removeClass('error-inp');
			}
		})
	},10)
*/
}