
define(['app/app',
    'app/services/abc/ABC_ListDongNghiepService',
    'app/services/abc/ABC_BoTieuChiService',
    'app/services/abc/ABC_RoleBoTieuChiService',
    'app/services/abc/ABC_QuanLyDonViService',
    'app/services/abc/ABC_UserDanhGiaService'],
    function (app) {
        "use strict";
        app.controller('ListDongNghiepController', ['$scope', '$rootScope', '$modal', '$modalInstance', '$state', 'ABC_ListDongNghiepService', 'BoTieuChiId', 'KyDanhGiaId', 'GroupDanhGiaId', 'ABC_RoleBoTieuChiService', 'ABC_BoTieuChiService', 'ABC_QuanLyDonViService', 'ABC_UserDanhGiaService',
            function ($scope, $rootScope, $modal, $modalInstance, $state, ABC_ListDongNghiepService, BoTieuChiId, KyDanhGiaId, GroupDanhGiaId, ABC_RoleBoTieuChiService, ABC_BoTieuChiService, ABC_QuanLyDonViService, ABC_UserDanhGiaService) {
                var currentTime = new Date();
                $scope.ShowListStaff = false;
                $scope.BoPhanDataSource = {};
                $scope.User = null;
                $scope.ListUser = [];
                $scope.BoPhanId = {};

                $scope.ListBoPhan123 = {
                    filter: "contains",
                    placeholder: "Nhóm đánh giá...",
                    valuePrimitive: true,
                    autoBind: true,
                    change: function (option) {
                        //ListUserByDepartmentId(this._old);
                    }
                }

                ABC_BoTieuChiService.GetById(BoTieuChiId).then(res => {
                    $scope.BoTieuChi = res.data;
                });

                var GetByBoTieuChiIdPromise = new Promise(function (resolve, reject) {
                    ABC_RoleBoTieuChiService.GetByBoTieuIdAndGroupDanhGiaId(BoTieuChiId, GroupDanhGiaId).then(res => {
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

                $scope.$watch('BoPhanId', function (newValue, oldValue) {

                });

                function ListUserByDepartmentId(DepartmentId) {
                    $scope.ShowListStaff = false;
                    var GetListUserDanhGiaByDepartmentIdPromise = new Promise((resolve, reject) => {
                        ABC_UserDanhGiaService.GetListUserDanhGiaInDonVi(BoTieuChiId, KyDanhGiaId, $rootScope.session.UserId, GroupDanhGiaId, DepartmentId).then(res => {
                            $scope.ListUser = res.data;
                            resolve();
                        }).catch(err => {
                            reject();
                        });
                    });

                    Promise.all([GetByBoTieuChiIdPromise, GetListUserDanhGiaByDepartmentIdPromise]).then((value) => {
                        $scope.ListUser.forEach(User => {
                            User.ListTuDanhGia = [];
                            User.ListCapTrenDanhGia = [];

                            if (User.ListDanhGia == null || User.ListDanhGia.length == 0) {
                                User.ListTuDanhGia.push({ IsLock: false, TongDiem: 0 });
                                User.ListCapTrenDanhGia.push({ IsLock: false, TongDiem: 0 });
                            }
                            else {
                                User.ListTuDanhGia = User.ListDanhGia.filter(DanhGia => DanhGia.UserDanhGiaId == DanhGia.UserDuocDanhGiaId);

                                $scope.ListBoTieuChiRole.forEach(BoTieuChiRole => {
                                    let ObjDanhGia = User.ListDanhGia.find(DanhGia => DanhGia.GroupUserDanhGiaId == BoTieuChiRole.GroupDanhGiaId);
                                    if (ObjDanhGia != null)
                                        User.ListCapTrenDanhGia.push(angular.copy(ObjDanhGia));
                                    else {
                                        User.ListCapTrenDanhGia.push({ IsLock: false, TongDiem: 0 });
                                    }
                                });
                            }
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
                    //var GetListUserDanhGiaPromise = {};
                    //GetListUserDanhGiaPromise = new Promise((resolve, reject) => {
                    //    ABC_UserDanhGiaService.GetListUserDanhGiaInDonVi(BoTieuChiId, KyDanhGiaId, $rootScope.session.UserId, GroupDanhGiaId, $scope.User.DepartmentId).then(res => {
                    //        $scope.ListUser = res.data;
                    //        resolve();
                    //    }).catch(err => { reject() });
                    //});

                    if ($scope.User.GroupDanhGiaHasQuanLyDonVi) {
                        $scope.ShowListStaff = true;
                        $scope.BoPhanDataSource = {
                            transport: {
                                read: function (options) {
                                    return ABC_QuanLyDonViService.GetListQuanLyDonViByUserId($rootScope.session.UserId).then(res => {
                                        options.success(res.data);
                                        //ListUserByDepartmentId(res.data[0].DepartmentId);
                                    });
                                }
                            }
                        };
                    } else {
                        ListUserByDepartmentId($scope.User.DepartmentId);
                        //Promise.all([GetByBoTieuChiIdPromise, GetListUserDanhGiaPromise]).then(() => {
                        //    $scope.ListUser.forEach(User => {
                        //        User.ListTuDanhGia = {};
                        //        User.ListCapTrenDanhGia = [];

                        //        if (User.ListDanhGia != null)
                        //            User.ListTuDanhGia = User.ListDanhGia.filter(DanhGia => DanhGia.UserDanhGiaId == DanhGia.UserDuocDanhGiaId);

                        //        $scope.ListBoTieuChiRole.forEach(BoTieuChiRole => {
                        //            let ObjDanhGia = User.ListDanhGia.filter(DanhGia => DanhGia.GroupUserDanhGiaId == BoTieuChiRole.GroupDanhGiaId);
                        //            if (ObjDanhGia.length != 0)
                        //                User.ListCapTrenDanhGia.push(angular.copy(ObjDanhGia[0]));
                        //            else {
                        //                ObjDanhGia = { IsLock: false, TongDiem: 0 }
                        //                User.ListCapTrenDanhGia.push(ObjDanhGia);
                        //            }
                        //        })
                        //    });
                        //    $scope.ShowListStaff = true;
                        //    $scope.$apply();
                        //}).catch(err => {

                        //});
                    }
                });

                $scope.StaffClick = function (user) {

                    //if ($scope.BoTieuChi.HasDieuKienDanhGia) {
                    //    DieuKienBoTieuChiService.GetCheckDieuKienBoTieuChi(BoTieuChiId, KyDanhGiaId, aBC_User.Id, GroupDanhGiaId).then(res => {

                    //        if (res.data == "") {
                    //            $state.go("DanhGiaDongNghiep", { KyDanhGiaId: KyDanhGiaId, BoTieuChiId: BoTieuChiId, GroupDanhGiaId: GroupDanhGiaId, ABC_UserId: aBC_User.Id });
                    //            $modalInstance.close();
                    //        }
                    //        else
                    //            if (res.data == "ERRORS") {

                    //            } else {
                    //                alert(res.data);
                    //            }

                    //    });
                    //}
                    //else {
                    $state.go("DanhGiaDongNghiep", { KyDanhGiaId: KyDanhGiaId, BoTieuChiId: BoTieuChiId, GroupDanhGiaId: GroupDanhGiaId, NhanVienWebUserId: user.Id });
                    $modalInstance.close();
                    //}
                }
            }
        ]);

    });