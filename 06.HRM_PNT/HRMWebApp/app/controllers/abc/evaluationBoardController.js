
define(['app/app', 'app/services/abc/evaluationBoardService', ], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('evaluationBoardController', ['$scope', '$rootScope', '$modal', '$state', 'evaluationBoardService',
            function ($scope, $rootScope, $modal, $state, evaluationBoardService) {

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
                //var evaluationBoardList = [];
                //var evaluationBoardListPromise = new Promise(function (resolve, reject) {
                //    evaluationBoardService.getEvaluationBoardList().then(function (result) {
                //        evaluationBoardList = result.data;
                //        resolve();
                //    });
                //});
                //evaluationBoardListPromise.then(function () {
                //    getYearList();
                //});
                $scope.yearSelected = "";
                evaluationBoardService.getEvaluationBoardList().then(function (result) {
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
                        $scope.dataSource = new kendo.data.TreeListDataSource({
                            transport: {
                                read: function (options) {
                                    return evaluationBoardService.getList().then(function (result) {
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
                        $scope.dataSource2 = new kendo.data.TreeListDataSource({
                            transport: {
                                read: function (options) {
                                    return evaluationBoardService.getListStaff().then(function (result) {
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
                $scope.$watch('EvaluationYear', function (newVal, oldVal) {
                    loadDataSource3();
                });
                loadDataSource3();
                function loadDataSource3() {
                    $scope.dataSource3 = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return evaluationBoardService.getListEvaluationManage().then(function (result) {
                                    var list = [];
                                    for (var i = 0, l = result.data.length; i < l; i++) {
                                        if (result.data[i].Year == $scope.EvaluationYear)
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
                $scope.updateYearValue = function () {
                    $scope.EvaluationYear = $scope.numeric.value();
                };
                $scope.$on("kendoWidgetCreated", function () {
                    if ($scope.numeric != undefined) {
                        $scope.numeric.element.on("keyup", function () {
                            $scope.numeric.value($scope.numeric.element.val());
                            $scope.numeric.trigger("change");
                        });
                    }
                });
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a ng-show='#:data.EvaluationBoardType #==1' class='year' ng-click='EvaluationManage(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==2' class='semester' ng-click='EvaluationManage(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==3' class='month' ng-click='EvaluationManage(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a>"
                    }]
                };
                $scope.mainGridOptions2 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a ng-show='#:data.EvaluationBoardType #==1' class='year' ng-click='UserManageEvaluation(\"#:data.Id #\")'   href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==2' class='semester' ng-click='UserManageEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==3' class='month' ng-click='UserManageEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>"
                    },
                    {
                        field: "IsSupervisorRated",
                        title: "Cấp quản lý đánh giá",
                        width: 100
                    },
                    ]
                };
                $scope.mainGridOptions3 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a ng-show='#:data.EvaluationBoardType #==1' class='year' ng-click='LowerGradeEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==2' class='semester' ng-click='LowerGradeEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==3' class='month' ng-click='LowerGradeEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>"
                    }]
                };
                $scope.mainGridOptions4 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a ng-show='#:data.EvaluationBoardType #==1' class='year' ng-click='DepartmentEvaluation(\"#:data.Id #\")'   href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==2' class='semester' ng-click='DepartmentEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.EvaluationBoardType #==3' class='month' ng-click='DepartmentEvaluation(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>"
                    }]
                };
                $scope.UserManageEvaluation = function (Id) {
                    //Kiểm tra nếu là đánh giá 6 tháng thì tất cả đánh giá tháng phải khóa (đã sửa lại chỉ cần 1 tháng)
                    //Nếu là đánh giá năm phải kiểm tra tất cả đánh giá 6 tháng trong năm đã khóa chưa (đã sửa lại chỉ cần 1 lần 6 tháng)
                    evaluationBoardService.getCheckLockAllChildRating(Id, $scope.staffId, $scope.departmentId, $scope.staffType).then(function (result) {
                        var check = result.data;
                        if (check == true) {
                            alert("Người quản lý vẫn chưa khóa kỳ đánh giá con!");
                        }
                        else {
                            if ($scope.staffType == 1) {
                                $state.go("ABC_RatingDetailProfessor", { evaluationId: Id, staffId: $scope.staffId, supervisorId: '00000000-0000-0000-0000-000000000000', departmentId: $scope.departmentId, isAdminRating: 0 });
                            }
                            else {
                                $state.go("ABC_RatingDetail", { evaluationId: Id, staffId: $scope.staffId, supervisorId: '00000000-0000-0000-0000-000000000000', departmentId: $scope.departmentId, isAdminRating: 0 });
                            }

                        }
                    });
                }
                $scope.EvaluationManage = function (Id) {
                    $state.go("EvaluationManage", { evaluationId: Id });
                }
                $scope.ProfessorManageEvaluation = function (Id) {
                    $state.go("ABC_RatingDetailProfessor", { evaluationId: Id, staffId: '00000000-0000-0000-0000-000000000000', supervisorId: '00000000-0000-0000-0000-000000000000', departmentId: '00000000-0000-0000-0000-000000000000', isAdminRating: 0 });
                }
                $scope.LowerGradeEvaluation = function (Id) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/abc/evaluationBoard/manageDetail.html',
                        controller: 'staffEvaluationController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            supervisorId: function () {
                                return $scope.staffId;
                            }
                        }
                    }).result.then(function () {
                    });
                }
                $scope.DepartmentEvaluation = function (Id) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/abc/evaluationBoard/manageDepartmentDetail.html',
                        controller: 'departmentEvaluationController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            supervisorId: function () {
                                return $scope.staffId;
                            }
                        }
                    }).result.then(function () {
                    });
                }
                $scope.New = function () {
                    var valid = window.confirm("Bạn muốn tạo không?");
                    if (!valid)
                        return;
                    evaluationBoardService.getCheckExistEvaluationBoard($scope.EvaluationYear).then(function (result) {
                        if (result.data == false) {
                            evaluationBoardService.getCreateEvaluationBoard($scope.EvaluationYear).then(function (result) {
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                $scope.grid.dataSource.read();
                            });
                        }
                        else {
                            Notify('Đã tồn tại!', 'top-right', '3000', 'warning', 'fa-warning', true);
                        }
                    });

                };

                $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/criterion/detail.html',
                        controller: 'criterionDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            targetGroupDetailId: function () {
                                return $scope.TargetGroupDetailId;
                            },
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                    });
                }


                $scope.Delete = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    professorCriterionService.getCheckHasDictionary(Id).then(function (result) {
                        var check = result.data;
                        if (check == true) {
                            alert("Tiêu chí này đã có từ điển con, xóa tiêu chí các từ điển con sẽ mất!");
                            return;
                        }
                        else {
                            var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                            if (!valid)
                                return;
                            professorCriterionService.getObj(Id).then(function (result) {
                                professorCriterionService.Delete(result.data).then(function () {
                                    if (result.data == 0) {
                                        alert("Xóa thất bại!");
                                    }
                                    else if (result.data == 1) {
                                        alert("Xóa thành công!");
                                    }
                                    else if (result.data == 0) {
                                        alert("Tiêu chí đang được sử dụng!");
                                    }
                                    $scope.grid.dataSource.read();
                                });
                            });
                        }
                    });
                };
            }
    ]);
    app.controller('staffEvaluationController', ['$scope', '$modal', '$modalInstance', '$state', 'id', 'supervisorId', 'evaluationBoardService',
            function ($scope, $modal, $modalInstance, $state, id, supervisorId, evaluationBoardService) {
                $scope.evaluationId = id;
                $scope.supervisorId = supervisorId;
                evaluationBoardService.getListStaffEvaluationBySupervisor($scope.evaluationId).then(function (result) {
                    $scope.staffList = result.data;
                });
                $scope.StaffSyntheticEvaluation = function () {
                    $modalInstance.dismiss('cancel');
                    $state.go("StaffSyntheticEvaluation", { evaluationId: $scope.evaluationId });
                }
                $scope.alert = function () {
                    alert('Cá nhân không được đánh giá!');
                }
                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
            }
    ]);
    app.controller('departmentEvaluationController', ['$scope', '$modal', '$modalInstance', '$state', 'id', 'supervisorId', 'evaluationBoardService',
            function ($scope, $modal, $modalInstance, $state, id, supervisorId, evaluationBoardService) {
                $scope.evaluationId = id;
                $scope.supervisorId = supervisorId;
                evaluationBoardService.getListDepartmentLeaderEvaluation($scope.evaluationId).then(function (result) {
                    $scope.staffList = result.data;
                });
                $scope.DepartmentSyntheticEvaluation = function () {
                    $modalInstance.dismiss('cancel');
                    $state.go("DepartmentSyntheticEvaluation", { evaluationId: $scope.evaluationId });
                }
                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
            }
    ]);
});