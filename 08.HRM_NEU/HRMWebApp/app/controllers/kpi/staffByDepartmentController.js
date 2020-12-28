
define(['app/app', 'app/services/kpi/agentObjectService', 'app/services/kpi/targetGroupService','app/services/kpi/StaffService', 'app/services/kpi/departmentService'], function (app) {
    "use strict";

    app.controller('staffByDepartmentController', ['$scope', '$modal', '$rootScope', 'agentObjectService', 'staffService', 'departmentService',
            function ($scope, $modal, $rootScope, agentObjectService, staffService, departmentService) {
                $("#sidebar").addClass("menu-compact");
                departmentService.getList().then(function (result) {
                    $scope.departments = result.data;
                });
                $scope.dataSourceAll = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return staffService.getListAll().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });

                $scope.obj = null;
                $scope.mainGridOptionsAll = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
                    selectable: "row",
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('STAFFSELECTION', this.dataItem(selectedRows[0]).Id);
                    },
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
                    columns: [
                        {
                            field: "ManageCode",
                            title: "Mã quản lý",
                            width: "30%"
                        },
                        {
                            field: "Name",
                            title: "Nhân viên"
                        }
                    ],
                };
                $scope.SearchDept = function () {
                    $scope.dataSourceAll = new kendo.data.DataSource({
                        dataType: 'json',
                        transport: {
                            read: function (options) {
                                var deptId = $scope.deptId;
                                return staffService.searchDeptOnlyProfessor(deptId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                };
            }
    ]);

});