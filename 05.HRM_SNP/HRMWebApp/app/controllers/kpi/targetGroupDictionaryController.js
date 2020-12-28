
define(['app/app', 'app/services/kpi/criterionService'], function (app) {
    "use strict";

    app.controller('targetGroupDictionaryController', ['$scope', '$modal', '$rootScope', 'criterionService',
            function ($scope, $modal, $rootScope, criterionService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];              
                $scope.obj = null;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return criterionService.getListTargetGroupDetail().then(function (result) {
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
                            field: "Name",
                            title: "Tên nhóm mục tiêu"
                        },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
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
                        templateUrl: '/app/views/kpi/targetGroupDictionary/detail.html',
                        controller: 'targetGroupDictionaryDetailController',
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

    app.controller('targetGroupDictionaryDetailController', ['$scope','$modal', '$modalInstance', 'id', 'criterionService',
        function ($scope,$modal, $modalInstance,id,criterionService) {
            $scope.title = "";
            $scope.obj = {};
            $scope.isEdit = false;
            $scope.targetGroupId=id;
            $scope.dataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return criterionService.getListDictionaryByTargetGroupDetailId(id).then(function (result) {
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
                        field: "Name",
                        title: "Tên"
                    },
                    {
                        field: "Record",
                        title: "Số điểm"
                    },
                    {
                        field: "MaxRecord",
                        title: "Điểm giới hạn"
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
            $scope.New = function () {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/targetGroupDictionary/dictionaryDetail.html',
                        controller: 'dictionaryDetailController',
                        resolve: {
                            id: function () {
                                return 0;
                            },
                            isnew: function () {
                                return 1;
                            },
                            targetGroupId: function () {
                                return $scope.targetGroupId;
                            }                         
                        }
                    }).result.finally(function (result) {
                        $scope.grid.dataSource.read();
                    });
            };
            $scope.Edit = function (Id) {
                if (Id == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }

                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/targetGroupDictionary/dictionaryDetail.html',
                    controller: 'dictionaryDetailController',
                    resolve: {
                        id: function () {
                            return Id;
                        },
                        isnew: function () {
                            return 0;
                        },
                        targetGroupId: function () {
                            return $scope.targetGroupId;
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

                criterionService.getCriterionDictionary(Id).then(function (result) {
                    $scope.obj = result.data;
                    criterionService.deleteCriterionDictionary($scope.obj).then(function (result) {
                        if (result == 0) {
                            alert("Đang được sử dụng!")
                        }
                        $scope.grid.dataSource.read();
                    });
                });
            };
            $scope.cancel = function () {
                $modalInstance.close();
            };
        }
    ]);
    app.controller('dictionaryDetailController', ['$scope', '$modalInstance', 'id','targetGroupId', 'isnew', 'criterionService',
       function ($scope, $modalInstance, id,targetGroupId,isnew,  criterionService) {
           $scope.isNew = isnew == 1 ? true : false;
           $scope.title = "Mã quản lý";
           $scope.obj = {};
                $scope.options = {
                    format: "n0",
                    max: 100,
                    min:0
                }
           if ($scope.isNew) {
               $scope.obj = {
                   Id: MANAGER.GUID_EMPTY,
                   Name: "",
                   TargetGroupDetail: { Id: targetGroupId }
               };
           } else {
               criterionService.getCriterionDictionary(id).then(function (result) {
                   $scope.obj = result.data;
               });
           }

           $scope.save = function () {
                   criterionService.SaveCriterionDictionary($scope.obj).then(function (result) {
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