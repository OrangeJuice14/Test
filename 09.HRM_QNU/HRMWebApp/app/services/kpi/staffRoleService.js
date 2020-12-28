define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('staffRoleService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function() {
            return $http.get('/Api/staffRoleApi/getList');

        };
        serviceResult.getListPosition = function () {
            return $http.get('/Api/staffRoleApi/GetListPosition');

        };

        //serviceResult.getListPaging = function (skip, take,departmentId) {
        //    return $http.get('/Api/staffRoleApi/GetListPaging?skip=' + skip + '&take=' + take + '&departmentId=' + departmentId);

        //};

        serviceResult.getObj = function(id) {
            return $http.get('/Api/staffRoleApi/GetObj?id=' + id);
        };

        serviceResult.getPositionObj = function (id) {
            return $http.get('/Api/staffRoleApi/GetPositionObj?id=' + id);
        };

        serviceResult.Save = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/staffRoleApi/Put',
                data: Obj
            }).success(function(result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.SavePosition = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/staffRoleApi/PutPosition',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.Delete = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/staffRoleApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function(result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        return serviceResult;
    }]);
});