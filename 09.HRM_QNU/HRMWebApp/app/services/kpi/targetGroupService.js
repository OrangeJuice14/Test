define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('targetGroupService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/targetGroupApi');
        }

        serviceResult.getListByClassId = function (id) {
            return $http.get('/Api/targetGroupApi?classId='+id);

              
        }

        serviceResult.getObj = function (id) {
            return $http.get('/Api/targetGroupApi/' + id);
        }

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/targetGroupApi',
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
                url: '/Api/targetGroupApi',
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