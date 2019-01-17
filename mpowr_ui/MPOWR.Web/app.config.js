angular.module('hpe').filter('checkZero',function(){
	return function(num){
		if(typeof num == 'undefined' || !num){
			return 0;
		}
		
		return num
	}
})	

angular.module('hpe').filter('trim',function(){
	return function(str,limit){
		if(typeof str != 'undefined'){
			if(str.length > limit ){
				return str.substr(0,limit) + "..."
			}else{
				return str;		
			}
			
		}
		return "";
		
	}
})	
angular.module('hpe')
  .filter('thousandSuffix', function () {
    return function (input, decimals) {
      var exp, rounded,
        suffixes = ['k', 'M', 'G', 'T', 'P', 'E'];

      if(window.isNaN(input)) {
        return null;
      }

      if(input < 1000) {
        return input;
      }

      exp = Math.floor(Math.log(input) / Math.log(1000));

      return (input / Math.pow(1000, exp)).toFixed(decimals) + suffixes[exp - 1];
    };
  });

angular.module('hpe').directive('formattedDate', function (dateFilter) {
    return {
        require: 'ngModel',
        scope: {
            format: "="
        },
        link: function (scope, element, attrs, ngModelController) {
            ngModelController.$parsers.push(function (data) {
                //convert data from view format to model format
                return dateFilter(data, scope.format); //converted
            });

            ngModelController.$formatters.push(function (data) {
                //convert data from model format to view format
                return dateFilter(data, scope.format); //converted
            });
        }
    }
});

angular.module('hpe').directive('pauseChildrenWatchersIf', function(){
    return {
        link: function (scope, element, attrs) {

            scope.$watch(attrs.pauseChildrenWatchersIf, function (newVal) {
                if (newVal === undefined) {
                    return;
                }
                if (newVal) {
                    toggleChildrenWatchers(element, true)
                } else {
                    toggleChildrenWatchers(element, false)
                }
            });
            function toggleChildrenWatchers(element, pause) {
                angular.forEach(element.children(), function (childElement) {
                    toggleAllWatchers(angular.element(childElement), pause);
                });
            }

            function toggleAllWatchers(element, pause) {
                var data = element.data();
                if (data.hasOwnProperty('$scope') && data.$scope.hasOwnProperty('$$watchers') && data.$scope.$$watchers) {
                    if (pause) {
                        data._bk_$$watchers = [];
                        $.each(data.$scope.$$watchers, function (i, watcher) {
                            data._bk_$$watchers.push($.extend(true, {}, watcher))
                        });

                        data.$scope.$$watchers = [];
                    } else {
                        if (data.hasOwnProperty('_bk_$$watchers')) {
                            $.each(data._bk_$$watchers, function (i, watcher) {
                                data.$scope.$$watchers.push($.extend(true, {}, watcher))
                            });
                        }
                    }

                }
                toggleChildrenWatchers(element, pause);
            }
        }
    }
});

angular.module('hpe')
  .filter('orNa', function () {
    return function (input) {
      if(typeof input == 'undefined' ){
        if(input.length == 0){
            return "-"    
        }else{
            return "-"  
        }
        
      }else{
        return input;
      }

    };
  });

  angular.module('hpe')
  .filter('nafilter', function () {
    return function (input) {
      if(input == "NOT AVAILABLE")
          return "Not Avail."
      else if(input.length > 9)
          return input.substr(0, 6) + "..";
      else
        return input

    };
  });


angular.module('hpe')
  .filter('decorateToPer', function ($filter) {
    return function (input) {

      if(typeof input == 'undefined' ){
        return "N/A"   
      }else{
        if(input.length == 0){
            return "N/A" 
        } else {
            var x = (parseFloat(input) * 100);
            return $filter('number')(x, 1) + "%"
            
        }
      }

    };
});



angular.module('hpe')
  .filter('decorateToNaStr', function ($filter) {
    return function (input) {

      if(typeof input == 'undefined' ){
        return "N/A"   
      }else{
        return input
      }

    };
});



angular.module('hpe')
  .filter('decorateToNaStr', function ($filter) {
    return function (input) {

      if(typeof input == 'undefined' || input.length == 0 ){
        return "N/A"   
      }else{
        return input
      }

    };
});



angular.module('hpe')
  .filter('decorateToNA', function ($filter) {
      return function (input,decorateTo) {

          if (typeof input == 'undefined') {
              return "N/A"
          } else {
              if (input.length == 0) {
                  return "N/A"
              } else {
                  var x = (parseFloat(input));
                  return $filter('number')(x, decorateTo);
              }
          }

      };
  });



angular.module('hpe')
  .filter('decorateSOW', function ($filter) {
      return function (input) {

          if (typeof input == 'undefined') {
              return "N/A"
          } else {
              if (input.length == 0) {
                  return "N/A"
              } else {
                  var x = parseFloat(input);
                  return $filter('number')(x, 1) + "%"

              }
          }

      };
  });



angular.module('hpe')
  .filter('conditionalDistributor', function ($filter,$sessionStorage) {
      return function (input) {
        var reseller = $sessionStorage.resellerPartner;
          if(reseller.partner.PartnerTypeID == 1){
            return $filter('number')(input, 0)
          }else{
            return "N/A"
          }
      };
  });
