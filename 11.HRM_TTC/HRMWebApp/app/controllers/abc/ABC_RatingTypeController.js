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
                    template: "<a ng-click='Edit(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>",
                    title: "Tên"
                }]
            };

            $scope.Edit = function (Id) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_Criterion/ratingTypeDetail.html',
                    controller: 'ABC_RatingTypeDetailController',
                    size: 'lg',
                    resolve: {
                        id: function () {
                            return Id;
                        }
                    }
                }).result.then(function () {
                    $scope.grid.select("tr.k-state-selected");
                });
            }
        }
    ]);

    app.controller('ABC_RatingTypeDetailController', ['$scope', '$rootScope', '$modalInstance', 'ABC_RatingTypeService', 'ABC_CriterionService', 'id',
        function ($scope, $rootScope, $modalInstance, ABC_RatingTypeService, ABC_CriterionService, id) {

            ABC_RatingTypeService.getObj(id).then(function (result) {
                $scope.obj = result.data;
            });

            ABC_CriterionService.getListByRatingTypeHierarchy(id).then(function (result) {
                var arr = result.data.filter(cri => !cri.IsTemp);
                $scope.treeData = new kendo.data.HierarchicalDataSource({
                    data: arr
                });
            });

            $scope.treeOptions = {
                checkboxes: {
                    checkChildren: true
                },
                dataTextField: ["Name", "Name"],
                check: onCheck,
                dataBound: treeDataBound
            };

            function treeDataBound(e) {
                this.expand(".k-item");
            }

            function checkedNodeIds(nodes, checkedNodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].checked) {
                        $scope.obj.CriterionIds.push(nodes[i].Id);
                    }

                    if (nodes[i].hasChildren) {
                        checkedNodeIds(nodes[i].children.view(), $scope.obj.CriterionIds);
                    }
                }
            }

            function onCheck() {
                $scope.obj.CriterionIds = [];
                var treeView = $scope.criterionTree;
                checkedNodeIds($scope.criterionTree.dataSource.view(), $scope.obj.CriterionIds);
            }

            $scope.save = function () {
                ABC_RatingTypeService.PutCriterionManage($scope.obj).then(function (result) {
                    if (result == 1) {
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $modalInstance.close();
                    }
                    else {
                        Notify('Thất bại!', 'top-right', '3000', 'error', 'fa-times', true);
                    }
                });
            }

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            }
        }
    ]);
});