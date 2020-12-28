
define(['app/app', 'app/services/kpi/targetGroupDetailService', 'app/services/kpi/agentObjectService'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('targetGroupDetail_ManageController', ['$scope', '$modal', '$rootScope', 'targetGroupDetailService',
            function ($scope, $modal,$rootScope, targetGroupDetailService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = null;
                $scope.agentObjectId = MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return targetGroupDetailService.getListByClassId($scope.agentObjectId).then(function (result) {
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
                        $rootScope.$broadcast('TARGETGROUPDETAILSELECTION', this.dataItem(selectedRows[0]).Id);
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
                        templateUrl: '/app/views/kpi/targetGroupDetail/detail.html',
                        controller: 'targetGroupDetail_DetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            agentObjectId: function () {
                                return $scope.agentObjectId;
                            }
                        }
                    }).result.then(function (result) {
                        $scope.grid.dataSource.read();
                    });
                };
                $scope.Show = function () {
                    $scope.agentObjectId = MANAGER.GUID_EMPTY;
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return targetGroupDetailService.getListByClassId($scope.agentObjectId).then(function (result) {
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
                        templateUrl: '/app/views/kpi/targetGroupDetail/detail.html',
                        controller: 'targetGroupDetail_DetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            agentObjectId: function () {
                                return $scope.agentObjectId;
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

                    targetGroupDetailService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        //$scope.obj.TargetGroup.Id = result.data.agentObjectId;
                        targetGroupDetailService.Delete($scope.obj).then(function () {
                                $scope.grid.dataSource.read();
                        });
                    });

                };


            }
    ]);

    app.controller('targetGroupDetail_DetailController', ['$scope', '$modalInstance', 'id', 'agentObjectId', 'targetGroupDetailService', 'targetGroupService','agentObjectService',
        function ($scope, $modalInstance, id, agentObjectId, targetGroupDetailService, targetGroupService, agentObjectService) {
       $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
       $scope.options = {
           format: "#'%'",
           max: 100
       }
       targetGroupService.getList().then(function (result) {
           $scope.targetGroups = result.data;
       });

       $scope.agentObjects = {
           placeholder: "Chọn đối tượng...",
           dataTextField: "Name",
           dataValueField: "Id",
           valuePrimitive: true,
           autoBind: false,
           dataSource: {
               transport: {
                   read: function (options) {
                       return agentObjectService.getList().then(function (result) {
                           options.success(result.data);
                       });
                   }
               }
           }
       };


       $scope.obj = {};
       $scope.title = "Chi tiết Nhóm mục tiêu - chi tiết";
       if ($scope.isNew) {
           $scope.formTitle = "Chi tiết đối tượng";
           $scope.obj = {
               Id: MANAGER.GUID_EMPTY,
               Name: "",
               TargetGroup: {
                   Id: agentObjectId
               }
           };
       } else {
           $scope.formTitle = "Chi tiết nhóm mục tiêu";
           targetGroupDetailService.getObj(id).then(function (result) {
               $scope.obj = result.data;
               $scope.obj.TargetGroup.Id = result.data.agentObjectId;
           });
       }

       $scope.save = function () {
           if ($scope.isNew) {
               targetGroupDetailService.Save($scope.obj).then(function () {
                   $modalInstance.close();
               });
           } else {
               targetGroupDetailService.Save($scope.obj).then(function () {
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