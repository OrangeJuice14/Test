define(['app/app'], function (app) {
    "use strict";
    app.factory('QuanLyKyDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetKyDanhGiaByNam = function (nam) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetByYear?nam=' + nam);
        }

        serviceResult.NewKyDanhGia = function (nam, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_KyDanhGiaApi/PutNew?nam=' + nam + '&userId=' + userId
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.SaveOrUpdate = function (obj, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_KyDanhGiaApi/PutSaveOrUpdate?userId=' + userId,
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});