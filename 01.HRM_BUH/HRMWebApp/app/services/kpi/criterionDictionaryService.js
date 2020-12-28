define(['app/app'], function (app) {
    "use strict";
    app.factory('criterionDictionaryService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/criterionContentDictionaryApi/GetList');
        }

        serviceResult.getListByClassId = function (id) {
            return $http.get('/Api/criterionContentDictionaryApi/GetListbyId?classId=' + id);
        }

        serviceResult.getListUser = function (userId) {
            return $http.get('/Api/criterionContentDictionaryApi/GetListbyId?classId=' + id);
        }

        serviceResult.Search = function (targetGroupDetailId, departmentId) {
            return $http.get('/Api/criterionContentDictionaryApi/GetSearch?targetGroupDetailId=' + targetGroupDetailId + '&departmentId=' + departmentId);
        }

        serviceResult.getObj = function (id) {
            return $http.get('/Api/criterionContentDictionaryApi/GetListByClass?id=' + id);
        }

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/criterionContentDictionaryApi/Put',
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
                url: '/Api/criterionContentDictionaryApi/Delete',
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