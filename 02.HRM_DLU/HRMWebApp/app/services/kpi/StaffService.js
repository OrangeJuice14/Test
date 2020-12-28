define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('staffService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function() {
            return $http.get('/Api/staffApi/getList');

        };

        serviceResult.getListPaging = function (skip, take,departmentId) {
            return $http.get('/Api/staffApi/GetListPaging?skip=' + skip + '&take=' + take + '&departmentId=' + departmentId);

        };
        serviceResult.getListAdminLeader = function () {
            return $http.get('/Api/staffApi/GetListAdminLeader');

        };
        serviceResult.getListAll = function (skip, take) {
            return $http.get('/Api/staffApi/GetListAll');

        };
        serviceResult.getCurrentStaff = function () {
            return $http.get('/Api/staffApi/GetCurrentStaff');
        };

        serviceResult.search = function (skip, take, departmentId,staffName) {
            return $http.get('/Api/staffApi/GetSearch?skip=' + skip + '&take=' + take + '&departmentId=' + departmentId+'&staffName='+staffName);

        };
        serviceResult.searchDept = function (deptId) {
            return $http.get('/Api/staffApi/GetSearchDept?deptId=' + deptId);

        };
        serviceResult.searchDeptOnlyProfessor= function (deptId) {
            return $http.get('/Api/staffApi/GetSearchDeptOnlyProfessor?deptId=' + deptId);
        };

        serviceResult.getAgentObjectTypeIdByStaffId = function (id) {
            return $http.get('/Api/staffApi/GetAgentObjectTypeIdByStaffId?id=' + id);
        };

        serviceResult.getAgentObjectIdByStaffId = function (id) {
            return $http.get('/Api/staffApi/GetAgentObjectIdByStaffId?id=' + id);
        };

        serviceResult.getStaffByAgentObjectType = function (typeId, departmentId, userRole) {
            return $http.get('/Api/staffApi/GetStaffByAgentObjectType?typeId=' + typeId + '&departmentId=' + departmentId + '&userRole=' + userRole);

        };

        
        serviceResult.getDepartmentLeader = function (departmentId) {
            return $http.get('/Api/staffApi/GetDepartmentLeader?departmentId=' + departmentId);
        };
        serviceResult.getDepartmentStaff = function (agentObjectTypeId, planId) {
            return $http.get('/Api/staffApi/GetDepartmentStaff?agentObjectTypeId=' + agentObjectTypeId +'&planId='+planId);
        };
        serviceResult.getDepartmentLeaderAgentObjectId = function (departmentId) {
            return $http.get('/Api/staffApi/GetDepartmentLeaderAgentObjectId?departmentId=' + departmentId);
        };
        serviceResult.getDepartmentLeaderId = function (departmentId) {
            return $http.get('/Api/staffApi/GetDepartmentLeaderId?departmentId=' + departmentId);
        };
        serviceResult.getViceDepartmentStaff = function (typeId, departmentId,planId) {
            return $http.get('/Api/staffApi/GetViceDepartmentStaff?typeId=' + typeId + '&departmentId=' + departmentId + '&planId=' + planId);
        };
        serviceResult.getProfessorInSubject = function (typeId) {
            return $http.get('/Api/staffApi/GetProfessorInSubject?typeId=' + typeId);
        };
        serviceResult.getAutoComplete = function (departmentId) {
            return $http.get('/Api/staffApi/GetAutoComplete?departmentId=' + departmentId);

        };
        serviceResult.getListByDepartmentId = function (id) {

            return $http.get('/Api/staffApi/GetListbyId?departmentId=' + id);
        };

        serviceResult.getObj = function(id) {
            return $http.get('/Api/staffApi/GetObj?id=' + id);
        };

        serviceResult.getCurrentUserGroupId = function () {
            return $http.get('/Api/staffApi/GetCurrentUserGroupId');
        };

        serviceResult.Save = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/staffApi/Put',
                data: Obj
            }).success(function(result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        serviceResult.SaveDepartmentManage = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/staffApi/PutDepartmentManage',
                data: Obj
            }).success(function(result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        serviceResult.Delete = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/staffApi/Delete',
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