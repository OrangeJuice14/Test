
define(['app/app',
    'app/components/abc/QuanLyDanhGia/QuanLyKyDanhGia/QuanLyKyDanhGiaService',
    'app/services/abc/ABC_UserDanhGiaService'],
    function (app) {
        "use strict";

        app.controller('QuanLyKyDanhGiaController', ['$scope', '$rootScope', '$modal', 'QuanLyKyDanhGiaService', 'ABC_UserDanhGiaService',
            function ($scope, $rootScope, $modal, QuanLyKyDanhGiaService, ABC_UserDanhGiaService) {

                var currentTime = new Date();
                $scope.nam = currentTime.getFullYear();
                $scope.GetKyDanhGia = function () {
                    $scope.kyDanhGiaList = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return QuanLyKyDanhGiaService.GetKyDanhGiaByNam($scope.nam).then(res => {
                                    options.success(res.data);
                                })
                            }
                        },
                        schema: {
                            model: {
                                id: "Id",
                                fields: {
                                    parentId: { field: "ParentId", nullable: true },
                                    Id: { field: "Id", type: "string" }
                                },
                                expanded: true
                            }
                        },
                    })
                }
                $scope.kyDanhGiaOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a ng-click='KyDanhGiaClick(dataItem)'  href='javascript:void(0)'>#:data.Name #</a>"
                    }]
                }

                $scope.TaoKyDanhGia = function () {

                    QuanLyKyDanhGiaService.NewKyDanhGia($scope.nam, $rootScope.session.UserId).then(res => {
                        if (res == false) {
                            Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                        }
                        else {
                            $scope.GetKyDanhGia();
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        }
                    })
                }
                $scope.GetKyDanhGia();
                $scope.KyDanhGiaClick = function (obj) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/QuanLyKyDanhGia/QuanLyKyDanhGia.Details.html',
                        controller: 'QuanLyKyDanhGiaDetailsController',
                        size: 'xs',
                        resolve: {
                            KyDanhGia: function () {
                                return obj;
                            },
                        }
                    }).result.then(function () {
                        $scope.GetKyDanhGia();
                    });
                }

            }
        ]);

        app.controller('QuanLyKyDanhGiaDetailsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'KyDanhGia', 'QuanLyKyDanhGiaService', 'ABC_UserDanhGiaService',
            function ($scope, $rootScope, $modal, $modalInstance, KyDanhGia, QuanLyKyDanhGiaService, ABC_UserDanhGiaService) {
                $scope.KyDanhGia = angular.copy(KyDanhGia);
                $scope.KyDanhGia.ParentId = $scope.KyDanhGia.parentId;
                $scope.KyDanhGia.NgayBatDauDanhGia = moment($scope.KyDanhGia.NgayBatDauDanhGia).format("DD/MM/YYYY");
                $scope.KyDanhGia.NgayKetThucDanhGia = moment($scope.KyDanhGia.NgayKetThucDanhGia).format("DD/MM/YYYY");

                $scope.Save = function () {
                    $scope.KyDanhGia.NgayBatDauDanhGia = moment($scope.KyDanhGia.NgayBatDauDanhGia, "DD/MM/YYYY").format("YYYY/MM/DD");
                    $scope.KyDanhGia.NgayKetThucDanhGia = moment($scope.KyDanhGia.NgayKetThucDanhGia, "DD/MM/YYYY").format("YYYY/MM/DD");
                    QuanLyKyDanhGiaService.SaveOrUpdate($scope.KyDanhGia, $rootScope.session.UserId).then(res => {
                        if (res == 1) {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $modalInstance.close();
                        }
                        else {
                            Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                        }
                    })
                }

                $scope.ChotTTTK = function () {
                    ABC_UserDanhGiaService.CheckChotTTTK($scope.KyDanhGia.Id).then(res => {
                        if (res.data != "" && confirm("Kỳ " + $scope.KyDanhGia.Name + " đã chốt thông tin tài khoản vào lúc: " + res.data)) {
                            ABC_UserDanhGiaService.ChotTTTK($scope.KyDanhGia.Id, $rootScope.session.UserId).then(res => {
                                if (res.data) {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                } else {
                                    Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                                }
                            });
                        }

                        if (res.data == "" ){
                            ABC_UserDanhGiaService.ChotTTTK($scope.KyDanhGia.Id, $rootScope.session.UserId).then(res => {
                                if (res.data) {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                } else {
                                    Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                                }
                            });
                        }
                    })
                }
            }
        ]);
    });