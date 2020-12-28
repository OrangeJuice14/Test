define(['app/app'], function (app) {
    "use strict";
    app.factory('ABC_RatingDetailService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getRatingDetail = function (evaluationId, staffId, supervisorId, departmentId, isAdminRating) {
            return $http.get('/Api/ABC_RatingDetailApi/GetRatingDetail?evaluationId=' + evaluationId + '&staffId=' + staffId + "&supervisorId=" + supervisorId + "&departmentId=" + departmentId + "&isAdminRating=" + isAdminRating);
        }
        serviceResult.getRating = function (Id) {
            return $http.get('/Api/ABC_RatingDetailApi/GetRating?Id=' + Id);
        }
        serviceResult.getCreateNewABCRating = function (staffId, evaluationId) {
            return $http.get('/Api/ABC_RatingDetailApi/GetCreateNewABCRating?staffId=' + staffId + '&evaluationId=' + evaluationId);
        }
        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_RatingDetailApi/Put',
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
                url: '/Api/ABC_RatingDetailApi/PutLock',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.SaveRating = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_RatingDetailApi/PutRating',
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