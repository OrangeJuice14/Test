define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_RoleBoTieuChiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetByBoTieuId = function (boTieuChiId) {
            return $http.get('/Api/ABC_RoleBoTieuChiApi/GetByBoTieuId?boTieuChiId=' + boTieuChiId);
        }

        serviceResult.GetByBoTieuIdAndGroupDanhGiaId = function (boTieuChiId, groupDanhGiaId) {
            return $http.get('/Api/ABC_RoleBoTieuChiApi/GetByBoTieuIdAndGroupDanhGiaId?boTieuChiId=' + boTieuChiId + '&groupDanhGiaId=' + groupDanhGiaId);
        }

        serviceResult.SaveOrUpdate = function (listObj, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_RoleBoTieuChiApi/Put?userId=' + userId,
                data: listObj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});