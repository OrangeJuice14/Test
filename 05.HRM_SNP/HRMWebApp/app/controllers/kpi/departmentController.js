define(['app/app', 'app/services/kpi/departmentService', 'app/services/kpi/StaffService'], function (app) {
    "use strict";
    app.controller('departmentController', ['$scope', '$modal', '$rootScope', 'departmentService',
            function ($scope, $modal, $rootScope, departmentService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.dataSource = new kendo.data.TreeListDataSource({
                    transport: {
                        read: function (options) {
                            return departmentService.getDepartmentTreeList().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                parentId: { field: "ParentId", nullable: true },
                                Id: { field: "Id", type: "string" }
                            },
                            expanded: true
                        }

                    },
                    sort: {
                        field: "Name",
                        dir: "asc"
                    }
                });
                $scope.mainGridOptions = {
                    sortable: true,
                    filterable: {
                        messages: {
                            info: "Lọc bởi : ",
                            filter: "Lọc",
                            clear: "Xóa"
                        },
                        extra: false,
                        operators: {
                            string: {
                                contains: "Từ khóa"
                            }
                        }
                    },
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('DEPARTMENTSELECTION', this.dataItem(selectedRows[0]).Id);
                    },
                    scrollable: true,
                    pageable: {
                        buttonCount: 5,
                        pageSize: 20,
                    },
                    selectable: true,
                    columns: [{ field: "Name", title: "Phòng ban - Khoa/Viện/Trung tâm" },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "50px",
                    }],
                };
                $scope.unlockRatingGridOptions = {
                    sortable: true,
                    filterable: {
                        messages: {
                            info: "Lọc bởi : ",
                            filter: "Lọc",
                            clear: "Xóa"
                        },
                        extra: false,
                        operators: {
                            string: {
                                contains: "Từ khóa"
                            }
                        }
                    },
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('UNLOCKDEPARTMENT', this.dataItem(selectedRows[0]).Id);
                    },
                    scrollable: true,
                    pageable: {
                        buttonCount: 5,
                        pageSize: 20,
                    },
                    selectable: true,
                    columns: [{ field: "Name", title: "Đơn vị" }]
                };
                $scope.Edit = function (Id) {

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/department/detail.html',
                        controller: 'departmentDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.then(function () {
                        departmentService.getList().then(function (result) {
                            $scope.grid.dataSource.read();
                        });
                    });
                };
            }
    ]);
    app.controller('departmentDetailController', ['$scope', '$modalInstance', 'id', 'departmentService', 'staffService',
        function ($scope, $modalInstance, id, departmentService, staffService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Quản lý";
            $scope.obj = {};
            staffService.getListByDepartmentId(id).then(function (result) {
                $scope.staffs = result.data;
            });
            departmentService.GetListStaffRole().then(function (result) {
                $scope.staffRoles = result.data;
            });
            $scope.changedValue = function (RoleId, DepartmentId) {
                departmentService.GetStaffId(DepartmentId, RoleId).then(function (result) {
                    $scope.obj.StaffId=result.data;
                });
                
            }
            if ($scope.isNew) {

                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    Name: "",
                };
            } else {

                departmentService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    departmentService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    departmentService.Save($scope.obj).then(function () {
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