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

    app.directive('formatDate', [function () {
        return {
            restrict: 'E',
            template: '{{getDate()}}',
            scope: {
                myValue: '='
            },
            link: function (scope, element, attrs) {
                scope.getDate = function () {
                    if (scope.myValue == null)
                        return '';
                    else {
                        var date = new Date(scope.myValue);
                        var day = ("0" + date.getDate()).slice(-2);
                        var month = ("0" + (date.getMonth() + 1)).slice(-2);
                        var year = date.getFullYear();
                        return day + "/" + month + "/" + year;
                    }
                }

            }
        };
    }]);

    app.directive('formatDateTime', [function () {
        return {
            restrict: 'E',
            template: '{{getDateTime()}}',
            scope: {
                myValue: '='
            },
            link: function (scope, element, attrs) {
                scope.getDateTime = function () {
                    if (scope.myValue == null)
                        return '';
                    else {
                        var date = new Date(scope.myValue);
                        var hour = date.getHours();
                        var minute = ("0" + date.getMinutes()).slice(-2);
                        var day = ("0" + date.getDate()).slice(-2);
                        var month = ("0" + (date.getMonth() + 1)).slice(-2);
                        var year = date.getFullYear();
                        if (hour == 0 && minute == 0)
                            return day + "/" + month + "/" + year;
                        return day + "/" + month + "/" + year + " " + hour + ":" + minute;
                    }
                }

            }
        };
    }]);

    app.directive('changeScoreDanhGia', [function () {
        return {
            restrict: 'AE',
            template:
                `<div ng-if='!valueItem.NoChild && !danhgia.IsLock' class='label-number-center'>{{valueItem.Diem}}</div>
                        <div ng-if='danhgia.IsLock' class='text-align-center'>{{valueItem.Diem}}</div>
                        <div ng-if='!danhgia.IsLock && valueItem.TieuChiIsAutoScore' class='label-number-center'>{{valueItem.Diem}}</div>

                        <div ng-if='valueItem.NoChild && valueItem.TieuChiDiemTru && valueItem.TieuChiDiemToiDa != null && !danhgia.IsLock && !valueItem.TieuChiIsAutoScore'>
                            <input style='width: 45px; height: 20px;' type='number' class='input-number-center' title='Nhập số <= 0 và >= -{{valueItem.TieuChiDiemToiDa}}' min='-{{valueItem.TieuChiDiemToiDa}}' max='0'   ng-model='valueItem.Diem' ng-change='ngChange()' ng-focus='ngFocus()'/>
                            <div class="padding-top-5"><label ng-show="valueItem.ScoreError" class="error">Nhập số <= 0 và >= -{{valueItem.TieuChiDiemToiDa}}</label></div>
                        </div>
                        <div ng-if='valueItem.NoChild && valueItem.TieuChiDiemTru && valueItem.TieuChiDiemToiDa == null && !danhgia.IsLock && !valueItem.TieuChiIsAutoScore'   >
                            <input style='width: 45px; height: 20px;' type='number' class='input-number-center' title='Nhập số <= 0 và >= -{{valueItem.TieuChiParentDiemToiDa}}' min='-{{valueItem.TieuChiParentDiemToiDa}}' max='0'ng-model='valueItem.Diem' ng-change='ngChange()' ng-focus='ngFocus()'/>
                            <div class="padding-top-5"><label ng-show="valueItem.ScoreError" class="error">Nhập số <= 0 và >= -{{valueItem.TieuChiParentDiemToiDa}}</label></div>
                        </div>
                        <div ng-if='valueItem.NoChild && !valueItem.TieuChiDiemTru && valueItem.TieuChiDiemToiDa != null && !danhgia.IsLock && !valueItem.TieuChiIsAutoScore'>
                            <input style='width: 45px; height: 20px;' type='number' class='input-number-center' title='Nhập số >= 0 và <= {{valueItem.TieuChiDiemToiDa}}'    min='0' max='{{valueItem.TieuChiDiemToiDa}}'         ng-model='valueItem.Diem' ng-change='ngChange()' ng-focus='ngFocus()'/>
                            <div class="padding-top-5"><label ng-show="valueItem.ScoreError" class="error">Nhập số >= 0 và <= {{valueItem.TieuChiDiemToiDa}}</label></div>
                        </div>
                        <div ng-if='valueItem.NoChild && !valueItem.TieuChiDiemTru && valueItem.TieuChiDiemToiDa == null && !danhgia.IsLock && !valueItem.TieuChiIsAutoScore'>
                            <input style='width: 45px; height: 20px;' type='number' class='input-number-center' title='Nhập số >= 0 và <= {{valueItem.TieuChiParentDiemToiDa}}'    min='0' max='{{valueItem.TieuChiParentDiemToiDa}}'   ng-model='valueItem.Diem' ng-change='ngChange()' ng-focus='ngFocus()'/>
                            <div class="padding-top-5"><label ng-show="valueItem.ScoreError" class="error">Nhập số >= 0 và <= {{valueItem.TieuChiParentDiemToiDa}}</label></div>
                        </div>`,
            scope: {
                valueItem: '=',
                ngChange: '&',
                ngFocus: '&',
                danhgia:'='
            },
            link: function (scope, element, attrs) {
                scope.valueItem;
                
                //scope.$watch(scope);
                scope.ngChange();
                //scope.temp = function () {
                //    if (!scope.valueItem.NoChild)
                //        return `<span>{{valueItem.Diem}}</span>`
                //    if (scope.valueItem.NoChild && scope.valueItem.TieuChiDiemTru && scope.valueItem.TieuChiDiemToiDa != null)
                //        return `<input type='number' style='width: 50px; height: 31px; margin-top: 2px' min='-{{valueItem.TieuChiDiemToiDa}}' max='0' ng-model='valueItem.Diem' ng-change='ngChange()'/>`;
                //    if (scope.valueItem.NoChild && scope.valueItem.TieuChiDiemTru && scope.valueItem.TieuChiDiemToiDa == null)
                //        return `<input type='number' style='width: 50px; height: 31px; margin-top: 2px' min='-{{valueItem.TieuChiParentDiemToiDa}}' max='0' ng-model='valueItem.Diem' ng-change='ngChange()'/>`;
                //    if (scope.valueItem.NoChild && !scope.valueItem.TieuChiDiemTru && scope.valueItem.TieuChiDiemToiDa != null)
                //        return `<input type='number' style='width: 50px; height: 31px; margin-top: 2px' min='0' max='{{valueItem.TieuChiDiemToiDa}}' ng-model='valueItem.Diem' ng-change='ngChange()' />`;
                //    if (scope.valueItem.NoChild && !scope.valueItem.TieuChiDiemTru && scope.valueItem.TieuChiDiemToiDa == null)
                //        return `<input type='number' style='width: 50px; height: 31px; margin-top: 2px' min='0' max='{{valueItem.TieuChiParentDiemToiDa}}' ng-model='valueItem.Diem' ng-change='ngChange()' />`;
                //}
            }
        };
    }]);

});