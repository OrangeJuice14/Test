define(['app/app', 'app/directives/directives', 'app/components/PMS/PMSService', 'moment'], function (app) {
    "use strict";

    app.controller('HoatDongKhacDuyetController', ['$scope', '$rootScope', '$modal', 'PMSService',
        function ($scope, $rootScope, $modal, PMSService) {
            var moment = require('moment');
            $scope.IsLoading = false;
            PMSService.GetNamHoc().then(function (result) {
                $scope.namHocList = result.data;
                if ($scope.namHocList.length > 0) {
                    var namHoc = $scope.namHocList.filter(q => moment().isSameOrAfter(q.NgayBatDau) && moment().isSameOrBefore(q.NgayKetThuc))[0];
                    $scope.namHocId = namHoc ? namHoc.Oid : $scope.namHocList[0].Oid;
                }
                else $scope.namHocId = MANAGER.GUID_EMPTY;
                $scope.getHocKy();
            })
            $scope.getHocKy = function () {
                PMSService.GetHocKy($scope.namHocId).then(function (result) {
                    $scope.hocKyList = result.data;
                    $scope.hocKyList.unshift({
                        Oid: MANAGER.GUID_EMPTY,
                        TenHocKy: 'Cả năm'
                    })
                    if ($scope.hocKyList.length > 0) {
                        var hocKy = $scope.hocKyList.filter(q => moment().isSameOrAfter(q.TuNgay) && moment().isSameOrBefore(q.DenNgay))[0];
                        $scope.hocKyId = hocKy ? hocKy.Oid : MANAGER.GUID_EMPTY;
                    }
                    else $scope.hocKyId = MANAGER.GUID_EMPTY;
                    $scope.loadData();
                    $scope.KiemTraKhoaImport();
                })
            }
            $scope.TrangThaiList = [
                { Id: 0, TenTrangThai: "Chờ xét duyệt" },
                { Id: 1, TenTrangThai: "Đã duyệt" },
                { Id: 2, TenTrangThai: "Không duyệt" }
            ]
            $scope.loadData = function () {
                PMSService.HDKhac_LayDanhSachKeKhai_Duyet($scope.namHocId, $scope.hocKyId, $scope.session.DepartmentId, $scope.session.Id).then(res => {
                    $scope.listHoatDongKhac = res.data;
                    $scope.listHoatDongKhac.forEach((item) => {
                        item.TenTrangThai = $scope.TrangThaiList.filter(q => q.Id == item.TrangThai)[0].TenTrangThai;
                    })
                })
            }
            $scope.duyet = function (item, isDuyet) {
                PMSService.HDKhac_DonVi_Duyet(item.Oid, isDuyet ? 1 : 2, $scope.session.UserName).then(result => {
                    if (result.data != 0) {
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $scope.loadData();
                    }
                    else Notify('Có lỗi!', 'top-right', '3000', 'warning', 'fa-warning', true);
                })
            }
            $scope.duocXacNhan = false;
            $scope.KiemTraKhoaImport = function () {
                PMSService.KiemTraKhoaImport($scope.namHocId, $scope.hocKyId, $rootScope.session.Id).then(function (result) {
                    $scope.duocXacNhan = result.data.KetQua_XacNhan;
                })
            }
        }
    ]);
});