define(['app/app'], function (app) {
    "use strict";
    app.factory('GroupDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetAll = function () {
            return $http.get('/Api/ABC_GroupDanhGiaApi/GetAll');
        }

        serviceResult.SaveOrUpdate = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_GroupDanhGiaApi/PutSaveOrUpdate',
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.Delete = function (obj, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_GroupDanhGiaApi/Delete?userId='+userId,
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});