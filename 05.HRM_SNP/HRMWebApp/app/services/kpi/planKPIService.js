define(['app/app'], function (app) {
    "use strict";

   

    app.factory('planKPIService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function() {
            return $http.get('/Api/planKPIApi/GetList');
        };
        serviceResult.GetDepartmentId = function () {
            return $http.get('/Api/planKPIApi/GetDepartmentId');
        };
        serviceResult.getListByUserId = function (id) {
            return $http.get('/Api/planKPIApi/GetListByUserId');
        };
        serviceResult.getListByPlanId = function (id,planid) {
            return $http.get('/Api/planKPIApi/GetListByPlanId?NormalStaffId=' + id + '&PlanId=' + planid);
        };
        serviceResult.getPlanListDepartment = function (id, planId) {
            return $http.get('/Api/planKPIApi/GetPlanListDepartment?normalStaffId=' + id + '&planId=' + planId);
        };
        serviceResult.GetIsSupervisor = function () {
            return $http.get('/Api/planKPIApi/GetIsSupervisor');
        };
        serviceResult.getObj = function(id) {
            return $http.get('/Api/planKPIApi/GetObj?id=' + id);
        };

        serviceResult.getListByAngentObjectId = function (id) {
            return $http.get('/Api/planKPIApi/GetListByAngentObjectId?classId=' + id);
        };
        serviceResult.getParentPlanListById = function (id) {
            return $http.get('/Api/planKPIApi/GetParentPlanListById?id=' + id);
        };
        serviceResult.getListByDepartment = function () {
            return $http.get('/Api/planKPIApi/GetListByDepartment');
        };
        serviceResult.getDateTime = function () {
            return $http.get('/Api/planKPIApi/GetDateTime');
        };
        serviceResult.Save = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIApi/Put',
                data: Obj
            }).success(function(result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        serviceResult.SavePlan = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIApi/PutNewPlan',
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
                url: '/Api/planKPIApi/Delete',
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