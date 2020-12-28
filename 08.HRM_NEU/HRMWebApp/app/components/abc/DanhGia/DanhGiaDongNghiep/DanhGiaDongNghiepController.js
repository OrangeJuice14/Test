
define(['app/app', 'app/components/abc/DanhGia/DanhGiaDongNghiep/DanhGiaDongNghiepService',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiService',
    'app/directives/directives',
    'app/components/abc/DanhGia/TuDanhGia/TuDanhGiaService',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiRoleService',
    'app/components/abc/DanhGia/KyDanhGia/KyDanhGiaService',
    'app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGiaService',
    'app/services/abc/ABC_UserDanhGiaService'],
    function (app) {
        "use strict";

        app.controller('DanhGiaDongNghiepController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'DanhGiaDongNghiepService', 'BoTieuChiService', 'TuDanhGiaService', 'BoTieuChiRoleService', 'KyDanhGiaService', 'XepLoaiDanhGiaService', 'ABC_UserDanhGiaService',
            function ($scope, $rootScope, $modal, $state, $stateParams, DanhGiaDongNghiepService, BoTieuChiService, TuDanhGiaService, BoTieuChiRoleService, KyDanhGiaService, XepLoaiDanhGiaService, ABC_UserDanhGiaService) {
                $scope.KyDanhGiaId = angular.copy($stateParams.KyDanhGiaId);
                $scope.BoTieuChiId = angular.copy($stateParams.BoTieuChiId);
                $scope.GroupDanhGiaId = angular.copy($stateParams.GroupDanhGiaId);
                $scope.ABC_UserId = angular.copy($stateParams.ABC_UserId);
                $scope.IsLoadding = true;
                $scope.UserDuocDanhGia = null;
                $scope.ChuaDanhGia = false;
                $scope.ListStaff = null;
                $scope.TongDiemToiDa = 0;
                $scope.Date = {};
                $scope.Date.FullTime = new Date();
                $scope.Date.Year = "năm " + moment($scope.Date.FullTime).format("YYYY");
                $scope.Date.Day = "ngày " + moment($scope.Date.FullTime).format("DD");
                $scope.Date.Month = "tháng " + moment($scope.Date.FullTime).format("MM");
                $scope.ListXepLoai = {};
                $scope.LoaddingXepLoai = true;

                KyDanhGiaService.GetById($scope.KyDanhGiaId).then(res => {
                    $scope.KyDanhGia = res.data;
                })

                var GetUserDanhGiaIdPromise = new Promise((resolve, reject) => {
                    ABC_UserDanhGiaService.GetUserWithGroupDanhGiaId($rootScope.session.UserId, $scope.KyDanhGiaId, $scope.GroupDanhGiaId).then(res => {
                        $scope.UserDanhGia = res.data;
                        resolve();
                    });
                })
                var GetListXepLoaiDanhGiaByBoTieuChiIdPromise = new Promise((resolve, reject) => {
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
                        })
                        $scope.LoaddingXepLoai = false;
                        resolve();
                    })
                });

                var GetListDieuKienXepLoaiPhuByBoTieuChiIdPromise = new Promise((resolve, reject) => {
                    XepLoaiDanhGiaService.GetListDieuKienXepLoaiPhuByBoTieuChiId($stateParams.BoTieuChiId).then(resListDieuKienPhu => {
                        $scope.ListDieuKienPhu = resListDieuKienPhu.data;
                        resolve();
                    })
                })


                var GetUserDuocDanhGiaIdPromise = new Promise((resolve, reject) => {
                    ABC_UserDanhGiaService.GetABCUserById($scope.ABC_UserId).then(res => {
                        $scope.UserDuocDanhGia = res.data;
                        resolve();
                    });
                })



                Promise.all([GetListXepLoaiDanhGiaByBoTieuChiIdPromise, GetListDieuKienXepLoaiPhuByBoTieuChiIdPromise, GetUserDuocDanhGiaIdPromise, GetUserDanhGiaIdPromise]).then(() => {
                    //$scope.ClickedStaff(UserDuocDanhGia);

                    var GetDanhGiaByFKPromise = new Promise((resolve, reject) => {
                        DanhGiaDongNghiepService.GetDanhGiaByFK($scope.UserDuocDanhGia.Id, $scope.UserDanhGia.Id, $scope.KyDanhGiaId, $scope.BoTieuChiId, $scope.GroupDanhGiaId).then(res => {
                            $scope.DanhGia = res.data;

                            if ($scope.DanhGia != null) {
                                if ($scope.DanhGia.IsLock) {
                                    $scope.Date.FullTime = $scope.DanhGia.ThoiGianDanhGia;
                                    $scope.Date.Year = "năm " + moment($scope.Date.FullTime).format("YYYY");
                                    $scope.Date.Day = "ngày " + moment($scope.Date.FullTime).format("DD");
                                    $scope.Date.Month = "tháng " + moment($scope.Date.FullTime).format("MM");
                                }
                                $scope.ChuaDanhGia = false;
                                resolve();
                            }
                            else {
                                $scope.ChuaDanhGia = true;
                            }

                            resolve();
                        })
                    });

                    GetDanhGiaByFKPromise.then(() => {
                        $scope.GetListTieuChi();
                    })
                })

                //DanhGiaDongNghiepService.GetListUserDanhGia($scope.BoTieuChiId).then(res => {
                //    $scope.ListStaff = res.data;
                //})
                BoTieuChiRoleService.GetByBoTieuId($scope.BoTieuChiId).then(res => {
                    $scope.BoTieuChiRole = res.data
                })
                BoTieuChiService.GetById($scope.BoTieuChiId).then(res => {
                    $scope.BoTieuChi = res.data;
                });

                function listToTree(list) {
                    var map = {}, node, roots = [], i;
                    for (i = 0; i < list.length; i += 1) {
                        map[list[i].TieuChiId] = i; // initialize the map
                        list[i].children = []; // initialize the children
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
                }

                $scope.ChangeScore = function (item) {
                    if (!isNaN(eval(item.Diem) * 1)) {
                        if (item.TieuChiParentId != null && item.TieuChiParentId != undefined)
                            sum(item.Id, item.TieuChiParentId);
                        sumTong();
                        GetKetQuaXepLoaiWithUserNow();
                        item.ScoreError = false;
                    }
                    else {
                        item.ScoreError = true;
                    }
                    //return item.Diem;
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

                $scope.GetListTieuChi = function () {
                    $scope.IsLoadding = true;
                    var GetListTieuChi = new Promise(function (resolve, reject) {
                        DanhGiaDongNghiepService.GetListKetQuaDanhGia($scope.DanhGia.Id).then(res => {
                            $scope.ListKetQuaDanhGia = res.data;
                            if ($scope.ListKetQuaDanhGia.length > 0) {
                                DanhGiaDongNghiepService.GetListChiTietDanhGiaByDanhGiaId($scope.DanhGia.Id).then(res => {
                                    $scope.list = treeToView(listToTree(res.data), []);
                                    $scope.TongDiemToiDa = 0;
                                    $scope.list.filter(e => e.TieuChiParentId == null).forEach(item => {
                                        $scope.TongDiemToiDa += item.TieuChiDiemToiDa;
                                    })
                                    var list = $scope.list.filter(e => e.NoChild == true);
                                    var TongDiem = angular.copy($scope.DanhGia.TongDiem);
                                    angular.forEach(list, function (item, key, valueList) {



                                        if (TongDiem == 0)
                                            item.Diem = $scope.ListKetQuaDanhGia[0].DanhGiaChiTiet.filter(e => e.TieuChiId == item.TieuChiId)[0].Diem;
                                        else
                                            if (item.TieuChiIsAutoScore && !$scope.DanhGia.IsLock) {
                                                if (item.TieuChiCongThucTinhDiem == null)
                                                    item.TieuChiCongThucTinhDiem = "";
                                                if (item.TieuChiIsTeacher && $scope.UserNowIsTeacher) {
                                                    item.Diem = Math.Round(eval(item.Diem + item.TieuChiCongThucTinhDiemTeacher.replace(/%/g, "/100")), -2);
                                                }
                                                else {
                                                    item.Diem = Math.Round(eval(item.Diem + item.TieuChiCongThucTinhDiem.replace(/%/g, "/100")), -2);
                                                }
                                                //$scope.ChangeScore(item);
                                            }
                                        //debugger;
                                        $scope.ChangeScore(item);
                                        //}
                                    })
                                    resolve();
                                })
                                angular.forEach($scope.ListKetQuaDanhGia, function (item, key) {
                                    //angular.forEach(item.DanhGiaChiTiet.filter(e => e.NoChild == true), (itemChiTiet, Key) => {
                                    //    debugger
                                    //    //itemChiTiet.Diem = $scope.list.filter(e => e.)
                                    //});
                                    item.DanhGiaChiTiet = treeToView(listToTree(item.DanhGiaChiTiet), []);
                                })
                            } else {
                                DanhGiaDongNghiepService.GetCapBacChuaDanhGia($scope.DanhGia.Id).then(res => {
                                    $scope.UserChuaDanhGia = res.data;
                                    resolve();
                                })
                            }
                        })
                    })
                    GetListTieuChi.then(function () {
                        GetKetQuaXepLoai();
                        GetKetQuaXepLoaiWithUserNow();
                        $scope.IsLoadding = false;
                        $scope.$apply();
                    })
                }

                //$scope.ClickedStaff = function (ABC_User) {
                //    $scope.IsLoadding = true;
                //    //$scope.UserDuocDanhGia = angular.copy(staff);
                //    DanhGiaDongNghiepService.GetDanhGiaByFK(ABC_User.Id, $rootScope.session.UserId, $scope.KyDanhGiaId, $scope.BoTieuChiId, $scope.GroupDanhGiaId).then(res => {
                //        $scope.DanhGia = res.data;
                //        TuDanhGiaService.CheckIsTeacher(ABC_UserId).then(resCheckIsTeacher => {
                //            $scope.UserDuocDanhGiaIsTeacher = resCheckIsTeacher.data;
                //            if ($scope.DanhGia != null) {
                //                if ($scope.DanhGia.IsLock) {
                //                    $scope.Date.FullTime = $scope.DanhGia.ThoiGianDanhGia;
                //                    $scope.Date.Year = "năm " + moment($scope.Date.FullTime).format("YYYY");
                //                    $scope.Date.Day = "ngày " + moment($scope.Date.FullTime).format("DD");
                //                    $scope.Date.Month = "tháng " + moment($scope.Date.FullTime).format("MM");
                //                }
                //                $scope.ChuaDanhGia = false;
                //                $scope.GetListTieuChi();
                //            }
                //            else {
                //                $scope.ChuaDanhGia = true;
                //            }
                //        })     
                //    })
                //}

                $scope.Save = function (isLock) {
                    $scope.IsLoadding = true;
                    angular.forEach($scope.list.filter(e => e.NoChild == true), function (item, key) {
                        $scope.ChangeScore(item);
                    })

                    if (isLock && confirm('Khóa sẽ không được chỉnh sửa. Bạn có chắc chắn không?')) {
                        $scope.DanhGia.IsLock = isLock;
                        DanhGiaDongNghiepService.SaveDanhGiaChiTiet($scope.list).then(res => {
                            if (res.data) {
                                $scope.GetListTieuChi();
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            }
                        })
                        DanhGiaDongNghiepService.PutSaveOrUpdateDanhGia($scope.DanhGia)
                    }
                    if (!isLock) {
                        DanhGiaDongNghiepService.SaveDanhGiaChiTiet($scope.list).then(res => {
                            if (res.data) {
                                $scope.GetListTieuChi();
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            }
                        })
                        DanhGiaDongNghiepService.PutSaveOrUpdateDanhGia($scope.DanhGia)
                    }
                }
            }
        ]);
    });