
define(['app/app', 'app/services/kpi/securityService'], function (app) {
    "use strict";

    app.controller('agentObjectTypeController', ['$scope', '$modal', '$rootScope', 'securityService',
            function ($scope, $modal, $rootScope, securityService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return securityService.getListAgent().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.obj = null;
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: "row",
                    columns: [{
                        template: "<a ng-click='EditAgent(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>",
                        title: "Loại đối tượng"
                    }]
                };
                $scope.New = function () {
                    $scope.isEdit = false;

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/role/detail.html',
                        controller: 'agentObjectTypeController',
                        resolve: {
                            id: function () {
                                return 0;
                            },
                            isnew: function () {
                                return 1;
                            }
                        }
                    }).result.finally(function (result) {
                        $scope.grid.dataSource.read();
                    });
                };

                $scope.EditAgent = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/role/role_Detail.html',
                        controller: 'agentObjectTypeRoleController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.finally(function () {
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

                    securityService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        securityService.Delete($scope.obj).then(function (result) {
                            if (result == 0)
                            {
                                alert("Đang được sử dụng!")
                            }
                            $scope.grid.dataSource.read();
                        });
                    });
                };
            }
    ]);

    app.controller('agentObjectTypeDetailController', ['$scope', '$modalInstance','id', 'isnew', 'securityService',
        function ($scope, $modalInstance,id, isnew,securityService) {
            $scope.isNew = isnew == 1 ? true : false;
            $scope.title = "Quyền";
            $scope.obj = {};
            if ($scope.isNew) {
                $scope.obj = {
                    Id: "",
                    Name: "",
                };
            } else {
                securityService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    securityService.Save($scope.obj).then(function (result) {
                        if (result == 0) {
                            alert("Trùng mã!")
                        }
                        else
                        {
                         $modalInstance.close();
                        }
                       
                    });
                } else {
                    securityService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.close();
            };
        }
    ]);
    app.controller('agentObjectTypeRoleController', ['$scope', '$modalInstance', 'id', 'securityService',
       function ($scope, $modalInstance, id, securityService) {
           $scope.title = "Loại đối tượng";
           $scope.obj = {};
           securityService.getAgentObj(id).then(function (result) {
               $scope.obj = result.data;
           });
           securityService.getRoleHierarchy(id).then(function (result) {
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
                       $scope.obj.RoleIds.push(nodes[i].Id);
                   }

                   if (nodes[i].hasChildren) {
                       checkedNodeIds(nodes[i].children.view(), $scope.obj.RoleIds);
                   }
               }
           }
           function onCheck() {
               $scope.obj.RoleIds = [];
               var treeView = $scope.departmentTree;
               checkedNodeIds($scope.departmentTree.dataSource.view(), $scope.obj.RoleIds);
           }
           $scope.save = function () {
               securityService.SaveAOTR($scope.obj).then(function (result) {
                   if (result == 1) {
                       alert("Thành công!");
                       $modalInstance.close();
                   }
                   else if (result == 2) {
                       alert("Đơn vị đã được người khác quản lý!");
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