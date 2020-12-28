
define(['app/app', 'app/services/kpi/agentObjectService', 'app/services/kpi/staffRoleService'], function (app) {
    "use strict";

    app.controller('staffRoleController', ['$scope', '$modal', '$rootScope', 'staffRoleService',
            function ($scope, $modal, $rootScope, staffRoleService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.targetGroupDetailId = MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return staffRoleService.getList().then(function (result) {
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
                        field: "Name",
                        title: "Nội dung"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "50px",
                    }],
                };
                var editRender = function (row, columnfield, value, defaulthtml, columnproperties) {
                    return "<button ng-click='Edit(value)' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button>";
                };
                var deleteRender = function (row, columnfield, value, defaulthtml, columnproperties) {
                    return "<button ng-click='Delete(value)' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button>";
                };

                $scope.New = function () {
                    $scope.isEdit = false;

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/staffRole/detail.html',
                        controller: 'staffRoleDetailController',
                        resolve: {
                            id: function () {
                                return 0;
                            }
                        }
                    }).result.finally(function (result) {
                        $scope.grid.dataSource.read();
                    });
                };

                $scope.Edit = function (Id) {
                    //var selectedItem = $scope.grid.dataItem($scope.grid.select());

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/staffRole/detail.html',
                        controller: 'staffRoleDetailController',
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

                    staffRoleService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        staffRoleService.Delete($scope.obj).then(function () {
                            $scope.grid.dataSource.read();
                        });
                    });
                };
            }
    ]);

    app.controller('staffRoleDetailController', ['$scope', '$modalInstance', 'id', 'agentObjectService', 'staffRoleService',
        function ($scope, $modalInstance, id, agentObjectService, staffRoleService) {
            $scope.isNew = id == 0 || '' ? true : false;
            $scope.title = "Quản lý chức danh";
            $scope.obj = {};
            agentObjectService.getList().then(function (result) {
                $scope.agentObjects = result.data;
            });
            if ($scope.isNew) {
                $scope.obj = {
                    Id: 0,
                    Name: "",
                };
            } else {
                staffRoleService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    staffRoleService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    staffRoleService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.close();
            };
        }
    ]);

});