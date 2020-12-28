
define(['app/app',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiService',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiRoleService',
    'app/components/abc/QuanLyDanhGia/GroupDanhGia/GroupDanhGiaService',
    'app/components/abc/QuanLyDanhGia/DieuKienBoTieuChi/DieuKienBoTieuChiService'], function (app) {
        "use strict";

        app.controller('BoTieuChiController', ['$scope', '$rootScope', '$modal', '$state', 'BoTieuChiService',
            function ($scope, $rootScope, $modal, $state, BoTieuChiService) {

                var currentTime = new Date();
                $scope.nam = currentTime.getFullYear();
                $scope.GetBoTieuChi = function () {
                    BoTieuChiService.GetAll().then(res => {
                        $scope.ListBoTieuChi = res.data;
                        $scope.ListBoTieuChi.forEach(item => {
                            item.TuNgay = moment(item.TuNgay).format("DD/MM/YYYY");
                            item.DenNgay = moment(item.DenNgay).format("DD/MM/YYYY");
                        })
                    })
                }
                $scope.GetBoTieuChi();
                $scope.New = function () {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChi.Details.html',
                        controller: 'BoTieuChiDetailsController',
                        size: 'lg',
                        resolve: {
                            BoTieuChi: function () {
                                return null;
                            },
                        }
                    }).result.then(function () {
                        $scope.GetBoTieuChi();
                    });
                }
                $scope.edit = function (obj) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChi.Details.html',
                        controller: 'BoTieuChiDetailsController',
                        size: 'lg',
                        resolve: {
                            BoTieuChi: function () {
                                return obj;
                            },
                        }
                    }).result.then(function () {
                        $scope.GetBoTieuChi();
                    });
                }
                $scope.delete = function (BoTieuChiId) {
                    BoTieuChiService.DeleteById(BoTieuChiId, $rootScope.session.UserId).then(res => {
                        switch (res.data) {
                            case 1:
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                $scope.GetBoTieuChi();
                                break;
                            case 2:
                                alert("Bộ tiêu chí đã được đánh giá. Không thể xóa! Vui lòng liên hệ kỹ thuật viên");
                                break;
                            default:
                                Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                                break
                        }
                    })
                }
                $scope.ShowTieuChi = function (item) {
                    $state.go("TieuChi", { BoTieuChiId: item.Id });
                }
                $scope.Role = function (obj) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChi.Role.html',
                        controller: 'BoTieuChiRoleController',
                        size: 'lg',
                        resolve: {
                            BoTieuChi: function () {
                                return obj;
                            },
                        }
                    }).result.then(function () {
                        $scope.GetBoTieuChi();
                    });
                }
            }
        ]);

        app.controller('BoTieuChiDetailsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'BoTieuChiService', 'BoTieuChi', 'DieuKienBoTieuChiService', 'BoTieuChiRoleService', 'GroupDanhGiaService',
            function ($scope, $rootScope, $modal, $modalInstance, BoTieuChiService, BoTieuChi, DieuKienBoTieuChiService, BoTieuChiRoleService, GroupDanhGiaService) {
                $scope.ListDieuKienBoTieuChi = [];
                $scope.DieuKienBoTieuChi = {};
                var DieuKienBoTieuChiEmpty = {
                    BoTieuChiId: undefined,
                    HoanThanhBoTieuChiId: undefined,
                    LoaiHoanThanh: undefined
                }
                $scope.ListGroupDanhGiaId = [];

                $scope.ListBoTieuChiOption = {
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Chọn bộ tiêu chí hoàn thành trước khi đánh giá bộ tiêu chí này...",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return BoTieuChiService.GetAll().then(res => {
                                    if ($scope.BoTieuChi.LoaiBoTieuChi != undefined)
                                        options.success(res.data.filter(e => e.LoaiBoTieuChi >= $scope.BoTieuChi.LoaiBoTieuChi));
                                    else
                                        options.success(res.data);
                                });
                            }
                        }
                    },
                    change: function () {
                        BoTieuChiService.GetById(this._old).then(res => {
                            $scope.HoanThanhBoTieuChiSelected = res.data;
                            this.$angular_scope.DieuKienBoTieuChi.HoanThanhBoTieuChiLoaiBoTieuChi = res.data.LoaiBoTieuChi;
                            if ($scope.HoanThanhBoTieuChiSelected.LoaiBoTieuChi != $scope.BoTieuChi.LoaiBoTieuChi) {
                                this.$angular_scope.DieuKienBoTieuChi.LoaiHoanThanh = 1;
                            }
                        })
                        this.refresh();
                    }
                }

                $scope.ListGroupDanhGiaOption = {
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return GroupDanhGiaService.GetAll().then(res => {
                                    options.success(res.data);
                                    //resolve();
                                });
                            }
                        }
                    },
                    change: function () {
                        this.refresh();
                    }
                }

                $scope.ListGroupTuDanhGiaOption = {
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return GroupDanhGiaService.GetAll().then(res => {
                                    options.success(res.data);
                                    //resolve();
                                });
                            }
                        }
                    },
                    change: function () {
                        this.refresh();
                    }
                }

                $scope.ChangeTuNgay = function () {
                    $scope.BoTieuChi.TuNgay = moment($scope.BoTieuChi.TuNgay, "DD/MM/YYYY").format("YYYY/MM/DD");
                }

                $scope.ChangeDenNgay = function () {
                    $scope.BoTieuChi.DenNgay = moment($scope.BoTieuChi.DenNgay, "DD/MM/YYYY").format("YYYY/MM/DD");
                }

                if (BoTieuChi == null) {
                    $scope.BoTieuChi = {}
                    $scope.BoTieuChi.TuNgay = moment(new Date).format("DD/MM/YYYY");
                    $scope.BoTieuChi.DenNgay = moment(new Date).format("DD/MM/YYYY");

                    $scope.ListDieuKienBoTieuChi = [];
                    $scope.ListDieuKienBoTieuChi.push(angular.copy(DieuKienBoTieuChiEmpty));
                    $scope.objBoTieuChiRole = {};
                } else {
                    $scope.BoTieuChi = angular.copy(BoTieuChi);

                    DieuKienBoTieuChiEmpty.BoTieuChiId = angular.copy(BoTieuChi.Id);

                    // Get Điều kiện bộ tiêu chí
                    DieuKienBoTieuChiService.GetByBoTieuChiId(BoTieuChi.Id).then(res => {
                        if (res.data.length != 0)
                            $scope.ListDieuKienBoTieuChi = res.data;
                        else {
                            $scope.ListDieuKienBoTieuChi = [];
                            $scope.ListDieuKienBoTieuChi.push(angular.copy(DieuKienBoTieuChiEmpty));
                        }
                    })

                    //get Role
                    BoTieuChiRoleService.GetByBoTieuId(BoTieuChi.Id).then(res => {
                        $scope.objBoTieuChiRole = res.data.length == 0 ? {} : angular.copy(res.data[0]);
                        $scope.objBoTieuChiRole.BoTieuChiName = BoTieuChi.Name;
                        $scope.objBoTieuChiRole.BoTieuChiId = BoTieuChi.Id;
                        res.data.forEach(item => {
                            $scope.ListGroupDanhGiaId.push(item.GroupDanhGiaId);
                        })
                    });
                }

                $scope.save = function () {
                    $scope.ChangeTuNgay();
                    $scope.ChangeDenNgay();
                    $scope.BoTieuChi.AddUserId = $rootScope.session.UserId;
                    $scope.BoTieuChi.LastEditUserId = $rootScope.session.UserId;
                    BoTieuChiService.SaveOrUpdate($scope.BoTieuChi).then(res => {
                        if (res.data != MANAGER.GUID_EMPTY) {
                            for (let i = 0; i < $scope.ListDieuKienBoTieuChi.length; i++) {
                                $scope.ListDieuKienBoTieuChi[i].BoTieuChiId = res.data
                            }

                            // Save điều kiện bộ tiêu chí
                            DieuKienBoTieuChiService.Save($scope.ListDieuKienBoTieuChi).then(resDieuKienBoTieuChi => {
                                $scope.resultNotify += resDieuKienBoTieuChi.data;
                                if (resDieuKienBoTieuChi.data != 1)
                                    Notify('Save điều kiện thất bại', 'top-right', '3000', 'custom', 'fa-warning', true);
                                else {

                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                }
                            })


                            //// Save Role
                            //$scope.objBoTieuChiRole.BoTieuChiId = res.data;
                            //$scope.objSave = [];
                            //$scope.ListGroupDanhGiaId.forEach(item => {
                            //    $scope.objBoTieuChiRole.GroupDanhGiaId = angular.copy(item);
                            //    $scope.objSave.push(angular.copy($scope.objBoTieuChiRole));
                            //})

                            //BoTieuChiRoleService.SaveOrUpdate($scope.objSave).then(res => {
                            //    if (res.data == 1) {
                            //    } else {
                            //        Notify('Save phân quyền thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                            //    }
                            //})

                            $modalInstance.close();
                        }
                        else
                            Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                    })
                }
                $scope.AddNewRow = function () {
                    $scope.ListDieuKienBoTieuChi.push(angular.copy(DieuKienBoTieuChiEmpty));
                }
                $scope.DelRow = function (obj) {
                    $scope.ListDieuKienBoTieuChi = $scope.ListDieuKienBoTieuChi.filter(e => e != obj);
                    if ($scope.ListDieuKienBoTieuChi.length == 0) {
                        $scope.ListDieuKienBoTieuChi.push(angular.copy(DieuKienBoTieuChiEmpty));
                    }
                }

            }
        ]);

        app.controller('BoTieuChiRoleController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'BoTieuChiService', 'BoTieuChi', 'BoTieuChiRoleService', 'GroupDanhGiaService',
            function ($scope, $rootScope, $modal, $modalInstance, BoTieuChiService, BoTieuChi, BoTieuChiRoleService, GroupDanhGiaService) {
                $scope.BoTieuChi = angular.copy(BoTieuChi);
                $scope.ListGroupDanhGiaId = [];
                $scope.RoleEmpty = {
                    GroupTuDanhGiaId: null,
                    ListGroupDanhGiaId: [],
                    UserDanhGiaNgangHang: null
                }
                BoTieuChiRoleService.GetByBoTieuId($scope.BoTieuChi.Id).then(res => {

                    $scope.objBoTieuChiRole = res.data.length == 0 ? {} : angular.copy(res.data[0]);
                    $scope.objBoTieuChiRole.ListRole = [];
                    const ListGroupTuDanhGia = [... new Set(res.data.map(x => x.GroupTuDanhGiaId))];
                    ListGroupTuDanhGia.forEach(item => {
                        const objTemp = angular.copy($scope.RoleEmpty);
                        objTemp.GroupTuDanhGiaId = item;
                        const listRoleBoTieuChiByGroupTuDanhGiaId = res.data.filter(x => x.GroupTuDanhGiaId == item);
                        objTemp.UserDanhGiaNgangHang = listRoleBoTieuChiByGroupTuDanhGiaId[0].UserDanhGiaNgangHang;
                        objTemp.ListGroupDanhGiaId = listRoleBoTieuChiByGroupTuDanhGiaId.map(x => x.GroupDanhGiaId);
                        $scope.objBoTieuChiRole.ListRole.push(objTemp);
                    });

                    if ($scope.objBoTieuChiRole.ListRole.length == 0) {
                        $scope.objBoTieuChiRole.ListRole.push(angular.copy($scope.RoleEmpty));
                    }


                    //$scope.objBoTieuChiRole.BoTieuChiName = $scope.BoTieuChi.Name;
                    //$scope.objBoTieuChiRole.BoTieuChiId = $scope.BoTieuChi.Id;
                    //res.data.forEach(item => {
                    //    $scope.ListGroupDanhGiaId.push(item.GroupDanhGiaId);
                    //});
                    //$scope.$apply();
                });

                $scope.ListGroupDanhGiaOption = {
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return GroupDanhGiaService.GetAll().then(res => {
                                    options.success(res.data);
                                    //resolve();
                                });
                            }
                        }
                    },
                    change: function () {
                        this.refresh();
                    }
                }
                $scope.ListGroupTuDanhGiaOption = {
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return GroupDanhGiaService.GetAll().then(res => {
                                    options.success(res.data);
                                    //resolve();
                                });
                            }
                        }
                    },
                    change: function () {
                        this.refresh();
                    }
                }
                $scope.Save = function () {
                    $scope.objBoTieuChiRole.BoTieuChiId = $scope.BoTieuChi.Id;

                    $scope.objSave = angular.copy($scope.objBoTieuChiRole);
                    $scope.listSave = [];

                    $scope.objBoTieuChiRole.ListRole.forEach(item => {
                        $scope.objSave.GroupTuDanhGiaId = angular.copy(item.GroupTuDanhGiaId);
                        $scope.objSave.UserDanhGiaNgangHang = angular.copy(item.UserDanhGiaNgangHang);
                        item.ListGroupDanhGiaId.forEach(groupDanhGiaId => {
                            $scope.objSave.GroupDanhGiaId = groupDanhGiaId;
                            $scope.listSave.push(angular.copy($scope.objSave));
                        })
                    })

                    BoTieuChiRoleService.SaveOrUpdate($scope.listSave).then(res => {
                        if (res.data == 1) {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $modalInstance.close();
                        } else {
                            Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                        }
                    });
                }

                $scope.AddNewRow = function () {
                    $scope.objBoTieuChiRole.ListRole.push(angular.copy($scope.RoleEmpty));
                }

                $scope.DelRow = function (obj) {
                    $scope.objBoTieuChiRole.ListRole = $scope.objBoTieuChiRole.ListRole.filter(e => e != obj);
                    if ($scope.objBoTieuChiRole.ListRole.length == 0) {
                        $scope.objBoTieuChiRole.ListRole.push(angular.copy($scope.RoleEmpty));
                    }
                }
            }
        ]);
    });