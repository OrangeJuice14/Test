
define(['app/app', 'app/services/abc/evaluationBoardService', 'app/services/abc/ABC_RatingTypeService' ], function (app) {
    "use strict";
    app.controller('evaluationBoardInformationController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'evaluationBoardService',
            function ($scope, $rootScope, $modal, $state, $stateParams, evaluationBoardService) {
                $scope.grid = {};
                $scope.departmentId = "";
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
                             template: "<a ng-click='Edit(\"#:data.StaffId #\")' href='javascript:void(0)'>#:data.StaffName #</a>",
                         },
                         {
                             field: "PositionName",
                             title: "Chức vụ",
                             width: 300
                         },
                         {
                             field: "RatingType",
                             title: "Nhóm tiêu chí đánh giá",
                             width: 200
                         }
                    ],
                };
                $scope.Search = function () {
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return evaluationBoardService.getListStaffByDepartment($scope.departmentId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                };
                $scope.Edit = function (StaffId) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/abc/evaluationBoardInformation/staffRatingType.html',
                        controller: 'staffRatingTypeController',
                        size: 'md',
                        resolve: {
                            staffId: function () {
                                return StaffId;
                            }
                        }
                    }).result.then(function (result) {
                        $scope.grid.dataSource.read();
                    });
                }
            }
    ]);
    app.controller('staffRatingTypeController', ['$scope', '$modal', '$modalInstance', '$state', 'staffId', 'ABC_RatingTypeService',
    function ($scope, $modal, $modalInstance, $state, staffId, ABC_RatingTypeService) {
        $scope.obj = {};
        $scope.obj.staffId = staffId;
        $scope.listRatingType = [];
        $scope.title = 'Nhóm tiêu chí đánh giá';
        ABC_RatingTypeService.getList().then(function (result) {
            $scope.listRatingType = result.data;
            ABC_RatingTypeService.getStaffRatingType($scope.obj.staffId).then(function (result2) {
                if (result2.data != null) {
                    if (result2.data.Id != MANAGER.GUID_EMPTY) {
                        $scope.obj.ratingTypeId = result2.data.Id;
                    }
                }
            });
        });
        $scope.ratingTypeResource = {
            dataTextField: "Name",
            dataValueField: "Id",
            valuePrimitive: true,
            filter: "contains",
            autoBind: false,
            dataSource: {
                transport: {
                    read: function (options) {
                        options.success($scope.listRatingType);
                    }
                }
            }
        };
        $scope.save = function () {
            ABC_RatingTypeService.saveStaffRatingType($scope.obj).then(function (result) {
                if (result != 0) {
                    $modalInstance.close();
                }
            });
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }]);
});