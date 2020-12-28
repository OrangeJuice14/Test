define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_LoaiDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.getByMaLoai = function (MaLoai) {
            return $http.get('/Api/ABC_LoaiDanhGiaApi/getLoaiDanhGia?MaLoai=' + MaLoai);
        }

        return serviceResult;
    }]);
});