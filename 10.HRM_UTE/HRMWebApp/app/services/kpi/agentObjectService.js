define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('agentObjectService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/agentObjectApi/GetList');
        }

        serviceResult.getListProfessor = function () {
            return $http.get('/Api/agentObjectApi/GetListProfessor');
        }
        serviceResult.getStudyYear = function () {
            return $http.get('/Api/criterionApi/GetStudyYear');
        }
        serviceResult.getObj = function (id) {
            return $http.get('/Api/agentObjectApi/GetClass?id=' +id);
        }

        serviceResult.getListByClassId = function (id) {
            return $http.get('/Api/agentObjectApi/GetListbyId?classId=' +id);
        }
        serviceResult.getListagentObject = function () {
            return $http.get('/Api/agentObjectApi/GetListagentObject');
        }
        serviceResult.getagent_targetGroupObj = function (id) {
            return $http.get('/Api/agentObjectApi/Gettagent_targetGroupObj?Id=' + id);
        }

        serviceResult.Saveagent_targetGroup = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/agentObjectApi/Putagent_targetGroup',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        
        serviceResult.getAgentObjectTypeRateList = function () {
            return $http.get('/Api/agentObjectApi/GetAgentObjectTypeRatingList');
        }

        serviceResult.getAgentObjectTypeRateById = function (id) {
            return $http.get('/Api/agentObjectApi/GetAgentObjectTypeRateById?id=' + id);
        }

        serviceResult.saveAgentObjectTypeRate = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/agentObjectApi/PutAgentObjectTypeRate',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
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

        serviceResult.getWorkingModeListByAgentObject = function (agentObjectId) {
            return $http.get('/Api/agentObjectApi/GetWorkingModeListByAgentObject?agentObjectId=' + agentObjectId);
        };

        serviceResult.getWorkingModeListForAdding = function (agentObjectId) {
            return $http.get('/Api/agentObjectApi/GetWorkingModeListForAdding?agentObjectId=' + agentObjectId);
        };

        serviceResult.SaveWorkingModeDetail = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/agentObjectApi/PutWorkingModeDetail',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

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