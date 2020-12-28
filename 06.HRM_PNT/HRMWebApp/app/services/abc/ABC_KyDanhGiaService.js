define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_KyDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.getList = function () {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetList');
        }
        serviceResult.saveKyDanhGia = function (nam) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetTaoKyDanhGiaNam?nam=' + nam);

        }
        serviceResult.getListKyDanhGia = function (nam) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetListKyDanhGia?nam=' +nam);
        }
        serviceResult.getKyDanhGiaById = function (id) {
            return $http.get('/Api/ABC_KyDanhGiaApi/getKyDanhGiaDTOById?id=' + id);
        }
        //serviceResult.saveKyDanhGia = function (Nam) {
        //    var deferred = $q.defer();
        //    $http({
        //        method: 'Put',
        //        url: '/Api/ABC_KyDanhGiaApi/PutKyDanhGia',
        //        data: Nam
        //    }).then(function (result) {
        //        deferred.resolve(result);
        //    }, function (error) {
        //        deferred.reject(error);
        //    });
        //    return deferred.promise;
        //}

        serviceResult.GetNam = function () {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetNam');
        }

        return serviceResult;
    }]);
});