define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_KetQuaXepLoaiService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.New = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_KetQuaXepLoaiApi/PutNew',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.GetListByDanhGiaId = function (DanhGiaId) {
            return $http.get('/Api/ABC_KetQuaXepLoaiApi/getListDTOByDanhGiaId?DanhGiaId=' + DanhGiaId);
        }
        serviceResult.Delete = function (Id) {
            return $http.get('/Api/ABC_KetQuaXepLoaiApi/getDelete?IdDelete=' + Id);
        }
        return serviceResult;
    }]);
});