(function () {
    'use strict';
    angular.module("hpe").controller('GlossaryManagementCtrl', GlossaryManagementCtrlFn)
    function GlossaryManagementCtrlFn($scope, $filter, $http, $state, $timeout, $rootScope, $localStorage, Notification, $uibModal, textAngularManager) {

        if ($localStorage.user.GlossaryApprover == null)
            $localStorage.user.GlossaryApprover = false;
        var isApprover = $localStorage.user.GlossaryApprover;

        var DescriptionTemplate = '<div id={{row.entity.$$hashKey}}ct1 ng-click="grid.appScope.EditParam(row,false,8000)"><p ng-bind-html=row.entity.Description></p><div id={{row.entity.$$hashKey}}et1 ng-if="row.entity.IsAltered==1 || row.entity.IsAltered==2"><br/><span><b class="heading-color">Suggested:</b></span><br/><p ng-bind-html=row.entity.EditedDesc></p></div>';
        var imgTemplate = '<img  title="Delete Parameter" ng-click="grid.appScope.DeleteParam(row.entity)" src="assets/resources/delete_BU@1x.png" class="ng-scope glossary-delete-icon">';

        var deleteTemplate = '<div ng-if="row.entity.IsAltered==0 || row.entity.IsAltered==2"><input type="checkbox" ng-click="grid.appScope.DeleteAction(row.entity)"  ng-checked=row.entity.IsDeleted id={{row.entity.$$hashKey}}dt1><label for={{row.entity.$$hashKey}}dt1 class="glossary-delete-checkbox left-margin10 top-margin10" ><span class ="check-label"></span></label></div>';

        var approvalTemplate = '<div ng-if="row.entity.IsAltered==1 || row.entity.IsAltered==2">' +
            '<i class="fa-lg fa fa-check-circle-o glossary-icon-gray left-margin10 top-margin10" title="Approve" ng-show="row.entity.showApproval && (row.entity.IsApproved==null || row.entity.IsApproved==0)"  ng-click="row.entity.IsApproved=1;row.entity.IsDeleted=false;grid.appScope.$parent.$parent.Saved=false;"></i>' +
            '<i class="fa-lg fa fa-check-circle-o glossary-icon-green left-margin10 top-margin10" title="Approve" ng-show="row.entity.showApproval && row.entity.IsApproved==1"></i>' +
            '<i class="fa-lg fa fa-ban glossary-icon-warning  left-margin10 top-margin10" title="Reject" ng-show="row.entity.showApproval && row.entity.IsApproved==0"></i>' +
            '<i class="fa-lg fa fa-ban glossary-icon-gray left-margin10 top-margin10" title="Reject" ng-show="row.entity.showApproval && (row.entity.IsApproved==null || row.entity.IsApproved==1)" ng-click="row.entity.IsApproved=0;row.entity.IsDeleted=false;grid.appScope.$parent.$parent.Saved=false;"></i></div>' +
            '</div>';

        var apiURls = {
            getGlossaryConfiguration: 'Glossary/GetGlossaryConfiguration',
            getGlossaryDetails: 'Glossary/GetGlossaryEditDetails',
            saveGlossaryParameter: 'Glossary/SaveGlossaryParameter?user=',
            saveGlossaryScreen: 'Glossary/SaveGlossaryScreen?user=',
            approveGlossary: 'Glossary/ApproveGlossary?user='
        }

        var partnerBudgetDef = [
          {
              field: 'Icon', width: '5%',
              enableCellEdit: false,
              cellTemplate: '<img src="{{COL_FIELD}}" style="text-align: center" class="dropdown-toggle">'
          },
          {
              field: 'ParameterName', displayName: "Icon name", width: '17%', enableCellEdit: false
          },
          {
              field: 'Description', displayName: "Criteria", width: ($localStorage.user.GlossaryApprover ? '65%' : '77%'),
              enableCellEdit: false, cellTemplate: DescriptionTemplate,
              cellClass: function (grid, row, col) {
                  return (row.entity.IsAltered == 1 || row.entity.IsAltered == 2 ? ((row.entity.Description != null && row.entity.Description != "") ? "glossary-col-alter" : "glossary-col-new") : "glossary-col-default");
              }
          },//, editableCellTemplate: DescriptionEditTemplate },
          {
              field: 'Delete', width: '6%', enableCellEdit: false, visible: $localStorage.user.GlossaryApprover, cellTemplate: deleteTemplate
          },
          { field: 'Approval', width: '7%', enableCellEdit: false, visible: $localStorage.user.GlossaryApprover, cellTemplate: approvalTemplate }

        ];
        var columnDefs = [

              {
                  field: 'ParameterName', width: '17%', enableCellEdit: false
              },
              {
                  field: 'Description', width: ($localStorage.user.GlossaryApprover ? '70%' : '82%'), enableCellEdit: false, cellTemplate: DescriptionTemplate,
                  cellClass: function (grid, row, col) {
                      return (row.entity.IsAltered == 1 || row.entity.IsAltered == 2 ? ((row.entity.Description != null && row.entity.Description != "") ? "glossary-col-alter" : "glossary-col-new") : "glossary-col-default");
                  }
              },
              {
                  field: 'Delete', width: '6%', enableCellEdit: false, visible: $localStorage.user.GlossaryApprover,
                  cellTemplate: deleteTemplate
              },
              { field: 'Approval', width: '7%', enableCellEdit: false, visible: $localStorage.user.GlossaryApprover, cellTemplate: approvalTemplate }

        ]
        $scope.gridOptions = {
            enableRowSelection: false,
            enableSorting: false,
            enableColumnMenus: false,
            enableColumnResizing: true,
            // enableCellEditOnFocus: true,

            rowTemplate: '<div grid="grid" class="ui-grid-draggable-row" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.field" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>',
            //enableRowHeaderSelection: false,
            columnDefs: columnDefs
        };

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            if (!isApprover) {
                gridApi.dragndrop.setDragDisabled(true);
            }
            else {
                gridApi.dragndrop.setDragDisabled(false);
                gridApi.draggableRows.on.rowDropped($scope, function (info, dropTarget) {
                    $rootScope.Saved = false;
                    var targetRow = info.targetRowEntity.DisplayOrder;
                    var movingRow = info.draggedRowEntity.DisplayOrder;
                    var currentScreen = $scope.Data.filter(function (e) {
                        return e.ID == $scope.selectedPage.ID;
                    });

                    if (currentScreen.length > 0 && targetRow > movingRow) {
                        var parameters = currentScreen[0].ParameterDetails.filter(function (e) {
                            return (e.DisplayOrder >= movingRow && e.DisplayOrder <= targetRow);
                        });

                        angular.forEach(parameters, function (e) {
                            if (e.DisplayOrder == movingRow) {
                                e.DisplayOrder = targetRow;
                                e.RowOrderChanged = true;
                            }
                            else if (e.DisplayOrder > movingRow) {
                                e.DisplayOrder = e.DisplayOrder - 1;
                                e.RowOrderChanged = true;
                            }
                        })
                    }
                    else if (currentScreen.length > 0 && targetRow < movingRow) {
                        var parameters = currentScreen[0].ParameterDetails.filter(function (e) {
                            return (e.DisplayOrder <= movingRow && e.DisplayOrder >= targetRow);
                        });

                        angular.forEach(parameters, function (e) {
                            if (e.DisplayOrder == movingRow) {
                                e.DisplayOrder = targetRow;
                                e.RowOrderChanged = true;
                            }
                            else if (e.DisplayOrder < movingRow) {
                                e.DisplayOrder = e.DisplayOrder + 1;
                                e.RowOrderChanged = true;
                            }
                        })
                    }
                });
            }
            $scope.gridApi = gridApi;

        };

        $http.get($rootScope.api + apiURls.getGlossaryConfiguration).success(function (response) {
            $scope.configuration = response;
            $scope.fyText = '';
            angular.forEach($scope.configuration, function (config) {
                if ($scope.fyText == '')
                    $scope.fyText = config.ShortName + " = " + config.Description;
                else
                    $scope.fyText = $scope.fyText + ", " + config.ShortName + " = " + config.Description;
            });

        });

        function GetGlossaryDetails() {
            $scope.disableAddNewParam = true;
            $http.get($rootScope.api + apiURls.getGlossaryDetails).success(function (response) {
                $scope.Data = response;
                $scope.childMenu = [];
                if ($scope.Data.length > 0) {
                    angular.forEach($scope.Data, function (value, key) {
                        if (key == 0) {
                            $scope.Data[key].active = true;
                            if ($scope.Data[key].IsParent) {
                                $scope.childMenu.push(angular.copy($scope.Data[key]));
                            }
                        }
                        else {
                            $scope.Data[key].active = false;
                            if ($scope.Data[key].ParentScreenID == $scope.Data[0].ID) {
                                $scope.childMenu.push(angular.copy($scope.Data[key]));
                            }
                        }
                    });
                    angular.forEach($scope.Data[0].ParameterDetails, function (paramRow) {
                        paramRow.showApproval = $localStorage.user.GlossaryApprover;
                    })
                    $scope.gridOptions.data = $scope.Data[0].ParameterDetails.filter(function (row) {
                        return ((row.Description != null && row.Description != "") || (row.EditedDesc != null && row.EditedDesc != ""));
                    });
                    $scope.selectedPage = angular.copy($scope.Data[0]);
                    $scope.selectedPage.showApproval = $localStorage.user.GlossaryApprover;
                    if ($scope.selectedPage.ParameterDetails.filter(function (e) { if ((e.Description == null || e.Description == "") && (e.EditedDesc == null || e.EditedDesc == "")) { return e; } }).length > 0)
                        $scope.disableAddNewParam = false;
                    else
                        $scope.disableAddNewParam = true;
                    delete $scope.selectedPage.ParameterDetails;
                }

            });
        }

        GetGlossaryDetails();

        $scope.Refresh = function () {
            $rootScope.Saved = true;
            GetGlossaryDetails();
        }

        $scope.loadMenuData = function (menu, fromChildTab) {

            $scope.selectedPage = angular.copy(menu);
            if ($scope.selectedPage.ParameterDetails.filter(function (e) { if ((e.Description == null || e.Description == "") && (e.EditedDesc == null || e.EditedDesc == "")) { return e; } }).length > 0)
                $scope.disableAddNewParam = false;
            else
                $scope.disableAddNewParam = true;
            delete $scope.selectedPage.ParameterDetails;
            $scope.selectedPage.showApproval = $localStorage.user.GlossaryApprover;

            if (fromChildTab) {
                $scope.childMenu.filter(function (e) { return e.active = false });
            }
            else if (!fromChildTab) {
                $scope.childMenu = [];
                $scope.Data.filter(function (e) { return e.active = false })
            }

            menu.active = true;
            if (menu.pageName === "Partner Budget Table icons") {
                $scope.gridOptions.columnDefs = partnerBudgetDef;
            }
            else {
                $scope.gridOptions.columnDefs = columnDefs;
            }
            angular.forEach(menu.ParameterDetails, function (paramRow) {
                paramRow.showApproval = $localStorage.user.GlossaryApprover;
            })
            $scope.gridOptions.data = menu.ParameterDetails.filter(function (row) {
                return ((row.Description != null && row.Description != "") || (row.EditedDesc != null && row.EditedDesc != ""));
            });
            if (menu.IsParent && !fromChildTab) {
                $scope.childMenu.push(angular.copy(menu));
                angular.forEach($scope.Data, function (value) {
                    if (value.ParentScreenID == menu.ID) {
                        $scope.childMenu.push(angular.copy(value));
                    }
                });
            }

            //else if (menu.IsChild) {
            //    $scope.childMenu.push(angular.copy(menu));
            //    angular.forEach($scope.Data, function (value) {
            //        if (value.ID == menu.ParentScreenID) {
            //            $scope.childMenu.push(angular.copy(value));
            //        }
            //    });
            //}
        };



        var ChangeOrder = function (row) {

        }

        $scope.EditParam = function (selectedRow, isAddNew, maxCharLength) {
            var griddata = angular.copy($scope.gridOptions.data);
            var newParams = [];
            if (isAddNew) {
                var parameterList = [];
                $scope.Data.filter(function (e) {
                    if (e.pageName == $scope.selectedPage.pageName) {
                        parameterList = e.ParameterDetails;
                    }
                });
                newParams = parameterList.filter(function (e) {
                    if ((e.Description == null || e.Description == "") && (e.EditedDesc == null || e.EditedDesc == "")) {
                        return e;
                    }
                });
            }
            if (selectedRow.entity.EditedDesc == null || selectedRow.entity.EditedDesc == "") {
                selectedRow.entity.EditedDesc = selectedRow.entity.Description != null ? selectedRow.entity.Description : "";
            };
            $uibModal.open({
                templateUrl: 'components/admin/Glossary-management/Edit-GlossaryParam-modal.html',
                size: 'lg',
                windowClass: 'glossary-edit-popup',
                controller: function ($scope, $uibModalInstance, textAngularManager, Notification) {
                    $scope.opted = angular.copy(selectedRow.entity);
                    $scope.opted.isAddNew = isAddNew;
                    $scope.maxCharLength = maxCharLength;

                    $scope.newParams = newParams;
                    if (isAddNew) {
                        $scope.charLeft = maxCharLength;
                        $scope.disableSave = true;
                    }
                    else {
                        $scope.charLeft = maxCharLength - selectedRow.entity.EditedDesc.length;
                        $scope.disableSave = false;
                    }

                    $scope.SaveData = function (selected) {
                        if (isAddNew) {
                            selected.IsEdited = true;
                            selected.IsAltered = 1;
                            griddata.push(selected);
                        }
                        else
                            griddata.filter(function (e) { if (selected.ID == e.ID) { e.EditedDesc = selected.EditedDesc; e.IsEdited = true; e.IsAltered = 2; } })
                        $uibModalInstance.close();
                        if (selectedRow.entity.$$hashKey != undefined) {
                            (angular.element(document.querySelector('#' + selectedRow.entity.$$hashKey + "ct1"))).removeClass('ui-grid-cell-focus');
                            (angular.element(document.querySelector('#' + selectedRow.entity.$$hashKey + "et1"))).removeClass('ui-grid-cell-focus');
                        }
                        $rootScope.$broadcast('SendParameter', griddata);
                    };

                    $scope.cancel = function () {
                        $uibModalInstance.close();
                        if (selectedRow.entity.$$hashKey != undefined) {
                            (angular.element(document.querySelector('#' + selectedRow.entity.$$hashKey + "ct1"))).removeClass('ui-grid-cell-focus');
                            (angular.element(document.querySelector('#' + selectedRow.entity.$$hashKey + "et1"))).removeClass('ui-grid-cell-focus');
                        }
                    };

                    $scope.selectPrameter = function (selectedParam) {
                        $scope.disableSave = true;
                        if (selectedParam != undefined && selectedParam.ID > 0) {
                            $scope.opted = angular.copy(selectedParam);
                            $scope.opted.isAddNew = isAddNew;
                            var inputTextLength = textAngularManager.retrieveEditor('textareaParameter').scope.displayElements.text[0].innerText.trim().length;
                            if (inputTextLength > 0) {
                                $scope.opted.EditedDesc = textAngularManager.retrieveEditor('textareaParameter').scope.displayElements.text[0].innerHTML;
                                $scope.disableSave = false;
                            }
                        }
                    }

                    $scope.textKeyDown = function (event) {
                        //var aa = textAngularManager.retrieveEditor('editParameter').scope.charcount;
                        var keyCode = event.keyCode;
                        //var input = document.querySelectorAll("input[name^='textareaParameter']");
                        var inputText = textAngularManager.retrieveEditor('textareaParameter').scope.displayElements.text[0].innerHTML;
                        inputText.replace(/&nbsp;/g, '');
                        $scope.charLeft = $scope.maxCharLength - inputText.length;
                        var inputTextLength = textAngularManager.retrieveEditor('textareaParameter').scope.displayElements.text[0].innerText.trim().length;
                        if (inputTextLength > 0 && $scope.opted.ID > 0)
                            $scope.disableSave = false;
                        else
                            $scope.disableSave = true;
                        if (inputText.length > $scope.maxCharLength && (keyCode != 8 && keyCode != 46)) {
                            event.preventDefault();
                        }
                        else {
                            if (keyCode != 8 && keyCode != 46)
                                $scope.charLeft = $scope.charLeft - 1;
                            return true;
                        }


                    }

                    $scope.ValidateLength = function (event) {
                        $scope.disableSave = true;
                        var inputText = textAngularManager.retrieveEditor('textareaParameter').scope.displayElements.text[0].innerHTML;
                        var inputTextLength = textAngularManager.retrieveEditor('textareaParameter').scope.displayElements.text[0].innerText.trim().length;
                        if (inputTextLength == 0) {
                            $scope.charLeft = $scope.maxCharLength;
                        }
                        else {
                            $scope.charLeft = $scope.maxCharLength - inputText.length;
                            if (inputText.length > $scope.maxCharLength) {
                                event.preventDefault();
                                Notification.error({
                                    message: "Description cannot exceed " + $scope.maxCharLength + " characters!" + $rootScope.closeNotify,
                                    delay: null
                                });
                            }
                            else if ($scope.opted.ID > 0) {
                                $scope.disableSave = false;
                                return true;
                            }

                        }
                    }


                }

            });

        }

        $scope.DeleteParam = function (selectedRow) {
            var modal = {
            };
            modal.title = "Delete Parameter";
            modal.message = "Are you sure you want to delete this parameter?";
            modal.cancelText = "Cancel";
            modal.deleteText = "Delete";
            var griddata = angular.extend($scope.gridOptions.data);
            $uibModal.open({
                templateUrl: 'components/modaldialog/delete-modal.html',
                size: 'sm',
                windowClass: 'glossary-delete-popup',
                controller: function ($scope, $uibModalInstance) {
                    $scope.modal = modal;
                    $scope.selectedRow = selectedRow;
                    $scope.griddata = angular.copy(griddata);
                    $scope.delete = function () {
                        $uibModalInstance.close();

                        angular.forEach($scope.griddata, function (e) {
                            if (e.ID == $scope.selectedRow.ID) {
                                e.IsEdited = true;
                                e.IsDeleted = true;
                            }
                        });
                        $rootScope.$broadcast('SendParmaterDelete', $scope.griddata);
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.close();
                    };

                }

            });

        }

        $scope.EditScreenDetail = function (maxCharLength) {

            var screenData = angular.copy($scope.selectedPage);
            if (screenData.EditedDesc == null || screenData.EditedDesc == "") {
                screenData.EditedDesc = screenData.Description != null ? screenData.Description : "";
            };
            if (screenData.EditedFormula == null || screenData.EditedFormula == "") {
                screenData.EditedFormula = screenData.Formula != null ? screenData.Formula : "";
            };
            $uibModal.open({
                templateUrl: 'components/admin/Glossary-management/Edit-Glossary-modal.html',
                size: 'lg',
                windowClass: 'glossary-screen-popup',
                controller: function ($scope, $uibModalInstance, textAngularManager, Notification) {
                    $scope.opted = screenData;
                    $scope.maxCharLength = maxCharLength;
                    $scope.descCharLeft = maxCharLength - screenData.EditedDesc.length;
                    $scope.formulaCharLeft = maxCharLength - screenData.EditedFormula.length;
                    $scope.disableSave = false;
                    $scope.SaveData = function (screenDetail) {
                        $uibModalInstance.close();
                        $rootScope.$broadcast('SendScreenDetail', screenDetail);
                    };

                    $scope.cancel = function () {
                        $uibModalInstance.close();

                    };
                    $scope.textKeyDown = function (event, ctrl, ctrlToValidate) {
                        var inputText = textAngularManager.retrieveEditor(ctrl).scope.displayElements.text[0].innerHTML;
                        inputText.replace(/&nbsp;/g, '');
                        var keyCode = event.keyCode;
                        if (ctrl == "textareaDesc")
                            $scope.descCharLeft = $scope.maxCharLength - inputText.length;
                        else if (ctrl == "textareaFormula")
                            $scope.formulaCharLeft = $scope.maxCharLength - inputText.length;
                        if (inputText.length > $scope.maxCharLength && (keyCode != 8 && keyCode != 46)) {
                            event.preventDefault();
                        }
                        else {
                            if (keyCode != 8 && keyCode != 46) {
                                if (ctrl == "textareaDesc")
                                    $scope.descCharLeft = $scope.descCharLeft - 1;
                                else if (ctrl == "textareaFormula")
                                    $scope.formulaCharLeft = $scope.formulaCharLeft - 1;
                            }
                            //var inputTextLength = textAngularManager.retrieveEditor(ctrl).scope.displayElements.text[0].innerText.trim().length;
                            //var ctrlToValidateTextLength = textAngularManager.retrieveEditor(ctrlToValidate).scope.displayElements.text[0].innerText.trim().length;
                            //if (inputTextLength == 0 || ctrlToValidateTextLength == 0)
                            //    $scope.disableSave = true;
                            //else
                            //    $scope.disableSave = false;
                            return true;
                        }


                    }

                    $scope.ValidateLength = function (ctrl, ctrlToValidate) {
                        var inputText = textAngularManager.retrieveEditor(ctrl).scope.displayElements.text[0].innerHTML;
                        var inputTextLength = textAngularManager.retrieveEditor(ctrl).scope.displayElements.text[0].innerText.trim().length;

                        inputText.replace(/&nbsp;/g, '');
                        var message = "";
                        if (ctrl == "textareaFormula") {
                            message = "Suggested Definitions/Formula";
                            if (inputTextLength == 0)
                                $scope.formulaCharLeft = $scope.maxCharLength;
                            else
                                $scope.formulaCharLeft = $scope.maxCharLength - inputText.length;
                        }
                        else if (ctrl == "textareaDesc") {
                            message = "Description";
                            if (inputTextLength == 0)
                                $scope.descCharLeft = $scope.maxCharLength;
                            else
                                $scope.descCharLeft = $scope.maxCharLength - inputText.length;
                        }
                        //if (inputTextLength == 0) {
                        //    $scope.disableSave = true;
                        //}
                        //else {
                        if (inputText.length > $scope.maxCharLength) {
                            $scope.disableSave = true;
                            Notification.error({
                                message: message + " cannot exceed " + $scope.maxCharLength + " characters!" + $rootScope.closeNotify,
                                delay: null
                            });
                        }
                        else {
                            inputText = textAngularManager.retrieveEditor(ctrlToValidate).scope.displayElements.text[0].innerHTML;
                            inputTextLength = textAngularManager.retrieveEditor(ctrlToValidate).scope.displayElements.text[0].innerText.trim().length;
                            inputText.replace(/&nbsp;/g, '');

                            // if (inputTextLength == 0 || inputText.length > $scope.maxCharLength)
                            if (inputText.length > $scope.maxCharLength)
                                $scope.disableSave = true;
                            else
                                $scope.disableSave = false;

                            return true;
                        }

                        //}
                    }

                }

            });

        }
        $scope.$on('SendParameter', function (event, data) {
            $scope.saveParameter(data);
        });

        $scope.$on('SendScreenDetail', function (event, data) {
            $scope.saveScreenDetail(data);
        });

        $scope.$on('SendParmaterDelete', function (event, data) {
            $scope.saveParameter(data);
        });

        $scope.saveParameter = function (data) {
            var editedRows = data.filter(function (e) {
                return e.IsEdited == true;
            });

            var ParameterEditModel = {
            };
            if (editedRows.length > 0) {
                ParameterEditModel = editedRows[0];
                $http.post($rootScope.api + apiURls.saveGlossaryParameter + $localStorage.user.UserID, ParameterEditModel).success(function (response) {
                    angular.forEach(editedRows, function (row) {
                        row.IsEdited = false;
                    });
                    if (response.ErrorCode == 1001) {
                        Notification.error({
                            message: "Data not saved. Data has been modified by some other user. Please refresh the screen to get the latest changes!" + $rootScope.closeNotify,
                            delay: null
                        })
                    }
                    else {
                        editedRows[0].EditParamID = response.ID;
                        editedRows[0].ModifiedDate = response.ModifiedDate;
                        if ($localStorage.user.GlossaryApprover) {
                            editedRows[0].IsApproved = 1;
                            $rootScope.Saved = false;
                        }
                        $scope.gridOptions.data = $filter('orderBy')(data, 'DisplayOrder', false);
                        //$scope.gridOptions.data = data;
                        $scope.Data.filter(function (e) {
                            if (e.ID == $scope.selectedPage.ID) {
                                for (var i = 0; i < e.ParameterDetails.length; i++) {
                                    if (e.ParameterDetails[i].ID == editedRows[0].ID) {
                                        e.ParameterDetails[i] = editedRows[0];
                                        break;
                                    }
                                }
                                if (editedRows[0].IsAltered == 1) {
                                    if (e.ParameterDetails.filter(function (e) { if ((e.EditedDesc == null || e.EditedDesc == "") && (e.Description == null || e.Description == "")) { return e; } }).length > 0)
                                        $scope.disableAddNewParam = false;
                                    else
                                        $scope.disableAddNewParam = true;
                                }
                            }
                        });
                        Notification.success({
                            message: "Saved successfully!" + $rootScope.closeNotify,
                            delay: null
                        });
                    }
                }).error(function () {
                    Notification.error({
                        message: "Error while saving the data. Please try again!!" + $rootScope.closeNotify,
                        delay: null
                    });
                });
            }
        }

        $scope.addParameter = function () {
            var selectedRow = {
            };
            selectedRow.entity = {
            };
            selectedRow.entity.ID = 0;
            selectedRow.entity.Description = "";
            selectedRow.entity.EditedDesc = "";
            $scope.EditParam(selectedRow, true, 8000);

        };

        $scope.saveScreenDetail = function (data) {
            var modifiedScreen = $scope.Data.filter(function (e) {
                if (e.ID == $scope.selectedPage.ID) return e;
            });
            $http.post($rootScope.api + apiURls.saveGlossaryScreen + $localStorage.user.UserID, data).success(function (response) {
                if (response.ErrorCode == 1001) {
                    Notification.error({
                        message: "Data not saved. Data has been modified by some other user. Please refresh the screen to get the latest changes!" + $rootScope.closeNotify,
                        delay: null
                    })
                }
                else {
                    modifiedScreen[0].EditedDesc = data.EditedDesc;
                    modifiedScreen[0].EditedFormula = data.EditedFormula;
                    if (modifiedScreen[0].EditScreenID > 0)
                        modifiedScreen[0].IsAltered = 2;
                    else
                        modifiedScreen[0].IsAltered = 1;
                    modifiedScreen[0].EditScreenID = response.ID;
                    modifiedScreen[0].ModifiedDate = response.ModifiedDate;

                    if ($localStorage.user.GlossaryApprover) {
                        modifiedScreen[0].IsApproved = 1;
                        $rootScope.Saved = false;
                    }
                    $scope.selectedPage = angular.copy(modifiedScreen[0]);
                    delete $scope.selectedPage.ParameterDetails;
                    $scope.selectedPage.showApproval = $localStorage.user.GlossaryApprover;


                    Notification.success({
                        message: "Saved successfully!" + $rootScope.closeNotify,
                        delay: null
                    });
                }
            }).error(function () {
                Notification.error({
                    message: "Error while saving the data. Please try again!!" + $rootScope.closeNotify,
                    delay: null
                });
            });

        }

        $scope.ApproverAction = function (action) {
            var modifiedScreen = $scope.Data.filter(function (e) {
                if (e.ID == $scope.selectedPage.ID) return e;
            });
            if (modifiedScreen.length > 0) {
                modifiedScreen[0].IsApproved = action;
                $scope.selectedPage.IsApproved = action;
            }
            $rootScope.Saved = false;
        }


        $scope.DeleteAction = function (entity) {
            entity.IsDeleted = !entity.IsDeleted;
            entity.IsApproved = null;
            $rootScope.Saved = false;
        }

        $scope.ApproveGlossary = function () {
            var screenData = [];
            var IsValid = true;
            var message = " Please Approve/Reject all suggestions and Submit.";
            for (var i = 0; i < $scope.Data.length; i++) {
                var screen = angular.copy($scope.Data[i]);
                if (screen.IsAltered == 1 || screen.IsAltered == 2) {
                    if (screen.IsApproved != 0 && screen.IsApproved != 1) {
                        Notification.error({
                            message: message + $rootScope.closeNotify,
                            delay: null
                        });
                        IsValid = false;
                        break;
                    }
                }

                var parameters = [];

                for (var j = 0; j < screen.ParameterDetails.length; j++) {
                    if (screen.ParameterDetails[j].IsDeleted)
                        parameters.push(screen.ParameterDetails[j]);
                    else if (screen.ParameterDetails[j].IsAltered == 1 || screen.ParameterDetails[j].IsAltered == 2) {
                        if (screen.ParameterDetails[j].IsApproved != 0 && screen.ParameterDetails[j].IsApproved != 1) {
                            Notification.error({
                                message: message + $rootScope.closeNotify,
                                delay: null
                            });
                            IsValid = false;
                            break;
                        }
                        else
                            parameters.push(screen.ParameterDetails[j]);
                    }
                    else if (screen.ParameterDetails[j].RowOrderChanged)
                        parameters.push(screen.ParameterDetails[j]);
                }
                screen.ParameterDetails = parameters;
                if (IsValid) {
                    if (screen.IsAltered == 1 || screen.IsAltered == 2 || screen.ParameterDetails.length > 0)
                        screenData.push(screen);
                }
                else
                    break;

            }

            if (IsValid) {
                if (screenData.length > 0) {
                    $http.post($rootScope.api + apiURls.approveGlossary + $localStorage.user.UserID, screenData).success(function (response) {
                        if (response.ErrorCode == 1001) {
                            Notification.error({
                                message: "Data not saved. Data has been modified by some other user, Please refresh the screen to get the latest changes!" + $rootScope.closeNotify,
                                delay: null
                            })
                        }
                        else {
                            $rootScope.Saved = true;
                            Notification.success({
                                message: "Data submitted successfully!" + $rootScope.closeNotify,
                                delay: null
                            });
                            GetGlossaryDetails();
                        }
                    }).error(function () {
                        Notification.error({
                            message: "Error while submit. Please try again!" + $rootScope.closeNotify,
                            delay: null
                        });
                    });
                }
                else {
                    Notification.warning({
                        message: "No Data modified!" + $rootScope.closeNotify,
                        delay: null
                    });
                }
            }
        }

        $scope.Preview = function () {
            var data = angular.copy($scope.Data);
            angular.forEach(data, function (screen) {
                if (screen.IsApproved == 1) {
                    screen.Description = screen.EditedDesc;
                    angular.forEach($scope.configuration, function (item) {
                        screen.EditedFormula = screen.EditedFormula.replace(item.ShortName, item.Description);
                    });
                    screen.RefinedFormula = screen.EditedFormula;
                }
                var parameters = screen.ParameterDetails.filter(function (e) {
                    return e.IsDeleted == false && (e.IsApproved == 1 || ((e.IsAltered == 0 || e.IsAltered == 2) && e.Description != null && e.Description != ""))
                });
                angular.forEach(parameters, function (param) {
                    if (param.IsApproved == 1) {
                        angular.forEach($scope.configuration, function (item) {
                            param.EditedDesc = param.EditedDesc.replace(item.ShortName, item.Description);
                        });
                        param.RefinedDescription = param.EditedDesc;
                    }
                })
                screen.ParameterDetails = parameters;
            });

            $rootScope.$broadcast('refreshGlossary', data);


        }

        $scope.refreshDirective = function () {
            $rootScope.$broadcast('refreshGlossary');
        }
        //$scope.addData = function () {
        //    if ($scope.gridOptions.data.filter(function (e) { return e.ParameterName == "" || e.Description == "" }).length != 0) {
        //        Notification.warning({
        //            message: "Parameter added already!" + $rootScope.closeNotify,
        //            delay: null
        //        });
        //        return;
        //    }
        //    var row = {
        //        "ParameterName": "",
        //        "Description": "",
        //        "ID": $scope.Data.filter(function (e) { return e.active == true })[0].ID,
        //        "new": true
        //    };

        //    $scope.gridOptions.data.push(row);

        //    //use timeout because it takes time or digest cylcle, to the grid API ,to "really" add this row.
        //    $scope.gridApi.selection.selectRow($scope.gridOptions.data[$scope.gridOptions.data.length - 1]);
        //    $timeout(function () {
        //        //  $scope.gridApi.selection.selectRow(0,true);

        //        $scope.gridApi.cellNav.scrollToFocus($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
        //    }, 100);

        //};



    };
})();