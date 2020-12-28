
define(['app/app', 'app/services/kpi/staffRoleService','app/services/kpi/agentObjectService'], function (app) {
    "use strict";

    app.controller('positionController', ['$scope', '$modal', '$rootScope', 'staffRoleService',
            function ($scope, $modal, $rootScope, staffRoleService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return staffRoleService.getListPosition().then(function (result) {
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
                        title: "Chức vụ"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    }]
                };                
                $scope.Edit = function (Id) {
                     if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/position/detail.html',
                        controller: 'positionDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                    });
                }               
            }
    ]);

    app.controller('positionDetailController', ['$scope', '$modalInstance', 'id', 'agentObjectService', 'staffRoleService',
        function ($scope, $modalInstance, id, agentObjectService, staffRoleService) {
            $scope.isNew = id == 0 || '' ? true : false;
            $scope.title = "Quản lý chức vụ";
            $scope.obj = {};
            agentObjectService.GetListAgentObjectType().then(function (result) {
                $scope.agentObjectTypes = result.data;
            });
            if ($scope.isNew) {
                $scope.obj = {
                    Id: 0,
                    Name: "",
                };
            } else {
                staffRoleService.getPositionObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    staffRoleService.SavePosition($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    staffRoleService.SavePosition($scope.obj).then(function () {
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