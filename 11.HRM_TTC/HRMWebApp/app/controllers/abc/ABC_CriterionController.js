define(['app/app', 'app/services/abc/ABC_CriterionService', 'app/services/abc/ABC_RatingTypeService'], function (app) {
    "use strict";
    app.controller('ABC_CriterionController', ['$scope', '$rootScope', '$modal', 'ABC_CriterionService',
            function ($scope, $rootScope, $modal, ABC_CriterionService) {
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.RatingType = MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return ABC_CriterionService.getListByRatingType($scope.RatingType).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.$on("RATINGTYPESELECTION", function (event, args) {
                    $scope.RatingType = args;
                    $scope.grid.dataSource.read();
                });
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<div style='text-align:center'>#:data.OrderNumber #</div>",
                        width: "50px",
                        title: "STT"
                    }, {
                        field: "Name",
                        title: "Tiêu chí đánh giá"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "50px",
                    }]
                };
                $scope.New = function () {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/abc/ABC_Criterion/detail.html',
                        controller: 'ABC_CriterionDetailController',
                        size: 'md',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            }
                        }
                    }).result.then(function (result) {
                        $scope.grid.dataSource.read();
                    });
                };
                $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/abc/ABC_Criterion/detail.html',
                        controller: 'ABC_CriterionDetailController',
                        size: 'md',
                        resolve: {
                            id: function () {
                                return Id;
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
                    var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                    if (!valid)
                        return;
                    ABC_CriterionService.getCriterionById(Id).then(function (result) {
                        ABC_CriterionService.DeleteCriterion(result.data).then(function (result1) {
                            if (result1 == 0) {
                                Notify('Xóa thất bại!', 'top-right', '3000', 'error', 'fa-times', true);
                            }
                            else if (result1 == 1) {
                                Notify('Xóa thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            }
                            $scope.grid.dataSource.read();
                        });
                    });
                };
            }
    ]);
    app.controller('ABC_CriterionDetailController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'id', 'ABC_CriterionService', 'ABC_RatingTypeService',
    function ($scope, $rootScope, $modal, $modalInstance, id, ABC_CriterionService, ABC_RatingTypeService) {
        $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
        $scope.options = {
            format: "n0",
            max: 100
        };
        $scope.criterionId = id;
        $scope.type = 0;
        $scope.obj = {};
        $scope.title = "Chi tiết tiêu chí đánh giá";
        if ($scope.isNew) {
            $scope.obj =
            {
                Id: MANAGER.GUID_EMPTY,
                Name: ""
            };
            ABC_CriterionService.clearTempRatingLevel();
        }
        else {
            ABC_CriterionService.getCriterionById(id).then(function (result) {
                $scope.obj = result.data;
                ABC_CriterionService.getRatingTypesByCriterionId(id).then(function (result) {
                    $scope.obj.RatingTypeIds = [];
                    for (var i = 0; i < result.data.length; i++) {
                        $scope.obj.RatingTypeIds.push(result.data[i].Id);
                    }
                });
            });
        }
        $scope.selectOptions = {
            placeholder: "",
            dataTextField: "Name",
            dataValueField: "Id",
            valuePrimitive: true,
            autoBind: false,
            dataSource: {
                transport: {
                    read: function (options) {
                        ABC_RatingTypeService.getList().then(function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }
        };
        $scope.dataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    ABC_CriterionService.getListRatingLevel().then(function (result) {
                        options.success(result.data);
                    });
                }
            },
            pageSize: 20
        });
        $scope.dataSource2 = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    ABC_CriterionService.getCriterionById(id).then(function (result) {
                        if (result.data.RatingLevels != null) {
                            options.success(result.data.RatingLevels);
                        }
                    });
                }
            },
            pageSize: 20
        });
        $scope.mainDictionaryGridOptions = {
            sortable: true,
            pageable: true,
            selectable: true,
            columns: [{
                field: "Name",
                title: "Mức độ đánh giá",
                attributes: {
                    style: "white-space: pre-wrap"
                }
            },
            //{
            //    template: "<div style='text-align:center'><p ng-if='#:data.Description # != null'>#:data.Description #</p></div>",
            //    title: "Mô tả"
            //},
            {
                template: "<div style='width: 30px;'><button ng-click='EditRatingLevel(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i> </button></div>",
                width: "50px"
            }
            //,{
            //    template: "<div style='width: 30px;'><button ng-click='DeleteCriterionDetail(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
            //    width: "50px",
            //}
            ],
        };
        $scope.EditRatingLevel = function (dictionaryId) {
            if (dictionaryId == "") {
                alert("Bạn chưa chọn phần tử");
                return;
            }
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: '/app/views/abc/ABC_Criterion/ratingLevelDetail.html',
                controller: 'ABC_RatingLevelInformationController',
                resolve: {
                    id: function () {
                        return dictionaryId;
                    },
                    criterionId: function () {
                        return $scope.criterionId;
                    }
                }
            }).result.then(function () {
                $scope.grid.dataSource.read();
            });
        }
        $scope.saveCriterion = function () {
            if ($scope.isNew) {
                ABC_CriterionService.SaveNewCriterion($scope.obj).then(function (result) {
                    ABC_CriterionService.SaveRatingLevelOfNewCriterion(result).then(function (result) {
                        $modalInstance.close();
                    });
                });
            } else {
                ABC_CriterionService.SaveCriterion($scope.obj).then(function () {
                    $modalInstance.close();
                });
            }
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
        //$scope.NewCriterion = function () {
        //    var modalInstance = $modal.open({
        //        animation: true,
        //        templateUrl: '/app/views/abc/ABC_Criterion/ratingLevelDetail.html',
        //        controller: 'ABC_RatingLevelInformationController',
        //        resolve: {
        //            id: function () {
        //                return MANAGER.GUID_EMPTY;
        //            },
        //            criterionId: function () {
        //                return $scope.criterionId;
        //            }
        //        }
        //    }).result.then(function (result) {
        //        $scope.grid.dataSource.read();
        //    });
        //}
        //$scope.DeleteRatingLevel = function (Id) {
        //    if (Id == "") {
        //        alert("Bạn chưa chọn phần tử");
        //        return;
        //    }
        //    var valid = window.confirm("Bạn có thật sự muốn xóa không?");
        //    if (!valid)
        //        return;
        //    ABC_CriterionService.getCriterionDetailById(Id).then(function (result) {
        //        ABC_CriterionService.DeleteCriterionDetail(result.data).then(function () {
        //            $scope.grid.dataSource.read();
        //        });
        //    });
        //};
    }]);

    app.controller('ABC_RatingLevelInformationController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'id', 'criterionId', 'ABC_CriterionService',
    function ($scope, $modal, $rootScope, $modalInstance, id, criterionId, ABC_CriterionService) {
        $scope.isNew = criterionId == MANAGER.GUID_EMPTY || '' ? true : false;
        $scope.options = {
            format: "n0",
            max: 100
        };
        $scope.obj = {};
        $scope.obj.id = id;
        $scope.obj.criterionId = criterionId;
        $scope.title = "Mức độ đánh giá";
        if ($scope.isNew) {
            ABC_CriterionService.getTempRatingLevelById(id).then(function (result) {
                $scope.obj = result.data;
            });
        } else {
            ABC_CriterionService.getRatingLevel(id, criterionId).then(function (result) {
                $scope.obj = result.data;
            });
        }
        $scope.saveRatingLevel = function () {
            if (criterionId == MANAGER.GUID_EMPTY) {
                ABC_CriterionService.SaveTempRatingLevel($scope.obj).then(function () {
                    $modalInstance.close();
                });
                $modalInstance.close();
            } else {
                ABC_CriterionService.SaveRatingLevel($scope.obj).then(function () {
                    $modalInstance.close();
                });
            }
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }]);
});