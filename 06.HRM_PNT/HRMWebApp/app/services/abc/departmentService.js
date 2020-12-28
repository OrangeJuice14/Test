define(['app/app'], function (app) {
    "use strict";

    app.factory('departmentService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.getDepartmentManagedByAdmin = function (adminId) {
            return $http.get('/Api/departmentApi/GetDepartmentManagedByAdmin?adminId=' + adminId);
        };

        serviceResult.getList = function () {
            return $http.get('/Api/departmentApi/GetList');
        };

        serviceResult.getAdminDepartmentListHierarchy = function (id) {
            return $http.get('/Api/departmentApi/GetAdminDepartmentListHierarchy?adminId=' + id);
        };

        return serviceResult;
    }]);
});