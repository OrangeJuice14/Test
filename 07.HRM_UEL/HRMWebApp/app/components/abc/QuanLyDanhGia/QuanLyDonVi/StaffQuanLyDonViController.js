
define(['app/app',
    'app/services/kpi/departmentService',
    'app/services/kpi/webUserService',
    'app/services/abc/ABC_QuanLyDonViService',
    'app/services/abc/ABC_WebUserGroupDanhGiaRoleService'],
    function (app) {
        "use strict";

        app.controller('StaffQuanLyDonViController', ['$scope', '$rootScope', '$modal', 'ABC_QuanLyDonViService','ABC_WebUserGroupDanhGiaRoleService',
            function ($scope, $rootScope, $modal, ABC_QuanLyDonViService, ABC_WebUserGroupDanhGiaRoleService) {

                $scope.GridUserOptions = {
                    sortable: true,
                    pageSize: 20,
                    pageable: {
                        refresh: true,
                    },
                    selectable: true,
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('ADMINSELECTION', this.dataItem(selectedRows[0]).WebUserId);
                    },
                    filterable: true,
                    columns: [
                        {
                            width: "",
                            title: "Họ và tên",
                            template: `<div style='width:auto; display:inline-block; cursor: pointer'><a ng-click="QuanLyDonVi(dataItem.WebUserId,dataItem.WebUserStaffInfoStaffProfileName)">{{dataItem.WebUserStaffInfoStaffProfileName}}</a></div>`,
                            filterable: {
                                search: true
                            }
                        },
                        { width: "", title: "Tên nhóm", template: "<div style='width:auto; display:inline-block; cursor: pointer'>{{dataItem.GroupDanhGiaName}}</div>", filterable: true },
                    ],
                }
                $scope.GridUserData = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return ABC_WebUserGroupDanhGiaRoleService.GetListUserQuanLyDonVi().then(function (result) {
                                options.success(result.data);
                                $scope.grid.select("tr:eq(1)");
                            });
                        }
                    },
                    pageSize: 20,
                });
                $scope.QuanLyDonVi = function (UserId, Name) {
                    if (UserId == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/QuanLyDonVi/QuanLyDonVi.Details.html',
                        controller: 'QuanLyDonViDetailsController',
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

        app.controller('QuanLyDonViDetailsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'ABC_QuanLyDonViService', 'UserId', 'Name', 'departmentService','webUserService',
            function ($scope, $rootScope, $modal, $modalInstance, ABC_QuanLyDonViService, UserId, Name, departmentService, webUserService) {
                $scope.Name = angular.copy(Name);
                $scope.UserId = angular.copy(UserId);
                $scope.LoaddingTree = true;
                webUserService.GetABCWebUserVMDTOByUserId(UserId).then(res => {
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
                        ABC_QuanLyDonViService.GetListQuanLyDonViByUserId($scope.UserId).then(resDVQL => {
                            for (let i = 0; i < res.data.length; i++) {
                                if (resDVQL.data.filter(e => e.DepartmentId == res.data[i].Id).length > 0) {
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


                //ABC_QuanLyDonViService.GetListTreeBoPhan(UserId).then(res => {
                //    if (res != null && res.data != null) {
                //        //res.data.forEach(item => {
                //        //    item.expanded = false;
                //        //})
                //        $scope.treeData = new kendo.data.HierarchicalDataSource({
                //            data: res.data,
                //        });
                //    } else {
                //        Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                //    }
                //    $scope.LoaddingTree = false;
                //})
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
                //$scope.$watch('Search', function (nValue,oValue) {
                //    //console.log(nValue);
                //    //console.log(oValue);
                //    if (nValue != null) {

                //        var query = nValue.toLowerCase();
                //        var dataSource = $scope.menuTree.dataSource;

                //        filter(dataSource, query);
                //    }
                //});
                //function filter(dataSource, query) {
                //    var hasVisibleChildren = false;
                //    var data = dataSource instanceof kendo.data.HierarchicalDataSource && dataSource.data();

                //    for (var i = 0; i < data.length; i++) {
                //        var item = data[i];
                //        var text = item.Name.toLowerCase();
                //        var itemVisible =
                //            query === true // parent already matches
                //            || query === "" // query is empty
                //            || text.indexOf(query) >= 0; // item text matches query

                //        var anyVisibleChildren = filter(item.children, itemVisible || query); // pass true if parent matches

                //        hasVisibleChildren = hasVisibleChildren || anyVisibleChildren || itemVisible;

                //        item.hidden = !itemVisible && !anyVisibleChildren;
                //    }

                //    if (data) {
                //        // Re-apply the filter on the children.
                //        dataSource.filter({ field: "hidden", operator: "neq", value: true });
                //    }
                //    return hasVisibleChildren;
                //}

                $scope.Save = function () {
                    $scope.LoaddingTree = true;
                    ABC_QuanLyDonViService.Save($scope.ListDonViSelected, $scope.UserId).then(res => {
                        if (res == "") {
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