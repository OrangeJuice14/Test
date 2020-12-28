
define(['app/app',
    'app/services/abc/ABC_WebUserGroupDanhGiaRoleService',
    'app/services/kpi/departmentService',
    'app/services/kpi/webUserService',
    'app/services/abc/ABC_GroupDanhGiaService',],
    function (app) {
        "use strict";

        app.controller('UserGroupDanhGiaRoleController', ['$scope', '$rootScope', '$modal', 'ABC_WebUserGroupDanhGiaRoleService','departmentService',
            function ($scope, $rootScope, $modal, ABC_WebUserGroupDanhGiaRoleService, departmentService) {
                $scope.indexPage = 1;
                $scope.Loadding = true;
                $scope.BoPhanId = null;
                $scope.FilterListRole = function FilterListRole(obj) {
                    let list = $scope.ListRole.filter(e => e.UserId == obj.Id)
                    if (list.lenght > 0) {
                        return list[0].GroupDanhGiaName;
                    }
                    return "";
                }

                departmentService.GetListAll().then(res => {
                    $scope.ListBoPhan = res.data
                })

                $scope.ListBoPhan = {
                    filter: "contains",
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return departmentService.GetListAll().then(res => {

                                    options.success(res.data.filter(e => e != null));
                                })
                            }
                        }
                    },
                    change: function () {
                        $scope.GetList(1, $scope.BoPhanId);
                        //$scope.GridUser.dataSource.sort({ field: "StaffInfoStaffProfileName", dir: "desc" });
                        $scope.$apply();
                        //this.refresh();
                    }
                }


                $scope.GetList = function (indexPage, maBoPhan) {
                    $scope.Loadding = true;
                    $scope.GridUserData = new kendo.data.DataSource({
                        pageSize: 20,
                        page: indexPage,
                        serverPaging: true,
                        type: "json",
                        transport: {
                            read: {
                                url: "/Api/WebUserApi/GetAllUser?maBoPhan=" + maBoPhan,
                                type: "GET",
                                dataType: "json",
                                data: {}
                            }
                        },
                        schema: {
                            total: "Total",
                            data: function (e) {
                                $scope.Loadding = false;
                                $scope.indexPage = e.IndexPage;
                                $scope.$apply();
                                return e.Users; // inspect 'e' as to where your data resides  
                            },
                            model: {
                                id: "Id",
                                fields: {
                                    Id: { type: "Id" },
                                    StaffInfoStaffProfileName: { type: "string" },
                                    GroupDanhGiaName: { type: "string" }
                                }
                            }
                        },
                    });
                }

                $scope.GridUserOptions = {
                    pageSize: 20,
                    pageable: {
                        refresh: true,
                    },
                    sortable: true,
                    selectable: true,
                    serverFiltering: true,
                    columns: [
                        { width: "30px", title: "", template: "<div v style='text-align:center; width:100%; '><input type='checkbox' ng-model='dataItem.IsChecked'></div>", filterable: true },
                        { width: "140px", title: "Số hiệu công chức", template: "<div style='text-align:center; width:100%; display:inline-block; cursor: pointer'>{{dataItem.WebUserUserName}}</div>", filterable: true },
                        {
                            width: "",
                            title: "Họ và tên",
                            template: `<div style='text-align:center; width:auto; display:inline-block; cursor: pointer'>{{dataItem.WebUserStaffInfoStaffProfileName}}</div>`,
                            filterable: true
                        },
                        { width: "", title: "Tên nhóm", sortable: { allowUnsort: true }, template: "<div style='text-align:center; width:auto; display:inline-block; cursor: pointer''>{{dataItem.GroupDanhGiaName}}</div>", filterable: true },
                        { width: "60px", title: "", template: "<div style='width: 30px;'><button ng-click='Details(dataItem.WebUserId)' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i></button></div>" },
                    ]
                }
                //ABC_WebUserGroupDanhGiaRoleService.GetUserGroupDanhGiaRole().then(resp => {
                //    $scope.ListRole = resp.data;
                //    //$scope.GetList(1,null);
                //})

                $scope.Details = function (UserId) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/UserGroupDanhGiaRole/Details.html',
                        controller: 'UserGroupDanhGiaRoleDetailsController',
                        size: 'sm',
                        resolve: {
                            UserId: function () {
                                return UserId;
                            },
                        }
                    }).result.then(function () {
                        $scope.GetList($scope.indexPage, $scope.BoPhanId);
                    });
                }
                $scope.Adds = function () {
                    var ListUser = $scope.GridUser.dataSource._data.filter(e => e.IsChecked == true);
                    switch (ListUser.length) {
                        case 0: {
                            Notify('Vui lòng chọn User cần thêm', 'top-right', '3000', 'error', 'exclamation-circle', true);
                            break;
                        }
                        case 1: {
                            $scope.Details(ListUser[0].Id);
                            break;
                        }
                        default: {
                            $modal.open({
                                animation: true,
                                templateUrl: '/app/components/abc/QuanLyDanhGia/UserGroupDanhGiaRole/Adds.html',
                                controller: 'UserGroupDanhGiaRoleAddsController',
                                size: 'sm',
                                resolve: {
                                    ListUser: function () {
                                        return ListUser;
                                    },
                                }
                            }).result.then(function () {
                                $scope.GetList($scope.indexPage, $scope.BoPhanId);
                            });
                            break;
                        }
                    }
                }
            }
        ]);

        app.controller('UserGroupDanhGiaRoleDetailsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'UserId', 'ABC_WebUserGroupDanhGiaRoleService', 'ABC_GroupDanhGiaService','webUserService',
            function ($scope, $rootScope, $modal, $modalInstance, UserId, ABC_WebUserGroupDanhGiaRoleService, ABC_GroupDanhGiaService, webUserService) {
                $scope.UserId = angular.copy(UserId);
                $scope.ListGroupDanhGiaId = [];
                //var GetUserPromise = new Promise(function (resolve, reject) {
                    webUserService.GetABCWebUserVMDTOByUserId($scope.UserId).then(res => {
                        $scope.User = res.data;
                    })
                //});
                var ListGroupOptionPromise = new Promise(function (resolve, reject) {
                    $scope.ListGroupOption = {
                        dataTextField: "Name",
                        dataValueField: "Id",
                        placeholder: "Chọn tiêu chí...",
                        valuePrimitive: true,
                        autoBind: true,
                        dataSource: {
                            transport: {
                                read: function (options) {
                                    return ABC_GroupDanhGiaService.GetAll().then(res => {
                                        options.success(res.data);
                                        resolve();
                                    });
                                }
                            }
                        }
                    }
                })

                ListGroupOptionPromise.then(function () {
                    ABC_WebUserGroupDanhGiaRoleService.GetUserGroupDanhGiaRoleByUserId($scope.UserId).then(res => {
                        $scope.listRole = res.data;
                        $scope.listRole.forEach(role => {
                            $scope.ListGroupDanhGiaId.push(role.GroupDanhGiaId);
                        });
                    });
                });

                $scope.Save = function () {
                    $scope.listRole = [];
                    $scope.EmptyUserGroupDanhGiaRole = {
                        WebUserId: $scope.User.WebUserId,
                        GroupDanhGiaId: "",
                    };
                    $scope.ListGroupDanhGiaId.forEach(groupDanhGiaId => {
                        $scope.EmptyUserGroupDanhGiaRole.GroupDanhGiaId = groupDanhGiaId;
                        $scope.listRole.push(angular.copy($scope.EmptyUserGroupDanhGiaRole));
                    });
                    ABC_WebUserGroupDanhGiaRoleService.SaveOrUpdate($scope.listRole, $rootScope.session.UserId).then(res => {
                        switch (res) {
                            case "ERRORS":
                                {
                                    Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                                    break;
                                }
                            case "SUCCESS":
                                {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $modalInstance.close();
                                    break;
                                }
                            default:
                                {
                                    alert(res);
                                    //Notify(res, 'top-right', '3000', 'warning', 'fa-check', true);
                                    $modalInstance.close();
                                    break;
                                }
                        }
                    })
                }
            }
        ]);
        app.controller('UserGroupDanhGiaRoleAddsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'ListUser', 'ABC_WebUserGroupDanhGiaRoleService', 'ABC_GroupDanhGiaService',
            function ($scope, $rootScope, $modal, $modalInstance, ListUser, ABC_WebUserGroupDanhGiaRoleService, ABC_GroupDanhGiaService) {
                $scope.ListUser = angular.copy(ListUser);
                var ListGroupOptionPromise = new Promise(function (resolve, reject) {
                    $scope.ListGroupOption = {
                        dataTextField: "Name",
                        dataValueField: "Id",
                        placeholder: "Chọn tiêu chí...",
                        valuePrimitive: true,
                        autoBind: false,
                        dataSource: {
                            transport: {
                                read: function (options) {
                                    return ABC_GroupDanhGiaService.GetAll().then(res => {
                                        options.success(res.data);
                                        resolve();
                                    });
                                }
                            }
                        }
                    }
                })

                ListGroupOptionPromise.then(function () {

                })

                $scope.Save = function () {
                    if ($scope.GroupDanhGiaSelectedId != undefined) {
                        ABC_WebUserGroupDanhGiaRoleService.SaveAdds($scope.ListUser, $scope.GroupDanhGiaSelectedId, $rootScope.session.UserId).then(res => {
                            switch (res) {
                                case "ERRORS":
                                    {
                                        Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                                        break;
                                    }
                                case "SUCCESS":
                                    {
                                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                        $modalInstance.close();
                                        break;
                                    }
                                default:
                                    {
                                        alert(res);
                                        //Notify(res, 'top-right', '3000', 'warning', 'fa-check', true);
                                        $modalInstance.close();
                                        break;
                                    }
                            }
                        })
                    } else {
                        Notify('Vui lòng chọn nhóm đánh giá', 'top-right', '3000', 'error', 'exclamation-circle', true);
                    }

                }
            }
        ]);
    });