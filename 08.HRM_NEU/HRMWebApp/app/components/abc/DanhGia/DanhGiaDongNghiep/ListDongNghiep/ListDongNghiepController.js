
define(['app/app',
    'app/components/abc/DanhGia/DanhGiaDongNghiep/ListDongNghiep/ListDongNghiepService',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiRoleService',
    'app/components/abc/QuanLyDanhGia/DieuKienBoTieuChi/DieuKienBoTieuChiService',
    'app/components/abc/DanhGia/BoTieuChi/BoTieuChiDanhGiaService',
    'app/components/abc/QuanLyDanhGia/UserGroupDanhGiaRole/UserGroupDanhGiaRoleService',
    'app/components/abc/QuanLyDanhGia/QuanLyDonVi/QuanLyDonViService',
    'app/services/abc/ABC_UserDanhGiaService'],
    function (app) {
        "use strict";
        app.controller('ListDongNghiepController', ['$scope', '$rootScope', '$modal', '$modalInstance', '$state', 'ListDongNghiepService', 'BoTieuChiId', 'KyDanhGiaId', 'GroupDanhGiaId', 'BoTieuChiRoleService', 'DieuKienBoTieuChiService', 'BoTieuChiDanhGiaService', 'UserGroupDanhGiaRoleService', 'QuanLyDonViService', 'ABC_UserDanhGiaService',
            function ($scope, $rootScope, $modal, $modalInstance, $state, ListDongNghiepService, BoTieuChiId, KyDanhGiaId, GroupDanhGiaId, BoTieuChiRoleService, DieuKienBoTieuChiService, BoTieuChiDanhGiaService, UserGroupDanhGiaRoleService, QuanLyDonViService, ABC_UserDanhGiaService) {
                var currentTime = new Date();
                $scope.ShowListStaff = false;
                $scope.ListBoPhan123 = {};
                $scope.BoPhanDataSource = {};
                $scope.User = null;
                $scope.ListUser = [];
                $scope.BoPhanId = {};
                var GetListUserDanhGiaPromise = {};
                BoTieuChiDanhGiaService.GetById(BoTieuChiId).then(res => {
                    $scope.BoTieuChi = res.data;
                });
                var GetByBoTieuChiIdPromise = new Promise(function (resolve, reject) {
                    BoTieuChiRoleService.GetByBoTieuIdAndGroupDanhGiaId(BoTieuChiId, GroupDanhGiaId).then(res => {
                        $scope.ListBoTieuChiRole = res.data;
                        resolve();
                    });
                });


                var UserNowPromise = new Promise((resolve, reject) => {
                    ABC_UserDanhGiaService.GetUserWithGroupDanhGiaId($rootScope.session.UserId, KyDanhGiaId, GroupDanhGiaId).then(res => {
                        if (res.data == null || res == null)
                            reject();
                        $scope.User = res.data; //$scope.UserNowGroupRole
                        resolve();
                    });
                });
                //UserNowPromise.then(() => {

                    
                //})

                $scope.ListBoPhan123 = {
                    filter: "contains",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: true,
                    change: function (option) {
                        ListUserByDepartmentId(this._old);
                    }
                }
                $scope.$watch('BoPhanId', function (newValue, oldValue) {


                });

                function ListUserByDepartmentId(DepartmentId) {
                    $scope.ShowListStaff = false;
                    var GetListUserDanhGiaByDepartmentIdPromise = new Promise((resolve, reject) => {
                        ABC_UserDanhGiaService.GetListUserDanhGiaInDonVi(BoTieuChiId, KyDanhGiaId, $rootScope.session.UserId, GroupDanhGiaId, DepartmentId).then(res => {
                            $scope.ListUser = res.data;
                            resolve();
                        });
                    });

                    Promise.all([GetByBoTieuChiIdPromise, GetListUserDanhGiaByDepartmentIdPromise]).then(function (value) {
                        $scope.ListUser.forEach(User => {
                            User.ListTuDanhGia = {};
                            User.ListCapTrenDanhGia = [];

                            if (User.ListDanhGia != null)
                                User.ListTuDanhGia = User.ListDanhGia.filter(DanhGia => DanhGia.UserDanhGiaId == DanhGia.UserDuocDanhGiaId);

                            $scope.ListBoTieuChiRole.forEach(BoTieuChiRole => {
                                let ObjDanhGia = User.ListDanhGia.filter(DanhGia => DanhGia.UserDanhGia_GroupId == BoTieuChiRole.GroupDanhGiaId);
                                if (ObjDanhGia.length != 0)
                                    User.ListCapTrenDanhGia.push(angular.copy(ObjDanhGia[0]));
                                else {
                                    ObjDanhGia = { IsLock: false, TongDiem: 0 }
                                    User.ListCapTrenDanhGia.push(ObjDanhGia);
                                }
                            })
                        });
                        $scope.ShowListStaff = true;
                        //this.refresh();
                        $scope.$apply();
                    }).catch(() => {
                        $scope.ShowListStaff = true;
                        $scope.$apply();
                    });
                }


                UserNowPromise.then(() => {

                    GetListUserDanhGiaPromise = new Promise((resolve, reject) => {
                        ABC_UserDanhGiaService.GetListUserDanhGiaInDonVi(BoTieuChiId, KyDanhGiaId, $rootScope.session.UserId, GroupDanhGiaId, $scope.User.DepartmentId).then(res => {
                            $scope.ListUser = res.data;
                            resolve();
                        });
                    });

                    if ($scope.User.GroupDanhGiaHasQuanLyDonVi) {
                        $scope.ShowListStaff = true;
                        $scope.BoPhanDataSource = {
                            transport: {
                                read: function (options) {
                                    return QuanLyDonViService.GetListQuanLyDonViByUserId($rootScope.session.UserId).then(res => {
                                        options.success(res.data);
                                        ListUserByDepartmentId(res.data[0].DepartmentId);
                                    });
                                }
                            }
                        };
                    } else {
                        Promise.all([GetByBoTieuChiIdPromise, GetListUserDanhGiaPromise]).then(() => {
                            $scope.ListUser.forEach(User => {
                                User.ListTuDanhGia = {};
                                User.ListCapTrenDanhGia = [];

                                if (User.ListDanhGia != null)
                                    User.ListTuDanhGia = User.ListDanhGia.filter(DanhGia => DanhGia.UserDanhGiaId == DanhGia.UserDuocDanhGiaId);

                                $scope.ListBoTieuChiRole.forEach(BoTieuChiRole => {
                                    let ObjDanhGia = User.ListDanhGia.filter(DanhGia => DanhGia.UserDanhGia_GroupId == BoTieuChiRole.GroupDanhGiaId);
                                    if (ObjDanhGia.length != 0)
                                        User.ListCapTrenDanhGia.push(angular.copy(ObjDanhGia[0]));
                                    else {
                                        ObjDanhGia = { IsLock: false, TongDiem: 0 }
                                        User.ListCapTrenDanhGia.push(ObjDanhGia);
                                    }
                                })
                            });
                            $scope.ShowListStaff = true;
                            $scope.$apply();
                        });
                    }

                })


                $scope.StaffClick = function (aBC_User) {

                    if ($scope.BoTieuChi.HasDieuKienDanhGia) {
                        DieuKienBoTieuChiService.GetCheckDieuKienBoTieuChi(BoTieuChiId, KyDanhGiaId, aBC_User.Id, GroupDanhGiaId).then(res => {

                            if (res.data == "") {
                                $state.go("DanhGiaDongNghiep", { KyDanhGiaId: KyDanhGiaId, BoTieuChiId: BoTieuChiId, GroupDanhGiaId: GroupDanhGiaId, ABC_UserId: aBC_User.Id });
                                $modalInstance.close();
                            }
                            else
                                if (res.data == "ERRORS") {

                                } else {
                                    alert(res.data);
                                }

                        });
                    }
                    else {
                        $state.go("DanhGiaDongNghiep", { KyDanhGiaId: KyDanhGiaId, BoTieuChiId: BoTieuChiId, GroupDanhGiaId: GroupDanhGiaId, ABC_UserId: aBC_User.Id });
                        $modalInstance.close();
                    }
                }
            }
        ]);

    });