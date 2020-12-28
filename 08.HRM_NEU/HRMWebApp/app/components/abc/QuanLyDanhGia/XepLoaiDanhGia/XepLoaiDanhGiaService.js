define(['app/app'], function (app) {
    "use strict";
    app.factory('XepLoaiDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetListXepLoaiDanhGiaByBoTieuChiId = function (boTieuChiId) {
            return $http.get('/Api/ABC_XepLoaiDanhGiaApi/GetListByBoTieuChiId?boTieuChiId=' + boTieuChiId);
        }
        serviceResult.GetByXepLoaiDanhGiaId = function (xepLoaiDanhGiaId) {
            return $http.get('/Api/ABC_XepLoaiDanhGiaApi/GetByXepLoaiDanhGiaId?xepLoaiDanhGiaId=' + xepLoaiDanhGiaId);
        }

        serviceResult.GetDieuKienXepLoaiPhuByXepLoaiDanhGiaId = function (xepLoaiDanhGiaId) {
            return $http.get('/Api/ABC_DieuKienXepLoaiPhuApi/GetByXepLoaiDanhGiaId?xepLoaiDanhGiaId=' + xepLoaiDanhGiaId);
        }

        serviceResult.GetListDieuKienXepLoaiPhuByBoTieuChiId = function (boTieuChiId) {
            return $http.get('/Api/ABC_DieuKienXepLoaiPhuApi/GetListDieuKienXepLoaiPhuByBoTieuChiId?boTieuChiId=' + boTieuChiId);
        }

        serviceResult.SaveOrUpdateXepLoaiDanhGia = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_XepLoaiDanhGiaApi/PutSaveOrUpdate',
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.SaveOrUpdateDieuKienXepLoai = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_DieuKienXepLoaiPhuApi/PutSaveOrUpdate',
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.Delete = function (xepLoaiDanhGiaId, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_XepLoaiDanhGiaApi/Delete?userId=' + userId + '&xepLoaiDanhGiaId=' + xepLoaiDanhGiaId,
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                }
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});