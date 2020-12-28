define(['app/app', 'app/services/abc/ABC_DongNghiepDanhGiaService', 'app/services/abc/ABC_KyDanhGiaService', 'app/services/abc/WebUserService', 'app/directives/treedirective', 'app/services/abc/ABC_ChiTietKetQuaService', 'app/services/abc/ABC_BoDanhGiaService', 'app/services/abc/ABC_TieuChiDanhGiaService', 'app/services/abc/ABC_KetQuaService', 'app/services/abc/staffService', 'app/services/abc/ABC_ChiTietNhanVienDanhGiaService', 'app/services/abc/ABC_KetQuaXepLoaiService'], function (app) {
    "use strict";
    //var HRMWebAppModule = angular.module('HRMWebApp');
    app.controller('ABC_KyDanhGiaCapDuoiController', ['$scope', '$rootScope', '$modal', '$state', 'ABC_KyDanhGiaService', 'WebUserService',
        function ($scope, $rootScope, $modal, $state, ABC_KyDanhGiaService, WebUserService) {
            $scope.grid = {};
            $scope.obj = {};
            $scope.isEdit = false;
            var currentTime = new Date();
            function listToTree(list) {
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].Id] = i; // initialize the map
                    list[i].children = []; // initialize the children
                }
                for (i = 0; i < list.length; i += 1) {
                    node = list[i];
                    if (node.ParentId !== null && node.ParentId != '00000000-0000-0000-0000-000000000000') {
                        // if you have dangling branches check that map[node.parentId] exists
                        list[map[node.ParentId]].children.push(node);
                    } else {
                        roots.push(node);
                    }
                }
                return roots;
            }
            ABC_KyDanhGiaService.GetNam().then(function (result) {
                $scope.ListNam = result.data;
            });
            $scope.Nam = currentTime.getFullYear();

            $scope.UpdateYearValue = function () {
                ABC_KyDanhGiaService.getListKyDanhGia($scope.Nam).then(function (result) {
                    $scope.TreeKyDanhGia = listToTree(angular.copy(result.data));
                });
            }
            $scope.UpdateYearValue();
            $scope.KyDanhGiaClicked = function (KyDanhGiaId) {
                        $state.go("DanhGiaCapDuoi", { KyDanhGiaId: KyDanhGiaId });
            }
            $scope.$watch('treeYears.currentNode', function (newObj, oldObj) {
                if ($scope.abc && angular.isObject($scope.treeYears.currentNode)) {
                    console.log('Node Selected!!');
                    console.log($scope.abc.currentNode);
                }
                $scope.KyDanhGiaClicked(newObj.Id);
            }, false);
        }
    ]);

    app.controller('ABC_DanhGiaCapDuoiController', ['$scope', '$rootScope', '$modal', '$stateParams', 'ABC_ChiTietKetQuaService', 'ABC_KyDanhGiaService', 'ABC_BoDanhGiaService', 'ABC_TieuChiDanhGiaService', 'WebUserService', 'ABC_KetQuaService', 'staffService', 'ABC_ChiTietNhanVienDanhGiaService','ABC_KetQuaXepLoaiService',
        function ($scope, $rootScope, $modal, $stateParams, ABC_ChiTietKetQuaService, ABC_KyDanhGiaService, ABC_BoDanhGiaService, ABC_TieuChiDanhGiaService, WebUserService, ABC_KetQuaService, staffService, ABC_ChiTietNhanVienDanhGiaService, ABC_KetQuaXepLoaiService) {

            $scope.LoaddingTableData = false;
            $scope.LoaddingBody = false;
            $scope.Loadding = true;
            $scope.HideTableData = true;
            $scope.KyDanhGia = {};
            $scope.SearchText = '';
            $scope.NguoiDuocDanhGia = {};
            $scope.IsLock = false;
            $scope.DisableLock = true;
            $scope.DisableSave = true;
            var PhanLoaiSTT = "A";
            ABC_KyDanhGiaService.getKyDanhGiaById($stateParams.KyDanhGiaId).then(function (result) {
                $scope.KyDanhGia = result.data;
            });
            ABC_BoDanhGiaService.getBoDanhGiaByTimeNow($stateParams.KyDanhGiaId, 3).then(function (result) {
                $scope.BoDanhGia = result.data;
                ABC_KetQuaXepLoaiService.GetListByDanhGiaId($scope.BoDanhGia.Id).then(function (resultXepLoai) {
                    $scope.ListXepLoai = resultXepLoai.data;
                });
            });
            WebUserService.getWebUserByUserId($rootScope.session.UserId).then(function (resultUser) {
                $scope.ObjStaff = resultUser.data;
                staffService.getListStaff($scope.ObjStaff.StaffInfoId).then(function (result) {
                    $scope.ListStaff = result.data;
                    $scope.LoaddingBody = true;
                });
            });

            function listToTree(list) {
                PhanLoaiSTT = "A";
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].TieuChiDanhGiaId] = i; // initialize the map
                    list[i].children = []; // initialize the children
                    list[i].isRadio = false;
                    list[i].childRadio = false;
                    list[i].IsChildren = false;
                    if (list[i].IsChecked == true) {
                        list[i].TieuChiDanhGiaIdChecked = list[i].TieuChiDanhGiaId;
                    }
                }
                for (i = 0; i < list.length; i += 1) {
                    node = list[i];
                    if (node.TieuChiDanhGiaParentId !== null && node.TieuChiDanhGiaParentId != '00000000-0000-0000-0000-000000000000') {
                        // if you have dangling branches check that map[node.parentId] exists
                        list[map[node.TieuChiDanhGiaParentId]].IsChildren = true;
                        if (list[map[node.TieuChiDanhGiaParentId]].TieuChiDanhGiaParentChildSelectOne == true)
                            node.isRadio == true;
                        list[map[node.TieuChiDanhGiaParentId]].children.push(node);
                    } else {
                        roots.push(node);
                    }
                }
                return roots;
            }
            function treeToView(tree, arrView, ParentIsChecked = null, ParentIsRadio = false) {
                for (let i = 0; i < tree.length; i += 1) {
                    if (tree[i].TieuChiDanhGiaSTT == "B")
                        PhanLoaiSTT = "B";
                    tree[i].PhanLoaiSTT = PhanLoaiSTT;
                    tree[i].ParentIsChecked = ParentIsChecked;
                    tree[i].childRadio = ParentIsRadio;
                    if (tree[i].ParentIsChecked === false) {
                        tree[i].Diem = null;
                    }
                    arrView.push(tree[i]);
                    if (tree[i].children) {
                        if (tree[i].IsChecked === true)
                            treeToView(tree[i].children, arrView, true, true);
                        else if (tree[i].IsChecked === false)
                            treeToView(tree[i].children, arrView, false, true);
                        else {
                            if (tree[i].TieuChiDanhGiaParentChildSelectOne)
                                treeToView(tree[i].children, arrView, null, true);
                            else
                                treeToView(tree[i].children, arrView);
                        }
                    }
                }
                return arrView;
            }


            function getListChiTietKetQua(StaffId, KyDanhGiaId) {
                $scope.Loadding = true;
                if (KyDanhGiaId != undefined && StaffId != undefined) {
                    ABC_ChiTietKetQuaService.getListChiTietKetQua(KyDanhGiaId, StaffId, $scope.ObjStaff.StaffInfoId, 3).then(function (result) {
                        if (result.data.length != 0) {
                            $scope.ListChiTietKetQua = treeToView(listToTree(result.data), []);
                            $scope.LoaddingTableData = true;
                            ABC_ChiTietNhanVienDanhGiaService.getById(result.data[0].ChiTietNhanVienDanhGiaId).then(function (resultKetQua) {
                                if (resultKetQua.data != null) {
                                    $scope.ObjKetQua = resultKetQua.data;
                                }
                                else {

                                }
                                $scope.HideTableData = false;
                                $scope.Loadding = false;
                                $scope.ChangeScore();
                            });
                        }
                        else {
                            $scope.ListChiTietKetQua = null;
                        }
                    });
                    ABC_ChiTietKetQuaService.getListChiTietKetQua(KyDanhGiaId, StaffId, StaffId, 1).then(function (result) {
                        if (result.data.length != 0) {
                            $scope.ListChiTietKetQuaTuDanhGia = treeToView(listToTree(result.data), []);
                            ABC_ChiTietNhanVienDanhGiaService.getById(result.data[0].ChiTietNhanVienDanhGiaId).then(function (resultKetQua) {
                                if (resultKetQua.data != null) {
                                    $scope.ObjKetQuaTuDanhGia = resultKetQua.data;
                                }
                                else {

                                }
                            });
                        }
                    });

                }
                    
            }
            $scope.aaaa = function (item) {
                alert(item.TieuChiDanhGiaId);
            }
            $scope.Save = function () {
                try {
                    ABC_ChiTietNhanVienDanhGiaService.PutUpdate($scope.ObjKetQua);
                    ABC_ChiTietKetQuaService.saveChiTietKetQua($scope.ListChiTietKetQua).then(function (result) {
                        if (result.data == true) {
                            getListChiTietKetQua($scope.NguoiDuocDanhGia.Id, $stateParams.KyDanhGiaId);
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        } else {
                            Notify('Lỗi!', 'top-right', '3000', 'error', 'fa-exclamation-circle', false);
                        }
                    });
                }
                catch (err) {
                    Notify('Lỗi!', 'top-right', '3000', 'error', 'fa-exclamation-circle', false);
                }
            }

            $scope.Lock = function () {
                $scope.ObjKetQua.isLock = true;
                $scope.ObjKetQua.TimeLock = moment(new Date()).format("YYYY/MM/DDTHH:mm:ss");
                $scope.Save();

            }

            $scope.ClickedStaff = function (Staff) {
                $scope.NguoiDuocDanhGia = Staff[0];
                $scope.HideTableData = true;
                getListChiTietKetQua(Staff[0].Id, $stateParams.KyDanhGiaId);

            }

            $scope.ChangeScore = function () {
                let sum = 0;
                $scope.ListChiTietKetQua.forEach(function (item) {
                    if (item.ParentIsChecked !== false)
                        sum += item.Diem;
                });
                $scope.ObjKetQua.TongDiem = sum;

            }

            $scope.ChangeRadio = function (obj) {
                $scope.ListChiTietKetQua.filter(e => e.TieuChiDanhGiaParentId == obj.TieuChiDanhGiaParentId).forEach(function (item) {
                    if (item.TieuChiDanhGiaId == obj.TieuChiDanhGiaId)
                        item.IsChecked = true;
                    else
                        item.IsChecked = false;
                });
            }

            $scope.ChangeRadioScore = function (obj) {
                if (!$scope.ObjKetQua.isLock) {
                    $scope.ListChiTietKetQua.filter(e => e.TieuChiDanhGiaParentId == obj.TieuChiDanhGiaParentId).forEach(function (item) {
                        if (item.TieuChiDanhGiaId == obj.TieuChiDanhGiaId) {
                            if (item.children.length == 0)
                                item.Diem = item.TieuChiDanhGiaDiemToiDa;
                            else {
                                item.Diem = 0;
                            }
                            item.IsChecked = true;
                        }
                        else {
                            item.Diem = 0;
                                item.GhiChu = null;
                            item.IsChecked = false;
                        }
                    });
                    obj.IsChecked = true;
                    $scope.ChangeScore();
                    if (obj.IsChildren) {
                        $scope.ListChiTietKetQua.filter(e => e.TieuChiDanhGiaParentId == obj.TieuChiDanhGiaParentId).forEach(function (item) {
                            $scope.ListChiTietKetQua.filter(e => e.TieuChiDanhGiaParentId == item.TieuChiDanhGiaId).forEach(function (item2) {
                                if (item2.TieuChiDanhGiaParentId == obj.TieuChiDanhGiaId)
                                    item2.ParentIsChecked = true;
                                else {
                                    item2.ParentIsChecked = false;
                                    item2.Diem = null;
                                    item2.GhiChu = null;
                                }
                            })
                        })
                        //$scope.Save();
                    }
                }
            }

        }
    ]);

});