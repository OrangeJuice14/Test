
define(['app/app', , 'app/directives/directives', 'app/services/abc/ABC_TieuChiDanhGiaService', 'app/services/abc/ABC_KyDanhGiaService', 'app/services/abc/ABC_BoDanhGiaService', 'app/services/abc/ABC_KetQuaXepLoaiService', 'app/directives/ckeditorDirectives'], function (app) {
    "use strict";
    //var HRMWebAppModule = angular.module('HRMWebApp');
    app.controller('ABC_TieuChiDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'ABC_TieuChiDanhGiaService', 'ABC_KyDanhGiaService', 'ABC_BoDanhGiaService','ABC_KetQuaXepLoaiService',
        function ($scope, $rootScope, $modal, $state, $stateParams, ABC_TieuChiDanhGiaService, ABC_KyDanhGiaService, ABC_BoDanhGiaService, ABC_KetQuaXepLoaiService) {
            $scope.BoDanhGiaId = angular.copy($stateParams.BoDanhGiaId);
            $scope.ListKetQuaXepLoai = {};
            function listToTree(list) {
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].Id] = i; // initialize the map
                    list[i].children = []; // initialize the children
                    list[i].IsChildren = false;
                }
                for (i = 0; i < list.length; i += 1) {
                    node = list[i];
                    if (node.ParentId !== null) {
                        list[map[node.ParentId]].IsChildren = true;
                        // if you have dangling branches check that map[node.parentId] exists
                        list[map[node.ParentId]].children.push(node);
                    } else {
                        roots.push(node);
                    }
                }
                return roots;
            }

            function treeToView(tree, arrView) {
                for (let i = 0; i < tree.length; i += 1) {
                    arrView.push(tree[i]);
                    if (tree[i].children)
                        treeToView(tree[i].children, arrView);
                }
                return arrView;
            }
            function GetDataByBoDanhGiaId(BoDanhGiaId) {
                if (BoDanhGiaId != null && BoDanhGiaId != undefined) {
                    ABC_BoDanhGiaService.GetBoDanhGiaById(BoDanhGiaId).then(function (result) {
                        if (result.data.length != 0) {
                            $scope.BoDanhGia = result.data;
                            ABC_KetQuaXepLoaiService.GetListByDanhGiaId($scope.BoDanhGia.Id).then(function (result) {
                                if (result.data.length != 0) {
                                    $scope.ListKetQuaXepLoai = result.data;
                                }
                                else
                                    $scope.ListKetQuaXepLoai = null;
                            })
                        }
                    });
                    ABC_TieuChiDanhGiaService.GetTieuChiDanhGiaById(BoDanhGiaId).then(function (result) {
                        if (result.data.length != 0) {
                            $scope.ListTieuChiDanhGia = treeToView(listToTree(result.data), []);
                        }
                    });
                }
            }
            GetDataByBoDanhGiaId($scope.BoDanhGiaId);
            $scope.New = function () {
                $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_TieuChiDanhGia/popupAddTieuChiDanhGia.html',
                    controller: 'ABC_popupTieuChiDanhGiaController',
                    size: 'lg',
                    resolve: {
                        BoDanhGia: function () {
                            return $scope.BoDanhGia;
                        },
                        TieuChiDanhGia: function () {
                            return null;
                        },
                    }
                }).result.then(function () {
                    GetDataByBoDanhGiaId($scope.BoDanhGiaId);
                });
            }

            $scope.Edit = function (item) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/views/abc/ABC_TieuChiDanhGia/popupEditTieuChiDanhGia.html',
                        controller: 'ABC_popupTieuChiDanhGiaController',
                        size: 'lg',
                        resolve: {
                            BoDanhGia: function () {
                                return $scope.BoDanhGia;
                            },
                            TieuChiDanhGia: function () {
                                return item;
                            },
                        }
                    }).result.then(function () {
                        GetDataByBoDanhGiaId($scope.BoDanhGia.Id);
                    });
            }

            $scope.Lock = function () {
                $scope.BoDanhGia.IsLock = true;
                ABC_BoDanhGiaService.Update($scope.BoDanhGia).then(function (result) {
                    if (result.data == true) {
                        GetDataByBoDanhGiaId($scope.BoDanhGia.Id);
                    }
                });
            }

            $scope.UnLock = function () {
                $scope.BoDanhGia.IsLock = false;
                ABC_BoDanhGiaService.Update($scope.BoDanhGia).then(function (result) {
                    if (result.data == true) {
                        GetDataByBoDanhGiaId($scope.BoDanhGia.Id);
                    }
                });
            }

            $scope.DeleteKetQuaXepLoai = function (Id) {
                ABC_KetQuaXepLoaiService.Delete(Id).then(function (result) {
                    if (result.data == true) {
                        ABC_KetQuaXepLoaiService.GetListByDanhGiaId($scope.BoDanhGia.Id).then(function (result) {
                            if (result.data.length != 0) {
                                $scope.ListKetQuaXepLoai = result.data;
                            }
                            else
                                $scope.ListKetQuaXepLoai = null;
                        })
                    }
                })
            }

            $scope.NewKetQuaXepLoai = function (ObjXepLoai) {
                ObjXepLoai.DanhGiaId = $scope.BoDanhGia.Id;
                ABC_KetQuaXepLoaiService.New(ObjXepLoai).then(function (result) {
                    if (result.data == true)
                        ABC_KetQuaXepLoaiService.GetListByDanhGiaId($scope.BoDanhGia.Id).then(function (result) {
                            if (result.data.length != 0) {
                                $scope.ListKetQuaXepLoai = result.data;
                                $scope.ObjXepLoai = null;
                            }
                            else
                                $scope.ListKetQuaXepLoai = null;
                        })
                })
            } 
            
        }
    ]);

    app.controller('ABC_popupTieuChiDanhGiaController', ['BoDanhGia','TieuChiDanhGia', '$scope', '$rootScope', '$modal', '$modalInstance', 'ABC_TieuChiDanhGiaService',
        function (BoDanhGia, TieuChiDanhGia, $scope, $rootScope, $modal, $modalInstance, ABC_TieuChiDanhGiaService) {
            $scope.obj = {};
            $scope.ShowParent = false;
            $scope.BoDanhGia = BoDanhGia;
            if (TieuChiDanhGia != null) {
                $scope.obj = angular.copy(TieuChiDanhGia);
                if ($scope.obj.ParentId != null)
                    $scope.ShowParent = true;
            }
            $scope.save = function () {
                $scope.obj.DanhGiaId = BoDanhGia.Id;
                if ($scope.ShowParent == false || $scope.ShowParent == undefined)
                    $scope.obj.ParentId = null;
                ABC_TieuChiDanhGiaService.saveTieuChiDanhGia($scope.obj).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $modalInstance.close();
                });
            }
            $scope.update = function () {
                $scope.obj.DanhGiaId = BoDanhGia.Id;
                if ($scope.ShowParent == false || $scope.ShowParent == undefined)
                    $scope.obj.ParentId = null;
                ABC_TieuChiDanhGiaService.update($scope.obj).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $modalInstance.close();
                });
            }
            $scope.delete = function () {
                ABC_TieuChiDanhGiaService.delete($scope.obj).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $modalInstance.close();
                });
            }

            function getChiTietDanhGia(BoDanhGiaId) {
                ABC_TieuChiDanhGiaService.GetTieuChiDanhGiaById(BoDanhGiaId).then(function (result) {
                    if (result.data.length != 0) {
                        $scope.ListTieuChiDanhGia = result.data;
                       // $scope.obj.ParentId = $scope.ListTieuChiDanhGia[0].Id;
                    } else {
                        $scope.ListTieuChiDanhGia = null;
                    }

                })
            }
            getChiTietDanhGia(BoDanhGia.Id);
        }
    ]);
    app.controller('ABC_TieuChiDanhGiaNamController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', 'ABC_TieuChiDanhGiaService', 'ABC_KyDanhGiaService', 'ABC_BoDanhGiaService',
        function ($scope, $rootScope, $modal, $state, $stateParams, ABC_TieuChiDanhGiaService, ABC_KyDanhGiaService, ABC_BoDanhGiaService) {
            $scope.Loadding = true;
            function listToTree(list) {
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].Id] = i; // initialize the map
                    list[i].children = []; // initialize the children
                    list[i].IsChildren = false;
                }
                for (i = 0; i < list.length; i += 1) {
                    node = list[i];
                    if (node.ParentId !== null) {

                        list[map[node.ParentId]].IsChildren = true;
                        // if you have dangling branches check that map[node.parentId] exists
                        list[map[node.ParentId]].children.push(node);
                    } else {
                        roots.push(node);
                    }
                }
                return roots;
            }

            function treeToView(tree, arrView) {
                for (let i = 0; i < tree.length; i += 1) {
                    arrView.push(tree[i]);
                    if (tree[i].children)
                        treeToView(tree[i].children, arrView);
                }
                return arrView;
            }
            function GetDataByBoDanhGiaId(BoDanhGiaId) {
                $scope.Loadding = true;
                ABC_TieuChiDanhGiaService.GetTieuChiDanhGiaById(BoDanhGiaId).then(function (result) {
                    if (result.data.length != 0) {
                        $scope.ListTieuChiDanhGia = treeToView(listToTree(result.data), []);
                        $scope.Loadding = false;
                    }
                    else {
                        var obj = {};
                        obj.STT = -1;
                        obj.ChildSelectOne = true;
                        obj.DanhGiaId = BoDanhGiaId;
                        ABC_TieuChiDanhGiaService.saveTieuChiDanhGia(obj).then(function () {
                            ABC_TieuChiDanhGiaService.GetTieuChiDanhGiaById(BoDanhGiaId).then(function (resultCT) {
                                var obj1 = {}, obj2 = {};
                                obj1.STT = "A";
                                obj1.TenTieuChi = "Tự đánh giá";
                                obj1.ParentId = resultCT.data[0].Id;
                                obj1.DanhGiaId = BoDanhGiaId;
                                ABC_TieuChiDanhGiaService.saveTieuChiDanhGia(obj1).then(function () {
                                    obj2.STT = "B";
                                    obj2.TenTieuChi = "Quản lý đánh giá";
                                    obj2.ParentId = resultCT.data[0].Id;
                                    obj2.DanhGiaId = BoDanhGiaId;
                                    ABC_TieuChiDanhGiaService.saveTieuChiDanhGia(obj2).then(function () {
                                        GetDataByBoDanhGiaId(BoDanhGiaId);
                                    });
                                });
                            });
                        });
                    }
                });
            }

            $scope.New = function () {
                $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_TieuChiDanhGia/TieuChiDanhGiaNam/popupAddTieuChiDanhGiaNam.html',
                    controller: 'ABC_popupTieuChiDanhGiaController',
                    size: 'lg',
                    resolve: {
                        BoDanhGia: function () {
                            return $scope.BoDanhGia;
                        },
                        TieuChiDanhGia: function () {
                            return null;
                        },
                    }
                }).result.then(function () {
                    GetDataByBoDanhGiaId($scope.BoDanhGia.Id);
                });
            }

            $scope.Edit = function (item) {
                $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_TieuChiDanhGia/TieuChiDanhGiaNam/popupEditTieuChiDanhGiaNam.html',
                    controller: 'ABC_popupTieuChiDanhGiaController',
                    size: 'lg',
                    resolve: {
                        BoDanhGia: function () {
                            return $scope.BoDanhGia;
                        },
                        TieuChiDanhGia: function () {
                            return item;
                        },
                    }
                }).result.then(function () {
                    GetDataByBoDanhGiaId($scope.BoDanhGia.Id);
                });
            }

            $scope.Lock = function () {
                $scope.BoDanhGia.IsLock = true;
                ABC_BoDanhGiaService.Update($scope.BoDanhGia).then(function (result) {
                    if (result.data == true) {
                        GetDataByBoDanhGiaId($scope.BoDanhGia.Id);
                    }
                });
            }

            ABC_BoDanhGiaService.GetBoDanhGiaById($stateParams.BoDanhGiaId).then(function (result) {
                if (result.data.length != 0) {
                    $scope.BoDanhGia = result.data;
                    GetDataByBoDanhGiaId($scope.BoDanhGia.Id);
                }
            });

        }
    ]);

});