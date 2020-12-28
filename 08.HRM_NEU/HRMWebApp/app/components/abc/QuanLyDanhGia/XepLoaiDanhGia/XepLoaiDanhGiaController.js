
define(['app/app', 'app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGiaService', 'app/components/abc/QuanLyDanhGia/TieuChi/TieuChiService'], function (app) {
    "use strict";

    app.controller('XepLoaiDanhGiaController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'XepLoaiDanhGiaService', 'BoTieuChiId', 'XepLoaiDanhGiaId', 'TieuChiService',
        function ($scope, $rootScope, $modal, $modalInstance, XepLoaiDanhGiaService, BoTieuChiId, XepLoaiDanhGiaId, TieuChiService) {
            $scope.XepLoaiDanhGia = {};
            $scope.DieuKienXepLoaiPhu = {};
            if (XepLoaiDanhGiaId != null) {
                XepLoaiDanhGiaService.GetByXepLoaiDanhGiaId(XepLoaiDanhGiaId).then(res => {
                    if (res.data != null) {
                        $scope.XepLoaiDanhGia = res.data;
                        if ($scope.XepLoaiDanhGia.HasDieuKienTieuChi) {
                            XepLoaiDanhGiaService.GetDieuKienXepLoaiPhuByXepLoaiDanhGiaId($scope.XepLoaiDanhGia.Id).then(resDieuKienPhu => {
                                $scope.DieuKienXepLoaiPhu = resDieuKienPhu.data;
                            })
                        }
                    }
                    else
                        Notify('Lỗi!!!', 'top-right', '3000', 'custom', 'fa-warning', true);
                });
            } else {
                $scope.XepLoaiDanhGia.BoTieuChiId = BoTieuChiId;
            }
            $scope.ListTieuChiOption = {
                dataTextField: "NoiDung",
                dataValueField: "Id",
                placeholder: "Chọn tiêu chí...",
                valuePrimitive: true,
                autoBind: false,
                filter: 'contains',
                valueTemplate: `<div ng-bind-html="' #:data.NoiDung # '"></div> `,
                template: `<div ng-bind-html="' #:data.NoiDung # '"></div> `,
                change: function () {
                    this.refresh();
                }
            }

            $scope.TieuChiDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return TieuChiService.GetByBoTieuId(BoTieuChiId).then(res => {
                            res.data.forEach(item => {
                                item.NoiDung = new DOMParser().parseFromString('<!doctype html><body>' + item.NoiDung, 'text/html').body.innerHTML.replace(/&nbsp;/g, " ");
                            })
                            options.success(res.data);
                        });
                    }
                }
            });
            $scope.Save = function () {
                XepLoaiDanhGiaService.SaveOrUpdateXepLoaiDanhGia($scope.XepLoaiDanhGia).then(res => {
                    if (res != MANAGER.GUID_EMPTY) {
                        if ($scope.XepLoaiDanhGia.HasDieuKienTieuChi) {
                            $scope.DieuKienXepLoaiPhu.XepLoaiDanhGiaId = res;
                            XepLoaiDanhGiaService.SaveOrUpdateDieuKienXepLoai($scope.DieuKienXepLoaiPhu).then(res => {
                                if (res == MANAGER.GUID_EMPTY)
                                    Notify('Lỗi lưu điều kiện tiêu chí', 'top-right', '3000', 'custom', 'fa-warning', true);
                                else {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $modalInstance.close();
                                }

                            });
                        } else {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $modalInstance.close();
                        }
                    } else {
                        Notify('Thất bại!', 'top-right', '3000', 'custom', 'fa-warning', true);
                    }
                });
            }
        }
    ]);
});