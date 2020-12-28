define(['app/app'], function (app) {
    "use strict";

    app.factory('classificationService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.getListClassificationSet = function () {
            return $http.get('/Api/ABC_CauHinhApi/GetListClassificationSet');
        };

        serviceResult.getClassificationBySet = function (id) {
            return $http.get('/Api/ABC_CauHinhApi/GetClassificationBySet?id=' + id);
        };

        serviceResult.PutClassification = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_CauHinhApi/PutClassification',
                data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        };

        serviceResult.DeleteClassification = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_CauHinhApi/DeleteClassification',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        };

        serviceResult.PutClassificationSet = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_CauHinhApi/PutClassificationSet',
                data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        };

        serviceResult.DeleteClassificationSet = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_CauHinhApi/DeleteClassificationSet',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        };

        return serviceResult;
    }]);
});