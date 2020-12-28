define(['app/app', 'app/services/abc/departmentService', 'app/services/abc/staffService'], function (app) {
    "use strict";
    app.controller('departmentController', ['$scope', '$modal', '$rootScope', 'departmentService', 'staffService',
            function ($scope, $modal, $rootScope, departmentService, staffService) {
                $scope.adminId = MANAGER.GUID_EMPTY;

                $scope.departmentDataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            var adminId = $scope.adminId;
                            return departmentService.getDepartmentManagedByAdmin(adminId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20,
                });
                $scope.departmentOptions = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
                    selectable: true,
                    columns: [{
                        field: "Name",
                        title: "Đơn vị"
                    }
                    ],
                };
                $scope.$on("ADMINSELECTION", function (event, args) {
                    $scope.adminId = args;
                    $scope.grid.dataSource.read();
                });
            }
    ]);
});