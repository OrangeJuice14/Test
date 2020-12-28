define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_ChiTietNhanVienDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.PutUpdate = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_ChiTietNhanVienDanhGiaApi/PutUpdateDTO',
                data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getById = function(Id){
            return $http.get('/Api/ABC_ChiTietNhanVienDanhGiaApi/getById?Id=' + Id);
        }

        serviceResult.getByRef = function (NhanVienDanhGiaId,KetQuaId, LoaiDanhGiaId){
            return $http.get('/Api/ABC_ChiTietNhanVienDanhGiaApi/getByRef?KetQuaId=' + KetQuaId + '&NhanVienDanhGiaId=' + NhanVienDanhGiaId + '&LoaiDanhGiaId=' + LoaiDanhGiaId);
        }
        serviceResult.getWithTruongPhong = function (KetQuaId) {
            return $http.get('/Api/ABC_ChiTietNhanVienDanhGiaApi/getWithTruongPhong?KetQuaId=' + KetQuaId);
        }

        return serviceResult;
    }]);
});