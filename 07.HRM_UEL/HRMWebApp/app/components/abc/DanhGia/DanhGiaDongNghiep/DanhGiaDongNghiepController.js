
define(['app/app',
    'app/services/abc/ABC_DanhGiaDongNghiepService',
    'app/services/abc/ABC_BoTieuChiService',
    'app/services/abc/ABC_RoleBoTieuChiService',
    'app/services/abc/ABC_KyDanhGiaService',
    //'app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGiaService',
    'app/services/abc/ABC_UserDanhGiaService',
    'app/services/abc/ABC_DanhGiaChiTietService',
    'app/services/abc/ABC_DanhGiaService',
    'app/directives/directives'],
    function (app) {
        "use strict";

        app.controller('DanhGiaDongNghiepController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'ABC_DanhGiaDongNghiepService', 'ABC_BoTieuChiService', 'ABC_RoleBoTieuChiService', 'ABC_KyDanhGiaService', 'ABC_UserDanhGiaService', 'ABC_DanhGiaChiTietService','ABC_DanhGiaService',
            function ($scope, $rootScope, $modal, $state, $stateParams, ABC_DanhGiaDongNghiepService, ABC_BoTieuChiService, ABC_RoleBoTieuChiService, ABC_KyDanhGiaService, ABC_UserDanhGiaService, ABC_DanhGiaChiTietService, ABC_DanhGiaService) {
                $scope.KyDanhGiaId = angular.copy($stateParams.KyDanhGiaId);
                $scope.BoTieuChiId = angular.copy($stateParams.BoTieuChiId);
                $scope.GroupDanhGiaId = angular.copy($stateParams.GroupDanhGiaId);
                $scope.NhanVienWebUserId = angular.copy($stateParams.NhanVienWebUserId);
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

                ABC_KyDanhGiaService.GetById($scope.KyDanhGiaId).then(res => {
                    $scope.KyDanhGia = res.data;
                })

                var GetUserDanhGiaIdPromise = new Promise((resolve, reject) => {
                    ABC_UserDanhGiaService.GetUserWithGroupDanhGiaId($rootScope.session.UserId, $scope.KyDanhGiaId, $scope.GroupDanhGiaId).then(res => {
                        $scope.UserDanhGia = res.data;
                        resolve();
                    });
                })
                //var GetListXepLoaiDanhGiaByBoTieuChiIdPromise = new Promise((resolve, reject) => {
                //    XepLoaiDanhGiaService.GetListXepLoaiDanhGiaByBoTieuChiId($stateParams.BoTieuChiId).then(res => {
                //        $scope.ListXepLoai = res.data;
                //        $scope.ListXepLoai.forEach(ObjXepLoai => {
                //            ObjXepLoai.DieuKien = "";
                //            if (ObjXepLoai.TuDiem != null)
                //                ObjXepLoai.DieuKien += "Từ " + ObjXepLoai.TuDiem + " điểm ";
                //            if (ObjXepLoai.DenDiem != null)
                //                ObjXepLoai.DieuKien += "Đến " + ObjXepLoai.DenDiem + " điểm ";
                //            //if()
                //            if (ObjXepLoai.HasDieuKienTieuChi) {
                //                XepLoaiDanhGiaService.GetDieuKienXepLoaiPhuByXepLoaiDanhGiaId(ObjXepLoai.Id).then(resDieuKienPhu => {
                //                    ObjXepLoai.DieuKien += "và các tiêu chí thuộc " + resDieuKienPhu.data.TieuChiNoiDung + " đạt " + resDieuKienPhu.data.DiemDat + "% số điểm.";
                //                })
                //            } else if (ObjXepLoai.HasDieuKienPhu) {
                //                ObjXepLoai.DieuKien += "và đạt " + ObjXepLoai.DiemDat + "% số điểm tất cả các tiêu chí.";
                //            }
                //        })
                //        $scope.LoaddingXepLoai = false;
                //        resolve();
                //    })
                //});

                //var GetListDieuKienXepLoaiPhuByBoTieuChiIdPromise = new Promise((resolve, reject) => {
                //    XepLoaiDanhGiaService.GetListDieuKienXepLoaiPhuByBoTieuChiId($stateParams.BoTieuChiId).then(resListDieuKienPhu => {
                //        $scope.ListDieuKienPhu = resListDieuKienPhu.data;
                //        resolve();
                //    })
                //})


                var GetUserDuocDanhGiaIdPromise = new Promise((resolve, reject) => {
                    ABC_UserDanhGiaService.GetUser($scope.NhanVienWebUserId, $scope.KyDanhGiaId).then(res => {
                        $scope.UserDuocDanhGia = res.data;
                        resolve();
                    });
                })



                Promise.all([/*GetListXepLoaiDanhGiaByBoTieuChiIdPromise, GetListDieuKienXepLoaiPhuByBoTieuChiIdPromise, */GetUserDuocDanhGiaIdPromise, GetUserDanhGiaIdPromise]).then(() => {
                    //$scope.ClickedStaff(UserDuocDanhGia);

                    var GetDanhGiaByFKPromise = new Promise((resolve, reject) => {
                        ABC_DanhGiaDongNghiepService.GetDanhGiaByFK($scope.UserDuocDanhGia.Id, $scope.UserDanhGia.Id, $scope.KyDanhGiaId, $scope.BoTieuChiId, $scope.GroupDanhGiaId).then(res => {
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
                            $scope.GetListTieuChi()
                            resolve();
                        })
                    });

                    GetDanhGiaByFKPromise.then(() => {
                        //$scope.GetListTieuChi();
                    })
                })

                //ABC_DanhGiaDongNghiepService.GetListUserDanhGia($scope.BoTieuChiId).then(res => {
                //    $scope.ListStaff = res.data;
                //})
                ABC_RoleBoTieuChiService.GetByBoTieuId($scope.BoTieuChiId).then(res => {
                    $scope.BoTieuChiRole = res.data
                })
                ABC_BoTieuChiService.GetById($scope.BoTieuChiId).then(res => {
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
                    })
                    $scope.DanhGia.TongDiem = Math.Round($scope.DanhGia.TongDiem, -2);
                }

                $scope.ChangeScore = function (item) {
                    if (!isNaN(Math.Round(item.Diem * 1, 0))) {
                        item.ScoreError = false;
                        if (item.TieuChiParentId != null)
                            sum(item.Id, item.TieuChiParentId);
                        sumTong();
                    } else {
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
                        ABC_DanhGiaDongNghiepService.GetListKetQuaDanhGia($scope.DanhGia.Id).then(res => {
                            $scope.ListKetQuaDanhGia = res.data;
                            if ($scope.ListKetQuaDanhGia.length > 0) {
                                ABC_DanhGiaDongNghiepService.GetListChiTietDanhGiaByDanhGiaId($scope.DanhGia.Id).then(res => {
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
                                angular.forEach($scope.ListKetQuaDanhGia, function (item, key) {
                                    //angular.forEach(item.DanhGiaChiTiet.filter(e => e.NoChild == true), (itemChiTiet, Key) => {
                                    //    debugger
                                    //    //itemChiTiet.Diem = $scope.list.filter(e => e.)
                                    //});
                                    item.DanhGiaChiTiet = treeToView(listToTree(item.DanhGiaChiTiet), []);
                                })
                            } else {
                                ABC_DanhGiaDongNghiepService.GetCapBacChuaDanhGia($scope.DanhGia.Id).then(res => {
                                    $scope.UserChuaDanhGia = res.data;
                                    resolve();
                                })
                            }
                        })
                    })
                    GetListTieuChi.then(function () {
                        //GetKetQuaXepLoai();
                        //GetKetQuaXepLoaiWithUserNow();
                        $scope.IsLoadding = false;
                        $scope.$apply();
                    })
                }

                $scope.Save = function (isLock) {
                    $scope.IsLoadding = true;

                    var SaveDanhGiaChiTietPromise = new Promise((resolve, reject) => {
                        ABC_DanhGiaChiTietService.SaveDanhGiaChiTiet($scope.list).then(res => {
                            if (res.data) {
                                resolve();
                            }
                            reject();
                        }).catch((err) => {
                            reject();
                        })
                    })

                    var SaveDanhGiaPrimise = new Promise((resolve, reject) => {
                        ABC_DanhGiaService.SaveDanhGia($scope.DanhGia).then(res => {
                            resolve();
                        }).catch((err) => {
                            reject();
                        })
                    })

                    angular.forEach($scope.list.filter(e => e.NoChild == true), function (item, key) {
                        $scope.ChangeScore(item);
                    })

                    if (!isLock) {
                        Promise.all([SaveDanhGiaChiTietPromise, SaveDanhGiaPrimise]).then(() => {
                            $scope.GetListTieuChi();
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        }).catch((err) => {
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

                    //if (!isLock) {
                    //    ABC_DanhGiaDongNghiepService.SaveDanhGiaChiTiet($scope.list).then(res => {
                    //        if (res.data) {
                    //            $scope.GetListTieuChi();
                    //            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    //        }
                    //    })
                    //    ABC_DanhGiaDongNghiepService.PutSaveOrUpdateDanhGia($scope.DanhGia)
                    //} else {
                    //    if (confirm('Khóa sẽ không được chỉnh sửa. Bạn có chắc chắn không?')) {
                    //        $scope.DanhGia.IsLock = isLock;
                    //        ABC_DanhGiaDongNghiepService.SaveDanhGiaChiTiet($scope.list).then(res => {
                    //            if (res.data) {
                    //                $scope.GetListTieuChi();
                    //                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    //            }
                    //        })
                    //        ABC_DanhGiaDongNghiepService.PutSaveOrUpdateDanhGia($scope.DanhGia)
                    //    } else {
                    //        $scope.IsLoadding = false;

                    //    }
                    //}
                }
            }
        ]);
    });