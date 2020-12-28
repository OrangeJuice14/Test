
define(['app/app', 'app/services/kpi/targetGroupService'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('targetGroupController', ['$scope', '$rootScope', '$modal', 'targetGroupService',
            function ($scope,$rootScope, $modal, targetGroupService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = null;
                $scope.agentObjectId = MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return targetGroupService.getListByClassId($scope.agentObjectId).then(function (result) {
                                options.success(result.data);
                                var row = $scope.grid.tbody.find("tr:first");
                                $scope.grid.select(row);
                                //$scope.grid.select("tr:eq(1)");
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.obj = null;

                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    change: function (e) {
                        var selectedRows = this.select();
                        try {
                            $rootScope.$broadcast('TARGETGROUP_SELECTION', this.dataItem(selectedRows[0]).Id);
                        }
                        catch (e)
                        { }
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


                $scope.$on("AGENTOBJECTSELECTION", function (event, args) {                 
                    $scope.agentObjectId = args;
                    $scope.grid.dataSource.read();                
                });

                $scope.New = function () {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/targetGroup/detail.html',
                        controller: 'targetGroupDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            agentObjectId: function () {
                                return $scope.agentObjectId;
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
                        templateUrl: '/app/views/kpi/targetGroup/detail.html',
                        controller: 'targetGroupDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            agentObjectId: function () {
                                return $scope.agentObjectId;
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

                    targetGroupService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.AgentObject.Id = result.data.AgentObjectId;
                        targetGroupService.Delete($scope.obj).then(function () {
                                $scope.grid.dataSource.read();
                        });
                    });

                };


            }
    ]);

    app.controller('targetGroupDetailController', ['$scope', '$modalInstance', 'id', 'agentObjectId', 'targetGroupService', 'agentObjectService',
  function ($scope, $modalInstance, id, agentObjectId, targetGroupService, agentObjectService) {
      $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;

      agentObjectService.getList().then(function (result) {
          $scope.agentObjects = result.data;

      });

      $scope.obj = {};
      $scope.title = "Chi tiết nhóm mục tiêu";
      if ($scope.isNew) {       
          $scope.formTitle = "Chi tiết đối tượng";
          $scope.obj = {
              Id: MANAGER.GUID_EMPTY,
              Name: "",
              AgentObject: {
                  Id: agentObjectId
              }
          };
      } else {        
          $scope.formTitle = "Chi tiết đối tượng";
          targetGroupService.getObj(id).then(function (result) {
              $scope.obj = result.data;
              $scope.obj.AgentObject.Id = result.data.AgentObjectId;
          });
      }

      $scope.save = function () {
          if ($scope.isNew) {
              targetGroupService.Save($scope.obj).then(function () {
                  $modalInstance.close();
              });
          } else {
              targetGroupService.Save($scope.obj).then(function () {
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