define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_BoDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.saveBoDanhGia = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_BoDanhGiaApi/PutNewBoDanhGia',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.GetListBoDanhGiaByNam = function (Nam) {
            return $http.get('/Api/ABC_BoDanhGiaApi/GetListBoDanhGiaByNam?Nam=' + Nam);
        }
        serviceResult.GetBoDanhGiaById = function (Id) {
            return $http.get('/Api/ABC_BoDanhGiaApi/GetBoDanhGiaById?Id=' + Id);
        }
        serviceResult.getDelete = function (Id) {
            return $http.get('/Api/ABC_BoDanhGiaApi/getDelete?Id=' + Id);
        }
        serviceResult.Update = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_BoDanhGiaApi/PutUpdate',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getBoDanhGiaByTimeNow = function (KyDanhGiaId, LoaiDanhGia) {
            return $http.get('/Api/ABC_BoDanhGiaApi/getBoDanhGiaByTimeNow?KyDanhGiaId=' + KyDanhGiaId + '&LoaiDanhGia=' + LoaiDanhGia);
        };

        return serviceResult;
    }]);
});