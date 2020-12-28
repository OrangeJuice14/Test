
define(['app/app', 'app/services/abc/evaluationBoardService', 'app/services/abc/ABC_ClassEvaluationBoardService', 'app/services/abc/ABC_ClassRatingDetailService'], function (app) {
    "use strict";

    app.controller('ABC_ClassEvaluationBoardController', ['$scope', '$rootScope', '$modal', '$state', 'evaluationBoardService', 'ABC_ClassEvaluationBoardService',
        function ($scope, $rootScope, $modal, $state, evaluationBoardService, ABC_ClassEvaluationBoardService) {

            $scope.grid = {};
            $scope.isEdit = false;
            $scope.isSupervisor = 0;
            $scope.staffType = 0;
            $scope.staffId = "";
            $scope.departmentId = "";
            var currentTime = new Date();
            $scope.EvaluationYear = currentTime.getFullYear();
            $scope.numericOptions = {
                format: "#",
                min: 2016,
                max: 3000,
                ForceMinValueOnEmpty: true
            }

            evaluationBoardService.getCheckIsSupervisor().then(function (result) {
                $scope.isSupervisor = result.data;
            });

            evaluationBoardService.getCurrentUser().then(function (result) {
                $scope.staffId = result.data.Id;
                $scope.departmentId = result.data.DepartmentId;
                $scope.staffType = result.data.StaffType;
            });

            $scope.yearSelected = "";
            ABC_ClassEvaluationBoardService.getEvaluationBoardList().then(function (result) {
                getYearList(result.data);
            });

            function getYearList(evaluationBoardList) {
                var currentYear = parseInt(moment().format('YYYY'));
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
                        $scope.yearSelected = $scope.yearList[$scope.yearList.length - 1];
                }
                if ($scope.yearSelected == "") {
                    $scope.yearSelected = yearList[yearList.length - 1];
                }
            }

            function onlyUnique(value, index, self) {
                return self.indexOf(value) === index;
            }

            $scope.loadEvaluationListByYear = function () {
                if ($scope.yearSelected != "") {
                    //tab đánh giá cấp dưới
                    $scope.dataSource = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return ABC_ClassEvaluationBoardService.getList().then(function (result) {
                                    var list = [];
                                    for (var i = 0, l = result.data.length; i < l; i++) {
                                        if (result.data[i].Year == $scope.yearSelected)
                                            list.push(result.data[i]);
                                    }
                                    options.success(list);
                                });
                            }
                        },
                        schema: {
                            model: {
                                id: "Id",
                                fields: {
                                    parentId: { field: "ParentId", nullable: true },
                                    Id: { field: "Id", type: "string" }
                                },
                                expanded: true
                            }
                        },
                        //pageSize: 20
                    });

                    //tab đánh giá bản thân
                    $scope.dataSource2 = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return ABC_ClassEvaluationBoardService.getListStaff().then(function (result) {
                                    var list = [];
                                    for (var i = 0, l = result.data.length; i < l; i++) {
                                        if (result.data[i].Year == $scope.yearSelected)
                                            list.push(result.data[i]);
                                    }
                                    options.success(list);
                                });
                            }
                        },
                        schema: {
                            model: {
                                id: "Id",
                                fields: {
                                    parentId: { field: "ParentId", nullable: true },
                                    Id: { field: "Id", type: "string" }
                                },
                                expanded: true
                            }
                        },
                        //pageSize: 20
                    });
                }
            }

            $scope.$watch('yearSelected', function (newVal, oldVal) {
                $scope.loadEvaluationListByYear();
            });

            $scope.$on("kendoWidgetCreated", function () {
                if ($scope.numeric != undefined) {
                    $scope.numeric.element.on("keyup", function () {
                        $scope.numeric.value($scope.numeric.element.val());
                        $scope.numeric.trigger("change");
                    });
                }
            });

            //cá nhân tự đánh giá
            $scope.mainGridOptions2 = {
                sortable: true,
                pageable: true,
                selectable: true,
                columns: [{
                    template: "<a class='month' ng-click='UserManageEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>"
                },
                {
                    field: "IsRatedSecond",
                    title: "Trưởng đơn vị đánh giá",
                    width: 100
                },
                {
                    field: "IsRatedThird",
                    title: "BGH đánh giá",
                    width: 100
                }]
            }

            $scope.UserManageEvaluation = function (Id) {
                $state.go("ABC_ClassRatingDetail", { evaluationId: Id, staffId: $scope.staffId, supervisorId: '00000000-0000-0000-0000-000000000000', departmentId: $scope.departmentId, isAdminRating: 0, isSupervisor: $scope.isSupervisor });
            }

            //Trưởng đơn vị đánh giá cấp dưới
            $scope.mainGridOptions3 = {
                sortable: true,
                pageable: true,
                selectable: true,
                columns: [{
                    template: "<a class='month' ng-click='LowerGradeEvaluation(\"#:data.Id #\", \"#:data.Name #\")'  href='javascript:void(0)'>#:data.Name #</a>"
                }]
            }

            $scope.LowerGradeEvaluation = function (Id, evaluationBroadName) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_ClassEvaluationBoard/manageDetail.html',
                    controller: 'staffClassEvaluationController',
                    size: 'lg',
                    resolve: {
                        id: function () {
                            return Id;
                        },
                        supervisorId: function () {
                            return $scope.staffId;
                        },
                        evaluationBroadName: function () {
                            return evaluationBroadName;
                        }
                    }
                }).result.then(function () {
                });
            }

            //Hiệu trưởng đánh giá cấp dưới
            $scope.mainGridOptions4 = {
                sortable: true,
                pageable: true,
                selectable: true,
                columns: [{
                    template: "<a class='month' ng-click='LowerGradePrincipalEvaluation(\"#:data.Id #\", \"#:data.Name #\")'   href='javascript:void(0)'>#:data.Name #</a>"
                }]
            }

            $scope.LowerGradePrincipalEvaluation = function (Id, evaluationBroadName) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_ClassEvaluationBoard/manageDetailPrincipal.html',
                    controller: 'staffPrincipalClassEvaluationController',
                    size: 'lg',
                    resolve: {
                        id: function () {
                            return Id;
                        },
                        supervisorId: function () {
                            return $scope.staffId;
                        },
                        evaluationBroadName: function () {
                            return evaluationBroadName;
                        }
                    }
                }).result.then(function () {
                });
            }

            $scope.checkLockVisible = function () {
                if (!$scope.obj.IsRatingLocked && (
                    (!$scope.obj.IsSupervisor && !$scope.obj.IsRated && !$scope.obj.IsRatedSecond && !$scope.obj.IsRatedThird) ||
                    ($scope.obj.IsSupervisor && $scope.obj.SupervisorType == 1 && !$scope.obj.IsRatedSecond) || //Trưởng đơn vị
                    ($scope.obj.IsSupervisor && $scope.obj.SupervisorType == 2 && !$scope.obj.IsRatedThird) //BGH
                    ))
                    return true;
                else return false;
            }
        }
    ]);

    app.controller('staffClassEvaluationController', ['$scope', '$modal', '$modalInstance', '$state', 'id', 'supervisorId', 'evaluationBroadName', 'ABC_ClassEvaluationBoardService', 'ABC_ClassRatingDetailService',
        function ($scope, $modal, $modalInstance, $state, id, supervisorId, evaluationBroadName, ABC_ClassEvaluationBoardService, ABC_ClassRatingDetailService) {
            $scope.evaluationId = id;
            $scope.supervisorId = supervisorId;
            $scope.evaluationBroadName = evaluationBroadName;
            $scope.IsRatedSecond = true;
            $scope.IsRatingLocked = true;

            ABC_ClassEvaluationBoardService.GetSupervisorType($scope.supervisorId).then(function (result) {
                $scope.supervisorType = result.data;
            });

            ABC_ClassEvaluationBoardService.getListStaffEvaluationBySupervisor($scope.evaluationId).then(function (result) {
                $scope.staffList = result.data;

                // lấy giá trị mặc định
                for (var i = 0; i < $scope.staffList.length; i++) {
                    if ($scope.staffList[i].ClassificationSecond == null) {
                        $scope.staffList[i].ClassificationSecond = 'A';
                    }
                    $scope.staffList[i].SupervisorType = $scope.supervisorType;

                    if ($scope.staffList[i].IsRatingLocked == false) {
                        $scope.IsRatingLocked = false;
                    }
                    // trưởng đơn vị chưa khóa đánh giá
                    if ($scope.staffList[i].IsRatedSecond == false) {
                        $scope.IsRatedSecond = false;
                    }
                }
            });

            $scope.alert = function () {
                alert('Cá nhân không được đánh giá!');
            }

            $scope.lock = function () {
                if (!$scope.formValid()) {
                    alert("Dữ liệu chưa hợp lệ");
                    return;
                }
                var valid = window.confirm("Bạn sẽ không thể chỉnh sửa sau khi khóa, bạn có muốn tiếp tục không?");
                if (!valid)
                    return;

                ABC_ClassRatingDetailService.PutListClassRatingDetail($scope.staffList).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    for (var i = 0; i < $scope.staffList.length; i++) {
                        $scope.staffList[i].IsRatedSecond = true;
                        $scope.IsRatedSecond = true;
                    }
                });
            }

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };

            $scope.$watch(
                function () { return angular.element('.error').attr("class"); },
                function (newVal, oldVal) {
                    /* execute onShow here if newVal === false */
            });

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

    app.controller('staffPrincipalClassEvaluationController', ['$scope', '$modal', '$modalInstance', '$state', 'id', 'supervisorId', 'evaluationBroadName', 'ABC_ClassEvaluationBoardService', 'ABC_ClassRatingDetailService',
        function ($scope, $modal, $modalInstance, $state, id, supervisorId, evaluationBroadName, ABC_ClassEvaluationBoardService, ABC_ClassRatingDetailService) {
            $scope.evaluationId = id;
            $scope.supervisorId = supervisorId;
            $scope.evaluationBroadName = evaluationBroadName;

            ABC_ClassEvaluationBoardService.GetDanhSachDonViDuocPhanQuyenChoHieuTruong($scope.supervisorId).then(function (result) {
                $scope.departmentList = result.data;
                $scope.departmentSelected = result.data.length > 0 ? result.data[0].Id : "";
                $scope.GetListStaffEvaluationByDepartment();
            });

            ABC_ClassEvaluationBoardService.GetSupervisorType($scope.supervisorId).then(function (result) {
                $scope.supervisorType = result.data;
            });

            $scope.GetListStaffEvaluationByDepartment = function () {
                ABC_ClassEvaluationBoardService.GetListStaffEvaluationByDepartment($scope.evaluationId, $scope.departmentSelected).then(function (result) {
                    $scope.staffList = result.data;
                    $scope.checkLockVisible($scope.staffList);
                    //$scope.checkSupervisorVisible($scope.staffList);
                    // lấy giá trị mặc định
                    for (var i = 0; i < $scope.staffList.length; i++) {
                        if ($scope.staffList[i].ClassificationThird == null) { // BGH chưa đánh giá
                            if ($scope.staffList[i].ClassificationSecond != 'A') {
                                $scope.staffList[i].ClassificationThird = $scope.staffList[i].ClassificationSecond;
                            } else {
                                $scope.staffList[i].ClassificationThird = 'A';
                            }
                            if ($scope.staffList[i].SupervisorType == 1 && $scope.staffList[i].Classification != null) {
                                $scope.staffList[i].ClassificationThird = $scope.staffList[i].Classification;
                                $scope.staffList[i].supervisorVisible = true;
                            }
                        }
                        // đánh giá ban giám hiệu
                        // ABC_ClassRatingDetailService.PutListClassRatingDetail RatedThird: SupervisorType == 2
                        $scope.staffList[i].SupervisorType = $scope.supervisorType;
                    }
                });
            }

            $scope.checkLockVisible = function (staffList) {
                $scope.lockVisible = staffList.filter(item => item.SupervisorType == 0 && !item.IsRatedThird && item.IsRatedSecond).length > 0 ? true : false;
            }

            $scope.alert = function () {
                alert('Cá nhân không được đánh giá!');
            }

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };

            $scope.$watch(
                function () { return angular.element('.error').attr("class"); },
                function (newVal, oldVal) { }
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

            $scope.lock = function () {
                if (!$scope.formValid()) {
                    alert("Dữ liệu chưa hợp lệ");
                    return;
                }

                var valid = window.confirm("Bạn sẽ không thể chỉnh sửa sau khi khóa, bạn có muốn tiếp tục không?");
                if (!valid)
                    return;
                ABC_ClassRatingDetailService.PutListClassRatingDetail($scope.staffList).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    for (var i = 0; i < $scope.staffList.length; i++) {
                        $scope.staffList[i].IsRatedThird = true;
                        $scope.staffList[i].supervisorVisible = false;
                    }
                    $scope.lockVisible = false;
                });
            }
        }
    ]);
});