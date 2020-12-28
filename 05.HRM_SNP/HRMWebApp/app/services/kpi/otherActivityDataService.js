define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('otherActivityDataService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/otherActivityDataApi/getList');
        };
        serviceResult.getListProfessorCriterion = function () {
            return $http.get('/Api/otherActivityDataApi/GetListProfessorCriterion');

        };
        serviceResult.getListPosition = function () {
            return $http.get('/Api/otherActivityDataApi/GetListPosition');

        };
        serviceResult.getListStudyYear = function () {
            return $http.get('/Api/otherActivityDataApi/GetListStudyYear');
        };
        serviceResult.getListDictionaryByManageCode = function (manageCode) {
            return $http.get('/Api/otherActivityDataApi/GetListDictionaryByManageCode?manageCode='+manageCode);
        };

        serviceResult.getListByStaffId = function (staffId) {
            return $http.get('/Api/otherActivityDataApi/GetListByStaffId?staffId='+staffId);
        };
        //serviceResult.getListPaging = function (skip, take,departmentId) {
        //    return $http.get('/Api/otherActivityDataApi/GetListPaging?skip=' + skip + '&take=' + take + '&departmentId=' + departmentId);

        //};

        serviceResult.getObj = function (id) {
            return $http.get('/Api/otherActivityDataApi/GetObj?id=' + id);
        };

        serviceResult.getPositionObj = function (id) {
            return $http.get('/Api/otherActivityDataApi/GetPositionObj?id=' + id);
        };

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/otherActivityDataApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.SavePosition = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/otherActivityDataApi/PutPosition',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/otherActivityDataApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        return serviceResult;
    }]);
});