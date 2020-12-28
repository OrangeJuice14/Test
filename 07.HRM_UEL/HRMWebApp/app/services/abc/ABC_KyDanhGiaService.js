define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_KyDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetListByYear = function (nam) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetListByYear?year=' + nam);
        }
        serviceResult.GetListYear = function () {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetListYear');
        }
        serviceResult.GetList = function () {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetListDTO');
        }
        serviceResult.GetById = function (id) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetDTOById?id=' + id);
        }

        serviceResult.NewKyDanhGia = function (nam, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Post',
                url: '/Api/ABC_KyDanhGiaApi/PostByYear?nam=' + nam + '&userId=' + userId
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.Update = function (id, obj, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_KyDanhGiaApi/Put?userId=' + userId + '&id=' + id,
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});