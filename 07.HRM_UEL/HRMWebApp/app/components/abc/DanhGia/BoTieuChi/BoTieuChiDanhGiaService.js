//define(['app/app'], function (app) {
//    "use strict";
//    app.factory('BoTieuChiDanhGiaService', ['$http', '$q', function ($http, $q) {
//        var serviceResult = {};
//        serviceResult.GetAll = function () {
//            return $http.get('/Api/ABC_BoTieuChiApi/GetAll');
//        }
//        serviceResult.GetById = function (Id) {
//            return $http.get('/Api/ABC_BoTieuChiApi/GetById?id='+ Id);
//        }
//        serviceResult.GetBoTieuChiTuDanhGia = function (userId, groupDanhGiaId,kyDanhGiaId) {
//            return $http.get('/Api/ABC_BoTieuChiApi/GetBoTieuChiTuDanhGia?userId=' + userId + '&groupDanhGiaId=' + groupDanhGiaId + '&kyDanhGiaId=' + kyDanhGiaId);
//        }
//        serviceResult.GetBoTieuChiDanhGiaDongNghiep = function (userId, groupDanhGiaId, kyDanhGiaId) {
//            return $http.get('/Api/ABC_BoTieuChiApi/GetBoTieuChiDanhGiaDongNghiep?userId=' + userId + '&groupDanhGiaId=' + groupDanhGiaId + '&kyDanhGiaId=' + kyDanhGiaId);
//        }
//        //serviceResult.GetUserNow = function (userId, kyDanhGiaId) {
//        //    return $http.get('/Api/ABC_UserDanhGiaApi/GetUserNow?userId=' + userId + '&kyDanhGiaId=' + kyDanhGiaId);
//        //}
//        return serviceResult;
//    }]);
//});