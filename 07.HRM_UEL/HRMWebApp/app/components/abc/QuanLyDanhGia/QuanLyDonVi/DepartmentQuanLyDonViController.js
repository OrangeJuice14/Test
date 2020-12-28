
define(['app/app', 'app/services/abc/ABC_QuanLyDonViService',], function (app) {
    "use strict";

    app.controller('DepartmentQuanLyDonViController', ['$scope', '$rootScope', '$modal', 'ABC_QuanLyDonViService',
        function ($scope, $rootScope, $modal, ABC_QuanLyDonViService) {
            $scope.adminId = MANAGER.GUID_EMPTY;
            $scope.ShowDepartment = false;
            $scope.departmentDataSource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return ABC_QuanLyDonViService.GetListQuanLyDonViByUserId($scope.adminId).then(function (result) {
                            options.success(result.data);
                            $scope.ShowDepartment = true;
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
                    field: "DepartmentName",
                    title: "Đơn vị",
                }],
                filterable: true,
            };
            $scope.$on("ADMINSELECTION", function (event, args) {
                $scope.adminId = args;
                $scope.ShowDepartment = false;
                $scope.grid.dataSource.read();
            });
        }
    ]);
});