
define(['app/app', 'app/services/kpi/securityService'], function (app) {
    "use strict";

    app.controller('roleController', ['$scope', '$modal', '$rootScope', 'securityService',
            function ($scope, $modal, $rootScope, securityService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return securityService.getList().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.agentDataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return securityService.getList().then(function (result) {
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
                        field: "Id",
                        title: "Mã quyền",
                        width:"30%"
                    }, {
                        field: "Name",
                        title: "Quyền"
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
                        templateUrl: '/app/views/kpi/role/detail.html',
                        controller: 'roleDetailController',
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

                $scope.Edit = function (Id) {
                    //var selectedItem = $scope.grid.dataItem($scope.grid.select());

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/role/detail.html',
                        controller: 'roleDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            isnew: function () {
                                return 0;
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

    app.controller('roleDetailController', ['$scope', '$modalInstance','id', 'isnew', 'securityService',
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

});