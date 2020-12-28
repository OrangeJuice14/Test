
define(['app/app', 'app/services/abc/evaluationBoardService', 'app/services/abc/ABC_RatingDetailService', 'moment', ], function (app) {
    "use strict";
    app.controller('SyntheticEvaluationBoardController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'evaluationBoardService', 'ABC_RatingDetailService',
            function ($scope, $rootScope, $modal, $state, $stateParams, evaluationBoardService, ABC_RatingDetailService) {
                var moment = require('moment');
                $scope.grid = {};
                $scope.departmentId = "";
                $scope.evaluationId = "";
                $scope.status = "";
                $scope.isAdmin = 0;
                $scope.yearList = [];
                $scope.yearSelected = "";
                $scope.evaluationBoardList2 = [];
                evaluationBoardService.getCheckIsAdminGroup().then(function (result) {
                    $scope.isAdmin = result.data;
                });

                var departmentList = [];
                evaluationBoardService.getDepartmentList().then(function (result) {
                    departmentList = result.data;
                    if (result.data.length > 0) {
                        $scope.departmentId = result.data[0].Id;
                        if ($scope.departmentId != "" && $scope.evaluationId != "")
                        {
                            $scope.Search();
                            $scope.Search2();
                        }
                    }
                });

                var evaluationBoardList = [];
                evaluationBoardService.getEvaluationBoardList().then(function (result) {
                    evaluationBoardList = result.data;
                    getYearList();
                    var month = moment().format('MM');
                    var year = moment().format('YYYY');
                    if (result.data.length > 0) {
                        for (i = 0; i < evaluationBoardList.length; i++) {
                            if (evaluationBoardList[i].Month == month && evaluationBoardList[i].Year == year) {
                                $scope.evaluationId = evaluationBoardList[i].Id;
                                if ($scope.departmentId != "" && $scope.evaluationId != "")
                                    $scope.Search();
                                if ($scope.evaluationId != "" && $state.current.name == "departmentSyntheticEvaluationBoard")
                                    $scope.Search2();
                                return;
                            }
                        }
                    }
                });

                function getYearList() {
                    var currentYear = moment().format('YYYY');
                    var yearList = [];
                    for (var i = 0, l = evaluationBoardList.length; i < l; i++) {
                        yearList.push(evaluationBoardList[i].Year);
                    }
                    $scope.yearList = yearList.filter(onlyUnique);
                    if ($scope.yearList.length > 0) {
                        for (var ii = 0, ll = $scope.yearList.length; ii < ll; ii++) {
                            if ($scope.yearList[ii] == currentYear)
                                $scope.yearSelected = $scope.yearList[ii];
                        }
                        if ($scope.yearSelected == "")
                            $scope.yearSelected = $scope.yearList[0];
                    }
                }

                function onlyUnique(value, index, self) {
                    return self.indexOf(value) === index;
                }
                
                $scope.departmentResource = {
                    placeholder: "Chọn Đơn vị ...",
                    dataTextField: "Name",
                    dataValueField: "Id",
                    valuePrimitive: true,
                    filter: "contains",
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                //return evaluationBoardService.getDepartmentList().then(function (result) {
                                //    options.success(result.data);
                                //    if (result.data.length > 0) {
                                //        $scope.departmentId = result.data[0].Id;
                                //    }
                                //});
                                return options.success(departmentList);
                            }
                        }
                    }
                };
                $scope.evaluationResource = {
                    placeholder: "Chọn Kỳ đánh giá ...",
                    dataTextField: "Name",
                    dataValueField: "Id",
                    valuePrimitive: true,
                    filter: "contains",
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return evaluationBoardService.getEvaluationBoardList().then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        }
                    }
                }
                $scope.$watch('yearSelected', function (newVal, oldVal) {
                    var list = [];
                    for (var i = 0, l = evaluationBoardList.length; i < l; i++) {
                        if (evaluationBoardList[i].Year == newVal) {
                            list.push(evaluationBoardList[i]);
                        }
                    }
                    var currentYear = moment().format('YYYY');
                    var currentMonth = moment().format('MM');
                    //tự động chọn evaluation theo đúng tháng hiện tại
                    if (list.length > 0) {
                        if (newVal != currentYear) {
                            $scope.evaluationId = list[0].Id;
                        }
                        else {
                            for (var i = 0, l = list.length; i < l; i++) {
                                if (list[i].Month == currentMonth && list[i].EvaluationBoardType == 3)
                                    $scope.evaluationId = list[i].Id;
                            }
                        }
                    }
                    $scope.evaluationDataSource = {
                        transport: {
                            read: function (options) {
                                //return evaluationBoardService.getEvaluationBoardList().then(function (result) {
                                //    options.success(result.data);
                                //});
                                options.success(list);
                            }
                        }
                    }
                });
                
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    filterable: {
                        messages: {
                            info: "Lọc bởi : ",
                            filter: "Lọc",
                            clear: "Xóa"
                        },
                        extra: false,
                        operators: {
                            string: {
                                contains: "Từ khóa"
                            }
                        }
                    },
                    columns: [
                        {
                            field: "Checked",
                            template: "<input ng-model='dataItem.Checked' ng-change='onChecked(\"#:data.RatingId #\", \"#:data.OrderNumber#\")' type='checkbox' style='margin: auto; display: block;'></input>",
                            width: 40,
                            title: " ",
                            filterable: false
                        },
                        {
                            field: "OrderNumber",
                            title: "STT",
                            width: 50,
                            filterable: false
                        },
                        {
                            template: "<a ng-if='isAdmin!=0' ng-click='RatingEdit(\"#:data.RatingId #\",\"#:data.EvaluationBoardType #\", \"#:data.StaffId #\")'  href='javascript:void(0)'>#:data.StaffName #</a><span ng-if='isAdmin==0'>#:data.StaffName#</span>",
                            title: "Họ tên"
                        }, {
                            field: "Record",
                            title: "Số điểm",
                            width: 65,
                            filterable: false
                        }, {
                            field: "Classification",
                            title: "Xếp loại",
                            width: 65,
                            filterable: false
                        },
                        {
                            field: "ClassificationSecond",
                            title: "Trưởng ĐV chỉnh sửa",
                            width: 100,
                            filterable: false
                        }, {
                            field: "NoteSecond",
                            title: "Trưởng ĐV ghi chú",
                            width: 170,
                            filterable: false
                        },
                        {
                            field: "ClassificationThird",
                            title: "HĐ trường đánh giá",
                            width: 100,
                            filterable: false
                        }, {
                            field: "NoteThird",
                            title: "HĐ trường ghi chú",
                            width: 170,
                            filterable: false
                        },
                    //{
                    //    field: "IsRatedString",
                    //    title: "Cá nhân đánh giá"
                    //},
                    // {
                    //     field: "IsSupervisorRatedString",
                    //     title: "Trưởng đơn vị đánh giá"
                    // }
                    ],
                };
                $scope.mainGridOptions2 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [
                        {
                            field: "Checked",
                            template: "<input ng-model='dataItem.Checked' ng-change='onChecked(\"#:data.RatingId #\", \"#:data.OrderNumber#\")' type='checkbox' style='margin: auto; display: block;'></input>",
                            width: 40,
                            title: " ",
                            filterable: false
                        },
                        {
                            field: "OrderNumber",
                            title: "STT",
                            width: 50
                        }, {
                            template: "<a ng-if='isAdmin!=2' ng-click='RatingEdit2(\"#:data.RatingId #\",\"#:data.EvaluationBoardType #\", \"#:data.StaffId #\")'  href='javascript:void(0)'>#:data.StaffName #</a><span ng-if='isAdmin==2'>#:data.StaffName#</span>",
                            width: "200px",
                            title: "Họ tên"
                        }, {
                            field: "DepartmentName",
                            width: "200px",
                            title: "Đơn vị"
                        }, {
                            field: "Record",
                            width: "80px",
                            title: "Số điểm"
                        }, {
                            field: "Classification",
                            width: "80px",
                            title: "Xếp loại"
                        }, {
                            field: "ClassificationSecond",
                            title: "Hiệu trưởng đánh giá",
                            width: "100px"
                        }, {
                            field: "NoteSecond",
                            title: "Hiệu trưởng ghi chú",
                            width: "160px"
                        },
                        {
                            field: "ClassificationThird",
                            title: "HĐ trường đánh giá",
                            width: "100px"
                        }, {
                            field: "NoteThird",
                            title: "HĐ trường ghi chú",
                            width: "160px"
                        },
                    ],
                };
                $scope.Search2 = function () {
                    $scope.dataSource2 = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var isGet1 = false;
                                if ($scope.isAdmin == 2) {
                                    isGet1 = true;
                                    $scope.grid.hideColumn("DepartmentName");
                                }
                                return evaluationBoardService.getListDepartmentLeaderSyntheticEvaluation_Principal($scope.evaluationId, isGet1).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                };
                $scope.RatingEdit2 = function (RatingId, EvaluationBoardType, StaffId) {
                    //Nếu là tháng thì ko cho sửa
                    if (EvaluationBoardType == 3) {
                        alert("Không thể chỉnh sửa xếp loại tháng!");
                        return;
                    }
                    if (RatingId == "00000000-0000-0000-0000-000000000000") {
                        var c = confirm("Cá nhân chưa đánh giá. Bạn có muốn tiếp tục chỉnh sửa?");
                        if (c) {
                            ABC_RatingDetailService.getCreateNewABCRating(StaffId, $scope.evaluationId).then(function (result) {
                                var modalInstance = $modal.open({
                                    animation: true,
                                    templateUrl: '/app/views/abc/SyntheticEvaluationBoard/departmentRatingEditDetail.html',
                                    controller: 'SyntheticEvaluationBoardDetailController',
                                    resolve: {
                                        id: function () {
                                            return result.data; //ratingId
                                        },
                                        isAdmin: function () {
                                            return $scope.isAdmin;
                                        }
                                    }
                                }).result.then(function () {
                                    $scope.grid.dataSource.read();
                                });
                            });
                        } else return;
                    }
                    else {
                        var modalInstance = $modal.open({
                            animation: true,
                            templateUrl: '/app/views/abc/SyntheticEvaluationBoard/departmentRatingEditDetail.html',
                            controller: 'SyntheticEvaluationBoardDetailController',
                            resolve: {
                                id: function () {
                                    return RatingId;
                                },
                                isAdmin: function () {
                                    return $scope.isAdmin;
                                }
                            }
                        }).result.then(function () {
                            $scope.grid.dataSource.read();
                        });
                    }
                }
                $scope.onChecked = function (ratingId, orderNumber) {
                    var flag = 0;
                    for (var i = 0; i < $scope.grid.dataSource.data().length; i++) {
                        var item = $scope.grid.dataSource.at(i);
                        if (item.OrderNumber == orderNumber && item.Checked) {
                            $scope.ratingId = ratingId;
                            flag = 1;
                        }
                        else {
                            item.Checked = false;
                        }
                    }
                    if (flag == 0)
                        $scope.ratingId = "";
                }
                $scope.RatingEdit = function (RatingId, EvaluationBoardType, StaffId) {
                    //Nếu là tháng thì ko cho sửa
                    if (EvaluationBoardType == 3) {
                        alert("Không thể chỉnh sửa xếp loại tháng!");
                        return;
                    }
                    if (RatingId == "00000000-0000-0000-0000-000000000000") {
                        var c = confirm("Cá nhân chưa đánh giá. Bạn có muốn tiếp tục chỉnh sửa?");
                        if (c) {
                            ABC_RatingDetailService.getCreateNewABCRating(StaffId, $scope.evaluationId).then(function (result) {
                                var modalInstance = $modal.open({
                                    animation: true,
                                    templateUrl: '/app/views/abc/SyntheticEvaluationBoard/ratingEditDetail.html',
                                    controller: 'SyntheticEvaluationBoardDetailController',
                                    resolve: {
                                        id: function () {
                                            return result.data; //ratingId
                                        },
                                        isAdmin: function () {
                                            return $scope.isAdmin;
                                        }
                                    }
                                }).result.then(function () {
                                    $scope.grid.dataSource.read();
                                });
                            });
                        } else return;
                    }
                    else {
                        var modalInstance = $modal.open({
                            animation: true,
                            templateUrl: '/app/views/abc/SyntheticEvaluationBoard/ratingEditDetail.html',
                            controller: 'SyntheticEvaluationBoardDetailController',
                            resolve: {
                                id: function () {
                                    return RatingId;
                                },
                                isAdmin: function () {
                                    return $scope.isAdmin;
                                }
                            }
                        }).result.then(function () {
                            $scope.grid.dataSource.read();
                        });
                    }
                }
                $scope.Search = function () {
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var isGet1 = $scope.isAdmin == 0 ? true : false;
                                return evaluationBoardService.getListStaffSyntheticEvaluationExcel($scope.evaluationId, $scope.departmentId, isGet1).then(function (result) {
                                    var data = result.data;
                                    if (data.length > 0) {
                                        for (i = 0; i < data.length; i++) {
                                            data[i].Checked = false;
                                        }
                                    }
                                    options.success(data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                    evaluationBoardService.getLockedRatingDepartment($scope.evaluationId, $scope.departmentId).then(function (result) {
                        if (result.data != null && result.data.Status) {
                            $scope.status = "(Đơn vị đã bị khóa đánh giá)";
                        }
                        else {
                            $scope.status = "";
                        }
                    });
                };
                $scope.LockRating = function () {
                    var valid = window.confirm("Bạn muốn khóa đánh giá của đơn vị này không?");
                    if (!valid)
                        return;
                    evaluationBoardService.getLockUnlockRating($scope.evaluationId, $scope.departmentId, 1).then(function (result) {
                        if (result.data == 0) {
                            Notify('Lỗi!', 'top-right', '3000', 'danger', 'fa-bolt', true);
                        }
                        else if (result.data == 1) {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $scope.status = "(Đơn vị đã bị khóa đánh giá)";
                        }
                    });
                }
                $scope.UnlockRating = function () {
                    var valid = window.confirm("Bạn muốn mở khóa đánh giá của đơn vị này không?");
                    if (!valid)
                        return;
                    evaluationBoardService.getLockUnlockRating($scope.evaluationId, $scope.departmentId, 2).then(function (result) {
                        if (result.data == 0) {
                            Notify('Lỗi!', 'top-right', '3000', 'danger', 'fa-bolt', true);
                        }
                        else if (result.data == 1) {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $scope.status = "";
                        }
                    });
                }
                $scope.UnlockRatingStaff = function (type, unlockMode) {
                    if ($scope.ratingId == "00000000-0000-0000-0000-000000000000") {
                        alert("Cá nhân chưa đánh giá!");
                        return;
                    }
                    else if ($scope.ratingId == "" || $scope.ratingId == undefined) {
                        alert("Chưa có dòng nào được chọn");
                        return;
                    }
                    else {
                        if (type == 1) {
                            evaluationBoardService.getUnlockRatingStaff($scope.ratingId, unlockMode).then(function (result) {
                                if (result.data == 0) {
                                    Notify('Lỗi!', 'top-right', '3000', 'danger', 'fa-bolt', true);
                                }
                                else if (result.data == 1) {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $scope.status = "";
                                }
                                else if (result.data == 2 && unlockMode == 1) { //trường hợp mở khóa trưởng đơn vị
                                    alert("Trưởng đơn vị chưa đánh giá!");
                                    $scope.status = "";
                                }
                                else if (result.data == 2 && unlockMode == 2) { //trường hợp mở khóa cá nhân
                                    alert("Cá nhân chưa đánh giá!");
                                    $scope.status = "";
                                }
                                else if (result.data == 3 && unlockMode == 2) { //trường hợp mở khóa cá nhân
                                    Notify('Thành công, đã mở khóa thêm trưởng đơn vị!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $scope.status = "";
                                }
                            });
                        }
                        else if (type == 2) {
                            evaluationBoardService.getUnlockRatingStaff($scope.ratingId, unlockMode).then(function (result) {
                                if (result.data == 0) {
                                    Notify('Lỗi!', 'top-right', '3000', 'danger', 'fa-bolt', true);
                                }
                                else if (result.data == 1) {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $scope.status = "";
                                }
                                else if (result.data == 2 && unlockMode == 1) { //trường hợp mở khóa hiệu trưởng
                                    alert("Hiệu trưởng chưa đánh giá!");
                                    $scope.status = "";
                                }
                                else if (result.data == 2 && unlockMode == 2) { //trường hợp mở khóa cá nhân
                                    alert("Cá nhân chưa đánh giá!");
                                    $scope.status = "";
                                }
                                else if (result.data == 3 && unlockMode == 2) { //trường hợp mở khóa cá nhân
                                    Notify('Thành công, đã mở khóa thêm hiệu trưởng!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $scope.status = "";
                                }
                            });
                        }
                    }
                }
                $scope.ExportExcel = function () {
                    window.open("/ExcelExport/StaffSyntheticEvaluation.ashx?evaluationId=" + $scope.evaluationId + "&departmentId=" + $scope.departmentId + "&type=1");
                }
                $scope.ExportExcelDepartment = function () {
                    window.open("/ExcelExport/StaffSyntheticEvaluation.ashx?evaluationId=" + $scope.evaluationId + "&departmentId=" + $scope.departmentId + "&type=2");
                }
            }
    ]);
    app.controller('SyntheticEvaluationBoardDetailController', ['$scope', '$modal', '$modalInstance', '$state', 'id', 'isAdmin', 'ABC_RatingDetailService',
function ($scope, $modal, $modalInstance, $state, id, isAdmin, ABC_RatingDetailService) {
    $scope.obj = {};
    $scope.ratingId = id;
    $scope.isAdmin = isAdmin;
    $scope.classifications = [
           {
               Id: "A",
               Name: "A"
           },
           {
               Id: "B",
               Name: "B"
           }, {
               Id: "C",
               Name: "C"
           },
            {
                Id: "D",
                Name: "D"
            }
    ]
    ABC_RatingDetailService.getRating(id).then(function (result) {
        $scope.obj = result.data;
    });
    $scope.save = function () {
        ABC_RatingDetailService.SaveRating($scope.obj).then(function () {
            $modalInstance.close();
            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
        });
    };
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}
    ]);
});