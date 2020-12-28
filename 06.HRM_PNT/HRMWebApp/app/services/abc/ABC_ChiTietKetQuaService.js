define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_ChiTietKetQuaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.newChiTietKetQua =  function(KetQuaId, DanhGiaId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_ChiTietKetQuaApi/PutNewChiTietKetQua?KetQuaId='+KetQuaId+'&DanhGiaId='+DanhGiaId,
               // data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getListChiTietKetQua = function (KyDanhGiaId, NhanVienDuocDanhGiaId, NhanVienDanhGiaId,LoaiDanhGia) {
            return $http.get('/Api/ABC_ChiTietKetQuaApi/GetListChiTietKetQua?KyDanhGiaId=' + KyDanhGiaId + '&NhanVienDuocDanhGiaId=' + NhanVienDuocDanhGiaId + '&NhanVienDanhGiaId=' + NhanVienDanhGiaId + '&LoaiDanhGia=' + LoaiDanhGia);
        }

        serviceResult.getListByRef = function (ChiTietNhanVienDanhGiaId) {
            return $http.get('/Api/ABC_ChiTietKetQuaApi/getListByRef?ChiTietNhanVienDanhGiaId=' + ChiTietNhanVienDanhGiaId);
        }

        serviceResult.saveChiTietKetQua = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_ChiTietKetQuaApi/PutSaveChiTietKetQua',
                data: Obj
            }).then(function(result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        

        return serviceResult;
    }]);
});