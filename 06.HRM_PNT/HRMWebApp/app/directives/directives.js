define(['app/app'], function (app) {
    "use strict";

    app.filter('trust', ['$sce', function ($sce) {
        return function (htmlCode) {
            return $sce.trustAsHtml(htmlCode);
        }
    }])

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
            scope: {
                myValue: '=',
                myArray: '='
            },
            link: function (scope, element, attrs) {
                scope.myResult = "";
                scope.getRangeType = function () {
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
    }]).directive('ngModelOnblur', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            priority: 1,
            link: function (scope, elm, attr, ngModelCtrl) {
                if (attr.type === 'radio' || attr.type === 'checkbox') return;

                elm.unbind('input').unbind('keydown').unbind('change');
                elm.bind('blur', function () {
                    scope.$apply(function () {
                        ngModelCtrl.$setViewValue(elm.val());
                    });
                });
            }
        };
    });
});