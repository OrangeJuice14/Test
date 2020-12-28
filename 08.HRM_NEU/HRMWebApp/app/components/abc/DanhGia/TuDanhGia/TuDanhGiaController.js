
define(['app/app',
    'app/components/abc/DanhGia/TuDanhGia/TuDanhGiaService',
    'app/components/abc/QuanLyDanhGia/TieuChi/TieuChiService',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiService',
    'app/directives/directives',
    'app/components/abc/DanhGia/KyDanhGia/KyDanhGiaService',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiRoleService',
    'app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGiaService',
    'app/services/abc/ABC_UserDanhGiaService'], function (app) {
        "use strict";

        app.controller('TuDanhGiaController', ['$scope', '$rootScope', '$timeout', '$modal', '$state', '$stateParams', 'TuDanhGiaService', 'BoTieuChiService', 'KyDanhGiaService', 'BoTieuChiRoleService', 'XepLoaiDanhGiaService', 'ABC_UserDanhGiaService',
            function ($scope, $rootScope, $timeout, $modal, $state, $stateParams, TuDanhGiaService, BoTieuChiService, KyDanhGiaService, BoTieuChiRoleService, XepLoaiDanhGiaService, ABC_UserDanhGiaService) {
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
                KyDanhGiaService.GetById($scope.KyDanhGiaId).then(res => {
                    $scope.KyDanhGia = res.data;
                })
                BoTieuChiRoleService.GetByBoTieuId($scope.BoTieuChiId).then(res => {
                    $scope.BoTieuChiRole = res.data;
                })
                BoTieuChiService.GetById($scope.BoTieuChiId).then(res => {
                    $scope.BoTieuChi = res.data;
                });

                //TuDanhGiaService.CheckIsTeacher($rootScope.session.UserId, $scope.KyDanhGiaId, $scope.GroupDanhGiaId).then(resCheckIsTeacher => {
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

                function sum(Id, parentId) {
                    var ListParent = $scope.list.filter(e => e.TieuChiId == parentId);
                    angular.forEach(ListParent, function (parent, key) {
                        parent.Diem = 0;
                        let stt = 0;
                        var ListChild = $scope.list.filter(e => e.TieuChiParentId == parentId)
                        let Diem = parent.TieuChiDiemToiDa != null ? parent.TieuChiDiemToiDa : parent.TieuChiParentDiemToiDa;
                        let ParentDiemToiDa = parent.TieuChiDiemToiDa || parent.TieuChiParentDiemToiDa;
                        let DiemCongToiDa = 0;
                        let TongDiemTru = 0;
                        let TongDiemCong = 0;
                        let SoLuongTieuChiConLaDiemCong = 0;
                        angular.forEach(ListChild, function (child, key) {
                            if (Number.isNaN(parent.Diem + child.Diem))
                                child.Diem = 0;
                            else {
                                if (child.TieuChiDiemTru) {
                                    if (ParentDiemToiDa - DiemCongToiDa + TongDiemCong + TongDiemTru + child.Diem < 0) {
                                        child.Diem = -(ParentDiemToiDa - DiemCongToiDa + TongDiemCong + TongDiemTru) * child.TieuChiHeSoTieuChiCon / 100;
                                    }
                                    else {
                                    }
                                    Diem += child.Diem * parent.TieuChiHeSoTieuChiCon / 100;
                                    TongDiemTru += child.Diem;
                                }
                                else {
                                    SoLuongTieuChiConLaDiemCong++;
                                    if (child.TieuChiIsAutoScore) {
                                        DiemCongToiDa += ParentDiemToiDa - child.TieuChiDiemToiDa * 1;
                                        if (child.TieuChiDiemToiDa == null) {
                                            if (child.Diem > ParentDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100)
                                                child.Diem = ParentDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100;
                                        } else {
                                            if (child.Diem > child.TieuChiDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100)
                                                child.Diem = child.TieuChiDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100;
                                        }
                                    }
                                    else {
                                        DiemCongToiDa += child.TieuChiDiemToiDa * 1;
                                    }

                                    if (Diem / parent.TieuChiHeSoTieuChiCon * 100 - child.Diem < 0) {
                                        child.Diem = Diem / (parent.TieuChiHeSoTieuChiCon * 100) * (child.TieuChiHeSoTieuChiCon / 100);
                                    }
                                    else {

                                    }
                                    Diem -= Math.Round(child.Diem * parent.TieuChiHeSoTieuChiCon / 100, -2);

                                    if (TongDiemCong + Math.Round(child.Diem, -2) <= ParentDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100)
                                        TongDiemCong += Math.Round(child.Diem, -2);
                                    else
                                        TongDiemCong = ParentDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100;
                                }
                            }
                            child.Diem = Math.Round(child.Diem, -2);
                        })

                        if (DiemCongToiDa > ParentDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100)
                            DiemCongToiDa = ParentDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100;

                        if (ParentDiemToiDa - DiemCongToiDa + TongDiemCong + TongDiemTru < 0 || - DiemCongToiDa + TongDiemCong + TongDiemTru > 0) {
                            parent.Diem = Math.Round((TongDiemCong + TongDiemTru) * parent.TieuChiHeSoTieuChiCon / 100, -2);
                        } else {
                            if (SoLuongTieuChiConLaDiemCong == ListChild.length && TongDiemCong == 0)
                                //if (ListChild.filter(e => e.TieuChiIsAutoScore == true).length > 0)
                                parent.Diem = 0
                            //else
                            //    parent.Diem = Math.Round(- DiemCongToiDa + TongDiemTru, -2);
                            else
                                parent.Diem = Math.Round(((ParentDiemToiDa / parent.TieuChiHeSoTieuChiCon * 100 - DiemCongToiDa + TongDiemCong + TongDiemTru) * parent.TieuChiHeSoTieuChiCon / 100), -2);
                        }

                        if (parent.TieuChiParentId != null && parent.TieuChiParentId != undefined) {
                            //$scope.$apply();
                            sum(parent.Id, parent.TieuChiParentId);
                        }
                    })
                }

                function sumTong() {
                    $scope.DanhGia.TongDiem = 0;
                    angular.forEach($scope.list.filter(e => e.TieuChiParentId == null), function (value, key) {
                        $scope.DanhGia.TongDiem += value.Diem;
                    });
                    $scope.DanhGia.TongDiem = Math.Round($scope.DanhGia.TongDiem, -2);
                    GetKetQuaXepLoaiWithUserNow();
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


                var GetListDieuKienXepLoaiPhuPromise = new Promise((resolve, reject) => {

                    XepLoaiDanhGiaService.GetListDieuKienXepLoaiPhuByBoTieuChiId($stateParams.BoTieuChiId).then(resListDieuKienPhu => {
                        $scope.ListDieuKienPhu = resListDieuKienPhu.data;
                        resolve();
                    })

                });

                var GetListXepLoaiDanhGiaPromise = new Promise((resolve, reject) => {
                    XepLoaiDanhGiaService.GetListXepLoaiDanhGiaByBoTieuChiId($stateParams.BoTieuChiId).then(res => {
                        $scope.ListXepLoai = res.data;
                        $scope.ListXepLoai.forEach(ObjXepLoai => {
                            ObjXepLoai.DieuKien = "";
                            if (ObjXepLoai.TuDiem != null)
                                ObjXepLoai.DieuKien += "Từ " + ObjXepLoai.TuDiem + " điểm ";
                            if (ObjXepLoai.DenDiem != null)
                                ObjXepLoai.DieuKien += "Đến " + ObjXepLoai.DenDiem + " điểm ";
                            //if()
                            if (ObjXepLoai.HasDieuKienTieuChi) {
                                XepLoaiDanhGiaService.GetDieuKienXepLoaiPhuByXepLoaiDanhGiaId(ObjXepLoai.Id).then(resDieuKienPhu => {
                                    ObjXepLoai.DieuKien += "và các tiêu chí thuộc " + resDieuKienPhu.data.TieuChiNoiDung + " đạt " + resDieuKienPhu.data.DiemDat + "% số điểm.";
                                })
                            } else if (ObjXepLoai.HasDieuKienPhu) {
                                ObjXepLoai.DieuKien += "và đạt " + ObjXepLoai.DiemDat + "% số điểm tất cả các tiêu chí.";
                            }
                        });
                        $scope.LoaddingXepLoai = false;
                        resolve();
                    });
                });

                $scope.GetXepLoai = function () {
                    $scope.LoaddingXepLoai = true;
                    Promise.all([GetListXepLoaiDanhGiaPromise, GetListDieuKienXepLoaiPhuPromise, GetUserNowPromise]).then(() => {
                        var GetDanhGiaByFKPromise = new Promise((resolve, reject) => {
                            TuDanhGiaService.GetDanhGiaByFK($scope.UserDanhGia.Id, $scope.UserDanhGia.Id, $scope.KyDanhGiaId, $scope.BoTieuChiId, $scope.GroupDanhGiaId).then(res => {
                                $scope.DanhGia = res.data;
                                if ($scope.DanhGia.IsLock) {
                                    $scope.Date.FullTime = $scope.DanhGia.ThoiGianDanhGia;
                                    $scope.Date.Year = "năm " + moment($scope.Date.FullTime).format("YYYY");
                                    $scope.Date.Day = "ngày " + moment($scope.Date.FullTime).format("DD");
                                    $scope.Date.Month = "tháng " + moment($scope.Date.FullTime).format("MM");
                                }
                                if ($scope.DanhGia.IsLock) {
                                    TuDanhGiaService.GetListKetQuaDanhGia($scope.DanhGia.Id).then(res => {
                                        $scope.ListKetQuaDanhGia = res.data;
                                        resolve(); //$scope.GetListTieuChi();
                                        if ($scope.ListKetQuaDanhGia.length > 0) {
                                            angular.forEach($scope.ListKetQuaDanhGia, function (item, key) {
                                                item.DanhGiaChiTiet = treeToView(listToTree(item.DanhGiaChiTiet), []);
                                            })
                                        } else {
                                            //DanhGiaDongNghiepService.GetCapBacChuaDanhGia($scope.DanhGia.Id).then(res => {
                                            //    $scope.UserChuaDanhGia = res.data;
                                            //    resolve();
                                            //})
                                            reject();
                                        }
                                    })
                                } else {

                                    resolve(); //$scope.GetListTieuChi();
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
                    if (!isNaN(Math.round(eval(item.Diem) * 1))) {
                        item.ScoreError = false;
                        if (item.TieuChiParentId != null && item.TieuChiParentId != undefined)
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
                        TuDanhGiaService.GetListChiTietDanhGiaByDanhGiaId($scope.DanhGia.Id).then(res => {
                            $scope.list = treeToView(listToTree(res.data), []);
                            $scope.TongDiemToiDa = 0;
                            $scope.list.filter(e => e.TieuChiParentId == null).forEach(item => {
                                $scope.TongDiemToiDa += item.TieuChiDiemToiDa;
                                item.Diem = Math.Round(item.Diem, -2);
                            })
                            var list = $scope.list.filter(e => e.NoChild == true);
                            angular.forEach(list, function (item, key) {
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
                            })
                            resolve();
                        })

                    });

                    GetListTieuChiPromise.then(function () {
                        $scope.IsLoadding = false;
                        GetKetQuaXepLoaiWithUserNow();
                        GetKetQuaXepLoai();
                        $scope.$apply();
                    })
                }


                $scope.Save = function (isLock) {
                    $scope.IsLoadding = true;
                    $scope.list.filter(e => e.NoChild == true).forEach(item => {
                        $scope.ChangeScore(item);
                    })

                    if (isLock && confirm('Khóa sẽ không được chỉnh sửa. Bạn có chắc chắn không?')) {
                        $scope.DanhGia.IsLock = isLock;

                        TuDanhGiaService.SaveDanhGiaChiTiet($scope.list).then(res => {
                            if (res.data) {
                                $scope.GetListTieuChi();
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            }
                        })
                        TuDanhGiaService.SaveDanhGia($scope.DanhGia)
                    }
                    if (!isLock) {
                        TuDanhGiaService.SaveDanhGiaChiTiet($scope.list).then(res => {
                            if (res.data) {
                                $scope.GetListTieuChi();
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            }
                        })
                        TuDanhGiaService.SaveDanhGia($scope.DanhGia);
                    }
                }
                $scope.AutoScore = function (tieuChiId) {
                    TuDanhGiaService.GetScoresAuto(tieuChiId, $rootScope.session.UserId, $rootScope.session.UserId).then(res => {

                    })
                }
            }
        ]);
    });