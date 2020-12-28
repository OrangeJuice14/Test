
define(['app/app', 'app/services/kpi/agentObjectService'], function (app) {
    "use strict";

    app.controller('agentObjectTypeRateController', ['$scope', '$modal', '$rootScope', 'agentObjectService',
            function ($scope, $modal, $rootScope, agentObjectService) {
                $scope.grid = {};
                $scope.obj = null;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return agentObjectService.getAgentObjectTypeRateList().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: "row",
                    columns: [
                        {
                            field: "AgentObjectTypeName",
                            title: "Loại chức danh"
                        },
                        {
                            field: "ResultRate",
                            title: "Tỷ trọng"
                        },

                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.AgentObjectTypeId #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    }],
                };
                $scope.Edit = function (Id) {

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/agentObjectTypeRate/detail.html',
                        controller: 'agentObjectTypeRateDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.finally(function () {
                        $scope.grid.dataSource.read();
                    });
                }
            }
    ]);
    app.controller('agentObjectTypeRateDetailController', ['$scope', '$modalInstance', 'id', 'agentObjectService',
       function ($scope, $modalInstance, id, agentObjectService) {
           $scope.obj = {};
           $scope.options = {
               format: "n0",
               min: 0
           }
           agentObjectService.getAgentObjectTypeRateById(id).then(function (result) {
               $scope.obj = result.data;
           });

           $scope.save = function () {
               agentObjectService.saveAgentObjectTypeRate($scope.obj).then(function (result) {
                   if (result == 0) {
                       alert("Lỗi!")
                   }
                   else {
                       alert("Thành công!")
                       $modalInstance.close();
                   }

               });
           };

           $scope.cancel = function () {
               $modalInstance.close();
           };
       }
    ]);
});