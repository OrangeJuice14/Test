define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_DanhGiaDongNghiepService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetListUserDanhGia = function (boTieuChiId) {
            return $http.get('/Api/ABC_UserGroupDanhGiaRoleApi/GetListUserDanhGia?boTieuChiId=' + boTieuChiId);
        }
        //serviceResult.GetUserDanhGiaByUserId = function (userId) {
        //    return $http.get('/Api/ABC_UserGroupDanhGiaRoleApi/GetUserDanhGiaByUserId?userId=' + userId);
        //}
        serviceResult.GetDanhGiaByFK = function (userDuocDanhGiaId, userDanhGiaId, kyDanhGiaId, boTieuChiId, groupDanhGiaId) {
            return $http.get('/Api/ABC_DanhGiaApi/GetDanhGiaByFK?userDuocDanhGiaId=' + userDuocDanhGiaId + '&userDanhGiaId=' + userDanhGiaId + '&kyDanhGiaId=' + kyDanhGiaId + '&boTieuChiId=' + boTieuChiId + '&groupDanhGiaId=' + groupDanhGiaId);
        }
        serviceResult.GetListChiTietDanhGiaByDanhGiaId = function (danhGiaId) {
            return $http.get('/Api/ABC_DanhGiaChiTietApi/GetListByDanhGiaId?danhGiaId=' + danhGiaId);
        }
        serviceResult.GetListKetQuaDanhGia = function (danhGiaId) {
            return $http.get('/Api/ABC_DanhGiaApi/GetListKetQuaDanhGia?danhGiaId=' + danhGiaId);
        }
        serviceResult.GetCapBacChuaDanhGia = function (danhGiaId) {
            return $http.get('/Api/ABC_DanhGiaApi/GetCapBacChuaDanhGia?danhGiaId=' + danhGiaId);
        }
        serviceResult.SaveDanhGiaChiTiet = function (list) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_DanhGiaChiTietApi/PutSave',
                data: list
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.SaveDanhGia = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_DanhGiaApi/PutLuuUpdateDanhGia',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.PutSaveOrUpdateDanhGia = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_DanhGiaApi/PutSaveOrUpdate',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});