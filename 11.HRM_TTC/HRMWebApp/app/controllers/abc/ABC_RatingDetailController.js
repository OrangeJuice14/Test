
define(['app/app', 'app/services/abc/ABC_RatingDetailService', 'app/directives/directives', 'moment'], function (app) {
    "use strict";

    app.controller('ABC_RatingDetailController', ['$scope', '$document', '$modal', '$rootScope', '$state', '$stateParams', 'ABC_RatingDetailService',
            function ($scope, $document, $modal, $rootScope, $state, $stateParams, ABC_RatingDetailService) {
                $("#sidebar").addClass("menu-compact");
                var moment = require('moment');
                $scope.TABLECOLORS = MANAGER.TABLECOLORS;
                $scope.recordTemp = 0;
                $scope.IsLoading = false;
                $scope.evaluationId = $stateParams.evaluationId != "" ? $stateParams.evaluationId : MANAGER.GUID_EMPTY;
                $scope.staffId = $stateParams.staffId;              
                $scope.supervisorId = $stateParams.supervisorId != "" ? $stateParams.supervisorId : MANAGER.GUID_EMPTY;
                $scope.departmentId = $stateParams.departmentId != "" ? $stateParams.departmentId : MANAGER.GUID_EMPTY;
                $scope.isAdminRating = $stateParams.isAdminRating;

                $scope.staffResultDetaitObject = {
                    Id: $scope.staffId,
                    ABC_RatingGroupDTOs: []
                };

                function getRatingDetailList() {
                    $scope.IsLoading = true;
                    ABC_RatingDetailService.getRatingDetail($scope.evaluationId, $scope.staffId, $scope.supervisorId, $scope.departmentId, $scope.isAdminRating).then(function (result) {
                        if (result.data.IsValid == false) {
                            Notify('Bạn không được phép đánh giá!', 'top-right', '3000', 'custom', 'fa-warning', true);
                            window.history.back();
                            return;
                        }
                        else {
                            $scope.staffResultDetaitObject = result.data;
                            if ($scope.staffResultDetaitObject.WebGroupId == '00000000-0000-0000-0000-000000000006') {
                                $scope.isDepartmentLeader = true;
                            }
                            else $scope.isDepartmentLeader = false;
                            //Nếu đang là admin: cá nhân hoặc trưởng đơn vị chưa đánh giá thì điểm = 0
                            //if ($scope.staffResultDetaitObject.IsAdmin == 2) {
                            //    $.each($scope.staffResultDetaitObject.ABC_RatingGroupDTOs, function (idx, item) {
                            //        $.each(item.ABC_RatingDetailDTOs, function (idx2, item2) {
                            //            item2.StaffRecord = $scope.staffResultDetaitObject.IsRated == true ? item2.StaffRecord : 0;
                            //            item2.SupervisorRecord = $scope.staffResultDetaitObject.IsSupervisorRated == true ? item2.SupervisorRecord : 0;
                            //        });
                            //    });
                            //    $.each($scope.staffResultDetaitObject.ABC_RatingGroupSpecialDTOs, function (idx, item) {
                            //        $.each(item.ABC_RatingDetailDTOs, function (idx2, item2) {
                            //            item2.StaffRecord = $scope.staffResultDetaitObject.IsRated == true ? item2.StaffRecord : 0;
                            //            item2.SupervisorRecord = $scope.staffResultDetaitObject.IsSupervisorRated == true ? item2.SupervisorRecord : 0;
                            //        });
                            //    });
                            //    $.each($scope.staffResultDetaitObject.ABC_RatingGroupPropertyDTOs, function (idx, item) {
                            //        $.each(item.ABC_RatingDetailDTOs, function (idx2, item2) {
                            //            item2.StaffRecord = $scope.staffResultDetaitObject.IsRated == true ? item2.StaffRecord : 0;
                            //            item2.SupervisorRecord = $scope.staffResultDetaitObject.IsSupervisorRated == true ? item2.SupervisorRecord : 0;
                            //        });
                            //    });
                            //}

                            if (!$scope.staffResultDetaitObject.IsValid)
                            {
                                alert('Không thể đánh giá!');
                                window.history.back();
                            }

                            // Nếu không phải admin
                            if ($scope.staffResultDetaitObject.IsAdmin != 2 && $scope.staffResultDetaitObject.IsRatingLocked) {
                                alert('Đơn vị đã bị khóa đánh giá!');
                                window.history.back();
                            }
                            $scope.IsLoading = false;
                        }
                    });
                }

                getRatingDetailList();
                
                $scope.ExportExcel = function () {
                    window.open("/ExcelExport/RatingDetailExport.ashx?evaluationId=" + $scope.evaluationId + "&staffId=" + $scope.staffId + "&supervisorId=" + $scope.supervisorId + "&departmentId=" + $scope.departmentId + "&option=1");
                }

                $scope.save = function () {
                    if (!$scope.formValid()) {
                        alert("Dữ liệu chưa hợp lệ");
                        return;
                    }
                    //var valid = window.confirm("Bạn muốn lưu thay đổi không?");
                    //if (!valid)
                    //    return;
                    
                    ABC_RatingDetailService.Save($scope.staffResultDetaitObject).then(function () {
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $state.go(
                            $state.current,
                            {},
                            { reload: true });
                    });
                };

                $scope.lock = function () {
                    var valid = window.confirm("Bạn sẽ không thể chỉnh sửa sau khi khóa, bạn có muốn tiếp tục không?");
                    if (!valid)
                        return;
                    ABC_RatingDetailService.Lock($scope.staffResultDetaitObject).then(function () {
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $state.go($state.current, {}, { reload: true });
                    });
                };

                $scope.unlock = function () {
                    ratingKPIService.getUnlock($scope.staffResultDetaitObject.RatingResultId).then(function () {
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

                $scope.validateRecord = function (value, note, fileCount) {
                    return !(value > 60 && (note == null || note == '') && fileCount <= 0);
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