
define(['app/app', 'app/services/abc/evaluationBoardService', ], function (app) {
    "use strict";
    app.controller('SyntheticEvaluationController', ['$scope', '$rootScope', '$modal', '$state','$stateParams', 'evaluationBoardService',
            function ($scope, $rootScope, $modal,$state,$stateParams, evaluationBoardService) {

                $scope.grid = {};
                $scope.evaluationId = $stateParams.evaluationId != "" ? $stateParams.evaluationId : MANAGER.GUID_EMPTY;
                $scope.departmentId = "";
                evaluationBoardService.getCurrentUserDepartment().then(function (result) {
                    $scope.departmentId = result.data;
                });
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return evaluationBoardService.getListStaffSyntheticEvaluation($scope.evaluationId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        field: "StaffName",
                        title: "Họ tên"
                    }, {
                        field: "IsRatedString",
                        title: "Cá nhân đánh giá"
                    },
                     {
                         field: "IsSupervisorRatedString",
                         title: "Trưởng đơn vị đánh giá"
                     }],
                };
                $scope.dataSource2 = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return evaluationBoardService.getListDepartmentLeaderSyntheticEvaluation($scope.evaluationId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.mainGridOptions2 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        field: "StaffName",
                        width: "200px",
                        title: "Họ tên"
                    }, {
                        field: "DepartmentName",
                        title: "Đơn vị"
                    }, {
                        field: "IsRatedString",
                        width: "140px",
                        title: "Cá nhân đánh giá"
                    },
                     {
                         field: "IsSupervisorRatedString",
                         width: "140px",
                         title: "BGH đánh giá"
                     }],
                };
                $scope.ExportExcel = function () {
                    window.open("/ExcelExport/StaffSyntheticEvaluation.ashx?evaluationId=" + $scope.evaluationId + "&departmentId=" + $scope.departmentId + "&type=1");
                }
                $scope.ExportExcelDepartment = function () {
                    window.open("/ExcelExport/StaffSyntheticEvaluation.ashx?evaluationId=" + $scope.evaluationId + "&departmentId=" + $scope.departmentId + "&type=2");
                }
            }
    ]);
});