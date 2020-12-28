
define(['app/app', 'app/services/abc/ABC_BoDanhGiaService', 'app/services/abc/ABC_KyDanhGiaService', 'app/services/abc/ABC_LoaiBoDanhGiaService'], function (app) {
    "use strict";
    //var HRMWebAppModule = angular.module('HRMWebApp');
    app.controller('ABC_BoDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', 'ABC_BoDanhGiaService', 'ABC_KyDanhGiaService',
        function ($scope, $rootScope, $modal, $state, ABC_BoDanhGiaService, ABC_KyDanhGiaService) {

            $scope.kyDanhGia = {}

            function GetAllListBoDanhGia() {
                ABC_BoDanhGiaService.GetListBoDanhGiaByNam($scope.Nam).then(function (result) {
                    if (result.data.lenght != 0) {
                        $scope.ListBoDanhGia = result.data;
                        $.each($scope.ListBoDanhGia,function () {
                            this.TuNgay = moment(this.TuNgay).format("DD/MM/YYYY");
                            this.DenNgay = moment(this.DenNgay).format("DD/MM/YYYY");
                        });
                    }
                    else {
                        $scope.ListBoDanhGia = null;
                    }
                });
            }

            $scope.New = function () {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_BaoCaoDanhGia/popupAddBaoCaoDanhGia.html',
                    controller: 'ABC_popupAddBaoCaoDanhGiaController',
                    size: 'lg',
                    resolve: {
                        kyDanhGia: function () {
                            return $scope.selected;
                        },
                    }
                }).result.then(function () {
                    GetAllListBoDanhGia();
                });
            };

            $scope.editBoDanhGia = function (obj) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_BaoCaoDanhGia/popupEditBaoCaoDanhGia.html',
                    controller: 'ABC_popupEditBaoCaoDanhGiaController',
                    size: 'lg',
                    resolve: {
                        DanhGia: function () {
                            return obj;
                        },
                    }
                }).result.then(function () {
                    GetAllListBoDanhGia();
                });
            };



            var currentTime = new Date();
            $scope.Nam = currentTime.getFullYear();

            $scope.UpdateYearValue = function () {
                GetAllListBoDanhGia();
            };
            $scope.UpdateYearValue();
            $scope.ShowTieuChiDanhGia = function (BoDanhGia) {
                //alert(BoDanhGiaId);
                if (BoDanhGia.LoaiBoDanhGiaId.toUpperCase() == "7B83ED33-DBA6-4378-8BCC-20B108092CDD") {
                    $state.go("TieuChiDanhGiaNam", { BoDanhGiaId: BoDanhGia.Id });
                }
                else
                    $state.go("TieuChiDanhGia", { BoDanhGiaId: BoDanhGia.Id });
            }
            $scope.deleteBoDanhGia = function (Id) {
                ABC_BoDanhGiaService.getDelete(Id).then(function (result) {
                    if (result.data == true) {
                        GetAllListBoDanhGia();
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    }
                    else {

                        Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                    }
                });
            }
        }
    ]);
    app.controller('ABC_popupAddBaoCaoDanhGiaController', ['$scope', '$rootScope', '$modal', 'kyDanhGia', '$modalInstance', 'ABC_BoDanhGiaService', 'ABC_KyDanhGiaService', 'ABC_LoaiBoDanhGiaService',
        function ($scope, $rootScope, $modal, kyDanhGia, $modalInstance, ABC_BoDanhGiaService, ABC_KyDanhGiaService, ABC_LoaiBoDanhGiaService) {

            ABC_LoaiBoDanhGiaService.GetAll().then(function (result) {
                $scope.ListBoDanhGia = result.data;
            })

            $scope.saveBoDanhGia = function () {
                var a = $scope.obj;
                $scope.obj.TuNgay = moment($scope.obj.TuNgay).format("YYYY/MM/DD");
                $scope.obj.DenNgay = moment($scope.obj.DenNgay).format("YYYY/MM/DD");
                ABC_BoDanhGiaService.saveBoDanhGia($scope.obj).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $modalInstance.close();
                });

            };
        }
    ]);
    app.controller('ABC_popupEditBaoCaoDanhGiaController', ['DanhGia','$scope', '$rootScope', '$modal', '$modalInstance', 'ABC_BoDanhGiaService', 'ABC_KyDanhGiaService', 'ABC_LoaiBoDanhGiaService',
        function (DanhGia, $scope, $rootScope, $modal, $modalInstance, ABC_BoDanhGiaService, ABC_KyDanhGiaService, ABC_LoaiBoDanhGiaService) {
            
            $scope.obj = angular.copy(DanhGia);
            $scope.obj.TuNgay = moment($scope.obj.TuNgay, "DD/MM/YYYY");
            $scope.obj.DenNgay = moment($scope.obj.DenNgay, "DD/MM/YYYY");
            ABC_LoaiBoDanhGiaService.GetAll().then(function (result) {
                $scope.ListBoDanhGia = result.data;
            })

            $scope.updateBoDanhGia = function () {
                $scope.obj.TuNgay = moment($scope.obj.TuNgay).format("YYYY/MM/DD");
                $scope.obj.DenNgay = moment($scope.obj.DenNgay).format("YYYY/MM/DD");
                ABC_BoDanhGiaService.Update($scope.obj).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $modalInstance.close();
                });

            };
        }
    ]);
});