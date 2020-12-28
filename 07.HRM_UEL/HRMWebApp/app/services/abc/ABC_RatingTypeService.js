define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_RatingTypeService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/ABC_RatingTypeApi/GetList');
        }
        serviceResult.getObj = function (id) {
            return $http.get('/Api/ABC_RatingTypeApi/GetObj?id=' + id);
        }
        serviceResult.getStaffRatingType = function (staffId) {
            return $http.get('/Api/ABC_RatingTypeApi/GetStaffRatingType?staffId=' + staffId);
        }
        serviceResult.saveRatingType = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_RatingTypeApi/PutRatingType',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.PutCriterionManage = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_RatingTypeApi/PutCriterionManage',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.DeleteRatingType = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_RatingTypeApi/DeleteRatingType',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.saveStaffRatingType = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_RatingTypeApi/PutStaffRatingType',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});