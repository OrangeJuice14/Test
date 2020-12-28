define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_KetQuaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.getLockMe = function (StaffId, KyDanhGiaId) {
            return $http.get('/Api/ABC_KetQuaApi/getLockMe?StaffId=' + StaffId + '&KyDanhGiaId=' + KyDanhGiaId);
        }
        serviceResult.getLockTruongPhong = function (TruongPhongId,StaffId, KyDanhGiaId) {
            return $http.get('/Api/ABC_KetQuaApi/getLockTruongPhong?TruongPhongId=' + TruongPhongId +'&StaffId=' + StaffId + '&KyDanhGiaId=' + KyDanhGiaId);
        }
        serviceResult.getLockDongNghiep = function (DongNghiepId,StaffId, KyDanhGiaId) {
            return $http.get('/Api/ABC_KetQuaApi/getLockDongNghiep?DongNghiepId=' + DongNghiepId +'&StaffId=' + StaffId + '&KyDanhGiaId=' + KyDanhGiaId);
        }

        serviceResult.getKetQuaDanhGia = function (StaffId, KyDanhGiaId) {
            return $http.get('/Api/ABC_KetQuaApi/getKetQuaDanhGia?StaffId=' + StaffId + '&KyDanhGiaId=' + KyDanhGiaId);
        }
        serviceResult.getById = function (Id) {
            return $http.get('/Api/ABC_KetQuaApi/getByIdDTO?Id=' + Id);
        }
        return serviceResult;

    }]);
});