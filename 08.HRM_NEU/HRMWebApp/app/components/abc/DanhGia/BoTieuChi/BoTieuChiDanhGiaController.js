
define(['app/app',
    'app/components/abc/DanhGia/BoTieuChi/BoTieuChiDanhGiaService',
    'app/components/abc/QuanLyDanhGia/DieuKienBoTieuChi/DieuKienBoTieuChiService',
    'app/services/abc/ABC_UserDanhGiaService'], function (app) {
    "use strict";

    app.controller('BoTieuChiDanhGiaController',
        ['$scope', '$rootScope', '$timeout', '$modal', '$state', '$stateParams', '$modalInstance', 'KyDanhGiaId', 'BoTieuChiDanhGiaService', 'DieuKienBoTieuChiService','ABC_UserDanhGiaService',
            function ($scope, $rootScope, $timeout, $modal, $state, $stateParams, $modalInstance, KyDanhGiaId, BoTieuChiDanhGiaService, DieuKienBoTieuChiService, ABC_UserDanhGiaService) {
                var currentTime = new Date();
                $scope.KyDanhGiaId = KyDanhGiaId;
                $scope.nam = currentTime.getFullYear();
                $scope.ListBoTieuChi = [];
                $scope.User = null;
                var GetUserNow = new Promise(function (resolve, reject) {
                    ABC_UserDanhGiaService.GetUserById($rootScope.session.UserId, $scope.KyDanhGiaId).then(res => {
                        if (res.data == null) {
                            reject();
                            $scope.User = "ERRORS";
                        } else {
                            $scope.User = res.data;
                            resolve();
                        }
                    });
                })
                GetUserNow.then(() => {
                    $scope.GroupDanhGiaId = $('#group-danh-gia').find('li.active a').attr('href').replace('#', '');
                })

                $scope.BoTieuChiTuDanhGiaClick = function (boTieuChi) {

                    $scope.GroupDanhGiaId = $('#group-danh-gia').find('li.active a').attr('href').replace('#', '');
                    if (boTieuChi.HasDieuKienDanhGia) {
                        ABC_UserDanhGiaService.GetUserWithGroupDanhGiaId($rootScope.session.UserId, $scope.KyDanhGiaId, $scope.GroupDanhGiaId).then(res => {
                            $scope.UserDanhGia = res.data;

                            DieuKienBoTieuChiService.GetCheckDieuKienBoTieuChi(boTieuChi.Id, $scope.KyDanhGiaId, $scope.UserDanhGia.Id, $scope.GroupDanhGiaId).then(res => {

                                if (res.data == "") {
                                    $state.go("TuDanhGia", { KyDanhGiaId: $scope.KyDanhGiaId, BoTieuChiId: boTieuChi.Id, GroupDanhGiaId: $scope.GroupDanhGiaId });
                                    $modalInstance.close();
                                }
                                else
                                    if (res.data == "ERRORS") {

                                    } else {
                                        alert(res.data);
                                    }

                            });
                        })
                    }
                    else {
                        $state.go("TuDanhGia", { KyDanhGiaId: $scope.KyDanhGiaId, BoTieuChiId: boTieuChi.Id, GroupDanhGiaId: $scope.GroupDanhGiaId });
                        $modalInstance.close();
                    }
                }

                $scope.BoTieuChiDanhGiaCapDuoiClick = function (item) {
                    $scope.GroupDanhGiaId = $('#group-danh-gia').find('li.active a').attr('href').replace('#', '');
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
                                return $scope.GroupDanhGiaId;
                            }
                        }
                    }).result.then(function () {
                        $modalInstance.close();
                    });
                }
            }
        ]);
});