angular.module('hpe').config(function ($provide) {
    $provide.decorator('taOptions', ['taRegisterTool', '$delegate', function (taRegisterTool, taOptions) {
        // taRegisterTool('fontColor', {
        //    display:"<spectrum-colorpicker trigger-id='{{trigger}}' ng-model='color' on-change='!!color && action(color)' format='\"hex\"' options='options'></spectrum-colorpicker>",
        //    action: function (color) {
        //        var me = this;
        //        if (!this.$editor().wrapSelection) {
        //            setTimeout(function () {
        //                me.action(color);
        //            }, 100)
        //        } else {
        //            return this.$editor().wrapSelection('foreColor', color);
        //        }
        //    },
        //    options: {
        //        replacerClassName: 'fa fa-font', showButtons: false
        //    },
        //    color: "#000"
        //});


        taRegisterTool('fontName', {
            display: "<span class='bar-btn-dropdown dropdown'>" +
            "<button class='btn btn-blue dropdown-toggle' type='button' ng-disabled='showHtml()' style='padding-top: 4px'><i class='fa fa-font'></i><i class='fa fa-caret-down'></i></button>" +
            "<ul class='dropdown-menu'><li ng-repeat='o in options'><button class='btn btn-blue checked-dropdown' style='font-family: {{o.css}}; width: 100%' type='button' ng-click='action($event, o.css)'><i ng-if='o.active' class='fa fa-check'></i>{{o.name}}</button></li></ul></span>",
            action: function (event, font) {
                //Ask if event is really an event.
                if (!!event.stopPropagation) {
                    //With this, you stop the event of textAngular.
                    event.stopPropagation();
                    //Then click in the body to close the dropdown.
                    //$("body").trigger("click");
                }
                return this.$editor().wrapSelection('fontName', font);
            },
            options: [
                { name: 'Sans-Serif', css: 'Arial, Helvetica, sans-serif' },
                { name: 'Serif', css: "'times new roman', serif" },
                { name: 'Wide', css: "'arial black', sans-serif" },
                { name: 'Narrow', css: "'arial narrow', sans-serif" },
                { name: 'Comic Sans MS', css: "'comic sans ms', sans-serif" },
                { name: 'Courier New', css: "'courier new', monospace" },
                { name: 'Garamond', css: 'garamond, serif' },
                { name: 'Georgia', css: 'georgia, serif' },
                { name: 'Tahoma', css: 'tahoma, sans-serif' },
                { name: 'Trebuchet MS', css: "'trebuchet ms', sans-serif" },
                { name: "Helvetica", css: "'Helvetica Neue', Helvetica, Arial, sans-serif" },
                { name: 'Verdana', css: 'verdana, sans-serif' },
                { name: 'Proxima Nova', css: 'proxima_nova_rgregular' }
            ]
        });

        taRegisterTool('fontColor', {
            display: "<spectrum-colorpicker trigger-id='{{trigger}}' ng-model='color' on-change='!!color && action(color)' format='\"hex\"' options='options'></spectrum-colorpicker>",
            action: function (color) {
                var me = this;
                if (!this.$editor().wrapSelection) {
                    setTimeout(function () {
                        me.action(color);
                    }, 100)
                } else {
                    return this.$editor().wrapSelection('foreColor', color);
                }
            },
            options: {
                replacerClassName: 'fa fa-font', showButtons: false
            },
            color: "#000"
        });

        taRegisterTool('fontSize', {
            display: "<span class='bar-btn-dropdown dropdown'>" +
            "<button class='btn btn-blue dropdown-toggle' type='button' ng-disabled='showHtml()' style='padding-top: 4px'><i class='fa fa-text-height'></i><i class='fa fa-caret-down'></i></button>" +
            "<ul class='dropdown-menu'><li ng-repeat='o in options'><button class='btn btn-blue checked-dropdown' style='font-size: {{o.css}}; width: 100%' type='button' ng-click='action($event, o.value)'><i ng-if='o.active' class='fa fa-check'></i> {{o.name}}</button></li></ul>" +
            "</span>",
            action: function (event, size) {
                //Ask if event is really an event.
                if (!!event.stopPropagation) {
                    //With this, you stop the event of textAngular.
                    event.stopPropagation();
                    //Then click in the body to close the dropdown.
                  //  $("body").trigger("click");
                }
                return this.$editor().wrapSelection('fontSize', parseInt(size));
            },
            options: [
                { name: 'xx-small', css: 'xx-small', value: 1 },
                { name: 'x-small', css: 'x-small', value: 2 },
                { name: 'small', css: 'small', value: 3 },
                { name: 'medium', css: 'medium', value: 4 },
                { name: 'large', css: 'large', value: 5 },
                { name: 'x-large', css: 'x-large', value: 6 },
                { name: 'xx-large', css: 'xx-large', value: 7 }

            ]
        });
        taOptions.toolbar[1].push('fontName', 'fontColor', 'fontSize');
        return taOptions;
    }
    ]);

})
angular.module('ui.bootstrap.dropdownToggle', []).directive('dropdownToggle', ['$document', '$location', function ($document, $location) {
    var openElement = null,
        closeMenu = angular.noop;
    return {
        restrict: 'CA',
        link: function (scope, element, attrs) {
            scope.$watch('$location.path', function () { closeMenu(); });
            element.parent().bind('click', function () { closeMenu(); });
            element.bind('click', function (event) {

                var elementWasOpen = (element === openElement);

                event.preventDefault();
                event.stopPropagation();

                if (!!openElement) {
                    closeMenu();
                }

                if (!elementWasOpen && !element.hasClass('disabled') && !element.prop('disabled')) {
                    element.parent().addClass('open');
                    openElement = element;
                    closeMenu = function (event) {
                        if (event) {
                            event.preventDefault();
                            event.stopPropagation();
                        }
                        $document.unbind('click', closeMenu);
                        element.parent().removeClass('open');
                        closeMenu = angular.noop;
                        openElement = null;
                    };
                    $document.bind('click', closeMenu);
                }
            });
        }
    };
}]);

String.prototype.endsWith = function (pattern) {
    var d = this.length - pattern.length;
    return d >= 0 && this.lastIndexOf(pattern) === d;
};