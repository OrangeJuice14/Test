define(['app/app'], function (app) {
    "use strict";

    app.factory('departmentService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function() {
            return $http.get('/Api/departmentApi/GetList');
        };
        serviceResult.getDepartmentTreeList = function (id) {
            return $http.get('/Api/departmentApi/GetDepartmentTreeList');
        };
        serviceResult.getMainDepartment = function (departmentId) {
            return $http.get('/Api/departmentApi/GetMainDepartmentList');
        };
        serviceResult.getMainDepartmentListHierarchy = function (id) {
            //var deferred = $q.defer();
            //$http({
            //    method: 'Post',
            //    url: '/Api/departmentApi/GetMainDepartmentListHierarchy',
            //    data: subDepartmentIds
            //}).success(function (result) {
            //    deferred.resolve(result);
            //});
            //return deferred.promise;


            return $http.get('/Api/departmentApi/GetMainDepartmentListHierarchy?planDetailId=' + id);
        };
        serviceResult.getAdminDepartmentListHierarchy = function (id) {
            return $http.get('/Api/departmentApi/GetAdminDepartmentListHierarchy?adminId=' + id);
        };
        serviceResult.GetListStaffRole = function () {
            return $http.get('/Api/departmentApi/GetListStaffRole');
        };
        serviceResult.getSubjectDepartment = function (departmentId) {
            return $http.get('/Api/departmentApi/GetSubjectDepartment?departmentId=' + departmentId);
        };
        serviceResult.getDepartmentManagedByAdmin = function (adminId) {
            return $http.get('/Api/departmentApi/GetDepartmentManagedByAdmin?adminId=' + adminId);
        };
        serviceResult.GetStaffId = function (DepartmentId, RoleId) {
            return $http.get('/Api/departmentApi/GetStaffId?DepartmentId=' + DepartmentId + '&RoleId=' + RoleId);
        };

        serviceResult.getObj = function(id) {
            return $http.get('/Api/departmentApi/GetClass?id=' + id);
        };

        serviceResult.Save = function(Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/departmentApi/Put',
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
                url: '/Api/departmentApi/Delete',
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