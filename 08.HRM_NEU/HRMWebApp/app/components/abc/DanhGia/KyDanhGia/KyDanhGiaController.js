
define(['app/app',
    'app/components/abc/DanhGia/KyDanhGia/KyDanhGiaService',
    'app/components/abc/DanhGia/BoTieuChi/BoTieuChiDanhGiaController',
    'app/components/abc/DanhGia/DanhGiaDongNghiep/BoTieuChiDanhGiaDongNghiepController'],
    function (app) {
        "use strict";
        app.controller('KyDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', 'KyDanhGiaService',
            function ($scope, $rootScope, $modal, $state, KyDanhGiaService) {

                var currentTime = new Date();
                $scope.UserId = angular.copy($rootScope.session.UserId);
                //KyDanhGiaService.GetUserGroupDanhGiaRoleByUserId($scope.UserId).then(res => {
                //    $scope.UserDanhGiaRole = res.data;
                //})
                var DataListNam = new Promise(function (resolve, reject) {
                    $scope.DataListNam = {
                        placeholder: "Chọn tiêu chí...",
                        valuePrimitive: true,
                        autoBind: true,
                        dataSource: {
                            transport: {
                                read: function (options) {
                                    return KyDanhGiaService.GetListNam(currentTime.getFullYear()).then(res => {
                                        options.success(res.data);
                                        resolve();
                                    });
                                }
                            }
                        }
                    }
                })


                $scope.GetKyDanhGia = function () {
                    $scope.kyDanhGiaList = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return KyDanhGiaService.GetListByNam($scope.nam).then(res => {
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

                $scope.kyDanhGiaCapDuoiOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a ng-click='KyDanhGiaCapDuoiClick(dataItem)'  href='javascript:void(0)'>#:data.Name #</a>"
                    }]
                }

                DataListNam.then(function () {
                    $scope.nam = currentTime.getFullYear();
                    $scope.GetKyDanhGia();
                    $scope.$apply();
                })

                $scope.KyDanhGiaClick = function (KyDanhGia) {
                    if ((moment(KyDanhGia.NgayBatDauDanhGia) <= moment(currentTime) && moment(currentTime) <= moment(KyDanhGia.NgayKetThucDanhGia))
                        || moment(currentTime) <= moment(KyDanhGia.NgayKetThucDanhGia)
                        || (moment(KyDanhGia.NgayBatDauDanhGia) <= moment(currentTime) && KyDanhGia.NgayKetThucDanhGia == null)) {
                        $modal.open({
                            animation: true,
                            templateUrl: '/app/components/abc/DanhGia/BoTieuChi/index.html',
                            controller: 'BoTieuChiDanhGiaController',
                            size: 'lg',
                            resolve: {
                                KyDanhGiaId: function () {
                                    return KyDanhGia.Id;
                                }
                            }
                        }).result.then(function () {
                        });
                    } else {
                        alert("Chưa đến hạn đánh giá!!!");
                    }
                }
                $scope.KyDanhGiaCapDuoiClick = function (KyDanhGia) {
                    $scope.GroupDanhGiaId = $('#group-danh-gia').find('li.active a').attr('href').replace('#', '');
                    if ((moment(KyDanhGia.NgayBatDauDanhGia) <= moment(currentTime) && moment(currentTime) <= moment(KyDanhGia.NgayKetThucDanhGia))
                        || moment(currentTime) <= moment(KyDanhGia.NgayKetThucDanhGia)
                        || (moment(KyDanhGia.NgayBatDauDanhGia) <= moment(currentTime) && KyDanhGia.NgayKetThucDanhGia == null)) {
                        $modal.open({
                            animation: true,
                            templateUrl: '/app/components/abc/DanhGia/DanhGiaDongNghiep/BoTieuChiDanhGiaDongNghiep.html',
                            controller: 'BoTieuChiDanhGiaDongNghiepController',
                            size: 'lg',
                            resolve: {
                                KyDanhGiaId: function () {
                                    return KyDanhGia.Id;
                                },
                                GroupDanhGiaId: function () {
                                    return $scope.GroupDanhGiaId;
                                }
                            }
                        }).result.then(function () {
                        });
                    } else {
                        alert("Chưa đến hạn đánh giá!!!");
                    }
                }
            }
        ]);
    });