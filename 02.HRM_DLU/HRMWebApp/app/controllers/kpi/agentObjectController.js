
define(['app/app', 'app/services/kpi/agentObjectService', 'app/services/kpi/targetGroupService'], function (app) {
    "use strict";

    app.controller('agentObjectController', ['$scope', '$modal', '$rootScope', 'agentObjectService','targetGroupService',
            function ($scope, $modal, $rootScope, agentObjectService, targetGroupService) {
                $("#sidebar").addClass("menu-compact");

                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.targetGroupDetailId = MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return agentObjectService.getListByClassId($scope.targetGroupDetailId).then(function (result) {
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
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('AGENTOBJECTSELECTION', this.dataItem(selectedRows[0]).Id);
                    },
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
                $scope.$on("TARGETGROUPDETAILSELECTION", function (event, args) {
                    $scope.targetGroupDetailId = args;
                    $scope.grid.dataSource.read();
                });

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
                        templateUrl: '/app/views/kpi/agentObject/detail.html',
                        controller: 'agentObjectDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            }
                        }
                    }).result.then(function (result) {
                            $scope.grid.dataSource.read();
                    });
                };

                $scope.Show = function () {
                    $scope.targetGroupDetailId = MANAGER.GUID_EMPTY;
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return agentObjectService.getListByClassId($scope.targetGroupDetailId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
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
                        templateUrl: '/app/views/kpi/agentObject/detail.html',
                        controller: 'agentObjectDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
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

                    agentObjectService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        agentObjectService.Delete($scope.obj).then(function () {
                                $scope.grid.dataSource.read();
                        });
                    });
                };
            }
    ]);

    app.controller('agentObjectDetailController', ['$scope', '$modalInstance', 'id', 'agentObjectService','targetGroupDetailService',
        function ($scope, $modalInstance, id, agentObjectService, targetGroupDetailService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết đối tượng";
            $scope.obj = {};
            $scope.options = {
                format: "# tiết",
                max: 1000
            }
            agentObjectService.GetListAgentObjectType().then(function (result) {
                $scope.agentObjectTypes = result.data;
            });
            $scope.targetGroupDetails = {
                placeholder: "Chọn nhóm mục tiêu...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                autoBind: false,
                dataSource: {
                    transport: {
                        read: function (options) {
                            return targetGroupDetailService.getList().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }
            };

            

            if ($scope.isNew) {
                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    Name: "",
                };
            } else {
                agentObjectService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    agentObjectService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    agentObjectService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);

});