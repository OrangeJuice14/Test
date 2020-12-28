define(['app/app'], function (app) {
    "use strict";

    app.directive('ckeditor', [function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, ngModel) {
                var ck = CKEDITOR.replace(element[0]);
                if (!ngModel) return;
                ck.on('instanceReady', function () {
                    if (ngModel.$viewValue == undefined) {
                        ngModel.$render = function () {
                            ck.setData(ngModel.$viewValue);
                        };
                    }
                    else ck.setData(ngModel.$viewValue);
                });
                function updateModel() {
                    scope.$apply(function () {
                        ngModel.$setViewValue(ck.getData());
                    });
                }
                ck.on('change', updateModel);
            }
        };
    }])
});

