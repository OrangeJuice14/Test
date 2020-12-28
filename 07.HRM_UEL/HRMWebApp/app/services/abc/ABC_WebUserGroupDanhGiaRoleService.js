define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_WebUserGroupDanhGiaRoleService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetUserGroupDanhGiaRole = function () {
            return $http.get('/Api/ABC_WebUserGroupDanhGiaRoleApi/GetAll');
        }

        serviceResult.GetUserGroupDanhGiaRoleByUserId = function (userId) {
            return $http.get('/Api/ABC_WebUserGroupDanhGiaRoleApi/GetByUserId?userId=' + userId);
        }
        serviceResult.GetListUserQuanLyDonVi = function () {
            return $http.get('/Api/ABC_WebUserGroupDanhGiaRoleApi/GetListUserQuanLyDonVi');
        }

        serviceResult.SaveOrUpdate = function (list, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_WebUserGroupDanhGiaRoleApi/Put?userId=' + userId,
                data: list
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.SaveAdds = function (listUser, groupDanhGiaId, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_WebUserGroupDanhGiaRoleApi/Puts?groupDanhGiaId=' + groupDanhGiaId + '&userId=' + userId,
                data: listUser
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});