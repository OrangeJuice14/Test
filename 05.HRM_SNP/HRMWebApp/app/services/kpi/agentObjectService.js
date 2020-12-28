define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('agentObjectService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/agentObjectApi/GetList');
        }

        serviceResult.getList = function () {
            return $http.get('/Api/agentObjectApi/GetList');
        }

        serviceResult.getList_KyTinhLuong = function () {
            return $http.get('/Api/agentObjectApi/GetList_KyTinhLuong');
        }
        serviceResult.getList_ThongTinTruong = function () {
            return $http.get('/Api/agentObjectApi/GetList_ThongTinTruong');
        }
        serviceResult.getObj = function (id) {
            return $http.get('/Api/agentObjectApi/GetClass?id=' +id);
        }

        serviceResult.getListByClassId = function (id) {
            return $http.get('/Api/agentObjectApi/GetListbyId?classId=' +id);
        }

        serviceResult.GetUserAgentObjectId = function (id) {
            return $http.get('/Api/agentObjectApi/GetUserAgentObjectId?userId=' + id);
        }
        

        serviceResult.GetListAgentObjectType = function () {
            return $http.get('/Api/agentObjectApi/GetListAgentObjectType');
        };
        serviceResult.getUserAgentObjectTypeId = function () {
            return $http.get('/Api/agentObjectApi/GetUserAgentObjectTypeId');
        };


        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/agentObjectApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/agentObjectApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});