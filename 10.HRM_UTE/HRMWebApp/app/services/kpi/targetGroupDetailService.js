define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('targetGroupDetailService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/targetGroupDetailApi/getList');
        }
        //serviceResult.getListTarget = function () {
        //    return $http.get('/Api/targetGroupDetailApi/GetListTarget');
        //}
        
        serviceResult.getListByClassId = function (id) {
            return $http.get('/Api/targetGroupDetailApi/GetListbyId?classId=' + id);
        }
        
        serviceResult.GetListbyAgentObjectId = function (id) {
            return $http.get('/Api/targetGroupDetailApi/GetListbyAgentObjectId?agentObjectId=' + id);
        }
        
        serviceResult.GetListbyAgentObject = function () {
            return $http.get('/Api/targetGroupDetailApi/GetListbyAgentObject');
        }

        serviceResult.GetListbyGV = function () {
            return $http.get('/Api/targetGroupDetailApi/GetListbyGV');
        }

        serviceResult.getObj = function (id) {
            return $http.get('/Api/targetGroupDetailApi/GetObj?id=' + id);
        }
        serviceResult.getTargetGroupDetailNameByCriterion = function (id) {
            return $http.get('/Api/targetGroupDetailApi/GetTargetGroupDetailNameByCriterion?id=' + id);
        }
        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/targetGroupDetailApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getTargetGroupDetailTypeId = function (id) {
            return $http.get('/Api/targetGroupDetailApi/GetTargetGroupDetailTypeId?id=' + id);
        }

        serviceResult.getListTargetGroupDetailType = function () {
            return $http.get('/Api/targetGroupDetailApi/GetListTargetGroupDetailType');
        }

        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/targetGroupDetailApi/Delete',
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