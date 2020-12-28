define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_TieuChiDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.saveTieuChiDanhGia = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_TieuChiDanhGiaApi/PutTieuChiDanhGia',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        
        serviceResult.update = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_TieuChiDanhGiaApi/PutUpdate',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.delete = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_TieuChiDanhGiaApi/PutDelete',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.GetTieuChiDanhGiaById = function (BoDanhGiaId) {
            return $http.get('/Api/ABC_TieuChiDanhGiaApi/GetTieuChiDanhGiaById?BoDanhGiaId=' + BoDanhGiaId);
        }

        return serviceResult;
    }]);
});