define(['app/app'], function (app) {
    "use strict";
    app.factory('BoTieuChiRoleService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetByBoTieuId = function (boTieuChiId) {
            return $http.get('/Api/ABC_BoTieuChiRoleApi/GetByBoTieuId?boTieuChiId=' + boTieuChiId);
        }
        serviceResult.GetByBoTieuIdAndGroupDanhGiaId = function (boTieuChiId, groupDanhGiaId) {
            return $http.get('/Api/ABC_BoTieuChiRoleApi/GetByBoTieuIdAndGroupDanhGiaId?boTieuChiId=' + boTieuChiId + '&groupDanhGiaId=' + groupDanhGiaId);
        }

        serviceResult.SaveOrUpdate = function (listObj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_BoTieuChiRoleApi/PutSaveOrUpdate',
                data: listObj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});