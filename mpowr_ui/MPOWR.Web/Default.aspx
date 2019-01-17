<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="MPOWR.Web.index" %>

<!DOCTYPE html>

<html ng-app="hpe">
<head>
    <title>HPE</title>
    <link rel="shortcut icon" href="favicon.ico" />

    <%
        string version = ConfigurationSettings.AppSettings["version"].ToString();
    %>

    <script>
        var version = '<%=ConfigurationSettings.AppSettings["version"].ToString() %>';
        
    </script>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <%--<meta http-equiv="Content-Security-Policy" content="default-src *; style-src 'self' 'unsafe-inline'; script-src
    'self' 'unsafe-inline' 'unsafe-eval' https://mpowr-webdev.azurewebsites.net">--%>
    <meta http-equiv="Content-Security-Policy" content="default-src *; style-src 'self' 'unsafe-inline'; script-src
    'self' 'unsafe-inline' 'unsafe-eval' https://mpowr-webtest.azurewebsites.net">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <meta name="description" content="">
    <meta name="author" content="">
    <meta name="viewport" content="width=device-width,minimum-scale=1,initial-scale=1">


    <!-- Library lins -->

    <link href="node_modules/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="node_modules/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="assets/css/style.css?v=<%=version%>" rel="stylesheet" />
    <link href="assets/css/custom.css?v=<%=version%>" rel="stylesheet" />
    <link href="assets/css/app.css?v=<%=version%>" rel="stylesheet" />
    <link href="assets/css/material-input.css" rel="stylesheet" />
    <link href="assets/css/checkbox.css" rel="stylesheet" />
    <script src="node_modules/jquery/dist/jquery.min.js"></script>




    <%-- <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>--%>
    <link href="node_modules/jquery-ui-themes-1.12.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <script src="node_modules/jquery-ui-1.12.1/jquery-ui.min.js"></script>

    <script src="node_modules/moment/min/moment.min.js"></script>
    <script src="node_modules/angular/angular.min.js"></script>
    <script src="node_modules/angular-ui-router/release/angular-ui-router.min.js"></script>
    <script src="node_modules/bootstrap/dist/js/bootstrap.min.js"></script>
    <link href="node_modules/angular-ui-notification/dist/angular-ui-notification.css" rel="stylesheet" />
    <script src="node_modules/angular-ui-notification/dist/angular-ui-notification.min.js"></script>
    <script src="node_modules/angular-ui-bootstrap/dist/ui-bootstrap.js"></script>
    <script src="node_modules/angular-ui-bootstrap/dist/ui-bootstrap-tpls.js"></script>
    <link href="node_modules/angular-ui-bootstrap/dist/ui-bootstrap-csp.css" rel="stylesheet" />
    <script src="node_modules/underscore/underscore-min.js"></script>
    <script src="node_modules/ngstorage/ngStorage.min.js"></script>
    <script src="node_modules/bootbox/bootbox.min.js"></script>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/lodash.js/3.7.0/lodash.min.js"></script>--%>
    <script src="node_modules/lodash/lodash.min.js"></script>
    <!--<script src="node_modules/lodash/dist/lodash.min.js"></script>-->

    <script src="node_modules/d3/d3.js"></script>
    <script src="node_modules/nvd3/build/nv.d3.js"></script>
    <link rel="stylesheet" type="text/css" href="node_modules/nvd3/build/nv.d3.css">
    <script src="node_modules/angular-nvd3/dist/angular-nvd3.min.js"></script>
    <script src="node_modules/angucomplete-alt/dist/angucomplete-alt.min.js"></script>
    <link rel="stylesheet" href="node_modules/angucomplete-alt/dist/angucomplete-alt.css">
    <script src="node_modules/ng-idle/angular-idle.min.js"></script>
    <script src="node_modules/angular-ui-grid/ui-grid.js"></script>
    <link rel='stylesheet' href='node_modules/textangular/dist/textAngular.css'>
    <link rel="stylesheet" href="node_modules/angular-ui-grid/ui-grid.css" type="text/css" />


    <script src="node_modules/ui-grid-draggable-rows/js/draggable-rows.js"></script>
    <link href="node_modules/angular-ui-grid/ui-grid.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="node_modules/angular-loading-bar/build/loading-bar.css">
    <script src="node_modules/angular-loading-bar/build/loading-bar.min.js"></script>
    <script src="node_modules/angular-sanitize/angular-sanitize.js"></script>
    <script src='node_modules/textangular/dist/textAngular-rangy.min.js'></script>
    <script src='node_modules/textangular/dist/textAngular-sanitize.min.js'></script>
    <script src='node_modules/textangular/dist/textAngular.min.js'></script>
    <!-- End of Library links -->
    <!-- Dependency modules -->
    <script type="text/javascript" src="modules/nav/nav.module.js?v=<%=version%>"></script>
    <link rel="stylesheet" type="text/css" href="modules/nav/nav.css?v=<%=version%>">

    <script type="text/javascript" src="modules/leftbar/leftbar.module.js?v=<%=version%>"></script>
    <link rel="stylesheet" type="text/css" href="modules/leftbar/leftbar.css?v=<%=version%>">

    <!-- End of Dependency modules -->
    <!-- Application Files -->
    <script src="app.module.js?v=<%=version%>"></script>
    <script src="app.route.js?v=<%=version%>"></script>
    <script src="app.config.js?v=<%=version%>"></script>
    <script src="app.run.js?v=<%=version%>"></script>
    <script src="services/tokenService.js?v=<%=version%>"></script>
    <script src="components/login/app.loginCtrl.js?v=<%=version%>"></script>
    <script src="components/dashboard/app.dashboardCtrl.js?v=<%=version%>"></script>
    <script src="components/budget/allocation/budgetCtrl.js?v=<%=version%>"></script>
    <script src="components/budget/prebudgetCtrl.js?v=<%=version%>"></script>
    <script src="libs/jquery.priceformat.js?v=<%=version%>"></script>
    <script src="directives/breadcrumb/breadCrumbFactory.js?v=<%=version%>"></script>
    <script src="directives/breadcrumb/breadcrumbBar.js?v=<%=version%>"></script>
    <script src="components/budget/allocation/budgetFactory.js?v=<%=version%>"></script>
    <script src="services/loginService.js?v=<%=version%>"></script>
    <script src="services/authService.js?v=<%=version%>"></script>
    <script src="services/UtilityService.js?v=<%=version%>"></script>
    <script src="factories/countryParnerFactoy.js?v=<%=version%>"></script>
    <script src="services/roleMappingService.js?v=<%=version%>"></script>
    <script src="factories/adminFactory.js?v=<%=version%>"></script>
    <!-- ModelParameter -->
    <script src="components/budget/model-parameters/ModelParamCtrl.js?v=<%=version%>"></script>
    <script src="components/budget/model-parameters/modelParameterFactory.js?v=<%=version%>"></script>
    <!-- HISTORICAL -->
    <script src="components/budget/historical/historicalCtrl.js?v=<%=version%>"></script>
    <script src="components/budget/historical/historicalFactory.js?v=<%=version%>"></script>
    <link href="components/budget/partner-budget/full-view/full.css?v=<%=version%>" rel="stylesheet" />
    <link href="components/budget/partner-budget/summary-view/compact.css?v=<%=version%>" rel="stylesheet" />
    <script src="components/budget/partner-budget/partnerCreditCtrl.js?v=<%=version%>"></script>
    <script src="components/admin/bu-management/bu-management.js"></script>
    <script src="components/budget/partner-budget/partnerBudgetDataService.js?v=<%=version%>"></script>
    <script src="components/budget/partner-budget/partnerCreditRound2Ctrl.js?v=<%=version%>"></script>
    <script src="components/budget/partner-budget/partnerCreditFactory.js?v=<%=version%>"></script>
    <script src="components/budget/partner-budget/graphFactory.js?v=<%=version%>"></script>
    <script src="components/budget/partner-budget/partnerCreditRound2Factory.js?v=<%=version%>"></script>
    <script src="components/budget/final-summary/finalSummaryController.js?v=<%=version%>"></script>
    <script src="components/budget/final-summary/summaryFactory.js?v=<%=version%>"></script>
    <script src="components/budget/final-summary/fsGraphFactory.js?v=<%=version%>"></script>
    <script src="components/budget/doc/documentController.js?v=<%=version%>"></script>


    <script src="components/versioning/versioningctrl.js?v=<%=version%>"></script>
    <script src="components/versioning/versioningservice.js?v=<%=version%>"></script>
    <script src="components/versioning/show-version/showVersion.js?v=<%=version%>"></script>

    <!-- Phase II Admin Modules  -->
    <script src="components/admin/role-management/roleCtrl.js?v=<%=version%>"></script>
    <script src="components/admin/set-milestones/setMilestoneCtrl.js?v=<%=version%>"></script>
    <script src="components/admin/set-milestones/setMilestoneFactory.js?v=<%=version%>"></script>
    <script src="components/admin/guardrial-setting/guardraliCtrl.js?v=<%=version%>"></script>
    <!-- End of Phase II -->

    <script src="directives/glossary/glossary.js?v=<%=version%>"></script>
    <script src="directives/docs/documentController.js?v=<%=version%>"></script>
    <!-- End of Application Files -->
    <!-- End of Application Files -->
    <!-- USER MANGEMENT -->
    <!-- <script src="directives/admin/adminTabBar.js"></script> -->
    <script src="components/admin/user-management/userManagementCtrl.js?v=<%=version%>"></script>
    <script src="components/admin/user-management/userManagementFactory.js?v=<%=version%>"></script>
    <script src="directives/multiselect/multiselect.js?v=<%=version%>"></script>
    <script src="components/admin/model-parameter/modelCtrl.js?v=<%=version%>"></script>
    <script src="directives/userManagementMultiSelect/umMultiSelect.js?v=<%=version%>"></script>
    <script src="components/admin/geo-management/geo-management.js?v=<%=version%>"></script>
    <script src="libs/DemoCtrl.js?v=<%=version%>"></script>
    <%--<script src="components/import/promonew.modal.js"></script>--%>
    <%-- Glossary Management --%>
    <script src="components/admin/Glossary-management/Glossary-management.js?v=<%=version%>"></script>
    <link href="components/admin/Glossary-management/Color-specturm/ColorSpecturmCustom.css?v=<%=version%>" rel="stylesheet" />
    <script src="node_modules/angular-spectrum-colorpicker/dist/angular-spectrum-colorpicker.js"></script>
    <script src="node_modules/angular-spectrum-colorpicker/dist/angular-spectrum-colorpicker.min.js"></script>
    <link href="components/admin/Glossary-management/Color-specturm/Specturm.css?v=<%=version%>" rel="stylesheet" />
    <script src="components/admin/Glossary-management/Color-specturm/Specturm.js?v=<%=version%>"></script>
