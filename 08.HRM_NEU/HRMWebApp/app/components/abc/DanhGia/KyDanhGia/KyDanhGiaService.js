define(['app/app'], function (app) {
    "use strict";
    app.factory('KyDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.GetListNam = function (nam) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetListNam?nam='+ nam);
        }
        serviceResult.GetListByNam = function (nam) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetByYear?nam=' + nam);
        }
        serviceResult.GetById = function (id) {
            return $http.get('/Api/ABC_KyDanhGiaApi/GetById?id=' + id);
        }
        serviceResult.GetUserGroupDanhGiaRoleByUserId = function (userId) {
            return $http.get('/Api/ABC_UserGroupDanhGiaRoleApi/GetByUserId?userId=' + userId);
        }
        return serviceResult;
    }]);
});