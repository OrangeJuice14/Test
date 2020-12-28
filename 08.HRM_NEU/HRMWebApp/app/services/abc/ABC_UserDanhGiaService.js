define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_UserDanhGiaService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.GetUserWithGroupDanhGiaId = function (userId, kyDanhGiaId, groupDanhGiaId) {
            return $http.get('/Api/ABC_UserDanhGiaApi/GetUserByKyDanhGiaId?userId=' + userId + '&kyDanhGiaId=' + kyDanhGiaId + '&groupDanhGiaId=' + groupDanhGiaId);
        }

        serviceResult.GetUserById = function (userId, kyDanhGiaId) {
            return $http.get('/Api/ABC_UserDanhGiaApi/GetUserByKyDanhGiaId?userId=' + userId + '&kyDanhGiaId=' + kyDanhGiaId);
        }

        serviceResult.GetUserNow = function (userId) {
            return $http.get('/Api/ABC_UserDanhGiaApi/GetUserNow?userId=' + userId);
        }

        serviceResult.GetListUserDanhGiaInDonVi = function (boTieuChiId, kyDanhGiaId, userId, groupDanhGiaId, donViId) {
            return $http.get('/Api/ABC_UserDanhGiaApi/GetListUserDanhGia?boTieuChiId=' + boTieuChiId + '&kyDanhGiaId=' + kyDanhGiaId + '&userId=' + userId + '&groupDanhGiaId= ' + groupDanhGiaId + '&donViId=' + donViId);
        }

        serviceResult.GetABCUserById = function (ABC_UserId) {
            return $http.get('/Api/ABC_UserDanhGiaApi/GetById?ABC_UserId=' + ABC_UserId);
        }

        serviceResult.CheckChotTTTK = function (kyDanhGiaId) {
            return $http.get('/Api/ABC_UserDanhGiaApi/GetCheckChotTTTK?kyDanhGiaId=' + kyDanhGiaId);
        }

        serviceResult.ChotTTTK = function (kyDanhGiaId, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_UserDanhGiaApi/PutChotTTTKTheoKyDanhGiaId?kyDanhGiaId=' + kyDanhGiaId + '&userId=' + userId
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});