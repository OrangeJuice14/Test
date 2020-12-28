define(['app/app'], function (app) {
    "use strict";

    app.factory('staffService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.getListAdminLeader = function () {
            return $http.get('/Api/staffApi/GetListAdminLeader');
        };

        serviceResult.getObj = function (id) {
            return $http.get('/Api/staffApi/GetObj?id=' + id);
        };

        serviceResult.SaveDepartmentManage = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/staffApi/PutDepartmentManage',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        return serviceResult;
    }]);
});