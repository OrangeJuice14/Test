
define(['app/app', 'app/components/abc/DanhGia/BoTieuChi/BoTieuChiDanhGiaService', 'app/components/abc/DanhGia/DanhGiaDongNghiep/ListDongNghiep/ListDongNghiepController'], function (app) {
    "use strict";

    app.controller('BoTieuChiDanhGiaDongNghiepController', ['$scope', '$rootScope', '$modal', '$state', '$stateParams', '$modalInstance', 'KyDanhGiaId', 'GroupDanhGiaId','BoTieuChiDanhGiaService',
        function ($scope, $rootScope, $modal, $state, $stateParams, $modalInstance, KyDanhGiaId, GroupDanhGiaId, BoTieuChiDanhGiaService) {
            var currentTime = new Date();
            $scope.KyDanhGiaId = KyDanhGiaId;
            $scope.nam = currentTime.getFullYear();
            $scope.ListBoTieuChi = {};
            $scope.GetBoTieuChi = function () {
                BoTieuChiDanhGiaService.GetBoTieuChiDanhGiaDongNghiep($rootScope.session.UserId, GroupDanhGiaId, $scope.KyDanhGiaId).then(res => {
                    $scope.ListBoTieuChi = res.data;
                    $scope.ListBoTieuChi.forEach(item => {
                        item.TuNgay = moment(item.TuNgay).format("DD/MM/YYYY");
                        item.DenNgay = moment(item.DenNgay).format("DD/MM/YYYY");
                    })
                })
            }

            $scope.GetBoTieuChi();

            $scope.BoTieuChiClick = function (item) {

                $modal.open({
                    animation: true,
                    templateUrl: '/app/components/abc/DanhGia/DanhGiaDongNghiep/ListDongNghiep/ListDongNghiep.html',
                    controller: 'ListDongNghiepController',
                    size: 'lg',
                    resolve: {
                        BoTieuChiId: function () {
                            return item.Id;
                        },
                        KyDanhGiaId: function () {
                            return KyDanhGiaId;
                        },
                        GroupDanhGiaId: function () {
                            return GroupDanhGiaId;
                        }
                    }
                }).result.then(function () {
                    $modalInstance.close();
                });
            }
        }
    ]);
});