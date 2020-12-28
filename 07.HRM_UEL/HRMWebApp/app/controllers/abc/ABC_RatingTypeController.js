define(['app/app', 'app/services/abc/ABC_CriterionService', 'app/services/abc/ABC_RatingTypeService'], function (app) {
    "use strict";
    app.controller('ABC_RatingTypeController', ['$scope', '$rootScope', '$modal', 'ABC_RatingTypeService',
        function ($scope, $rootScope, $modal, ABC_RatingTypeService) {

            $scope.dataSource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return ABC_RatingTypeService.getList().then(function (result) {
                            options.success(result.data);
                            $scope.list = result.data;
                            $scope.grid.select("tr:eq(1)");
                        });
                    }
                },
                pageSize: 20,
            });

            $scope.options = {
                sortable: true,
                pageable: {
                    buttonCount: 7
                },
                selectable: true,
                change: function (e) {
                    var selectedRows = this.select();
                    $rootScope.$broadcast('RATINGTYPESELECTION', this.dataItem(selectedRows[0]).Id);
                },
                columns: [{
                    //template: "<a ng-click='Edit(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>",
                    template: "#:data.Name #",
                    title: "Tên"
                },
                {
                    template: "<button href='javascript:void(0)' ng-click='Detail(\"#:data.Id #\")' class=\"btn btn-block btn-success btn-xs\"><i class=\"fa fa-pencil\"></i></button>",
                    width: 50
                },
                {
                    template: "<button href='javascript:void(0)' ng-click='Delete(\"#:data.Id #\")' class=\"btn btn-block btn-danger btn-xs\"><i class=\"fa fa-times\"></i></button>",
                    width: 50
                }]
            };

            $scope.NewRatingType = function () {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_Criterion/ratingTypeInfo.html',
                    controller: 'ABC_RatingTypeInfoController',
                    resolve: {
                        id: function () {
                            return MANAGER.GUID_EMPTY;
                        }
                    }
                }).result.then(function () {
                    $scope.grid.dataSource.read();
                });
            };

            $scope.Detail = function (Id) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_Criterion/ratingTypeInfo.html',
                    controller: 'ABC_RatingTypeInfoController',
                    resolve: {
                        id: function () {
                            return Id;
                        }
                    }
                }).result.then(function () {
                    $scope.grid.dataSource.read();
                });
            }

            //$scope.Edit = function (Id) {
            //    var modalInstance = $modal.open({
            //        animation: true,
            //        templateUrl: '/app/views/abc/ABC_Criterion/ratingTypeDetail.html',
            //        controller: 'ABC_RatingTypeDetailController',
            //        size: 'lg',
            //        resolve: {
            //            id: function () {
            //                return Id;
            //            }
            //        }
            //    }).result.then(function () {
            //        $scope.grid.select("tr.k-state-selected");
            //    });
            //};

            $scope.Delete = function (Id) {
                var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                if (!valid)
                    return;
                ABC_RatingTypeService.getObj(Id).then(function (result) {
                    var obj = result.data;
                    ABC_RatingTypeService.DeleteRatingType(obj).then(function (data) {
                        if (data == 1) {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $scope.grid.dataSource.read();
                            return;
                        }
                        alert('Không thể xóa do đối đượng đánh đã có nhóm tiêu chí hoặc đã được phân nhóm cho nhân viên');
                    });
                });
            };
        }
    ]);

    app.controller('ABC_RatingTypeInfoController', ['$scope', '$modal', '$modalInstance', 'ABC_RatingTypeService', 'ABC_CriterionService', 'id',
    function ($scope, $modal, $modalInstance, ABC_RatingTypeService, ABC_CriterionService, id) {

        $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
        $scope.obj = {};
        $scope.obj.RatingTypeIncludedIds = [];
        $scope.title = "Đối tượng đánh giá";
        $scope.options = {
            format: "n0",
            max: 100
        };
        if ($scope.isNew) {
            $scope.obj = {
                Id: MANAGER.GUID_EMPTY
            };
        } else {
            ABC_RatingTypeService.getObj(id).then(function (result) {
                $scope.obj = result.data;
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
                            options.success(result.data.filter(ratingType => ratingType.Id != id));
                        });
                    }
                }
            }
        };
        $scope.save = function () {
            ABC_RatingTypeService.saveRatingType($scope.obj).then(function (result) {
                if ($scope.obj.RatingTypeIncludedIds.length != 0) {
                    ABC_CriterionService.saveCriterionsOfRatingTypes(result).then(function () {
                        ABC_CriterionService.saveRatingDetailOfCriterions(result).then(function () {
                            $modalInstance.close();
                        });
                    });
                }
            });
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);

    //app.controller('ABC_RatingTypeDetailController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'ABC_RatingTypeService', 'ABC_CriterionService', 'id',
    //    function ($scope, $rootScope, $modal, $modalInstance, ABC_RatingTypeService, ABC_CriterionService, id) {
    //        ABC_RatingTypeService.getObj(id).then(function (result) {
    //            $scope.obj = result.data;
    //        });
    //        ABC_CriterionService.getListByRatingTypeHierarchy(id).then(function (result) {
    //            $scope.treeData = new kendo.data.HierarchicalDataSource({
    //                data: result.data
    //            });
    //        });
    //        $scope.treeOptions = {
    //            checkboxes: {
    //                checkChildren: true
    //            },
    //            dataTextField: ["Name", "Name"],
    //            check: onCheck,
    //            dataBound: treeDataBound
    //        };
    //        function treeDataBound(e) {
    //            this.expand(".k-item");
    //        }
    //        function checkedNodeIds(nodes, checkedNodes) {
    //            for (var i = 0; i < nodes.length; i++) {
    //                if (nodes[i].checked) {
    //                    $scope.obj.CriterionIds.push(nodes[i].Id);
    //                }

    //                if (nodes[i].hasChildren) {
    //                    checkedNodeIds(nodes[i].children.view(), $scope.obj.CriterionIds);
    //                }
    //            }
    //        }
    //        function onCheck() {
    //            $scope.obj.CriterionIds = [];
    //            var treeView = $scope.criterionTree;
    //            checkedNodeIds($scope.criterionTree.dataSource.view(), $scope.obj.CriterionIds);
    //        }

    //        $scope.save = function () {
    //            ABC_RatingTypeService.PutCriterionManage($scope.obj).then(function (result) {
    //                if (result == 1) {
    //                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
    //                    $modalInstance.close();
    //                }
    //                else {
    //                    Notify('Thất bại!', 'top-right', '3000', 'error', 'fa-times', true);
    //                }
    //            });
    //        }

    //        $scope.cancel = function () {
    //            $modalInstance.dismiss('cancel');
    //        }
    //    }
    //]);
});