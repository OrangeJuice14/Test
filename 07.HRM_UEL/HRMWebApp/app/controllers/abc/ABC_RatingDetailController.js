
define(['app/app', 'app/services/abc/ABC_RatingDetailService', 'app/services/abc/ABC_RatingTypeService', 'app/directives/directives', 'moment'], function (app) {
    "use strict";

    app.controller('ABC_RatingDetailController', ['$scope', '$document', '$modal', '$rootScope', '$state', '$stateParams', 'ABC_RatingDetailService', 'ABC_RatingTypeService',
            function ($scope, $document, $modal, $rootScope, $state, $stateParams, ABC_RatingDetailService, ABC_RatingTypeService) {
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
                $scope.ratingType = $stateParams.ratingType;
                $scope.staffResultDetaitObject = {
                    Id: $scope.staffId,
                    ABC_RatingGroupDTOs: [],
                    ABC_RatingGroupPropertyDTOs: [],
                    ABC_RatingGroupSpecialDTOs: []
                };
                $scope.numericOptions = {
                    format: "n0",
                    min: 0,
                    //max: 100,
                    ForceMinValueOnEmpty: true
                }
                //Tính tổng điểm nhóm tiêu chí chung
                $scope.totalRatingGroupMaxRecord = function (items) {
                    var result = 0;             
                    result = $scope.sum(items, 'MaxRecord');
                    //return result.toFixed(0);
                    if (result == 0) result = '';
                    return result;
                };
                $scope.totalRatingGroupStaffRecord = function (items) {
                    var result = 0;
                    result = $scope.sum(items, 'StaffRecord');
                    //return result.toFixed(0);
                    return result;
                };
                $scope.totalRatingGroupSupervisorRecord = function (items) {
                    var result = 0;
                    result = $scope.sum(items, 'SupervisorRecord');
                    //return result.toFixed(0);
                    return result;
                };
                //Tính tổng điểm nhóm tiêu chí riêng
                $scope.totalRatingGroupSpecialMaxRecord = function (items) {
                    var result = 0;             
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupSpecialDTOs, function (idx, item) {
                        result += $scope.sum(item.ABC_RatingDetailDTOs, 'MaxRecord');
                    });
                    //return result.toFixed(0);
                    return result;
                };
                $scope.totalRatingGroupSpecialStaffRecord = function (items) {
                    var result = 0;
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupSpecialDTOs, function (idx, item) {
                        result += $scope.sum(item.ABC_RatingDetailDTOs, 'StaffRecord');
                    });
                    //return result.toFixed(0);
                    return result;
                };
                $scope.totalRatingGroupSpecialSupervisorRecord = function (items) {
                    var result = 0;
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupSpecialDTOs, function (idx, item) {
                        result += $scope.sum(item.ABC_RatingDetailDTOs, 'SupervisorRecord');
                    });
                    //return result.toFixed(0);
                    return result;
                };
                //Tính tổng điểm tất cả
                $scope.totalStaffRecord = function () {
                    var result1 = 0;
                    var result2 = 0;
                    var result3 = 0;
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupDTOs, function (idx, item) {
                       result1 += $scope.sum(item.ABC_RatingDetailDTOs, 'StaffRecord');
                    });
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupPropertyDTOs, function (idx, item)
                    {
                       result2 += $scope.sum(item.ABC_RatingDetailDTOs, 'StaffRecord');
                    });
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupSpecialDTOs, function (idx, item) {
                       result3 += $scope.sum(item.ABC_RatingDetailDTOs, 'StaffRecord');
                    });
                    var result = result1 + result2 + result3;
                    $scope.staffResultDetaitObject.TotalStaffRecord = result;
                    //return result.toFixed(0);
                    return result;
                };
                $scope.totalSupervisorRecord = function () {
                    var result1 = 0;
                    var result2 = 0;
                    var result3 = 0;
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupDTOs, function (idx, item) {
                        result1 += $scope.sum(item.ABC_RatingDetailDTOs, 'SupervisorRecord');
                    });
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupPropertyDTOs, function (idx, item) {
                        result2 += $scope.sum(item.ABC_RatingDetailDTOs, 'SupervisorRecord');
                    });
                    $.each($scope.staffResultDetaitObject.ABC_RatingGroupSpecialDTOs, function (idx, item) {
                        result3 += $scope.sum(item.ABC_RatingDetailDTOs, 'SupervisorRecord');
                    });
                    var result = result1 + result2 + result3;
                    $scope.staffResultDetaitObject.TotalSupervisorRecord = result;
                    //return result.toFixed(0);
                    return result;
                };
                //--------------------------------------------------Sum functions----------------------------------------------------------
                $scope.sum = function (items, prop) {
                    return items.reduce(function (a, b) {

                        //Nếu loại tiêu chí là trừ điểm
                        if (b.ABC_CriterionDetailType == 1) {
                            return a - b[prop];
                        }
                            //Nếu loại checkbox thì ko tính điểm
                        else if (b.ABC_CriterionDetailType == 2) {
                            return 0;
                        }
                        else {
                            return a + b[prop];
                        }
                    }, 0);
                };
                //------------------------------------------------------------------------------------------------------------------------

                // kiểm tra nhóm tiêu chí có phải check chọn không, nếu đúng thì không hiện điểm của nhóm
                $scope.isCheckboxGroup = function (items) {
                    if (items.filter(q => q.ABC_CriterionDetailType != 2).length > 0) {
                        return false;
                    }
                    return true;
                }

                function getRatingDetailList() {
                    $scope.IsLoading = true;
                    ABC_RatingDetailService.getRatingDetail($scope.evaluationId, $scope.staffId, $scope.supervisorId, $scope.departmentId, $scope.isAdminRating).then(function (result) {
                        if (result.data.IsValid == false) {
                            Notify('Cá nhân chưa được phân nhóm đánh giá!', 'top-right', '3000', 'custom', 'fa-warning', true);
                            window.history.back();
                            return;
                        }
                        else {
                            $scope.staffResultDetaitObject = result.data;
                            console.log($scope.staffResultDetaitObject);
                            if ($scope.staffResultDetaitObject.WebGroupId == '00000000-0000-0000-0000-000000000004') {
                                $scope.isDepartmentLeader = true;
                            }
                            else $scope.isDepartmentLeader = false;
                            //Nếu đang là admin: cá nhân hoặc trưởng đơn vị chưa đánh giá thì điểm = 0
                            if ($scope.staffResultDetaitObject.IsAdmin == 2) {
                                $.each($scope.staffResultDetaitObject.ABC_RatingGroupDTOs, function (idx, item) {
                                    $.each(item.ABC_RatingDetailDTOs, function (idx2, item2) {
                                        item2.StaffRecord = $scope.staffResultDetaitObject.IsRated == true ? item2.StaffRecord : 0;
                                        item2.SupervisorRecord = $scope.staffResultDetaitObject.IsSupervisorRated == true ? item2.SupervisorRecord : 0;
                                    });
                                });
                            }
                            //if (!$scope.staffResultDetaitObject.IsValid)
                            //{
                            //    alert('Không thể đánh giá hoặc tồn tại bảng đánh giá 6 tháng chưa được khóa!');
                            //    window.history.back();
                            //}
                            //Nếu là không phải admin
                            //if ($scope.staffResultDetaitObject.IsAdmin != 2 && $scope.staffResultDetaitObject.IsRatingLocked) {
                            //    alert('Đơn vị đã bị khóa đánh giá!');
                            //    window.history.back();
                            //}
                            $scope.IsLoading = false;
                        }
                    });
                }
                getRatingDetailList();

                ABC_RatingDetailService.GetListTitle().then(function (result) {
                    $scope.titleList = result.data;
                });
                
                $scope.ExportExcel = function () {
                    window.open("/ExcelExport/RatingDetailExport.ashx?evaluationId=" + $scope.evaluationId + "&staffId=" + $scope.staffId + "&supervisorId=" + $scope.supervisorId + "&departmentId=" + $scope.departmentId + "&option=1");
                }

                $scope.ThanhTichExportExcel = function () {
                    window.open("/ExcelExport/RatingDetailExport.ashx?evaluationId=" + $scope.evaluationId + "&staffId=" + $scope.staffId + "&option=2");
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
                    if (!$scope.formValid()) {
                        alert("Dữ liệu chưa hợp lệ");
                        return;
                    }
                    var valid = window.confirm("Bạn sẽ không thể chỉnh sửa điểm sau khi khóa, bạn có muốn tiếp tục không?");
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
                }

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