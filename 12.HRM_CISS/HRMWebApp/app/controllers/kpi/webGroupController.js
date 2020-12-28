
define(['app/app', 'app/services/kpi/menuRoleService'], function (app) {
    "use strict";

    app.controller('webGroupController', ['$scope', '$modal', '$rootScope', 'menuRoleService',
            function ($scope, $modal, $rootScope, menuRoleService) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return menuRoleService.getListWebGroup().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                });              
                $scope.mainGridOptions = {              
                    sortable: true,
                    columns: [{ field: "Name", title: dataLocalize("QUAN_LY_MENU_VA_PHAN_QUYEN.PHAN_QUYEN.DANH_SACH_NHOM_NGUOI_DUNG") },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    }],
                };
                $scope.Edit = function (Id) {

                    if (Id == "") {
                        alert(dataLocalize("QUAN_LY_MENU_VA_PHAN_QUYEN.PHAN_QUYEN.ALERT.BAN_CHUA_CHON_PHAN_TU"));
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/menuRole/detail_webGroup.html',
                        controller: 'webGroupDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.then(function () {
                    });
                };
                $scope.uploadOptions = {
                    async: {
                        saveUrl: "/FileUpload/UploadSource",
                        autoUpload: true
                    },
                    upload: function (e) {
                        e.data = { path: $scope.path, backup: $scope.backup };
                    },
                    success: function (result) {
                        console.log(dataLocalize("QUAN_LY_MENU_VA_PHAN_QUYEN.PHAN_QUYEN.ALERT.THANH_CONG"));
                    }
                }
            }
    ]);
    app.controller('webGroupDetailController', ['$scope', '$modalInstance', 'id', 'menuRoleService',
        function ($scope, $modalInstance, id, menuRoleService) {

            $scope.title = "Quản lý";
            $scope.obj = {};
            $scope.parentMenuList = {};
            $scope.options = {
                format: "n0",
                min: 0
            }
            function checkedNodeIds(nodes, checkedNodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].checked) {
                        checkedNodes.push(nodes[i].Id);
                    }

                    if (nodes[i].hasChildren) {
                        checkedNodeIds(nodes[i].children.view(), checkedNodes);
                    }
                }
            }
            function onCheck() {
                $scope.obj.WebMenuIds = [];
                checkedNodeIds($scope.menuTree.dataSource.view(), $scope.obj.WebMenuIds);
            }
            $scope.treeOptions = {
                checkboxes: {
                    checkChildren: true
                },
                dataTextField: ["Name", "Name"],
                check: onCheck,
                dataBound: treeDataBound
            };
            function treeDataBound(e) {
                this.expand(".k-item");
            }
            menuRoleService.getWebMenuHierarchy(id).then(function (result) {
                $scope.treeData = new kendo.data.HierarchicalDataSource({
                    data: result.data
                });

            });
            menuRoleService.getWebGroup(id).then(function (result) {
                $scope.obj = result.data;
                });
            $scope.save = function () {
                menuRoleService.SaveWebGroup($scope.obj).then(function (result) {
                    if (result == 1)
                    {
                        alert(dataLocalize("QUAN_LY_MENU_VA_PHAN_QUYEN.PHAN_QUYEN.ALERT.THANH_CONG"));
                        $modalInstance.close();
                    }
                    else
                    {
                        alert(dataLocalize("QUAN_LY_MENU_VA_PHAN_QUYEN.PHAN_QUYEN.ALERT.THAT_BAI"));
                    }
                       
                    });
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
});