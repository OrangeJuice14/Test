
define(['app/app', 'app/services/abc/evaluationBoardService', ], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('EvaluationManageController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'evaluationBoardService',
            function ($scope, $rootScope, $modal, $state, $stateParams, evaluationBoardService) {

                $scope.grid = {};
                $scope.departmentId ="";
                $scope.evaluationId = $stateParams.evaluationId != "" ? $stateParams.evaluationId : MANAGER.GUID_EMPTY;
                $scope.departmentResource = {
                    placeholder: "Chọn Đơn vị...",
                    dataTextField: "Name",
                    dataValueField: "Id",
                    valuePrimitive: true,
                    filter: "contains",
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return evaluationBoardService.getDepartmentList().then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        }
                    }
                };
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return evaluationBoardService.getListStaffEvaluationByDepartment($scope.evaluationId, $scope.departmentId).then(function (result) {
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
                    columns: [
                         {
                             field: "OrderNumber",
                             title: "STT",
                             width: 50
                         },
                         {
                             title: "Họ tên",
                             template: "<a ng-click='UserManageEvaluation(\"#:data.StaffId #\",\"#:data.DepartmentId #\",\"#:data.StaffType #\")'   href='javascript:void(0)'>#:data.StaffName #</a>"
                         },
                         {
                             title: "Cá nhân đánh giá",
                             template: "<span style='text-align:center;' ng-if='#:data.IsRated# == true'><input type='checkbox' disabled='disabled' checked='checked'></span><span style='text-align:center;' ng-if='#:data.IsRated# == false'><input type='checkbox' disabled='disabled'></span>",
                             width: 150
                         },
                         {
                             title: "Trưởng đơn vị đánh giá",
                             template: "<span style='text-align:center;' ng-if='#:data.IsSupervisorRated# == true'><input type='checkbox' disabled='disabled' checked='checked'></span><span style='text-align:center;' ng-if='#:data.IsSupervisorRated# == false'><input type='checkbox' disabled='disabled'></span>",
                             width: 180
                         },
                         {
                             field: "Record",
                             title: "Số điểm"
                         },
                        {
                            field: "Classification",
                            title: "Xếp loại"
                        },
                    ],
                };
                $scope.Search = function () {        
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return evaluationBoardService.getListStaffEvaluationByDepartment($scope.evaluationId, $scope.departmentId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                };
                $scope.UserManageEvaluation = function (StaffId, DepartmentId, StaffType) {
                    //Kiểm tra nếu là đánh giá 6 tháng thì tất cả đánh giá tháng phải khóa
                    //Nếu là đánh giá năm phải kiểm tra tất cả đánh giá 6 tháng trong năm đã khóa chưa
                    evaluationBoardService.getCheckLockAllChildRating($scope.evaluationId, StaffId, DepartmentId, StaffType).then(function (result) {
                        var check = result.data;
                        if (check==true)
                        {
                            alert("Người quản lý vẫn chưa khóa kỳ đánh giá con!");
                        }
                        else
                        {
                            $state.go("ABC_RatingDetail", { evaluationId: $scope.evaluationId, staffId: StaffId, supervisorId: '00000000-0000-0000-0000-000000000000', departmentId: DepartmentId, isAdminRating: 2 });
                        }
                    });                   
                }
            }
    ]);

});