define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_BoTieuChiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetAll = function () {
            return $http.get('/Api/ABC_BoTieuChiApi/GetList');
        }

        serviceResult.GetById = function (Id) {
            return $http.get('/Api/ABC_BoTieuChiApi/GetById?id='+ Id);
        }

        serviceResult.SaveOrUpdate = function (id, obj, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_BoTieuChiApi/Put?id=' + id + '&userId=' + userId,
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.DeleteById = function (id, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_BoTieuChiApi/DeleteById?id=' + id + '&userId=' + userId,
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});