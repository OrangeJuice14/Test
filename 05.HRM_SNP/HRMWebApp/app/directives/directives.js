define(['app/app'], function (app) {
    "use strict";
    
    app.directive('loading', [function () {
      return {
          restrict: 'A',
          link: function ($scope, element, attrs) {
              //$scope.isLoading = function () {
              //    return attributes["loading"];
              //    //return $http.pendingRequests.length > 0;
              //};
              $scope.$watch("IsLoading", function (value) {
                  if (value) {
                      element.removeClass('ng-hide');
                  } else {
                      element.addClass('ng-hide');
                  }
              });
          }
      };
    }]).directive('myShowScoreRange', [function () {
        return {
            restrict: 'E',
            template: '<div>Loại: {{getRangeType()}}</div>',
            scope:{
                myValue: '=',
                myArray: '='
            },
            link: function (scope, element, attrs) {
                scope.myResult = "";
                scope.getRangeType=function()
                {
                    var result = "";
                    $.each(scope.myArray, function (idx, item) {
                        if (scope.myValue >= item.Record && scope.myValue < item.MaxRecord) {
                            result = item.Name;
                        }
                    })
                    return result;
                }
                
            }
        };
    }]);

});