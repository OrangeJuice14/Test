
define(['app/app', 'app/services/abc/ABC_DongNghiepDanhGiaService', 'app/services/abc/ABC_KyDanhGiaService', 'app/services/abc/WebUserService', 'app/directives/treedirective', 'app/services/abc/ABC_ChiTietKetQuaService', 'app/services/abc/ABC_BoDanhGiaService', 'app/services/abc/ABC_TieuChiDanhGiaService', 'app/services/abc/ABC_KetQuaService', 'app/services/abc/staffService', 'app/services/abc/ABC_ChiTietNhanVienDanhGiaService'], function (app) {
    "use strict";
    //var HRMWebAppModule = angular.module('HRMWebApp');
    app.controller('ABC_KyDongNghiepDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', 'ABC_DongNghiepDanhGiaService', 'ABC_KyDanhGiaService', 'WebUserService',
        function ($scope, $rootScope, $modal, $state, ABC_DongNghiepDanhGiaService, ABC_KyDanhGiaService, WebUserService) {
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

            WebUserService.getWebUserByUserId($rootScope.session.UserId).then(function (result) {
                if (result.data.StaffInfoId != "00000000-0000-0000-0000-000000000000") {
                    //$scope.Show
                }
            });

            $scope.New = function () {
                //alert($scope.Nam);
                ABC_KyDanhGiaService.saveKyDanhGia($scope.Nam).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $scope.UpdateYearValue();
                });

            }
            $scope.UpdateYearValue = function () {
                ABC_KyDanhGiaService.getListKyDanhGia($scope.Nam).then(function (result) {
                    $scope.TreeKyDanhGia = listToTree(angular.copy(result.data));
                });
            }
            $scope.UpdateYearValue();
            $scope.KyDanhGiaClicked = function (KyDanhGiaId) {
                $state.go("DanhGiaDongNghiep", { KyDanhGiaId: KyDanhGiaId });
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

    app.controller('ABC_DongNghiepDanhGiaController', ['$scope', '$rootScope', '$modal', '$stateParams', 'ABC_ChiTietKetQuaService', 'ABC_KyDanhGiaService', 'ABC_BoDanhGiaService', 'ABC_TieuChiDanhGiaService', 'WebUserService', 'ABC_KetQuaService', 'staffService', 'ABC_ChiTietNhanVienDanhGiaService',
        function ($scope, $rootScope, $modal, $stateParams, ABC_ChiTietKetQuaService, ABC_KyDanhGiaService, ABC_BoDanhGiaService, ABC_TieuChiDanhGiaService, WebUserService, ABC_KetQuaService, staffService, ABC_ChiTietNhanVienDanhGiaService) {

            $scope.LoaddingTableData = false;
            $scope.LoaddingBody = false;
            $scope.HideTableData = true;
            $scope.KyDanhGia = {};
            $scope.SearchText = '';
            $scope.NguoiDuocDanhGia = {};
            ABC_KyDanhGiaService.getKyDanhGiaById($stateParams.KyDanhGiaId).then(function (result) {
                $scope.KyDanhGia = result.data;
            });
            WebUserService.getWebUserByUserId($rootScope.session.UserId).then(function (resultUser) {
                $scope.ObjStaff = resultUser.data;
                if ($scope.ObjStaff.StaffInfoPositionId.toUpperCase() == 'D5ED97E6-0D1A-4159-9B4A-E43CC31BE292')
                    $scope.ShowWithAdmin = true;
                else
                    $scope.ShowWithAdmin = false;
                staffService.getListStaff($scope.ObjStaff.StaffInfoId).then(function (result) {
                    $scope.ListStaff = result.data;
                    $scope.LoaddingBody = true;
                });
            });


            function listToTree(list) {
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].TieuChiDanhGiaId] = i; // initialize the map
                    list[i].children = []; // initialize the children
                    list[i].IsChildren = false;
                    list[i].isRadio = false;
                    list[i].childRadio = false;
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
                $scope.HideTableData = true;
                $scope.Loadding = true;

                if (KyDanhGiaId != undefined && StaffId != undefined)
                    ABC_ChiTietKetQuaService.getListChiTietKetQua(KyDanhGiaId, StaffId, $scope.ObjStaff.StaffInfoId, 2).then(function (result) {
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
                    });
            }

            $scope.Save = function () {
                try {
                    ABC_ChiTietNhanVienDanhGiaService.PutUpdate($scope.ObjKetQua);
                    ABC_ChiTietKetQuaService.saveChiTietKetQua($scope.ListChiTietKetQua).then(function (result) {
                        if (result.data == true) {
                            getListChiTietKetQua($scope.NguoiDuocDanhGia.Id, $stateParams.KyDanhGiaId);
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $scope.DisableLock = false;
                        } else {
                            Notify('Lỗi!', 'top-right', '3000', 'error', 'fa-exclamation-circle', true);
                        }
                    });
                }
                catch (err) {
                    Notify('Lỗi!', 'top-right', '3000', 'error', 'fa-exclamation-circle', true);
                }
            }

            $scope.ClickedStaff = function (Staff) {
                $scope.NguoiDuocDanhGia = Staff[0];
                $scope.HideTableData = true;
                getListChiTietKetQua(Staff[0].Id, $stateParams.KyDanhGiaId);
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