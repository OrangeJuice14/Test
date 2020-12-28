define(['app/app'], function (app) {
    "use strict";

    app.factory('ratingKPIService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/resultDetailApi/GetList');
        }

        serviceResult.getObj = function (id) {
            return $http.get('/Api/resultDetailApi/GetObject?id=' + id);
        }

        serviceResult.getCheckUnlockable = function (planId) {
            return $http.get('/Api/resultDetailApi/GetCheckUnlockable?planId=' + planId);
        }

        serviceResult.getResult = function (id) {
            return $http.get('/Api/resultDetailApi/GetResult?id=' + id);
        }
        serviceResult.getRatingResultDetail = function (id, agentObjectId, planStaffId, supervisorId, departmentId,isAdminRating) {
            return $http.get('/Api/resultDetailApi/GetRatingResultDetail?planId=' + id + '&agentObjectId=' + agentObjectId + "&planStaffId=" + planStaffId + "&supervisorId=" + supervisorId + "&departmentId=" + departmentId + "&isAdminRating=" + isAdminRating);
        }

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/resultDetailApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.Lock = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/resultDetailApi/PutLock',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.saveEditRecord = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/resultDetailApi/PutEditRecord',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.getUnlock = function (id) {
            return $http.get('/Api/resultDetailApi/GetUnlock?id=' + id);
        }

        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/resultDetailApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.getListByStaffId = function (id) {
            return $http.get('/Api/resultDetailApi/GetListByStaffId?id=' + id);
        }
        serviceResult.getPlanIdByResultId = function (id) {
            return $http.get('/Api/resultDetailApi/GetPlanIdByResultId?id=' + id);
        }
        serviceResult.getStaffIdByResultId = function (id) {
            return $http.get('/Api/resultDetailApi/GetStaffIdByResultId?id=' + id);
        }
        serviceResult.SaveUnlockRating = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/resultDetailApi/PutUnlockRating',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});