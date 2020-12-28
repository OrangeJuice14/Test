define(['app/app'], function (app) {
    "use strict";
    app.factory('ListDongNghiepService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetListUserDanhGia = function (boTieuChiId, kyDanhGiaId, userId, groupDanhGiaId) {
            return $http.get('/Api/ABC_UserDanhGiaApi/GetListUserDanhGia?boTieuChiId=' + boTieuChiId + '&kyDanhGiaId=' + kyDanhGiaId + '&userId=' + userId + '&groupDanhGiaId= '+ groupDanhGiaId);
        }
        //serviceResult.GetUserNow = function (userId) {
        //    return $http.get('/Api/ABC_UserDanhGiaApi/GetUserNow?userId=' + userId);
        //}
        return serviceResult;
    }]);
});