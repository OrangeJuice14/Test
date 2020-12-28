define(['app/app'], function (app) {
    "use strict";
    app.factory('TieuChiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetById = function (Id) {
            return $http.get('/Api/ABC_TieuChiApi/GetById?id=' + Id);
        }
        serviceResult.GetByBoTieuId = function (Id) {
            return $http.get('/Api/ABC_TieuChiApi/GetByBoTieuChiId?boTieuChiId=' + Id);
        }
        serviceResult.GetMaxDiem = function (Id, ParentId) {
            return $http.get('/Api/ABC_TieuChiApi/GetMaxDiem?TieuChiId=' + Id + '&ParentId=' + ParentId);
        }
        serviceResult.SaveOrUpdate = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_TieuChiApi/PutSaveOrUpdate',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.DeleteById = function (id,userId) {
            return $http.get('/Api/ABC_TieuChiApi/GetDeleteById?id=' + id + '&userId=' + userId);
        }

        return serviceResult;
    }]);
});