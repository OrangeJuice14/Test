define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('scienceResearchDataService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/scienceResearchDataApi/getList');

        };
        serviceResult.getListPosition = function () {
            return $http.get('/Api/scienceResearchDataApi/GetListPosition');

        };

        //serviceResult.getListPaging = function (skip, take,departmentId) {
        //    return $http.get('/Api/scienceResearchDataApi/GetListPaging?skip=' + skip + '&take=' + take + '&departmentId=' + departmentId);

        //};

        serviceResult.getObj = function (id) {
            return $http.get('/Api/scienceResearchDataApi/GetObj?id=' + id);
        };

        serviceResult.getPositionObj = function (id) {
            return $http.get('/Api/scienceResearchDataApi/GetPositionObj?id=' + id);
        };

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/scienceResearchDataApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.SavePosition = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/scienceResearchDataApi/PutPosition',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/scienceResearchDataApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        return serviceResult;
    }]);
});