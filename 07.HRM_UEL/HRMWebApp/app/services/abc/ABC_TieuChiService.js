define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_TieuChiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetAll = function () {
            return $http.get('/Api/ABC_TieuChiApi/GetListDTO');
        }

        serviceResult.GetById = function (Id) {
            return $http.get('/Api/ABC_TieuChiApi/GetDTOById?id='+ Id);
        }

        serviceResult.GetByBoTieuChiId = function (boTieuChiId) {
            return $http.get('/Api/ABC_TieuChiApi/GetDTOByBoTieuChiId?boTieuChiId=' + boTieuChiId);
        }

        serviceResult.GetByParentId = function (parentId) {
            return $http.get('/Api/ABC_TieuChiApi/GetDTOByParentId?parentId=' + parentId);
        }

        serviceResult.SaveOrUpdate = function (id, obj, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_TieuChiApi/Put?id=' + id + '&userId=' + userId,
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
                url: '/Api/ABC_TieuChiApi/DeleteById?id=' + id + '&userId=' + userId,
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});