define(['app/app'], function (app) {
    "use strict";



    app.factory('webUserService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};
        
        serviceResult.GetListHaveStaffInfoByDepartmentId  = function (departmentId) {
            return $http.get('/Api/WebUserApi/GetListHaveStaffInfoByDepartmentId?departmentId=' + departmentId);
        };
        serviceResult.GetUser = function (userId) {
            return $http.get('/Api/WebUserApi/GetUserDTO?userId=' + userId);
        };
        serviceResult.GetABCWebUserVMDTOByUserId = function (userId) {
            return $http.get('/Api/WebUserApi/GetABCWebUserVMDTOByUserId?userId=' + userId);
        };
        return serviceResult;
    }]);
});