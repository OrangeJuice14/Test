define(['app/app'], function (app) {
    "use strict";
    app.factory('UserGroupDanhGiaRoleService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetAllGroupDanhGia = function () {
            return $http.get('/Api/ABC_GroupDanhGiaApi/GetAll');
        }
        serviceResult.GetAllUser = function () {
            return $http.get('/Api/StaffInfoDanhGiaApi/GetAllUser');
        }
        serviceResult.GetUserById = function (userId) {
            return $http.get('/Api/StaffInfoDanhGiaApi/GetUserById?userId=' + userId);
        }
        serviceResult.GetUserGroupDanhGiaRole = function () {
            return $http.get('/Api/ABC_UserGroupDanhGiaRoleApi/GetAll');
        }
        serviceResult.GetUserGroupDanhGiaRoleByUserId = function (userId) {
            return $http.get('/Api/ABC_UserGroupDanhGiaRoleApi/GetByUserId?userId=' + userId);
        }
        serviceResult.GetListBoPhan = function () {
            return $http.get('/Api/ABC_DepartmentApi/GetList');
        }
        serviceResult.SaveOrUpdate = function (obj, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_UserGroupDanhGiaRoleApi/PutSaveOrUpdate?userId='+userId,
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.SaveAdds = function (listUser, groupDanhGiaId, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_UserGroupDanhGiaRoleApi/PutSaveAdds?groupDanhGiaId=' + groupDanhGiaId + '&userId=' + userId,
                data: listUser
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});