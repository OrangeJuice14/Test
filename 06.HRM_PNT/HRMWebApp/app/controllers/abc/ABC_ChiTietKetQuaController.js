
define(['app/app', 'moment', 'app/directives/directives', 'app/services/abc/ABC_ChiTietKetQuaService', 'app/directives/ckeditorDirectives', 'app/services/abc/ABC_KyDanhGiaService', 'app/services/abc/ABC_BoDanhGiaService', 'app/services/abc/ABC_TieuChiDanhGiaService', 'app/services/abc/WebUserService', 'app/services/abc/ABC_KetQuaService', 'app/services/abc/ABC_ChiTietNhanVienDanhGiaService', 'app/services/abc/staffService', 'app/services/abc/ABC_LoaiDanhGiaService', 'app/services/abc/ABC_KetQuaXepLoaiService'], function (app) {
    "use strict";
    //var HRMWebAppModule = angular.module('HRMWebApp');
    app.controller('ABC_ChiTietKetQuaController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'ABC_ChiTietKetQuaService', 'ABC_KyDanhGiaService', 'WebUserService', 'ABC_ChiTietNhanVienDanhGiaService', 'ABC_KetQuaXepLoaiService', 'ABC_BoDanhGiaService',
        function ($scope, $rootScope, $modal, $state, $stateParams, ABC_ChiTietKetQuaService, ABC_KyDanhGiaService, WebUserService, ABC_ChiTietNhanVienDanhGiaService, ABC_KetQuaXepLoaiService, ABC_BoDanhGiaService) {
            var moment = require('moment');
            $scope.Loadding = false;

            var PhanLoaiSTT = "A";
            $scope.ListChiTietKetQua = {};
            ABC_KyDanhGiaService.getKyDanhGiaById($stateParams.KyDanhGiaId).then(function (result) {
                $scope.KyDanhGia = result.data;
            });
            ABC_BoDanhGiaService.getBoDanhGiaByTimeNow($stateParams.KyDanhGiaId, 1).then(function (result) {
                $scope.BoDanhGia = result.data;
                ABC_KetQuaXepLoaiService.GetListByDanhGiaId($scope.BoDanhGia.Id).then(function (resultXepLoai) {
                    $scope.ListXepLoai = resultXepLoai.data;
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



            function getListChiTietKetQua(KyDanhGiaId, StaffId) {
                $scope.Loadding = true;
                if (KyDanhGiaId != undefined && StaffId != undefined) {
                    ABC_ChiTietKetQuaService.getListChiTietKetQua(KyDanhGiaId, StaffId, StaffId, 1).then(function (result) {
                        if (result.data.length != 0) {
                            $scope.ListChiTietKetQua = treeToView(listToTree(result.data), []);
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
                    });
                }
            }

            function getWebUserByUserId() {
                WebUserService.getWebUserByUserId($rootScope.session.UserId).then(function (resultUser) {
                    $scope.ObjStaff = resultUser.data;
                    getListChiTietKetQua($stateParams.KyDanhGiaId, $scope.ObjStaff.StaffInfoId);
                });
            }

            getWebUserByUserId();

            $scope.Save = function () {
                try {
                    ABC_ChiTietNhanVienDanhGiaService.PutUpdate($scope.ObjKetQua);
                    ABC_ChiTietKetQuaService.saveChiTietKetQua($scope.ListChiTietKetQua).then(function (result) {
                        if (result.data == true) {
                            getListChiTietKetQua($stateParams.KyDanhGiaId, $scope.ObjStaff.StaffInfoId);
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        } else {
                            Notify('Lỗi!', 'top-right', '3000', 'error', 'fa-exclamation-circle', true);
                        }
                    })
                }
                catch (err) {
                    Notify('Lỗi!', 'top-right', '3000', 'error', 'fa-exclamation-circle', true);
                }

            }

            $scope.Lock = function () {
                $scope.ObjKetQua.isLock = true;
                $scope.ObjKetQua.TimeLock = moment(new Date()).format("YYYY/MM/DDTHH:mm:ss");
                $scope.Save();
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
            $scope.check = function (item) {
                console.log()
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

            $scope.KetQuaDongNghiepDanhGia = function () {
                $state.go("KetQuaDongNghiepDanhGia", { KetQuaId: $scope.ObjKetQua.KetQuaId, StaffId: $scope.ObjStaff.StaffInfoId });
            }
            $scope.KetQuaCapTrenDanhGia = function () {
                $state.go("KetQuaCapTrenDanhGia", { KetQuaId: $scope.ObjKetQua.KetQuaId });
            }
        }
    ]);

    app.controller('ABC_KyTuDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', 'ABC_KyDanhGiaService', 'WebUserService',
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


            $scope.New = function () {
                //alert($scope.Nam);
                ABC_KyDanhGiaService.saveKyDanhGia($scope.Nam).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $scope.UpdateYearValue();
                });

            }
            $scope.UpdateYearValue = function () {
                //alert($scope.Nam);
                ABC_KyDanhGiaService.getListKyDanhGia($scope.Nam).then(function (result) {
                    $scope.TreeKyDanhGia = listToTree(angular.copy(result.data));
                });
            }
            $scope.UpdateYearValue();
            $scope.KyDanhGiaClicked = function (KyDanhGiaId) {
                        $state.go("ChiTietKetQua", { KyDanhGiaId: KyDanhGiaId });
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

    app.controller('ABC_KetQuaDongNghiepDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'staffService', 'ABC_KetQuaService', 'ABC_ChiTietNhanVienDanhGiaService', 'ABC_LoaiDanhGiaService', 'ABC_ChiTietKetQuaService',
        function ($scope, $rootScope, $modal, $state, $stateParams, staffService, ABC_KetQuaService, ABC_ChiTietNhanVienDanhGiaService, ABC_LoaiDanhGiaService, ABC_ChiTietKetQuaService) {
            var moment = require('moment');
            $scope.ListChiTietKetQua = {};
            $scope.StaffId = angular.copy($stateParams.StaffId);
            $scope.StaffDanhGia = null;
            $scope.Loadding = true
            staffService.getListStaff($scope.StaffId).then(function (result) {
                $scope.ListStaff = result.data;
                $scope.Loadding = false;
            });

            ABC_KetQuaService.getById($stateParams.KetQuaId).then(function (result) {
                $scope.KetQua = result.data;
            });
            ABC_LoaiDanhGiaService.getByMaLoai(2).then(function (result) {
                $scope.LoaiDanhGia = result.data;
            })


            function listToTree(list) {
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].TieuChiDanhGiaId] = i; // initialize the map
                    list[i].children = []; // initialize the children
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
                        list[map[node.TieuChiDanhGiaParentId]].children.push(node);
                    } else {
                        roots.push(node);
                    }
                }
                return roots;
            }
            function treeToView(tree, arrView, ParentIsChecked = null) {
                for (let i = 0; i < tree.length; i += 1) {
                    tree[i].ParentIsChecked = ParentIsChecked;
                    if (tree[i].ParentIsChecked === false) {
                        tree[i].Diem = null;
                    }
                    arrView.push(tree[i]);
                    if (tree[i].children) {
                        if (tree[i].IsChecked === true)
                            treeToView(tree[i].children, arrView, true);
                        else if (tree[i].IsChecked === false)
                            treeToView(tree[i].children, arrView, false);
                        else
                            treeToView(tree[i].children, arrView);
                    }
                }
                return arrView;
            }

            function getListChiTietKetQua(NhanVienDanhGiaId, KetQuaId, LoaiDanhGiaId) {
                $scope.BodyLoadding = true;
                ABC_ChiTietNhanVienDanhGiaService.getByRef(NhanVienDanhGiaId, KetQuaId, LoaiDanhGiaId).then(function (result) {

                    $scope.ChiTietNhanVienDanhGia = result.data;
                    if ($scope.ChiTietNhanVienDanhGia != null) {
                        if ($scope.ChiTietNhanVienDanhGia.isLock) {
                            ABC_ChiTietKetQuaService.getListByRef($scope.ChiTietNhanVienDanhGia.Id).then(function (resultChiTietKetQua) {
                                if (resultChiTietKetQua.data.length != 0)
                                    $scope.ListChiTietKetQua = treeToView(listToTree(resultChiTietKetQua.data), []);
                                $scope.BodyLoadding = false;
                            });
                        } else {
                            $scope.BodyLoadding = false;
                        }

                    }
                });

            }

            $scope.ClickedStaff = function (Staff) {
                $scope.StaffDanhGia = Staff[0];
                getListChiTietKetQua($scope.StaffDanhGia.Id, $scope.KetQua.Id, $scope.LoaiDanhGia.Id);
            }

        }
    ]);

    app.controller('ABC_KetQuaCapTrenDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'staffService', 'ABC_KetQuaService', 'ABC_ChiTietNhanVienDanhGiaService', 'ABC_LoaiDanhGiaService', 'ABC_ChiTietKetQuaService',
        function ($scope, $rootScope, $modal, $state, $stateParams, staffService, ABC_KetQuaService, ABC_ChiTietNhanVienDanhGiaService, ABC_LoaiDanhGiaService, ABC_ChiTietKetQuaService) {
            var moment = require('moment');
            $scope.ListChiTietKetQua = null;
            $scope.Loadding = true;
            var PhanLoaiSTT = "A";
            function listToTree(list) {
                PhanLoaiSTT = "A";
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].TieuChiDanhGiaId] = i; // initialize the map
                    list[i].children = []; // initialize the children
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
                        list[map[node.TieuChiDanhGiaParentId]].children.push(node);
                    } else {
                        roots.push(node);
                    }
                }
                return roots;
            }
            function treeToView(tree, arrView, ParentIsChecked = null) {
                for (let i = 0; i < tree.length; i += 1) {
                    if (tree[i].TieuChiDanhGiaSTT == "B")
                        PhanLoaiSTT = "B";
                    tree[i].PhanLoaiSTT = PhanLoaiSTT;
                    tree[i].ParentIsChecked = ParentIsChecked;
                    if (tree[i].ParentIsChecked === false) {
                        tree[i].Diem = null;
                    }
                    arrView.push(tree[i]);
                    if (tree[i].children) {
                        if (tree[i].IsChecked === true)
                            treeToView(tree[i].children, arrView, true);
                        else if (tree[i].IsChecked === false)
                            treeToView(tree[i].children, arrView, false);
                        else
                            treeToView(tree[i].children, arrView);
                    }
                }
                return arrView;
            }

            ABC_KetQuaService.getById($stateParams.KetQuaId).then(function (result) {
                $scope.KetQua = result.data;
                ABC_ChiTietNhanVienDanhGiaService.getWithTruongPhong($scope.KetQua.Id).then(function (result) {
                    $scope.ChiTietNhanVienDanhGia = result.data;
                    if ($scope.ChiTietNhanVienDanhGia != null) {
                        if ($scope.ChiTietNhanVienDanhGia.isLock) {
                            ABC_ChiTietKetQuaService.getListByRef($scope.ChiTietNhanVienDanhGia.Id).then(function (resultChiTietKetQua) {
                                if (resultChiTietKetQua.data.length != 0) {
                                    $scope.ListChiTietKetQua = treeToView(listToTree(resultChiTietKetQua.data), []);
                                    ABC_ChiTietNhanVienDanhGiaService.getById(resultChiTietKetQua.data[0].ChiTietNhanVienDanhGiaId).then(function (resultKetQua) {
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
                            });

                        }

                    }
                });
                $scope.Loadding = false;
            });


        }
    ]);


});