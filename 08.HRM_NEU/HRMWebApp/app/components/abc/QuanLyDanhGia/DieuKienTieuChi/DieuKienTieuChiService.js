define(['app/app'], function (app) {
    "use strict";
    app.factory('DieuKienTieuChiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetByTieuChiId = function (tieuChiId) {
            return $http.get('/Api/ABC_DieuKienTieuChiApi/GetByTieuChiId?tieuChiId=' + tieuChiId);
        }
        serviceResult.Save = function (list) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_DieuKienTieuChiApi/PutSave',
                data: list
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});