</head>
<body class="container-fluid">
    <!-- body overlay -->


    <div class="body-overlay default-loader" id="commonLoader">
        <div class="loading-block">
            <img class="loadingImage" src="assets/images/icons/loading-dots.gif" />

        </div>
    </div>
    <!-- body pverlay ends -->
    <!-- MDF allocation body overlay -->
    <div class="body-overlay default-loader" id="mdf-loader">
        <div class="loading-block">
            <img class="loadingImage" src="assets/images/icons/loading-animation.gif" />
        </div>
    </div>
    <div class="body-overlay default-loader" id="mdf_loader">
        <div class="loading-block">
            <img class="loadingImage" src="assets/images/icons/loading-dots.gif" />
        </div>
    </div>
    <!-- MDF allocation body pverlay ends -->

    <div ui-view>
    </div>
    <glossary>

    </glossary>
    <document>

    </document>
    <!-- IDLE -->

    <section data-ng-controller="DemoCtrl">
        <!-- <p>
        <button type="button" class="btn btn-success" data-ng-hide="started" data-ng-click="start()">Start Demo</button>
        <button type="button" class="btn btn-danger" data-ng-show="started" data-ng-click="stop()">Stop Demo</button>
      </p> -->
    </section>

    <script type="text/ng-template" id="warning-dialog.html">
      <div class="modal-header">
       <h3>You're Idle. Do Something!</h3>
      </div>
      <div idle-countdown="countdown" ng-init="countdown=5" class="modal-body">
       <uib-progressbar max="5" value="5" animate="false" class="progress-striped active">You'll be logged out in {{countdown}} second(s).</uib-progressbar>
      </div>

    </script>
    <script type="text/ng-template" id="timedout-dialog.html">
      <div class="modal-header">
       <h3>You've Timed Out!</h3>
      </div>
      <div class="modal-body">
       <p>
          You have logged out due to inactivity, please login again to continue!
       </p>
     </div>
    </script>


    <!-- IDLE -->

</body>
</html>


