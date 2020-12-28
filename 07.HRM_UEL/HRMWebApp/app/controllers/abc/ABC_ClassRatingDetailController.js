
define(['app/app', 'app/services/abc/ABC_RatingDetailService', 'app/services/abc/ABC_ClassRatingDetailService', 'app/directives/directives', 'moment'], function (app) {
    "use strict";

    app.controller('ABC_ClassRatingDetailController', ['$scope', '$document', '$modal', '$rootScope', '$state', '$stateParams', 'ABC_ClassRatingDetailService',
        function ($scope, $document, $modal, $rootScope, $state, $stateParams, ABC_ClassRatingDetailService) {
            //$("#sidebar").addClass("menu-compact");
            var moment = require('moment');
            $scope.IsLoading = false;
            $scope.evaluationId = $stateParams.evaluationId != "" ? $stateParams.evaluationId : MANAGER.GUID_EMPTY;
            $scope.staffId = $stateParams.staffId;
            $scope.supervisorId = $stateParams.supervisorId != "" ? $stateParams.supervisorId : MANAGER.GUID_EMPTY;
            $scope.isSupervisor = $stateParams.isSupervisor;
            $scope.departmentId = $stateParams.departmentId != "" ? $stateParams.departmentId : MANAGER.GUID_EMPTY;
            $scope.isAdminRating = $stateParams.isAdminRating;
            $scope.obj = {};
            //------------------------------------------------------------------------------------------------------------------------

            function getRatingDetailList() {
                $scope.IsLoading = true;
                ABC_ClassRatingDetailService.GetClassRatingDetail($scope.evaluationId, $scope.staffId, $scope.supervisorId, $scope.departmentId, $scope.isAdminRating).then(function (result) {
                    if (result.data.IsValid == false) {
                        Notify('Bạn không được phép đánh giá!', 'top-right', '3000', 'custom', 'fa-warning', true);
                        window.history.back();
                        return;
                    }
                    else {
                        $scope.obj = result.data;
                        $scope.IsLoading = false;
                    }
                });
            }
            getRatingDetailList();

            $scope.checkLockVisible = function () {
                if (!$scope.obj.IsRatingLocked && (
                    (!$scope.obj.IsSupervisor && !$scope.obj.IsRated && !$scope.obj.IsRatedSecond && !$scope.obj.IsRatedThird) ||
                    ($scope.obj.IsSupervisor && $scope.obj.SupervisorType == 1 && !$scope.obj.IsRatedSecond) || //Trưởng đơn vị
                    ($scope.obj.IsSupervisor && $scope.obj.SupervisorType == 2 && !$scope.obj.IsRatedThird) //BGH
                    ))
                    return true;
                else return false;
            }

            $scope.lock = function () {
                if (!$scope.formValid()) {
                    alert("Dữ liệu chưa hợp lệ");
                    return;
                }
                var valid = window.confirm("Bạn sẽ không thể chỉnh sửa sau khi khóa, bạn có muốn tiếp tục không?");
                if (!valid)
                    return;
                ABC_ClassRatingDetailService.PutClassRatingDetail($scope.obj).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $state.go($state.current, {}, { reload: true });
                });
            };

            $scope.$watch(
              function () { return angular.element('.error').attr("class"); },
              function (newVal, oldVal) {
                  /* execute onShow here if newVal === false */
              }
            );

            $scope.formChange = function () {
                var elements = $(".error");
                var isvalid = true;
                $.each(elements, function (idx, item) {
                    if (!$(item).hasClass("ng-hide")) {
                        $scope.formValid = false;
                        return;
                    }
                });
            }

            $scope.formValid = function () {
                var result = true;
                $.each($(".error"), function (idx, item) {
                    if (!$(item).hasClass("ng-hide")) {
                        result = false;
                    }
                });
                return result;
            }
        }
    ]);
});