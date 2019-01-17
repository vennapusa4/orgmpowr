/*
	Name: 			BreadCrumb Factory
	Developed by: 	Aamin Khan
	Created at: 	29 Mar 2017
*/

(function () {
    "use strict";
    angular.module("hpe").factory("breadCrumbFactory", breadcrumbFn);

    function breadcrumbFn(roleMappingService) {
        return {
            admin: function ($state, $scope, $localStorage) {
                var pointer = 0;
                $scope.menus = [
					{
					    name: 'User Management',
					    url: 'admin.user-management',
					    active: false,
					    enabled: true
					},
					{
					    name: 'Role Management',
					    url: 'admin.role-management',
					    active: false,
					    enabled: true
					},
                    {
                        name: 'Glossary Management',
                        url: 'admin.Glossary-management',
                        active: false,
                        enabled: true
                    },
                     {
                         name: 'BU Configuration',
                         url: 'admin.geo-management',
                         active: false,
                         enabled: true
                     }
                    //,
					//{
					//    name: 'Guardrail Settings',
					//	url: 'admin.guardrial-setting',
					//	active: false,
					//	enabled: true
					//},
					//{
					//	name: 'Set Milestones',
					//	url: 'admin.set-milestones',
					//	active: false,
					//	enabled: true
					//},
					//{
					//	name: 'Model Parameter Settings',
					//	url: 'admin.model-parameter',
					//	active: false,
					//	enabled: true
					//}

                ];




                switch ($state.current.name) {
                    case 'admin.user-management':
                        pointer = 0;
                        $scope.menus[pointer].active = true;
                        break
                    case 'admin.role-management':
                        pointer = 1;
                        $scope.menus[pointer].active = true;
                        break
                    case 'admin.Glossary-management':
                        pointer = 2;
                        $scope.menus[pointer].active = true;
                        break
                    case 'admin.geo-management':
                        pointer = 3;
                        $scope.menus[pointer].active = true;
                        break

                        //case 'admin.guardrial-setting':
                        //		pointer = 2;
                        //		$scope.menus[pointer].active = true;
                        //		break
                        //case 'budget.partner-budget':
                        //		pointer = 3;
                        //		$scope.menus[pointer].active = true;
                        //		break;
                        //case 'admin.set-milestones':
                        //		pointer = 3;
                        //		$scope.menus[pointer].active = true;
                        //		break;
                        //case 'admin.model-parameter':
                        //    pointer = 4;
                        //    $scope.menus[pointer].active = true;
                        //    break;
                }
            },
            //update: function ($http, $scope, $rootScope, $localStorage) {
            //    console.log($localStorage.user.UserID);
            //    $http({
            //        method: 'GET',
            //        url: $rootScope.api + 'LastUpdated/GetLastUpdated?userid=' + $localStorage.user.UserID + '&partnerid=' + $localStorage.PartnerTypeID

            //    }).then(function (resp) {

            //        $rootScope.UpdatedDate = resp.data;

            //    });
            //},
            portal: function ($state, $scope, $localStorage) {
                var pointer = 0;
                $scope.menus = roleMappingService.getMenu();


                /*$scope.menus = [
					{
						name: 'Budget Settings',
						url: 'budget.allocation',
						active: false,
						enabled: true
					},
					{
						name: 'Historical Performance',
						url: 'budget.historical-perfomance',
						active: false,
						enabled: true
					},
					{
						name: 'Model Parameters',
						url: 'budget.model-parameter',
						active: false,
						enabled: true
					},
					{
						name: 'Partner Budget',
						url: 'budget.partner-budget',
						active: false,
						enabled: true
					},
					{
						name: 'Final summary',
						url: 'budget.final-summary',
						active: false,
						enabled: true
					}

				];*/
                var user = $localStorage.user;
                if (user.UserTypeID == 2) {
                    /*$scope.menus = [
						{
							name: 'Budget Settings',
							url: 'budget.allocation',
							active: false,
							enabled: false
						},
						{
							name: 'Historical Performance',
							url: 'budget.historical-perfomance',
							active: false,
							enabled: false
						},
						{
							name: 'Model Parameters',
							url: 'budget.model-parameter',
							active: false,
							enabled: false
						},
						{
							name: 'Partner Budget',
							url: 'budget.partner-budget',
							active: false,
							enabled: true
						},
						{
							name: 'Final summary',
							url: 'budget.final-summary',
							active: false,
							enabled: true
						}
					]*/
                }

                //$scope.menus[pointer].active = false;

                switch ($state.current.name) {
                    case 'budget.versioning':
                        pointer = 0;
                        $scope.menus[pointer].active = true;
                        break;
                    case 'budget.allocation':
                        pointer = 1;
                        $scope.menus[pointer].active = true;
                        break
                    case 'budget.historical-perfomance':
                        pointer = 2;
                        $scope.menus[pointer].active = true;
                        break
                    case 'budget.model-parameter':
                        pointer = 3;
                        $scope.menus[pointer].active = true;
                        break
                    case 'budget.partner-budget':
                        pointer = 4;
                        $scope.menus[pointer].active = true;
                        break;
                    case 'budget.partner-budget-round2':
                        pointer = 4;
                        //pointer = user.UserTypeID == 2 ? 0 : 3;
                        $scope.menus[pointer].active = true;
                        break;
                    case 'budget.final-summary':
                        pointer = 5;
                        $scope.menus[pointer].active = true;
                        break;
                }
            }
        }
    }

})();