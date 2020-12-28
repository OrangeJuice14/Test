define(['app/app'], function (app) {
    "use strict";
    app.factory('BoTieuChiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetAll = function () {
            return $http.get('/Api/ABC_BoTieuChiApi/GetAll');
        }
        serviceResult.GetById = function (Id) {
            return $http.get('/Api/ABC_BoTieuChiApi/GetById?id='+ Id);
        }
        serviceResult.SaveOrUpdate = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_BoTieuChiApi/PutSaveOrUpdate',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.DeleteById = function (Id, userId) {
            return $http.get('/Api/ABC_BoTieuChiApi/GetDeleteById?id=' + Id + '&userId=' + userId);
        }
        return serviceResult;
    }]);
});