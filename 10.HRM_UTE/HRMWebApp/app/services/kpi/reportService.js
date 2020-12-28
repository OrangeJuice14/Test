define(['app/app'], function (app) {
    "use strict";

    app.factory('reportService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getReportDepartmentStaffResult = function (planId, departmentId) {
            return $http.get('/Api/reportApi/GetReportDepartmentStaffResult?planId=' + planId + "&departmentId=" + departmentId);
        }
        serviceResult.getReportTotalDepartmentResult = function (planId, departmentId) {
            return $http.get('/Api/reportApi/GetReportTotalDepartmentResult?planId=' + planId);
        }
        serviceResult.getListPlanKPIDetailByDepartment = function (planId, departmentId) {
            return $http.get('/Api/reportApi/GetListPlanKPIDetailByDepartment?planId=' + planId + "&departmentId=" + departmentId);
        }

        return serviceResult;

    }]);
});