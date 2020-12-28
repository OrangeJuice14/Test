
define(['app/app', 'app/services/kpi/criterionService'], function (app) {
    "use strict";

    app.controller('manageCodeController', ['$scope', '$modal', '$rootScope', 'criterionService',
            function ($scope, $modal, $rootScope, criterionService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return criterionService.getListManageCode().then(function (result) {
                                options.success(result.data);
                                $scope.grid.select("tr:eq(1)");
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
                        $rootScope.$broadcast('MANAGESELECTION', this.dataItem(selectedRows[0]).Id);
                    },
                    columns: [{
                        field: "Id",
                        title: "Mã quản lý",
                        width: "100px"
                    }, {
                        field: "Name",
                        title: "Tên mã quản lý"
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
                        templateUrl: '/app/views/kpi/manageCode/detail.html',
                        controller: 'manageCodeDetailController',
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
                        templateUrl: '/app/views/kpi/manageCode/detail.html',
                        controller: 'manageCodeDetailController',
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

                    criterionService.getManageCode(Id).then(function (result) {
                        $scope.obj = result.data;
                        criterionService.DeleteManageCode($scope.obj).then(function (result) {
                            if (result == 0) {
                                alert("Mã quản lý đang được sử dụng!")
                            }
                            $scope.grid.dataSource.read();
                        });
                    });
                };
            }
    ]);

    app.controller('manageCodeDetailController', ['$scope', '$modalInstance', 'id', 'isnew', 'criterionService',
        function ($scope, $modalInstance, id, isnew, criterionService) {
            $scope.isNew = isnew == 1 ? true : false;
            $scope.title = "Mã quản lý";
            $scope.obj = {};
            if ($scope.isNew) {
                $scope.obj = {
                    Id: "",
                    Name: "",
                };
            } else {
                criterionService.getManageCode(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    criterionService.SaveNewManageCode($scope.obj).then(function (result) {
                        if (result == 0) {
                            alert("Trùng Mã quản lý!")
                        }
                        else {
                            $modalInstance.close();
                        }

                    });
                } else {
                    criterionService.SaveManageCode($scope.obj).then(function () {
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