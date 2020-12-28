
define(['app/app',
    'app/services/abc/ABC_DanhGiaService',
    'app/services/abc/ABC_DanhGiaChiTietService',
    'app/services/abc/ABC_BoTieuChiService',
    'app/directives/directives',
    'app/services/abc/ABC_KyDanhGiaService',
    'app/services/abc/ABC_RoleBoTieuChiService',
    //'app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGiaService',
    'app/services/abc/ABC_UserDanhGiaService'], function (app) {
        "use strict";

        app.controller('TuDanhGiaController', ['$scope', '$rootScope', '$timeout', '$modal', '$state', '$stateParams', 'ABC_DanhGiaService', 'ABC_BoTieuChiService', 'ABC_KyDanhGiaService', 'ABC_RoleBoTieuChiService', 'ABC_UserDanhGiaService', 'ABC_DanhGiaChiTietService',
            function ($scope, $rootScope, $timeout, $modal, $state, $stateParams, ABC_DanhGiaService, ABC_BoTieuChiService, ABC_KyDanhGiaService, ABC_RoleBoTieuChiService, ABC_UserDanhGiaService, ABC_DanhGiaChiTietService) {
                var currentTime = new Date();
                $scope.IsLoadding = true;
                $scope.A = false;
                $scope.TongDiemToiDa = 0;
                $scope.nam = currentTime.getFullYear();
                $scope.KyDanhGiaId = angular.copy($stateParams.KyDanhGiaId);
                $scope.BoTieuChiId = angular.copy($stateParams.BoTieuChiId);
                $scope.GroupDanhGiaId = angular.copy($stateParams.GroupDanhGiaId);
                $scope.Date = {};
                $scope.Date.FullTime = new Date();
                $scope.Date.Year = "năm " + moment($scope.Date.FullTime).format("YYYY");
                $scope.Date.Day = "ngày " + moment($scope.Date.FullTime).format("DD");
                $scope.Date.Month = "tháng " + moment($scope.Date.FullTime).format("MM");
                $scope.UserDanhGia = {};
                ABC_KyDanhGiaService.GetById($scope.KyDanhGiaId).then(res => {
                    $scope.KyDanhGia = res.data;
                })
                ABC_RoleBoTieuChiService.GetByBoTieuId($scope.BoTieuChiId).then(res => {
                    $scope.BoTieuChiRole = res.data;
                })
                ABC_BoTieuChiService.GetById($scope.BoTieuChiId).then(res => {
                    $scope.BoTieuChi = res.data;
                });

                //ABC_DanhGiaService.CheckIsTeacher($rootScope.session.UserId, $scope.KyDanhGiaId, $scope.GroupDanhGiaId).then(resCheckIsTeacher => {
                //    $scope.UserNowIsTeacher = resCheckIsTeacher.data;
                //})
                function listToTree(list) {
                    var map = {}, node, roots = [], i;
                    for (i = 0; i < list.length; i += 1) {
                        map[list[i].TieuChiId] = i; // initialize the map
                        list[i].children = []; // initialize the children
                        list[i].Diem = Math.round(Math.ceil(list[i].Diem * 100)) / 100;
                    }
                    for (i = 0; i < list.length; i += 1) {
                        node = list[i];
                        if (node.TieuChiParentId != null) {
                            // if you have dangling branches check that map[node.parentId] exists
                            list[map[node.TieuChiParentId]].children.push(node);
                        } else {
                            roots.push(node);
                        }
                    }
                    return roots;
                }

                function treeToView(tree, arrView) {
                    for (let i = 0; i < tree.length; i += 1) {
                        arrView.push(tree[i]);
                        if (tree[i].children) {
                            treeToView(tree[i].children, arrView);
                        }
                    }
                    return arrView;
                }

                function sum(id, parentId) {
                    var parent = $scope.list.find(e => e.TieuChiId == parentId);
                    parent.Diem = 0;
                    var ListChild = $scope.list.filter(e => e.TieuChiParentId == parentId && e.Id != id);
                    var Child = $scope.list.find(e => e.Id == id);
                    let Diem = parent.TieuChiDiemToiDa || parent.TieuChiParentDiemToiDa;
                    let parentDiemToiDa = parent.TieuChiDiemToiDa || parent.TieuChiParentDiemToiDa;
                    let DiemCongToiDa = 0;
                    let TongDiemTru = 0;
                    let TongDiemCong = 0;
                    let TongDiemCon = 0;
                    if (Child.TieuChiDiemToiDa != null)
                        Child.Diem = Child.TieuChiDiemToiDa >= Child.Diem ? Child.Diem : Child.TieuChiDiemToiDa;
                    angular.forEach(ListChild, function (child, key) {
                        //TongDiemCon += child.Diem;
                        if (Number.isNaN(parent.Diem + child.Diem))
                            child.Diem = 0;
                        else {
                            TongDiemCon += +child.Diem;

                            //DiemCongToiDa += child.TieuChiDiemToiDa * 1;

                            //if (Diem - child.Diem < 0) {
                            //    child.Diem = Diem;
                            //}
                            //else {

                            //}
                            //Diem -= Math.Round(child.Diem, -2);

                            //if (TongDiemCong + Math.Round(child.Diem, -2) <= ParentDiemToiDa)
                            //    TongDiemCong += Math.Round(child.Diem, -2);
                            //else
                            //    TongDiemCong = ParentDiemToiDa;
                        }
                        //child.Diem = Math.Round(child.Diem, -2);

                        //if (DiemCongToiDa > ParentDiemToiDa)
                        //    DiemCongToiDa = ParentDiemToiDa;

                        //if (ParentDiemToiDa - DiemCongToiDa + TongDiemCong + TongDiemTru < 0 || - DiemCongToiDa + TongDiemCong + TongDiemTru > 0) {
                        //    parent.Diem = Math.Round((TongDiemCong + TongDiemTru), -2);
                        //} else {
                        //        parent.Diem = Math.Round(ParentDiemToiDa - DiemCongToiDa + TongDiemCong + TongDiemTru, -2);
                        //}

                    });
                    if (parentDiemToiDa != null)
                        Child.Diem = TongDiemCon + +Child.Diem <= parentDiemToiDa ? Child.Diem : (parentDiemToiDa - TongDiemCon);

                    parent.Diem = Math.Round(TongDiemCon + +Child.Diem, -2);
                    if (parent.TieuChiParentId != null) {
                        sum(parent.Id, parent.TieuChiParentId);
                    }
                }

                function sumTong() {
                    $scope.DanhGia.TongDiem = 0;
                    angular.forEach($scope.list.filter(e => e.TieuChiParentId == null), function (value, key) {
                        $scope.DanhGia.TongDiem += value.Diem;
                    });
                    $scope.DanhGia.TongDiem = Math.Round($scope.DanhGia.TongDiem, -2);
                    //GetKetQuaXepLoaiWithUserNow();
                }

                function KiemTraKetQuaTieuChi(ListKetQua, ObjKetQua, HeSo) {
                    if (ObjKetQua.Diem >= ObjKetQua.TieuChiDiemToiDa * HeSo / 100) {
                        var ListChild = ListKetQua.filter(e => e.TieuChiParentId == ObjKetQua.TieuChiId);
                        if (ListChild.length > 0) {
                            let i = 0;
                            ListChild.forEach(Child => {
                                if (KiemTraKetQuaTieuChi(ListKetQua, Child, HeSo) == true) {
                                    i++;
                                }
                            })
                            return i == ListChild.length
                        }
                        return true;
                    }
                    return false;
                }

                function KiemTraDieuKienTieuChiDanhGiaPhu(ListKetQua, DieuKienPhu) {
                    return KiemTraKetQuaTieuChi(ListKetQua, ListKetQua.filter(e => e.TieuChiId == DieuKienPhu.TieuChiId)[0], DieuKienPhu.DiemDat);
                }

                function KiemTraDieuKienDanhGiaPhu(ListKetQua, DiemDat) {
                    let i = 0;
                    let ListNoParent = ListKetQua.filter(e => e.TieuChiParentId == null);
                    ListNoParent.forEach(ObjKetQua => {
                        if (KiemTraKetQuaTieuChi(ListKetQua, ObjKetQua, DiemDat) == true) {
                            i++;
                        } else { return false; }
                    })
                    return ListNoParent.length == i;
                }

                function GetKetQuaXepLoaiWithUserNow() {
                    for (let i = $scope.ListXepLoai.length - 1; i >= 0; i--) {
                        if ($scope.ListXepLoai[i].TuDiem != null) {
                            if ($scope.DanhGia.TongDiem >= $scope.ListXepLoai[i].TuDiem) {
                                if ($scope.ListXepLoai[i].HasDieuKienTieuChi && $scope.ListXepLoai[i].HasDieuKienPhu) { // nếu có tiêu chí phụ thì kiểm tra 
                                    let ObjDieuKienPhu = $scope.ListDieuKienPhu.filter(e => e.XepLoaiDanhGiaId == $scope.ListXepLoai[i].Id);
                                    if (ObjDieuKienPhu.length != 0) {
                                        if (KiemTraDieuKienTieuChiDanhGiaPhu($scope.list.filter(e => e.NoChild == false), ObjDieuKienPhu[0])) {
                                            $scope.DanhGia.KetQuaXepLoai = $scope.ListXepLoai[i];
                                        }
                                    }
                                }
                                else {
                                    if ($scope.ListXepLoai[i].HasDieuKienPhu) { // nếu tính tất cả các tiêu chí
                                        if (KiemTraDieuKienDanhGiaPhu($scope.list.filter(e => e.NoChild == false), $scope.ListXepLoai[i].DiemDat)) {
                                            $scope.DanhGia.KetQuaXepLoai = $scope.ListXepLoai[i];
                                        }
                                    } else { // nếu không xét điều kiện phụ phụ
                                        $scope.DanhGia.KetQuaXepLoai = $scope.ListXepLoai[i];
                                    }
                                }
                            }
                        } else {
                            $scope.DanhGia.KetQuaXepLoai = $scope.ListXepLoai[i];
                        }
                    }
                }

                function GetKetQuaXepLoai() {
                    if ($scope.DanhGia.IsLock && $scope.ListKetQuaDanhGia != undefined)
                        $scope.ListKetQuaDanhGia.forEach(ObjKetQua => {
                            let max = $scope.ListXepLoai.length;
                            for (let i = max - 1; i >= 0; i--) {
                                if ($scope.ListXepLoai[i].TuDiem != null) {
                                    if (ObjKetQua.TongDiem >= $scope.ListXepLoai[i].TuDiem) {
                                        if ($scope.ListXepLoai[i].HasDieuKienTieuChi && $scope.ListXepLoai[i].HasDieuKienPhu) { // nếu có tiêu chí phụ thì kiểm tra
                                            let ObjDieuKienPhu = $scope.ListDieuKienPhu.filter(e => e.XepLoaiDanhGiaId == $scope.ListXepLoai[i].Id);
                                            if (ObjDieuKienPhu.length != 0) {
                                                if (KiemTraDieuKienTieuChiDanhGiaPhu(ObjKetQua.DanhGiaChiTiet.filter(e => e.NoChild == false), ObjDieuKienPhu[0])) {
                                                    ObjKetQua.KetQuaXepLoai = $scope.ListXepLoai[i];
                                                }
                                            }
                                        }
                                        else {
                                            if ($scope.ListXepLoai[i].HasDieuKienPhu) { // nếu tính tất cả các tiêu chí
                                                if (KiemTraDieuKienDanhGiaPhu(ObjKetQua.DanhGiaChiTiet.filter(e => e.NoChild == false), $scope.ListXepLoai[i].DiemDat)) {
                                                    ObjKetQua.KetQuaXepLoai = $scope.ListXepLoai[i];
                                                }
                                            } else { // nếu không xét điều kiện phụ phụ
                                                ObjKetQua.KetQuaXepLoai = $scope.ListXepLoai[i];
                                            }
                                        }
                                    }
                                } else {
                                    ObjKetQua.KetQuaXepLoai = $scope.ListXepLoai[i];
                                }
                            }
                        })
                }

                var GetUserNowPromise = new Promise((resolve, reject) => {
                    ABC_UserDanhGiaService.GetUserWithGroupDanhGiaId($scope.session.UserId, $scope.KyDanhGiaId, $scope.GroupDanhGiaId).then(res => {
                        $scope.UserDanhGia = res.data;
                        resolve();
                    });
                });


                //var GetListDieuKienXepLoaiPhuPromise = new Promise((resolve, reject) => {
                //    XepLoaiDanhGiaService.GetListDieuKienXepLoaiPhuByBoTieuChiId($stateParams.BoTieuChiId).then(resListDieuKienPhu => {
                //        $scope.ListDieuKienPhu = resListDieuKienPhu.data;
                //        resolve();
                //    })
                //});

                //var GetListXepLoaiDanhGiaPromise = new Promise((resolve, reject) => {
                //    XepLoaiDanhGiaService.GetListXepLoaiDanhGiaByBoTieuChiId($stateParams.BoTieuChiId).then(res => {
                //        $scope.ListXepLoai = res.data;
                //        $scope.ListXepLoai.forEach(ObjXepLoai => {
                //            ObjXepLoai.DieuKien = "";
                //            if (ObjXepLoai.TuDiem != null)
                //                ObjXepLoai.DieuKien += "Từ " + ObjXepLoai.TuDiem + " điểm ";

                //            if (ObjXepLoai.DenDiem != null)
                //                ObjXepLoai.DieuKien += "Đến " + ObjXepLoai.DenDiem + " điểm ";

                //            if (ObjXepLoai.HasDieuKienTieuChi) {
                //                XepLoaiDanhGiaService.GetDieuKienXepLoaiPhuByXepLoaiDanhGiaId(ObjXepLoai.Id).then(resDieuKienPhu => {
                //                    ObjXepLoai.DieuKien += "và các tiêu chí thuộc " + resDieuKienPhu.data.TieuChiNoiDung + " đạt " + resDieuKienPhu.data.DiemDat + "% số điểm.";
                //                })
                //            } else if (ObjXepLoai.HasDieuKienPhu) {
                //                ObjXepLoai.DieuKien += "và đạt " + ObjXepLoai.DiemDat + "% số điểm tất cả các tiêu chí.";
                //            }
                //        });
                //        $scope.LoaddingXepLoai = false;
                //        resolve();
                //    });
                //});

                $scope.GetXepLoai = function () {
                    $scope.LoaddingXepLoai = true;
                    Promise.all([/*GetListXepLoaiDanhGiaPromise, GetListDieuKienXepLoaiPhuPromise,*/ GetUserNowPromise]).then(() => {
                        var GetDanhGiaByFKPromise = new Promise((resolve, reject) => {
                            ABC_DanhGiaService.GetDanhGiaByFK($scope.UserDanhGia.Id, $scope.UserDanhGia.Id, $scope.KyDanhGiaId, $scope.BoTieuChiId, $scope.GroupDanhGiaId).then(res => {
                                $scope.DanhGia = res.data;

                                if ($scope.DanhGia.IsLock) {
                                    $scope.Date.FullTime = $scope.DanhGia.ThoiGianDanhGia;
                                    $scope.Date.Year = "năm " + moment($scope.Date.FullTime).format("YYYY");
                                    $scope.Date.Day = "ngày " + moment($scope.Date.FullTime).format("DD");
                                    $scope.Date.Month = "tháng " + moment($scope.Date.FullTime).format("MM");
                                }

                                if ($scope.DanhGia.IsLock) {
                                    ABC_DanhGiaService.GetListKetQuaDanhGia($scope.DanhGia.Id).then(res => {
                                        $scope.ListKetQuaDanhGia = res.data;
                                        resolve();
                                        if ($scope.ListKetQuaDanhGia.length > 0) {
                                            angular.forEach($scope.ListKetQuaDanhGia, function (item, key) {
                                                item.DanhGiaChiTiet = treeToView(listToTree(item.DanhGiaChiTiet), []);
                                            })
                                        } else {
                                            reject();
                                        }
                                    })
                                } else {
                                    resolve();
                                }
                            })
                        })

                        GetDanhGiaByFKPromise.then(() => {
                            $scope.GetListTieuChi();
                        });
                    }).catch(() => {
                        //lỗi
                    });
                }



                $scope.GetXepLoai();

                $scope.ChangeScore = function (item) {
                    //if (!item.TieuChiIsAutoScore) {
                    if (!isNaN(Math.Round(item.Diem * 1, 0))) {
                        item.ScoreError = false;
                        if (item.TieuChiParentId != null)
                            sum(item.Id, item.TieuChiParentId);
                        sumTong();
                    } else {
                        item.ScoreError = true;
                    }
                    //}
                    //return item.Diem;
                }
                $scope.GetListTieuChi = function () {
                    $scope.IsLoadding = true;
                    var GetListTieuChiPromise = new Promise(function (resolve, reject) {
                        ABC_DanhGiaChiTietService.GetListChiTietDanhGiaByDanhGiaId($scope.DanhGia.Id).then(res => {
                            var list = treeToView(listToTree(res.data), []);
                            $scope.TongDiemToiDa = 0;
                            list.filter(e => e.TieuChiParentId == null).forEach(item => {
                                $scope.TongDiemToiDa += item.TieuChiDiemToiDa;
                                item.Diem = Math.Round(item.Diem, -2);
                            })
                            angular.forEach(list, function (item, key) {
                                if (item.NoChild == true) {
                                    item.TieuChiListDiem = item.TieuChiListDiem ? item.TieuChiListDiem.split(";").filter(e => e != "").map(x => +x) : a;
                                    if (item.TieuChiIsAutoScore && !$scope.DanhGia.IsLock) {
                                        if (item.TieuChiCongThucTinhDiem == null)
                                            item.TieuChiCongThucTinhDiem = "";
                                        if (item.TieuChiIsTeacher && $scope.UserNowIsTeacher) {
                                            item.Diem = Math.Round(eval(item.Diem + item.TieuChiCongThucTinhDiemTeacher.replace(/%/g, "/100")), -2);
                                        }
                                        else {
                                            item.Diem = Math.Round(eval(item.Diem + item.TieuChiCongThucTinhDiem.replace(/%/g, "/100")), -2);
                                        }
                                        $scope.ChangeScore(item);
                                    }
                                }
                            })
                            $scope.list = list;
                            resolve();
                        })

                    });

                    GetListTieuChiPromise.then(function () {
                        $scope.IsLoadding = false;
                        //GetKetQuaXepLoaiWithUserNow();
                        GetKetQuaXepLoai();
                        $scope.$apply();
                    })
                }


                $scope.Save = function (isLock) {
                    var SaveDanhGiaChiTietPromise = new Promise((resolve, reject) => {
                        ABC_DanhGiaChiTietService.SaveDanhGiaChiTiet($scope.list).then(res => {
                            if (res.data) {
                                resolve();
                            }
                            reject();
                        })
                    })
                    var SaveDanhGiaPrimise = new Promise((resolve, reject) => {
                        ABC_DanhGiaService.SaveDanhGia($scope.DanhGia).then(res => {
                            resolve();
                        })
                    })
                    $scope.IsLoadding = true;
                    $scope.list.filter(e => e.NoChild == true).forEach(item => {
                        $scope.ChangeScore(item);
                    })

                    
                    if (!isLock) {
                        Promise.all([SaveDanhGiaChiTietPromise, SaveDanhGiaPrimise]).then(() => {
                            $scope.GetListTieuChi();
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        }).catch(() => {
                            $scope.GetListTieuChi();
                            Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                        })
                    } else {
                        if (confirm('Khóa sẽ không được chỉnh sửa. Bạn có chắc chắn không?')) {
                            $scope.DanhGia.IsLock = isLock;
                            Promise.all([SaveDanhGiaChiTietPromise, SaveDanhGiaPrimise]).then(() => {
                                $scope.GetListTieuChi();
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            }).catch(() => {
                                $scope.GetListTieuChi();
                                Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                            })
                        } else {
                            $scope.IsLoadding = false;
                        }
                    }
                }
                //$scope.AutoScore = function (tieuChiId) {
                //    ABC_DanhGiaService.GetScoresAuto(tieuChiId, $rootScope.session.UserId, $rootScope.session.UserId).then(res => {

                //    })
                //}
            }
        ]);
    });