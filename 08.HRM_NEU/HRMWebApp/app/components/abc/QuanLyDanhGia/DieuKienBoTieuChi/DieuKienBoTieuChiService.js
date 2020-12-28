define(['app/app'], function (app) {
    "use strict";
    app.factory('DieuKienBoTieuChiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetByBoTieuChiId = function (boTieuChiId) {
            return $http.get('/Api/ABC_DieuKienBoTieuChiApi/GetByBoTieuChiId?boTieuChiId=' + boTieuChiId);
        }
        serviceResult.GetCheckDieuKienBoTieuChi = function (boTieuChiId, kyDanhGiaId, aBC_UserId, groupDanhGiaId) {
            return $http.get('/Api/ABC_DieuKienBoTieuChiApi/GetCheckDieuKienBoTieuChi?boTieuChiId=' + boTieuChiId + '&kyDanhGiaId=' + kyDanhGiaId + '&aBC_UserId=' + aBC_UserId + '&groupDanhGiaId=' + groupDanhGiaId);
        }
        serviceResult.Save = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_DieuKienBoTieuChiApi/PutSave',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});