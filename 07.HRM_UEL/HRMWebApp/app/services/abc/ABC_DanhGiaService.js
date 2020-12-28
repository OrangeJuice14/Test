define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_DanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetDanhGiaByFK = function (userDuocDanhGiaId, userDanhGiaId, kyDanhGiaId, boTieuChiId, groupDanhGiaId) {
            return $http.get('/Api/ABC_DanhGiaApi/GetDanhGiaByFK?userDuocDanhGiaId=' + userDuocDanhGiaId + '&userDanhGiaId=' + userDanhGiaId + '&kyDanhGiaId=' + kyDanhGiaId + '&boTieuChiId=' + boTieuChiId + '&groupDanhGiaId=' + groupDanhGiaId);
        }
        serviceResult.GetListKetQuaDanhGia = function (danhGiaId) {
            return $http.get('/Api/ABC_DanhGiaApi/GetListKetQuaDanhGia?danhGiaId=' + danhGiaId);
        }
        //serviceResult.CheckIsTeacher = function (userId, kyDanhGiaId, groupDanhGiaId) {
        //    return $http.get('/Api/ABC_UserDanhGiaApi/GetCheckIsTeacher?userId=' + userId + '&kyDanhGiaId=' + kyDanhGiaId + '&groupDanhGiaId=' + groupDanhGiaId);
        //}
        //serviceResult.SaveDanhGiaChiTiet = function (list) {
        //    var deferred = $q.defer();
        //    $http({
        //        method: 'Put',
        //        url: '/Api/ABC_DanhGiaChiTietApi/PutSave',
        //        data: list
        //    }).then(function (result) {
        //        deferred.resolve(result);
        //    });
        //    return deferred.promise;
        //}
        serviceResult.SaveDanhGia = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_DanhGiaApi/Put',
                data: obj
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});