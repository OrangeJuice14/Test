define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('securityService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function() {
            return $http.get('/Api/securityApi/GetList');
        };
        serviceResult.getListAgent = function() {
            return $http.get('/Api/securityApi/GetListAgent');
        };

        serviceResult.getObj = function(id) {
            return $http.get('/Api/securityApi/GetObj?id=' + id);
        };
        serviceResult.getAgentObj = function(id) {
            return $http.get('/Api/securityApi/GetAgentObj?id=' + id);
        };
        serviceResult.getRoleHierarchy = function (agentObjectTypeId) {
            return $http.get('/Api/securityApi/GetRoleHierarchy?agentObjectTypeId=' + agentObjectTypeId);
        };

        serviceResult.Save = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/securityApi/Put',
                data: Obj
            }).success(function(result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        serviceResult.SaveAOTR = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/securityApi/PutAOTR',
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
                url: '/Api/securityApi/Delete',
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