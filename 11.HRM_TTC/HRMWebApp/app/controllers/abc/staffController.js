define(['app/app', 'app/services/abc/staffService', 'app/services/abc/departmentService'], function (app) {
    "use strict";

    app.controller('staffController', ['$scope', '$modal', '$rootScope', '$state', '$stateParams', 'staffService', 'departmentService',
            function ($scope, $modal, $rootScope, $state, $stateParams, staffService, departmentService) {
                $scope.adminDataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return staffService.getListAdminLeader($rootScope.session.CongTyId).then(function (result) {
                                options.success(result.data);
                                $scope.grid.select("tr:eq(1)");
                            });
                        }
                    },
                    pageSize: 20,
                });

                $scope.adminOptions = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
                    selectable: true,
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('ADMINSELECTION', this.dataItem(selectedRows[0]).Id);
                    },
                    columns: [{
                        template: "<a ng-click='EditAdmin(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>",
                        title: "Họ tên"
                    },
                    {
                        field: "PositionName",
                        title: "Chức vụ"
                    }],
                };

                $scope.EditAdmin = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/abc/staffDepartment/detail.html',
                        controller: 'adminDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.then(function () {
                        $scope.grid.select("tr:eq(1)");
                    });
                };
            }
    ]);
    app.controller('adminDetailController', ['$scope', '$rootScope', '$modalInstance', 'id', 'staffService', 'departmentService',
        function ($scope, $rootScope, $modalInstance, id, staffService, departmentService) {
            $scope.title = "Đơn vị được quản lý";
            $scope.obj = {};
            staffService.getObj(id).then(function (result) {
                $scope.obj = result.data;
            });
            departmentService.getAdminDepartmentListHierarchy(id, $rootScope.session.CongTyId).then(function (result) {
                $scope.treeData = new kendo.data.HierarchicalDataSource({
                    data: result.data
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
                        $scope.obj.DepartmentIds.push(nodes[i].Id);
                    }

                    if (nodes[i].hasChildren) {
                        checkedNodeIds(nodes[i].children.view(), $scope.obj.DepartmentIds);
                    }
                }
            }
            function onCheck() {
                $scope.obj.DepartmentIds = [];
                var treeView = $scope.departmentTree;
                checkedNodeIds($scope.departmentTree.dataSource.view(), $scope.obj.DepartmentIds);
            }
            departmentService.getList().then(function (result) {
                $scope.departments = result.data;
            });
            $scope.save = function () {
                staffService.SaveDepartmentManage($scope.obj).then(function (result) {
                    if (result == 1) {
                        alert("Thành công!");
                        $modalInstance.close();
                    }
                    else {
                        alert("Thất bại!");
                    }


                });
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
});