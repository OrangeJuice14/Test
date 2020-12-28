
define(['app/app',
    'app/services/kpi/webUserService',
    'app/services/kpi/departmentService',
    'app/services/kpi/PhanQuyenBoPhanDuToanService',],
    function (app) {
        "use strict";
        app.controller('StaffPhanQuyenBoPhanDuToanController', ['$scope', '$rootScope', '$modal', 'webUserService', 'departmentService',
            function ($scope, $rootScope, $modal, webUserService, departmentService) {
                $scope.ShowStaff = false;

                $scope.ListBoPhan = {
                    filter: "contains",
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: true,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return departmentService.getList().then(res => {
                                    options.success(res.data);
                                    $scope.BoPhanId = res.data[0].Id;
                                    var GetListHaveStaffInfoPromise = new Promise((resolve, reject) => {
                                        webUserService.GetListHaveStaffInfoByDepartmentId($scope.BoPhanId).then(function (result) {
                                            $scope.GridUserData = new kendo.data.DataSource({
                                                data: result.data,
                                                pageSize: 20,
                                            });
                                            resolve();
                                        });
                                    })

                                    GetListHaveStaffInfoPromise.then(() => {
                                        $scope.ShowStaff = true;
                                        $scope.grid.select("tr:eq(1)");
                                        $scope.$apply();
                                    })
                                })
                            }
                        }
                    },
                    change: function () {
                        var GetListHaveStaffInfoPromise = new Promise((resolve, reject) => {
                            webUserService.GetListHaveStaffInfoByDepartmentId($scope.BoPhanId).then(function (result) {
                                $scope.GridUserData = new kendo.data.DataSource({
                                    data: result.data,
                                    pageSize: 20,
                                });
                                resolve();
                            });
                        })

                        GetListHaveStaffInfoPromise.then(() => {
                            $scope.ShowStaff = true;
                            $scope.grid.select("tr:eq(1)");
                            $scope.$apply();
                        })
                        //$scope.GetList(1, $scope.BoPhanId);
                        ////$scope.GridUser.dataSource.sort({ field: "StaffInfoStaffProfileName", dir: "desc" });
                        //$scope.$apply();
                        ////this.refresh();
                    }
                }

                $scope.GridUserOptions = {
                    sortable: true,
                    pageSize: 20,
                    pageable: {
                        refresh: true,
                    },
                    selectable: true,
                    change: function (e) {
                        $rootScope.$broadcast('ADMINSELECTION', this.dataItem($scope.grid.select()).Id);
                    },
                    filterable: true,
                    columns: [
                        {
                            width: "",
                            title: "Họ và tên",
                            template: `<div style='width:auto; display:inline-block; cursor: pointer'><a ng-click="QuanLyDonVi(dataItem.Id,dataItem.StaffInfoStaffProfileName)">{{dataItem.StaffInfoStaffProfileName}}</a></div>`,
                            filterable: {
                                search: true
                            }
                        },
                        { width: "", title: "Đơn vị", template: "<div style='width:auto; display:inline-block; cursor: pointer'>{{dataItem.StaffInfoStaffDepartmentName}}</div>", filterable: true },
                    ],
                }

                $scope.QuanLyDonVi = function (UserId, Name) {
                    if (UserId == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/PhanQuyenBoPhanDuToan/PhanQuyenBoPhanDuToan.Details.html',
                        controller: 'PhanQuyenBoPhanDuToanDetailsController',
                        resolve: {
                            UserId: function () {
                                return UserId;
                            },
                            Name: function () {
                                return Name;
                            }
                        }
                    }).result.then(function () {
                        $scope.grid.select("tr:eq(1)");
                    });
                }
            }
        ]);
        app.controller('DepartmentPhanQuyenBoPhanDuToanController', ['$scope', '$rootScope', '$modal', 'PhanQuyenBoPhanDuToanService','departmentService',
            function ($scope, $rootScope, $modal, PhanQuyenBoPhanDuToanService, departmentService) {
                $scope.adminId = MANAGER.GUID_EMPTY;
                $scope.ShowDepartment = false;
                $scope.departmentDataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            var adminId = $scope.adminId;
                            return PhanQuyenBoPhanDuToanService.GetPhanQuyenBoPhanDuToanByUserId(adminId).then(function (result) {
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
                        field: "Name",
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

        app.controller('PhanQuyenBoPhanDuToanDetailsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'UserId', 'Name', 'webUserService', 'departmentService','PhanQuyenBoPhanDuToanService',
            function ($scope, $rootScope, $modal, $modalInstance, UserId, Name, webUserService, departmentService, PhanQuyenBoPhanDuToanService) {
                $scope.Name = angular.copy(Name);
                $scope.UserId = angular.copy(UserId);
                $scope.LoaddingTree = true;
                webUserService.GetUser(UserId).then(res => {
                    $scope.UserNow = res.data;
                });
                $scope.ListDonViSelected = [];
                $scope.ListDonVi = {};


                function ListToTree(list) {
                    var map = {}, node, roots = [], i;
                    for (i = 0; i < list.length; i += 1) {
                        map[list[i].Id] = i; // initialize the map
                        list[i].items = []; // initialize the children
                    }
                    for (i = 0; i < list.length; i += 1) {
                        node = list[i];

                        if (node.ParentDepartmentId != null) {
                            // if you have dangling branches check that map[node.parentId] exists
                            list[map[node.ParentDepartmentId]].items.push(node);
                            list[map[node.ParentDepartmentId]].expanded = true;
                        } else {
                            roots.push(node);
                        }
                    }
                    return roots;
                }

                departmentService.GetListAll().then(res => {
                    if (res != null && res.data != null) {
                        PhanQuyenBoPhanDuToanService.GetPhanQuyenBoPhanDuToanByUserId($scope.UserId).then(resDVQL => {
                            for (let i = 0; i < res.data.length; i++) {
                                if (resDVQL.data.filter(e => e.Id == res.data[i].Id).length > 0) {
                                    res.data[i].checked = true;
                                    $scope.ListDonViSelected.push(res.data[i].Id);
                                }
                            }
                            $scope.ListDonVi = ListToTree(res.data);

                            $scope.treeData = new kendo.data.HierarchicalDataSource({
                                data: $scope.ListDonVi,
                            });
                            $scope.LoaddingTree = false;
                        });
                    } else {
                        Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                    }
                });

                function checkedNodeIds(nodes) {
                    for (var i = 0; i < nodes.length; i++) {
                        if (nodes[i].checked) {
                            $scope.ListDonViSelected.push(nodes[i].Id);
                        }

                        if (nodes[i].hasChildren) {
                            checkedNodeIds(nodes[i].children.view());
                        }
                    }
                }
                function onCheck() {
                    $scope.DisableBtnSave = true;
                    $scope.ListDonViSelected = [];
                    checkedNodeIds($scope.menuTree.dataSource.view());
                    $scope.DisableBtnSave = false;
                }

                $scope.treeOptions = {
                    checkboxes: {
                        checkChildren: true
                    },
                    dataTextField: ["Name", "Name"],

                    check: onCheck,
                    //expanded: true,
                    dataBound: treeDataBound,

                };
                function treeDataBound(e) {

                    this.expand(".k-item");

                }

                $scope.Save = function () {
                    $scope.LoaddingTree = true;
                    PhanQuyenBoPhanDuToanService.Save($scope.ListDonViSelected, $scope.UserId).then(res => {
                        if (res.data == 1) {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $scope.LoaddingTree = false;
                            $modalInstance.close();
                        } else if (res == "ERRORS") {
                            Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                        }
                    })
                }
            }
        ]);
    });