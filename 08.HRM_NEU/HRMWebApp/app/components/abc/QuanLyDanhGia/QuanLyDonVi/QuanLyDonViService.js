define(['app/app'], function (app) {
    "use strict";
    app.factory('QuanLyDonViService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetListUserQuanLyDonVi = function () {
            return $http.get('/Api/ABC_QuanLyDonViApi/GetListUserQuanLyDonVi');
        }

        serviceResult.GetListQuanLyDonViByUserId = function (userId) {
            return $http.get('/Api/ABC_QuanLyDonViApi/GetListQuanLyDonViByUserId?userId=' + userId);
        }

        serviceResult.GetListQuanLyDonViIdByUserId = function (userId) {
            return $http.get('/Api/ABC_QuanLyDonViApi/GetListQuanLyDonViIdByUserId?userId=' + userId);
        }
        serviceResult.GetUserById = function (userId) {
            return $http.get('/Api/StaffInfoDanhGiaApi/GetUserById?userId=' + userId);
        }
        serviceResult.GetListTreeBoPhan = function (userId) {
            return $http.get('/Api/ABC_DepartmentApi/GetListTree?userId=' + userId);
        }
        serviceResult.GetListBoPhan = function () {
            return $http.get('/Api/ABC_DepartmentApi/GetList');
        }
        serviceResult.CheckQLDV = function (userId, departmentId) {
            return $http.get('/Api/ABC_QuanLyDonViApi/GetCheckQLDV?userId=' + userId + '&departmentId=' + departmentId);
        }
        serviceResult.Save = function (listSelected,userId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_QuanLyDonViApi/PutSave?userId=' + userId,
                data: listSelected
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.Delete = function (obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_GroupDanhGiaApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});