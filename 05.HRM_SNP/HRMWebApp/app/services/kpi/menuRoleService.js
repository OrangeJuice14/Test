define(['app/app'], function (app) {
    "use strict";

   

    app.factory('menuRoleService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};
        //WebMenu
        serviceResult.getObject = function (id) {
            return $http.get('/Api/menuRoleApi/GetObject?id='+id);
        };
        serviceResult.getWebMenuTreeList = function () {
            return $http.get('/Api/menuRoleApi/GetWebMenuTreeList');
        };
        serviceResult.getListParentMenu = function () {
            return $http.get('/Api/menuRoleApi/GetListParentMenu');
        };
        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/menuRoleApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        //WebGroup
        serviceResult.getWebGroup = function (id) {
            return $http.get('/Api/menuRoleApi/GetWebGroup?id='+id);
        };
        serviceResult.getListWebGroup = function () {
            return $http.get('/Api/menuRoleApi/GetListWebGroup');
        };
        serviceResult.getWebMenuHierarchy = function (webGroupId) {
            return $http.get('/Api/menuRoleApi/GetWebMenuHierarchy?webGroupId='+webGroupId);
        };
        serviceResult.SaveWebGroup = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/menuRoleApi/PutWebGroup',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
         return serviceResult;
    }]);
});